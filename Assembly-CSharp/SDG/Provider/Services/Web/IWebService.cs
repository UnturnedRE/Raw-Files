﻿// Decompiled with JetBrains decompiler
// Type: SDG.Provider.Services.Web.IWebService
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services;

namespace SDG.Provider.Services.Web
{
  public interface IWebService : IService
  {
    IWebRequestHandle createRequest(string url, ERequestType requestType, WebRequestReadyCallback webRequestReadyCallback);

    void updateRequest(IWebRequestHandle webRequestHandle, string key, string value);

    void submitRequest(IWebRequestHandle webRequestHandle);

    void releaseRequest(IWebRequestHandle webRequestHandle);

    uint getResponseBodySize(IWebRequestHandle webRequestHandle);

    void getResponseBodyData(IWebRequestHandle webRequestHandle, byte[] data, uint size);
  }
}
