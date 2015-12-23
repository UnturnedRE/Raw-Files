// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.UseableFisher
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class UseableFisher : Useable
  {
    private float startedCast;
    private float startedReel;
    private float castTime;
    private float reelTime;
    private bool isCasting;
    private bool isReeling;
    private bool isFishing;
    private bool isBobbing;
    private bool isLuring;
    private bool isCatch;
    private Transform bob;
    private Transform firstHook;
    private Transform thirdHook;
    private LineRenderer firstLine;
    private LineRenderer thirdLine;
    private Vector3 water;
    private float lastLuck;
    private float luckTime;
    private bool hasSplashed;
    private bool hasTugged;

    private bool isCastable
    {
      get
      {
        return (double) Time.realtimeSinceStartup - (double) this.startedCast > (double) this.castTime;
      }
    }

    private bool isReelable
    {
      get
      {
        return (double) Time.realtimeSinceStartup - (double) this.startedReel > (double) this.reelTime;
      }
    }

    private bool isBobable
    {
      get
      {
        if (this.isCasting)
          return (double) Time.realtimeSinceStartup - (double) this.startedCast > (double) this.castTime * 0.449999988079071;
        return (double) Time.realtimeSinceStartup - (double) this.startedReel > (double) this.reelTime * 0.75;
      }
    }

    private void reel()
    {
      this.player.playSound(((ItemFisherAsset) this.player.equipment.asset).reel);
      this.player.animator.play("Reel", false);
    }

    [SteamCall]
    public void askCatch(CSteamID steamID)
    {
      if (!this.channel.checkOwner(steamID))
        return;
      this.isCatch = true;
    }

    [SteamCall]
    public void askReel(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID) || !this.player.equipment.isEquipped)
        return;
      this.reel();
    }

    private void cast()
    {
      this.player.playSound(((ItemFisherAsset) this.player.equipment.asset).cast);
      this.player.animator.play("Cast", false);
    }

    [SteamCall]
    public void askCast(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID) || !this.player.equipment.isEquipped)
        return;
      this.cast();
    }

    public override void startPrimary()
    {
      if (this.player.equipment.isBusy)
        return;
      if (this.isFishing)
      {
        this.isFishing = false;
        this.player.equipment.isBusy = true;
        this.startedReel = Time.realtimeSinceStartup;
        this.isReeling = true;
        if (this.channel.isOwner)
        {
          this.isBobbing = true;
          if ((double) Time.realtimeSinceStartup - (double) this.lastLuck > (double) this.luckTime - 1.39999997615814 && (double) Time.realtimeSinceStartup - (double) this.lastLuck < (double) this.luckTime)
            this.channel.send("askCatch", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER);
        }
        this.reel();
        if (!Provider.isServer)
          return;
        if (this.isCatch)
        {
          this.isCatch = false;
          if (((ItemFisherAsset) this.player.equipment.asset).fishes.Length > 0)
          {
            ushort newID = ((ItemFisherAsset) this.player.equipment.asset).fishes[Random.Range(0, ((ItemFisherAsset) this.player.equipment.asset).fishes.Length)];
            if ((int) newID != 0)
              this.player.inventory.forceAddItem(new Item(newID, true), false);
          }
          this.player.sendStat(EPlayerStat.FOUND_RESOURCES);
          this.player.skills.askAward(3U);
        }
        this.channel.send("askReel", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
      }
      else
      {
        this.isFishing = true;
        this.player.equipment.isBusy = true;
        this.startedCast = Time.realtimeSinceStartup;
        this.isCasting = true;
        if (this.channel.isOwner)
        {
          this.isBobbing = true;
          this.resetLuck();
        }
        this.cast();
        if (!Provider.isServer)
          return;
        if (Provider.mode != EGameMode.EASY && (int) this.player.equipment.quality > 0)
        {
          if ((int) this.player.equipment.quality > (int) ((ItemFisherAsset) this.player.equipment.asset).durability)
            this.player.equipment.quality -= ((ItemFisherAsset) this.player.equipment.asset).durability;
          else
            this.player.equipment.quality = (byte) 0;
          this.player.equipment.sendUpdateQuality();
        }
        this.channel.send("askCast", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
      }
    }

    public override void equip()
    {
      this.player.animator.play("Equip", true);
      this.castTime = this.player.animator.getAnimationLength("Cast");
      this.reelTime = this.player.animator.getAnimationLength("Reel");
      if (!this.channel.isOwner)
        return;
      this.firstHook = this.player.equipment.firstModel.FindChild("Hook");
      this.thirdHook = this.player.equipment.thirdModel.FindChild("Hook");
      this.firstLine = (LineRenderer) this.player.equipment.firstModel.FindChild("Line").GetComponent<Renderer>();
      this.firstLine.tag = "Environment";
      this.firstLine.gameObject.layer = LayerMasks.ENVIRONMENT;
      this.firstLine.gameObject.SetActive(true);
      this.thirdLine = (LineRenderer) this.player.equipment.thirdModel.FindChild("Line").GetComponent<Renderer>();
      this.thirdLine.gameObject.SetActive(true);
    }

    public override void dequip()
    {
      if (!this.channel.isOwner || !((Object) this.bob != (Object) null))
        return;
      Object.Destroy((Object) this.bob.gameObject);
    }

    public override void tick()
    {
      if (!this.player.equipment.isEquipped || !this.channel.isOwner)
        return;
      if (this.isBobable && this.isBobbing)
      {
        if (this.isCasting)
        {
          this.bob = ((GameObject) Object.Instantiate(Resources.Load("Fishers/Bob"))).transform;
          this.bob.name = "Bob";
          this.bob.parent = Level.effects;
          this.bob.position = this.player.look.aim.position + this.player.look.aim.forward;
          this.bob.GetComponent<Rigidbody>().AddForce(this.player.look.aim.forward * 750f);
          this.isBobbing = false;
          this.isLuring = true;
        }
        else if (this.isReeling && (Object) this.bob != (Object) null)
          Object.Destroy((Object) this.bob.gameObject);
      }
      if ((Object) this.bob != (Object) null)
      {
        if (this.player.look.perspective == EPlayerPerspective.FIRST)
        {
          this.firstLine.SetPosition(0, this.firstHook.position);
          this.firstLine.SetPosition(1, this.bob.position);
        }
        else
        {
          this.thirdLine.SetPosition(0, this.thirdHook.position);
          this.thirdLine.SetPosition(1, this.bob.position);
        }
      }
      else if (this.player.look.perspective == EPlayerPerspective.FIRST)
      {
        this.firstLine.SetPosition(0, Vector3.zero);
        this.firstLine.SetPosition(1, Vector3.zero);
      }
      else
      {
        this.thirdLine.SetPosition(0, Vector3.zero);
        this.thirdLine.SetPosition(1, Vector3.zero);
      }
    }

    public override void simulate(uint simulation)
    {
      if (this.isCasting && this.isCastable)
      {
        this.player.equipment.isBusy = false;
        this.isCasting = false;
      }
      else
      {
        if (!this.isReeling || !this.isReelable)
          return;
        this.player.equipment.isBusy = false;
        this.isReeling = false;
      }
    }

    private void resetLuck()
    {
      this.lastLuck = Time.realtimeSinceStartup;
      this.luckTime = (float) ((45.0 - (double) this.player.skills.mastery(2, 4) * 22.5 + (double) Random.Range(5f, 15f)) * ((int) this.player.equipment.quality >= 50 ? 1.0 : 1.0 + (1.0 - (double) this.player.equipment.quality / 50.0)));
      this.hasSplashed = false;
      this.hasTugged = false;
    }

    private void Update()
    {
      if (!((Object) this.bob != (Object) null))
        return;
      if (this.isLuring)
      {
        if ((double) this.bob.position.y >= (double) LevelLighting.seaLevel * (double) Level.TERRAIN - 4.0)
          return;
        this.bob.GetComponent<Rigidbody>().useGravity = false;
        this.bob.GetComponent<Rigidbody>().isKinematic = true;
        this.water = this.bob.position;
        this.water.y = LevelLighting.seaLevel * Level.TERRAIN;
        this.isLuring = false;
      }
      else
      {
        if ((double) Time.realtimeSinceStartup - (double) this.lastLuck > (double) this.luckTime)
        {
          if (!this.isReeling)
            this.resetLuck();
        }
        else if ((double) Time.realtimeSinceStartup - (double) this.lastLuck > (double) this.luckTime - 1.39999997615814)
        {
          if (!this.hasTugged)
          {
            this.hasTugged = true;
            this.player.playSound(((ItemFisherAsset) this.player.equipment.asset).tug);
            this.player.animator.play("Tug", false);
          }
        }
        else if ((double) Time.realtimeSinceStartup - (double) this.lastLuck > (double) this.luckTime - 2.40000009536743 && !this.hasSplashed)
        {
          this.hasSplashed = true;
          Transform transform = ((GameObject) Object.Instantiate(Resources.Load("Fishers/Splash"))).transform;
          transform.name = "Splash";
          transform.parent = Level.effects;
          transform.position = this.water;
          transform.rotation = Quaternion.Euler(-90f, Random.Range(0.0f, 360f), 0.0f);
          Object.Destroy((Object) transform.gameObject, 8f);
        }
        if ((double) Time.realtimeSinceStartup - (double) this.lastLuck > (double) this.luckTime - 1.39999997615814)
          this.bob.GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(this.bob.position, this.water + Vector3.down * 4f + Vector3.left * Random.Range(-4f, 4f) + Vector3.forward * Random.Range(-4f, 4f), 4f * Time.deltaTime));
        else
          this.bob.GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(this.bob.position, this.water + Vector3.up * Mathf.Sin(Time.time) * 0.25f, 4f * Time.deltaTime));
      }
    }
  }
}
