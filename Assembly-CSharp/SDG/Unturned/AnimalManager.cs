// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.AnimalManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class AnimalManager : SteamCaller
  {
    private static readonly float RESPAWN = 180f;
    private static AnimalManager manager;
    private static List<Animal> _animals;
    public static ushort updates;
    private static ushort respawnAnimalIndex;

    public static List<Animal> animals
    {
      get
      {
        return AnimalManager._animals;
      }
    }

    [SteamCall]
    public void tellAnimalAlive(CSteamID steamID, ushort index, Vector3 newPosition, byte newAngle)
    {
      if (!this.channel.checkServer(steamID) || (int) index >= AnimalManager.animals.Count)
        return;
      AnimalManager.animals[(int) index].tellAlive(newPosition, newAngle);
    }

    [SteamCall]
    public void tellAnimalDead(CSteamID steamID, ushort index, Vector3 newRagdoll)
    {
      if (!this.channel.checkServer(steamID) || (int) index >= AnimalManager.animals.Count)
        return;
      AnimalManager.animals[(int) index].tellDead(newRagdoll);
    }

    [SteamCall]
    public void tellAnimalStates(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID))
        return;
      ushort num = (ushort) this.channel.read(Types.UINT16_TYPE);
      for (int index = 0; index < (int) num; ++index)
      {
        object[] objArray = this.channel.read(Types.UINT16_TYPE, Types.VECTOR3_TYPE, Types.BYTE_TYPE);
        if ((int) (ushort) objArray[0] >= AnimalManager.animals.Count)
          break;
        AnimalManager.animals[(int) (ushort) objArray[0]].tellState((Vector3) objArray[1], (byte) objArray[2]);
      }
    }

    [SteamCall]
    public void askAnimalStartle(CSteamID steamID, ushort index)
    {
      if (!this.channel.checkServer(steamID) || (int) index >= AnimalManager.animals.Count)
        return;
      AnimalManager.animals[(int) index].askStartle();
    }

    [SteamCall]
    public void askAnimalAttack(CSteamID steamID, ushort index)
    {
      if (!this.channel.checkServer(steamID) || (int) index >= AnimalManager.animals.Count)
        return;
      AnimalManager.animals[(int) index].askAttack();
    }

    [SteamCall]
    public void tellAnimals(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID))
        return;
      ushort num = (ushort) this.channel.read(Types.UINT16_TYPE);
      for (int index = 0; index < (int) num; ++index)
      {
        object[] objArray = this.channel.read(Types.UINT16_TYPE, Types.VECTOR3_TYPE, Types.BYTE_TYPE, Types.BOOLEAN_TYPE);
        this.addAnimal((ushort) objArray[0], (Vector3) objArray[1], (float) ((int) (byte) objArray[2] * 2), (bool) objArray[3]);
      }
    }

    [SteamCall]
    public void askAnimals(CSteamID steamID)
    {
      this.channel.openWrite();
      this.channel.write((object) (ushort) AnimalManager.animals.Count);
      for (int index = 0; index < AnimalManager.animals.Count; ++index)
      {
        Animal animal = AnimalManager.animals[index];
        this.channel.write((object) animal.id, (object) animal.transform.position, (object) MeasurementTool.angleToByte(animal.transform.rotation.eulerAngles.y), (object) (bool) (animal.isDead ? 1 : 0));
      }
      this.channel.closeWrite("tellAnimals", steamID, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
    }

    public static void sendAnimalAlive(Animal animal, Vector3 newPosition, byte newAngle)
    {
      AnimalManager.manager.channel.send("tellAnimalAlive", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[3]
      {
        (object) animal.index,
        (object) newPosition,
        (object) newAngle
      });
    }

    public static void sendAnimalDead(Animal animal, Vector3 newRagdoll)
    {
      AnimalManager.manager.channel.send("tellAnimalDead", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[2]
      {
        (object) animal.index,
        (object) newRagdoll
      });
    }

    public static void sendAnimalStartle(Animal animal)
    {
      AnimalManager.manager.channel.send("askAnimalStartle", ESteamCall.ALL, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[1]
      {
        (object) animal.index
      });
    }

    public static void sendAnimalAttack(Animal animal)
    {
      AnimalManager.manager.channel.send("askAnimalAttack", ESteamCall.ALL, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[1]
      {
        (object) animal.index
      });
    }

    public static void dropLoot(Animal animal)
    {
      if ((int) animal.asset.meat != 0)
      {
        int num = Random.Range(2, 5);
        for (int index = 0; index < num; ++index)
          ItemManager.dropItem(new Item(animal.asset.meat, true), animal.transform.position, false, Dedicator.isDedicated, true);
      }
      if ((int) animal.asset.pelt == 0)
        return;
      int num1 = Random.Range(2, 5);
      for (int index = 0; index < num1; ++index)
        ItemManager.dropItem(new Item(animal.asset.pelt, true), animal.transform.position, false, Dedicator.isDedicated, true);
    }

    private void addAnimal(ushort id, Vector3 point, float angle, bool isDead)
    {
      AnimalAsset animalAsset = (AnimalAsset) Assets.find(EAssetType.ANIMAL, id);
      if (animalAsset == null)
        return;
      Transform transform = !Dedicator.isDedicated ? (!Provider.isServer ? Object.Instantiate<GameObject>(animalAsset.client).transform : Object.Instantiate<GameObject>(animalAsset.server).transform) : Object.Instantiate<GameObject>(animalAsset.dedicated).transform;
      transform.name = id.ToString();
      transform.parent = LevelAnimals.models;
      transform.position = point;
      transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
      Animal animal = transform.gameObject.AddComponent<Animal>();
      animal.index = (ushort) AnimalManager.animals.Count;
      animal.id = id;
      animal.isDead = isDead;
      AnimalManager.animals.Add(animal);
    }

    public static Animal getAnimal(ushort index)
    {
      if ((int) index >= AnimalManager.animals.Count)
        return (Animal) null;
      return AnimalManager.animals[(int) index];
    }

    private void respawnAnimals()
    {
      if ((int) AnimalManager.respawnAnimalIndex >= AnimalManager.animals.Count)
        AnimalManager.respawnAnimalIndex = (ushort) (AnimalManager.animals.Count - 1);
      Animal animal = AnimalManager.animals[(int) AnimalManager.respawnAnimalIndex];
      ++AnimalManager.respawnAnimalIndex;
      if ((int) AnimalManager.respawnAnimalIndex >= AnimalManager.animals.Count)
        AnimalManager.respawnAnimalIndex = (ushort) 0;
      if (!animal.isDead || (double) Time.realtimeSinceStartup - (double) animal.lastDead <= (double) AnimalManager.RESPAWN)
        return;
      AnimalSpawnpoint animalSpawnpoint = LevelAnimals.spawns[Random.Range(0, LevelAnimals.spawns.Count)];
      for (ushort index = (ushort) 0; (int) index < AnimalManager.animals.Count; ++index)
      {
        if (!AnimalManager.animals[(int) index].isDead && (double) (AnimalManager.animals[(int) index].transform.position - animalSpawnpoint.point).sqrMagnitude < 4.0)
          return;
      }
      Vector3 point = animalSpawnpoint.point;
      point.y += 0.1f;
      animal.sendRevive(point, Random.Range(0.0f, 360f));
    }

    private void onLevelLoaded(int level)
    {
      if (level <= Level.SETUP)
        return;
      AnimalManager._animals = new List<Animal>();
      AnimalManager.updates = (ushort) 0;
      if (!Provider.isServer || LevelAnimals.spawns.Count <= 0)
        return;
      List<AnimalSpawnpoint> list = new List<AnimalSpawnpoint>();
      for (int index = 0; index < LevelAnimals.spawns.Count; ++index)
        list.Add(LevelAnimals.spawns[index]);
      while (AnimalManager.animals.Count < (int) Level.animals && list.Count > 0)
      {
        int index = Random.Range(0, list.Count);
        AnimalSpawnpoint spawn = list[index];
        list.RemoveAt(index);
        Vector3 point = spawn.point;
        point.y += 0.1f;
        ushort animal = LevelAnimals.getAnimal(spawn);
        if ((int) animal != 0)
          this.addAnimal(animal, point, Random.Range(0.0f, 360f), false);
      }
    }

    private void onClientConnected()
    {
      this.channel.send("askAnimals", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER);
    }

    private void FixedUpdate()
    {
      if (!Provider.isServer || !Level.isLoaded || (LevelAnimals.spawns == null || LevelAnimals.spawns.Count == 0) || (AnimalManager.animals == null || AnimalManager.animals.Count == 0))
        return;
      if ((int) AnimalManager.updates > 0)
      {
        this.channel.openWrite();
        this.channel.write((object) AnimalManager.updates);
        for (int index = 0; index < AnimalManager.animals.Count; ++index)
        {
          Animal animal = AnimalManager.animals[index];
          if (animal.isUpdated)
          {
            animal.isUpdated = false;
            this.channel.write((object) animal.index, (object) animal.transform.position, (object) MeasurementTool.angleToByte(animal.transform.rotation.eulerAngles.y));
          }
        }
        AnimalManager.updates = (ushort) 0;
        this.channel.closeWrite("tellAnimalStates", ESteamCall.OTHERS, ESteamPacket.UPDATE_UNRELIABLE_CHUNK_BUFFER);
      }
      this.respawnAnimals();
    }

    private void Start()
    {
      AnimalManager.manager = this;
      Level.onLevelLoaded += new LevelLoaded(this.onLevelLoaded);
      Provider.onClientConnected += new Provider.ClientConnected(this.onClientConnected);
    }
  }
}
