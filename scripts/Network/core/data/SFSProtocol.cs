using Godot;
using System;
using Google.Protobuf.WellKnownTypes;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sfs2X.Entities.Data;
using common;
using Timestamp = Google.Protobuf.WellKnownTypes.Timestamp;

namespace Network.core.data
{
    public class SFSProtocol
    {
        protected Random _randGen;
        private static int currentID;

        public SFSProtocol()
        {
            _randGen = new Random(DateTime.Now.Millisecond);
        }

        protected void SetID(ref SFSObject obj)
        {
            obj.PutInt("id", GetNextRequestID());
        }

        private int GetNextRequestID()
        {
            if (currentID == int.MaxValue)
                currentID = int.MinValue;
            return ++currentID;
        }

        protected void PackDataAsBytes(ref SFSObject outObj, byte[] data)
        {
            outObj.PutByteArray("b", data);
            SetID(ref outObj);
        }

        public SFSObject PackObject(byte[] data)
        {
            SFSObject outObj = new SFSObject();
            PackDataAsBytes(ref outObj, data);
            return outObj;
        }

        public Timestamp GetTimestamp()
        {
            Timestamp timestamp = new Timestamp();
            timestamp.Value = (long)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            return timestamp;
        }

        public SFSObject GetCommonInt64(long value)
        {
            SFSObject outObj = new SFSObject();
            byte[] data = BitConverter.GetBytes(value);
            PackDataAsBytes(ref outObj, data);
            return outObj;
        }

        public SFSObject GetCommonInt32(int value)
        {
            SFSObject outObj = new SFSObject();
            byte[] data = BitConverter.GetBytes(value);
            GD.Print("param.value = " + value);
            PackDataAsBytes(ref outObj, data);
            return outObj;
        }

        public SFSObject GetCommonStr(string value)
        {
            SFSObject outObj = new SFSObject();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(value);
            GD.Print("param.value = " + value);
            PackDataAsBytes(ref outObj, data);
            return outObj;
        }

        public virtual SFSObject GetLoginRequest(string sessionToken, string deviceID, int version, JObject extraData = null)
        {
            SFSObject outObj = new SFSObject();
            LoginRequest loginRequest = new LoginRequest();
            loginRequest.Version = version;
            loginRequest.PrimaryAuthToken = getPrimaryAuthData(deviceID, PasswordUtil.MD5Password(sessionToken + deviceID));
            if (extraData == null)
                extraData = new JObject();
            extraData.Add("platform", getPlatform());
            loginRequest.ExtData = extraData.ToString(Formatting.None);
            PackDataAsBytes(ref outObj, System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(loginRequest)));
            return outObj;
        }

        public static string getPlatform()
        {
            return "Windows";
        }

        public byte[] DecodeResponse(SFSObject sfsObj)
        {
            return sfsObj.GetByteArray("b");
        }

        private AuthToken getPrimaryAuthData(string login, string password)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("login", login);
            dictionary.Add("password", password);
            AuthToken authToken = new AuthToken();
            authToken.Type = AuthType.Device;
            authToken.Data = JsonConvert.SerializeObject(dictionary);
            return authToken;
        }
    }
}
