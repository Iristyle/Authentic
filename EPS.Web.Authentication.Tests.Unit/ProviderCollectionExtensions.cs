using System;
using System.Collections;
using System.Configuration.Provider;
using System.Reflection;
using System.Web.Security;
using System.Diagnostics.CodeAnalysis;

namespace EPS.Web.Authentication
{
	//some nice hacks ;0
	//http://owlsayswoot.therandomist.com/2010/07/28/mocking-membership-provider/
	public static class ProviderCollectionExtensions
	{
		//hacks around the fact that we can't call ProviderCollection.Add
		public static void AddMembershipProvider(this ProviderCollection providers, MembershipProvider provider)
		{
			GetMembershipHashtable(providers).Add(provider.Name, provider);
		}

		public static void AddMembershipProvider(this ProviderCollection providers, string providerName, MembershipProvider provider)
		{
			GetMembershipHashtable(providers).Add(providerName, provider);
		}

		public static void RemoveMembershipProvider(this ProviderCollection providers, string providerName)
		{
			GetMembershipHashtable(providers).Remove(providerName);
		}

		static Hashtable GetMembershipHashtable(ProviderCollection providers)
		{
			var hashtableField = typeof(ProviderCollection).GetField("_Hashtable", BindingFlags.Instance | BindingFlags.NonPublic);
			return hashtableField.GetValue(providers) as Hashtable;
		}
	}
}
