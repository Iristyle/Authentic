using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace EPS.Web.Authentication.Abstractions
{
	public class FailureHandlerAction
	{
		public IPrincipal User { get; set; }
		public bool ShouldTerminateRequest { get; set; }
	}
}