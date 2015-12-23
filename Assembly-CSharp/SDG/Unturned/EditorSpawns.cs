// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorSpawns
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorSpawns : MonoBehaviour
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 1;
    public static readonly byte MIN_REMOVE_SIZE = (byte) 2;
    public static readonly byte MAX_REMOVE_SIZE = (byte) 30;
    private static bool _isSpawning;
    public static byte selectedResource;
    public static byte selectedItem;
    public static byte selectedZombie;
    public static byte selectedVehicle;
    public static byte selectedAnimal;
    private static Transform _itemSpawn;
    private static Transform _playerSpawn;
    private static Transform _zombieSpawn;
    private static Transform _vehicleSpawn;
    private static Transform _animalSpawn;
    private static Transform _remove;
    private static float _rotation;
    private static byte _radius;
    private static ESpawnMode _spawnMode;

    public static bool isSpawning
    {
      get
      {
        return EditorSpawns._isSpawning;
      }
      set
      {
        EditorSpawns._isSpawning = value;
        if (EditorSpawns.isSpawning)
          return;
        EditorSpawns.itemSpawn.gameObject.SetActive(false);
        EditorSpawns.playerSpawn.gameObject.SetActive(false);
        EditorSpawns.zombieSpawn.gameObject.SetActive(false);
        EditorSpawns.vehicleSpawn.gameObject.SetActive(false);
        EditorSpawns.animalSpawn.gameObject.SetActive(false);
        EditorSpawns.remove.gameObject.SetActive(false);
      }
    }

    public static Transform itemSpawn
    {
      get
      {
        return EditorSpawns._itemSpawn;
      }
    }

    public static Transform playerSpawn
    {
      get
      {
        return EditorSpawns._playerSpawn;
      }
    }

    public static Transform zombieSpawn
    {
      get
      {
        return EditorSpawns._zombieSpawn;
      }
    }

    public static Transform vehicleSpawn
    {
      get
      {
        return EditorSpawns._vehicleSpawn;
      }
    }

    public static Transform animalSpawn
    {
      get
      {
        return EditorSpawns._animalSpawn;
      }
    }

    public static Transform remove
    {
      get
      {
        return EditorSpawns._remove;
      }
    }

    public static float rotation
    {
      get
      {
        return EditorSpawns._rotation;
      }
      set
      {
        EditorSpawns._rotation = value;
        if ((Object) EditorSpawns.playerSpawn != (Object) null)
          EditorSpawns.playerSpawn.transform.rotation = Quaternion.Euler(0.0f, EditorSpawns.rotation, 0.0f);
        if (!((Object) EditorSpawns.vehicleSpawn != (Object) null))
          return;
        EditorSpawns.vehicleSpawn.transform.rotation = Quaternion.Euler(0.0f, EditorSpawns.rotation, 0.0f);
      }
    }

    public static byte radius
    {
      get
      {
        return EditorSpawns._radius;
      }
      set
      {
        EditorSpawns._radius = value;
        if (!((Object) EditorSpawns.remove != (Object) null))
          return;
        EditorSpawns.remove.localScale = new Vector3((float) ((int) EditorSpawns.radius * 2), (float) ((int) EditorSpawns.radius * 2), (float) ((int) EditorSpawns.radius * 2));
      }
    }

    public static ESpawnMode spawnMode
    {
      get
      {
        return EditorSpawns._spawnMode;
      }
      set
      {
        EditorSpawns._spawnMode = value;
        EditorSpawns.itemSpawn.gameObject.SetActive(EditorSpawns.spawnMode == ESpawnMode.ADD_ITEM && EditorSpawns.isSpawning);
        EditorSpawns.playerSpawn.gameObject.SetActive(EditorSpawns.spawnMode == ESpawnMode.ADD_PLAYER && EditorSpawns.isSpawning);
        EditorSpawns.zombieSpawn.gameObject.SetActive(EditorSpawns.spawnMode == ESpawnMode.ADD_ZOMBIE && EditorSpawns.isSpawning);
        EditorSpawns.vehicleSpawn.gameObject.SetActive(EditorSpawns.spawnMode == ESpawnMode.ADD_VEHICLE && EditorSpawns.isSpawning);
        EditorSpawns.animalSpawn.gameObject.SetActive(EditorSpawns.spawnMode == ESpawnMode.ADD_ANIMAL && EditorSpawns.isSpawning);
        EditorSpawns.remove.gameObject.SetActive((EditorSpawns.spawnMode == ESpawnMode.REMOVE_RESOURCE || EditorSpawns.spawnMode == ESpawnMode.REMOVE_ITEM || (EditorSpawns.spawnMode == ESpawnMode.REMOVE_PLAYER || EditorSpawns.spawnMode == ESpawnMode.REMOVE_ZOMBIE) || (EditorSpawns.spawnMode == ESpawnMode.REMOVE_VEHICLE || EditorSpawns.spawnMode == ESpawnMode.REMOVE_ANIMAL)) && EditorSpawns.isSpawning);
      }
    }

    private void Update()
    {
      if (!EditorSpawns.isSpawning || EditorInteract.isFlying || GUIUtility.hotControl != 0)
        return;
      if (Input.GetKeyDown(ControlsSettings.tool_0))
      {
        if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_RESOURCE)
          EditorSpawns.spawnMode = ESpawnMode.ADD_RESOURCE;
        else if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_ITEM)
          EditorSpawns.spawnMode = ESpawnMode.ADD_ITEM;
        else if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_PLAYER)
          EditorSpawns.spawnMode = ESpawnMode.ADD_PLAYER;
        else if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_ZOMBIE)
          EditorSpawns.spawnMode = ESpawnMode.ADD_ZOMBIE;
        else if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_VEHICLE)
          EditorSpawns.spawnMode = ESpawnMode.ADD_VEHICLE;
        else if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_ANIMAL)
          EditorSpawns.spawnMode = ESpawnMode.ADD_ANIMAL;
      }
      if (Input.GetKeyDown(ControlsSettings.tool_1))
      {
        if (EditorSpawns.spawnMode == ESpawnMode.ADD_RESOURCE)
          EditorSpawns.spawnMode = ESpawnMode.REMOVE_RESOURCE;
        else if (EditorSpawns.spawnMode == ESpawnMode.ADD_ITEM)
          EditorSpawns.spawnMode = ESpawnMode.REMOVE_ITEM;
        else if (EditorSpawns.spawnMode == ESpawnMode.ADD_PLAYER)
          EditorSpawns.spawnMode = ESpawnMode.REMOVE_PLAYER;
        else if (EditorSpawns.spawnMode == ESpawnMode.ADD_ZOMBIE)
          EditorSpawns.spawnMode = ESpawnMode.REMOVE_ZOMBIE;
        else if (EditorSpawns.spawnMode == ESpawnMode.ADD_VEHICLE)
          EditorSpawns.spawnMode = ESpawnMode.REMOVE_VEHICLE;
        else if (EditorSpawns.spawnMode == ESpawnMode.ADD_ANIMAL)
          EditorSpawns.spawnMode = ESpawnMode.REMOVE_ANIMAL;
      }
      if ((Object) EditorInteract.worldHit.transform != (Object) null)
      {
        if (EditorSpawns.spawnMode == ESpawnMode.ADD_ITEM)
          EditorSpawns.itemSpawn.position = EditorInteract.worldHit.point;
        else if (EditorSpawns.spawnMode == ESpawnMode.ADD_PLAYER)
          EditorSpawns.playerSpawn.position = EditorInteract.worldHit.point;
        else if (EditorSpawns.spawnMode == ESpawnMode.ADD_ZOMBIE)
          EditorSpawns.zombieSpawn.position = EditorInteract.worldHit.point + Vector3.up;
        else if (EditorSpawns.spawnMode == ESpawnMode.ADD_VEHICLE)
          EditorSpawns.vehicleSpawn.position = EditorInteract.worldHit.point;
        else if (EditorSpawns.spawnMode == ESpawnMode.ADD_ANIMAL)
          EditorSpawns.animalSpawn.position = EditorInteract.worldHit.point;
        else if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_RESOURCE || EditorSpawns.spawnMode == ESpawnMode.REMOVE_ITEM || (EditorSpawns.spawnMode == ESpawnMode.REMOVE_PLAYER || EditorSpawns.spawnMode == ESpawnMode.REMOVE_ZOMBIE) || (EditorSpawns.spawnMode == ESpawnMode.REMOVE_VEHICLE || EditorSpawns.spawnMode == ESpawnMode.REMOVE_ANIMAL))
          EditorSpawns.remove.position = EditorInteract.worldHit.point;
      }
      if (!Input.GetKeyDown(ControlsSettings.primary) || !((Object) EditorInteract.worldHit.transform != (Object) null))
        return;
      Vector3 point = EditorInteract.worldHit.point;
      if (EditorSpawns.spawnMode == ESpawnMode.ADD_RESOURCE)
      {
        if (!Level.checkLevel(point) || (int) EditorSpawns.selectedResource >= LevelGround.resources.Length)
          return;
        LevelGround.addSpawn(point);
      }
      else if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_RESOURCE)
        LevelGround.removeSpawn(point, (float) EditorSpawns.radius);
      else if (EditorSpawns.spawnMode == ESpawnMode.ADD_ITEM)
      {
        if (!Level.checkSafe(point) || (int) EditorSpawns.selectedItem >= LevelItems.tables.Count)
          return;
        LevelItems.addSpawn(point);
      }
      else if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_ITEM)
        LevelItems.removeSpawn(point, (float) EditorSpawns.radius);
      else if (EditorSpawns.spawnMode == ESpawnMode.ADD_PLAYER)
      {
        if (!Level.checkSafe(point))
          return;
        LevelPlayers.addSpawn(point, EditorSpawns.rotation);
      }
      else if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_PLAYER)
        LevelPlayers.removeSpawn(point, (float) EditorSpawns.radius);
      else if (EditorSpawns.spawnMode == ESpawnMode.ADD_ZOMBIE)
      {
        if (!Level.checkSafe(point) || (int) EditorSpawns.selectedZombie >= LevelZombies.tables.Count)
          return;
        LevelZombies.addSpawn(point);
      }
      else if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_ZOMBIE)
        LevelZombies.removeSpawn(point, (float) EditorSpawns.radius);
      else if (EditorSpawns.spawnMode == ESpawnMode.ADD_VEHICLE)
      {
        if (!Level.checkSafe(point))
          return;
        LevelVehicles.addSpawn(point, EditorSpawns.rotation);
      }
      else if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_VEHICLE)
        LevelVehicles.removeSpawn(point, (float) EditorSpawns.radius);
      else if (EditorSpawns.spawnMode == ESpawnMode.ADD_ANIMAL)
      {
        if (!Level.checkSafe(point))
          return;
        LevelAnimals.addSpawn(point);
      }
      else
      {
        if (EditorSpawns.spawnMode != ESpawnMode.REMOVE_ANIMAL)
          return;
        LevelAnimals.removeSpawn(point, (float) EditorSpawns.radius);
      }
    }

    private void Start()
    {
      EditorSpawns._isSpawning = false;
      EditorSpawns._itemSpawn = ((GameObject) Object.Instantiate(Resources.Load("Edit/Item"))).transform;
      EditorSpawns.itemSpawn.name = "Item Spawn";
      EditorSpawns.itemSpawn.parent = Level.editing;
      EditorSpawns.itemSpawn.gameObject.SetActive(false);
      if ((int) EditorSpawns.selectedItem < LevelItems.tables.Count)
        EditorSpawns.itemSpawn.GetComponent<Renderer>().material.color = LevelItems.tables[(int) EditorSpawns.selectedItem].color;
      EditorSpawns._playerSpawn = ((GameObject) Object.Instantiate(Resources.Load("Edit/Player"))).transform;
      EditorSpawns.playerSpawn.name = "Player Spawn";
      EditorSpawns.playerSpawn.parent = Level.editing;
      EditorSpawns.playerSpawn.gameObject.SetActive(false);
      EditorSpawns._zombieSpawn = ((GameObject) Object.Instantiate(Resources.Load("Edit/Zombie"))).transform;
      EditorSpawns.zombieSpawn.name = "Zombie Spawn";
      EditorSpawns.zombieSpawn.parent = Level.editing;
      EditorSpawns.zombieSpawn.gameObject.SetActive(false);
      if ((int) EditorSpawns.selectedZombie < LevelZombies.tables.Count)
        EditorSpawns.zombieSpawn.GetComponent<Renderer>().material.color = LevelZombies.tables[(int) EditorSpawns.selectedZombie].color;
      EditorSpawns._vehicleSpawn = ((GameObject) Object.Instantiate(Resources.Load("Edit/Vehicle"))).transform;
      EditorSpawns.vehicleSpawn.name = "Vehicle Spawn";
      EditorSpawns.vehicleSpawn.parent = Level.editing;
      EditorSpawns.vehicleSpawn.gameObject.SetActive(false);
      if ((int) EditorSpawns.selectedVehicle < LevelVehicles.tables.Count)
      {
        EditorSpawns.vehicleSpawn.GetComponent<Renderer>().material.color = LevelVehicles.tables[(int) EditorSpawns.selectedVehicle].color;
        EditorSpawns.vehicleSpawn.FindChild("Arrow").GetComponent<Renderer>().material.color = LevelVehicles.tables[(int) EditorSpawns.selectedVehicle].color;
      }
      EditorSpawns._animalSpawn = ((GameObject) Object.Instantiate(Resources.Load("Edit/Animal"))).transform;
      EditorSpawns._animalSpawn.name = "Animal Spawn";
      EditorSpawns._animalSpawn.parent = Level.editing;
      EditorSpawns._animalSpawn.gameObject.SetActive(false);
      if ((int) EditorSpawns.selectedAnimal < LevelAnimals.tables.Count)
        EditorSpawns.animalSpawn.GetComponent<Renderer>().material.color = LevelAnimals.tables[(int) EditorSpawns.selectedAnimal].color;
      EditorSpawns._remove = ((GameObject) Object.Instantiate(Resources.Load("Edit/Remove"))).transform;
      EditorSpawns.remove.name = "Remove";
      EditorSpawns.remove.parent = Level.editing;
      EditorSpawns.remove.gameObject.SetActive(false);
      EditorSpawns.spawnMode = ESpawnMode.ADD_ITEM;
      EditorSpawns.load();
    }

    public static void load()
    {
      if (ReadWrite.fileExists(Level.info.path + "/Editor/Spawns.dat", false, false))
      {
        Block block = ReadWrite.readBlock(Level.info.path + "/Editor/Spawns.dat", false, false, (byte) 1);
        EditorSpawns.rotation = block.readSingle();
        EditorSpawns.radius = block.readByte();
      }
      else
      {
        EditorSpawns.rotation = 0.0f;
        EditorSpawns.radius = EditorSpawns.MIN_REMOVE_SIZE;
      }
    }

    public static void save()
    {
      Block block = new Block();
      block.writeByte(EditorSpawns.SAVEDATA_VERSION);
      block.writeSingle(EditorSpawns.rotation);
      block.writeByte(EditorSpawns.radius);
      ReadWrite.writeBlock(Level.info.path + "/Editor/Spawns.dat", false, false, block);
    }
  }
}
