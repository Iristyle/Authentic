using System.Diagnostics.CodeAnalysis;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Basic.Configuration;

namespace EPS.Web.Authentication.Basic
{
	/// <summary>   An implementation of a simple Basic authentication failure handler factory. </summary>
	/// <remarks>   ebrown, 1/3/2011. </remarks>
	[SuppressMessage("Gendarme.Rules.Naming", "AvoidRedundancyInTypeNameRule", Justification = "Redundancy in type name is to avoid naming clashes / make class name more clear")]
	public class BasicFailureHandlerFactory :
		FailureHandlerFactoryBase<IBasicFailureHandlerConfiguration>
	{
		#region IFailureHandlerFactory Members
		/// <summary>   
		/// Constructs a new instance of a <see cref="T:EPS.Web.Authentication.Basic.AuthenticationFailureHandler"/>, returning a
		/// <see cref="T:
		/// EPS.Web.Authentication.Abstractions.IFailureHandler{IFailureHandlerConfigurationSection}"/> . 
		/// </summary>
		/// <remarks>   ebrown, 1/3/2011. </remarks>
		/// <param name="config">   The configuration. </param>
		/// <returns>   A new failure handler instance. </returns>
		public override IFailureHandler<IBasicFailureHandlerConfiguration> Construct(IBasicFailureHandlerConfiguration config)
		{
			return new BasicFailureHandler(config);
		}
		#endregion
	}
}
