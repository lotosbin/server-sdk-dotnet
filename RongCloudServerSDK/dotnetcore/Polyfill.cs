using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RongCloudServerSDK.dotnetcore {
    public class HttpUtility {
        internal static string UrlEncode(string value, Encoding uTF8) {
            return WebUtility.UrlEncode(value);
        }

        public static string UrlDecode(string arrParam, Encoding utf8) {
            return WebUtility.UrlDecode(arrParam);
        }
    }
}
