namespace Network.core.data
{
    public class SFSProtocolResponse
    {
        protected int _code;

        protected string _message;

        protected byte[] _byteArray;

        public int Code
        {
            get
            {
                return _code;
            }
        }

        public string Message
        {
            get
            {
                return _message;
            }
        }

        public byte[] ByteArray
        {
            get
            {
                return _byteArray;
            }
        }

        public void Init(int code, string message, byte[] byteArray)
        {
            _code = code;
            _message = message;
            _byteArray = byteArray;
        }
    }
}
