using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using Newtonsoft.Json;

public class NekkiWebRequest
{
    private object _externalData;
    private HttpClient _httpClient;
    private bool _isDispose;
    private bool _isTimeOut;
    private float _lastUpdateTime;
    private readonly float _downloadTimeout;
    private int _currentPosition;
    private string _url;
    private bool _certCheck;
    private bool _certError;
    private byte[] _responseBytes;
    private string _responseText;
    private float _downloadProgress;
    private long _totalBytes;
    private long _downloadedBytes;
    private bool _isDone;
    private bool _isSuccessful;
    private bool _isFail;
    private string _error;
    private Task _requestTask;

    public bool IsDone { get { return _isDone || IsFail; } }
    public bool IsSuccessful { get { return _isDone && !IsFail; } }
    public bool IsFail { get { return _certError || _isTimeOut || _isFail; } }
    public string Url { get { return _url; } }
    public string Error { get { return GetError(); } }
    public byte[] Bytes { get { return _responseBytes; } }
    public string Text { get { return _responseText; } }
    public float Progress { get { return _downloadProgress; } }
    public float Total { get { return _totalBytes; } }
    public int Position { get { return (int)_downloadedBytes; } }

    public event Action<NekkiWebRequest> OnSuccessful = delegate { };
    public event Action<NekkiWebRequest> OnError = delegate { };
    public event Action<NekkiWebRequest> OnProgress = delegate { };

    public NekkiWebRequest(float timeout = 5f)
    {
        _downloadTimeout = timeout;
        Reset();
    }

    public virtual void Send(string urlRequest, bool checkCert)
    {
        _certCheck = checkCert;
        _url = urlRequest;
        _lastUpdateTime = Time.GetTicksMsec() / 1000f;
        SendRequestAsync();
    }

    public virtual void Send(string urlRequest, string postData, bool checkCert)
    {
        _certCheck = checkCert;
        _url = urlRequest;
        _lastUpdateTime = Time.GetTicksMsec() / 1000f;
        SendPostRequestAsync(postData);
    }

    public virtual void Send(string urlRequest, Dictionary<string, string> postData, bool checkCert)
    {
        _certCheck = checkCert;
        _url = urlRequest;
        _lastUpdateTime = Time.GetTicksMsec() / 1000f;
        string encoded = "";
        foreach (var kv in postData)
            encoded += Uri.EscapeUriString(kv.Key) + "=" + Uri.EscapeUriString(kv.Value) + "&";
        if (encoded.Length > 0) encoded = encoded.Substring(0, encoded.Length - 1);
        SendPostRequestAsync(encoded);
    }

    private async void SendRequestAsync()
    {
        try
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(_downloadTimeout);
            var response = await _httpClient.GetAsync(_url);
            _responseBytes = await response.Content.ReadAsByteArrayAsync();
            _responseText = System.Text.Encoding.UTF8.GetString(_responseBytes);
            _isDone = true;
            _isSuccessful = true;
            SendSuccessful();
        }
        catch (Exception ex)
        {
            _error = ex.Message;
            _isFail = true;
            SendError();
        }
    }

    private async void SendPostRequestAsync(string postData)
    {
        try
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(_downloadTimeout);
            var content = new StringContent(postData, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await _httpClient.PostAsync(_url, content);
            _responseBytes = await response.Content.ReadAsByteArrayAsync();
            _responseText = System.Text.Encoding.UTF8.GetString(_responseBytes);
            _isDone = true;
            _isSuccessful = true;
            SendSuccessful();
        }
        catch (Exception ex)
        {
            _error = ex.Message;
            _isFail = true;
            SendError();
        }
    }

    protected virtual void SendSuccessful()
    {
        OnSuccessful.InvokeSafe(this);
        ResetDelegates();
    }

    protected virtual void SendError()
    {
        OnError.InvokeSafe(this);
        ResetDelegates();
    }

    protected virtual void SendProgress()
    {
        OnProgress.InvokeSafe(this);
    }

    public void Abort(bool isError = false)
    {
        if (isError)
            SendError();
        if (!_isDispose)
        {
            _httpClient?.Dispose();
            _isDispose = true;
            _externalData = null;
        }
        Reset();
    }

    private void Reset()
    {
        _isDispose = false;
        _isTimeOut = false;
        _lastUpdateTime = 0f;
        _currentPosition = 0;
    }

    private void ResetDelegates()
    {
        OnSuccessful = null;
        OnError = null;
    }

    private bool IsResponseCodeSuccessful() { return true; }
    private bool InProgress() { return false; }

    private string GetError()
    {
        if (_certError) return "HTTPS certificate check error";
        if (_isTimeOut) return "Failed with timeout";
        return _error;
    }

    public T GetJson<T>() where T : class
    {
        try { return JsonConvert.DeserializeObject<T>(Text); }
        catch (Exception ex) { GD.PrintErr("NekkiWeb - Error Parse JSON (" + ex.Message + ")"); }
        return (T)null;
    }

    public void SetExternalData(object value) { _externalData = value; }
    public T GetExternalData<T>() { return (T)_externalData; }
}
