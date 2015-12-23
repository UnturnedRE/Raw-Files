// Decompiled with JetBrains decompiler
// Type: SDG.SteamworksProvider.Services.Economy.SteamworksEconomyService
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services;
using SDG.Provider.Services.Economy;
using Steamworks;
using System;
using System.Collections.Generic;

namespace SDG.SteamworksProvider.Services.Economy
{
  public class SteamworksEconomyService : Service, IEconomyService, IService
  {
    private List<SteamworksEconomyRequestHandle> steamworksEconomyRequestHandles;
    private Callback<SteamInventoryResultReady_t> steamInventoryResultReady;

    public bool canOpenInventory
    {
      get
      {
        return SteamUtils.IsOverlayEnabled();
      }
    }

    public SteamworksEconomyService()
    {
      this.steamworksEconomyRequestHandles = new List<SteamworksEconomyRequestHandle>();
      this.steamInventoryResultReady = Callback<SteamInventoryResultReady_t>.Create(new Callback<SteamInventoryResultReady_t>.DispatchDelegate(this.onSteamInventoryResultReady));
    }

    public IEconomyRequestHandle requestInventory(EconomyRequestReadyCallback inventoryRequestReadyCallback)
    {
      SteamInventoryResult_t pResultHandle;
      SteamInventory.GetAllItems(out pResultHandle);
      return this.addInventoryRequestHandle(pResultHandle, inventoryRequestReadyCallback);
    }

    public IEconomyRequestHandle requestPromo(EconomyRequestReadyCallback inventoryRequestReadyCallback)
    {
      SteamInventoryResult_t pResultHandle;
      SteamInventory.GrantPromoItems(out pResultHandle);
      return this.addInventoryRequestHandle(pResultHandle, inventoryRequestReadyCallback);
    }

    public IEconomyRequestHandle exchangeItems(IEconomyItemInstance[] inputItemInstanceIDs, uint[] inputItemQuantities, IEconomyItemDefinition[] outputItemDefinitionIDs, uint[] outputItemQuantities, EconomyRequestReadyCallback inventoryRequestReadyCallback)
    {
      if (inputItemInstanceIDs.Length != inputItemQuantities.Length)
        throw new ArgumentException("Input item arrays need to be the same length.", "inputItemQuantities");
      if (outputItemDefinitionIDs.Length != outputItemQuantities.Length)
        throw new ArgumentException("Output item arrays need to be the same length.", "outputItemQuantities");
      SteamItemInstanceID_t[] pArrayDestroy = new SteamItemInstanceID_t[inputItemInstanceIDs.Length];
      for (int index = 0; index < inputItemInstanceIDs.Length; ++index)
      {
        SteamworksEconomyItemInstance economyItemInstance = (SteamworksEconomyItemInstance) inputItemInstanceIDs[index];
        pArrayDestroy[index] = economyItemInstance.steamItemInstanceID;
      }
      SteamItemDef_t[] pArrayGenerate = new SteamItemDef_t[outputItemDefinitionIDs.Length];
      for (int index = 0; index < outputItemDefinitionIDs.Length; ++index)
      {
        SteamworksEconomyItemDefinition economyItemDefinition = (SteamworksEconomyItemDefinition) outputItemDefinitionIDs[index];
        pArrayGenerate[index] = economyItemDefinition.steamItemDef;
      }
      SteamInventoryResult_t pResultHandle;
      SteamInventory.ExchangeItems(out pResultHandle, pArrayGenerate, outputItemQuantities, (uint) pArrayGenerate.Length, pArrayDestroy, inputItemQuantities, (uint) pArrayDestroy.Length);
      return this.addInventoryRequestHandle(pResultHandle, inventoryRequestReadyCallback);
    }

    public void open(ulong id)
    {
      SteamFriends.ActivateGameOverlayToWebPage("http://steamcommunity.com/profiles/" + (object) SteamUser.GetSteamID() + "/inventory/?sellOnLoad=1#" + (string) (object) SteamUtils.GetAppID() + "_2_" + (string) (object) id);
    }

    private SteamworksEconomyRequestHandle findSteamworksEconomyRequestHandles(SteamInventoryResult_t steamInventoryResult)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return this.steamworksEconomyRequestHandles.Find(new Predicate<SteamworksEconomyRequestHandle>(new SteamworksEconomyService.\u003CfindSteamworksEconomyRequestHandles\u003Ec__AnonStoreyB()
      {
        steamInventoryResult = steamInventoryResult
      }.\u003C\u003Em__8));
    }

    private IEconomyRequestHandle addInventoryRequestHandle(SteamInventoryResult_t steamInventoryResult, EconomyRequestReadyCallback inventoryRequestReadyCallback)
    {
      SteamworksEconomyRequestHandle economyRequestHandle = new SteamworksEconomyRequestHandle(steamInventoryResult, inventoryRequestReadyCallback);
      this.steamworksEconomyRequestHandles.Add(economyRequestHandle);
      return (IEconomyRequestHandle) economyRequestHandle;
    }

    private IEconomyRequestResult createInventoryRequestResult(SteamInventoryResult_t steamInventoryResult)
    {
      uint punOutItemsArraySize = 0U;
      SteamworksEconomyItem[] steamworksEconomyItemArray;
      if (SteamGameServerInventory.GetResultItems(steamInventoryResult, (SteamItemDetails_t[]) null, ref punOutItemsArraySize) && punOutItemsArraySize > 0U)
      {
        SteamItemDetails_t[] pOutItemsArray = new SteamItemDetails_t[(IntPtr) punOutItemsArraySize];
        SteamGameServerInventory.GetResultItems(steamInventoryResult, pOutItemsArray, ref punOutItemsArraySize);
        steamworksEconomyItemArray = new SteamworksEconomyItem[(IntPtr) punOutItemsArraySize];
        for (uint index = 0U; index < punOutItemsArraySize; ++index)
        {
          SteamworksEconomyItem steamworksEconomyItem = new SteamworksEconomyItem(pOutItemsArray[(IntPtr) index]);
          steamworksEconomyItemArray[(IntPtr) index] = steamworksEconomyItem;
        }
      }
      else
        steamworksEconomyItemArray = new SteamworksEconomyItem[0];
      return (IEconomyRequestResult) new EconomyRequestResult(EEconomyRequestState.SUCCESS, (IEconomyItem[]) steamworksEconomyItemArray);
    }

    private void onSteamInventoryResultReady(SteamInventoryResultReady_t callback)
    {
      SteamworksEconomyRequestHandle economyRequestHandles = this.findSteamworksEconomyRequestHandles(callback.m_handle);
      if (economyRequestHandles == null)
        return;
      IEconomyRequestResult inventoryRequestResult = this.createInventoryRequestResult(economyRequestHandles.steamInventoryResult);
      economyRequestHandles.triggerInventoryRequestReadyCallback(inventoryRequestResult);
      SteamInventory.DestroyResult(economyRequestHandles.steamInventoryResult);
    }
  }
}
