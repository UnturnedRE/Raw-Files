// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.VehicleManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class VehicleManager : SteamCaller
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 3;
    private static readonly float RESPAWN = 300f;
    private static VehicleManager manager;
    private static List<InteractableVehicle> _vehicles;
    public static ushort updates;
    private static ushort respawnVehicleIndex;

    public static List<InteractableVehicle> vehicles
    {
      get
      {
        return VehicleManager._vehicles;
      }
    }

    public static InteractableVehicle getVehicle(ushort index)
    {
      if ((int) index >= VehicleManager.vehicles.Count)
        return (InteractableVehicle) null;
      return VehicleManager.vehicles[(int) index];
    }

    public static void damage(InteractableVehicle vehicle, float damage, float times, bool canRepair)
    {
      if (vehicle.isDead)
        return;
      ushort amount = (ushort) ((double) damage * (double) times);
      vehicle.askDamage(amount, canRepair);
    }

    public static void repair(InteractableVehicle vehicle, float damage, float times)
    {
      if (vehicle.isExploded || vehicle.isRepaired)
        return;
      ushort amount = (ushort) ((double) damage * (double) times);
      vehicle.askRepair(amount);
    }

    public static void spawnVehicle(ushort id, Vector3 point, Quaternion angle)
    {
      if ((VehicleAsset) Assets.find(EAssetType.VEHICLE, id) == null)
        return;
      VehicleManager.manager.addVehicle(id, point, angle, false, false, false, ushort.MaxValue, false, ushort.MaxValue, (CSteamID[]) null);
      VehicleManager.manager.channel.openWrite();
      VehicleManager.manager.sendVehicle(VehicleManager.vehicles[VehicleManager.vehicles.Count - 1]);
      VehicleManager.manager.channel.closeWrite("tellVehicle", ESteamCall.OTHERS, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
      BarricadeManager.askPlants(VehicleManager.vehicles[VehicleManager.vehicles.Count - 1].transform);
    }

    public static void enterVehicle(Transform vehicle)
    {
      for (ushort index = (ushort) 0; (int) index < VehicleManager.vehicles.Count; ++index)
      {
        if ((Object) vehicle == (Object) VehicleManager.vehicles[(int) index].transform)
        {
          VehicleManager.manager.channel.send("askEnterVehicle", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[1]
          {
            (object) index
          });
          break;
        }
      }
    }

    public static void exitVehicle()
    {
      if (!((Object) Player.player.movement.getVehicle() != (Object) null))
        return;
      VehicleManager.manager.channel.send("askExitVehicle", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[1]
      {
        (object) Player.player.movement.getVehicle().GetComponent<Rigidbody>().velocity
      });
    }

    public static void swapVehicle(byte toSeat)
    {
      if (!((Object) Player.player.movement.getVehicle() != (Object) null))
        return;
      VehicleManager.manager.channel.send("askSwapVehicle", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[1]
      {
        (object) toSeat
      });
    }

    public static void sendVehicleHeadlights()
    {
      if (!((Object) Player.player.movement.getVehicle() != (Object) null))
        return;
      VehicleManager.manager.channel.send("askVehicleHeadlights", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
    }

    public static void sendVehicleSirens()
    {
      if (!((Object) Player.player.movement.getVehicle() != (Object) null))
        return;
      VehicleManager.manager.channel.send("askVehicleSirens", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
    }

    public static void sendVehicleHorn()
    {
      if (!((Object) Player.player.movement.getVehicle() != (Object) null))
        return;
      VehicleManager.manager.channel.send("askVehicleHorn", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
    }

    private void sendVehicle(InteractableVehicle vehicle)
    {
      this.channel.write((object) vehicle.id, (object) vehicle.transform.position, (object) MeasurementTool.angleToByte(vehicle.transform.rotation.eulerAngles.x), (object) MeasurementTool.angleToByte(vehicle.transform.rotation.eulerAngles.y), (object) MeasurementTool.angleToByte(vehicle.transform.rotation.eulerAngles.z), (object) (bool) (vehicle.sirensOn ? 1 : 0), (object) (bool) (vehicle.headlightsOn ? 1 : 0), (object) (bool) (vehicle.taillightsOn ? 1 : 0), (object) vehicle.fuel, (object) (bool) (vehicle.isExploded ? 1 : 0), (object) vehicle.health, (object) (byte) vehicle.passengers.Length);
      for (byte index = (byte) 0; (int) index < vehicle.passengers.Length; ++index)
      {
        Passenger passenger = vehicle.passengers[(int) index];
        if (passenger.player != null)
          this.channel.write((object) passenger.player.playerID.steamID);
        else
          this.channel.write((object) CSteamID.Nil);
      }
    }

    [SteamCall]
    public void tellVehicleSirens(CSteamID steamID, ushort index, bool on)
    {
      if (!this.channel.checkServer(steamID) || (int) index >= VehicleManager.vehicles.Count)
        return;
      VehicleManager.vehicles[(int) index].tellSirens(on);
    }

    [SteamCall]
    public void tellVehicleHeadlights(CSteamID steamID, ushort index, bool on)
    {
      if (!this.channel.checkServer(steamID) || (int) index >= VehicleManager.vehicles.Count)
        return;
      VehicleManager.vehicles[(int) index].tellHeadlights(on);
    }

    [SteamCall]
    public void tellVehicleHorn(CSteamID steamID, ushort index)
    {
      if (!this.channel.checkServer(steamID) || (int) index >= VehicleManager.vehicles.Count)
        return;
      VehicleManager.vehicles[(int) index].tellHorn();
    }

    [SteamCall]
    public void tellVehicleFuel(CSteamID steamID, ushort index, ushort newFuel)
    {
      if (!this.channel.checkServer(steamID) || (int) index >= VehicleManager.vehicles.Count)
        return;
      VehicleManager.vehicles[(int) index].tellFuel(newFuel);
    }

    [SteamCall]
    public void tellVehicleExploded(CSteamID steamID, ushort index)
    {
      if (!this.channel.checkServer(steamID) || (int) index >= VehicleManager.vehicles.Count)
        return;
      if (!VehicleManager.vehicles[(int) index].isExploded)
        BarricadeManager.trimPlant(VehicleManager.vehicles[(int) index].transform);
      VehicleManager.vehicles[(int) index].tellExploded();
    }

    [SteamCall]
    public void tellVehicleHealth(CSteamID steamID, ushort index, ushort newHealth)
    {
      if (!this.channel.checkServer(steamID) || (int) index >= VehicleManager.vehicles.Count)
        return;
      VehicleManager.vehicles[(int) index].tellHealth(newHealth);
    }

    [SteamCall]
    public void tellVehiclePosition(CSteamID steamID, ushort index, Vector3 newPosition)
    {
      if (!this.channel.checkServer(steamID) || (int) index >= VehicleManager.vehicles.Count)
        return;
      VehicleManager.vehicles[(int) index].tellPosition(newPosition);
    }

    [SteamCall]
    public void tellVehicleStates(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID))
        return;
      ushort num = (ushort) this.channel.read(Types.UINT16_TYPE);
      for (int index = 0; index < (int) num; ++index)
      {
        object[] objArray = this.channel.read(Types.UINT16_TYPE, Types.VECTOR3_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE);
        if ((int) (ushort) objArray[0] >= VehicleManager.vehicles.Count)
          break;
        VehicleManager.vehicles[(int) (ushort) objArray[0]].tellState((Vector3) objArray[1], (byte) objArray[2], (byte) objArray[3], (byte) objArray[4], (byte) objArray[5], (byte) objArray[6]);
      }
    }

    [SteamCall]
    public void tellVehicleDestroy(CSteamID steamID, ushort index)
    {
      if (!this.channel.checkServer(steamID) || (int) index >= VehicleManager.vehicles.Count)
        return;
      BarricadeManager.uprootPlant(VehicleManager.vehicles[(int) index].transform);
      Object.Destroy((Object) VehicleManager.vehicles[(int) index].gameObject);
      VehicleManager.vehicles.RemoveAt((int) index);
      --VehicleManager.respawnVehicleIndex;
      for (int index1 = (int) index; index1 < VehicleManager.vehicles.Count; ++index1)
        --VehicleManager.vehicles[index1].index;
    }

    [SteamCall]
    public void tellVehicle(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID))
        return;
      object[] objArray = this.channel.read(Types.UINT16_TYPE, Types.VECTOR3_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BOOLEAN_TYPE, Types.BOOLEAN_TYPE, Types.BOOLEAN_TYPE, Types.UINT16_TYPE, Types.BOOLEAN_TYPE, Types.UINT16_TYPE, Types.BYTE_TYPE);
      CSteamID[] passengers = new CSteamID[(int) (byte) objArray[11]];
      for (int index = 0; index < passengers.Length; ++index)
        passengers[index] = (CSteamID) this.channel.read(Types.STEAM_ID_TYPE);
      this.addVehicle((ushort) objArray[0], (Vector3) objArray[1], Quaternion.Euler((float) ((int) (byte) objArray[2] * 2), (float) ((int) (byte) objArray[3] * 2), (float) ((int) (byte) objArray[4] * 2)), (bool) objArray[5], (bool) objArray[6], (bool) objArray[7], (ushort) objArray[8], (bool) objArray[9], (ushort) objArray[10], passengers);
    }

    [SteamCall]
    public void tellVehicles(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID))
        return;
      ushort num = (ushort) this.channel.read(Types.UINT16_TYPE);
      for (int index = 0; index < (int) num; ++index)
        this.tellVehicle(steamID);
      Level.isLoadingVehicles = false;
    }

    [SteamCall]
    public void askVehicles(CSteamID steamID)
    {
      this.channel.openWrite();
      this.channel.write((object) (ushort) VehicleManager.vehicles.Count);
      for (int index = 0; index < VehicleManager.vehicles.Count; ++index)
        this.sendVehicle(VehicleManager.vehicles[index]);
      this.channel.closeWrite("tellVehicles", steamID, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
      BarricadeManager.askPlants(steamID);
    }

    [SteamCall]
    public void tellEnterVehicle(CSteamID steamID, ushort index, byte seat, CSteamID player)
    {
      if (!this.channel.checkServer(steamID) || (int) index >= VehicleManager.vehicles.Count)
        return;
      VehicleManager.vehicles[(int) index].addPlayer(seat, player);
    }

    [SteamCall]
    public void tellExitVehicle(CSteamID steamID, ushort index, byte seat, Vector3 point, byte angle, bool forceUpdate)
    {
      if (!this.channel.checkServer(steamID) || (int) index >= VehicleManager.vehicles.Count)
        return;
      VehicleManager.vehicles[(int) index].removePlayer(seat, point, angle, forceUpdate);
    }

    [SteamCall]
    public void tellSwapVehicle(CSteamID steamID, ushort index, byte fromSeat, byte toSeat)
    {
      if (!this.channel.checkServer(steamID) || (int) index >= VehicleManager.vehicles.Count)
        return;
      VehicleManager.vehicles[(int) index].swapPlayer(fromSeat, toSeat);
    }

    [SteamCall]
    public void askVehicleHeadlights(CSteamID steamID)
    {
      if (!Provider.isServer)
        return;
      Player player = PlayerTool.getPlayer(steamID);
      if ((Object) player == (Object) null)
        return;
      InteractableVehicle vehicle = player.movement.getVehicle();
      if ((Object) vehicle == (Object) null || !vehicle.checkDriver(steamID) || !vehicle.asset.hasHeadlights)
        return;
      this.channel.send("tellVehicleHeadlights", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[2]
      {
        (object) vehicle.index,
        (object) (bool) (!vehicle.headlightsOn ? 1 : 0)
      });
      EffectManager.sendEffect((ushort) 8, EffectManager.SMALL, vehicle.transform.position);
    }

    [SteamCall]
    public void askVehicleSirens(CSteamID steamID)
    {
      if (!Provider.isServer)
        return;
      Player player = PlayerTool.getPlayer(steamID);
      if ((Object) player == (Object) null)
        return;
      InteractableVehicle vehicle = player.movement.getVehicle();
      if ((Object) vehicle == (Object) null || !vehicle.checkDriver(steamID) || !vehicle.asset.hasSirens)
        return;
      this.channel.send("tellVehicleSirens", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[2]
      {
        (object) vehicle.index,
        (object) (bool) (!vehicle.sirensOn ? 1 : 0)
      });
      EffectManager.sendEffect((ushort) 8, EffectManager.SMALL, vehicle.transform.position);
    }

    [SteamCall]
    public void askVehicleHorn(CSteamID steamID)
    {
      if (!Provider.isServer)
        return;
      Player player = PlayerTool.getPlayer(steamID);
      if ((Object) player == (Object) null)
        return;
      InteractableVehicle vehicle = player.movement.getVehicle();
      if ((Object) vehicle == (Object) null || !vehicle.isHornable || !vehicle.checkDriver(steamID))
        return;
      this.channel.send("tellVehicleHorn", ESteamCall.ALL, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[1]
      {
        (object) vehicle.index
      });
    }

    [SteamCall]
    public void askEnterVehicle(CSteamID steamID, ushort index)
    {
      if (!Provider.isServer)
        return;
      Player player = PlayerTool.getPlayer(steamID);
      if ((Object) player == (Object) null || (int) index >= VehicleManager.vehicles.Count || (Object) player.movement.getVehicle() != (Object) null)
        return;
      InteractableVehicle interactableVehicle = VehicleManager.vehicles[(int) index];
      byte seat;
      if ((double) (interactableVehicle.transform.position - player.transform.position).sqrMagnitude > 40.0 || !interactableVehicle.tryAddPlayer(out seat))
        return;
      this.channel.send("tellEnterVehicle", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[3]
      {
        (object) index,
        (object) seat,
        (object) steamID
      });
    }

    [SteamCall]
    public void askExitVehicle(CSteamID steamID, Vector3 velocity)
    {
      if (!Provider.isServer)
        return;
      Player player = PlayerTool.getPlayer(steamID);
      if ((Object) player == (Object) null)
        return;
      InteractableVehicle vehicle = player.movement.getVehicle();
      byte seat;
      Vector3 point;
      byte angle;
      if ((Object) vehicle == (Object) null || !vehicle.tryRemovePlayer(out seat, steamID, out point, out angle))
        return;
      this.channel.send("tellExitVehicle", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[5]
      {
        (object) vehicle.index,
        (object) seat,
        (object) point,
        (object) angle,
        (object) false
      });
      if ((int) seat != 0 || !Dedicator.isDedicated)
        return;
      vehicle.GetComponent<Rigidbody>().velocity = velocity;
    }

    [SteamCall]
    public void askSwapVehicle(CSteamID steamID, byte toSeat)
    {
      if (!Provider.isServer)
        return;
      Player player = PlayerTool.getPlayer(steamID);
      if ((Object) player == (Object) null)
        return;
      InteractableVehicle vehicle = player.movement.getVehicle();
      byte fromSeat;
      if ((Object) vehicle == (Object) null || !vehicle.trySwapPlayer(steamID, toSeat, out fromSeat))
        return;
      this.channel.send("tellSwapVehicle", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[3]
      {
        (object) vehicle.index,
        (object) fromSeat,
        (object) toSeat
      });
    }

    public static void sendExitVehicle(InteractableVehicle vehicle, byte seat, Vector3 point, byte angle, bool forceUpdate)
    {
      VehicleManager.manager.channel.send("tellExitVehicle", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[5]
      {
        (object) vehicle.index,
        (object) seat,
        (object) point,
        (object) angle,
        (object) (bool) (forceUpdate ? 1 : 0)
      });
    }

    public static void sendVehicleFuel(InteractableVehicle vehicle, ushort newFuel)
    {
      VehicleManager.manager.channel.send("tellVehicleFuel", ESteamCall.CLIENTS, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[2]
      {
        (object) vehicle.index,
        (object) newFuel
      });
    }

    public static void sendVehicleExploded(InteractableVehicle vehicle)
    {
      VehicleManager.manager.channel.send("tellVehicleExploded", ESteamCall.ALL, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[1]
      {
        (object) vehicle.index
      });
    }

    public static void sendVehicleHealth(InteractableVehicle vehicle, ushort newHealth)
    {
      VehicleManager.manager.channel.send("tellVehicleHealth", ESteamCall.ALL, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[2]
      {
        (object) vehicle.index,
        (object) newHealth
      });
    }

    public static void sendVehiclePosition(InteractableVehicle vehicle, Vector3 newPosition)
    {
      if (vehicle.passengers[0].player == null)
        return;
      VehicleManager.manager.channel.send("tellVehiclePosition", vehicle.passengers[0].player.playerID.steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[2]
      {
        (object) vehicle.index,
        (object) newPosition
      });
    }

    private void addVehicle(ushort id, Vector3 point, Quaternion angle, bool sirens, bool headlights, bool taillights, ushort fuel, bool isExploded, ushort health, CSteamID[] passengers)
    {
      VehicleAsset vehicleAsset = (VehicleAsset) Assets.find(EAssetType.VEHICLE, id);
      if (vehicleAsset == null)
        return;
      Transform parent = !Dedicator.isDedicated || !((Object) vehicleAsset.clip != (Object) null) ? Object.Instantiate<GameObject>(vehicleAsset.vehicle).transform : Object.Instantiate<GameObject>(vehicleAsset.clip).transform;
      parent.name = id.ToString();
      parent.parent = LevelVehicles.models;
      parent.position = point;
      parent.rotation = angle;
      parent.GetComponent<Rigidbody>().useGravity = true;
      parent.GetComponent<Rigidbody>().isKinematic = false;
      InteractableVehicle interactableVehicle = parent.gameObject.AddComponent<InteractableVehicle>();
      interactableVehicle.index = (ushort) VehicleManager.vehicles.Count;
      interactableVehicle.id = id;
      interactableVehicle.fuel = fuel;
      interactableVehicle.isExploded = isExploded;
      interactableVehicle.health = health;
      interactableVehicle.init();
      interactableVehicle.tellSirens(sirens);
      interactableVehicle.tellHeadlights(headlights);
      interactableVehicle.tellTaillights(taillights);
      if (passengers != null)
      {
        for (byte seat = (byte) 0; (int) seat < passengers.Length; ++seat)
        {
          if (passengers[(int) seat] != CSteamID.Nil)
            interactableVehicle.addPlayer(seat, passengers[(int) seat]);
        }
      }
      VehicleManager.vehicles.Add(interactableVehicle);
      BarricadeManager.waterPlant(parent);
    }

    private void respawnVehicles()
    {
      if ((int) VehicleManager.respawnVehicleIndex >= VehicleManager.vehicles.Count)
        VehicleManager.respawnVehicleIndex = (ushort) (VehicleManager.vehicles.Count - 1);
      InteractableVehicle interactableVehicle = VehicleManager.vehicles[(int) VehicleManager.respawnVehicleIndex];
      ++VehicleManager.respawnVehicleIndex;
      if ((int) VehicleManager.respawnVehicleIndex >= VehicleManager.vehicles.Count)
        VehicleManager.respawnVehicleIndex = (ushort) 0;
      if ((!interactableVehicle.isExploded || (double) Time.realtimeSinceStartup - (double) interactableVehicle.lastExploded <= (double) VehicleManager.RESPAWN) && (!interactableVehicle.isDrowned || (double) Time.realtimeSinceStartup - (double) interactableVehicle.lastUnderwater <= (double) VehicleManager.RESPAWN) || !interactableVehicle.isEmpty)
        return;
      VehicleSpawnpoint spawn = (VehicleSpawnpoint) null;
      if (VehicleManager.vehicles.Count < (int) Level.vehicles)
      {
        spawn = LevelVehicles.spawns[Random.Range(0, LevelVehicles.spawns.Count)];
        for (ushort index = (ushort) 0; (int) index < VehicleManager.vehicles.Count; ++index)
        {
          if ((double) (VehicleManager.vehicles[(int) index].transform.position - spawn.point).sqrMagnitude < 64.0)
            return;
        }
      }
      this.channel.send("tellVehicleDestroy", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) interactableVehicle.index
      });
      if (spawn == null)
        return;
      Vector3 point = spawn.point;
      ++point.y;
      ushort vehicle = LevelVehicles.getVehicle(spawn);
      if ((int) vehicle == 0)
        return;
      this.addVehicle(vehicle, point, Quaternion.Euler(0.0f, spawn.angle, 0.0f), false, false, false, ushort.MaxValue, false, ushort.MaxValue, (CSteamID[]) null);
      VehicleManager.manager.channel.openWrite();
      VehicleManager.manager.sendVehicle(VehicleManager.vehicles[VehicleManager.vehicles.Count - 1]);
      VehicleManager.manager.channel.closeWrite("tellVehicle", ESteamCall.OTHERS, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
    }

    private void onLevelLoaded(int level)
    {
      if (level <= Level.SETUP)
        return;
      VehicleManager._vehicles = new List<InteractableVehicle>();
      VehicleManager.updates = (ushort) 0;
      VehicleManager.respawnVehicleIndex = (ushort) 0;
      BarricadeManager.clearPlants();
      if (!Provider.isServer)
        return;
      VehicleManager.load();
      if (LevelVehicles.spawns.Count <= 0)
        return;
      List<VehicleSpawnpoint> list = new List<VehicleSpawnpoint>();
      for (int index = 0; index < LevelVehicles.spawns.Count; ++index)
        list.Add(LevelVehicles.spawns[index]);
      while (VehicleManager.vehicles.Count < (int) Level.vehicles && list.Count > 0)
      {
        int index1 = Random.Range(0, list.Count);
        VehicleSpawnpoint spawn = list[index1];
        list.RemoveAt(index1);
        bool flag = true;
        for (ushort index2 = (ushort) 0; (int) index2 < VehicleManager.vehicles.Count; ++index2)
        {
          if ((double) (VehicleManager.vehicles[(int) index2].transform.position - spawn.point).sqrMagnitude < 64.0)
          {
            flag = false;
            break;
          }
        }
        if (flag)
        {
          Vector3 point = spawn.point;
          ++point.y;
          ushort vehicle = LevelVehicles.getVehicle(spawn);
          if ((int) vehicle != 0)
            this.addVehicle(vehicle, point, Quaternion.Euler(0.0f, spawn.angle, 0.0f), false, false, false, ushort.MaxValue, false, ushort.MaxValue, (CSteamID[]) null);
        }
      }
    }

    private void onClientConnected()
    {
      this.channel.send("askVehicles", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER);
    }

    private void onServerDisconnected(CSteamID player)
    {
      if (!Provider.isServer)
        return;
      for (ushort index = (ushort) 0; (int) index < VehicleManager.vehicles.Count; ++index)
      {
        byte seat;
        Vector3 point;
        byte angle;
        if (VehicleManager.vehicles[(int) index].tryRemovePlayer(out seat, player, out point, out angle))
          this.channel.send("tellExitVehicle", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[5]
          {
            (object) index,
            (object) seat,
            (object) point,
            (object) angle,
            (object) true
          });
      }
    }

    private void FixedUpdate()
    {
      if (!Provider.isServer || !Level.isLoaded || (LevelVehicles.spawns == null || LevelVehicles.spawns.Count == 0) || (VehicleManager.vehicles == null || VehicleManager.vehicles.Count == 0))
        return;
      if ((int) VehicleManager.updates > 0)
      {
        this.channel.openWrite();
        this.channel.write((object) VehicleManager.updates);
        for (int index = 0; index < VehicleManager.vehicles.Count; ++index)
        {
          InteractableVehicle interactableVehicle = VehicleManager.vehicles[index];
          if (interactableVehicle.isUpdated)
          {
            interactableVehicle.isUpdated = false;
            this.channel.write((object) interactableVehicle.index, (object) interactableVehicle.transform.position, (object) MeasurementTool.angleToByte(interactableVehicle.transform.rotation.eulerAngles.x), (object) MeasurementTool.angleToByte(interactableVehicle.transform.rotation.eulerAngles.y), (object) MeasurementTool.angleToByte(interactableVehicle.transform.rotation.eulerAngles.z), (object) (byte) ((double) Mathf.Clamp(interactableVehicle.speed, -100f, 100f) + 128.0), (object) (byte) (interactableVehicle.turn + 1));
          }
        }
        VehicleManager.updates = (ushort) 0;
        this.channel.closeWrite("tellVehicleStates", ESteamCall.OTHERS, ESteamPacket.UPDATE_UNRELIABLE_CHUNK_BUFFER);
      }
      this.respawnVehicles();
    }

    private void Start()
    {
      VehicleManager.manager = this;
      Level.onPrePreLevelLoaded += new PrePreLevelLoaded(this.onLevelLoaded);
      Provider.onClientConnected += new Provider.ClientConnected(this.onClientConnected);
      Provider.onServerDisconnected += new Provider.ServerDisconnected(this.onServerDisconnected);
    }

    public static void load()
    {
      if (LevelSavedata.fileExists("/Vehicles.dat") && Level.info.type == ELevelType.SURVIVAL)
      {
        River river = LevelSavedata.openRiver("/Vehicles.dat", true);
        byte num1 = river.readByte();
        if ((int) num1 > 2)
        {
          ushort num2 = river.readUInt16();
          for (ushort index = (ushort) 0; (int) index < (int) num2; ++index)
          {
            ushort id = river.readUInt16();
            Vector3 point = river.readSingleVector3();
            Quaternion angle = river.readSingleQuaternion();
            ushort fuel = river.readUInt16();
            ushort health = river.readUInt16();
            ++point.y;
            VehicleManager.manager.addVehicle(id, point, angle, false, false, false, fuel, false, health, (CSteamID[]) null);
          }
        }
        else
        {
          ushort num2 = river.readUInt16();
          for (ushort index = (ushort) 0; (int) index < (int) num2; ++index)
          {
            river.readUInt16();
            river.readColor();
            Vector3 point = river.readSingleVector3();
            Quaternion angle = river.readSingleQuaternion();
            ushort fuel = river.readUInt16();
            ushort health = ushort.MaxValue;
            ushort id = (ushort) Random.Range(1, 51);
            if ((int) num1 > 1)
              health = river.readUInt16();
            ++point.y;
            VehicleManager.manager.addVehicle(id, point, angle, false, false, false, fuel, false, health, (CSteamID[]) null);
          }
        }
      }
      Level.isLoadingVehicles = false;
    }

    public static void save()
    {
      River river = LevelSavedata.openRiver("/Vehicles.dat", false);
      river.writeByte(VehicleManager.SAVEDATA_VERSION);
      ushort num = (ushort) 0;
      for (ushort index = (ushort) 0; (int) index < VehicleManager.vehicles.Count; ++index)
      {
        InteractableVehicle interactableVehicle = VehicleManager.vehicles[(int) index];
        if (!interactableVehicle.isExploded && !interactableVehicle.isUnderwater)
          ++num;
      }
      river.writeUInt16(num);
      for (ushort index = (ushort) 0; (int) index < VehicleManager.vehicles.Count; ++index)
      {
        InteractableVehicle interactableVehicle = VehicleManager.vehicles[(int) index];
        if (!interactableVehicle.isExploded && !interactableVehicle.isUnderwater)
        {
          river.writeUInt16(interactableVehicle.id);
          river.writeSingleVector3(interactableVehicle.transform.position);
          river.writeSingleQuaternion(interactableVehicle.transform.rotation);
          river.writeUInt16(interactableVehicle.fuel);
          river.writeUInt16(interactableVehicle.health);
        }
      }
      river.closeRiver();
    }
  }
}
