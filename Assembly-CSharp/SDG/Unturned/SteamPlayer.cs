// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SteamPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class SteamPlayer
  {
    private SteamPlayerID _playerID;
    private Transform _model;
    private Player _player;
    private bool _isPro;
    private int _channel;
    public bool isAdmin;
    private float[] pings;
    private float _ping;
    private float _joined;
    private byte _face;
    private byte _hair;
    private byte _beard;
    private Color _skin;
    private Color _color;
    private bool _hand;
    public int shirtItem;
    public int pantsItem;
    public int hatItem;
    public int backpackItem;
    public int vestItem;
    public int maskItem;
    public int glassesItem;
    public int[] skinItems;
    public Dictionary<ushort, int> skins;
    public SteamItemDetails_t[] inventoryDetails;
    private EPlayerSpeciality _speciality;
    public float lastNet;
    public float lastPing;
    public float lastChat;

    public SteamPlayerID playerID
    {
      get
      {
        return this._playerID;
      }
    }

    public Transform model
    {
      get
      {
        return this._model;
      }
    }

    public Player player
    {
      get
      {
        return this._player;
      }
    }

    public bool isPro
    {
      get
      {
        return this._isPro;
      }
    }

    public int channel
    {
      get
      {
        return this._channel;
      }
    }

    public float ping
    {
      get
      {
        return this._ping;
      }
    }

    public float joined
    {
      get
      {
        return this._joined;
      }
    }

    public byte face
    {
      get
      {
        return this._face;
      }
    }

    public byte hair
    {
      get
      {
        return this._hair;
      }
    }

    public byte beard
    {
      get
      {
        return this._beard;
      }
    }

    public Color skin
    {
      get
      {
        return this._skin;
      }
    }

    public Color color
    {
      get
      {
        return this._color;
      }
    }

    public bool hand
    {
      get
      {
        return this._hand;
      }
    }

    public EPlayerSpeciality speciality
    {
      get
      {
        return this._speciality;
      }
    }

    public SteamPlayer(SteamPlayerID newPlayerID, Transform newModel, bool newPro, bool newAdmin, int newChannel, byte newFace, byte newHair, byte newBeard, Color newSkin, Color newColor, bool newHand, int newShirtItem, int newPantsItem, int newHatItem, int newBackpackItem, int newVestItem, int newMaskItem, int newGlassesItem, int[] newSkinItems, EPlayerSpeciality newSpeciality)
    {
      this._playerID = newPlayerID;
      this._model = newModel;
      this.model.name = this.playerID.characterName + " [" + this.playerID.playerName + "]";
      this.model.parent = LevelPlayers.models;
      this.model.GetComponent<SteamChannel>().id = newChannel;
      this.model.GetComponent<SteamChannel>().owner = this;
      this.model.GetComponent<SteamChannel>().isOwner = this.playerID.steamID == Provider.client;
      this.model.GetComponent<SteamChannel>().setup();
      this._player = this.model.GetComponent<Player>();
      this._isPro = newPro;
      this._channel = newChannel;
      this.isAdmin = newAdmin;
      this._face = newFace;
      this._hair = newHair;
      this._beard = newBeard;
      this._skin = newSkin;
      this._color = newColor;
      this._hand = newHand;
      this._speciality = newSpeciality;
      this.shirtItem = newShirtItem;
      this.pantsItem = newPantsItem;
      this.hatItem = newHatItem;
      this.backpackItem = newBackpackItem;
      this.vestItem = newVestItem;
      this.maskItem = newMaskItem;
      this.glassesItem = newGlassesItem;
      this.skinItems = newSkinItems;
      if (!Dedicator.isDedicated)
      {
        this.skins = new Dictionary<ushort, int>();
        for (int index = 0; index < this.skinItems.Length; ++index)
        {
          int num = this.skinItems[index];
          if (num != 0)
          {
            ushort inventoryItemId = Provider.provider.economyService.getInventoryItemID(num);
            if ((int) inventoryItemId != 0 && !this.skins.ContainsKey(inventoryItemId))
              this.skins.Add(inventoryItemId, num);
          }
        }
      }
      this.pings = new float[4];
      this.lastNet = Time.realtimeSinceStartup;
      this.lastChat = Time.realtimeSinceStartup;
      this._joined = Time.realtimeSinceStartup;
    }

    public void lag(float value)
    {
      this._ping = value;
      for (int index = this.pings.Length - 1; index > 0; --index)
      {
        this.pings[index] = this.pings[index - 1];
        if ((double) this.pings[index] > 1.0 / 1000.0)
          this._ping += this.pings[index];
      }
      this._ping /= (float) this.pings.Length;
      this.pings[0] = value;
    }
  }
}
