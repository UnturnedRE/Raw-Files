// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.UseableStructure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class UseableStructure : Useable
  {
    private Transform help;
    private Transform arrow;
    private float startedUse;
    private float useTime;
    private bool isConstructing;
    private bool isUsing;
    private bool isValid;
    private RaycastHit hit;
    private Vector3 point;
    private float angle;
    private float offset;
    private byte rotate;

    private bool isUseable
    {
      get
      {
        return (double) Time.realtimeSinceStartup - (double) this.startedUse > (double) this.useTime;
      }
    }

    private bool isConstructable
    {
      get
      {
        return (double) Time.realtimeSinceStartup - (double) this.startedUse > (double) this.useTime * 0.800000011920929;
      }
    }

    [SteamCall]
    public void askStructure(CSteamID steamID, Vector3 newPoint, float newAngle)
    {
      if (!this.channel.checkOwner(steamID))
        return;
      this.point = newPoint;
      this.angle = newAngle;
      this.isValid = true;
    }

    private bool check()
    {
      this.angle = this.player.look.yaw;
      if (((ItemStructureAsset) this.player.equipment.asset).construct == EConstruct.FLOOR)
      {
        Physics.Raycast(this.player.look.aim.position, this.player.look.aim.forward, out this.hit, ((ItemStructureAsset) this.player.equipment.asset).range, RayMasks.STRUCTURE_INTERACT);
        if ((Object) this.hit.transform != (Object) null)
        {
          if (this.hit.transform.tag == "Structure")
          {
            ItemStructureAsset itemStructureAsset = (ItemStructureAsset) Assets.find(EAssetType.ITEM, ushort.Parse(this.hit.transform.name));
            if (itemStructureAsset == null || itemStructureAsset.construct != EConstruct.FLOOR)
            {
              this.point = this.hit.point;
              if (this.channel.isOwner)
                PlayerUI.hint((Transform) null, EPlayerMessage.ROOF);
              return false;
            }
            this.angle = this.hit.transform.rotation.eulerAngles.y;
            if ((double) (this.hit.transform.position - this.hit.transform.right * StructureManager.WALL - this.hit.point).sqrMagnitude < 4.0)
              this.point = this.hit.transform.position - this.hit.transform.right * StructureManager.WALL * 2f;
            else if ((double) (this.hit.transform.position + this.hit.transform.right * StructureManager.WALL - this.hit.point).sqrMagnitude < 4.0)
              this.point = this.hit.transform.position + this.hit.transform.right * StructureManager.WALL * 2f;
            else if ((double) (this.hit.transform.position - this.hit.transform.up * StructureManager.WALL - this.hit.point).sqrMagnitude < 4.0)
              this.point = this.hit.transform.position - this.hit.transform.up * StructureManager.WALL * 2f;
            else if ((double) (this.hit.transform.position + this.hit.transform.up * StructureManager.WALL - this.hit.point).sqrMagnitude < 4.0)
            {
              this.point = this.hit.transform.position + this.hit.transform.up * StructureManager.WALL * 2f;
            }
            else
            {
              this.point = this.hit.point;
              if (this.channel.isOwner)
                PlayerUI.hint((Transform) null, EPlayerMessage.BLOCKED);
              return false;
            }
            if (Physics.OverlapSphere(this.point, 1f, RayMasks.BLOCK_STRUCTURE).Length != 0)
            {
              if (this.channel.isOwner)
                PlayerUI.hint((Transform) null, EPlayerMessage.BLOCKED);
              return false;
            }
          }
          else
          {
            this.point = this.hit.point;
            if (this.hit.transform.tag == "Ground" && (double) this.hit.normal.y > 0.5)
            {
              if (!Level.checkSafe(this.point))
              {
                PlayerUI.hint((Transform) null, EPlayerMessage.BOUNDS);
                return false;
              }
              if (Physics.OverlapSphere(this.point, 1f, RayMasks.BLOCK_STRUCTURE).Length != 0)
              {
                if (this.channel.isOwner)
                  PlayerUI.hint((Transform) null, EPlayerMessage.BLOCKED);
                return false;
              }
            }
            else
            {
              if (this.channel.isOwner)
                PlayerUI.hint((Transform) null, EPlayerMessage.GROUND);
              return false;
            }
          }
          return true;
        }
        this.point = Vector3.zero;
        return false;
      }
      if (((ItemStructureAsset) this.player.equipment.asset).construct == EConstruct.WALL || ((ItemStructureAsset) this.player.equipment.asset).construct == EConstruct.RAMPART)
      {
        Physics.Raycast(this.player.look.aim.position, this.player.look.aim.forward, out this.hit, ((ItemStructureAsset) this.player.equipment.asset).range, RayMasks.WALLS_INTERACT);
        if ((Object) this.hit.transform != (Object) null)
        {
          if (this.hit.transform.tag == "Logic" && this.hit.transform.name == "Wall")
          {
            this.point = this.hit.transform.position;
            this.angle = this.hit.transform.rotation.eulerAngles.y;
            if (((ItemStructureAsset) this.player.equipment.asset).construct == EConstruct.RAMPART)
              this.point += Vector3.down * 1.225f;
            if (Physics.OverlapSphere(this.hit.transform.position, 0.25f, RayMasks.BLOCK_STRUCTURE).Length != 0)
            {
              if (this.channel.isOwner)
                PlayerUI.hint((Transform) null, EPlayerMessage.BLOCKED);
              return false;
            }
            if (Physics.OverlapSphere(this.hit.transform.position + Vector3.up * 1.5f, 0.25f, RayMasks.BLOCK_STRUCTURE).Length != 0)
            {
              if (this.channel.isOwner)
                PlayerUI.hint((Transform) null, EPlayerMessage.BLOCKED);
              return false;
            }
            if (Physics.OverlapSphere(this.hit.transform.position - Vector3.up * 1.5f, 0.25f, RayMasks.BLOCK_STRUCTURE).Length != 0)
            {
              if (this.channel.isOwner)
                PlayerUI.hint((Transform) null, EPlayerMessage.BLOCKED);
              return false;
            }
            if (Physics.OverlapSphere(this.point - this.hit.transform.up * StructureManager.WALL, 0.01f, RayMasks.BLOCK_STRUCTURE).Length == 0)
            {
              if (this.channel.isOwner)
                PlayerUI.hint((Transform) null, ((ItemStructureAsset) this.player.equipment.asset).construct != EConstruct.RAMPART ? EPlayerMessage.PILLAR : EPlayerMessage.POST);
              return false;
            }
            if (Physics.OverlapSphere(this.point + this.hit.transform.up * StructureManager.WALL, 0.01f, RayMasks.BLOCK_STRUCTURE).Length != 0)
              return true;
            if (this.channel.isOwner)
              PlayerUI.hint((Transform) null, ((ItemStructureAsset) this.player.equipment.asset).construct != EConstruct.RAMPART ? EPlayerMessage.PILLAR : EPlayerMessage.POST);
            return false;
          }
          this.point = Vector3.zero;
          if (this.channel.isOwner)
            PlayerUI.hint((Transform) null, EPlayerMessage.WALL);
          return false;
        }
        this.point = Vector3.zero;
        if (this.channel.isOwner)
          PlayerUI.hint((Transform) null, EPlayerMessage.WALL);
        return false;
      }
      if (((ItemStructureAsset) this.player.equipment.asset).construct == EConstruct.ROOF)
      {
        Physics.Raycast(this.player.look.aim.position, this.player.look.aim.forward, out this.hit, ((ItemStructureAsset) this.player.equipment.asset).range, RayMasks.ROOFS_INTERACT);
        if ((Object) this.hit.transform != (Object) null)
        {
          if (this.hit.transform.tag == "Logic" && this.hit.transform.name == "Roof")
          {
            this.point = this.hit.transform.position;
            this.angle = this.hit.transform.rotation.eulerAngles.y;
            if (Physics.OverlapSphere(this.hit.transform.position - this.hit.transform.right * StructureManager.WALL - this.hit.transform.up * StructureManager.WALL - this.hit.transform.forward * StructureManager.HEIGHT, 0.01f, RayMasks.BLOCK_STRUCTURE).Length == 0)
            {
              if (this.channel.isOwner)
                PlayerUI.hint((Transform) null, EPlayerMessage.PILLAR);
              return false;
            }
            if (Physics.OverlapSphere(this.hit.transform.position - this.hit.transform.right * StructureManager.WALL + this.hit.transform.up * StructureManager.WALL - this.hit.transform.forward * StructureManager.HEIGHT, 0.01f, RayMasks.BLOCK_STRUCTURE).Length == 0)
            {
              if (this.channel.isOwner)
                PlayerUI.hint((Transform) null, EPlayerMessage.PILLAR);
              return false;
            }
            if (Physics.OverlapSphere(this.hit.transform.position + this.hit.transform.right * StructureManager.WALL - this.hit.transform.up * StructureManager.WALL - this.hit.transform.forward * StructureManager.HEIGHT, 0.01f, RayMasks.BLOCK_STRUCTURE).Length == 0)
            {
              if (this.channel.isOwner)
                PlayerUI.hint((Transform) null, EPlayerMessage.PILLAR);
              return false;
            }
            if (Physics.OverlapSphere(this.hit.transform.position + this.hit.transform.right * StructureManager.WALL + this.hit.transform.up * StructureManager.WALL - this.hit.transform.forward * StructureManager.HEIGHT, 0.01f, RayMasks.BLOCK_STRUCTURE).Length == 0)
            {
              if (this.channel.isOwner)
                PlayerUI.hint((Transform) null, EPlayerMessage.PILLAR);
              return false;
            }
            if (Physics.OverlapSphere(this.point + this.hit.transform.right * StructureManager.WALL * 0.5f, 0.25f, RayMasks.BLOCK_STRUCTURE).Length != 0)
            {
              if (this.channel.isOwner)
                PlayerUI.hint((Transform) null, EPlayerMessage.BLOCKED);
              return false;
            }
            if (Physics.OverlapSphere(this.point + Vector3.down * 2f, 0.25f, RayMasks.BLOCK_STRUCTURE).Length != 0)
            {
              if (this.channel.isOwner)
                PlayerUI.hint((Transform) null, EPlayerMessage.BLOCKED);
              return false;
            }
          }
          else if (this.hit.transform.tag == "Structure")
          {
            ItemStructureAsset itemStructureAsset = (ItemStructureAsset) Assets.find(EAssetType.ITEM, ushort.Parse(this.hit.transform.name));
            if (itemStructureAsset == null || itemStructureAsset.construct != EConstruct.FLOOR && itemStructureAsset.construct != EConstruct.ROOF)
            {
              this.point = Vector3.zero;
              if (this.channel.isOwner)
                PlayerUI.hint((Transform) null, EPlayerMessage.ROOF);
              return false;
            }
            this.angle = this.hit.transform.rotation.eulerAngles.y;
            if ((double) (this.hit.transform.position - this.hit.transform.right * StructureManager.WALL - this.hit.point).sqrMagnitude < 4.0)
              this.point = this.hit.transform.position - this.hit.transform.right * StructureManager.WALL * 2f;
            else if ((double) (this.hit.transform.position + this.hit.transform.right * StructureManager.WALL - this.hit.point).sqrMagnitude < 4.0)
              this.point = this.hit.transform.position + this.hit.transform.right * StructureManager.WALL * 2f;
            else if ((double) (this.hit.transform.position - this.hit.transform.up * StructureManager.WALL - this.hit.point).sqrMagnitude < 4.0)
              this.point = this.hit.transform.position - this.hit.transform.up * StructureManager.WALL * 2f;
            else if ((double) (this.hit.transform.position + this.hit.transform.up * StructureManager.WALL - this.hit.point).sqrMagnitude < 4.0)
            {
              this.point = this.hit.transform.position + this.hit.transform.up * StructureManager.WALL * 2f;
            }
            else
            {
              this.point = Vector3.zero;
              if (this.channel.isOwner)
                PlayerUI.hint((Transform) null, EPlayerMessage.ROOF);
            }
            if (Physics.OverlapSphere(this.point + this.hit.transform.right * StructureManager.WALL * 0.5f, 0.25f, RayMasks.BLOCK_STRUCTURE).Length != 0)
            {
              if (this.channel.isOwner)
                PlayerUI.hint((Transform) null, EPlayerMessage.BLOCKED);
              return false;
            }
            if (Physics.OverlapSphere(this.point + Vector3.down * 2f, 0.25f, RayMasks.BLOCK_STRUCTURE).Length != 0)
            {
              if (this.channel.isOwner)
                PlayerUI.hint((Transform) null, EPlayerMessage.BLOCKED);
              return false;
            }
          }
          else
          {
            this.point = Vector3.zero;
            if (this.channel.isOwner)
              PlayerUI.hint((Transform) null, EPlayerMessage.ROOF);
            return false;
          }
          return true;
        }
        this.point = Vector3.zero;
        return false;
      }
      if (((ItemStructureAsset) this.player.equipment.asset).construct == EConstruct.PILLAR || ((ItemStructureAsset) this.player.equipment.asset).construct == EConstruct.POST)
      {
        Physics.Raycast(this.player.look.aim.position, this.player.look.aim.forward, out this.hit, ((ItemStructureAsset) this.player.equipment.asset).range, RayMasks.CORNERS_INTERACT);
        if ((Object) this.hit.transform != (Object) null)
        {
          if (this.hit.transform.tag == "Logic" && this.hit.transform.name == "Pillar")
          {
            this.point = this.hit.transform.position;
            this.angle = this.hit.transform.rotation.eulerAngles.y;
            if (((ItemStructureAsset) this.player.equipment.asset).construct == EConstruct.POST)
              this.point += Vector3.down * 1.225f;
            if (Physics.OverlapSphere(this.point, 0.01f, RayMasks.BLOCK_STRUCTURE).Length == 0)
              return true;
            if (this.channel.isOwner)
              PlayerUI.hint((Transform) null, EPlayerMessage.BLOCKED);
            return false;
          }
          this.point = Vector3.zero;
          if (this.channel.isOwner)
            PlayerUI.hint((Transform) null, EPlayerMessage.CORNER);
          return false;
        }
        this.point = Vector3.zero;
        return false;
      }
      this.point = Vector3.zero;
      return false;
    }

    private void construct()
    {
      this.startedUse = Time.realtimeSinceStartup;
      this.isUsing = true;
      this.isConstructing = true;
      this.player.animator.play("Use", false);
    }

    [SteamCall]
    public void askConstruct(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID) || !this.player.equipment.isEquipped)
        return;
      this.construct();
    }

    public override void startPrimary()
    {
      if (this.player.equipment.isBusy || (!Dedicator.isDedicated || !this.isValid) && !this.check())
        return;
      if (this.channel.isOwner)
        this.channel.send("askStructure", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[2]
        {
          (object) this.point,
          (object) this.angle
        });
      if ((double) (this.point - this.player.look.aim.position).sqrMagnitude >= 256.0)
        return;
      this.player.equipment.isBusy = true;
      this.construct();
      if (!Provider.isServer)
        return;
      this.channel.send("askConstruct", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
    }

    public override void startSecondary()
    {
      if (this.player.equipment.isBusy || ((ItemStructureAsset) this.player.equipment.asset).construct != EConstruct.FLOOR && ((ItemStructureAsset) this.player.equipment.asset).construct != EConstruct.ROOF)
        return;
      ++this.rotate;
    }

    public override void equip()
    {
      this.player.animator.play("Equip", true);
      this.useTime = this.player.animator.getAnimationLength("Use");
      if (!this.channel.isOwner)
        return;
      this.help = StructureTool.getStructure(this.player.equipment.itemID, false);
      this.help.position = Vector3.zero;
      this.help.rotation = Quaternion.Euler(-90f, 0.0f, 0.0f);
      Object.Destroy((Object) this.help.GetComponent<Collider>());
      HighlighterTool.help(this.help, this.isValid);
      if (((ItemStructureAsset) this.player.equipment.asset).construct == EConstruct.WALL || ((ItemStructureAsset) this.player.equipment.asset).construct == EConstruct.RAMPART || (((ItemStructureAsset) this.player.equipment.asset).construct == EConstruct.FLOOR || ((ItemStructureAsset) this.player.equipment.asset).construct == EConstruct.ROOF))
      {
        this.arrow = ((GameObject) Object.Instantiate(Resources.Load("Build/Arrow"))).transform;
        this.arrow.name = "Arrow";
        this.arrow.parent = this.help;
        this.arrow.localPosition = Vector3.zero;
        this.arrow.localRotation = Quaternion.Euler(90f, 0.0f, 0.0f);
        HighlighterTool.help(this.arrow, this.isValid);
      }
      if ((Object) this.help.FindChild("Clip") != (Object) null)
        Object.Destroy((Object) this.help.FindChild("Clip").gameObject);
      if ((Object) this.help.FindChild("Nav") != (Object) null)
        Object.Destroy((Object) this.help.FindChild("Nav").gameObject);
      if ((Object) this.help.FindChild("Cutter") != (Object) null)
        Object.Destroy((Object) this.help.FindChild("Cutter").gameObject);
      if ((Object) this.help.FindChild("Roof") != (Object) null)
        Object.Destroy((Object) this.help.FindChild("Roof").gameObject);
      if ((Object) this.help.FindChild("Block") != (Object) null)
        Object.Destroy((Object) this.help.FindChild("Block").gameObject);
      for (int index = 0; index < 4 && (Object) this.help.FindChild("Wall") != (Object) null; ++index)
        Object.Destroy((Object) this.help.FindChild("Wall").gameObject);
      for (int index = 0; index < 4 && (Object) this.help.FindChild("Pillar") != (Object) null; ++index)
        Object.Destroy((Object) this.help.FindChild("Pillar").gameObject);
    }

    public override void dequip()
    {
      if (!this.channel.isOwner || !((Object) this.help != (Object) null))
        return;
      Object.Destroy((Object) this.help.gameObject);
    }

    public override void simulate(uint simulation)
    {
      if (!this.isUsing || !this.isUseable)
        return;
      this.player.equipment.isBusy = false;
      if (!Provider.isServer)
        return;
      if ((ItemStructureAsset) this.player.equipment.asset != null)
        StructureManager.dropStructure(new Structure(this.player.equipment.itemID), this.point, this.angle + (float) ((int) this.rotate * 90), this.channel.owner.playerID.steamID.m_SteamID, this.channel.owner.playerID.group.m_SteamID);
      this.player.equipment.use();
    }

    public override void tick()
    {
      if (this.isConstructing && this.isConstructable)
      {
        this.isConstructing = false;
        if (!Dedicator.isDedicated)
          this.player.playSound(((ItemStructureAsset) this.player.equipment.asset).use);
      }
      if (!this.channel.isOwner || (Object) this.help == (Object) null || this.isUsing)
        return;
      if (this.check())
      {
        if (!this.isValid)
        {
          this.isValid = true;
          HighlighterTool.help(this.help, this.isValid);
          if ((Object) this.arrow != (Object) null)
            HighlighterTool.help(this.arrow, this.isValid);
        }
      }
      else if (this.isValid)
      {
        this.isValid = false;
        HighlighterTool.help(this.help, this.isValid);
        if ((Object) this.arrow != (Object) null)
          HighlighterTool.help(this.arrow, this.isValid);
      }
      this.offset = Mathf.Lerp(this.offset, (float) ((int) this.rotate * 90), 8f * Time.deltaTime);
      this.help.position = this.point;
      this.help.rotation = Quaternion.Euler(-90f, this.angle + this.offset, 0.0f);
    }
  }
}
