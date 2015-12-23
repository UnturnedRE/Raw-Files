// Decompiled with JetBrains decompiler
// Type: SDG.Provider.TempSteamworksWorkshop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.SteamworksProvider;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Provider
{
  public class TempSteamworksWorkshop
  {
    private SteamworksAppInfo appInfo;
    public TempSteamworksWorkshop.PublishedAdded onPublishedAdded;
    public TempSteamworksWorkshop.PublishedRemoved onPublishedRemoved;
    private PublishedFileId_t publishedFileID;
    private UGCQueryHandle_t ugcRequest;
    private string ugcName;
    private string ugcDescription;
    private string ugcPath;
    private string ugcPreview;
    private string ugcChange;
    private ESteamUGCType ugcType;
    private string ugcTag;
    private ESteamUGCVisibility ugcVisibility;
    public int installed;
    public List<PublishedFileId_t> downloaded;
    public List<PublishedFileId_t> installing;
    private List<SteamContent> _ugc;
    private List<SteamPublished> _published;
    private CallResult<CreateItemResult_t> createItemResult;
    private CallResult<SubmitItemUpdateResult_t> submitItemUpdateResult;
    private CallResult<SteamUGCQueryCompleted_t> queryCompleted;
    private Callback<DownloadItemResult_t> itemDownloaded;

    public bool canOpenWorkshop
    {
      get
      {
        return SteamUtils.IsOverlayEnabled();
      }
    }

    public List<SteamContent> ugc
    {
      get
      {
        return this._ugc;
      }
    }

    public List<SteamPublished> published
    {
      get
      {
        return this._published;
      }
    }

    public TempSteamworksWorkshop(SteamworksAppInfo newAppInfo)
    {
      this.appInfo = newAppInfo;
      this.downloaded = new List<PublishedFileId_t>();
      if (this.appInfo.isDedicated)
        return;
      this.createItemResult = CallResult<CreateItemResult_t>.Create(new CallResult<CreateItemResult_t>.APIDispatchDelegate(this.onCreateItemResult));
      this.submitItemUpdateResult = CallResult<SubmitItemUpdateResult_t>.Create(new CallResult<SubmitItemUpdateResult_t>.APIDispatchDelegate(this.onSubmitItemUpdateResult));
      this.queryCompleted = CallResult<SteamUGCQueryCompleted_t>.Create(new CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate(this.onQueryCompleted));
      this.itemDownloaded = Callback<DownloadItemResult_t>.Create(new Callback<DownloadItemResult_t>.DispatchDelegate(this.onItemDownloaded));
    }

    public void open(PublishedFileId_t id)
    {
      SteamFriends.ActivateGameOverlayToWebPage("http://steamcommunity.com/sharedfiles/filedetails/?id=" + (object) id.m_PublishedFileId);
    }

    private void onCreateItemResult(CreateItemResult_t callback, bool io)
    {
      Debug.Log((object) ("onCreateItemResult:" + (object) (bool) (callback.m_bUserNeedsToAcceptWorkshopLegalAgreement ? 1 : 0) + " " + (string) (object) callback.m_eResult + " " + (string) (object) (bool) (io ? 1 : 0)));
      if (callback.m_bUserNeedsToAcceptWorkshopLegalAgreement || callback.m_eResult != EResult.k_EResultOK || io)
      {
        MenuUI.alert(Provider.localization.format("UGC_Fail"));
      }
      else
      {
        this.publishedFileID = callback.m_nPublishedFileId;
        this.updateUGC();
      }
    }

    private void onSubmitItemUpdateResult(SubmitItemUpdateResult_t callback, bool io)
    {
      Debug.Log((object) ("onSubmitItemUpdateResult:" + (object) (bool) (callback.m_bUserNeedsToAcceptWorkshopLegalAgreement ? 1 : 0) + " " + (string) (object) callback.m_eResult + " " + (string) (object) (bool) (io ? 1 : 0)));
      if (callback.m_bUserNeedsToAcceptWorkshopLegalAgreement || callback.m_eResult != EResult.k_EResultOK || io)
      {
        MenuUI.alert(Provider.localization.format("UGC_Fail"));
      }
      else
      {
        MenuUI.alert(Provider.localization.format("UGC_Success"));
        Provider.provider.workshopService.open(this.publishedFileID);
        this.refreshPublished();
      }
    }

    private void onQueryCompleted(SteamUGCQueryCompleted_t callback, bool io)
    {
      if (callback.m_eResult != EResult.k_EResultOK || io)
        return;
      for (uint index = 0U; index < callback.m_unNumResultsReturned; ++index)
      {
        SteamUGCDetails_t pDetails;
        SteamUGC.GetQueryUGCResult(this.ugcRequest, index, out pDetails);
        this.published.Add(new SteamPublished(pDetails.m_rgchTitle, pDetails.m_nPublishedFileId));
      }
      if (this.onPublishedAdded == null)
        return;
      this.onPublishedAdded();
    }

    private void onItemDownloaded(DownloadItemResult_t callback)
    {
      if (this.installing == null || this.installing.Count == 0)
        return;
      this.installing.Remove(callback.m_nPublishedFileId);
      LoadingUI.updateProgress((float) (this.installed - this.installing.Count) / (float) this.installed);
      ulong punSizeOnDisk;
      string pchFolder;
      uint punTimeStamp;
      if (SteamUGC.GetItemInstallInfo(callback.m_nPublishedFileId, out punSizeOnDisk, out pchFolder, 1024U, out punTimeStamp) && ReadWrite.folderExists(pchFolder, false))
      {
        if (WorkshopTool.checkMapMeta(pchFolder, false))
          this.ugc.Add(new SteamContent(callback.m_nPublishedFileId, pchFolder, ESteamUGCType.MAP));
        else if (WorkshopTool.checkLocalizationMeta(pchFolder, false))
          this.ugc.Add(new SteamContent(callback.m_nPublishedFileId, pchFolder, ESteamUGCType.LOCALIZATION));
        else if (WorkshopTool.checkObjectMeta(pchFolder, false))
        {
          this.ugc.Add(new SteamContent(callback.m_nPublishedFileId, pchFolder, ESteamUGCType.OBJECT));
          Assets.load(pchFolder, false);
        }
        else if (WorkshopTool.checkItemMeta(pchFolder, false))
        {
          this.ugc.Add(new SteamContent(callback.m_nPublishedFileId, pchFolder, ESteamUGCType.ITEM));
          Assets.load(pchFolder, false);
        }
        else if (WorkshopTool.checkVehicleMeta(pchFolder, false))
        {
          this.ugc.Add(new SteamContent(callback.m_nPublishedFileId, pchFolder, ESteamUGCType.VEHICLE));
          Assets.load(pchFolder, false);
        }
      }
      if (this.installing.Count == 0)
        Provider.launch();
      else
        SteamUGC.DownloadItem(this.installing[0], true);
    }

    private void cleanupUGCRequest()
    {
      if (this.ugcRequest == UGCQueryHandle_t.Invalid)
        return;
      SteamUGC.ReleaseQueryUGCRequest(this.ugcRequest);
      this.ugcRequest = UGCQueryHandle_t.Invalid;
    }

    public void prepareUGC(string name, string description, string path, string preview, string change, ESteamUGCType type, string tag, ESteamUGCVisibility visibility)
    {
      this.ugcName = name;
      this.ugcDescription = description;
      this.ugcPath = path;
      this.ugcPreview = preview;
      this.ugcChange = change;
      this.ugcType = type;
      this.ugcTag = tag;
      this.ugcVisibility = visibility;
    }

    public void prepareUGC(PublishedFileId_t id)
    {
      this.publishedFileID = id;
    }

    public void createUGC(bool ugcFor)
    {
      this.createItemResult.Set(SteamUGC.CreateItem(SteamUtils.GetAppID(), !ugcFor ? EWorkshopFileType.k_EWorkshopFileTypeFirst : EWorkshopFileType.k_EWorkshopFileTypeMicrotransaction), (CallResult<CreateItemResult_t>.APIDispatchDelegate) null);
    }

    public void updateUGC()
    {
      UGCUpdateHandle_t ugcUpdateHandleT = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), this.publishedFileID);
      if (this.ugcType == ESteamUGCType.MAP)
        ReadWrite.writeBytes(this.ugcPath + "/Map.meta", false, false, new byte[1]);
      else if (this.ugcType == ESteamUGCType.LOCALIZATION)
        ReadWrite.writeBytes(this.ugcPath + "/Localization.meta", false, false, new byte[1]);
      else if (this.ugcType == ESteamUGCType.OBJECT)
        ReadWrite.writeBytes(this.ugcPath + "/Object.meta", false, false, new byte[1]);
      else if (this.ugcType == ESteamUGCType.ITEM)
        ReadWrite.writeBytes(this.ugcPath + "/Item.meta", false, false, new byte[1]);
      else if (this.ugcType == ESteamUGCType.VEHICLE)
        ReadWrite.writeBytes(this.ugcPath + "/Vehicle.meta", false, false, new byte[1]);
      else if (this.ugcType == ESteamUGCType.SKIN)
        ReadWrite.writeBytes(this.ugcPath + "/Skin.meta", false, false, new byte[1]);
      SteamUGC.SetItemContent(ugcUpdateHandleT, this.ugcPath);
      if (this.ugcDescription.Length > 0)
        SteamUGC.SetItemDescription(ugcUpdateHandleT, this.ugcDescription);
      if (this.ugcPreview.Length > 0)
        SteamUGC.SetItemPreview(ugcUpdateHandleT, this.ugcPreview);
      List<string> list = new List<string>();
      if (this.ugcType == ESteamUGCType.MAP)
        list.Add("Map");
      else if (this.ugcType == ESteamUGCType.LOCALIZATION)
        list.Add("Localization");
      else if (this.ugcType == ESteamUGCType.OBJECT)
        list.Add("Object");
      else if (this.ugcType == ESteamUGCType.ITEM)
        list.Add("Item");
      else if (this.ugcType == ESteamUGCType.VEHICLE)
        list.Add("Vehicle");
      else if (this.ugcType == ESteamUGCType.SKIN)
        list.Add("Skin");
      if (this.ugcTag != null && this.ugcTag.Length > 0)
        list.Add(this.ugcTag);
      SteamUGC.SetItemTags(ugcUpdateHandleT, (IList<string>) list.ToArray());
      if (this.ugcName.Length > 0)
        SteamUGC.SetItemTitle(ugcUpdateHandleT, this.ugcName);
      if (this.ugcVisibility == ESteamUGCVisibility.PUBLIC)
        SteamUGC.SetItemVisibility(ugcUpdateHandleT, ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic);
      else if (this.ugcVisibility == ESteamUGCVisibility.FRIENDS_ONLY)
        SteamUGC.SetItemVisibility(ugcUpdateHandleT, ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityFriendsOnly);
      else if (this.ugcVisibility == ESteamUGCVisibility.PRIVATE)
        SteamUGC.SetItemVisibility(ugcUpdateHandleT, ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPrivate);
      this.submitItemUpdateResult.Set(SteamUGC.SubmitItemUpdate(ugcUpdateHandleT, this.ugcChange), (CallResult<SubmitItemUpdateResult_t>.APIDispatchDelegate) null);
    }

    public void refreshUGC()
    {
      this._ugc = new List<SteamContent>();
      uint numSubscribedItems = SteamUGC.GetNumSubscribedItems();
      PublishedFileId_t[] pvecPublishedFileID = new PublishedFileId_t[(IntPtr) numSubscribedItems];
      int num = (int) SteamUGC.GetSubscribedItems(pvecPublishedFileID, numSubscribedItems);
      for (uint index = 0U; index < numSubscribedItems; ++index)
      {
        ulong punSizeOnDisk;
        string pchFolder;
        uint punTimeStamp;
        if (SteamUGC.GetItemInstallInfo(pvecPublishedFileID[(IntPtr) index], out punSizeOnDisk, out pchFolder, 1024U, out punTimeStamp) && ReadWrite.folderExists(pchFolder, false))
        {
          if (WorkshopTool.checkMapMeta(pchFolder, false))
            this.ugc.Add(new SteamContent(pvecPublishedFileID[(IntPtr) index], pchFolder, ESteamUGCType.MAP));
          else if (WorkshopTool.checkLocalizationMeta(pchFolder, false))
            this.ugc.Add(new SteamContent(pvecPublishedFileID[(IntPtr) index], pchFolder, ESteamUGCType.LOCALIZATION));
          else if (WorkshopTool.checkObjectMeta(pchFolder, false))
            this.ugc.Add(new SteamContent(pvecPublishedFileID[(IntPtr) index], pchFolder, ESteamUGCType.OBJECT));
          else if (WorkshopTool.checkItemMeta(pchFolder, false))
            this.ugc.Add(new SteamContent(pvecPublishedFileID[(IntPtr) index], pchFolder, ESteamUGCType.ITEM));
          else if (WorkshopTool.checkVehicleMeta(pchFolder, false))
            this.ugc.Add(new SteamContent(pvecPublishedFileID[(IntPtr) index], pchFolder, ESteamUGCType.VEHICLE));
        }
      }
    }

    public void refreshPublished()
    {
      if (this.onPublishedRemoved != null)
        this.onPublishedRemoved();
      this.cleanupUGCRequest();
      this._published = new List<SteamPublished>();
      this.ugcRequest = SteamUGC.CreateQueryUserUGCRequest(Provider.client.GetAccountID(), EUserUGCList.k_EUserUGCList_Published, EUGCMatchingUGCType.k_EUGCMatchingUGCType_Items, EUserUGCListSortOrder.k_EUserUGCListSortOrder_CreationOrderAsc, SteamUtils.GetAppID(), SteamUtils.GetAppID(), 1U);
      this.queryCompleted.Set(SteamUGC.SendQueryUGCRequest(this.ugcRequest), (CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate) null);
    }

    public delegate void PublishedAdded();

    public delegate void PublishedRemoved();
  }
}
