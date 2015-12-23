// Decompiled with JetBrains decompiler
// Type: SDG.SteamworksProvider.Services.Web.SteamworksWebService
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services;
using SDG.Provider.Services.Web;
using Steamworks;
using System;
using System.Collections.Generic;

namespace SDG.SteamworksProvider.Services.Web
{
  public class SteamworksWebService : Service, IService, IWebService
  {
    private List<SteamworksWebRequestHandle> steamworksWebRequestHandles;

    public SteamworksWebService()
    {
      this.steamworksWebRequestHandles = new List<SteamworksWebRequestHandle>();
    }

    private SteamworksWebRequestHandle findSteamworksWebRequestHandle(IWebRequestHandle webRequestHandle)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return this.steamworksWebRequestHandles.Find(new Predicate<SteamworksWebRequestHandle>(new SteamworksWebService.\u003CfindSteamworksWebRequestHandle\u003Ec__AnonStoreyC()
      {
        webRequestHandle = webRequestHandle
      }.\u003C\u003Em__9));
    }

    public IWebRequestHandle createRequest(string url, ERequestType requestType, WebRequestReadyCallback webRequestReadyCallback)
    {
      SteamworksWebRequestHandle webRequestHandle = new SteamworksWebRequestHandle(SteamHTTP.CreateHTTPRequest(requestType != ERequestType.GET ? EHTTPMethod.k_EHTTPMethodPOST : EHTTPMethod.k_EHTTPMethodGET, url), webRequestReadyCallback);
      this.steamworksWebRequestHandles.Add(webRequestHandle);
      return (IWebRequestHandle) webRequestHandle;
    }

    public void updateRequest(IWebRequestHandle webRequestHandle, string key, string value)
    {
      SteamHTTP.SetHTTPRequestGetOrPostParameter(this.findSteamworksWebRequestHandle(webRequestHandle).getHTTPRequestHandle(), key, value);
    }

    public void submitRequest(IWebRequestHandle webRequestHandle)
    {
      SteamworksWebRequestHandle webRequestHandle1 = this.findSteamworksWebRequestHandle(webRequestHandle);
      SteamAPICall_t pCallHandle;
      SteamHTTP.SendHTTPRequest(webRequestHandle1.getHTTPRequestHandle(), out pCallHandle);
      CallResult<HTTPRequestCompleted_t> newHTTPRequestCompletedCallResult = CallResult<HTTPRequestCompleted_t>.Create(new CallResult<HTTPRequestCompleted_t>.APIDispatchDelegate(this.onHTTPRequestCompleted));
      newHTTPRequestCompletedCallResult.Set(pCallHandle, (CallResult<HTTPRequestCompleted_t>.APIDispatchDelegate) null);
      webRequestHandle1.setHTTPRequestCompletedCallResult(newHTTPRequestCompletedCallResult);
    }

    public void releaseRequest(IWebRequestHandle webRequestHandle)
    {
      SteamworksWebRequestHandle webRequestHandle1 = this.findSteamworksWebRequestHandle(webRequestHandle);
      this.steamworksWebRequestHandles.Remove(webRequestHandle1);
      SteamHTTP.ReleaseHTTPRequest(webRequestHandle1.getHTTPRequestHandle());
    }

    public uint getResponseBodySize(IWebRequestHandle webRequestHandle)
    {
      uint unBodySize;
      SteamHTTP.GetHTTPResponseBodySize(this.findSteamworksWebRequestHandle(webRequestHandle).getHTTPRequestHandle(), out unBodySize);
      return unBodySize;
    }

    public void getResponseBodyData(IWebRequestHandle webRequestHandle, byte[] data, uint size)
    {
      SteamHTTP.GetHTTPResponseBodyData(this.findSteamworksWebRequestHandle(webRequestHandle).getHTTPRequestHandle(), data, size);
    }

    private void onHTTPRequestCompleted(HTTPRequestCompleted_t callback, bool ioFailure)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      this.steamworksWebRequestHandles.Find(new Predicate<SteamworksWebRequestHandle>(new SteamworksWebService.\u003ConHTTPRequestCompleted\u003Ec__AnonStoreyD()
      {
        callback = callback
      }.\u003C\u003Em__A)).triggerWebRequestReadyCallback();
    }
  }
}
