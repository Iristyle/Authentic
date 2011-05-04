using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Security;
using EPS.Web.Authentication.Abstractions;
using FakeItEasy;
using Xunit;

namespace EPS.Web.Authentication.Tests.Unit
{
	public class RepositoryBasedMembershipProviderTest
	{
		private Dictionary<string, IMembershipRepository> repositories = new Dictionary<string, IMembershipRepository>();
		private static Func<string, IMembershipRepository> internalResolve = (name) => null;

		static RepositoryBasedMembershipProviderTest()
		{
			MembershipRepositoryFactory.Resolve = (name) => internalResolve(name);
		}

		private RepositoryBasedMembershipProvider CreateProvider(string repositoryName)
		{
			internalResolve = (name) => repositories.ContainsKey(name) ? repositories[name] : null;
			var provider = new RepositoryBasedMembershipProvider();
			var repository = A.Fake<IMembershipRepository>();
			repositories.Add(repositoryName, repository);

			provider.Initialize(provider.ApplicationName, new NameValueCollection() { { "repositoryName", repositoryName } });

			return provider;
		}

		private void CleanUp()
		{
			internalResolve = null;
		}

		[Fact]
		public void Initialize_ThrowsOnNullMembershipFactoryResolve()
		{
			CleanUp();
			var provider = new RepositoryBasedMembershipProvider();
			Assert.Throws<NotSupportedException>(() => provider.Initialize(provider.ApplicationName, new NameValueCollection()));
		}

		[Fact]
		public void Initialize_ThrowsOnMissingConfigurationKey()
		{
			internalResolve = (name) => null;
			var provider = new RepositoryBasedMembershipProvider();
			Assert.Throws<ConfigurationErrorsException>(() => provider.Initialize(provider.ApplicationName, new NameValueCollection()));
			CleanUp();
		}

		[Fact]
		public void Initialize_ThrowsOnUnresolvableRepositoryKey()
		{
			internalResolve = (name) => null;
			var provider = new RepositoryBasedMembershipProvider();
			Assert.Throws<ConfigurationErrorsException>(() => provider.Initialize(provider.ApplicationName, new NameValueCollection() { { "repositoryName", "foo" } }));
			CleanUp();
		}

		[Fact]
		public void ChangePassword_CallsRepository()
		{
			string name = "password";
			var provider = CreateProvider(name);
			provider.ChangePassword("username", "oldPassword", "newPassword");
			A.CallTo(() => repositories[name].ChangePassword(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored))
				.MustHaveHappened(Repeated.Exactly.Once);
			CleanUp();
		}

		[Fact]
		public void ChangePasswordQuestionAndAnswer_CallsRepository()
		{
			string name = "passwordQuestionAndAnswer";
			var provider = CreateProvider(name);
			provider.ChangePasswordQuestionAndAnswer("username", "password", "newPasswordQuestion", "newPasswordAnswer");
			A.CallTo(() => repositories[name].ChangePasswordQuestionAndAnswer(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, A<string>.Ignored))
				.MustHaveHappened(Repeated.Exactly.Once);
			CleanUp();
		}

		[Fact]
		public void CreateUser_CallsRepository()
		{
			string name = "createUser";
			var provider = CreateProvider(name);
			MembershipCreateStatus status;
			provider.CreateUser("username", "password", "email", "passwordQuestion", "passwordAnswer", false, null, out status);
			A.CallTo(() => repositories[name].CreateUser(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, A<string>.Ignored,
				A<string>.Ignored, A<bool>.Ignored, A<object>.Ignored, out status))
				.MustHaveHappened(Repeated.Exactly.Once);
			CleanUp();
		}

		[Fact]
		public void DeleteUser_CallsRepository()
		{
			string name = "deleteUser";
			var provider = CreateProvider(name);
			provider.DeleteUser("username", false);
			A.CallTo(() => repositories[name].DeleteUser(A<string>.Ignored, A<bool>.Ignored))
				.MustHaveHappened(Repeated.Exactly.Once);
			CleanUp();
		}

		[Fact]
		public void EnablePasswordReset_CallsRepository()
		{
			string name = "enableReset";
			var provider = CreateProvider(name);
			bool reset = provider.EnablePasswordReset;
			A.CallTo(() => repositories[name].EnablePasswordReset)
				.MustHaveHappened(Repeated.Exactly.Once);
			CleanUp();
		}

		[Fact]
		public void EnablePasswordRetrieval_CallsRepository()
		{
			string name = "enableRetrieval";
			var provider = CreateProvider(name);
			bool retrieval = provider.EnablePasswordRetrieval;
			A.CallTo(() => repositories[name].EnablePasswordRetrieval)
				.MustHaveHappened(Repeated.Exactly.Once);
			CleanUp();
		}

		[Fact]
		public void FindUsersByEmail_CallsRepository()
		{
			string name = "findUsersByEmail";
			var provider = CreateProvider(name);
			int count;
			provider.FindUsersByEmail("emailToMatch", 0, 2, out count);
			A.CallTo(() => repositories[name].FindUsersByEmail(A<string>.Ignored, A<int>.Ignored, A<int>.Ignored, out count))
				.MustHaveHappened(Repeated.Exactly.Once);
			CleanUp();
		}

