using System;
using System.Configuration;
using System.Linq;
using System.Web.Security;
using EPS.Web.Authentication.Abstractions;

namespace EPS.Web.Authentication
{
	/// <summary>	Repository based membership provider that allows us to use any repository and support a bastardized form of property injection.  ASP.NET controls 
	/// 			the lifetime of the MembershipProviders defined in config, so we're at the mercy of their instantiation / initialization process. </summary>
	/// <remarks>	ebrown, 5/3/2011. </remarks>
	public class RepositoryBasedMembershipProvider : MembershipProvider
	{
		/// <summary> Name of the repository configuration value </summary>
		protected string RepositoryConfigValueName = "repositoryName";

        //filled in by service locator / property assignment in Initialize
		private IMembershipRepository repository = null;
		private string applicationName = "RepositoryBasedMembershipProvider";

		/// <summary>	The name of the application using the custom membership provider. </summary>
		/// <value>	The name of the application using the custom membership provider. </value>
		public override string ApplicationName
		{
			get { return applicationName; }
			set { applicationName = value; }
		}

		/// <summary>	
		/// Initializes this object, using MembershipRepositoryFactory to resolve a IMembershipRepository that is named in the 'repositoryName'
		/// configuration attribute for RepositoryBasedMembershipProvider. 
		/// </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <param name="name">		The name. </param>
		/// <param name="config">	The configuration. </param>
		public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
		{
			if (null == MembershipRepositoryFactory.Resolve)
			{
				throw new NotSupportedException("MembershipRepositoryFactory.Resolve must not be null.  Please properly initialize the service locator to find instances");
			}
			
			if (!config.AllKeys.Contains(RepositoryConfigValueName))
			{
				throw new ConfigurationErrorsException(String.Format("Configuration does not contain a key/value pair with a key of {0}", RepositoryConfigValueName));
			}

			repository = MembershipRepositoryFactory.Resolve(config[RepositoryConfigValueName]);

			if (null == repository)
			{
				throw new ConfigurationErrorsException(String.Format("Value of configuration attribute [{0}] had value of [{1}] which could not be found in the MembershipRepositoryFactory registry", RepositoryConfigValueName, config[RepositoryConfigValueName]));
			}

			base.Initialize(name, config);
		}

		/// <summary>	Processes a request to update the password for a membership user. </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <param name="username">		The user to update the password for. </param>
		/// <param name="oldPassword">	The current password for the specified user. </param>
		/// <param name="newPassword">	The new password for the specified user. </param>
		/// <returns>	true if the password was updated successfully; otherwise, false. </returns>
		public override bool ChangePassword(string username, string oldPassword, string newPassword)
		{
			return repository.ChangePassword(username, oldPassword, newPassword);
		}

		/// <summary>	Processes a request to update the password question and answer for a membership user. </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <param name="username">				The user to change the password question and answer for. </param>
		/// <param name="password">				The password for the specified user. </param>
		/// <param name="newPasswordQuestion">	The new password question for the specified user. </param>
		/// <param name="newPasswordAnswer">	The new password answer for the specified user. </param>
		/// <returns>	true if the password question and answer are updated successfully; otherwise, false. </returns>
		public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
		{
			return repository.ChangePasswordQuestionAndAnswer(username, password, newPasswordQuestion, newPasswordAnswer);
		}

		/// <summary>	Adds a new membership user to the data source. </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <param name="username">			The user name for the new user. </param>
		/// <param name="password">			The password for the new user. </param>
		/// <param name="email">			The e-mail address for the new user. </param>
		/// <param name="passwordQuestion">	The password question for the new user. </param>
		/// <param name="passwordAnswer">	The password answer for the new user. </param>
		/// <param name="isApproved">		Whether or not the new user is approved to be validated. </param>
		/// <param name="providerUserKey">	The unique identifier from the membership data source for the user. </param>
		/// <param name="status">			[out] A <see cref="T:System.Web.Security.MembershipCreateStatus" /> enumeration value indicating
		/// 								whether the user was created successfully. </param>
		/// <returns>	A <see cref="T:System.Web.Security.MembershipUser" /> object populated with the information for the newly created user. </returns>
		public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
		{
			return repository.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, out status);
		}

