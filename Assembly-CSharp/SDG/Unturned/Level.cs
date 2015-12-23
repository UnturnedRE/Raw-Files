// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Level
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace SDG.Unturned
{
  public class Level : MonoBehaviour
  {
    private static readonly float STEPS = 12f;
    public static readonly int MENU = 1;
    public static readonly float HEIGHT = 512f;
    public static readonly float TERRAIN = 256f;
    public static readonly ushort CLIP = (ushort) 8;
    public static readonly ushort TINY_BORDER = (ushort) 16;
    public static readonly ushort SMALL_BORDER = (ushort) 64;
    public static readonly ushort MEDIUM_BORDER = (ushort) 64;
    public static readonly ushort LARGE_BORDER = (ushort) 64;
    public static readonly ushort INSANE_BORDER = (ushort) 128;
    public static readonly ushort TINY_SIZE = (ushort) 512;
    public static readonly ushort SMALL_SIZE = (ushort) 1024;
    public static readonly ushort MEDIUM_SIZE = (ushort) 2048;
    public static readonly ushort LARGE_SIZE = (ushort) 4096;
    public static readonly ushort INSANE_SIZE = (ushort) 8192;
    public static readonly ushort TINY_VEHICLES = (ushort) 4;
    public static readonly ushort SMALL_VEHICLES = (ushort) 8;
    public static readonly ushort MEDIUM_VEHICLES = (ushort) 24;
    public static readonly ushort LARGE_VEHICLES = (ushort) 32;
    public static readonly ushort INSANE_VEHICLES = (ushort) 64;
    public static readonly ushort TINY_ANIMALS = (ushort) 4;
    public static readonly ushort SMALL_ANIMALS = (ushort) 8;
    public static readonly ushort MEDIUM_ANIMALS = (ushort) 24;
    public static readonly ushort LARGE_ANIMALS = (ushort) 32;
    public static readonly ushort INSANE_ANIMALS = (ushort) 64;
    public static readonly byte SAVEDATA_VERSION = (byte) 2;
    public static readonly int SETUP;
    public static PrePreLevelLoaded onPrePreLevelLoaded;
    public static PreLevelLoaded onPreLevelLoaded;
    public static LevelLoaded onLevelLoaded;
    public static PostLevelLoaded onPostLevelLoaded;
    public static LevelsRefreshed onLevelsRefreshed;
    private static LevelInfo _info;
    private static Transform mapper;
    private static Transform _level;
    private static Transform _clips;
    private static Transform _effects;
    private static Transform _spawns;
    private static Transform _editing;
    private static bool _isInitialized;
    private static bool _isEditor;
    public static bool isLoadingContent;
    public static bool isLoadingLighting;
    public static bool isLoadingVehicles;
    public static bool isLoadingBarricades;
    public static bool isLoadingStructures;
    public static bool isLoadingArea;
    private static bool _isLoaded;
    private static byte[] _hash;

    public static ushort border
    {
      get
      {
        if (Level.info.size == ELevelSize.TINY)
          return Level.TINY_BORDER;
        if (Level.info.size == ELevelSize.SMALL)
          return Level.SMALL_BORDER;
        if (Level.info.size == ELevelSize.MEDIUM)
          return Level.MEDIUM_BORDER;
        if (Level.info.size == ELevelSize.LARGE)
          return Level.LARGE_BORDER;
        if (Level.info.size == ELevelSize.INSANE)
          return Level.INSANE_BORDER;
        return (ushort) 0;
      }
    }

    public static ushort size
    {
      get
      {
        if (Level.info.size == ELevelSize.TINY)
          return Level.TINY_SIZE;
        if (Level.info.size == ELevelSize.SMALL)
          return Level.SMALL_SIZE;
        if (Level.info.size == ELevelSize.MEDIUM)
          return Level.MEDIUM_SIZE;
        if (Level.info.size == ELevelSize.LARGE)
          return Level.LARGE_SIZE;
        if (Level.info.size == ELevelSize.INSANE)
          return Level.INSANE_SIZE;
        return (ushort) 0;
      }
    }

    public static ushort vehicles
    {
      get
      {
        if (Level.info.size == ELevelSize.TINY)
          return Level.TINY_VEHICLES;
        if (Level.info.size == ELevelSize.SMALL)
          return Level.SMALL_VEHICLES;
        if (Level.info.size == ELevelSize.MEDIUM)
          return Level.MEDIUM_VEHICLES;
        if (Level.info.size == ELevelSize.LARGE)
          return Level.LARGE_VEHICLES;
        if (Level.info.size == ELevelSize.INSANE)
          return Level.INSANE_VEHICLES;
        return (ushort) 0;
      }
    }

    public static ushort animals
    {
      get
      {
        if (Level.info.size == ELevelSize.TINY)
          return Level.TINY_ANIMALS;
        if (Level.info.size == ELevelSize.SMALL)
          return Level.SMALL_ANIMALS;
        if (Level.info.size == ELevelSize.MEDIUM)
          return Level.MEDIUM_ANIMALS;
        if (Level.info.size == ELevelSize.LARGE)
          return Level.LARGE_ANIMALS;
        if (Level.info.size == ELevelSize.INSANE)
          return Level.INSANE_ANIMALS;
        return (ushort) 0;
      }
    }

    public static LevelInfo info
    {
      get
      {
        return Level._info;
      }
    }

    public static Transform level
    {
      get
      {
        return Level._level;
      }
    }

    public static Transform clips
    {
      get
      {
        return Level._clips;
      }
    }

    public static Transform effects
    {
      get
      {
        return Level._effects;
      }
    }

    public static Transform spawns
    {
      get
      {
        return Level._spawns;
      }
    }

    public static Transform editing
    {
      get
      {
        return Level._editing;
      }
    }

    public static bool isInitialized
    {
      get
      {
        return Level._isInitialized;
      }
    }

    public static bool isEditor
    {
      get
      {
        return Level._isEditor;
      }
    }

    public static bool isLoading
    {
      get
      {
        if (!Level.isLoadingContent && !Level.isLoadingLighting && (!Level.isLoadingVehicles && !Level.isLoadingBarricades) && !Level.isLoadingStructures)
          return Level.isLoadingArea;
        return true;
      }
    }

    public static bool isLoaded
    {
      get
      {
        return Level._isLoaded;
      }
    }

    public static byte[] hash
    {
      get
      {
        return Level._hash;
      }
    }

    public static bool checkSafe(Vector3 point)
    {
      if ((double) point.x > (double) ((int) -Level.size / 2 + (int) Level.border) && (double) point.y > 0.0 && ((double) point.z > (double) ((int) -Level.size / 2 + (int) Level.border) && (double) point.x < (double) ((int) Level.size / 2 - (int) Level.border)) && (double) point.y < (double) Level.HEIGHT)
        return (double) point.z < (double) ((int) Level.size / 2 - (int) Level.border);
      return false;
    }

    public static bool checkLevel(Vector3 point)
    {
      if ((double) point.x > (double) ((int) -Level.size / 2) && (double) point.y > 0.0 && ((double) point.z > (double) ((int) -Level.size / 2) && (double) point.x < (double) ((int) Level.size / 2)) && (double) point.y < (double) Level.HEIGHT)
        return (double) point.z < (double) ((int) Level.size / 2);
      return false;
    }

    public static void setEnabled(bool isEnabled)
    {
      Level.clips.gameObject.SetActive(isEnabled);
    }

    public static void add(string name, ELevelSize size, ELevelType type)
    {
      if (ReadWrite.folderExists("/Maps/" + name))
        return;
      ReadWrite.createFolder("/Maps/" + name);
      Block block = new Block();
      block.writeByte(Level.SAVEDATA_VERSION);
      block.writeSteamID(Provider.client);
      block.writeByte((byte) size);
      block.writeByte((byte) type);
      ReadWrite.writeBlock("/Maps/" + name + "/Level.dat", false, block);
      ReadWrite.copyFile("/Bundles/Level/Details.unity3d", "/Maps/" + name + "/Terrain/Details.unity3d");
      ReadWrite.copyFile("/Bundles/Level/Details.dat", "/Maps/" + name + "/Terrain/Details.dat");
      ReadWrite.copyFile("/Bundles/Level/Materials.unity3d", "/Maps/" + name + "/Terrain/Materials.unity3d");
      ReadWrite.copyFile("/Bundles/Level/Materials.dat", "/Maps/" + name + "/Terrain/Materials.dat");
      ReadWrite.copyFile("/Bundles/Level/Resources.dat", "/Maps/" + name + "/Terrain/Resources.dat");
      ReadWrite.copyFile("/Bundles/Level/Lighting.dat", "/Maps/" + name + "/Environment/Lighting.dat");
      ReadWrite.copyFile("/Bundles/Level/Roads.unity3d", "/Maps/" + name + "/Environment/Roads.unity3d");
      ReadWrite.copyFile("/Bundles/Level/Roads.dat", "/Maps/" + name + "/Environment/Roads.dat");
      ReadWrite.copyFile("/Bundles/Level/Ambience.unity3d", "/Maps/" + name + "/Environment/Ambience.unity3d");
      if (Level.onLevelsRefreshed == null)
        return;
      Level.onLevelsRefreshed();
    }

    public static void remove(string name)
    {
      ReadWrite.deleteFolder("/Maps/" + name);
      if (Level.onLevelsRefreshed == null)
        return;
      Level.onLevelsRefreshed();
    }

    public static void save()
    {
      LevelObjects.save();
      LevelLighting.save();
      LevelGround.save();
      LevelRoads.save();
      LevelNavigation.save();
      LevelNodes.save();
      LevelItems.save();
      LevelPlayers.save();
      LevelZombies.save();
      LevelVehicles.save();
      LevelAnimals.save();
      LevelVisibility.save();
      Editor.save();
    }

    public static void edit(LevelInfo newInfo)
    {
      Level._isEditor = true;
      Level._info = newInfo;
      LoadingUI.updateScene();
      Application.LoadLevel("Game");
      if (Dedicator.isDedicated)
        return;
      Provider.provider.communityService.setStatus("Editing: " + Level.info.name);
    }

    public static void load(LevelInfo newInfo)
    {
      Level._isEditor = false;
      Level._info = newInfo;
      LoadingUI.updateScene();
      Application.LoadLevel("Game");
      if (!Dedicator.isDedicated)
      {
        string key = Level.info.name.ToLower();
        if (key != null)
        {
          // ISSUE: reference to a compiler-generated field
          if (Level.\u003C\u003Ef__switch\u0024map1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Level.\u003C\u003Ef__switch\u0024map1 = new Dictionary<string, int>(3)
            {
              {
                "pei",
                0
              },
              {
                "yukon",
                1
              },
              {
                "washington",
                2
              }
            };
          }
          int num;
          // ISSUE: reference to a compiler-generated field
          if (Level.\u003C\u003Ef__switch\u0024map1.TryGetValue(key, out num))
          {
            switch (num)
            {
              case 0:
                Provider.provider.achievementsService.setAchievement("PEI");
                break;
              case 1:
                Provider.provider.achievementsService.setAchievement("Yukon");
                break;
              case 2:
                Provider.provider.achievementsService.setAchievement("Washington");
                break;
            }
          }
        }
      }
      if (Dedicator.isDedicated)
        return;
      Provider.provider.communityService.setStatus("Playing: " + Level.info.name);
    }

    public static void loading()
    {
      Application.LoadLevel("Loading");
    }

    public static void exit()
    {
      Level._isEditor = false;
      Level._info = (LevelInfo) null;
      LoadingUI.updateScene();
      Application.LoadLevel("Menu");
      if (Dedicator.isDedicated)
        return;
      Provider.provider.communityService.setStatus("Menu");
    }

    public static bool exists(string name)
    {
      if (ReadWrite.folderExists("/Maps/" + name))
        return true;
      if (Provider.provider.workshopService.ugc != null)
      {
        for (int index = 0; index < Provider.provider.workshopService.ugc.Count; ++index)
        {
          SteamContent steamContent = Provider.provider.workshopService.ugc[index];
          if (steamContent.type == ESteamUGCType.MAP && ReadWrite.folderExists(steamContent.path + "/" + name, false))
            return true;
        }
      }
      else
      {
        foreach (string str in ReadWrite.getFolders("/Bundles/Workshop/Maps"))
        {
          if (ReadWrite.folderExists(str + "/" + name, false))
            return true;
        }
        foreach (string str in ReadWrite.getFolders(ServerSavedata.directory + "/" + Provider.serverID + "/Workshop/Maps"))
        {
          if (ReadWrite.folderExists(str + "/" + name, false))
            return true;
        }
      }
      return false;
    }

    public static byte[] getLevelHash(string path)
    {
      return Level.getLevelHash(path, true);
    }

    public static byte[] getLevelHash(string path, bool usePath)
    {
      if (ReadWrite.fileExists(path + "/Level.dat", false, usePath))
        return ReadWrite.readBlock(path + "/Level.dat", false, usePath, (byte) 1).getHash();
      return new byte[20];
    }

    public static LevelInfo getLevel(string name)
    {
      if (ReadWrite.folderExists("/Maps/" + name))
        return Level.getLevel("/Maps/" + name, true);
      if (Provider.provider.workshopService.ugc != null)
      {
        for (int index = 0; index < Provider.provider.workshopService.ugc.Count; ++index)
        {
          SteamContent steamContent = Provider.provider.workshopService.ugc[index];
          if (steamContent.type == ESteamUGCType.MAP && ReadWrite.folderExists(steamContent.path + "/" + name, false))
            return Level.getLevel(steamContent.path + "/" + name, false);
        }
      }
      else
      {
        string[] folders1 = ReadWrite.getFolders("/Bundles/Workshop/Maps");
        for (int index = 0; index < folders1.Length; ++index)
        {
          if (ReadWrite.folderExists(folders1[index] + "/" + name, false))
            return Level.getLevel(folders1[index] + "/" + name, false);
        }
        string[] folders2 = ReadWrite.getFolders(ServerSavedata.directory + "/" + Provider.serverID + "/Workshop/Maps");
        for (int index = 0; index < folders2.Length; ++index)
        {
          if (ReadWrite.folderExists(folders2[index] + "/" + name, false))
            return Level.getLevel(folders2[index] + "/" + name, false);
        }
      }
      return (LevelInfo) null;
    }

    public static LevelInfo getLevel(string path, bool usePath)
    {
      if (!ReadWrite.fileExists(path + "/Level.dat", false, usePath))
        return (LevelInfo) null;
      Block block = ReadWrite.readBlock(path + "/Level.dat", false, usePath, (byte) 0);
      byte num = block.readByte();
      bool newEditable = block.readSteamID() == Provider.client || ReadWrite.fileExists(path + "/.unlocker", false, usePath);
      ELevelSize newSize = (ELevelSize) block.readByte();
      ELevelType newType = ELevelType.SURVIVAL;
      if ((int) num > 1)
        newType = (ELevelType) block.readByte();
      return new LevelInfo(!usePath ? path : ReadWrite.PATH + path, ReadWrite.folderName(path), newSize, newType, newEditable);
    }

    public static LevelInfo[] getLevels()
    {
      List<LevelInfo> list = new List<LevelInfo>();
      foreach (string path in ReadWrite.getFolders("/Maps"))
      {
        LevelInfo level = Level.getLevel(path, false);
        if (level != null)
          list.Add(level);
      }
      if (Provider.provider.workshopService.ugc != null)
      {
        for (int index = 0; index < Provider.provider.workshopService.ugc.Count; ++index)
        {
          SteamContent steamContent = Provider.provider.workshopService.ugc[index];
          if (steamContent.type == ESteamUGCType.MAP)
            list.Add(Level.getLevel(ReadWrite.folderFound(steamContent.path, false), false));
        }
      }
      return list.ToArray();
    }

    public static void mapify()
    {
      RenderTexture temporary = RenderTexture.GetTemporary((int) Level.size / 2, (int) Level.size / 2, 32);
      temporary.name = "Texture";
      Level.mapper.GetComponent<Camera>().targetTexture = temporary;
      bool fog = RenderSettings.fog;
      Color ambientSkyColor = RenderSettings.ambientSkyColor;
      Color ambientEquatorColor = RenderSettings.ambientEquatorColor;
      Color ambientGroundColor = RenderSettings.ambientGroundColor;
      float heightmapPixelError1 = LevelGround.terrain.heightmapPixelError;
      float heightmapPixelError2 = LevelGround.terrain2.heightmapPixelError;
      float basemapDistance = LevelGround.terrain.basemapDistance;
      float lodBias = QualitySettings.lodBias;
      float @float = LevelLighting.sea.GetFloat("_Shininess");
      Color color = LevelLighting.sea.GetColor("_SpecularColor");
      RenderSettings.fog = false;
      RenderSettings.ambientSkyColor = Palette.AMBIENT;
      RenderSettings.ambientEquatorColor = Palette.AMBIENT;
      RenderSettings.ambientGroundColor = Palette.AMBIENT;
      LevelGround.terrain.heightmapPixelError = 1f;
      LevelGround.terrain2.heightmapPixelError = 1f;
      LevelGround.terrain.basemapDistance = 8192f;
      LevelLighting.sea.SetFloat("_Shininess", 500f);
      LevelLighting.sea.SetColor("_SpecularColor", Color.black);
      QualitySettings.lodBias = float.MaxValue;
      for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
        {
          if (!LevelObjects.regions[(int) index1, (int) index2])
          {
            List<LevelObject> list = LevelObjects.objects[(int) index1, (int) index2];
            for (int index3 = 0; index3 < list.Count; ++index3)
              list[index3].enable();
          }
          if (!LevelGround.regions[(int) index1, (int) index2])
          {
            List<ResourceSpawnpoint> list = LevelGround.trees[(int) index1, (int) index2];
            for (int index3 = 0; index3 < list.Count; ++index3)
              list[index3].enable();
          }
        }
      }
      Level.mapper.GetComponent<Camera>().Render();
      for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
        {
          if (!LevelObjects.regions[(int) index1, (int) index2])
          {
            List<LevelObject> list = LevelObjects.objects[(int) index1, (int) index2];
            for (int index3 = 0; index3 < list.Count; ++index3)
              list[index3].disable();
          }
          if (!LevelGround.regions[(int) index1, (int) index2])
          {
            List<ResourceSpawnpoint> list = LevelGround.trees[(int) index1, (int) index2];
            for (int index3 = 0; index3 < list.Count; ++index3)
              list[index3].disable();
          }
        }
      }
      RenderSettings.fog = fog;
      RenderSettings.ambientSkyColor = ambientSkyColor;
      RenderSettings.ambientEquatorColor = ambientEquatorColor;
      RenderSettings.ambientGroundColor = ambientGroundColor;
      LevelGround.terrain.heightmapPixelError = heightmapPixelError1;
      LevelGround.terrain2.heightmapPixelError = heightmapPixelError2;
      LevelGround.terrain.basemapDistance = basemapDistance;
      LevelLighting.sea.SetFloat("_Shininess", @float);
      LevelLighting.sea.SetColor("_SpecularColor", color);
      QualitySettings.lodBias = lodBias;
      RenderTexture.active = temporary;
      Texture2D texture2D = new Texture2D(temporary.width, temporary.height);
      texture2D.name = "Texture";
      texture2D.hideFlags = HideFlags.HideAndDontSave;
      texture2D.filterMode = FilterMode.Trilinear;
      texture2D.ReadPixels(new Rect(0.0f, 0.0f, (float) temporary.width, (float) temporary.height), 0, 0);
      for (int x = 0; x < texture2D.width; ++x)
      {
        for (int y = 0; y < texture2D.height; ++y)
        {
          Color pixel = texture2D.GetPixel(x, y);
          if ((double) pixel.a < 1.0)
          {
            pixel.a = 1f;
            texture2D.SetPixel(x, y, pixel);
          }
        }
      }
      texture2D.Apply();
      ReadWrite.writeBytes(Level.info.path + "/Map.png", false, false, texture2D.EncodeToPNG());
    }

    [DebuggerHidden]
    public IEnumerator init(int id)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Level.\u003Cinit\u003Ec__Iterator1()
      {
        id = id,
        \u003C\u0024\u003Eid = id
      };
    }

    private void Awake()
    {
      if (Level.isInitialized)
      {
        Object.Destroy((Object) this.gameObject);
      }
      else
      {
        Level._isInitialized = true;
        Object.DontDestroyOnLoad((Object) this.gameObject);
      }
    }

    private void OnLevelWasLoaded(int id)
    {
      if (id == 3)
        return;
      if (id > Level.SETUP && Level.info != null)
      {
        if (Level.isEditor && !Level.info.isEditable)
          Application.Quit();
        if (Provider.isConnected)
        {
          Level.isLoadingLighting = true;
          Level.isLoadingVehicles = true;
          Level.isLoadingBarricades = true;
          Level.isLoadingStructures = true;
        }
        else
        {
          Level.isLoadingLighting = false;
          Level.isLoadingVehicles = false;
          Level.isLoadingBarricades = false;
          Level.isLoadingStructures = false;
        }
        Level.isLoadingContent = true;
        Level.isLoadingArea = true;
        Level._level = new GameObject().transform;
        Level.level.name = Level.info.name;
        Level.level.tag = "Logic";
        Level.level.gameObject.layer = LayerMasks.LOGIC;
        Level._clips = new GameObject().transform;
        Level.clips.name = "Clips";
        Level.clips.parent = Level.level;
        Level.clips.tag = "Clip";
        Level.clips.gameObject.layer = LayerMasks.CLIP;
        Transform transform1 = ((GameObject) Object.Instantiate(Resources.Load("Level/Cap"))).transform;
        transform1.position = new Vector3(0.0f, -4f, 0.0f);
        transform1.localScale = new Vector3((float) ((int) Level.size - (int) Level.border * 2 + (int) Level.CLIP * 2), (float) ((int) Level.size - (int) Level.border * 2 + (int) Level.CLIP * 2), 1f);
        transform1.rotation = Quaternion.Euler(-90f, 0.0f, 0.0f);
        transform1.name = "Cap";
        transform1.parent = Level.clips;
        Transform transform2 = ((GameObject) Object.Instantiate(Resources.Load("Level/Cap"))).transform;
        transform2.position = new Vector3(0.0f, Level.HEIGHT + 4f, 0.0f);
        transform2.localScale = new Vector3((float) ((int) Level.size - (int) Level.border * 2 + (int) Level.CLIP * 2), (float) ((int) Level.size - (int) Level.border * 2 + (int) Level.CLIP * 2), 1f);
        transform2.rotation = Quaternion.Euler(90f, 0.0f, 0.0f);
        transform2.name = "Cap";
        transform2.parent = Level.clips;
        Transform transform3 = ((GameObject) Object.Instantiate(Resources.Load("Level/Cap"))).transform;
        transform3.position = new Vector3((float) ((int) Level.size / 2 - (int) Level.border), Level.HEIGHT / 2f, (float) ((int) Level.size / 2 - (int) Level.border));
        transform3.localScale = new Vector3((float) ((int) Level.CLIP * 4), (float) ((int) Level.CLIP * 4), 64f);
        transform3.rotation = Quaternion.Euler(90f, 0.0f, 45f);
        transform3.name = "Cap";
        transform3.parent = Level.clips;
        Transform transform4 = ((GameObject) Object.Instantiate(Resources.Load("Level/Cap"))).transform;
        transform4.position = new Vector3((float) ((int) -Level.size / 2 + (int) Level.border), Level.HEIGHT / 2f, (float) ((int) Level.size / 2 - (int) Level.border));
        transform4.localScale = new Vector3((float) ((int) Level.CLIP * 4), (float) ((int) Level.CLIP * 4), 64f);
        transform4.rotation = Quaternion.Euler(90f, 0.0f, 45f);
        transform4.name = "Cap";
        transform4.parent = Level.clips;
        Transform transform5 = ((GameObject) Object.Instantiate(Resources.Load("Level/Cap"))).transform;
        transform5.position = new Vector3((float) ((int) Level.size / 2 - (int) Level.border), Level.HEIGHT / 2f, (float) ((int) -Level.size / 2 + (int) Level.border));
        transform5.localScale = new Vector3((float) ((int) Level.CLIP * 4), (float) ((int) Level.CLIP * 4), 64f);
        transform5.rotation = Quaternion.Euler(90f, 0.0f, 45f);
        transform5.name = "Cap";
        transform5.parent = Level.clips;
        Transform transform6 = ((GameObject) Object.Instantiate(Resources.Load("Level/Cap"))).transform;
        transform6.position = new Vector3((float) ((int) -Level.size / 2 + (int) Level.border), Level.HEIGHT / 2f, (float) ((int) -Level.size / 2 + (int) Level.border));
        transform6.localScale = new Vector3((float) ((int) Level.CLIP * 4), (float) ((int) Level.CLIP * 4), 64f);
        transform6.rotation = Quaternion.Euler(90f, 0.0f, 45f);
        transform6.name = "Cap";
        transform6.parent = Level.clips;
        Transform transform7 = ((GameObject) Object.Instantiate(Resources.Load(!Level.isEditor ? "Level/Clip" : "Level/Wall"))).transform;
        transform7.position = new Vector3((float) ((int) Level.size / 2 - (int) Level.border), Level.HEIGHT / 8f, 0.0f);
        transform7.localScale = new Vector3((float) ((int) Level.size - (int) Level.border * 2), Level.HEIGHT / 4f, 1f);
        transform7.rotation = Quaternion.Euler(0.0f, -90f, 0.0f);
        transform7.name = "Clip";
        transform7.parent = Level.clips;
        if (Level.isEditor)
          transform7.GetComponent<Renderer>().material.mainTextureScale = new Vector2((float) ((int) Level.size - (int) Level.border * 2) / 32f, 4f);
        Transform transform8 = ((GameObject) Object.Instantiate(Resources.Load(!Level.isEditor ? "Level/Clip" : "Level/Wall"))).transform;
        transform8.position = new Vector3((float) ((int) -Level.size / 2 + (int) Level.border), Level.HEIGHT / 8f, 0.0f);
        transform8.localScale = new Vector3((float) ((int) Level.size - (int) Level.border * 2), Level.HEIGHT / 4f, 1f);
        transform8.rotation = Quaternion.Euler(0.0f, 90f, 0.0f);
        transform8.name = "Clip";
        transform8.parent = Level.clips;
        if (Level.isEditor)
          transform8.GetComponent<Renderer>().material.mainTextureScale = new Vector2((float) ((int) Level.size - (int) Level.border * 2) / 32f, 4f);
        Transform transform9 = ((GameObject) Object.Instantiate(Resources.Load(!Level.isEditor ? "Level/Clip" : "Level/Wall"))).transform;
        transform9.position = new Vector3(0.0f, Level.HEIGHT / 8f, (float) ((int) Level.size / 2 - (int) Level.border));
        transform9.localScale = new Vector3((float) ((int) Level.size - (int) Level.border * 2), Level.HEIGHT / 4f, 1f);
        transform9.rotation = Quaternion.Euler(0.0f, 180f, 0.0f);
        transform9.name = "Clip";
        transform9.parent = Level.clips;
        if (Level.isEditor)
          transform9.GetComponent<Renderer>().material.mainTextureScale = new Vector2((float) ((int) Level.size - (int) Level.border * 2) / 32f, 4f);
        Transform transform10 = ((GameObject) Object.Instantiate(Resources.Load(!Level.isEditor ? "Level/Clip" : "Level/Wall"))).transform;
        transform10.position = new Vector3(0.0f, Level.HEIGHT / 8f, (float) ((int) -Level.size / 2 + (int) Level.border));
        transform10.localScale = new Vector3((float) ((int) Level.size - (int) Level.border * 2), Level.HEIGHT / 4f, 1f);
        transform10.rotation = Quaternion.identity;
        transform10.name = "Clip";
        transform10.parent = Level.clips;
        if (!Level.isEditor && (Level.info.name.ToLower() == "pei" || Level.level.name.ToLower() == "yukon" || Level.level.name.ToLower() == "washington"))
        {
          Transform transform11 = (Transform) null;
          string key = Level.info.name.ToLower();
          if (key != null)
          {
            // ISSUE: reference to a compiler-generated field
            if (Level.\u003C\u003Ef__switch\u0024map2 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Level.\u003C\u003Ef__switch\u0024map2 = new Dictionary<string, int>(1)
              {
                {
                  "pei",
                  0
                }
              };
            }
            int num;
            // ISSUE: reference to a compiler-generated field
            if (Level.\u003C\u003Ef__switch\u0024map2.TryGetValue(key, out num) && num == 0)
              transform11 = ((GameObject) Object.Instantiate(Resources.Load("Level/Triggers_PEI"))).transform;
          }
          if ((Object) transform11 != (Object) null)
          {
            transform11.position = Vector3.zero;
            transform11.rotation = Quaternion.identity;
            transform11.name = "Triggers";
            transform11.parent = Level.clips;
          }
        }
        if (Level.isEditor)
          transform10.GetComponent<Renderer>().material.mainTextureScale = new Vector2((float) ((int) Level.size - (int) Level.border * 2) / 32f, 4f);
        Level._effects = new GameObject().transform;
        Level.effects.name = "Effects";
        Level.effects.parent = Level.level;
        Level.effects.tag = "Logic";
        Level.effects.gameObject.layer = LayerMasks.LOGIC;
        Level._spawns = new GameObject().transform;
        Level.spawns.name = "Spawns";
        Level.spawns.parent = Level.level;
        Level.spawns.tag = "Logic";
        Level.spawns.gameObject.layer = LayerMasks.LOGIC;
        this.StartCoroutine("init", (object) id);
      }
      else
      {
        Level.isLoadingLighting = false;
        Level.isLoadingVehicles = false;
        Level.isLoadingBarricades = false;
        Level.isLoadingStructures = false;
        Level.isLoadingContent = false;
        Level.isLoadingArea = false;
        Level._isLoaded = false;
        if (Level.onLevelLoaded != null)
          Level.onLevelLoaded(id);
      }
      Resources.UnloadUnusedAssets();
    }
  }
}
