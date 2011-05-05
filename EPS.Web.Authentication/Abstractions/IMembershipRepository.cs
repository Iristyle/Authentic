using System;
using System.Web.Security;

namespace EPS.Web.Authentication.Abstractions
{
	//TODO: 5-4-2011 consider making this IMembershipRepository<T> with a property
	// of IMembershipUserConverter<T> 
	// and all the methods spec'd out so that they return a T, or ReadOnlyCollection<T> -- ie UpdateUser<T>, etc
	/// <summary>	Interface for membership repository. Can be used as the backing store for provider RepositoryBasedMembershipProvider.  This interface
	/// 			is almost completely derived from IMembershipProvider</summary>
	/// <remarks>	ebrown, 5/2/2011. </remarks>
	public interface IMembershipRepository
	{
		/// <summary>	Processes a request to update the password for a membership user. </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <param name="username">		The user to update the password for. </param>
		/// <param name="oldPassword">	The current password for the specified user. </param>
		/// <param name="newPassword">	The new password for the specified user. </param>
		/// <returns>	true if the password was updated successfully; otherwise, false. </returns>
		bool ChangePassword(string username, string oldPassword, string newPassword);
		
		/// <summary>	Processes a request to update the password question and answer for a membership user. </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <param name="username">				The user to change the password question and answer for. </param>
		/// <param name="password">				The password for the specified user. </param>
		/// <param name="newPasswordQuestion">	The new password question for the specified user. </param>
		/// <param name="newPasswordAnswer">	The new password answer for the specified user. </param>
		/// <returns>	true if the password question and answer are updated successfully; otherwise, false. </returns>
		bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer);
		
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
		MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status);
		
		/// <summary>	Removes a user from the membership data source. </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <param name="username">				The name of the user to delete. </param>
		/// <param name="deleteAllRelatedData">	true to delete data related to the user from the database; false to leave data related to the
		/// 									user in the database. </param>
		/// <returns>	true if the user was successfully deleted; otherwise, false. </returns>
		bool DeleteUser(string username, bool deleteAllRelatedData);
		
		/// <summary>	Indicates whether the membership provider is configured to allow users to reset their passwords. </summary>
		/// <value>	true if the membership provider supports password reset; otherwise, false. The default is true. </value>
		bool EnablePasswordReset { get; }
		
		/// <summary>	Indicates whether the membership provider is configured to allow users to retrieve their passwords. </summary>
		/// <value>	true if the membership provider is configured to support password retrieval; otherwise, false. The default is false. </value>
		bool EnablePasswordRetrieval { get; }
		
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
		MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords);
		
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
		MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords);
		
		/// <summary>	Gets a collection of all the users in the data source in pages of data. </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <param name="pageIndex">	The index of the page of results to return. <paramref name="pageIndex" /> is zero-based. </param>
		/// <param name="pageSize">		The size of the page of results to return. </param>
		/// <param name="totalRecords">	[out] The total number of matched users. </param>
		/// <returns>	
		/// A <see cref="T:System.Web.Security.MembershipUserCollection" /> collection that contains a page of <paramref name="pageSize" /><see
		/// cref="T:System.Web.Security.MembershipUser" /> objects beginning at the page specified by <paramref name="pageIndex" />. 
		/// </returns>
		MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords);

		/// <summary>	Gets the password for the specified user name from the data source. </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <param name="username">	The user to retrieve the password for. </param>
		/// <param name="answer">	The password answer for the user. </param>
		/// <returns>	The password for the specified user name. </returns>
		string GetPassword(string username, string answer);
		
		/// <summary>	Gets a user. </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <param name="username">		The username. </param>
		/// <param name="userIsOnline">	true if user is online. </param>
		/// <returns>	The user. </returns>
		MembershipUser GetUser(string username, bool userIsOnline);
		
		/// <summary>	Gets a user. </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <param name="providerUserKey">	The unique identifier from the membership data source for the user. </param>
		/// <param name="userIsOnline">		true if user is online. </param>
		/// <returns>	The user. </returns>
		MembershipUser GetUser(object providerUserKey, bool userIsOnline);

		/// <summary>	Gets a user name by email. </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <param name="email">	The email. </param>
		/// <returns>	The user name by email. </returns>
		string GetUserNameByEmail(string email);

		/// <summary>	Resets the password. </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <param name="username">	The username. </param>
		/// <param name="answer">	The answer. </param>
		/// <returns>	. </returns>
		string ResetPassword(string username, string answer);
		
		/// <summary>	Unlocks the user. </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <param name="userName">	Name of the user. </param>
		/// <returns>	true if it succeeds, false if it fails. </returns>
		bool UnlockUser(string userName);
		
		/// <summary>	Updates the user described by user. </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <param name="user">	The user. </param>
		void UpdateUser(MembershipUser user);
		
		/// <summary>	Validate user. </summary>
		/// <remarks>	ebrown, 5/2/2011. </remarks>
		/// <param name="username">	The username. </param>
		/// <param name="password">	The password. </param>
		/// <returns>	true if it succeeds, false if it fails. </returns>
		bool ValidateUser(string username, string password);
	}
}