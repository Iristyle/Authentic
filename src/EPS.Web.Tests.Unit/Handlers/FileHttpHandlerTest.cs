using System;
using System.Net;
using System.Web;
using Autofac;
using EPS.Annotations;
using EPS.Web.Configuration;
using FakeItEasy;
using Xunit;

namespace EPS.Web.Handlers.Tests.Unit
{
	public class FileHttpHandlerTest : IDisposable
	{
		private bool disposed;
		private IContainer container;

		public FileHttpHandlerTest()
		{
			var builder = new ContainerBuilder();
			builder.RegisterType<FileHttpHandler>();
			builder.Register(context => A.Fake<IFileHttpHandlerConfiguration>());
			builder.Register(context => A.Fake<IFileHttpHandlerStatusLog>());
			builder.Register(context => A.Fake<IFileHttpHandlerStreamLoader>());

			container = builder.Build();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					container.Dispose();
				}

				disposed = true;
			}
		}


		[Fact]
		public void FileHttpHandler_ThrowsOnNullConfiguration()
		{
			Assert.Throws<ArgumentNullException>(() =>
				{
					using (var fileHandler = new FileHttpHandler(null, container.Resolve<IFileHttpHandlerStreamLoader>(), container.Resolve<IFileHttpHandlerStatusLog>())) { }
				});
		}

		[Fact]
		public void FileHttpHandler_ThrowsOnNullStreamLoader()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				using (var handler = new FileHttpHandler(container.Resolve<IFileHttpHandlerConfiguration>(), null, container.Resolve<IFileHttpHandlerStatusLog>())) { }
			});
		}

		[Fact]
		public void FileHttpHandler_ThrowsOnNullLogger()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				using (var handler = new FileHttpHandler(container.Resolve<IFileHttpHandlerConfiguration>(), container.Resolve<IFileHttpHandlerStreamLoader>(), null)) { }
			});
		}

		[Fact]
		public void ProcessRequest_ThrowsOnNullContext()
		{
			Assert.Throws<ArgumentNullException>(() => container.Resolve<FileHttpHandler>().ProcessRequest(null as HttpContext));
		}

		[Fact]
		public void ProcessRequest_ThrowsOnNullContextBase()
		{

			Assert.Throws<ArgumentNullException>(() => container.Resolve<FileHttpHandler>().ProcessRequest(null as HttpContextBase));
		}


		private HttpResponseBase ProcessRequestOfHttpMethod(HttpMethodNames methodName, string redirectUrl = "http://blah.blah.com")
		{
			var handler = container.Resolve<FileHttpHandler>();
			A.CallTo(() => handler.Configuration.UnauthorizedErrorRedirectUrl).Returns(redirectUrl);
			var context = A.Fake<HttpContextBase>();
			var request = A.Fake<HttpRequestBase>();
			var response = A.Fake<HttpResponseBase>();

			A.CallTo(() => context.Request).Returns(request);
			A.CallTo(() => context.Response).Returns(response);
			A.CallTo(() => request.HttpMethod).Returns(methodName.ToEnumValueString());

			handler.ProcessRequest(context);
			return response;
		}

		[Fact]
		public void ProcessRequest_RejectsHttpDeleteWithNotImplementedStatusCode()
		{
			HttpResponseBase response = ProcessRequestOfHttpMethod(HttpMethodNames.Delete);
			Assert.Equal((int)HttpStatusCode.NotImplemented, response.StatusCode);
		}

		[Fact]
		public void ProcessRequest_RejectsHttpDeleteAndRedirectsToConfiguredUrl()
		{
			var url = "http://delete.test.com";
			HttpResponseBase response = ProcessRequestOfHttpMethod(HttpMethodNames.Delete, url);
			A.CallTo(() => response.Redirect(url, false)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Fact]
		public void ProcessRequest_RejectsHttpOptionsWithNotImplementedStatusCode()
		{
			HttpResponseBase response = ProcessRequestOfHttpMethod(HttpMethodNames.Options);
			Assert.Equal((int)HttpStatusCode.NotImplemented, response.StatusCode);
		}

		[Fact]
		public void ProcessRequest_RejectsHttpOptionsAndRedirectsToConfiguredUrl()
		{
			var url = "http://options.test.com";
			HttpResponseBase response = ProcessRequestOfHttpMethod(HttpMethodNames.Options, url);
			A.CallTo(() => response.Redirect(url, false)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Fact]
		public void ProcessRequest_RejectsHttpPostWithNotImplementedStatusCode()
		{
			HttpResponseBase response = ProcessRequestOfHttpMethod(HttpMethodNames.Post);
			Assert.Equal((int)HttpStatusCode.NotImplemented, response.StatusCode);
		}

		[Fact]
		public void ProcessRequest_RejectsHttpPostAndRedirectsToConfiguredUrl()
		{
			var url = "http://post.test.com";
			HttpResponseBase response = ProcessRequestOfHttpMethod(HttpMethodNames.Post, url);
			A.CallTo(() => response.Redirect(url, false)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Fact]
		public void ProcessRequest_RejectsHttpPutWithNotImplementedStatusCode()
		{
			HttpResponseBase response = ProcessRequestOfHttpMethod(HttpMethodNames.Put);
			Assert.Equal((int)HttpStatusCode.NotImplemented, response.StatusCode);
		}

		[Fact]
		public void ProcessRequest_RejectsHttpPutAndRedirectsToConfiguredUrl()
		{
			var url = "http://put.test.com";
			HttpResponseBase response = ProcessRequestOfHttpMethod(HttpMethodNames.Put, url);
			A.CallTo(() => response.Redirect(url, false)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Fact]
		public void ProcessRequest_RejectsHttpTraceWithNotImplementedStatusCode()
		{
			HttpResponseBase response = ProcessRequestOfHttpMethod(HttpMethodNames.Trace);
			Assert.Equal((int)HttpStatusCode.NotImplemented, response.StatusCode);
		}

		[Fact]
		public void ProcessRequest_RejectsHttpTraceAndRedirectsToConfiguredUrl()
		{
			var url = "http://trace.test.com";
			HttpResponseBase response = ProcessRequestOfHttpMethod(HttpMethodNames.Trace, url);
			A.CallTo(() => response.Redirect(url, false)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Fact(Skip = "This set of tests is a super high priority")]
		public void ProcessRequest_CorrectlyHandlesVariousHeaderCombinations()
		{
		}

		[Fact(Skip = "This set of tests is a super high priority")]
		public void ProcessRequest_CorrectlyHandlesRangeRequests()
		{
		}

		[Fact(Skip = "This set of tests is a super high priority")]
		public void ProcessRequest_CorrectlyHandlesStreamLoadFailuresWithRedirects()
		{
		}

		[Fact(Skip = "This set of tests is a super high priority")]
		public void ProcessRequest_RedirectsToCloudUriWhenReturnedStreamIsCloudBased()
		{
		}
	}
}
