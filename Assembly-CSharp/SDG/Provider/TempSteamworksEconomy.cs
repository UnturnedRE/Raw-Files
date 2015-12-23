// Decompiled with JetBrains decompiler
// Type: SDG.Provider.TempSteamworksEconomy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.IO.Deserialization;
using SDG.SteamworksProvider;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace SDG.Provider
{
  public class TempSteamworksEconomy
  {
    public SteamInventoryResult_t promoResult = SteamInventoryResult_t.Invalid;
    public SteamInventoryResult_t exchangeResult = SteamInventoryResult_t.Invalid;
    public SteamInventoryResult_t dropResult = SteamInventoryResult_t.Invalid;
    public SteamInventoryResult_t wearingResult = SteamInventoryResult_t.Invalid;
    public SteamInventoryResult_t inventoryResult = SteamInventoryResult_t.Invalid;
    public SteamItemDetails_t[] inventoryDetails = new SteamItemDetails_t[0];
    private List<UnturnedEconInfo> econInfo;
    public TempSteamworksEconomy.InventoryRefreshed onInventoryRefreshed;
    public TempSteamworksEconomy.InventoryDropped onInventoryDropped;
    public TempSteamworksEconomy.InventoryExchanged onInventoryExchanged;
    private SteamworksAppInfo appInfo;
    private Callback<SteamInventoryResultReady_t> inventoryResultReady;
    private Callback<GameOverlayActivated_t> gameOverlayActivated;

    public bool canOpenInventory
    {
      get
      {
        return SteamUtils.IsOverlayEnabled();
      }
    }

    public SteamItemDetails_t[] inventory
    {
      get
      {
        return this.inventoryDetails;
      }
    }

    public TempSteamworksEconomy(SteamworksAppInfo newAppInfo)
    {
      this.appInfo = newAppInfo;
      this.econInfo = new List<UnturnedEconInfo>();
      if (this.appInfo.isDedicated)
      {
        this.inventoryResultReady = Callback<SteamInventoryResultReady_t>.CreateGameServer(new Callback<SteamInventoryResultReady_t>.DispatchDelegate(this.onInventoryResultReady));
      }
      else
      {
        this.econInfo = new JSONDeserializer<List<UnturnedEconInfo>>().deserialize(ReadWrite.PATH + "/EconInfo.json");
        this.inventoryResultReady = Callback<SteamInventoryResultReady_t>.Create(new Callback<SteamInventoryResultReady_t>.DispatchDelegate(this.onInventoryResultReady));
        this.gameOverlayActivated = Callback<GameOverlayActivated_t>.Create(new Callback<GameOverlayActivated_t>.DispatchDelegate(this.onGameOverlayActivated));
      }
    }

    public void open(ulong id)
    {
      SteamFriends.ActivateGameOverlayToWebPage("http://steamcommunity.com/profiles/" + (object) SteamUser.GetSteamID() + "/inventory/?sellOnLoad=1#" + (string) (object) SteamUtils.GetAppID() + "_2_" + (string) (object) id);
    }

    public ulong getInventoryPackage(int item)
    {
      if (this.inventoryDetails != null)
      {
        for (int index = 0; index < this.inventoryDetails.Length; ++index)
        {
          if (this.inventoryDetails[index].m_iDefinition.m_SteamItemDef == item)
            return this.inventoryDetails[index].m_itemId.m_SteamItemInstanceID;
        }
      }
      return 0UL;
    }

    public int getInventoryItem(ulong package)
    {
      if (this.inventoryDetails != null)
      {
        for (int index = 0; index < this.inventoryDetails.Length; ++index)
        {
          if ((long) this.inventoryDetails[index].m_itemId.m_SteamItemInstanceID == (long) package)
            return this.inventoryDetails[index].m_iDefinition.m_SteamItemDef;
        }
      }
      return 0;
    }

    public string getInventoryName(int item)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      UnturnedEconInfo unturnedEconInfo = this.econInfo.Find(new Predicate<UnturnedEconInfo>(new TempSteamworksEconomy.\u003CgetInventoryName\u003Ec__AnonStorey3()
      {
        item = item
      }.\u003C\u003Em__0));
      if (unturnedEconInfo == null)
        return string.Empty;
      return unturnedEconInfo.name;
    }

    public string getInventoryType(int item)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      UnturnedEconInfo unturnedEconInfo = this.econInfo.Find(new Predicate<UnturnedEconInfo>(new TempSteamworksEconomy.\u003CgetInventoryType\u003Ec__AnonStorey4()
      {
        item = item
      }.\u003C\u003Em__1));
      if (unturnedEconInfo == null)
        return string.Empty;
      return unturnedEconInfo.type;
    }

    public string getInventoryDescription(int item)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      UnturnedEconInfo unturnedEconInfo = this.econInfo.Find(new Predicate<UnturnedEconInfo>(new TempSteamworksEconomy.\u003CgetInventoryDescription\u003Ec__AnonStorey5()
      {
        item = item
      }.\u003C\u003Em__2));
      if (unturnedEconInfo == null)
        return string.Empty;
      return unturnedEconInfo.description;
    }

    public bool getInventoryMarketable(int item)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      UnturnedEconInfo unturnedEconInfo = this.econInfo.Find(new Predicate<UnturnedEconInfo>(new TempSteamworksEconomy.\u003CgetInventoryMarketable\u003Ec__AnonStorey6()
      {
        item = item
      }.\u003C\u003Em__3));
      if (unturnedEconInfo == null)
        return false;
      return unturnedEconInfo.marketable;
    }

    public Color getInventoryColor(int item)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      UnturnedEconInfo unturnedEconInfo = this.econInfo.Find(new Predicate<UnturnedEconInfo>(new TempSteamworksEconomy.\u003CgetInventoryColor\u003Ec__AnonStorey7()
      {
        item = item
      }.\u003C\u003Em__4));
      uint result;
      if (unturnedEconInfo == null || unturnedEconInfo.name_color == null || (unturnedEconInfo.name_color.Length <= 0 || !uint.TryParse(unturnedEconInfo.name_color, NumberStyles.HexNumber, (IFormatProvider) CultureInfo.CurrentCulture, out result)))
        return Color.white;
      return new Color((float) (result >> 16 & (uint) byte.MaxValue) / (float) byte.MaxValue, (float) (result >> 8 & (uint) byte.MaxValue) / (float) byte.MaxValue, (float) (result & (uint) byte.MaxValue) / (float) byte.MaxValue);
    }

    public ushort getInventoryMythicID(int item)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      UnturnedEconInfo unturnedEconInfo = this.econInfo.Find(new Predicate<UnturnedEconInfo>(new TempSteamworksEconomy.\u003CgetInventoryMythicID\u003Ec__AnonStorey8()
      {
        item = item
      }.\u003C\u003Em__5));
      if (unturnedEconInfo == null)
        return (ushort) 0;
      return (ushort) unturnedEconInfo.item_effect;
    }

    public ushort getInventoryItemID(int item)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      UnturnedEconInfo unturnedEconInfo = this.econInfo.Find(new Predicate<UnturnedEconInfo>(new TempSteamworksEconomy.\u003CgetInventoryItemID\u003Ec__AnonStorey9()
      {
        item = item
      }.\u003C\u003Em__6));
      if (unturnedEconInfo == null)
        return (ushort) 0;
      return (ushort) unturnedEconInfo.item_id;
    }

    public ushort getInventorySkinID(int item)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      UnturnedEconInfo unturnedEconInfo = this.econInfo.Find(new Predicate<UnturnedEconInfo>(new TempSteamworksEconomy.\u003CgetInventorySkinID\u003Ec__AnonStoreyA()
      {
        item = item
      }.\u003C\u003Em__7));
      if (unturnedEconInfo == null)
        return (ushort) 0;
      return (ushort) unturnedEconInfo.item_skin;
    }

    public void exchangeInventory(int generate, params ulong[] destroy)
    {
      SteamItemDef_t[] pArrayGenerate = new SteamItemDef_t[1];
      uint[] punArrayGenerateQuantity = new uint[1];
      pArrayGenerate[0] = (SteamItemDef_t) generate;
      punArrayGenerateQuantity[0] = 1U;
      SteamItemInstanceID_t[] pArrayDestroy = new SteamItemInstanceID_t[destroy.Length];
      uint[] punArrayDestroyQuantity = new uint[destroy.Length];
      for (int index = 0; index < destroy.Length; ++index)
      {
        pArrayDestroy[index] = (SteamItemInstanceID_t) destroy[index];
        punArrayDestroyQuantity[index] = 1U;
      }
      SteamInventory.ExchangeItems(out this.exchangeResult, pArrayGenerate, punArrayGenerateQuantity, (uint) pArrayGenerate.Length, pArrayDestroy, punArrayDestroyQuantity, (uint) pArrayDestroy.Length);
    }

    public void updateInventory()
    {
      SteamInventory.SendItemDropHeartbeat();
    }

    public void dropInventory()
    {
      SteamInventory.TriggerItemDrop(out this.dropResult, (SteamItemDef_t) 10000);
    }

    public void refreshInventory()
    {
      if (SteamInventory.GetAllItems(out this.inventoryResult))
        return;
      Provider.isLoadingInventory = false;
    }

    private void onInventoryResultReady(SteamInventoryResultReady_t callback)
    {
      if (this.appInfo.isDedicated)
      {
        SteamPending steamPending = (SteamPending) null;
        for (int index = 0; index < Provider.pending.Count; ++index)
        {
          if (Provider.pending[index].inventoryResult == callback.m_handle)
          {
            steamPending = Provider.pending[index];
            break;
          }
        }
        if (steamPending == null)
          return;
        if (callback.m_result != EResult.k_EResultOK || !SteamGameServerInventory.CheckResultSteamID(callback.m_handle, steamPending.playerID.steamID))
        {
          Debug.Log((object) string.Concat(new object[4]
          {
            (object) "inventory auth: ",
            (object) callback.m_result,
            (object) " ",
            (object) (bool) (SteamGameServerInventory.CheckResultSteamID(callback.m_handle, steamPending.playerID.steamID) ? 1 : 0)
          }));
          Provider.reject(steamPending.playerID.steamID, ESteamRejection.AUTH_ECON);
        }
        else
        {
          uint punOutItemsArraySize = 0U;
          if (SteamGameServerInventory.GetResultItems(callback.m_handle, (SteamItemDetails_t[]) null, ref punOutItemsArraySize) && punOutItemsArraySize > 0U)
          {
            steamPending.inventoryDetails = new SteamItemDetails_t[(IntPtr) punOutItemsArraySize];
            SteamGameServerInventory.GetResultItems(callback.m_handle, steamPending.inventoryDetails, ref punOutItemsArraySize);
          }
          steamPending.shirtItem = steamPending.getInventoryItem(steamPending.packageShirt);
          steamPending.pantsItem = steamPending.getInventoryItem(steamPending.packagePants);
          steamPending.hatItem = steamPending.getInventoryItem(steamPending.packageHat);
          steamPending.backpackItem = steamPending.getInventoryItem(steamPending.packageBackpack);
          steamPending.vestItem = steamPending.getInventoryItem(steamPending.packageVest);
          steamPending.maskItem = steamPending.getInventoryItem(steamPending.packageMask);
          steamPending.glassesItem = steamPending.getInventoryItem(steamPending.packageGlasses);
          List<int> list = new List<int>();
          for (int index = 0; index < steamPending.packageSkins.Length; ++index)
          {
            ulong package = steamPending.packageSkins[index];
            if ((long) package != 0L)
            {
              int inventoryItem = steamPending.getInventoryItem(package);
              if (inventoryItem != 0)
                list.Add(inventoryItem);
            }
          }
          steamPending.skinItems = list.ToArray();
          steamPending.hasProof = true;
          if (!steamPending.hasAuthentication)
            return;
          Provider.accept(steamPending.playerID, steamPending.assignedPro, steamPending.assignedAdmin, steamPending.face, steamPending.hair, steamPending.beard, steamPending.skin, steamPending.color, steamPending.hand, steamPending.shirtItem, steamPending.pantsItem, steamPending.hatItem, steamPending.backpackItem, steamPending.vestItem, steamPending.maskItem, steamPending.glassesItem, steamPending.skinItems, steamPending.speciality);
        }
      }
      else if (this.promoResult != SteamInventoryResult_t.Invalid && callback.m_handle == this.promoResult)
      {
        SteamInventory.DestroyResult(this.promoResult);
        this.promoResult = SteamInventoryResult_t.Invalid;
      }
      else if (this.exchangeResult != SteamInventoryResult_t.Invalid && callback.m_handle == this.exchangeResult)
      {
        SteamItemDetails_t[] pOutItemsArray = (SteamItemDetails_t[]) null;
        uint punOutItemsArraySize = 0U;
        if (SteamInventory.GetResultItems(this.exchangeResult, (SteamItemDetails_t[]) null, ref punOutItemsArraySize) && punOutItemsArraySize > 0U)
        {
          pOutItemsArray = new SteamItemDetails_t[(IntPtr) punOutItemsArraySize];
          SteamInventory.GetResultItems(this.exchangeResult, pOutItemsArray, ref punOutItemsArraySize);
        }
        if (pOutItemsArray != null && punOutItemsArraySize > 0U)
        {
          if (this.onInventoryExchanged != null)
            this.onInventoryExchanged(pOutItemsArray[(IntPtr) (punOutItemsArraySize - 1U)].m_iDefinition.m_SteamItemDef, pOutItemsArray[(IntPtr) (punOutItemsArraySize - 1U)].m_unQuantity, pOutItemsArray[(IntPtr) (punOutItemsArraySize - 1U)].m_itemId.m_SteamItemInstanceID);
          this.refreshInventory();
        }
        SteamInventory.DestroyResult(this.exchangeResult);
        this.exchangeResult = SteamInventoryResult_t.Invalid;
      }
      else if (this.dropResult != SteamInventoryResult_t.Invalid && callback.m_handle == this.dropResult)
      {
        SteamItemDetails_t[] pOutItemsArray = (SteamItemDetails_t[]) null;
        uint punOutItemsArraySize = 0U;
        if (SteamInventory.GetResultItems(this.dropResult, (SteamItemDetails_t[]) null, ref punOutItemsArraySize) && punOutItemsArraySize > 0U)
        {
          pOutItemsArray = new SteamItemDetails_t[(IntPtr) punOutItemsArraySize];
          SteamInventory.GetResultItems(this.dropResult, pOutItemsArray, ref punOutItemsArraySize);
        }
        if (pOutItemsArray != null && punOutItemsArraySize > 0U)
        {
          if (this.onInventoryDropped != null)
            this.onInventoryDropped(pOutItemsArray[0].m_iDefinition.m_SteamItemDef, pOutItemsArray[0].m_unQuantity, pOutItemsArray[0].m_itemId.m_SteamItemInstanceID);
          this.refreshInventory();
        }
        SteamInventory.DestroyResult(this.dropResult);
        this.dropResult = SteamInventoryResult_t.Invalid;
      }
      else
      {
        if (!(this.inventoryResult != SteamInventoryResult_t.Invalid) || !(callback.m_handle == this.inventoryResult))
          return;
        uint punOutItemsArraySize = 0U;
        if (SteamInventory.GetResultItems(this.inventoryResult, (SteamItemDetails_t[]) null, ref punOutItemsArraySize) && punOutItemsArraySize > 0U)
        {
          this.inventoryDetails = new SteamItemDetails_t[(IntPtr) punOutItemsArraySize];
          SteamInventory.GetResultItems(this.inventoryResult, this.inventoryDetails, ref punOutItemsArraySize);
        }
        if (this.onInventoryRefreshed != null)
          this.onInventoryRefreshed();
        Provider.isLoadingInventory = false;
        SteamInventory.DestroyResult(this.inventoryResult);
        this.inventoryResult = SteamInventoryResult_t.Invalid;
      }
    }

    private void onGameOverlayActivated(GameOverlayActivated_t callback)
    {
      if ((int) callback.m_bActive != 0)
        return;
      this.refreshInventory();
    }

    public delegate void InventoryRefreshed();

    public delegate void InventoryDropped(int item, ushort quantity, ulong instance);

    public delegate void InventoryExchanged(int item, ushort quantity, ulong instance);
  }
}
