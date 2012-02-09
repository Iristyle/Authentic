using System;
using EPS.Annotations;
using Xunit;

namespace EPS.Web.Tests.Unit
{
	public class HttpHeaderFieldsTest
	{
		[Fact]
		public void Accept_HasProperEnumValue()
		{
			Assert.Equal("Accept", HttpHeaderFields.Accept.ToEnumValueString());
		}

		[Fact]
		public void AcceptCharacterSet_HasProperEnumValue()
		{
			Assert.Equal("Accept-Charset", HttpHeaderFields.AcceptCharset.ToEnumValueString());
		}

		[Fact]
		public void AcceptEncoding_HasProperEnumValue()
		{
			Assert.Equal("Accept-Encoding", HttpHeaderFields.AcceptEncoding.ToEnumValueString());
		}

		[Fact]
		public void AcceptLanguage_HasProperEnumValue()
		{
			Assert.Equal("Accept-Language", HttpHeaderFields.AcceptLanguage.ToEnumValueString());
		}

		[Fact]
		public void AcceptRanges_HasProperEnumValue()
		{
			Assert.Equal("Accept-Ranges", HttpHeaderFields.AcceptRanges.ToEnumValueString());
		}

		[Fact]
		public void Age_HasProperEnumValue()
		{
			Assert.Equal("Age", HttpHeaderFields.Age.ToEnumValueString());
		}

		[Fact]
		public void Allow_HasProperEnumValue()
		{
			Assert.Equal("Allow", HttpHeaderFields.Allow.ToEnumValueString());
		}

		[Fact]
		public void Authorization_HasProperEnumValue()
		{
			Assert.Equal("Authorization", HttpHeaderFields.Authorization.ToEnumValueString());
		}

		[Fact]
		public void CacheControl_HasProperEnumValue()
		{
			Assert.Equal("Cache-Control", HttpHeaderFields.CacheControl.ToEnumValueString());
		}

		[Fact]
		public void Connection_HasProperEnumValue()
		{
			Assert.Equal("Connection", HttpHeaderFields.Connection.ToEnumValueString());
		}

		[Fact]
		public void ContentEncoding_HasProperEnumValue()
		{
			Assert.Equal("Content-Encoding", HttpHeaderFields.ContentEncoding.ToEnumValueString());
		}

		[Fact]
		public void ContentLanguage_HasProperEnumValue()
		{
			Assert.Equal("Content-Language", HttpHeaderFields.ContentLanguage.ToEnumValueString());
		}

		[Fact]
		public void ContentLength_HasProperEnumValue()
		{
			Assert.Equal("Content-Length", HttpHeaderFields.ContentLength.ToEnumValueString());
		}

		[Fact]
		public void ContentLocation_HasProperEnumValue()
		{
			Assert.Equal("Content-Location", HttpHeaderFields.ContentLocation.ToEnumValueString());
		}

		[Fact]
		public void ContentMD5_HasProperEnumValue()
		{
			Assert.Equal("Content-MD5", HttpHeaderFields.ContentMD5.ToEnumValueString());
		}

		[Fact]
		public void ContentRange_HasProperEnumValue()
		{
			Assert.Equal("Content-Range", HttpHeaderFields.ContentRange.ToEnumValueString());
		}

		[Fact]
		public void ContentType_HasProperEnumValue()
		{
			Assert.Equal("Content-Type", HttpHeaderFields.ContentType.ToEnumValueString());
		}

		[Fact]
		public void Date_HasProperEnumValue()
		{
			Assert.Equal("Date", HttpHeaderFields.Date.ToEnumValueString());
		}
		
		[Fact]
		public void EntityTag_HasProperEnumValue()
		{
			Assert.Equal("ETag", HttpHeaderFields.EntityTag.ToEnumValueString());
		}

		[Fact]
		public void Expect_HasProperEnumValue()
		{
			Assert.Equal("Expect", HttpHeaderFields.Expect.ToEnumValueString());
		}

		[Fact]
		public void Expires_HasProperEnumValue()
		{
			Assert.Equal("Expires", HttpHeaderFields.Expires.ToEnumValueString());
		}


		[Fact]
		public void From_HasProperEnumValue()
		{
			Assert.Equal("From", HttpHeaderFields.From.ToEnumValueString());
		}

		[Fact]
		public void Host_HasProperEnumValue()
		{
			Assert.Equal("Host", HttpHeaderFields.Host.ToEnumValueString());
		}

		[Fact]
		public void IfMatch_HasProperEnumValue()
		{
			Assert.Equal("If-Match", HttpHeaderFields.IfMatch.ToEnumValueString());
		}
		
