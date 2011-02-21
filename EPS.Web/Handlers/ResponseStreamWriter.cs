using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using EPS.Text;
using log4net;

namespace EPS.Web.Handlers
{
    /// <summary>   
    /// This class is responsible for streaming a given FileStreamDetails data over a HttpResponseBase.  It can handle multipart and partial
    /// range requests. 
    /// </summary>
    /// <remarks>   ebrown, 2/15/2011. </remarks>
    public class ResponseStreamWriter
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private HttpResponseBase _response;
        private long _bufferSizeBytes;


        /// <summary>   Initializes a new instance of the ResponseStreamWriter class, with a HttpResponseBase to write to, and a buffer size. </summary>
        /// <remarks>   ebrown, 2/15/2011. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when the response is null. </exception>
        /// <param name="response">                         The response. </param>
        /// <param name="fileTransferBufferSizeInKBytes">   The size of the buffer in KB to use when streaming the data. </param>
        public ResponseStreamWriter(HttpResponseBase response, long fileTransferBufferSizeInKBytes)
        {
            if (null == response) { throw new ArgumentNullException("response"); }

            this._response = response;
            this._bufferSizeBytes = fileTransferBufferSizeInKBytes * 1024;
        }

        /// <summary>   Stream file. </summary>
        /// <remarks>   ebrown, 2/15/2011. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <exception cref="Exception">                Thrown when exception. </exception>
        /// <param name="requestedResponseType">    Type of the requested response. </param>
        /// <param name="downloadProperties">       The download properties. </param>
        /// <param name="rangeRequests">            The range requests. </param>
        /// <param name="ifRangeEntityTag">         if range entity tag. </param>
        /// <param name="forceDownload">            true to force download. </param>
        /// <returns>   . </returns>
        public StreamWriteStatus StreamFile(HttpResponseType requestedResponseType, StreamLoaderResult downloadProperties, IEnumerable<RangeRequest> rangeRequests, string ifRangeEntityTag, bool forceDownload)
        {
            if (null == downloadProperties) { throw new ArgumentNullException("downloadProperties"); }
            if (null == rangeRequests) { throw new ArgumentNullException("rangeRequests"); }

            try
            {
                string contentType = !string.IsNullOrEmpty(downloadProperties.ContentType) ? 
                    downloadProperties.ContentType : MimeTypes.GetMimeTypeForFileExtension(Path.GetExtension(downloadProperties.FileName));

                if (forceDownload)
                {
                    //TODO: not sure if we need these anymore or not
                    //contentType = "application/x-msdownload";
                    this._response.AppendHeader("content-disposition", String.Format(CultureInfo.InvariantCulture, "attachment; filename={0}", downloadProperties.FileName));
                }

                long responseContentLength = 0;
                bool isMultipart = RangeRequestHelpers.IsMultipartRequest(rangeRequests),
                    isRangeRequest = RangeRequestHelpers.IsPartialOrMultipleRangeRequests(rangeRequests, downloadProperties.Size.Value);

                //TODO: is this response.Clear() necessary here?
                this._response.Clear();

                string multipartFooter = String.Format(CultureInfo.InvariantCulture, "--{0}--{1}{1}", MultipartNames.MultipartBoundary, Environment.NewLine);

                if (isRangeRequest && 
                    (string.IsNullOrWhiteSpace(ifRangeEntityTag) 
                    || string.Equals(downloadProperties.ExpectedMD5, ifRangeEntityTag, StringComparison.OrdinalIgnoreCase)))
                {
                    //sum the content length of each request, taking special care when we support multipart
                    responseContentLength = rangeRequests.Sum(rangeRequest => rangeRequest.End - rangeRequest.Start + 1 +
                        //length of our header, plus our intermediate header footer (line break)
                        (isMultipart ? rangeRequest.GetMultipartIntermediateHeader(contentType).Length + Environment.NewLine.Length : 0));
                    //responseContentLength += 49 + HttpMethodNames.MULTIPART_BOUNDARY.Length + contentType.Length + rangeRequests[i].startRange.ToString().Length + rangeRequests[i].endRange.ToString().Length + downloadProperties.Attachment.Size.ToString().Length;

                    //tack on the last intermediate header length if applicable
                    if (isMultipart)
                    {
                        responseContentLength += multipartFooter.Length;
                    }
                    else
                    {
                        var first = rangeRequests.First();
                        //must indicate the Response Range of in the initial HTTP Header since this isn't multipart
                        this._response.AppendHeader(HttpHeaderFields.ContentRange.ToEnumValueString(),
                            String.Format(CultureInfo.InvariantCulture, "bytes {0}-{1}/{2}", first.Start, first.End, downloadProperties.Size.Value));
                    }

                    // Range response 
                    this._response.StatusCode = (int)HttpStatusCode.PartialContent;
                }
                // not a Range request, or the requested Range entity ID does not match the current entity ID, so start a new download
                else
                {
                    responseContentLength = downloadProperties.Size.Value;
                    this._response.StatusCode = (int)HttpStatusCode.OK;
                }

                this._response.AppendHeader(HttpHeaderFields.ContentLength.ToEnumValueString(), responseContentLength.ToString(CultureInfo.InvariantCulture));
                //don't go off the DB insert date for last modified b/c the file system file could be older (b/c multiple files records inserted at different dates could share the same physical file)
                if (downloadProperties.LastWriteTimeUtc.HasValue)
                    this._response.AppendHeader(HttpHeaderFields.LastModified.ToEnumValueString(), downloadProperties.LastWriteTimeUtc.Value.ToString("r", CultureInfo.InvariantCulture));
                this._response.AppendHeader(HttpHeaderFields.AcceptRanges.ToEnumValueString(), "bytes");
                this._response.AppendHeader(HttpHeaderFields.EntityTag.ToEnumValueString(), String.Format(CultureInfo.InvariantCulture, "\"{0}\"", downloadProperties.ExpectedMD5));
                //not sure if we should use the Keep-Alive header?
                //this._response.AppendHeader(HttpHeaderFields.HTTP_HEADER_KEEP_ALIVE, "timeout=15, max=30");

                //multipart messages are special -> file's actual mime type written into Response later
                this._response.ContentType = (isMultipart ? MultipartNames.MultipartContentType : contentType);

                //we've dumped our HEAD and can return now
                if (HttpResponseType.HeadOnly == requestedResponseType)
                {
                    return StreamWriteStatus.SentHttpHead;
                }

                // Flush the HEAD information to the client...
                this._response.Flush();

                StreamWriteStatus result = BufferStreamToResponse(downloadProperties, rangeRequests, contentType, multipartFooter);
                if (StreamWriteStatus.ClientDisconnected == result)
                {
                    return result;
                }

                //this causes a ThreadAbortException
                //Response.End();    
            }
            catch (IOException ex)
            {
                //, this._request.ToLogString()
                log.Error(String.Format(CultureInfo.InvariantCulture, "File read failure for user {0}", WindowsIdentity.GetCurrent().Name), ex);
                return StreamWriteStatus.StreamReadError;
            }
            catch (Exception ex)
            {
                HttpException httpEx = ex as HttpException;
                //suck up the remote connection failure
                if ((null != httpEx) && (HttpExceptionErrorCodes.ConnectionAborted == httpEx.ErrorCode))
                    return StreamWriteStatus.ClientDisconnected;

                //bubble up to the caller, and let them log it -- this maintains the identity of our original exception as the innerexception
                throw;
                //log.Error("Unexpected failure for " + WindowsIdentity.GetCurrent().Name, ex);

                //error = "Unexpected File Transfer Error - FileId [" + FileId.ToString() + "] - User Id [" + context.OnyxUser().Identity.OCPUser.Individual.OnyxId.ToString() + "] / Name [" + ((userProperties.Name == null) ? string.Empty : userProperties.Name) + "] / Email [" + ((userProperties.Email == null) ? string.Empty : userProperties.Email) + "]" + Environment.NewLine + "File Path [ " + properties.MappedPath + " ]";
                //return DownloadFileStatus.UnexpectedError;
            }

            return StreamWriteStatus.SentFile;
        }

