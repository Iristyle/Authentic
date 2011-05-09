using System;

namespace EPS.Web.Authentication
{
	/// <summary>	Represents the basic set of information generally captured in a MembershipUser. </summary>
	/// <remarks>	ebrown, 5/9/2011. </remarks>
	public interface IBasicMembership
	{
		/// <summary>	Gets application-specific information for the membership user. </summary>
		/// <value>	Application-specific information for the membership user. </value>
		string Comment { get; }
		/// <summary>	Gets the date and time when the user was added to the membership data store. </summary>
		/// <value>	The date and time when the user was added to the membership data store. </value>
		DateTime CreationDate { get; }
		/// <summary>	Gets the e-mail address for the membership user. </summary>
		/// <value>	The e-mail address for the membership user. </value>
		string Email { get; }
		/// <summary>	Gets whether the membership user can be authenticated. </summary>
		/// <value>	true if the user can be authenticated; otherwise, false. </value>
		bool IsApproved { get; }
		/// <summary>	Gets a value indicating whether the membership user is locked out and unable to be validated.. </summary>
		/// <value>	true if the membership user is locked out and unable to be validated; otherwise, false. </value>
		bool IsLockedOut { get; }
		/// <summary>	Gets whether the user is currently online. </summary>
		/// <value>	true if the user is online; otherwise, false. </value>
		bool IsOnline { get; }
		/// <summary>	Gets the date and time when the membership user was last authenticated or accessed the application. </summary>
		/// <value>	The date and time when the membership user was last authenticated or accessed the application. </value>
		DateTime LastActivityDate { get; }
		/// <summary>	Gets the most recent date and time that the membership user was locked out. </summary>
		/// <value>	A System.DateTime object that represents the most recent date and time that the membership user was locked out. </value>
		DateTime LastLockoutDate { get; }
		/// <summary>	Gets the date and time when the user was last authenticated. </summary>
		/// <value>	The date and time when the user was last authenticated. </value>
		DateTime LastLoginDate { get; }
		/// <summary>	Gets the date and time when the membership user's password was last updated. </summary>
		/// <value>	The date and time when the membership user's password was last updated. </value>
		DateTime LastPasswordChangedDate { get; }
		/// <summary>	Gets the password question for the membership user. </summary>
		/// <value>	The password question for the membership user. </value>
		string PasswordQuestion { get; }
		/// <summary>	Gets the name of the membership provider that stores and retrieves user information for the membership user. </summary>
		/// <value>	The name of the membership provider that stores and retrieves user information for the membership user. </value>
		string ProviderName { get; }
		/// <summary>	Gets the user identifier from the membership data source for the user. </summary>
		/// <value>	The user identifier from the membership data source for the user. </value>
		object ProviderUserKey { get; }
		/// <summary>	Gets the logon name of the membership user. </summary>
		/// <value>	The logon name of the membership user. </value>
		string UserName { get; }
	}
}
