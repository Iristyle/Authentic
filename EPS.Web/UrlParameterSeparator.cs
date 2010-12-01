using System;
using EPS.Text;

namespace EPS.Web
{
    /// <summary>   Values that represent separators in a URL. </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public enum UrlParameterSeparator
    {
        /// <summary> No separator.  </summary>
        [EnumValue("")]
        None,
        /// <summary> Query string separator ?.  </summary>
        [EnumValue("?")]
        QuestionMark,
        /// <summary> Query parameter separator &amp;.  </summary>
        [EnumValue("&")]
        Ampersand
    }
}
