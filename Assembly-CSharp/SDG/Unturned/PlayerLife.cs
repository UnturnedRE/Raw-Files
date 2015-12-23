// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerLife
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class PlayerLife : PlayerCaller
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 2;
    public static readonly float TIMER_HOME = 30f;
    public static readonly float TIMER_RESPAWN = 10f;
    public LifeUpdated onLifeUpdated;
    public HealthUpdated onHealthUpdated;
    public FoodUpdated onFoodUpdated;
    public WaterUpdated onWaterUpdated;
    public VirusUpdated onVirusUpdated;
    public StaminaUpdated onStaminaUpdated;
    public VisionUpdated onVisionUpdated;
    public OxygenUpdated onOxygenUpdated;
    public BleedingUpdated onBleedingUpdated;
    public BrokenUpdated onBrokenUpdated;
    public TemperatureUpdated onTemperatureUpdated;
    public Damaged onDamaged;
    private static EDeathCause _deathCause;
    private static ELimb _deathLimb;
    private static CSteamID _deathKiller;
    private bool _isDead;
    private byte lastHealth;
    private byte _health;
    private byte _food;
    private byte _water;
    private byte _virus;
    private byte _vision;
    private uint _warmth;
    private byte _stamina;
    private byte _oxygen;
    private bool _isBleeding;
    private bool _isBroken;
    private EPlayerTemperature _temperature;
    private uint lastStarve;
    private uint lastDehydrate;
    private uint lastView;
    private uint lastTire;
    private uint lastSuffocate;
    private uint lastRest;
    private uint lastBreath;
    private uint lastInfect;
    private uint lastBleed;
    private uint lastBleeding;
    private uint lastBroken;
    private uint lastFreeze;
    private uint lastWarm;
    private uint lastBurn;
    private uint lastCovered;
    private uint lastRegenerate;
    private bool wasWarm;
    private bool wasCovered;
    private float _lastRespawn;
    private float _lastDeath;
    private Vector3 ragdoll;

    public static EDeathCause deathCause
    {
      get
      {
        return PlayerLife._deathCause;
      }
    }

    public static ELimb deathLimb
    {
      get
      {
        return PlayerLife._deathLimb;
      }
    }

    public static CSteamID deathKiller
    {
      get
      {
        return PlayerLife._deathKiller;
      }
    }

    public bool isDead
    {
      get
      {
        return this._isDead;
      }
    }

    public byte health
    {
      get
      {
        return this._health;
      }
    }

    public byte food
    {
      get
      {
        return this._food;
      }
    }

    public byte water
    {
      get
      {
        return this._water;
      }
    }

    public byte virus
    {
      get
      {
        return this._virus;
      }
    }

    public byte vision
    {
      get
      {
        return this._vision;
      }
    }

    public uint warmth
    {
      get
      {
        return this._warmth;
      }
    }

    public byte stamina
    {
      get
      {
        return this._stamina;
      }
    }

    public byte oxygen
    {
      get
      {
        return this._oxygen;
      }
    }

    public bool isBleeding
    {
      get
      {
        return this._isBleeding;
      }
    }

    public bool isBroken
    {
      get
      {
        return this._isBroken;
      }
    }

    public EPlayerTemperature temperature
    {
      get
      {
        return this._temperature;
      }
    }

    public float lastRespawn
    {
      get
      {
        return this._lastRespawn;
      }
    }

    public float lastDeath
    {
      get
      {
        return this._lastDeath;
      }
    }

    [SteamCall]
    public void tellDeath(CSteamID steamID, byte newCause, byte newLimb, CSteamID newKiller)
    {
      if (!this.channel.checkServer(steamID))
        return;
      PlayerLife._deathCause = (EDeathCause) newCause;
      PlayerLife._deathLimb = (ELimb) newLimb;
      PlayerLife._deathKiller = newKiller;
      int data;
      if (!this.channel.isOwner || !Provider.provider.statisticsService.userStatisticsService.getStatistic("Deaths_Players", out data))
        return;
      Provider.provider.statisticsService.userStatisticsService.setStatistic("Deaths_Players", data + 1);
    }

    [SteamCall]
    public void tellDead(CSteamID steamID, Vector3 newRagdoll)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this._isDead = true;
      this._lastDeath = Time.realtimeSinceStartup;
      this.ragdoll = newRagdoll;
      if (!Dedicator.isDedicated)
        RagdollTool.ragdollPlayer(this.transform.position, this.transform.rotation, this.player.animator.thirdSkeleton, this.ragdoll, this.player.clothing);
      if (this.onLifeUpdated == null)
        return;
      this.onLifeUpdated(this.isDead);
    }

    [SteamCall]
    public void tellRevive(CSteamID steamID, Vector3 position, byte angle)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this._isDead = false;
      this._lastRespawn = Time.realtimeSinceStartup;
      this.player.askTeleport(steamID, position, angle);
      if (this.onLifeUpdated == null)
        return;
      this.onLifeUpdated(this.isDead);
    }

    [SteamCall]
    public void tellLife(CSteamID steamID, byte newHealth, byte newFood, byte newWater, byte newVirus, bool newBleeding, bool newBroken)
    {
      this.tellHealth(steamID, newHealth);
      this.tellFood(steamID, newFood);
      this.tellWater(steamID, newWater);
      this.tellVirus(steamID, newVirus);
      this.tellBleeding(steamID, newBleeding);
      this.tellBroken(steamID, newBroken);
      if (!this.channel.checkServer(steamID))
        return;
      this._stamina = (byte) 100;
      this._oxygen = (byte) 100;
      this._vision = (byte) 0;
      this._warmth = 0U;
      this._temperature = EPlayerTemperature.NONE;
      this.wasWarm = false;
      this.wasCovered = false;
      if (this.onVisionUpdated != null)
        this.onVisionUpdated(false);
      if (this.onStaminaUpdated != null)
        this.onStaminaUpdated(this.stamina);
      if (this.onOxygenUpdated != null)
        this.onOxygenUpdated(this.oxygen);
      if (this.onTemperatureUpdated != null)
        this.onTemperatureUpdated(this.temperature);
      Player.isLoadingLife = false;
    }

    [SteamCall]
    public void askLife(CSteamID steamID)
    {
      if (!Provider.isServer)
        return;
      if (this.channel.checkOwner(steamID))
      {
        this.channel.send("tellLife", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) this.health, (object) this.food, (object) this.water, (object) this.virus, (object) (bool) (this.isBleeding ? 1 : 0), (object) (bool) (this.isBroken ? 1 : 0));
      }
      else
      {
        if (!this.isDead)
          return;
        this.channel.send("tellDead", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
        {
          (object) this.ragdoll
        });
      }
    }

    [SteamCall]
    public void tellHealth(CSteamID steamID, byte newHealth)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this._health = newHealth;
      if (this.onHealthUpdated != null)
        this.onHealthUpdated(this.health);
      if ((int) newHealth < (int) this.lastHealth - 3 && this.onDamaged != null)
        this.onDamaged((byte) ((uint) this.lastHealth - (uint) newHealth));
      this.lastHealth = newHealth;
    }

    [SteamCall]
    public void tellFood(CSteamID steamID, byte newFood)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this._food = newFood;
      if (this.onFoodUpdated == null)
        return;
      this.onFoodUpdated(this.food);
    }

    [SteamCall]
    public void tellWater(CSteamID steamID, byte newWater)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this._water = newWater;
      if (this.onWaterUpdated == null)
        return;
      this.onWaterUpdated(this.water);
    }

    [SteamCall]
    public void tellVirus(CSteamID steamID, byte newVirus)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this._virus = newVirus;
      if (this.onVirusUpdated == null)
        return;
      this.onVirusUpdated(this.virus);
    }

    [SteamCall]
    public void tellBleeding(CSteamID steamID, bool newBleeding)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this._isBleeding = newBleeding;
      if (this.onBleedingUpdated == null)
        return;
      this.onBleedingUpdated(this.isBleeding);
    }

    [SteamCall]
    public void tellBroken(CSteamID steamID, bool newBroken)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this._isBroken = newBroken;
      if (this.onBrokenUpdated == null)
        return;
      this.onBrokenUpdated(this.isBroken);
    }

    public void askDamage(byte amount, Vector3 newRagdoll, EDeathCause newCause, ELimb newLimb, CSteamID newKiller, out EPlayerKill kill)
    {
      kill = EPlayerKill.NONE;
      if ((int) amount == 0 || this.isDead || (this.player.movement.isSafe || this.isDead))
        return;
      if ((int) amount >= (int) this.health)
        this._health = (byte) 0;
      else
        this._health -= amount;
      this.ragdoll = newRagdoll;
      this.channel.send("tellHealth", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) this.health
      });
      if ((int) this.health == 0)
      {
        kill = EPlayerKill.PLAYER;
        this.channel.send("tellDeath", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[3]
        {
          (object) (byte) newCause,
          (object) (byte) newLimb,
          (object) newKiller
        });
        this.channel.send("tellDead", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
        {
          (object) this.ragdoll
        });
        if (newCause == EDeathCause.BLEEDING)
          CommandWindow.Log((object) Provider.localization.format("Bleeding", (object) this.channel.owner.playerID.characterName, (object) this.channel.owner.playerID.playerName));
        else if (newCause == EDeathCause.BONES)
          CommandWindow.Log((object) Provider.localization.format("Bones", (object) this.channel.owner.playerID.characterName, (object) this.channel.owner.playerID.playerName));
        else if (newCause == EDeathCause.FREEZING)
          CommandWindow.Log((object) Provider.localization.format("Freezing", (object) this.channel.owner.playerID.characterName, (object) this.channel.owner.playerID.playerName));
        else if (newCause == EDeathCause.BURNING)
          CommandWindow.Log((object) Provider.localization.format("Burning", (object) this.channel.owner.playerID.characterName, (object) this.channel.owner.playerID.playerName));
        else if (newCause == EDeathCause.FOOD)
          CommandWindow.Log((object) Provider.localization.format("Food", (object) this.channel.owner.playerID.characterName, (object) this.channel.owner.playerID.playerName));
        else if (newCause == EDeathCause.WATER)
          CommandWindow.Log((object) Provider.localization.format("Water", (object) this.channel.owner.playerID.characterName, (object) this.channel.owner.playerID.playerName));
        else if (newCause == EDeathCause.GUN || newCause == EDeathCause.MELEE || (newCause == EDeathCause.PUNCH || newCause == EDeathCause.ROADKILL))
        {
          SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(newKiller);
          string str1;
          string str2;
          if ((Object) this.player != (Object) null)
          {
            str1 = steamPlayer.playerID.characterName;
            str2 = steamPlayer.playerID.playerName;
          }
          else
          {
            str1 = "?";
            str2 = "?";
          }
          string str3 = string.Empty;
          if (newLimb == ELimb.LEFT_FOOT || newLimb == ELimb.LEFT_LEG || (newLimb == ELimb.RIGHT_FOOT || newLimb == ELimb.RIGHT_LEG))
            str3 = Provider.localization.format("Leg");
          else if (newLimb == ELimb.LEFT_HAND || newLimb == ELimb.LEFT_ARM || (newLimb == ELimb.RIGHT_HAND || newLimb == ELimb.RIGHT_ARM))
            str3 = Provider.localization.format("Arm");
          else if (newLimb == ELimb.SPINE)
            str3 = Provider.localization.format("Spine");
          else if (newLimb == ELimb.SKULL)
            str3 = Provider.localization.format("Skull");
          if (newCause == EDeathCause.GUN)
            CommandWindow.Log((object) Provider.localization.format("Gun", (object) this.channel.owner.playerID.characterName, (object) this.channel.owner.playerID.playerName, (object) str3, (object) str1, (object) str2));
          else if (newCause == EDeathCause.MELEE)
            CommandWindow.Log((object) Provider.localization.format("Melee", (object) this.channel.owner.playerID.characterName, (object) this.channel.owner.playerID.playerName, (object) str3, (object) str1, (object) str2));
          else if (newCause == EDeathCause.PUNCH)
          {
            CommandWindow.Log((object) Provider.localization.format("Punch", (object) this.channel.owner.playerID.characterName, (object) this.channel.owner.playerID.playerName, (object) str3, (object) str1, (object) str2));
          }
          else
          {
            if (newCause != EDeathCause.ROADKILL)
              return;
            CommandWindow.Log((object) Provider.localization.format("Roadkill", (object) this.channel.owner.playerID.characterName, (object) this.channel.owner.playerID.playerName, (object) str1, (object) str2));
          }
        }
        else if (newCause == EDeathCause.ZOMBIE)
          CommandWindow.Log((object) Provider.localization.format("Zombie", (object) this.channel.owner.playerID.characterName, (object) this.channel.owner.playerID.playerName));
        else if (newCause == EDeathCause.ANIMAL)
          CommandWindow.Log((object) Provider.localization.format("Animal", (object) this.channel.owner.playerID.characterName, (object) this.channel.owner.playerID.playerName));
        else if (newCause == EDeathCause.SUICIDE)
          CommandWindow.Log((object) Provider.localization.format("Suicide", (object) this.channel.owner.playerID.characterName, (object) this.channel.owner.playerID.playerName));
        else if (newCause == EDeathCause.INFECTION)
          CommandWindow.Log((object) Provider.localization.format("Infection", (object) this.channel.owner.playerID.characterName, (object) this.channel.owner.playerID.playerName));
        else if (newCause == EDeathCause.BREATH)
          CommandWindow.Log((object) Provider.localization.format("Breath", (object) this.channel.owner.playerID.characterName, (object) this.channel.owner.playerID.playerName));
        else if (newCause == EDeathCause.ZOMBIE)
          CommandWindow.Log((object) Provider.localization.format("Zombie", (object) this.channel.owner.playerID.characterName, (object) this.channel.owner.playerID.playerName));
        else if (newCause == EDeathCause.VEHICLE)
          CommandWindow.Log((object) Provider.localization.format("Vehicle", (object) this.channel.owner.playerID.characterName, (object) this.channel.owner.playerID.playerName));
        else if (newCause == EDeathCause.GRENADE)
          CommandWindow.Log((object) Provider.localization.format("Grenade", (object) this.channel.owner.playerID.characterName, (object) this.channel.owner.playerID.playerName));
        else if (newCause == EDeathCause.SHRED)
        {
          CommandWindow.Log((object) Provider.localization.format("Shred", (object) this.channel.owner.playerID.characterName, (object) this.channel.owner.playerID.playerName));
        }
        else
        {
          if (newCause != EDeathCause.LANDMINE)
            return;
          CommandWindow.Log((object) Provider.localization.format("Landmine", (object) this.channel.owner.playerID.characterName, (object) this.channel.owner.playerID.playerName));
        }
      }
      else
      {
        if (Provider.mode == EGameMode.EASY || (int) amount < 20 || this.isBleeding)
          return;
        this._isBleeding = true;
        this.lastBleeding = this.player.input.simulation;
        this.lastBleed = this.player.input.simulation;
        this.channel.send("tellBleeding", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
        {
          (object) (bool) (this.isBleeding ? 1 : 0)
        });
      }
    }

    public void askHeal(byte amount, bool healBleeding, bool healBroken)
    {
      if ((int) amount == 0 || this.isDead || this.isDead)
        return;
      if ((int) amount >= 100 - (int) this.health)
        this._health = (byte) 100;
      else
        this._health += amount;
      this.channel.send("tellHealth", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) this.health
      });
      if (this.isBleeding && healBleeding)
      {
        this._isBleeding = false;
        this.channel.send("tellBleeding", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
        {
          (object) (bool) (this.isBleeding ? 1 : 0)
        });
      }
      if (!this.isBroken || !healBroken)
        return;
      this._isBroken = false;
      this.channel.send("tellBroken", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) (bool) (this.isBroken ? 1 : 0)
      });
    }

    public void askStarve(byte amount)
    {
      if ((int) amount == 0 || this.isDead || this.isDead)
        return;
      if ((int) amount >= (int) this.food)
        this._food = (byte) 0;
      else
        this._food -= amount;
      this.channel.send("tellFood", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) this.food
      });
    }

    public void askEat(byte amount)
    {
      if ((int) amount == 0 || this.isDead || this.isDead)
        return;
      if ((int) amount >= 100 - (int) this.food)
        this._food = (byte) 100;
      else
        this._food += amount;
      this.channel.send("tellFood", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) this.food
      });
    }

    public void askDehydrate(byte amount)
    {
      if ((int) amount == 0 || this.isDead || this.isDead)
        return;
      if ((int) amount >= (int) this.water)
        this._water = (byte) 0;
      else
        this._water -= amount;
      this.channel.send("tellWater", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) this.water
      });
    }

    public void askDrink(byte amount)
    {
      if ((int) amount == 0 || this.isDead || this.isDead)
        return;
      if ((int) amount >= 100 - (int) this.water)
        this._water = (byte) 100;
      else
        this._water += amount;
      this.channel.send("tellWater", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) this.water
      });
    }

    public void askInfect(byte amount)
    {
      if ((int) amount == 0 || this.isDead || this.isDead)
        return;
      if ((int) amount >= (int) this.virus)
        this._virus = (byte) 0;
      else
        this._virus -= amount;
      this.channel.send("tellVirus", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) this.virus
      });
    }

    public void askDisinfect(byte amount)
    {
      if ((int) amount == 0 || this.isDead || this.isDead)
        return;
      if ((int) amount >= 100 - (int) this.virus)
        this._virus = (byte) 100;
      else
        this._virus += amount;
      this.channel.send("tellVirus", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) this.virus
      });
    }

    public void askTire(byte amount)
    {
      if ((int) amount == 0 || this.isDead || this.isDead)
        return;
      this.lastTire = this.player.input.simulation;
      if ((int) amount >= (int) this.stamina)
        this._stamina = (byte) 0;
      else
        this._stamina -= amount;
      if (this.onStaminaUpdated == null)
        return;
      this.onStaminaUpdated(this.stamina);
    }

    public void askRest(byte amount)
    {
      if ((int) amount == 0 || this.isDead || this.isDead)
        return;
      if ((int) amount >= 100 - (int) this.stamina)
        this._stamina = (byte) 100;
      else
        this._stamina += amount;
      if (this.onStaminaUpdated == null)
        return;
      this.onStaminaUpdated(this.stamina);
    }

    public void askView(byte amount)
    {
      if ((int) amount == 0 || this.isDead || this.isDead)
        return;
      this.lastView = this.player.input.simulation;
      if ((int) amount >= 120 - (int) this.vision)
        this._vision = (byte) 120;
      else
        this._vision += amount;
      if (this.onVisionUpdated == null)
        return;
      this.onVisionUpdated(true);
    }

    public void askWarm(uint amount)
    {
      if ((int) amount == 0 || this.isDead || this.isDead)
        return;
      this._warmth += amount;
    }

    public void askBlind(byte amount)
    {
      if ((int) amount == 0 || this.isDead || this.isDead)
        return;
      if ((int) amount >= (int) this.vision)
        this._vision = (byte) 0;
      else
        this._vision -= amount;
      if ((int) this.vision != 0 || this.onVisionUpdated == null)
        return;
      this.onVisionUpdated(false);
    }

    public void askSuffocate(byte amount)
    {
      if ((int) amount == 0 || this.isDead || this.isDead)
        return;
      this.lastSuffocate = this.player.input.simulation;
      if ((int) amount >= (int) this.oxygen)
        this._oxygen = (byte) 0;
      else
        this._oxygen -= amount;
      if (this.onOxygenUpdated == null)
        return;
      this.onOxygenUpdated(this.oxygen);
    }

    public void askBreath(byte amount)
    {
      if ((int) amount == 0 || this.isDead || this.isDead)
        return;
      if ((int) amount >= 100 - (int) this.oxygen)
        this._oxygen = (byte) 100;
      else
        this._oxygen += amount;
      if (this.onOxygenUpdated == null)
        return;
      this.onOxygenUpdated(this.oxygen);
    }

    [SteamCall]
    public void askRespawn(CSteamID steamID, bool atHome)
    {
      if (!this.channel.checkOwner(steamID) || !Provider.isServer || !this.isDead)
        return;
      if (atHome)
      {
        if ((!Provider.isServer || Dedicator.isDedicated) && Provider.isPvP)
        {
          if ((double) Time.realtimeSinceStartup - (double) this.lastDeath < (double) PlayerLife.TIMER_HOME)
            return;
        }
        else if ((double) Time.realtimeSinceStartup - (double) this.lastRespawn < (double) PlayerLife.TIMER_RESPAWN)
          return;
      }
      else if ((double) Time.realtimeSinceStartup - (double) this.lastRespawn < (double) PlayerLife.TIMER_RESPAWN)
        return;
      this._health = (byte) 100;
      this._food = (byte) 100;
      this._water = (byte) 100;
      this._virus = (byte) 100;
      this._stamina = (byte) 100;
      this._oxygen = (byte) 100;
      this._vision = (byte) 0;
      this._warmth = 0U;
      this._isBleeding = false;
      this._isBroken = false;
      this._temperature = EPlayerTemperature.NONE;
      this.wasWarm = false;
      this.wasCovered = false;
      this.lastStarve = this.player.input.simulation;
      this.lastDehydrate = this.player.input.simulation;
      this.lastTire = this.player.input.simulation;
      this.lastRest = this.player.input.simulation;
      this.channel.send("tellLife", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) this.health, (object) this.food, (object) this.water, (object) this.virus, (object) (bool) (this.isBleeding ? 1 : 0), (object) (bool) (this.isBroken ? 1 : 0));
      Vector3 point;
      byte angle;
      if (!atHome || !BarricadeManager.tryGetBed(this.channel.owner.playerID.steamID, out point, out angle))
      {
        PlayerSpawnpoint spawn = LevelPlayers.getSpawn();
        point = spawn.point;
        angle = MeasurementTool.angleToByte(spawn.angle);
      }
      this.channel.send("tellRevive", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[2]
      {
        (object) (point + new Vector3(0.0f, 0.5f, 0.0f)),
        (object) angle
      });
    }

    [SteamCall]
    public void askSuicide(CSteamID steamID)
    {
      if (!this.channel.checkOwner(steamID) || !Provider.isServer || this.isDead)
        return;
      EPlayerKill kill;
      this.askDamage((byte) 100, Vector3.up * 10f, EDeathCause.SUICIDE, ELimb.SKULL, steamID, out kill);
    }

    public void sendRespawn(bool atHome)
    {
      this.channel.send("askRespawn", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[1]
      {
        (object) (bool) (atHome ? 1 : 0)
      });
    }

    public void sendSuicide()
    {
      this.channel.send("askSuicide", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
    }

    public void init()
    {
      this.channel.send("askLife", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER);
    }

    public void simulate(uint simulation)
    {
      if (Provider.isServer)
      {
        if (Level.info.type == ELevelType.SURVIVAL)
        {
          if ((int) this.food > 0)
          {
            if ((double) (simulation - this.lastStarve) > 350.0 * (1.0 + (double) this.player.skills.mastery(1, 6)) * ((double) LevelLighting.snowLevel <= 0.00999999977648258 || (double) this.transform.position.y <= (double) LevelLighting.snowLevel * (double) Level.TERRAIN ? 1.0 : 0.5 + (double) this.player.skills.mastery(1, 5) * 0.5))
            {
              this.lastStarve = simulation;
              this.askStarve((byte) 1);
            }
          }
          else if (simulation - this.lastStarve > 15U)
          {
            this.lastStarve = simulation;
            EPlayerKill kill;
            this.askDamage((byte) 1, Vector3.up, EDeathCause.FOOD, ELimb.SPINE, Provider.server, out kill);
          }
          if ((int) this.water > 0)
          {
            if ((double) (simulation - this.lastDehydrate) > 320.0 * (1.0 + (double) this.player.skills.mastery(1, 6)))
            {
              this.lastDehydrate = simulation;
              this.askDehydrate((byte) 1);
            }
          }
          else if (simulation - this.lastDehydrate > 20U)
          {
            this.lastDehydrate = simulation;
            EPlayerKill kill;
            this.askDamage((byte) 1, Vector3.up, EDeathCause.WATER, ELimb.SPINE, Provider.server, out kill);
          }
          if ((int) this.virus == 0 && simulation - this.lastInfect > 25U)
          {
            this.lastInfect = simulation;
            EPlayerKill kill;
            this.askDamage((byte) 1, Vector3.up, EDeathCause.INFECTION, ELimb.SPINE, Provider.server, out kill);
          }
        }
        if (this.isBleeding)
        {
          if (simulation - this.lastBleed > 10U)
          {
            this.lastBleed = simulation;
            EPlayerKill kill;
            this.askDamage((byte) 1, Vector3.up, EDeathCause.BLEEDING, ELimb.SPINE, Provider.server, out kill);
          }
        }
        else if ((int) this.health < 100 && (int) this.food > 90 && ((int) this.water > 90 && (double) (simulation - this.lastRegenerate) > 60.0 * (1.0 - (double) this.player.skills.mastery(1, 1) * 0.5)))
        {
          this.lastRegenerate = simulation;
          this.askHeal((byte) 1, false, false);
        }
        if (Provider.mode != EGameMode.HARD)
        {
          if (this.isBleeding && (double) (simulation - this.lastBleeding) > 750.0 * (1.0 - (double) this.player.skills.mastery(1, 4) * 0.5))
          {
            this._isBleeding = false;
            this.channel.send("tellBleeding", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
            {
              (object) (bool) (this.isBleeding ? 1 : 0)
            });
          }
          if (this.isBroken && (double) (simulation - this.lastBroken) > 750.0 * (1.0 - (double) this.player.skills.mastery(1, 4) * 0.5))
          {
            this._isBroken = false;
            this.channel.send("tellBroken", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
            {
              (object) (bool) (this.isBroken ? 1 : 0)
            });
          }
        }
      }
      if (this.channel.isOwner)
      {
        if ((int) this.vision > 0 && simulation - this.lastView > 12U)
        {
          this.lastView = simulation;
          this.askBlind((byte) 1);
        }
        if (!this.isDead)
          Provider.provider.economyService.updateInventory();
      }
      if (!this.channel.isOwner && !Provider.isServer)
        return;
      if (this.player.stance.stance == EPlayerStance.SPRINT && (double) (simulation - this.lastTire) > 2.0 * (1.0 + (double) this.player.skills.mastery(0, 4) * 5.0))
      {
        this.lastTire = simulation;
        this.askTire((byte) (3.0 - (double) this.player.skills.mastery(0, 4) * 2.0));
      }
      if ((int) this.stamina < 100 && (double) (simulation - this.lastTire) > 32.0 * (1.0 - (double) this.player.skills.mastery(0, 3) * 0.5) && simulation - this.lastRest > 1U)
      {
        this.lastRest = simulation;
        this.askRest((byte) (1.0 + (double) this.player.skills.mastery(0, 3) * 2.0));
      }
      if (this.player.stance.isSubmerged)
      {
        if ((int) this.oxygen > 0)
        {
          if ((double) (simulation - this.lastSuffocate) > 2.0 * (1.0 + (double) this.player.skills.mastery(0, 5) * 5.0))
          {
            this.lastSuffocate = simulation;
            this.askSuffocate((byte) 1);
          }
        }
        else if (simulation - this.lastSuffocate > 10U)
        {
          this.lastSuffocate = simulation;
          if (Provider.isServer)
          {
            EPlayerKill kill;
            this.askDamage((byte) 10, Vector3.up, EDeathCause.BREATH, ELimb.SPINE, Provider.server, out kill);
          }
        }
      }
      else if ((int) this.oxygen < 100 && (double) (simulation - this.lastSuffocate) > 4.0 * (1.0 - (double) this.player.skills.mastery(0, 3) * 0.5) && simulation - this.lastBreath > 0U)
      {
        this.lastBreath = simulation;
        this.askBreath((byte) (1.0 + (double) this.player.skills.mastery(0, 3) * 2.0));
      }
      if (this.warmth > 0U)
        --this._warmth;
      EPlayerTemperature temperature = this.temperature;
      EPlayerTemperature eplayerTemperature;
      switch (TemperatureManager.checkPointTemperature(this.transform.position))
      {
        case EPlayerTemperature.BURNING:
          eplayerTemperature = EPlayerTemperature.BURNING;
          if (Provider.isServer && simulation - this.lastBurn > 10U)
          {
            this.lastBurn = simulation;
            EPlayerKill kill;
            this.askDamage((byte) 10, Vector3.up, EDeathCause.BURNING, ELimb.SPINE, Provider.server, out kill);
          }
          this.lastWarm = simulation;
          this.wasWarm = true;
          break;
        case EPlayerTemperature.WARM:
          eplayerTemperature = EPlayerTemperature.WARM;
          this.lastWarm = simulation;
          this.wasWarm = true;
          break;
        default:
          if (this.warmth <= 0U)
          {
            if ((double) LevelLighting.snowLevel > 0.00999999977648258 && (double) this.transform.position.y > (double) LevelLighting.snowLevel * (double) Level.TERRAIN)
            {
              if (this.player.stance.stance == EPlayerStance.SWIM)
              {
                eplayerTemperature = EPlayerTemperature.FREEZING;
                if (simulation - this.lastFreeze > 25U)
                {
                  this.lastFreeze = simulation;
                  byte amount = (byte) 8;
                  if ((int) this.player.clothing.shirt != 0 || (int) this.player.clothing.vest != 0)
                    amount -= (byte) 2;
                  if ((int) this.player.clothing.pants != 0)
                    amount -= (byte) 2;
                  if ((int) this.player.clothing.hat != 0)
                    amount -= (byte) 2;
                  EPlayerKill kill;
                  this.askDamage(amount, Vector3.up, EDeathCause.FREEZING, ELimb.SPINE, Provider.server, out kill);
                  break;
                }
                break;
              }
              if (!this.wasWarm || (double) (simulation - this.lastWarm) > 250.0 * (1.0 + (double) this.player.skills.mastery(1, 5)))
              {
                if (Physics.Raycast(this.transform.position + Vector3.up, Quaternion.Euler(45f, LevelLighting.wind, 0.0f) * -Vector3.forward, 32f, RayMasks.BLOCK_WIND))
                {
                  eplayerTemperature = EPlayerTemperature.COVERED;
                  this.lastCovered = simulation;
                  this.wasCovered = true;
                  break;
                }
                byte num = (byte) 1;
                if ((int) this.player.clothing.shirt != 0 || (int) this.player.clothing.vest != 0)
                  ++num;
                if ((int) this.player.clothing.pants != 0)
                  ++num;
                if ((int) this.player.clothing.hat != 0)
                  ++num;
                if (!this.wasCovered || (long) (simulation - this.lastCovered) > (long) (50 * (int) num))
                {
                  eplayerTemperature = EPlayerTemperature.FREEZING;
                  if (simulation - this.lastFreeze > 75U)
                  {
                    this.lastFreeze = simulation;
                    byte amount = (byte) 4;
                    if ((int) this.player.clothing.shirt != 0 || (int) this.player.clothing.vest != 0)
                      --amount;
                    if ((int) this.player.clothing.pants != 0)
                      --amount;
                    if ((int) this.player.clothing.hat != 0)
                      --amount;
                    EPlayerKill kill;
                    this.askDamage(amount, Vector3.up, EDeathCause.FREEZING, ELimb.SPINE, Provider.server, out kill);
                    break;
                  }
                  break;
                }
                eplayerTemperature = EPlayerTemperature.COLD;
                break;
              }
              eplayerTemperature = EPlayerTemperature.COLD;
              this.lastCovered = simulation;
              this.wasCovered = true;
              break;
            }
            eplayerTemperature = EPlayerTemperature.NONE;
            break;
          }
          goto case EPlayerTemperature.WARM;
      }
      if (eplayerTemperature == this.temperature)
        return;
      this._temperature = eplayerTemperature;
      if (this.onTemperatureUpdated == null)
        return;
      this.onTemperatureUpdated(this.temperature);
    }

    public void breakLegs()
    {
      this.lastBroken = this.player.input.simulation;
      if (this.isBroken)
        return;
      this._isBroken = true;
      this.channel.send("tellBroken", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) (bool) (this.isBroken ? 1 : 0)
      });
      EffectManager.sendEffect((ushort) 31, EffectManager.SMALL, this.transform.position);
    }

    private void onLanded(float fall)
    {
      if ((double) fall >= -5.5)
        return;
      if ((double) fall < -20.0)
        fall = -20f;
      EPlayerKill kill;
      this.askDamage((byte) ((double) Mathf.Abs(fall) * (1.0 - (double) this.player.skills.mastery(1, 4) * 0.75)), Vector3.down, EDeathCause.BONES, ELimb.SPINE, Provider.server, out kill);
      if (Provider.mode == EGameMode.EASY || (double) this.player.movement.gravity <= 0.670000016689301)
        return;
      this.breakLegs();
    }

    private void Start()
    {
      if (Provider.isServer)
      {
        this.player.movement.onLanded += new Landed(this.onLanded);
        this.load();
      }
      this.Invoke("init", 0.1f);
    }

    public void load()
    {
      this._isDead = false;
      if (PlayerSavedata.fileExists(this.channel.owner.playerID, "/Player/Life.dat") && Level.info.type == ELevelType.SURVIVAL)
      {
        Block block = PlayerSavedata.readBlock(this.channel.owner.playerID, "/Player/Life.dat", (byte) 0);
        if ((int) block.readByte() > 1)
        {
          this._health = block.readByte();
          this._food = block.readByte();
          this._water = block.readByte();
          this._virus = block.readByte();
          this._stamina = (byte) 100;
          this._oxygen = (byte) 100;
          this._isBleeding = block.readBoolean();
          this._isBroken = block.readBoolean();
          this._temperature = EPlayerTemperature.NONE;
          this.wasWarm = false;
          this.wasCovered = false;
          return;
        }
      }
      this._health = (byte) 100;
      this._food = (byte) 100;
      this._water = (byte) 100;
      this._virus = (byte) 100;
      this._stamina = (byte) 100;
      this._oxygen = (byte) 100;
      this._isBleeding = false;
      this._isBroken = false;
      this._temperature = EPlayerTemperature.NONE;
      this.wasWarm = false;
      this.wasCovered = false;
    }

    public void save()
    {
      if (this.player.life.isDead)
      {
        if (!PlayerSavedata.fileExists(this.channel.owner.playerID, "/Player/Life.dat"))
          return;
        PlayerSavedata.deleteFile(this.channel.owner.playerID, "/Player/Life.dat");
      }
      else
      {
        Block block = new Block();
        block.writeByte(PlayerLife.SAVEDATA_VERSION);
        block.writeByte(this.health);
        block.writeByte(this.food);
        block.writeByte(this.water);
        block.writeByte(this.virus);
        block.writeBoolean(this.isBleeding);
        block.writeBoolean(this.isBroken);
        PlayerSavedata.writeBlock(this.channel.owner.playerID, "/Player/Life.dat", block);
      }
    }
  }
}
