// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.UseableBarricade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System;
using UnityEngine;

namespace SDG.Unturned
{
  public class UseableBarricade : Useable
  {
    private Transform parent;
    private Transform help;
    private Transform arrow;
    private float startedUse;
    private float useTime;
    private bool isRotating;
    private bool isBuilding;
    private bool isUsing;
    private bool isValid;
    private RaycastHit hit;
    private Vector3 point;
    private float angle_x;
    private float angle_y;
    private float angle_z;
    private float rotate_x;
    private float rotate_y;
    private float rotate_z;

    private bool isUseable
    {
      get
      {
        return (double) Time.realtimeSinceStartup - (double) this.startedUse > (double) this.useTime;
      }
    }

    private bool isBuildable
    {
      get
      {
        return (double) Time.realtimeSinceStartup - (double) this.startedUse > (double) this.useTime * 0.800000011920929;
      }
    }

    [SteamCall]
    public void askBarricadeVehicle(CSteamID steamID, Vector3 newPoint, float newAngle_X, float newAngle_Y, float newAngle_Z, ushort index)
    {
      if (!this.channel.checkOwner(steamID))
        return;
      InteractableVehicle vehicle = VehicleManager.getVehicle(index);
      if (!((UnityEngine.Object) vehicle != (UnityEngine.Object) null) || (double) (this.player.look.aim.position - vehicle.transform.position).sqrMagnitude >= 4096.0)
        return;
      this.parent = vehicle.transform;
      this.point = newPoint;
      if (((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.FREEFORM)
      {
        this.angle_x = newAngle_X;
        this.angle_z = newAngle_Z;
      }
      else
      {
        this.angle_x = 0.0f;
        this.angle_z = 0.0f;
      }
      this.angle_y = newAngle_Y;
      this.rotate_x = 0.0f;
      this.rotate_y = 0.0f;
      this.rotate_z = 0.0f;
      this.isValid = true;
    }

    [SteamCall]
    public void askBarricadeNone(CSteamID steamID, Vector3 newPoint, float newAngle_X, float newAngle_Y, float newAngle_Z)
    {
      if (!this.channel.checkOwner(steamID))
        return;
      this.parent = (Transform) null;
      this.point = newPoint;
      if (((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.FREEFORM)
      {
        this.angle_x = newAngle_X;
        this.angle_z = newAngle_Z;
      }
      else
      {
        this.angle_x = 0.0f;
        this.angle_z = 0.0f;
      }
      this.angle_y = newAngle_Y;
      this.rotate_x = 0.0f;
      this.rotate_y = 0.0f;
      this.rotate_z = 0.0f;
      this.isValid = true;
    }

    private bool check()
    {
      this.angle_y = this.player.look.yaw;
      if (((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.FORTIFICATION)
      {
        Physics.Raycast(this.player.look.aim.position, this.player.look.aim.forward, out this.hit, ((ItemBarricadeAsset) this.player.equipment.asset).range, RayMasks.SLOTS_INTERACT);
        if ((UnityEngine.Object) this.hit.transform != (UnityEngine.Object) null)
        {
          if (this.hit.transform.tag == "Logic" && this.hit.transform.name == "Slot")
          {
            this.point = this.hit.point - this.hit.normal * ((ItemBarricadeAsset) this.player.equipment.asset).offset;
            this.angle_y = this.hit.transform.rotation.eulerAngles.y;
            if (!Level.checkSafe(this.point))
            {
              if (this.channel.isOwner)
                PlayerUI.hint((Transform) null, EPlayerMessage.BOUNDS);
              return false;
            }
            if (Physics.OverlapSphere(this.point, ((ItemBarricadeAsset) this.player.equipment.asset).radius, RayMasks.BLOCK_BARRICADE).Length == 0)
              return true;
            if (this.channel.isOwner)
              PlayerUI.hint((Transform) null, EPlayerMessage.BLOCKED);
            return false;
          }
          this.point = Vector3.zero;
          if (this.channel.isOwner)
            PlayerUI.hint((Transform) null, EPlayerMessage.WINDOW);
          return false;
        }
        this.point = Vector3.zero;
        if (this.channel.isOwner)
          PlayerUI.hint((Transform) null, EPlayerMessage.WINDOW);
        return false;
      }
      if (((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.BARRICADE || ((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.VEHICLE || (((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.BED || ((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.STORAGE) || (((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.GENERATOR || ((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.SPOT || (((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.CAMPFIRE || ((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.SPIKE)) || (((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.SAFEZONE || ((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.SIGN))
      {
        Physics.Raycast(this.player.look.aim.position, this.player.look.aim.forward, out this.hit, ((ItemBarricadeAsset) this.player.equipment.asset).range, RayMasks.BARRICADE_INTERACT);
        if ((UnityEngine.Object) this.hit.transform != (UnityEngine.Object) null)
        {
          if ((double) this.hit.normal.y < 0.00999999977648258)
          {
            if (this.channel.isOwner)
              PlayerUI.hint((Transform) null, EPlayerMessage.BLOCKED);
            return false;
          }
          this.point = (double) this.hit.normal.y <= 0.75 ? this.hit.point + Vector3.up * ((ItemBarricadeAsset) this.player.equipment.asset).offset : this.hit.point + this.hit.normal * ((ItemBarricadeAsset) this.player.equipment.asset).offset;
          if (!Level.checkSafe(this.point))
          {
            if (this.channel.isOwner)
              PlayerUI.hint((Transform) null, EPlayerMessage.BOUNDS);
            return false;
          }
          if (((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.BED)
          {
            if (Physics.OverlapSphere(this.point + Vector3.up, 1.1f, RayMasks.BLOCK_BARRICADE).Length != 0)
            {
              if (this.channel.isOwner)
                PlayerUI.hint((Transform) null, EPlayerMessage.BLOCKED);
              return false;
            }
          }
          else if (Physics.OverlapSphere(this.point, ((ItemBarricadeAsset) this.player.equipment.asset).radius, RayMasks.BLOCK_BARRICADE).Length != 0)
          {
            if (this.channel.isOwner)
              PlayerUI.hint((Transform) null, EPlayerMessage.BLOCKED);
            return false;
          }
          return true;
        }
        this.point = Vector3.zero;
        return false;
      }
      if (((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.WIRE)
      {
        Physics.Raycast(this.player.look.aim.position, this.player.look.aim.forward, out this.hit, ((ItemBarricadeAsset) this.player.equipment.asset).range, RayMasks.BARRICADE_INTERACT);
        if ((UnityEngine.Object) this.hit.transform != (UnityEngine.Object) null)
        {
          this.point = this.hit.point + this.hit.normal * ((ItemBarricadeAsset) this.player.equipment.asset).offset;
          if (!Level.checkSafe(this.point))
          {
            if (this.channel.isOwner)
              PlayerUI.hint((Transform) null, EPlayerMessage.BOUNDS);
            return false;
          }
          if (Physics.OverlapSphere(this.point, ((ItemBarricadeAsset) this.player.equipment.asset).radius, RayMasks.BLOCK_BARRICADE).Length == 0)
            return true;
          if (this.channel.isOwner)
            PlayerUI.hint((Transform) null, EPlayerMessage.BLOCKED);
          return false;
        }
        this.point = Vector3.zero;
        return false;
      }
      if (((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.FARM)
      {
        Physics.Raycast(this.player.look.aim.position, this.player.look.aim.forward, out this.hit, ((ItemBarricadeAsset) this.player.equipment.asset).range, RayMasks.BARRICADE_INTERACT);
        if ((UnityEngine.Object) this.hit.transform != (UnityEngine.Object) null)
        {
          this.point = (double) this.hit.normal.y <= 0.75 ? this.hit.point + Vector3.up * ((ItemBarricadeAsset) this.player.equipment.asset).offset : this.hit.point + this.hit.normal * ((ItemBarricadeAsset) this.player.equipment.asset).offset;
          if (this.hit.transform.tag == "Ground")
          {
            if (PhysicsTool.checkMaterial(this.point) != EPhysicsMaterial.FOLIAGE_STATIC)
            {
              if (this.channel.isOwner)
                PlayerUI.hint((Transform) null, EPlayerMessage.SOIL);
              return false;
            }
          }
          else if (PhysicsTool.checkMaterial(this.hit.collider) != EPhysicsMaterial.GRAVEL_STATIC)
          {
            if (this.channel.isOwner)
              PlayerUI.hint((Transform) null, EPlayerMessage.SOIL);
            return false;
          }
          if (!Level.checkSafe(this.point))
          {
            if (this.channel.isOwner)
              PlayerUI.hint((Transform) null, EPlayerMessage.BOUNDS);
            return false;
          }
          if (Physics.OverlapSphere(this.point, ((ItemBarricadeAsset) this.player.equipment.asset).radius, RayMasks.BLOCK_BARRICADE).Length == 0)
            return true;
          if (this.channel.isOwner)
            PlayerUI.hint((Transform) null, EPlayerMessage.BLOCKED);
          return false;
        }
        this.point = Vector3.zero;
        return false;
      }
      if (((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.DOOR)
      {
        Physics.Raycast(this.player.look.aim.position, this.player.look.aim.forward, out this.hit, ((ItemBarricadeAsset) this.player.equipment.asset).range, RayMasks.SLOTS_INTERACT);
        if ((UnityEngine.Object) this.hit.transform != (UnityEngine.Object) null)
        {
          if (this.hit.transform.tag == "Logic" && this.hit.transform.name == "Door")
          {
            this.point = this.hit.transform.position;
            this.angle_y = this.hit.transform.rotation.eulerAngles.y;
            if (!Level.checkSafe(this.point))
            {
              if (this.channel.isOwner)
                PlayerUI.hint((Transform) null, EPlayerMessage.BOUNDS);
              return false;
            }
            if (Physics.OverlapSphere(this.point, ((ItemBarricadeAsset) this.player.equipment.asset).radius, RayMasks.BARRICADE).Length == 0)
              return true;
            if (this.channel.isOwner)
              PlayerUI.hint((Transform) null, EPlayerMessage.BLOCKED);
            return false;
          }
          this.point = Vector3.zero;
          if (this.channel.isOwner)
            PlayerUI.hint((Transform) null, EPlayerMessage.DOORWAY);
          return false;
        }
        this.point = Vector3.zero;
        if (this.channel.isOwner)
          PlayerUI.hint((Transform) null, EPlayerMessage.DOORWAY);
        return false;
      }
      if (((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.GATE)
      {
        Physics.Raycast(this.player.look.aim.position, this.player.look.aim.forward, out this.hit, ((ItemBarricadeAsset) this.player.equipment.asset).range, RayMasks.SLOTS_INTERACT);
        if ((UnityEngine.Object) this.hit.transform != (UnityEngine.Object) null)
        {
          if (this.hit.transform.tag == "Logic" && this.hit.transform.name == "Gate")
          {
            this.point = this.hit.transform.position;
            this.angle_y = this.hit.transform.rotation.eulerAngles.y;
            if (!Level.checkSafe(this.point))
            {
              if (this.channel.isOwner)
                PlayerUI.hint((Transform) null, EPlayerMessage.BOUNDS);
              return false;
            }
            if (Physics.OverlapSphere(this.point, ((ItemBarricadeAsset) this.player.equipment.asset).radius, RayMasks.BARRICADE).Length != 0)
            {
              if (this.channel.isOwner)
                PlayerUI.hint((Transform) null, EPlayerMessage.BLOCKED);
              return false;
            }
            if (Physics.OverlapSphere(this.point + this.hit.transform.forward * -1.5f + this.hit.transform.up * -2f, 0.25f, RayMasks.BARRICADE).Length == 0)
              return true;
            if (this.channel.isOwner)
              PlayerUI.hint((Transform) null, EPlayerMessage.BLOCKED);
            return false;
          }
          this.point = Vector3.zero;
          if (this.channel.isOwner)
            PlayerUI.hint((Transform) null, EPlayerMessage.GARAGE);
          return false;
        }
        this.point = Vector3.zero;
        if (this.channel.isOwner)
          PlayerUI.hint((Transform) null, EPlayerMessage.GARAGE);
        return false;
      }
      if (((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.LADDER)
      {
        Physics.Raycast(this.player.look.aim.position, this.player.look.aim.forward, out this.hit, ((ItemBarricadeAsset) this.player.equipment.asset).range, RayMasks.LADDERS_INTERACT);
        if ((UnityEngine.Object) this.hit.transform != (UnityEngine.Object) null)
        {
          if (this.hit.transform.tag == "Logic" && this.hit.transform.name == "Climb")
          {
            this.point = this.hit.transform.position;
            this.angle_y = this.hit.transform.rotation.eulerAngles.y;
            if (Physics.OverlapSphere(this.point + this.hit.transform.up * 0.5f, 0.1f, RayMasks.BLOCK_BARRICADE).Length != 0)
            {
              if (this.channel.isOwner)
                PlayerUI.hint((Transform) null, EPlayerMessage.BLOCKED);
              return false;
            }
            if (Physics.OverlapSphere(this.point + this.hit.transform.up * -0.5f, 0.1f, RayMasks.BLOCK_BARRICADE).Length != 0)
            {
              if (this.channel.isOwner)
                PlayerUI.hint((Transform) null, EPlayerMessage.BLOCKED);
              return false;
            }
          }
          else
          {
            if ((double) Mathf.Abs(this.hit.normal.y) < 0.100000001490116)
            {
              this.point = this.hit.point + this.hit.normal * ((ItemBarricadeAsset) this.player.equipment.asset).offset;
              this.angle_y = Quaternion.LookRotation(this.hit.normal).eulerAngles.y;
              if (Physics.OverlapSphere(this.point + Quaternion.Euler(0.0f, this.angle_y, 0.0f) * Vector3.right * 0.5f, 0.1f, RayMasks.BLOCK_BARRICADE).Length != 0)
              {
                if (this.channel.isOwner)
                  PlayerUI.hint((Transform) null, EPlayerMessage.BLOCKED);
                return false;
              }
              if (Physics.OverlapSphere(this.point + Quaternion.Euler(0.0f, this.angle_y, 0.0f) * Vector3.left * 0.5f, 0.1f, RayMasks.BLOCK_BARRICADE).Length != 0)
              {
                if (this.channel.isOwner)
                  PlayerUI.hint((Transform) null, EPlayerMessage.BLOCKED);
                return false;
              }
            }
            else
            {
              this.point = (double) this.hit.normal.y <= 0.75 ? this.hit.point + Vector3.up * StructureManager.HEIGHT : this.hit.point + this.hit.normal * StructureManager.HEIGHT;
              if (Physics.OverlapSphere(this.point, 0.5f, RayMasks.BLOCK_BARRICADE).Length != 0)
              {
                if (this.channel.isOwner)
                  PlayerUI.hint((Transform) null, EPlayerMessage.BLOCKED);
                return false;
              }
            }
            if (!Level.checkSafe(this.point))
            {
              if (this.channel.isOwner)
                PlayerUI.hint((Transform) null, EPlayerMessage.BOUNDS);
              return false;
            }
          }
          return true;
        }
        this.point = Vector3.zero;
        return false;
      }
      if (((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.TORCH)
      {
        Physics.Raycast(this.player.look.aim.position, this.player.look.aim.forward, out this.hit, ((ItemBarricadeAsset) this.player.equipment.asset).range, RayMasks.BARRICADE_INTERACT);
        if ((UnityEngine.Object) this.hit.transform != (UnityEngine.Object) null && (double) Mathf.Abs(this.hit.normal.y) < 0.100000001490116)
        {
          this.point = this.hit.point + this.hit.normal * ((ItemBarricadeAsset) this.player.equipment.asset).offset;
          this.angle_y = Quaternion.LookRotation(this.hit.normal).eulerAngles.y;
          if (Physics.OverlapSphere(this.point, 0.1f, RayMasks.BLOCK_BARRICADE).Length != 0)
          {
            if (this.channel.isOwner)
              PlayerUI.hint((Transform) null, EPlayerMessage.BLOCKED);
            return false;
          }
          if (Level.checkSafe(this.point))
            return true;
          if (this.channel.isOwner)
            PlayerUI.hint((Transform) null, EPlayerMessage.BOUNDS);
          return false;
        }
        if (this.channel.isOwner)
          PlayerUI.hint((Transform) null, EPlayerMessage.WALL);
        this.point = Vector3.zero;
        return false;
      }
      if (((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.FREEFORM)
      {
        Physics.Raycast(this.player.look.aim.position, this.player.look.aim.forward, out this.hit, ((ItemBarricadeAsset) this.player.equipment.asset).range, RayMasks.BARRICADE_INTERACT);
        if ((UnityEngine.Object) this.hit.transform != (UnityEngine.Object) null)
        {
          this.point = this.hit.point + this.hit.normal * -0.125f + Quaternion.Euler(0.0f, this.angle_y + this.rotate_y, 0.0f) * Quaternion.Euler((((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.DOOR || ((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.GATE ? 0.0f : -90f) + this.angle_x + this.rotate_x, 0.0f, 0.0f) * Quaternion.Euler(0.0f, this.angle_z + this.rotate_z, 0.0f) * Vector3.forward * ((ItemBarricadeAsset) this.player.equipment.asset).offset;
          if (!Level.checkSafe(this.point))
          {
            if (this.channel.isOwner)
              PlayerUI.hint((Transform) null, EPlayerMessage.BOUNDS);
            return false;
          }
          if (Physics.OverlapSphere(this.point, ((ItemBarricadeAsset) this.player.equipment.asset).radius, RayMasks.BLOCK_BARRICADE).Length == 0)
            return true;
          if (this.channel.isOwner)
            PlayerUI.hint((Transform) null, EPlayerMessage.BLOCKED);
          return false;
        }
        this.point = Vector3.zero;
        return false;
      }
      this.point = Vector3.zero;
      return false;
    }

    private void build()
    {
      this.startedUse = Time.realtimeSinceStartup;
      this.isUsing = true;
      this.isBuilding = true;
      this.player.animator.play("Use", false);
    }

    [SteamCall]
    public void askBuild(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID) || !this.player.equipment.isEquipped)
        return;
      this.build();
    }

    public override void startPrimary()
    {
      if (this.player.equipment.isBusy || (!Dedicator.isDedicated || !this.isValid) && !this.check())
        return;
      if (this.channel.isOwner)
      {
        if ((UnityEngine.Object) this.parent != (UnityEngine.Object) null)
          this.channel.send("askBarricadeVehicle", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[5]
          {
            (object) this.parent.InverseTransformPoint(this.point),
            (object) (float) ((double) this.angle_x + (double) this.rotate_x),
            (object) (float) ((double) this.angle_y + (double) this.rotate_y - (double) this.parent.localRotation.eulerAngles.y),
            (object) (float) ((double) this.angle_z + (double) this.rotate_z),
            (object) DamageTool.getVehicle(this.parent).index
          });
        else
          this.channel.send("askBarricadeNone", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[4]
          {
            (object) this.point,
            (object) (float) ((double) this.angle_x + (double) this.rotate_x),
            (object) (float) ((double) this.angle_y + (double) this.rotate_y),
            (object) (float) ((double) this.angle_z + (double) this.rotate_z)
          });
      }
      if (!((UnityEngine.Object) this.parent != (UnityEngine.Object) null) && (double) (this.point - this.player.look.aim.position).sqrMagnitude >= 256.0)
        return;
      this.player.equipment.isBusy = true;
      this.build();
      if (!Provider.isServer)
        return;
      this.channel.send("askBuild", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
    }

    public override void startSecondary()
    {
      if (this.player.equipment.isBusy || ((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.FORTIFICATION || (((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.DOOR || ((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.GATE) || ((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.TORCH)
        return;
      this.player.look.isIgnoringInput = true;
      this.isRotating = true;
    }

    public override void stopSecondary()
    {
      this.player.look.isIgnoringInput = false;
      this.isRotating = false;
    }

    public override void equip()
    {
      this.player.animator.play("Equip", true);
      this.useTime = this.player.animator.getAnimationLength("Use");
      if (!this.channel.isOwner)
        return;
      this.help = BarricadeTool.getBarricade((Transform) null, false, Vector3.zero, Quaternion.identity, this.player.equipment.itemID, this.player.equipment.state);
      HighlighterTool.help(this.help, this.isValid);
      this.arrow = ((GameObject) UnityEngine.Object.Instantiate(Resources.Load("Build/Arrow"))).transform;
      this.arrow.name = "Arrow";
      this.arrow.parent = this.help;
      this.arrow.localPosition = Vector3.zero;
      this.arrow.localRotation = ((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.DOOR || ((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.GATE ? Quaternion.identity : Quaternion.Euler(90f, 0.0f, 0.0f);
      HighlighterTool.help(this.arrow, this.isValid);
      if ((UnityEngine.Object) this.help.FindChild("Radius") != (UnityEngine.Object) null)
        this.help.FindChild("Radius").gameObject.SetActive(true);
      Interactable component = this.help.GetComponent<Interactable>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) component);
      if (((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.SPIKE || ((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.WIRE)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.help.FindChild("Trap").GetComponent<InteractableTrap>());
      if (((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.DOOR || ((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.GATE)
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) this.help.FindChild("Skeleton").FindChild("Hinge").GetComponent<Collider>());
        UnityEngine.Object.Destroy((UnityEngine.Object) this.help.FindChild("Skeleton").FindChild("Hinge").GetComponent<InteractableDoor>());
        if ((UnityEngine.Object) this.help.FindChild("Skeleton").FindChild("Hinge").FindChild("Clip") != (UnityEngine.Object) null)
          UnityEngine.Object.Destroy((UnityEngine.Object) this.help.FindChild("Skeleton").FindChild("Hinge").FindChild("Clip").gameObject);
        if ((UnityEngine.Object) this.help.FindChild("Skeleton").FindChild("Hinge").FindChild("Nav") != (UnityEngine.Object) null)
          UnityEngine.Object.Destroy((UnityEngine.Object) this.help.FindChild("Skeleton").FindChild("Hinge").FindChild("Nav").gameObject);
      }
      else
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) this.help.GetComponent<Collider>());
        if ((UnityEngine.Object) this.help.FindChild("Clip") != (UnityEngine.Object) null)
          UnityEngine.Object.Destroy((UnityEngine.Object) this.help.FindChild("Clip").gameObject);
        if ((UnityEngine.Object) this.help.FindChild("Nav") != (UnityEngine.Object) null)
          UnityEngine.Object.Destroy((UnityEngine.Object) this.help.FindChild("Nav").gameObject);
        if ((UnityEngine.Object) this.help.FindChild("Ladder") != (UnityEngine.Object) null)
          UnityEngine.Object.Destroy((UnityEngine.Object) this.help.FindChild("Ladder").gameObject);
        if ((UnityEngine.Object) this.help.FindChild("Block") != (UnityEngine.Object) null)
          UnityEngine.Object.Destroy((UnityEngine.Object) this.help.FindChild("Block").gameObject);
      }
      for (int index = 0; index < 2 && (UnityEngine.Object) this.help.FindChild("Climb") != (UnityEngine.Object) null; ++index)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.help.FindChild("Climb").gameObject);
    }

    public override void dequip()
    {
      this.player.look.isIgnoringInput = false;
      this.isRotating = false;
      if (!this.channel.isOwner || !((UnityEngine.Object) this.help != (UnityEngine.Object) null))
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.help.gameObject);
    }

    public override void simulate(uint simulation)
    {
      if (!this.isUsing || !this.isUseable)
        return;
      this.player.equipment.isBusy = false;
      if (!Provider.isServer)
        return;
      ItemBarricadeAsset itemBarricadeAsset = (ItemBarricadeAsset) this.player.equipment.asset;
      if (itemBarricadeAsset != null)
      {
        if (itemBarricadeAsset.build == EBuild.VEHICLE)
        {
          VehicleManager.spawnVehicle(itemBarricadeAsset.explosion, this.point, Quaternion.Euler(this.angle_x + this.rotate_x, this.angle_y + this.rotate_y, this.angle_z + this.rotate_z));
        }
        else
        {
          Barricade barricade = new Barricade(this.player.equipment.itemID, itemBarricadeAsset.health, itemBarricadeAsset.getState());
          if (itemBarricadeAsset.build == EBuild.DOOR || itemBarricadeAsset.build == EBuild.GATE || itemBarricadeAsset.build == EBuild.SIGN)
          {
            BitConverter.GetBytes(this.channel.owner.playerID.steamID.m_SteamID).CopyTo((Array) barricade.state, 0);
            BitConverter.GetBytes(this.channel.owner.playerID.group.m_SteamID).CopyTo((Array) barricade.state, 8);
          }
          else if (itemBarricadeAsset.build == EBuild.BED)
            BitConverter.GetBytes(CSteamID.Nil.m_SteamID).CopyTo((Array) barricade.state, 0);
          else if (itemBarricadeAsset.build == EBuild.STORAGE)
          {
            BitConverter.GetBytes(this.channel.owner.playerID.steamID.m_SteamID).CopyTo((Array) barricade.state, 0);
            BitConverter.GetBytes(this.channel.owner.playerID.group.m_SteamID).CopyTo((Array) barricade.state, 8);
          }
          else if (itemBarricadeAsset.build == EBuild.FARM)
            BitConverter.GetBytes(Provider.time - (uint) ((double) ((ItemFarmAsset) this.player.equipment.asset).growth * ((double) this.player.skills.mastery(2, 5) * 0.5))).CopyTo((Array) barricade.state, 0);
          else if (itemBarricadeAsset.build == EBuild.TORCH || itemBarricadeAsset.build == EBuild.CAMPFIRE || (itemBarricadeAsset.build == EBuild.SPOT || itemBarricadeAsset.build == EBuild.SAFEZONE))
            barricade.state[0] = (byte) 1;
          else if (itemBarricadeAsset.build == EBuild.GENERATOR)
          {
            barricade.state[0] = (byte) 1;
            BitConverter.GetBytes((ushort) ((uint) InteractableGenerator.FUEL / 2U)).CopyTo((Array) barricade.state, 1);
          }
          BarricadeManager.dropBarricade(barricade, this.parent, this.point, this.angle_x + this.rotate_x, this.angle_y + this.rotate_y, this.angle_z + this.rotate_z, this.channel.owner.playerID.steamID.m_SteamID, this.channel.owner.playerID.group.m_SteamID);
        }
      }
      this.player.equipment.use();
    }

    public override void tick()
    {
      if (this.isBuilding && this.isBuildable)
      {
        this.isBuilding = false;
        if (!Dedicator.isDedicated)
          this.player.playSound(((ItemBarricadeAsset) this.player.equipment.asset).use);
      }
      if (!this.channel.isOwner || (UnityEngine.Object) this.help == (UnityEngine.Object) null || this.isUsing)
        return;
      if (this.isRotating)
      {
        if (((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.FREEFORM)
        {
          if (ControlsSettings.invert)
            this.rotate_x += ControlsSettings.look * 2f * Input.GetAxis("mouse_y");
          else
            this.rotate_x -= ControlsSettings.look * 2f * Input.GetAxis("mouse_y");
        }
        this.rotate_y += ControlsSettings.look * 2f * Input.GetAxis("mouse_x");
        if (((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.FREEFORM)
          this.rotate_z += ControlsSettings.look * 30f * Input.GetAxis("mouse_z");
      }
      if (this.check())
      {
        if (!this.isValid)
        {
          this.isValid = true;
          HighlighterTool.help(this.help, this.isValid);
          if ((UnityEngine.Object) this.arrow != (UnityEngine.Object) null)
            HighlighterTool.help(this.arrow, this.isValid);
        }
      }
      else if (this.isValid)
      {
        this.isValid = false;
        HighlighterTool.help(this.help, this.isValid);
        if ((UnityEngine.Object) this.arrow != (UnityEngine.Object) null)
          HighlighterTool.help(this.arrow, this.isValid);
      }
      this.parent = ((ItemBarricadeAsset) this.player.equipment.asset).build != EBuild.VEHICLE ? (!((UnityEngine.Object) this.hit.transform != (UnityEngine.Object) null) || !((UnityEngine.Object) this.hit.transform.parent != (UnityEngine.Object) null) || (!((UnityEngine.Object) this.hit.transform.parent.parent != (UnityEngine.Object) null) || !(this.hit.transform.parent.parent.tag == "Vehicle")) ? (!((UnityEngine.Object) this.hit.transform != (UnityEngine.Object) null) || !((UnityEngine.Object) this.hit.transform.parent != (UnityEngine.Object) null) || !(this.hit.transform.parent.tag == "Vehicle") ? (!((UnityEngine.Object) this.hit.transform != (UnityEngine.Object) null) || !(this.hit.transform.tag == "Vehicle") ? (Transform) null : this.hit.transform) : this.hit.transform.parent) : this.hit.transform.parent.parent) : (Transform) null;
      if ((UnityEngine.Object) this.help.parent != (UnityEngine.Object) this.parent)
      {
        this.help.parent = this.parent;
        this.help.gameObject.SetActive(false);
        this.help.gameObject.SetActive(true);
      }
      if ((UnityEngine.Object) this.parent != (UnityEngine.Object) null)
      {
        this.help.localPosition = this.parent.InverseTransformPoint(this.point);
        this.help.localRotation = Quaternion.Euler(0.0f, this.angle_y + this.rotate_y - this.parent.localRotation.eulerAngles.y, 0.0f);
        this.help.localRotation *= Quaternion.Euler((((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.DOOR || ((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.GATE ? 0.0f : -90f) + this.angle_x + this.rotate_x, 0.0f, 0.0f);
        this.help.localRotation *= Quaternion.Euler(0.0f, this.angle_z + this.rotate_z, 0.0f);
      }
      else
      {
        this.help.position = this.point;
        this.help.rotation = Quaternion.Euler(0.0f, this.angle_y + this.rotate_y, 0.0f);
        this.help.rotation *= Quaternion.Euler((((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.DOOR || ((ItemBarricadeAsset) this.player.equipment.asset).build == EBuild.GATE ? 0.0f : -90f) + this.angle_x + this.rotate_x, 0.0f, 0.0f);
        this.help.rotation *= Quaternion.Euler(0.0f, this.angle_z + this.rotate_z, 0.0f);
      }
    }
  }
}
