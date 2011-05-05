using System;
using System.Web.Security;

namespace EPS.Web.Authentication
{
	/// <summary>	Interface that describes two functions that can be used to convert a given type to a MembershipUser. </summary>
	/// <remarks>	ebrown, 5/4/2011. </remarks>
	public interface IMembershipUserConverter<T>
	{
		/// <summary>	Converts a user to a membership user. </summary>
		/// <param name="user">	The user. </param>
		/// <returns>	This object as a MembershipUser. </returns>
		MembershipUser ToMembershipUser(T user);
		
		/// <summary>	Initializes this object from the given from membership user. </summary>
		/// <param name="user">	The user. </param>
		/// <returns>	The requested type. </returns>
		T FromMembershipUser(MembershipUser user);
	}
}