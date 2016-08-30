using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.ObjectModel;
#if DOTNETCORE
using RongCloudServerSDK.dotnetcore;
#else
using System.Web;
#endif
namespace io.rong {
    public class RongCloudServer {
        private static Collection<KeyValuePair<string, string>> buildParamStr(String[] arrParams) {
            var dicList = new Collection<KeyValuePair<String, String>>();
            foreach (string t in arrParams) {
                dicList.Add(new KeyValuePair<string, string>("chatroomId", t));
            }
            return dicList;
        }

        /**
         * 获取 token
         */
        public static String GetToken(String appkey, String appSecret, String userId, String name, String portraitUri) {
            Dictionary<String, String> dicList = new Dictionary<String, String>{
                {"userId", userId},
                {"name", name},
                {"portraitUri", portraitUri}
            };

            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.getTokenUrl, dicList);

            return client.ExecutePost();

        }

        /**
         * 加入 群组
         */
        public static String JoinGroup(String appkey, String appSecret, String userId, String groupId, String groupName) {
            Dictionary<String, String> dicList = new Dictionary<String, String>{
                {"userId", userId},
                {"groupId", groupId},
                {"groupName", groupName}
            };


            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.joinGroupUrl, dicList);

            return client.ExecutePost();
        }

        /**
         * 退出 群组
         */
        public static String QuitGroup(String appkey, String appSecret, String userId, String groupId) {
            Dictionary<String, String> dicList = new Dictionary<String, String>{
                {"userId", userId},
                {"groupId", groupId}
            };


            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.quitGroupUrl, dicList);

            return client.ExecutePost();
        }

        /**
         * 解散 群组
         */
        public static String DismissGroup(String appkey, String appSecret, String userId, String groupId) {
            Dictionary<String, String> dicList = new Dictionary<String, String>{
                {"userId", userId},
                {"groupId", groupId}
            };


            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.dismissUrl, dicList);

            return client.ExecutePost();

        }

        /**
         * 同步群组
         */
        public static String syncGroup(String appkey, String appSecret, String userId, String[] groupId, String[] groupName) {
            var dicList = new Dictionary<String, String> { { "userId", userId } };

            for (int i = 0; i < groupId.Length; i++) {
                var id = HttpUtility.UrlEncode(groupId[i], Encoding.UTF8);
                var name = HttpUtility.UrlEncode(groupName[i], Encoding.UTF8);
                dicList.Add("group[" + id + "]", name);
            }
            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.syncGroupUrl, dicList);

            return client.ExecutePost();
        }


        /// <summary>
        /// 发送二人消息
        /// </summary>
        /// <param name="appkey"></param>
        /// <param name="appSecret"></param>
        /// <param name="fromUserId"></param>
        /// <param name="toUserId"></param>
        /// <param name="objectName"></param>
        /// <param name="content">RC:TxtMsg消息格式{"content":"hello"}  RC:ImgMsg消息格式{"content":"ergaqreg", "imageKey":"http://www.demo.com/1.jpg"}  RC:VcMsg消息格式{"content":"ergaqreg","duration":3}</param>
        /// <returns></returns>
        public static String PublishMessage(String appkey, String appSecret, String fromUserId, String toUserId, String objectName, String content) {
            //此数据结构不适用多个toUserId情况,请注意
            Dictionary<String, String> dicList = new Dictionary<String, String>{
                {"fromUserId", fromUserId},
                {"toUserId", toUserId},
                {"objectName", objectName},
                {"content", content}
            };


            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.sendMsgUrl, dicList);

            return client.ExecutePost();
        }
        /// <summary>
        /// 广播消息暂时未开放
        /// </summary>
        /// <param name="appkey"></param>
        /// <param name="appSecret"></param>
        /// <param name="fromUserId"></param>
        /// <param name="objectName"></param>
        /// <param name="content">RC:TxtMsg消息格式{"content":"hello"}  RC:ImgMsg消息格式{"content":"ergaqreg", "imageKey":"http://www.demo.com/1.jpg"}  RC:VcMsg消息格式{"content":"ergaqreg","duration":3}</param>
        /// <returns></returns>
        public static String BroadcastMessage(String appkey, String appSecret, String fromUserId, String objectName, String content) {
            Dictionary<String, String> dicList = new Dictionary<String, String>{
                {"content", content},
                {"fromUserId", fromUserId},
                {"objectName", objectName},
                {"pushContent", ""},
                {"pushData", ""}
            };

            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.broadcastUrl, dicList);

            return client.ExecutePost();
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="appkey"></param>
        /// <param name="appSecret"></param>
        /// <param name="chatroomInfo">chatroom[id10001]=name1001</param>
        /// <returns></returns>
        public static String CreateChatroom(String appkey, String appSecret, String[] chatroomId, String[] chatroomName) {
            var dicList = new Dictionary<String, String>();

            for (int i = 0; i < chatroomId.Length; i++) {
                var id = HttpUtility.UrlEncode(chatroomId[i], Encoding.UTF8);
                var name = HttpUtility.UrlEncode(chatroomName[i], Encoding.UTF8);
                dicList.Add("chatroom[" + id + "]", name);
            }

            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.createChatroomUrl, dicList);

            return client.ExecutePost();
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="appkey"></param>
        /// <param name="appSecret"></param>
        /// <param name="chatroomIdInfo">chatroomId=id1001</param>
        /// <returns></returns>
        public static String DestroyChatroom(String appkey, String appSecret, String[] chatroomIdInfo) {
            var postStr = buildParamStr(chatroomIdInfo);

            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.destroyChatroomUrl, postStr);

            return client.ExecutePost();
        }
        public static String queryChatroom(String appkey, String appSecret, String[] chatroomId) {
            var postStr = buildParamStr(chatroomId);

            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.queryChatroomUrl, postStr);

            return client.ExecutePost();
        }
    }
}
