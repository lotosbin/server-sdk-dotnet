using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Security.Cryptography;
#if DOTNETCORE
#else
using System.Web;
#endif
using System.IO;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

namespace io.rong {
    class RongHttpClient {
        String methodUrl = null;
        String appkey = null;
        String appSecret = null;
        private Dictionary<string, string> dickList;
        private Collection<KeyValuePair<string, string>> collection;

        public RongHttpClient(String appkey, String appSecret, String methodUrl, Dictionary<string, string> dicList) {
            this.methodUrl = methodUrl;
            this.dickList = dicList;
            this.appkey = appkey;
            this.appSecret = appSecret;
        }

        public RongHttpClient(String appkey, String appSecret, String methodUrl, Collection<KeyValuePair<string, string>> postStr) {
            this.methodUrl = methodUrl;
            this.collection = postStr;
            this.appkey = appkey;
            this.appSecret = appSecret;
        }


        public String ExecutePost() {
            Random rd = new Random();
            int rd_i = rd.Next();
            String nonce = Convert.ToString(rd_i);

            String timestamp = Convert.ToString(ConvertDateTimeInt(DateTime.Now));

            String signature = GetHash(this.appSecret + nonce + timestamp);

            //ServicePointManager.ServerCertificateValidationCallback += ValidateRemoteCertificate;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            var client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("App-Key", appkey);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Nonce", nonce);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Timestamp", timestamp);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Signature", signature);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
            if (this.collection != null) {
                var responseMessage = client.PostAsync(this.methodUrl, new FormUrlEncodedContent(collection)).Result;
                return responseMessage.Content.ReadAsStringAsync().Result;
            }
            else {
                var responseMessage = client.PostAsync(this.methodUrl, new FormUrlEncodedContent(dickList)).Result;
                return responseMessage.Content.ReadAsStringAsync().Result;
            }
        }
        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"> DateTime时间格式</param>
        /// <returns>Unix时间戳格式</returns>
        public int ConvertDateTimeInt(DateTime time) {
            return (int)(time - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }

        public String GetHash(String input) {
            //建立SHA1对象
            SHA1 sha = SHA1.Create();

            //将mystr转换成byte[]
            UTF8Encoding enc = new UTF8Encoding();
            byte[] dataToHash = enc.GetBytes(input);

            //Hash运算
            byte[] dataHashed = sha.ComputeHash(dataToHash);

            //将运算结果转换成string
            string hash = BitConverter.ToString(dataHashed).Replace("-", "");

            return hash;
        }

        /// <summary>
        /// Certificate validation callback.
        /// </summary>
        private static bool ValidateRemoteCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error) {
            // If the certificate is a valid, signed certificate, return true.
            if (error == System.Net.Security.SslPolicyErrors.None) {
                return true;
            }

            Console.WriteLine("X509Certificate [{0}] Policy Error: '{1}'",
                cert.Subject,
                error.ToString());

            return false;
        }
        /**
 * 构建请求参数
 */
        private static String buildQueryStr(Dictionary<String, String> dicList) {
            String postStr = "";

            foreach (var item in dicList) {
                postStr += item.Key + "=" + WebUtility.UrlEncode(item.Value) + "&";
            }
            postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
            return postStr;
        }

    }
}
