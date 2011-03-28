using System;
using EPS.Text;

namespace EPS.Web
{
    /// <summary>   Values that represent the valid values as defined in <a href="http://tools.ietf.org/html/rfc2617#section-3.2.2">RFC 2617 section 3.2.2.</a> </summary>
    /// <remarks>   ebrown, 3/28/2011. </remarks>
    public enum DigestQualityOfProtectionType
    {
        /// <summary> No quality of protection specified, effectively emulating <a href="http://tools.ietf.org/html/rfc2069">RFC 2069</a>.  </summary>
        [EnumValue("")]
        Unspecified = 0,

        /// <summary> Quality of protection is simply authentication.  </summary>
        [EnumValue("auth")]
        Authentication,

        /// <summary> Quality of protection is authneitcation with integrity protection.  </summary>
        [EnumValue("auth-int")]
        AuthenticationWithIntegrity,
    }
}
