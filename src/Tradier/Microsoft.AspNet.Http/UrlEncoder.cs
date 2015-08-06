using System.Web;

namespace Microsoft.Framework.WebEncoders {
    internal class UrlEncoder {
        public static UrlEncoder Default { get; } = new UrlEncoder();

        public string UrlEncode(string value) {
            return value;
            //return HttpUtility.UrlEncode(value);
        }
    }
}
