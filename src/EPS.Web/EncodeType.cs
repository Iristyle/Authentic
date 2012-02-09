namespace EPS.Web
{
    /// <summary>   A simple enum that can be used as method parameter to direct whether Html or Url encoding is used. </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public enum EncodeType
    {
        /// <summary> Use System.Web.HttpUtility.UrlEncode. / System.Uri.EscapeDataString </summary>
        Url,
        /// <summary> Use System.Web.HttpUtility.HtmlEncode / System.Net.WebUtility.HtmlEncode  </summary>
        Html
    }
}
