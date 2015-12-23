// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ZombieManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class ZombieManager : SteamCaller
  {
    private static readonly float CHANCE_EASY = 0.2f;
    private static readonly float CHANCE_NORMAL = 0.25f;
    private static readonly float CHANCE_HARD = 0.3f;
    private static readonly float CHANCE_PRO = 0.25f;
    private static readonly float RESPAWN = 360f;
    private static readonly float LOOT_EASY = 0.55f;
    private static readonly float LOOT_NORMAL = 0.5f;
    private static readonly float LOOT_HARD = 0.3f;
    private static readonly float LOOT_PRO = 0.65f;
    private static readonly float MOON = 0.05f;
    private static ZombieManager manager;
    private static ZombieRegion[] _regions;
    private static byte respawnZombiesBound;
    private static float lastWave;
    private static bool _waveReady;
    private static int _waveIndex;
    private static int _waveRemaining;
    public static WaveUpdated onWaveUpdated;

    private static float chance
    {
      get
      {
        if (Provider.mode == EGameMode.EASY)
          return ZombieManager.CHANCE_EASY;
        if (Provider.mode == EGameMode.NORMAL)
          return ZombieManager.CHANCE_NORMAL;
        if (Provider.mode == EGameMode.HARD)
          return ZombieManager.CHANCE_HARD;
        if (Provider.mode == EGameMode.PRO)
          return ZombieManager.CHANCE_PRO;
        return 0.0f;
      }
    }

    private static float loot
    {
      get
      {
        if (Provider.mode == EGameMode.EASY)
          return ZombieManager.LOOT_EASY;
        if (Provider.mode == EGameMode.NORMAL)
          return ZombieManager.LOOT_NORMAL;
        if (Provider.mode == EGameMode.HARD)
          return ZombieManager.LOOT_HARD;
        if (Provider.mode == EGameMode.PRO)
          return ZombieManager.LOOT_PRO;
        return 1f;
      }
    }

    public static ZombieRegion[] regions
    {
      get
      {
        return ZombieManager._regions;
      }
    }

    public static bool waveReady
    {
      get
      {
        return ZombieManager._waveReady;
      }
    }

    public static int waveIndex
    {
      get
      {
        return ZombieManager._waveIndex;
      }
    }

    public static int waveRemaining
    {
      get
      {
        return ZombieManager._waveRemaining;
      }
    }

    [SteamCall]
    public void tellWave(CSteamID steamID, bool newWaveReady, int newWave)
    {
      if (!this.channel.checkServer(steamID))
        return;
      ZombieManager._waveReady = newWaveReady;
      ZombieManager._waveIndex = newWave;
      if (ZombieManager.onWaveUpdated == null)
        return;
      ZombieManager.onWaveUpdated(ZombieManager.waveReady, ZombieManager.waveIndex);
    }

    [SteamCall]
    public void askWave(CSteamID steamID)
    {
      this.channel.send("tellWave", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[2]
      {
        (object) (bool) (ZombieManager.waveReady ? 1 : 0),
        (object) ZombieManager.waveIndex
      });
    }

    [SteamCall]
    public void tellZombieAlive(CSteamID steamID, byte reference, ushort id, byte newType, byte newSpeciality, byte newShirt, byte newPants, byte newHat, byte newGear, Vector3 newPosition, byte newAngle)
    {
      if (!this.channel.checkServer(steamID) || !Provider.isServer && !ZombieManager.regions[(int) reference].isNetworked || (int) id >= ZombieManager.regions[(int) reference].zombies.Count)
        return;
      ZombieManager.regions[(int) reference].zombies[(int) id].tellAlive(newType, newSpeciality, newShirt, newPants, newHat, newGear, newPosition, newAngle);
    }

    [SteamCall]
    public void tellZombieDead(CSteamID steamID, byte reference, ushort id, Vector3 newRagdoll)
    {
      if (!this.channel.checkServer(steamID) || !Provider.isServer && !ZombieManager.regions[(int) reference].isNetworked || (int) id >= ZombieManager.regions[(int) reference].zombies.Count)
        return;
      ZombieManager.regions[(int) reference].zombies[(int) id].tellDead(newRagdoll);
    }

    [SteamCall]
    public void tellZombieStates(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID))
        return;
      byte num1 = (byte) this.channel.read(Types.BYTE_TYPE);
      if (!Provider.isServer && !ZombieManager.regions[(int) num1].isNetworked)
        return;
      ushort num2 = (ushort) this.channel.read(Types.UINT16_TYPE);
      for (int index = 0; index < (int) num2; ++index)
      {
        object[] objArray = this.channel.read(Types.UINT16_TYPE, Types.VECTOR3_TYPE, Types.BYTE_TYPE);
        if ((int) (ushort) objArray[0] >= ZombieManager.regions[(int) num1].zombies.Count)
          break;
        ZombieManager.regions[(int) num1].zombies[(int) (ushort) objArray[0]].tellState((Vector3) objArray[1], (byte) objArray[2]);
      }
    }

    [SteamCall]
    public void askZombieAttack(CSteamID steamID, byte reference, ushort id, byte attack)
    {
      if (!this.channel.checkServer(steamID) || !Provider.isServer && !ZombieManager.regions[(int) reference].isNetworked || (int) id >= ZombieManager.regions[(int) reference].zombies.Count)
        return;
      ZombieManager.regions[(int) reference].zombies[(int) id].askAttack(attack);
    }

    [SteamCall]
    public void askZombieStartle(CSteamID steamID, byte reference, ushort id, byte startle)
    {
      if (!this.channel.checkServer(steamID) || !Provider.isServer && !ZombieManager.regions[(int) reference].isNetworked || (int) id >= ZombieManager.regions[(int) reference].zombies.Count)
        return;
      ZombieManager.regions[(int) reference].zombies[(int) id].askStartle(startle);
    }

    [SteamCall]
    public void askZombieStun(CSteamID steamID, byte reference, ushort id, byte stun)
    {
      if (!this.channel.checkServer(steamID) || !Provider.isServer && !ZombieManager.regions[(int) reference].isNetworked || (int) id >= ZombieManager.regions[(int) reference].zombies.Count)
        return;
      ZombieManager.regions[(int) reference].zombies[(int) id].askStun(stun);
    }

    [SteamCall]
    public void tellZombies(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID))
        return;
      byte bound = (byte) this.channel.read(Types.BYTE_TYPE);
      if (ZombieManager.regions[(int) bound].isNetworked)
        return;
      ZombieManager.regions[(int) bound].isNetworked = true;
      ushort num = (ushort) this.channel.read(Types.UINT16_TYPE);
      for (int index = 0; index < (int) num; ++index)
      {
        object[] objArray = this.channel.read(Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.VECTOR3_TYPE, Types.BYTE_TYPE, Types.BOOLEAN_TYPE);
        this.addZombie(bound, (byte) objArray[0], (byte) objArray[1], (byte) objArray[2], (byte) objArray[3], (byte) objArray[4], (byte) objArray[5], (byte) objArray[6], (byte) objArray[7], (Vector3) objArray[8], (float) ((int) (byte) objArray[9] * 2), (bool) objArray[10]);
      }
    }

    public void askZombies(CSteamID steamID, byte bound)
    {
      this.channel.openWrite();
      this.channel.write((object) bound);
      this.channel.write((object) (ushort) ZombieManager.regions[(int) bound].zombies.Count);
      for (int index = 0; index < ZombieManager.regions[(int) bound].zombies.Count; ++index)
      {
        Zombie zombie = ZombieManager.regions[(int) bound].zombies[index];
        this.channel.write((object) zombie.type, (object) (byte) zombie.speciality, (object) zombie.shirt, (object) zombie.pants, (object) zombie.hat, (object) zombie.gear, (object) zombie.move, (object) zombie.idle, (object) zombie.transform.position, (object) MeasurementTool.angleToByte(zombie.transform.rotation.eulerAngles.y), (object) (bool) (zombie.isDead ? 1 : 0));
      }
      this.channel.closeWrite("tellZombies", steamID, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
    }

    public static void sendZombieAlive(Zombie zombie, byte newType, byte newSpeciality, byte newShirt, byte newPants, byte newHat, byte newGear, Vector3 newPosition, byte newAngle)
    {
      ZombieManager.manager.channel.send("tellZombieAlive", ESteamCall.ALL, zombie.bound, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) zombie.bound, (object) zombie.id, (object) newType, (object) newSpeciality, (object) newShirt, (object) newPants, (object) newHat, (object) newGear, (object) newPosition, (object) newAngle);
    }

    public static void sendZombieDead(Zombie zombie, Vector3 newRagdoll)
    {
      ZombieManager.manager.channel.send("tellZombieDead", ESteamCall.ALL, zombie.bound, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[3]
      {
        (object) zombie.bound,
        (object) zombie.id,
        (object) newRagdoll
      });
    }

    public static void sendZombieAttack(Zombie zombie, byte attack)
    {
      ZombieManager.manager.channel.send("askZombieAttack", ESteamCall.ALL, zombie.bound, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
      {
        (object) zombie.bound,
        (object) zombie.id,
        (object) attack
      });
    }

    public static void sendZombieStartle(Zombie zombie, byte startle)
    {
      ZombieManager.manager.channel.send("askZombieStartle", ESteamCall.ALL, zombie.bound, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
      {
        (object) zombie.bound,
        (object) zombie.id,
        (object) startle
      });
    }

    public static void sendZombieStun(Zombie zombie, byte stun)
    {
      ZombieManager.manager.channel.send("askZombieStun", ESteamCall.ALL, zombie.bound, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
      {
        (object) zombie.bound,
        (object) zombie.id,
        (object) stun
      });
    }

    public static void dropLoot(Zombie zombie)
    {
      int num = !LevelZombies.tables[(int) zombie.type].isMega ? 1 : 5;
      if (num > 1)
      {
        ZombieManager.regions[(int) zombie.bound].lastMega = Time.realtimeSinceStartup;
        ZombieManager.regions[(int) zombie.bound].hasMega = false;
      }
      if ((int) LevelZombies.tables[(int) zombie.type].loot >= LevelItems.tables.Count || num <= 1 && (double) Random.value >= (double) ZombieManager.loot)
        return;
      for (int index = 0; index < num; ++index)
      {
        ushort newID = LevelItems.getItem(LevelZombies.tables[(int) zombie.type].loot);
        if ((int) newID != 0)
          ItemManager.dropItem(new Item(newID, false), zombie.transform.position, false, Dedicator.isDedicated, true);
      }
    }

    private void addZombie(byte bound, byte type, byte speciality, byte shirt, byte pants, byte hat, byte gear, byte move, byte idle, Vector3 position, float angle, bool isDead)
    {
      Transform transform = !Dedicator.isDedicated ? (!Provider.isServer ? ((GameObject) Object.Instantiate(Resources.Load("Characters/Zombie_Client"))).transform : ((GameObject) Object.Instantiate(Resources.Load("Characters/Zombie_Server"))).transform) : ((GameObject) Object.Instantiate(Resources.Load("Characters/Zombie_Dedicated"))).transform;
      transform.name = "Zombie";
      transform.parent = LevelZombies.models;
      transform.position = position;
      transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
      Zombie component = transform.GetComponent<Zombie>();
      component.id = (ushort) ZombieManager.regions[(int) bound].zombies.Count;
      component.speciality = (EZombieSpeciality) speciality;
      component.bound = bound;
      component.type = type;
      component.shirt = shirt;
      component.pants = pants;
      component.hat = hat;
      component.gear = gear;
      component.move = move;
      component.idle = idle;
      component.isDead = isDead;
      if (!isDead)
        ++ZombieManager.regions[(int) bound].alive;
      ZombieManager.regions[(int) bound].zombies.Add(transform.GetComponent<Zombie>());
    }

    public static Zombie getZombie(Vector3 point, ushort id)
    {
      byte bound;
      if (!LevelNavigation.tryGetBounds(point, out bound))
        return (Zombie) null;
      if ((int) id >= ZombieManager.regions[(int) bound].zombies.Count)
        return (Zombie) null;
      if (ZombieManager.regions[(int) bound].zombies[(int) id].isDead)
        return (Zombie) null;
      return ZombieManager.regions[(int) bound].zombies[(int) id];
    }

    private void generateZombies(byte bound)
    {
      if (LevelNavigation.bounds.Count == 0 || LevelZombies.zombies.Length == 0 || (LevelNavigation.bounds.Count != LevelZombies.zombies.Length || LevelZombies.zombies[(int) bound].Count <= 0))
        return;
      List<ZombieSpawnpoint> list = new List<ZombieSpawnpoint>();
      for (int index = 0; index < LevelZombies.zombies[(int) bound].Count; ++index)
      {
        ZombieSpawnpoint zombieSpawnpoint = LevelZombies.zombies[(int) bound][index];
        if (SafezoneManager.checkPointValid(zombieSpawnpoint.point))
          list.Add(zombieSpawnpoint);
      }
      while (list.Count > 0)
      {
        if (Level.info.type == ELevelType.HORDE)
        {
          if (ZombieManager.regions[(int) bound].zombies.Count >= 40)
            break;
        }
        else if ((double) ZombieManager.regions[(int) bound].zombies.Count >= (double) LevelZombies.zombies[(int) bound].Count * (double) ZombieManager.chance)
          break;
        int index = Random.Range(0, list.Count);
        ZombieSpawnpoint zombieSpawnpoint = list[index];
        list.RemoveAt(index);
        ZombieTable zombieTable = LevelZombies.tables[(int) zombieSpawnpoint.type];
        if (!zombieTable.isMega || !ZombieManager.regions[(int) ZombieManager.respawnZombiesBound].hasMega && (double) Time.realtimeSinceStartup - (double) ZombieManager.regions[(int) ZombieManager.respawnZombiesBound].lastMega > 600.0)
        {
          EZombieSpeciality ezombieSpeciality = EZombieSpeciality.NORMAL;
          if (zombieTable.isMega)
          {
            ZombieManager.regions[(int) ZombieManager.respawnZombiesBound].lastMega = Time.realtimeSinceStartup;
            ZombieManager.regions[(int) ZombieManager.respawnZombiesBound].hasMega = true;
            ezombieSpeciality = EZombieSpeciality.MEGA;
          }
          else if (Provider.mode != EGameMode.EASY && Level.info.type == ELevelType.SURVIVAL)
          {
            float num = Random.value;
            if ((double) num < 0.150000005960464)
              ezombieSpeciality = EZombieSpeciality.CRAWLER;
            else if ((double) num < 0.300000011920929)
              ezombieSpeciality = EZombieSpeciality.SPRINTER;
          }
          byte shirt = byte.MaxValue;
          if (zombieTable.slots[0].table.Count > 0 && (double) Random.value < (double) zombieTable.slots[0].chance)
            shirt = (byte) Random.Range(0, zombieTable.slots[0].table.Count);
          byte pants = byte.MaxValue;
          if (zombieTable.slots[1].table.Count > 0 && (double) Random.value < (double) zombieTable.slots[1].chance)
            pants = (byte) Random.Range(0, zombieTable.slots[1].table.Count);
          byte hat = byte.MaxValue;
          if (zombieTable.slots[2].table.Count > 0 && (double) Random.value < (double) zombieTable.slots[2].chance)
            hat = (byte) Random.Range(0, zombieTable.slots[2].table.Count);
          byte gear = byte.MaxValue;
          if (zombieTable.slots[3].table.Count > 0 && (double) Random.value < (double) zombieTable.slots[3].chance)
            gear = (byte) Random.Range(0, zombieTable.slots[3].table.Count);
          byte move = (byte) Random.Range(0, 4);
          byte idle = (byte) Random.Range(0, 3);
          Vector3 point = zombieSpawnpoint.point;
          point.y += 0.1f;
          this.addZombie(bound, zombieSpawnpoint.type, (byte) ezombieSpeciality, shirt, pants, hat, gear, move, idle, point, Random.Range(0.0f, 360f), Level.info.type == ELevelType.HORDE);
        }
      }
    }

    private void respawnZombies()
    {
      if (Level.info.type == ELevelType.HORDE)
      {
        if (ZombieManager.waveRemaining > 0 || ZombieManager.regions[(int) ZombieManager.respawnZombiesBound].alive > 0)
          ZombieManager.lastWave = Time.realtimeSinceStartup;
        if (ZombieManager.waveRemaining == 0)
        {
          if (ZombieManager.regions[(int) ZombieManager.respawnZombiesBound].alive > 0)
            return;
          if ((double) Time.realtimeSinceStartup - (double) ZombieManager.lastWave > 10.0 || ZombieManager.waveIndex == 0)
          {
            if (!ZombieManager.waveReady)
            {
              ZombieManager._waveReady = true;
              ++ZombieManager._waveIndex;
              ZombieManager._waveRemaining = (int) Mathf.Ceil(Mathf.Pow((float) (ZombieManager.waveIndex + 5), 1.5f));
              this.channel.send("tellWave", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[2]
              {
                (object) (bool) (ZombieManager.waveReady ? 1 : 0),
                (object) ZombieManager.waveIndex
              });
            }
          }
          else
          {
            if (!ZombieManager.waveReady)
              return;
            ZombieManager._waveReady = false;
            this.channel.send("tellWave", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[2]
            {
              (object) (bool) (ZombieManager.waveReady ? 1 : 0),
              (object) ZombieManager.waveIndex
            });
            return;
          }
        }
      }
      if (ZombieManager.regions[(int) ZombieManager.respawnZombiesBound].zombies.Count <= 0)
        return;
      if ((int) ZombieManager.regions[(int) ZombieManager.respawnZombiesBound].respawnZombieIndex >= ZombieManager.regions[(int) ZombieManager.respawnZombiesBound].zombies.Count)
        ZombieManager.regions[(int) ZombieManager.respawnZombiesBound].respawnZombieIndex = (ushort) (ZombieManager.regions[(int) ZombieManager.respawnZombiesBound].zombies.Count - 1);
      Zombie zombie = ZombieManager.regions[(int) ZombieManager.respawnZombiesBound].zombies[(int) ZombieManager.regions[(int) ZombieManager.respawnZombiesBound].respawnZombieIndex];
      ++ZombieManager.regions[(int) ZombieManager.respawnZombiesBound].respawnZombieIndex;
      if ((int) ZombieManager.regions[(int) ZombieManager.respawnZombiesBound].respawnZombieIndex >= ZombieManager.regions[(int) ZombieManager.respawnZombiesBound].zombies.Count)
        ZombieManager.regions[(int) ZombieManager.respawnZombiesBound].respawnZombieIndex = (ushort) 0;
      if (!zombie.isDead || (double) Time.realtimeSinceStartup - (double) zombie.lastDead <= (double) ZombieManager.RESPAWN * (!LightingManager.isFullMoon ? 1.0 : (double) ZombieManager.MOON))
        return;
      ZombieSpawnpoint zombieSpawnpoint = LevelZombies.zombies[(int) ZombieManager.respawnZombiesBound][Random.Range(0, LevelZombies.zombies[(int) ZombieManager.respawnZombiesBound].Count)];
      if (!SafezoneManager.checkPointValid(zombieSpawnpoint.point))
        return;
      for (ushort index = (ushort) 0; (int) index < ZombieManager.regions[(int) ZombieManager.respawnZombiesBound].zombies.Count; ++index)
      {
        if (!ZombieManager.regions[(int) ZombieManager.respawnZombiesBound].zombies[(int) index].isDead && (double) (ZombieManager.regions[(int) ZombieManager.respawnZombiesBound].zombies[(int) index].transform.position - zombieSpawnpoint.point).sqrMagnitude < 4.0)
          return;
      }
      ZombieTable zombieTable = LevelZombies.tables[(int) zombieSpawnpoint.type];
      if (zombieTable.isMega && (ZombieManager.regions[(int) ZombieManager.respawnZombiesBound].hasMega || (double) Time.realtimeSinceStartup - (double) ZombieManager.regions[(int) ZombieManager.respawnZombiesBound].lastMega <= 600.0))
        return;
      EZombieSpeciality ezombieSpeciality = EZombieSpeciality.NORMAL;
      if (zombieTable.isMega)
      {
        ZombieManager.regions[(int) ZombieManager.respawnZombiesBound].lastMega = Time.realtimeSinceStartup;
        ZombieManager.regions[(int) ZombieManager.respawnZombiesBound].hasMega = true;
        ezombieSpeciality = EZombieSpeciality.MEGA;
      }
      else if (Provider.mode != EGameMode.EASY && Level.info.type == ELevelType.SURVIVAL)
      {
        float num = Random.value;
        if ((double) num < 0.150000005960464)
          ezombieSpeciality = EZombieSpeciality.CRAWLER;
        else if ((double) num < 0.300000011920929)
          ezombieSpeciality = EZombieSpeciality.SPRINTER;
      }
      byte shirt = byte.MaxValue;
      if (zombieTable.slots[0].table.Count > 0 && (double) Random.value < (double) zombieTable.slots[0].chance)
        shirt = (byte) Random.Range(0, zombieTable.slots[0].table.Count);
      byte pants = byte.MaxValue;
      if (zombieTable.slots[1].table.Count > 0 && (double) Random.value < (double) zombieTable.slots[1].chance)
        pants = (byte) Random.Range(0, zombieTable.slots[1].table.Count);
      byte hat = byte.MaxValue;
      if (zombieTable.slots[2].table.Count > 0 && (double) Random.value < (double) zombieTable.slots[2].chance)
        hat = (byte) Random.Range(0, zombieTable.slots[2].table.Count);
      byte gear = byte.MaxValue;
      if (zombieTable.slots[3].table.Count > 0 && (double) Random.value < (double) zombieTable.slots[3].chance)
        gear = (byte) Random.Range(0, zombieTable.slots[3].table.Count);
      Vector3 point = zombieSpawnpoint.point;
      point.y += 0.1f;
      zombie.sendRevive(zombieSpawnpoint.type, (byte) ezombieSpeciality, shirt, pants, hat, gear, point, Random.Range(0.0f, 360f));
      if (Level.info.type != ELevelType.HORDE)
        return;
      --ZombieManager._waveRemaining;
    }

    private void onBoundUpdated(Player player, byte oldBound, byte newBound)
    {
      if (player.channel.isOwner && LevelNavigation.checkSafe(oldBound) && ZombieManager.regions[(int) oldBound].isNetworked)
      {
        ZombieManager.regions[(int) oldBound].destroy();
        ZombieManager.regions[(int) oldBound].isNetworked = false;
      }
      if (!Provider.isServer)
        return;
      if (LevelNavigation.checkSafe(oldBound) && player.movement.loadedBounds[(int) oldBound].isZombiesLoaded)
        player.movement.loadedBounds[(int) oldBound].isZombiesLoaded = false;
      if (!LevelNavigation.checkSafe(newBound) || player.movement.loadedBounds[(int) newBound].isZombiesLoaded)
        return;
      if (player.channel.isOwner)
      {
        this.generateZombies(newBound);
        ZombieManager.regions[(int) newBound].isNetworked = true;
      }
      else
        this.askZombies(player.channel.owner.playerID.steamID, newBound);
      player.movement.loadedBounds[(int) newBound].isZombiesLoaded = true;
    }

    private void onClientConnected()
    {
      if (Level.info.type != ELevelType.HORDE)
        return;
      this.channel.send("askWave", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER);
    }

    private void onPlayerCreated(Player player)
    {
      player.movement.onBoundUpdated += new PlayerBoundUpdated(this.onBoundUpdated);
    }

    private void onLevelLoaded(int level)
    {
      if (level <= Level.MENU)
        return;
      ZombieManager._regions = new ZombieRegion[LevelNavigation.bounds.Count];
      for (int index = 0; index < ZombieManager.regions.Length; ++index)
        ZombieManager.regions[index] = new ZombieRegion();
      ZombieManager.respawnZombiesBound = (byte) 0;
      ZombieManager._waveReady = false;
      ZombieManager._waveIndex = 0;
      ZombieManager._waveRemaining = 0;
      ZombieManager.onWaveUpdated = (WaveUpdated) null;
      if (Dedicator.isDedicated)
      {
        if (LevelNavigation.bounds.Count == 0 || LevelZombies.zombies.Length == 0 || LevelNavigation.bounds.Count != LevelZombies.zombies.Length)
          return;
        for (byte bound = (byte) 0; (int) bound < LevelNavigation.bounds.Count; ++bound)
          this.generateZombies(bound);
      }
      else
        ZombieClothing.build();
    }

    private void FixedUpdate()
    {
      if (!Level.isLoaded || !LightingManager.isFullMoon && !Dedicator.isDedicated || (LevelNavigation.bounds == null || LevelNavigation.bounds.Count == 0 || (LevelZombies.zombies == null || LevelZombies.zombies.Length == 0)) || (LevelNavigation.bounds.Count != LevelZombies.zombies.Length || ZombieManager.regions == null))
        return;
      for (byte bound = (byte) 0; (int) bound < ZombieManager.regions.Length; ++bound)
      {
        if ((int) ZombieManager.regions[(int) bound].updates > 0)
        {
          this.channel.openWrite();
          this.channel.write((object) bound);
          this.channel.write((object) ZombieManager.regions[(int) bound].updates);
          for (int index = 0; index < ZombieManager.regions[(int) bound].zombies.Count; ++index)
          {
            Zombie zombie = ZombieManager.regions[(int) bound].zombies[index];
            if (zombie.isUpdated)
            {
              zombie.isUpdated = false;
              this.channel.write((object) zombie.id, (object) zombie.transform.position, (object) MeasurementTool.angleToByte(zombie.transform.rotation.eulerAngles.y));
            }
          }
          ZombieManager.regions[(int) bound].updates = (ushort) 0;
          this.channel.closeWrite("tellZombieStates", ESteamCall.OTHERS, bound, ESteamPacket.UPDATE_UNRELIABLE_CHUNK_BUFFER);
        }
      }
      this.respawnZombies();
      ++ZombieManager.respawnZombiesBound;
      if ((int) ZombieManager.respawnZombiesBound < LevelZombies.zombies.Length)
        return;
      ZombieManager.respawnZombiesBound = (byte) 0;
    }

    private void Start()
    {
      ZombieManager.manager = this;
      Level.onLevelLoaded += new LevelLoaded(this.onLevelLoaded);
      Provider.onClientConnected += new Provider.ClientConnected(this.onClientConnected);
      Player.onPlayerCreated += new PlayerCreated(this.onPlayerCreated);
    }
  }
}
