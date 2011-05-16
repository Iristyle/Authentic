using System;

namespace EPS.Web.Authentication.Digest
{
	/// <summary>	
	/// Interface for the password retriever. Digest authentication requires an implementation of some sort of password retrieval.  This can
	/// be through a membership provider that supports GetPassword(), or it can be through a custom PasswordRetriever. 
	/// </summary>
	/// <remarks>	ebrown, 5/16/2011. </remarks>
	public interface IPasswordRetriever
	{
		/// <summary>	Gets a password. </summary>
		/// <param name="userName">	Name of the user. </param>
		/// <returns>	The password. </returns>
		string GetPassword(string userName);
	}
}
