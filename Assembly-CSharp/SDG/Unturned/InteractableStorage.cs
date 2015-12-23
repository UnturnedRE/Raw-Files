// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.InteractableStorage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System;
using UnityEngine;

namespace SDG.Unturned
{
  public class InteractableStorage : Interactable
  {
    private CSteamID _owner;
    private CSteamID _group;
    private Items _items;
    public bool isOpen;
    private bool isLocked;

    public CSteamID owner
    {
      get
      {
        return this._owner;
      }
    }

    public CSteamID group
    {
      get
      {
        return this._group;
      }
    }

    public Items items
    {
      get
      {
        return this._items;
      }
    }

    private void onStateUpdated()
    {
      SteamPacker.openWrite(0);
      SteamPacker.write((object) this.owner, (object) this.group, (object) this.items.getItemCount());
      for (byte index = (byte) 0; (int) index < (int) this.items.getItemCount(); ++index)
      {
        ItemJar itemJar = this.items.getItem(index);
        SteamPacker.write((object) itemJar.x, (object) itemJar.y, (object) itemJar.item.id, (object) itemJar.item.amount, (object) itemJar.item.quality, (object) itemJar.item.state);
      }
      int size;
      BarricadeManager.updateState(this.transform, SteamPacker.closeWrite(out size), size);
    }

    public bool checkStore(CSteamID enemyPlayer, CSteamID enemyGroup)
    {
      if (Provider.isServer && !Dedicator.isDedicated)
        return true;
      if (!this.isLocked || enemyPlayer == this.owner || this.group != CSteamID.Nil && enemyGroup == this.group)
        return !this.isOpen;
      return false;
    }

    public override void updateState(Asset asset, byte[] state)
    {
      this.isLocked = ((ItemBarricadeAsset) asset).isLocked;
      if (Provider.isServer)
      {
        SteamPacker.openRead(0, state);
        this._owner = (CSteamID) SteamPacker.read(Types.STEAM_ID_TYPE);
        this._group = (CSteamID) SteamPacker.read(Types.STEAM_ID_TYPE);
        this._items = new Items(PlayerInventory.STORAGE);
        this.items.resize(((ItemStorageAsset) asset).storage_x, ((ItemStorageAsset) asset).storage_y);
        byte num = (byte) SteamPacker.read(Types.BYTE_TYPE);
        for (byte index = (byte) 0; (int) index < (int) num; ++index)
        {
          object[] objArray = SteamPacker.read(Types.BYTE_TYPE, Types.BYTE_TYPE, Types.UINT16_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_ARRAY_TYPE);
          if ((ItemAsset) Assets.find(EAssetType.ITEM, (ushort) objArray[2]) != null)
            this.items.loadItem((byte) objArray[0], (byte) objArray[1], new Item((ushort) objArray[2], (byte) objArray[3], (byte) objArray[4], (byte[]) objArray[5]));
        }
        this.items.onStateUpdated = new StateUpdated(this.onStateUpdated);
        SteamPacker.closeRead();
      }
      else
      {
        this._owner = new CSteamID(BitConverter.ToUInt64(state, 0));
        this._group = new CSteamID(BitConverter.ToUInt64(state, 8));
      }
    }

    public override bool checkUseable()
    {
      if (this.checkStore(Provider.client, Characters.active.group))
        return !PlayerUI.window.showCursor;
      return false;
    }

    public override void use()
    {
      BarricadeManager.storeStorage(this.transform);
    }

    public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
    {
      text = string.Empty;
      color = Color.white;
      if (PlayerUI.window.showCursor)
      {
        message = EPlayerMessage.LOCKED;
        return false;
      }
      message = !this.checkUseable() ? EPlayerMessage.LOCKED : EPlayerMessage.STORAGE;
      return true;
    }

    private void OnDestroy()
    {
      if (!Provider.isServer)
        return;
      for (byte index = (byte) 0; (int) index < (int) this.items.getItemCount(); ++index)
        ItemManager.dropItem(this.items.getItem(index).item, this.transform.position, false, true, true);
      this.items.clear();
    }
  }
}
