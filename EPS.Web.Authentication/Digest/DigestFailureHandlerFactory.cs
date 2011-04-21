using System.Diagnostics.CodeAnalysis;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Digest.Configuration;

namespace EPS.Web.Authentication.Digest
{
	/// <summary>   An implementation of a simple Digest authentication failure handler factory. </summary>
	/// <remarks>   ebrown, 1/3/2011. </remarks>
	[SuppressMessage("Gendarme.Rules.Naming", "AvoidRedundancyInTypeNameRule", Justification = "Redundancy in type name is to avoid naming clashes / make class name more clear")]
	public class DigestFailureHandlerFactory :
		FailureHandlerFactoryBase<IDigestFailureHandlerConfiguration>
	{
		#region IDigestFailureHandlerFactory Members
		/// <summary>   
		/// Constructs a new instance of a <see cref="T:EPS.Web.Authentication.Digest.DigestFailureHandler"/>, returning a
		/// <see cref="T:
		/// EPS.Web.Authentication.Abstractions.IFailureHandler{IFailureHandlerConfigurationSection}"/> . 
		/// </summary>
		/// <remarks>   ebrown, 1/3/2011. </remarks>
		/// <param name="config">   The configuration. </param>
		/// <returns>   A new failure handler instance. </returns>
		public override IFailureHandler<IDigestFailureHandlerConfiguration> Construct(IDigestFailureHandlerConfiguration config)
		{
			return new DigestFailureHandler(config);
		}
		#endregion
	}
}