		[Fact]
		public void FindUsersByName_CallsRepository()
		{
			string name = "findUsersByName";
			var provider = CreateProvider(name);
			int count;
			provider.FindUsersByName("nametoMatch", 0, 2, out count);
			A.CallTo(() => repositories[name].FindUsersByName(A<string>.Ignored, A<int>.Ignored, A<int>.Ignored, out count))
				.MustHaveHappened(Repeated.Exactly.Once);
			CleanUp();
		}

		[Fact]
		public void GetAllUsers_CallsRepository()
		{
			string name = "getAllUsers";
			var provider = CreateProvider(name);
			int count;
			provider.GetAllUsers(0, 2, out count);
			A.CallTo(() => repositories[name].GetAllUsers(A<int>.Ignored, A<int>.Ignored, out count))
				.MustHaveHappened(Repeated.Exactly.Once);
			CleanUp();
		}

		[Fact]
		public void GetPassword_CallsRepository()
		{
			string name = "getPassword";
			var provider = CreateProvider(name);
			provider.GetPassword("username", "answer");
			A.CallTo(() => repositories[name].GetPassword(A<string>.Ignored, A<string>.Ignored))
				.MustHaveHappened(Repeated.Exactly.Once);
			CleanUp();
		}

		[Fact]
		public void GetUser_CallsRepository()
		{
			string name = "getUser";
			var provider = CreateProvider(name);
			provider.GetUser("username", false);
			A.CallTo(() => repositories[name].GetUser(A<string>.Ignored, A<bool>.Ignored))
				.MustHaveHappened(Repeated.Exactly.Once);
			CleanUp();
		}

		[Fact]
		public void GetUser2_CallsRepository()
		{
			string name = "getUser2";
			var provider = CreateProvider(name);
			provider.GetUser(null as object, false);
			A.CallTo(() => repositories[name].GetUser(A<object>.Ignored, A<bool>.Ignored))
				.MustHaveHappened(Repeated.Exactly.Once);
			CleanUp();
		}

		[Fact]
		public void GetUserNameByEmail_CallsRepository()
		{
			string name = "getUserNamebyEmail";
			var provider = CreateProvider(name);
			provider.GetUserNameByEmail("email");
			A.CallTo(() => repositories[name].GetUserNameByEmail(A<string>.Ignored))
				.MustHaveHappened(Repeated.Exactly.Once);
			CleanUp();
		}

		[Fact]
		public void ResetPassword_CallsRepository()
		{
			string name = "resetPassword";
			var provider = CreateProvider(name);
			provider.ResetPassword("username", "answer");
			A.CallTo(() => repositories[name].ResetPassword(A<string>.Ignored, A<string>.Ignored))
				.MustHaveHappened(Repeated.Exactly.Once);
			CleanUp();
		}

		[Fact]
		public void UnlockUser_CallsRepository()
		{
			string name = "unlockUser";
			var provider = CreateProvider(name);
			provider.UnlockUser("username");
			A.CallTo(() => repositories[name].UnlockUser(A<string>.Ignored))
				.MustHaveHappened(Repeated.Exactly.Once);
			CleanUp();
		}

		[Fact]
		public void UpdateUser_CallsRepository()
		{
			string name = "updateUser";
			var provider = CreateProvider(name);
			provider.UpdateUser(A.Dummy<MembershipUser>());
			A.CallTo(() => repositories[name].UpdateUser(A<MembershipUser>.Ignored))
				.MustHaveHappened(Repeated.Exactly.Once);
			CleanUp();
		}

		[Fact]
		public void ValidateUser_CallsRepository()
		{
			string name = "validateUser";
			var provider = CreateProvider(name);
			provider.ValidateUser("username", "password");
			A.CallTo(() => repositories[name].ValidateUser(A<string>.Ignored, A<string>.Ignored))
				.MustHaveHappened(Repeated.Exactly.Once);
			CleanUp();
		}

		//none of this stuff delegates
		[Fact(Skip = "Implement at some point")]
		public void GetNumberOfUsersOnline_DoesStuff()
		{
		}

		[Fact(Skip = "Implement at some point")]
		public void MaxInvalidPasswordAttempts_DoesStuff()
		{
		}

		[Fact(Skip = "Implement at some point")]
		public void MinRequiredNonAlphanumericCharacters_DoesStuff()
		{

		}

		[Fact(Skip = "Implement at some point")]
		public void MinRequiredPasswordLength_DoesStuff()
		{

		}

		[Fact(Skip = "Implement at some point")]
		public void PasswordAttemptWindow_DoesStuff()
		{
		}

		[Fact(Skip = "Implement at some point")]
		public void PasswordFormat_DoesStuff()
		{
		}

		[Fact(Skip = "Implement at some point")]
		public void PasswordStrengthRegularExpression_DoesStuff()
		{
		}

		[Fact(Skip = "Implement at some point")]
		public void RequiresQuestionAndAnswer_DoesStuff()
		{
		}

		[Fact(Skip = "Implement at some point")]
		public void RequiresUniqueEmail_DoesStuff()
		{
		}
	}
}