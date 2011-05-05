using System;

namespace EPS.Web.Authentication
{
	[Flags]
	public enum MembershipRepositoryOperations
	{
		None = 0,
		ChangePassword = 1,
		ChangePasswordQuestionAndAnswer = 1 << 1,
		CreateUser = (2 << 1),
		DeleteUser = (3 << 1),
		EnablePasswordReset = (4 << 1),
		EnablePasswordRetrieval = (5 << 1),
		FindUsersByEmail = (6 << 1),
		FindUsersByName = (7 << 1),
		GetAllUsers = (8 << 1),
		GetPassword = (9 << 1),
		GetUser = (10 << 1),
		GetUserNameByEmail = (11 << 1),
		ResetPassword = (12 << 1),
		UnlockUser = (13 << 1),
		UpdateUser = (14 << 1),
		ValidateUser = (15 << 1),
		ReadOnly = EnablePasswordRetrieval 
			+ FindUsersByEmail 
			+ FindUsersByName 
			+ GetAllUsers 
			+ GetPassword 
			+ GetUser 
			+ GetUserNameByEmail 
			+ ValidateUser,
		WriteOnly = ChangePassword 
			+ ChangePasswordQuestionAndAnswer 
			+ CreateUser 
			+ DeleteUser 
			+ EnablePasswordReset 
			+ ResetPassword 
			+ UnlockUser 
			+ UpdateUser,
		All = ReadOnly + WriteOnly
	}
}