		/// <summary>	Removes a user from the membership data source. </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <param name="username">				The name of the user to delete. </param>
		/// <param name="deleteAllRelatedData">	true to delete data related to the user from the database; false to leave data related to the
		/// 									user in the database. </param>
		/// <returns>	true if the user was successfully deleted; otherwise, false. </returns>
		public override bool DeleteUser(string username, bool deleteAllRelatedData)
		{
			return repository.DeleteUser(username, deleteAllRelatedData);
		}

		/// <summary>	Indicates whether the membership provider is configured to allow users to reset their passwords. </summary>
		/// <value>	true if the membership provider supports password reset; otherwise, false. The default is true. </value>
		public override bool EnablePasswordReset
		{
			get { return repository.EnablePasswordReset; }
		}

		/// <summary>	Indicates whether the membership provider is configured to allow users to retrieve their passwords. </summary>
		/// <value>	true if the membership provider is configured to support password retrieval; otherwise, false. The default is false. </value>
		public override bool EnablePasswordRetrieval
		{
			get { return repository.EnablePasswordRetrieval; }
		}

		/// <summary>	Gets a collection of membership users where the e-mail address contains the specified e-mail address to match. </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <param name="emailToMatch">	The e-mail address to search for. </param>
		/// <param name="pageIndex">	The index of the page of results to return. <paramref name="pageIndex" /> is zero-based. </param>
		/// <param name="pageSize">		The size of the page of results to return. </param>
		/// <param name="totalRecords">	[out] The total number of matched users. </param>
		/// <returns>	
		/// A <see cref="T:System.Web.Security.MembershipUserCollection" /> collection that contains a page of <paramref name="pageSize" /><see
		/// cref="T:System.Web.Security.MembershipUser" /> objects beginning at the page specified by <paramref name="pageIndex" />. 
		/// </returns>
		public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			return repository.FindUsersByEmail(emailToMatch, pageIndex, pageSize, out totalRecords);
		}

		/// <summary>	Gets a collection of membership users where the user name contains the specified user name to match. </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <param name="usernameToMatch">	The user name to search for. </param>
		/// <param name="pageIndex">		The index of the page of results to return. <paramref name="pageIndex" /> is zero-based. </param>
		/// <param name="pageSize">			The size of the page of results to return. </param>
		/// <param name="totalRecords">		[out] The total number of matched users. </param>
		/// <returns>	
		/// A <see cref="T:System.Web.Security.MembershipUserCollection" /> collection that contains a page of <paramref name="pageSize" /><see
		/// cref="T:System.Web.Security.MembershipUser" /> objects beginning at the page specified by <paramref name="pageIndex" />. 
		/// </returns>
		public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			return repository.FindUsersByName(usernameToMatch, pageIndex, pageSize, out totalRecords);
		}

		/// <summary>	Gets a collection of all the users in the data source in pages of data. </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <param name="pageIndex">	The index of the page of results to return. <paramref name="pageIndex" /> is zero-based. </param>
		/// <param name="pageSize">		The size of the page of results to return. </param>
		/// <param name="totalRecords">	[out] The total number of matched users. </param>
		/// <returns>	
		/// A <see cref="T:System.Web.Security.MembershipUserCollection" /> collection that contains a page of <paramref name="pageSize" /><see
		/// cref="T:System.Web.Security.MembershipUser" /> objects beginning at the page specified by <paramref name="pageIndex" />. 
		/// </returns>
		public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
		{
			return repository.GetAllUsers(pageIndex, pageSize, out totalRecords);
		}

		/// <summary>	Gets the number of users currently accessing the application. </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <exception cref="NotImplementedException">	Thrown when the requested operation is unimplemented. </exception>
		/// <returns>	The number of users currently accessing the application. </returns>
		public override int GetNumberOfUsersOnline()
		{
			//TODO: 5-2-2011 -- implement in some way
			throw new NotImplementedException();
		}

		/// <summary>	Gets the password for the specified user name from the data source. </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <param name="username">	The user to retrieve the password for. </param>
		/// <param name="answer">	The password answer for the user. </param>
		/// <returns>	The password for the specified user name. </returns>
		public override string GetPassword(string username, string answer)
		{
			return repository.GetPassword(username, answer);
		}

		/// <summary>	Gets a user. </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <param name="username">		The username. </param>
		/// <param name="userIsOnline">	true if user is online. </param>
		/// <returns>	The user. </returns>
		public override MembershipUser GetUser(string username, bool userIsOnline)
		{
			return repository.GetUser(username, userIsOnline);
		}

		/// <summary>	Gets a user. </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <param name="providerUserKey">	The unique identifier from the membership data source for the user. </param>
		/// <param name="userIsOnline">		true if user is online. </param>
		/// <returns>	The user. </returns>
		public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
		{
			return repository.GetUser(providerUserKey, userIsOnline);
		}

		/// <summary>	Gets a user name by email. </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <param name="email">	The email. </param>
		/// <returns>	The user name by email. </returns>
		public override string GetUserNameByEmail(string email)
		{
			return repository.GetUserNameByEmail(email);
		}

		/// <summary>	Gets the number of invalid password or password-answer attempts allowed before the membership user is locked out. </summary>
		/// <value>	The number of invalid password or password-answer attempts allowed before the membership user is locked out. </value>
		public override int MaxInvalidPasswordAttempts
		{
			//TODO: 5-2-2011 -- implement in some way
			get { throw new NotImplementedException(); }
		}

		/// <summary>	Gets the minimum number of special characters that must be present in a valid password. </summary>
		/// <value>	The minimum number of special characters that must be present in a valid password. </value>
		public override int MinRequiredNonAlphanumericCharacters
		{
			//TODO: 5-2-2011 -- implement in some way
			get { throw new NotImplementedException(); }
		}

		/// <summary>	Gets the minimum length required for a password. </summary>
		/// <value>	The minimum length required for a password. </value>
		public override int MinRequiredPasswordLength
		{
			//TODO: 5-2-2011 -- implement in some way
			get { throw new NotImplementedException(); }
		}

		/// <summary>	
		/// Gets the number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the
		/// membership user is locked out. 
		/// </summary>
		/// <value>	
		/// The number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership
		/// user is locked out. 
		/// </value>
		public override int PasswordAttemptWindow
		{
			//TODO: 5-2-2011 -- implement in some way
			get { throw new NotImplementedException(); }
		}

		/// <summary>	Gets a value indicating the format for storing passwords in the membership data store. </summary>
		/// <value>	One of the <see cref="T:System.Web.Security.MembershipPasswordFormat" /> values indicating the format for storing passwords in the
		/// data store. 
		/// </value>
		public override MembershipPasswordFormat PasswordFormat
		{
			//TODO: 5-2-2011 -- implement in some way
			get { throw new NotImplementedException(); }
		}

		/// <summary>	Gets the regular expression used to evaluate a password. </summary>
		/// <value>	A regular expression used to evaluate a password. </value>
		public override string PasswordStrengthRegularExpression
		{
			//TODO: 5-2-2011 -- implement in some way
			get { throw new NotImplementedException(); }
		}

		/// <summary>	
		/// Gets a value indicating whether the membership provider is configured to require the user to answer a password question for password
		/// reset and retrieval. 
		/// </summary>
		/// <value>	true if a password answer is required for password reset and retrieval; always true. </value>
		public override bool RequiresQuestionAndAnswer
		{
			get { return true; }
		}

		/// <summary>	
		/// Gets a value indicating whether the membership provider is configured to require a unique e-mail address for each user name. 
		/// </summary>
		/// <value>	true if the membership provider requires a unique e-mail address; always true. </value>
		public override bool RequiresUniqueEmail
		{
			get { return true; }
		}

		/// <summary>	Resets the password. </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <param name="username">	The username. </param>
		/// <param name="answer">	The answer. </param>
		/// <returns>	. </returns>
		public override string ResetPassword(string username, string answer)
		{
			return repository.ResetPassword(username, answer);
		}

		/// <summary>	Unlocks the user. </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <param name="userName">	Name of the user. </param>
		/// <returns>	true if it succeeds, false if it fails. </returns>
		public override bool UnlockUser(string userName)
		{
			return repository.UnlockUser(userName);
		}

		/// <summary>	Updates the user described by user. </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <param name="user">	The user. </param>
		public override void UpdateUser(MembershipUser user)
		{
			repository.UpdateUser(user);
		}

		/// <summary>	Validate user. </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <param name="username">	The username. </param>
		/// <param name="password">	The password. </param>
		/// <returns>	true if it succeeds, false if it fails. </returns>
		public override bool ValidateUser(string username, string password)
		{
			return repository.ValidateUser(username, password);
		}
	}
}