        private StreamWriteStatus BufferStreamToResponse(StreamLoaderResult fileStreamDetails, IEnumerable<RangeRequest> rangeRequests, string contentType, string multipartFooter)
        {
            bool isMultipart = RangeRequestHelpers.IsMultipartRequest(rangeRequests);
            int bytesRead;
            long bytesToRead;
            long remainingBytes = fileStreamDetails.Size.Value;
            byte[] buffer = new byte[this._bufferSizeBytes];

            //stream each requested range to the client
            foreach (RangeRequest rangeRequest in rangeRequests)
            {
                fileStreamDetails.FileStream.Seek(rangeRequest.Start, SeekOrigin.Begin);
                bytesToRead = rangeRequest.End - rangeRequest.Start + 1;
                // If this is a multipart response, we must add certain headers before streaming the content:
                if (isMultipart)
                {
                    this._response.Output.Write(rangeRequest.GetMultipartIntermediateHeader(contentType));
                }

                while (bytesToRead > 0)
                {
                    if (!this._response.IsClientConnected)
                    {
                        return StreamWriteStatus.ClientDisconnected;
                    }

                    //buffer length can only be a max of int32 in length -- so we know that this value will always be less than an int32
                    int toRead = Convert.ToInt32(Math.Min(Convert.ToInt64(buffer.Length), bytesToRead));
                    bytesRead = fileStreamDetails.FileStream.Read(buffer, 0, toRead);
                    this._response.OutputStream.Write(buffer, 0, bytesRead);
                    this._response.Flush();
                    bytesToRead -= bytesRead;
                }

                // In Multipart responses, mark the end of the part
                if (isMultipart)
                {
                    this._response.Output.WriteLine();
                }
            }

            if (isMultipart)
            {
                this._response.Output.Write(multipartFooter);
            }
            return StreamWriteStatus.ApplicationRestarted;
        }
    }
}