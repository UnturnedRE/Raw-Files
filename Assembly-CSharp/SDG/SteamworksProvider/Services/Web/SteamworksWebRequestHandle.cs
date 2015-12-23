// Decompiled with JetBrains decompiler
// Type: SDG.SteamworksProvider.Services.Web.SteamworksWebRequestHandle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services.Web;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Web
{
  public class SteamworksWebRequestHandle : IWebRequestHandle
  {
    private HTTPRequestHandle httpRequestHandle;
    private CallResult<HTTPRequestCompleted_t> httpRequestCompletedCallResult;
    private WebRequestReadyCallback webRequestReadyCallback;

    public SteamworksWebRequestHandle(HTTPRequestHandle newHTTPRequestHandle, WebRequestReadyCallback newWebRequestReadyCallback)
    {
      this.setHTTPRequestHandle(newHTTPRequestHandle);
      this.webRequestReadyCallback = newWebRequestReadyCallback;
    }

    public HTTPRequestHandle getHTTPRequestHandle()
    {
      return this.httpRequestHandle;
    }

    protected void setHTTPRequestHandle(HTTPRequestHandle newHTTPRequestHandle)
    {
      this.httpRequestHandle = newHTTPRequestHandle;
    }

    public void setHTTPRequestCompletedCallResult(CallResult<HTTPRequestCompleted_t> newHTTPRequestCompletedCallResult)
    {
      this.httpRequestCompletedCallResult = newHTTPRequestCompletedCallResult;
    }

    public void triggerWebRequestReadyCallback()
    {
      if (this.webRequestReadyCallback == null)
        return;
      this.webRequestReadyCallback((IWebRequestHandle) this);
    }
  }
}
