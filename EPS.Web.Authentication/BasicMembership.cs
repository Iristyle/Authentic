using System;

namespace EPS.Web.Authentication
{
	/// <summary>	Represents the basic set of information captured in a MembershipUser, but without behavior. </summary>
	/// <remarks>	ebrown, 5/9/2011. </remarks>
	public class BasicMembership : IBasicMembership
	{
		/// <summary>	Gets or sets application-specific information for the membership user. </summary>
		/// <value>	Application-specific information for the membership user. </value>
		public string Comment { get; set; }
		/// <summary>	Gets the date and time when the user was added to the membership data store. </summary>
		/// <value>	The date and time when the user was added to the membership data store. </value>
		public DateTime CreationDate { get; private set; }
		/// <summary>	Gets or sets the e-mail address for the membership user. </summary>
		/// <value>	The e-mail address for the membership user. </value>
		public string Email { get; set; }
		/// <summary>	Gets or sets whether the membership user can be authenticated. </summary>
		/// <value>	true if the user can be authenticated; otherwise, false. </value>
		public bool IsApproved { get; set; }
		/// <summary>	Gets a value indicating whether the membership user is locked out and unable to be validated.. </summary>
		/// <value>	true if the membership user is locked out and unable to be validated; otherwise, false. </value>
		public bool IsLockedOut { get; private set; }
		/// <summary>	Gets whether the user is currently online. </summary>
		/// <value>	true if the user is online; otherwise, false. </value>
		public bool IsOnline { get; private set; }
		/// <summary>	Gets or sets the date and time when the membership user was last authenticated or accessed the application. </summary>
		/// <value>	The date and time when the membership user was last authenticated or accessed the application. </value>
		public DateTime LastActivityDate { get; set; }
		/// <summary>	Gets the most recent date and time that the membership user was locked out. </summary>
		/// <value>	A System.DateTime object that represents the most recent date and time that the membership user was locked out. </value>
		public DateTime LastLockoutDate { get; private set; }
		/// <summary>	Gets or sets the date and time when the user was last authenticated. </summary>
		/// <value>	The date and time when the user was last authenticated. </value>
		public DateTime LastLoginDate { get; set; }
		/// <summary>	Gets the date and time when the membership user's password was last updated. </summary>
		/// <value>	The date and time when the membership user's password was last updated. </value>
		public DateTime LastPasswordChangedDate { get; private set; }
		/// <summary>	Gets the password question for the membership user. </summary>
		/// <value>	The password question for the membership user. </value>
		public string PasswordQuestion { get; private set; }
		/// <summary>	Gets the name of the membership provider that stores and retrieves user information for the membership user. </summary>
		/// <value>	The name of the membership provider that stores and retrieves user information for the membership user. </value>
		public string ProviderName { get; private set; }
		/// <summary>	Gets the user identifier from the membership data source for the user. </summary>
		/// <value>	The user identifier from the membership data source for the user. </value>
		public object ProviderUserKey { get; private set; }
		/// <summary>	Gets the logon name of the membership user. </summary>
		/// <value>	The logon name of the membership user. </value>
		public string UserName { get; private set; }
	}
}