		[Fact]
		public void IfModifiedSince_HasProperEnumValue()
		{
			Assert.Equal("If-Modified-Since", HttpHeaderFields.IfModifiedSince.ToEnumValueString());
		}

		[Fact]
		public void IfNoneMatch_HasProperEnumValue()
		{
			Assert.Equal("If-None-Match", HttpHeaderFields.IfNoneMatch.ToEnumValueString());
		}

		[Fact]
		public void IfRange_HasProperEnumValue()
		{
			Assert.Equal("If-Range", HttpHeaderFields.IfRange.ToEnumValueString());
		}

		[Fact]
		public void IfUnmodifiedSince_HasProperEnumValue()
		{
			Assert.Equal("If-Unmodified-Since", HttpHeaderFields.IfUnmodifiedSince.ToEnumValueString());
		}

		[Fact]
		public void KeepAlive_HasProperEnumValue()
		{
			Assert.Equal("Keep-Alive", HttpHeaderFields.KeepAlive.ToEnumValueString());
		}

		[Fact]
		public void LastModified_HasProperEnumValue()
		{
			Assert.Equal("Last-Modified", HttpHeaderFields.LastModified.ToEnumValueString());
		}

		[Fact]
		public void Location_HasProperEnumValue()
		{
			Assert.Equal("Location", HttpHeaderFields.Location.ToEnumValueString());
		}

		[Fact]
		public void MaxForwards_HasProperEnumValue()
		{
			Assert.Equal("Max-Forwards", HttpHeaderFields.MaxForwards.ToEnumValueString());
		}

		[Fact]
		public void Pragma_HasProperEnumValue()
		{
			Assert.Equal("Pragma", HttpHeaderFields.Pragma.ToEnumValueString());
		}

		[Fact]
		public void ProxyAuthenticate_HasProperEnumValue()
		{
			Assert.Equal("Proxy-Authenticate", HttpHeaderFields.ProxyAuthenticate.ToEnumValueString());
		}

		[Fact]
		public void ProxyAuthorization_HasProperEnumValue()
		{
			Assert.Equal("Proxy-Authorization", HttpHeaderFields.ProxyAuthorization.ToEnumValueString());
		}

		[Fact]
		public void Range_HasProperEnumValue()
		{
			Assert.Equal("Range", HttpHeaderFields.Range.ToEnumValueString());
		}

		[Fact]
		public void Referrer_HasProperEnumValue()
		{
			Assert.Equal("Referer", HttpHeaderFields.Referer.ToEnumValueString());
		}

		[Fact]
		public void RetryAfter_HasProperEnumValue()
		{
			Assert.Equal("Retry-After", HttpHeaderFields.RetryAfter.ToEnumValueString());
		}

		[Fact]
		public void Server_HasProperEnumValue()
		{
			Assert.Equal("Server", HttpHeaderFields.Server.ToEnumValueString());
		}

		[Fact]
		public void TE_HasProperEnumValue()
		{
			Assert.Equal("TE", HttpHeaderFields.TE.ToEnumValueString());
		}

		[Fact]
		public void Trailer_HasProperEnumValue()
		{
			Assert.Equal("Trailer", HttpHeaderFields.Trailer.ToEnumValueString());
		}

		[Fact]
		public void TransferEncoding_HasProperEnumValue()
		{
			Assert.Equal("Transfer-Encoding", HttpHeaderFields.TransferEncoding.ToEnumValueString());
		}

		[Fact]
		public void UnlessModifiedSince_HasProperEnumValue()
		{
			Assert.Equal("Unless-Modified-Since", HttpHeaderFields.UnlessModifiedSince.ToEnumValueString());
		}

		[Fact]
		public void Upgrade_HasProperEnumValue()
		{
			Assert.Equal("Upgrade", HttpHeaderFields.Upgrade.ToEnumValueString());
		}


		[Fact]
		public void UserAgent_HasProperEnumValue()
		{
			Assert.Equal("User-Agent", HttpHeaderFields.UserAgent.ToEnumValueString());
		}
		
		[Fact]
		public void Vary_HasProperEnumValue()
		{
			Assert.Equal("Vary", HttpHeaderFields.Vary.ToEnumValueString());
		}
		
		[Fact]
		public void Via_HasProperEnumValue()
		{
			Assert.Equal("Via", HttpHeaderFields.Via.ToEnumValueString());
		}

		[Fact]
		public void Warning_HasProperEnumValue()
		{
			Assert.Equal("Warning", HttpHeaderFields.Warning.ToEnumValueString());
		}

		[Fact]
		public void WwwAuthenticate_HasProperEnumValue()
		{
			Assert.Equal("WWW-Authenticate", HttpHeaderFields.WwwAuthenticate.ToEnumValueString());
		}
	}    
}