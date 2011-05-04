using System;
using EPS.Web.Authentication.Abstractions;

namespace EPS.Web.Authentication
{
	public class MembershipRepositoryFactory
	{
		public static Func<string, IMembershipRepository> Resolve
		{
			get
			{
				return resolve.Value;
			}
			set
			{
				if (resolve.IsValueCreated)
					throw new InvalidOperationException("Resolution function may only be assigned once");
				resolve = new Lazy<Func<string, IMembershipRepository>>(() => value);
			}
		}
		private static Lazy<Func<string, IMembershipRepository>> resolve = new Lazy<Func<string,IMembershipRepository>>();
	}
}