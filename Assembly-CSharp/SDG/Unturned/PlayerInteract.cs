// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerInteract
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class PlayerInteract : PlayerCaller
  {
    private static Transform focus;
    private static Transform target;
    private static ItemAsset purchaseAsset;
    private static Interactable _interactable;
    private static Interactable2 _interactable2;
    private static RaycastHit hit;
    private static float lastInteract;
    private static float lastKeyDown;
    private static bool isHoldingKey;

    public static Interactable interactable
    {
      get
      {
        return PlayerInteract._interactable;
      }
    }

    public static Interactable2 interactable2
    {
      get
      {
        return PlayerInteract._interactable2;
      }
    }

    private void hotkey(byte button)
    {
      VehicleManager.swapVehicle(button);
    }

    [SteamCall]
    public void askInspect(CSteamID steamID)
    {
      if (!this.channel.checkOwner(steamID) || !this.player.equipment.canInspect)
        return;
      this.channel.send("tellInspect", ESteamCall.ALL, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
    }

    [SteamCall]
    public void tellInspect(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this.player.equipment.inspect();
    }

    private void onPurchaseUpdated(PurchaseNode node)
    {
      if (node == null)
        PlayerInteract.purchaseAsset = (ItemAsset) null;
      else
        PlayerInteract.purchaseAsset = (ItemAsset) Assets.find(EAssetType.ITEM, node.id);
    }

    private void FixedUpdate()
    {
      if (!this.channel.isOwner)
        return;
      if (this.player.stance.stance != EPlayerStance.DRIVING && this.player.stance.stance != EPlayerStance.SITTING && !this.player.life.isDead)
      {
        if ((double) Time.realtimeSinceStartup - (double) PlayerInteract.lastInteract > 0.100000001490116)
        {
          PlayerInteract.lastInteract = Time.realtimeSinceStartup;
          if (this.player.look.isCam)
            Physics.Raycast(this.player.look.aim.position, this.player.look.aim.forward, out PlayerInteract.hit, 4f, RayMasks.PLAYER_INTERACT);
          else
            Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out PlayerInteract.hit, this.player.look.perspective != EPlayerPerspective.THIRD ? 4f : 6f, RayMasks.PLAYER_INTERACT);
        }
        if (!((Object) PlayerInteract.hit.transform != (Object) PlayerInteract.focus))
          return;
        if ((Object) PlayerInteract.focus != (Object) null)
        {
          if (PlayerInteract.focus.name == "Hinge")
            HighlighterTool.unhighlight(PlayerInteract.focus.parent.parent);
          else
            HighlighterTool.unhighlight(PlayerInteract.focus);
        }
        PlayerInteract.focus = (Transform) null;
        PlayerInteract.target = (Transform) null;
        PlayerInteract._interactable = (Interactable) null;
        PlayerInteract._interactable2 = (Interactable2) null;
        if (!((Object) PlayerInteract.hit.transform != (Object) null))
          return;
        PlayerInteract.focus = PlayerInteract.hit.transform;
        PlayerInteract.target = TransformRecursiveFind.FindChildRecursive(PlayerInteract.focus, "Target");
        PlayerInteract._interactable = PlayerInteract.focus.GetComponent<Interactable>();
        PlayerInteract._interactable2 = PlayerInteract.focus.GetComponent<Interactable2>();
        if (!((Object) PlayerInteract.interactable != (Object) null))
          return;
        if (PlayerInteract.interactable.checkInteractable())
        {
          if (!PlayerUI.window.isEnabled)
            return;
          if (PlayerInteract.interactable.checkUseable())
          {
            if (PlayerInteract.focus.name == "Hinge")
              HighlighterTool.highlight(PlayerInteract.focus.parent.parent, Color.green);
            else
              HighlighterTool.highlight(PlayerInteract.focus, Color.green);
          }
          else if (PlayerInteract.focus.name == "Hinge")
            HighlighterTool.highlight(PlayerInteract.focus.parent.parent, Color.red);
          else
            HighlighterTool.highlight(PlayerInteract.focus, Color.red);
        }
        else
        {
          PlayerInteract.focus = (Transform) null;
          PlayerInteract.target = (Transform) null;
          PlayerInteract._interactable = (Interactable) null;
        }
      }
      else
      {
        if ((Object) PlayerInteract.focus != (Object) null)
        {
          if (PlayerInteract.focus.name == "Hinge")
            HighlighterTool.unhighlight(PlayerInteract.focus.parent.parent);
          else
            HighlighterTool.unhighlight(PlayerInteract.focus);
        }
        PlayerInteract.focus = (Transform) null;
        PlayerInteract.target = (Transform) null;
        PlayerInteract._interactable = (Interactable) null;
        PlayerInteract._interactable2 = (Interactable2) null;
      }
    }

    private void Update()
    {
      if (!this.channel.isOwner || this.player.life.isDead)
        return;
      if ((Object) PlayerInteract.interactable != (Object) null)
      {
        EPlayerMessage message;
        string text;
        Color color;
        if (PlayerInteract.interactable.checkHint(out message, out text, out color))
        {
          if (PlayerInteract.interactable.tag == "Item")
            PlayerUI.hint(!((Object) PlayerInteract.target != (Object) null) ? PlayerInteract.focus : PlayerInteract.target, message, text, color, (object) ((InteractableItem) PlayerInteract.interactable).item, (object) ((InteractableItem) PlayerInteract.interactable).asset);
          else
            PlayerUI.hint(!((Object) PlayerInteract.target != (Object) null) ? PlayerInteract.focus : PlayerInteract.target, message, text, color);
        }
      }
      else if (PlayerInteract.purchaseAsset != null)
        PlayerUI.hint((Transform) null, EPlayerMessage.PURCHASE, string.Empty, Color.white, (object) PlayerInteract.purchaseAsset.itemName, (object) this.player.movement.purchaseNode.cost);
      else if ((Object) PlayerInteract.focus != (Object) null && PlayerInteract.focus.tag == "Enemy")
      {
        Player player = DamageTool.getPlayer(PlayerInteract.focus);
        if ((Object) player != (Object) null && (Object) player != (Object) Player.player)
        {
          Color color = Color.white;
          if (player.channel.owner.isAdmin)
            color = Palette.ADMIN;
          else if (player.channel.owner.isPro)
            color = Palette.PRO;
          PlayerUI.hint((Transform) null, EPlayerMessage.ENEMY, player.channel.owner.playerID.characterName, color);
        }
      }
      EPlayerMessage message1;
      if ((Object) PlayerInteract.interactable2 != (Object) null && PlayerInteract.interactable2.checkHint(out message1))
        PlayerUI.hint2(message1);
      if ((this.player.stance.stance == EPlayerStance.DRIVING || this.player.stance.stance == EPlayerStance.SITTING) && !Input.GetKey(KeyCode.LeftShift))
      {
        if (Input.GetKeyDown(KeyCode.F1))
          this.hotkey((byte) 0);
        if (Input.GetKeyDown(KeyCode.F2))
          this.hotkey((byte) 1);
        if (Input.GetKeyDown(KeyCode.F3))
          this.hotkey((byte) 2);
        if (Input.GetKeyDown(KeyCode.F4))
          this.hotkey((byte) 3);
        if (Input.GetKeyDown(KeyCode.F5))
          this.hotkey((byte) 4);
        if (Input.GetKeyDown(KeyCode.F6))
          this.hotkey((byte) 5);
        if (Input.GetKeyDown(KeyCode.F7))
          this.hotkey((byte) 6);
        if (Input.GetKeyDown(KeyCode.F8))
          this.hotkey((byte) 7);
        if (Input.GetKeyDown(KeyCode.F9))
          this.hotkey((byte) 8);
        if (Input.GetKeyDown(KeyCode.F10))
          this.hotkey((byte) 9);
      }
      if (Input.GetKeyDown(ControlsSettings.interact))
      {
        PlayerInteract.lastKeyDown = Time.realtimeSinceStartup;
        PlayerInteract.isHoldingKey = true;
      }
      if (!PlayerInteract.isHoldingKey)
        return;
      if (Input.GetKeyUp(ControlsSettings.interact))
      {
        PlayerInteract.isHoldingKey = false;
        if (PlayerUI.window.showCursor)
        {
          if (this.player.inventory.isStoring)
          {
            PlayerDashboardUI.close();
            PlayerLifeUI.open();
          }
          else
          {
            if (!PlayerBarricadeSignUI.active)
              return;
            PlayerBarricadeSignUI.close();
            PlayerLifeUI.open();
          }
        }
        else if (this.player.stance.stance == EPlayerStance.DRIVING || this.player.stance.stance == EPlayerStance.SITTING)
          VehicleManager.exitVehicle();
        else if ((Object) PlayerInteract.focus != (Object) null && (Object) PlayerInteract.interactable != (Object) null)
        {
          if (!PlayerInteract.interactable.checkUseable())
            return;
          PlayerInteract.interactable.use();
        }
        else if (PlayerInteract.purchaseAsset != null)
        {
          if (this.player.skills.experience < this.player.movement.purchaseNode.cost)
            return;
          this.player.skills.sendPurchase(this.player.movement.purchaseNode);
        }
        else
        {
          if (!this.player.equipment.canInspect)
            return;
          this.channel.send("askInspect", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
        }
      }
      else
      {
        if ((double) Time.realtimeSinceStartup - (double) PlayerInteract.lastKeyDown <= 1.0)
          return;
        PlayerInteract.isHoldingKey = false;
        if (!((Object) PlayerInteract.focus != (Object) null) || !((Object) PlayerInteract.interactable2 != (Object) null))
          return;
        PlayerInteract.interactable2.use();
      }
    }

    private void Start()
    {
      if (!this.channel.isOwner)
        return;
      this.player.movement.onPurchaseUpdated += new PurchaseUpdated(this.onPurchaseUpdated);
    }
  }
}
