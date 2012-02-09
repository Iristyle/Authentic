using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using Common.Logging;
using EPS.Annotations;
using EPS.Web.Abstractions;
using EPS.Web.Configuration;

namespace EPS.Web.Handlers
{
	/// <summary>   File http handler. </summary>
	/// <remarks>   ebrown, 2/9/2011. </remarks>
	public class FileHttpHandler : HttpHandlerBase, IReadOnlySessionState, IDisposable
	{
		private Stopwatch stopwatch = new Stopwatch();
		private static readonly ILog log = LogManager.GetCurrentClassLogger();
		private readonly IFileHttpHandlerConfiguration _configuration;
		private readonly IFileHttpHandlerStreamLoader _streamLoader;
		private readonly IFileHttpHandlerStatusLog _statusLogger;

		//since we'll need these all over the place, just stash them
		private HttpContextBase _context;
		private HttpResponseBase _response;
		private HttpRequestBase _request;
		private bool _disposed;

		/// <summary>   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
		/// <remarks>   ebrown, 12/1/2010. </remarks>
		public void Dispose()
		{
			if (!this._disposed)
			{
				Dispose(true);
				GC.SuppressFinalize(this);
			}
		}

		/// <summary>   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
		/// <remarks>   ebrown, 12/23/2010. </remarks>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (null != _streamLoader)
				{
					_streamLoader.Dispose();
				}
				this._disposed = true;
			}
		}

		/// <summary>   Gets the instance of the configuration used by this class as set in the constructor. </summary>
		/// <value> The configuration. </value>
		public IFileHttpHandlerConfiguration Configuration
		{
			get { return _configuration; }
		}

		/// <summary>   Gets the instance of the stream loader used by this class as set in the constructor. </summary>
		/// <value> The stream loader. </value>
		public IFileHttpHandlerStreamLoader StreamLoader
		{
			get { return _streamLoader; }
		}

		/// <summary>   Gets the instance of the status logger used by this class as set in the constructor. </summary>
		/// <value> The status logger. </value>
		public IFileHttpHandlerStatusLog StatusLogger
		{
			get { return _statusLogger; }
		}

		/// <summary>   Initializes a new instance of the FileHttpHandler class. </summary>
		/// <remarks>   ebrown, 2/9/2011. </remarks>
		/// <exception cref="ArgumentNullException">    Thrown when the configuration, streamLoader or statusLogger are null. </exception>
		/// <param name="configuration">    The configuration used to control the behavior of the FileHttpHandler. </param>
		/// <param name="streamLoader">     The class responsible for loading a FileStream / Cloud URI given a HttpRequestBase. </param>
		/// <param name="statusLogger">     The class responsible for logging status information as requests are processed by the handler. </param>
		public FileHttpHandler(IFileHttpHandlerConfiguration configuration,
			IFileHttpHandlerStreamLoader streamLoader,
			IFileHttpHandlerStatusLog statusLogger)
		{
			if (null == configuration) { throw new ArgumentNullException("configuration"); }
			if (null == streamLoader) { throw new ArgumentNullException("streamLoader"); }
			if (null == statusLogger) { throw new ArgumentNullException("statusLogger"); }

			this._configuration = configuration;
			this._streamLoader = streamLoader;
			this._statusLogger = statusLogger;
		}

		/// <summary>	
		/// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"
		/// />interface. 
		/// </summary>
		/// <remarks>	ebrown, 2/10/2011. </remarks>
		/// <exception cref="ObjectDisposedException">	Thrown when a supplied object has been disposed. </exception>
		/// <exception cref="ArgumentNullException">	Thrown when one or more required arguments are null. </exception>
		/// <param name="context">	An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects
		/// 						(for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
		public override void ProcessRequest(HttpContextBase context)
		{
			if (_disposed) { throw new ObjectDisposedException("this"); }
			stopwatch.Start();
			//TODO: 5-1-09 -- Response.Redirect doesn't make any sense... maybe we should just give them an 'unauthorized' empty file thing
			if (null == context) { throw new ArgumentNullException("context"); }

			this._context = context;
			//assign to local fields for micro-perf since these are virtuals            
			this._request = context.Request;
			this._response = context.Response;

			try
			{
				var method = this._request.HttpMethod;
				//we only accept GET or HEAD - don't log these
				if ((HttpMethodNames.Get.ToEnumValueString() != method)
					&& (HttpMethodNames.Header.ToEnumValueString() != method))
				{
					this._response.StatusCode = (int)HttpStatusCode.NotImplemented;
					this._response.Redirect(_configuration.UnauthorizedErrorRedirectUrl, false);
					return;
				}

				_statusLogger.Log(context, StreamWriteStatus.ApplicationRestarted);

				//now pass the HttpRequestBase to our class responsible for examining specific state and loading the Stream
				using (StreamLoaderResult fileDetails = _streamLoader.ParseRequest(this._request))
				{
					if (fileDetails.Status != StreamLoadStatus.Success)
					{
						RedirectOnFailedStatus(fileDetails);
						return;
					}

					IEnumerable<RangeRequest> rangeRequests;
					if (!HeadersValidate(fileDetails, out rangeRequests)) { return; }

					HttpResponseType responseStreamType = method == HttpMethodNames.Header.ToEnumValueString() ?
						HttpResponseType.HeadOnly : HttpResponseType.Complete;
					//we've passed our credential checking and we're ready to send out our response
					StreamWriteStatus streamWriteStatus = new ResponseStreamWriter(this._response, this._configuration.FileTransferBufferSizeInKBytes)
						.StreamFile(responseStreamType, fileDetails, rangeRequests, this._request.RetrieveHeader(HttpHeaderFields.IfRange), true);
					HandleStreamWriteStatus(streamWriteStatus);
				}
			}
			catch (HttpException hex)
			{
				//remote host closed the connection
				if (HttpExceptionErrorCodes.ConnectionAborted == hex.ErrorCode)
				{
					//log.Warn(String.Format("Remote Host Closed Transfer - Identity [{0}] - FileId [{1}] - User Id [{2}] / Name [{3}] / Email [{4}]{5}{5}{6}", WindowsIdentity.GetCurrent().Name, FileId, context.OnyxUser().Identity.OCPUser.Individual.OnyxId, context.OnyxUser().Identity.OCPUser.Individual.UserName, context.OnyxUser().Identity.OCPUser.Individual.UserEmailAddress, Environment.NewLine, this._request.ToLogString()), ex);
				}
			}
			//eat it -- nothing we can do
			catch (ThreadAbortException)
			{ }
			//log.Error(String.Format("Unexpected Download Exception Occurred - Identity [{0}] - FileId [{1}] - User Id [{2}] / Name [{3}] / Email [{4}]{5}{5}{6}", WindowsIdentity.GetCurrent().Name, FileId, context.OnyxUser().Identity.OCPUser.Individual.OnyxId, context.OnyxUser().Identity.OCPUser.Individual.UserName, context.OnyxUser().Identity.OCPUser.Individual.UserEmailAddress, Environment.NewLine, this._request.ToLogString()), ex);
			//SafeInternalServerError(context);
		}

		private bool HeadersValidate(StreamLoaderResult fileDetails, out IEnumerable<RangeRequest> rangeRequests)
		{
			rangeRequests = null;

			//we have ranges specified in the header, but they're invalid
			if (this._request.HasRangeHeaders() &&
				!this._request.TryParseRangeRequests(fileDetails.Metadata.Size.Value, out rangeRequests))
			{
				RedirectWithStatusCode(this._response, HttpStatusCode.BadRequest, _configuration.UnauthorizedErrorRedirectUrl);
				return false;
			}
			//modified since the requested date -- passing this sessionType value back will keep everything behind the scenes w/ the browser
			else if (fileDetails.Metadata.LastWriteTimeUtc.HasValue &&
				!this._request.IfModifiedSinceHeaderIsBeforeGiven(fileDetails.Metadata.LastWriteTimeUtc.Value))
			{
				this._response.StatusCode = (int)HttpStatusCode.NotModified;
				return false;
			}
			else if (fileDetails.Metadata.LastWriteTimeUtc.HasValue &&
				!this._request.IfUnmodifiedHeaderIsAfterGivenDate(fileDetails.Metadata.LastWriteTimeUtc.Value))
			{
				this._response.StatusCode = (int)HttpStatusCode.PreconditionFailed;
				return false;
			}
			//ETag doesn't match request
			else if (!this._request.IfMatchHeaderIsValid(fileDetails.Metadata.ExpectedMD5))
			{
				this._response.StatusCode = (int)HttpStatusCode.PreconditionFailed;
				return false;
			}
			// There is an If-None-Match header with "*" specified, which we don't allow
			else if (this._request.IfNoneMatchHeaderIsWildcard())
			{
				this._response.StatusCode = (int)HttpStatusCode.PreconditionFailed;
				return false;
			}
			// The entity does match the none-match request, the response code was set inside the IsIfNoneMatchHeaderValid function
			else if (this._request.IfNoneMatchHeaderHasMatchingETagSpecified(fileDetails.Metadata.ExpectedMD5))
			{
				this._response.StatusCode = (int)HttpStatusCode.NotModified;
				this._response.AppendHeader(HttpHeaderFields.EntityTag.ToEnumValueString(), fileDetails.Metadata.ExpectedMD5);
				return false;
			}

			return true;
		}

		private void RedirectOnFailedStatus(StreamLoaderResult fileDetails)
		{
			switch (fileDetails.Status)
			{
				case StreamLoadStatus.FileStoredInCloud:
					_statusLogger.Log(this._context, StreamWriteStatus.RedirectedClientToCloudUri);
					RedirectWithStatusCode(this._response, HttpStatusCode.MovedPermanently, fileDetails.CloudLocation.ToUrl());
					return;
				case StreamLoadStatus.UnauthorizedAccess:
					_statusLogger.Log(this._context, StreamWriteStatus.UnauthorizedAccessError);
					RedirectWithStatusCode(this._response, HttpStatusCode.BadRequest, _configuration.UnauthorizedErrorRedirectUrl);
					return;
				//TODO: these 2 might be the same scenario
				case StreamLoadStatus.InvalidRequest:
					_statusLogger.Log(this._context, StreamWriteStatus.UserRequestError);
					RedirectWithStatusCode(this._response, HttpStatusCode.BadRequest, _configuration.UnauthorizedErrorRedirectUrl);
					return;
				case StreamLoadStatus.FileMetadataNotFound:
					_statusLogger.Log(this._context, StreamWriteStatus.UserRequestError);
					RedirectWithStatusCode(this._response, HttpStatusCode.BadRequest, _configuration.UnauthorizedErrorRedirectUrl);
					return;
				case StreamLoadStatus.FileSizeMismatched:
					_statusLogger.Log(this._context, StreamWriteStatus.MismatchedSizeError);
					RedirectWithStatusCode(this._response, HttpStatusCode.InternalServerError, _configuration.ServerErrorUrl);
					return;
				case StreamLoadStatus.MD5Failed:
					_statusLogger.Log(this._context, StreamWriteStatus.MD5Failed);
					//log.Error(String.Format("File Corrupt -- Failed MD5 check - Identity [{0}] - FileId [{1}] - User Id [{2}] / Name [{3}] / Email [{4}]{5}File Path [ {6} ]{5}{5}{7}", WindowsIdentity.GetCurrent().Name, FileId, context.OnyxUser().Identity.OCPUser.Individual.OnyxId, context.OnyxUser().Identity.OCPUser.Individual.UserName, context.OnyxUser().Identity.OCPUser.Individual.UserEmailAddress, Environment.NewLine, properties.MappedPath, this._request.ToLogString()));
					RedirectWithStatusCode(this._response, HttpStatusCode.InternalServerError, _configuration.ServerErrorUrl);
					return;
				case StreamLoadStatus.FileNotFound:
					_statusLogger.Log(this._context, StreamWriteStatus.NotFound);
					//log.Error(String.Format("File Not Found - Identity [{0}] - FileId [{1}] - User Id [{2}] / Name [{3}] / Email [{4}]{5}File Path [ {6} ]{5}{5}{7}", WindowsIdentity.GetCurrent().Name, FileId, context.OnyxUser().Identity.OCPUser.Individual.OnyxId, context.OnyxUser().Identity.OCPUser.Individual.UserName, context.OnyxUser().Identity.OCPUser.Individual.UserEmailAddress, Environment.NewLine, properties.MappedPath, this._request.ToLogString()));
					RedirectWithStatusCode(this._response, HttpStatusCode.InternalServerError, _configuration.ServerErrorUrl);
					return;
			}
		}

		private void HandleStreamWriteStatus(StreamWriteStatus fileSendStatus)
		{
			switch (fileSendStatus)
			{
				case StreamWriteStatus.UnexpectedError:
					_statusLogger.Log(this._context, StreamWriteStatus.UnexpectedError);
					//log.Error(String.Format("Unexpected File Transfer Error - Identity [{0}] - FileId [{1}] - User Id [{2}] / Name [{3}] / Email [{4}]{5}File Path [ {6} ]{5}{5}{7}", WindowsIdentity.GetCurrent().Name, FileId, context.OnyxUser().Identity.OCPUser.Individual.OnyxId, context.OnyxUser().Identity.OCPUser.Individual.UserName, context.OnyxUser().Identity.OCPUser.Individual.UserEmailAddress, Environment.NewLine, properties.MappedPath, this._request.ToLogString()));
					SafeInternalServerError(this._context, this._configuration.ServerErrorUrl);
					break;

				case StreamWriteStatus.StreamReadError:
					_statusLogger.Log(this._context, StreamWriteStatus.StreamReadError);
					//log.Error(String.Format("File Could Not Be Read - Identity [{0}] - FileId [{1}] - User Id [{2}] / Name [{3}] / Email [{4}]{5}File Path [ {6} ]{5}{5}{7}", WindowsIdentity.GetCurrent().Name, FileId, context.OnyxUser().Identity.OCPUser.Individual.OnyxId, context.OnyxUser().Identity.OCPUser.Individual.UserName, context.OnyxUser().Identity.OCPUser.Individual.UserEmailAddress, Environment.NewLine, properties.MappedPath, this._request.ToLogString()));
					SafeInternalServerError(this._context, this._configuration.ServerErrorUrl);
					break;

				//nothing to do here
				case StreamWriteStatus.SentHttpHead:
				case StreamWriteStatus.SentFile:
				case StreamWriteStatus.ClientDisconnected:
				default:
					break;
			}
		}

		[SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "A string based url is more convenient since Redirect takes a string")]
		private static void RedirectWithStatusCode(HttpResponseBase response, HttpStatusCode statusCode, string url)
		{
			response.StatusCode = (int)statusCode;
			response.Redirect(url);
		}

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Eat any failures incurred during transfer since this is last resort anyhow")]
		[SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "A string based url is more convenient since Transfer takes a string")]
		private static void SafeInternalServerError(HttpContextBase context, string url)
		{
			try
			{
				//TODO: 2-21-2011 -- I'm pretty confident at this point, that its too late to actually generate a server error / transfer
				//since the file has already started streaming, but verify that
				context.Response.ClearHeaders();
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				context.Server.Transfer(url);
			}
			catch (Exception) { }
		}
	}
}