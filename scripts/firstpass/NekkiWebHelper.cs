using System;

public class NekkiWebHelper
{
    public static NekkiWebRequest DownloadFile(string url, string path, Action<NekkiWebRequest> onSuccessful, Action<NekkiWebRequest> onError, Action<NekkiWebRequest> onProgress = null, object data = null, float timeout = 5f, bool checkCert = true)
    {
        NekkiWebRequest request = new NekkiWebRequest(timeout);
        request.OnSuccessful += onSuccessful;
        request.OnError += onError;
        request.OnProgress += onProgress;
        request.SetExternalData(data);
        request.Send(url, checkCert);
        return request;
    }

    public static NekkiWebRequest SendRequest(string url, Action<NekkiWebRequest> onSuccessful, Action<NekkiWebRequest> onError, Action<NekkiWebRequest> onProgress = null, object data = null, float timeout = 5f, bool checkCert = true)
    {
        NekkiWebRequest request = new NekkiWebRequest(timeout);
        request.OnSuccessful += onSuccessful;
        request.OnError += onError;
        request.OnProgress += onProgress;
        request.SetExternalData(data);
        request.Send(url, checkCert);
        return request;
    }
}
