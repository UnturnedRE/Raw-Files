// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Player
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class Player : MonoBehaviour
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 1;
    public static PlayerCreated onPlayerCreated;
    public static bool isLoadingInventory;
    public static bool isLoadingLife;
    public static bool isLoadingClothing;
    public int agro;
    private static Player _player;
    protected SteamChannel _channel;
    private PlayerAnimator _animator;
    private PlayerClothing _clothing;
    private PlayerInventory _inventory;
    private PlayerEquipment _equipment;
    private PlayerLife _life;
    private PlayerCrafting _crafting;
    private PlayerSkills _skills;
    private PlayerMovement _movement;
    private PlayerLook _look;
    private PlayerStance _stance;
    private PlayerInput _input;
    private PlayerVoice _voice;
    private PlayerInteract _interact;
    private Transform _first;
    private Transform _third;
    private Transform _character;
    private Transform firstSpot;
    private Transform thirdSpot;
    private bool spotOn;
    private AudioSource sound;

    public static bool isLoading
    {
      get
      {
        if (!Player.isLoadingLife && !Player.isLoadingInventory)
          return Player.isLoadingClothing;
        return true;
      }
    }

    public static Player player
    {
      get
      {
        return Player._player;
      }
    }

    public SteamChannel channel
    {
      get
      {
        return this._channel;
      }
    }

    public PlayerAnimator animator
    {
      get
      {
        return this._animator;
      }
    }

    public PlayerClothing clothing
    {
      get
      {
        return this._clothing;
      }
    }

    public PlayerInventory inventory
    {
      get
      {
        return this._inventory;
      }
    }

    public PlayerEquipment equipment
    {
      get
      {
        return this._equipment;
      }
    }

    public PlayerLife life
    {
      get
      {
        return this._life;
      }
    }

    public PlayerCrafting crafting
    {
      get
      {
        return this._crafting;
      }
    }

    public PlayerSkills skills
    {
      get
      {
        return this._skills;
      }
    }

    public PlayerMovement movement
    {
      get
      {
        return this._movement;
      }
    }

    public PlayerLook look
    {
      get
      {
        return this._look;
      }
    }

    public PlayerStance stance
    {
      get
      {
        return this._stance;
      }
    }

    public PlayerInput input
    {
      get
      {
        return this._input;
      }
    }

    public PlayerVoice voice
    {
      get
      {
        return this._voice;
      }
    }

    public PlayerInteract interact
    {
      get
      {
        return this._interact;
      }
    }

    public Transform first
    {
      get
      {
        return this._first;
      }
    }

    public Transform third
    {
      get
      {
        return this._third;
      }
    }

    public Transform character
    {
      get
      {
        return this._character;
      }
    }

    public void playSound(AudioClip clip, float volume, float pitch, float deviation)
    {
      if ((Object) clip == (Object) null || Dedicator.isDedicated)
        return;
      this.sound.pitch = Random.Range(pitch - deviation, pitch + deviation);
      this.sound.PlayOneShot(clip, volume);
    }

    public void playSound(AudioClip clip, float pitch, float deviation)
    {
      this.playSound(clip, 1f, pitch, deviation);
    }

    public void playSound(AudioClip clip, float volume)
    {
      this.playSound(clip, volume, 1f, 0.1f);
    }

    public void playSound(AudioClip clip)
    {
      this.playSound(clip, 1f, 1f, 0.1f);
    }

    [SteamCall]
    public void askTeleport(CSteamID steamID, Vector3 position, byte angle)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this.transform.position = position;
      this.transform.rotation = Quaternion.Euler(0.0f, (float) ((int) angle * 2), 0.0f);
      this.look.updateLook();
      this.movement.updateMovement();
      this.movement.isAllowed = true;
    }

    public void sendTeleport(Vector3 position, byte angle)
    {
      this.channel.send("askTeleport", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[2]
      {
        (object) position,
        (object) angle
      });
    }

    [SteamCall]
    public void tellStat(CSteamID steamID, byte newStat)
    {
      if (!this.channel.checkServer(steamID))
        return;
      switch ((EPlayerStat) newStat)
      {
        case EPlayerStat.KILLS_PLAYERS:
          int data1;
          if (!Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Players", out data1))
            break;
          Provider.provider.statisticsService.userStatisticsService.setStatistic("Kills_Players", data1 + 1);
          break;
        case EPlayerStat.KILLS_ZOMBIES_NORMAL:
          int data2;
          if (!Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Zombies_Normal", out data2))
            break;
          Provider.provider.statisticsService.userStatisticsService.setStatistic("Kills_Zombies_Normal", data2 + 1);
          break;
        case EPlayerStat.KILLS_ZOMBIES_MEGA:
          int data3;
          if (!Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Zombies_Mega", out data3))
            break;
          Provider.provider.statisticsService.userStatisticsService.setStatistic("Kills_Zombies_Mega", data3 + 1);
          break;
        case EPlayerStat.FOUND_ITEMS:
          int data4;
          if (!Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Items", out data4))
            break;
          Provider.provider.statisticsService.userStatisticsService.setStatistic("Found_Items", data4 + 1);
          break;
        case EPlayerStat.FOUND_RESOURCES:
          int data5;
          if (!Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Resources", out data5))
            break;
          Provider.provider.statisticsService.userStatisticsService.setStatistic("Found_Resources", data5 + 1);
          break;
        case EPlayerStat.KILLS_ANIMALS:
          int data6;
          if (!Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Animals", out data6))
            break;
          Provider.provider.statisticsService.userStatisticsService.setStatistic("Kills_Animals", data6 + 1);
          break;
      }
    }

    public void sendStat(EPlayerStat stat)
    {
      if (Provider.mode == EGameMode.HARD)
        return;
      this.channel.send("tellStat", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) (byte) stat
      });
    }

    [SteamCall]
    public void askMessage(CSteamID steamID, byte message)
    {
      if (!this.channel.checkServer(steamID))
        return;
      PlayerUI.message((EPlayerMessage) message, string.Empty);
    }

    public void sendMessage(EPlayerMessage message)
    {
      this.channel.send("askMessage", ESteamCall.OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[1]
      {
        (object) (byte) message
      });
    }

    public void updateSpot(bool on)
    {
      this.spotOn = on;
      if (this.channel.isOwner)
      {
        this.firstSpot.gameObject.SetActive(on && Player.player.look.perspective == EPlayerPerspective.FIRST);
        this.thirdSpot.gameObject.SetActive(on && Player.player.look.perspective == EPlayerPerspective.THIRD);
      }
      else
        this.thirdSpot.gameObject.SetActive(on);
    }

    private void onPerspectiveUpdated(EPlayerPerspective newPerspective)
    {
      if (!this.spotOn)
        return;
      this.updateSpot(this.spotOn);
    }

    private void Start()
    {
      if (this.channel.isOwner)
      {
        Player._player = this;
        this._first = this.transform.FindChild("First");
        this._third = this.transform.FindChild("Third");
        this.first.gameObject.SetActive(true);
        this.third.gameObject.SetActive(true);
        this._character = ((GameObject) Object.Instantiate(Resources.Load("Characters/Inspect"))).transform;
        this.character.name = "Inspect";
        this.character.transform.position = new Vector3(256f, -256f, 0.0f);
        this.character.transform.rotation = Quaternion.Euler(90f, 0.0f, 0.0f);
        this.firstSpot = Camera.main.transform.FindChild("Spot");
        Player.isLoadingInventory = true;
        Player.isLoadingLife = true;
        Player.isLoadingClothing = true;
        this.look.onPerspectiveUpdated += new PerspectiveUpdated(this.onPerspectiveUpdated);
      }
      else
      {
        Object.Destroy((Object) this.transform.FindChild("First").gameObject);
        this._first = (Transform) null;
        this._third = this.transform.FindChild("Third");
        this.third.gameObject.SetActive(true);
      }
      this.thirdSpot = this.third.FindChild("Skeleton").FindChild("Spine").FindChild("Skull").FindChild("Spot");
      if (!this.channel.isOwner && !Provider.isServer || Player.onPlayerCreated == null)
        return;
      Player.onPlayerCreated(this);
    }

    private void Awake()
    {
      this._channel = this.GetComponent<SteamChannel>();
      this.agro = 0;
      this._animator = this.GetComponent<PlayerAnimator>();
      this._clothing = this.GetComponent<PlayerClothing>();
      this._inventory = this.GetComponent<PlayerInventory>();
      this._equipment = this.GetComponent<PlayerEquipment>();
      this._life = this.GetComponent<PlayerLife>();
      this._crafting = this.GetComponent<PlayerCrafting>();
      this._skills = this.GetComponent<PlayerSkills>();
      this._movement = this.GetComponent<PlayerMovement>();
      this._look = this.GetComponent<PlayerLook>();
      this._stance = this.GetComponent<PlayerStance>();
      this._input = this.GetComponent<PlayerInput>();
      this._voice = this.GetComponent<PlayerVoice>();
      this._interact = this.GetComponent<PlayerInteract>();
      this.sound = this.transform.FindChild("Sound").GetComponent<AudioSource>();
    }

    public void save()
    {
      if (this.life.isDead)
      {
        if (PlayerSavedata.fileExists(this.channel.owner.playerID, "/Player/Player.dat"))
          PlayerSavedata.deleteFile(this.channel.owner.playerID, "/Player/Player.dat");
      }
      else
      {
        Block block = new Block();
        block.writeByte(Player.SAVEDATA_VERSION);
        block.writeSingleVector3(this.transform.position);
        block.writeByte((byte) ((double) this.transform.rotation.eulerAngles.y / 2.0));
        PlayerSavedata.writeBlock(this.channel.owner.playerID, "/Player/Player.dat", block);
      }
      this.clothing.save();
      this.inventory.save();
      this.life.save();
      this.skills.save();
    }
  }
}
