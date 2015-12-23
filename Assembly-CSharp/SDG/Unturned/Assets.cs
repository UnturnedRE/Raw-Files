// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Assets
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace SDG.Unturned
{
  public class Assets : MonoBehaviour
  {
    private static readonly float STEPS = 7f;
    private static Assets asset;
    private static bool hasLoaded;
    private static bool _isLoading;
    public static AssetsRefreshed onAssetsRefreshed;
    private static Dictionary<EAssetType, Dictionary<ushort, Asset>> assets;

    public static bool isLoading
    {
      get
      {
        return Assets._isLoading;
      }
    }

    public static Asset find(EAssetType type, ushort id)
    {
      Asset asset = (Asset) null;
      Assets.assets[type].TryGetValue(id, out asset);
      return asset;
    }

    public static Asset[] find(EAssetType type)
    {
      Asset[] assetArray = new Asset[Assets.assets[type].Values.Count];
      int index = 0;
      using (Dictionary<ushort, Asset>.Enumerator enumerator = Assets.assets[type].GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<ushort, Asset> current = enumerator.Current;
          assetArray[index] = current.Value;
          ++index;
        }
      }
      return assetArray;
    }

    private static void add(EAssetType type, ushort id, Asset asset)
    {
      if (Assets.assets[type].ContainsKey(id))
      {
        Asset asset1;
        Assets.assets[type].TryGetValue(id, out asset1);
        UnityEngine.Debug.LogError((object) ("The ID " + (object) id + " for " + asset.name + " is already taken by " + asset1.name + "!"));
      }
      else
        Assets.assets[type].Add(id, asset);
    }

    public static void refresh()
    {
      Assets.asset.StartCoroutine("init");
    }

    private static void load(string path)
    {
      Assets.load(path, true);
    }

    public static void load(string path, bool usePath)
    {
      string fileName = Path.GetFileName(path);
      if (ReadWrite.fileExists(path + "/" + fileName + ".dat", false, usePath))
      {
        Data data;
        try
        {
          data = ReadWrite.readData(path + "/" + fileName + ".dat", false, usePath);
        }
        catch (Exception ex)
        {
          UnityEngine.Debug.LogError((object) (fileName + ": " + ex.Message));
          return;
        }
        Bundle bundle = new Bundle(path + "/" + fileName + ".unity3d", usePath);
        string str = data.readString("Type");
        ushort id = data.readUInt16("ID");
        EAssetType type = EAssetType.OBJECT;
        Asset asset = (Asset) null;
        try
        {
          string key = str;
          if (key != null)
          {
            // ISSUE: reference to a compiler-generated field
            if (Assets.\u003C\u003Ef__switch\u0024map0 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Assets.\u003C\u003Ef__switch\u0024map0 = new Dictionary<string, int>(42)
              {
                {
                  "Hat",
                  0
                },
                {
                  "Pants",
                  1
                },
                {
                  "Shirt",
                  2
                },
                {
                  "Backpack",
                  3
                },
                {
                  "Vest",
                  4
                },
                {
                  "Mask",
                  5
                },
                {
                  "Glasses",
                  6
                },
                {
                  "Gun",
                  7
                },
                {
                  "Sight",
                  8
                },
                {
                  "Tactical",
                  9
                },
                {
                  "Grip",
                  10
                },
                {
                  "Barrel",
                  11
                },
                {
                  "Magazine",
                  12
                },
                {
                  "Food",
                  13
                },
                {
                  "Water",
                  14
                },
                {
                  "Medical",
                  15
                },
                {
                  "Melee",
                  16
                },
                {
                  "Fuel",
                  17
                },
                {
                  "Tool",
                  18
                },
                {
                  "Barricade",
                  19
                },
                {
                  "Storage",
                  20
                },
                {
                  "Farm",
                  21
                },
                {
                  "Trap",
                  22
                },
                {
                  "Structure",
                  23
                },
                {
                  "Supply",
                  24
                },
                {
                  "Throwable",
                  25
                },
                {
                  "Grower",
                  26
                },
                {
                  "Optic",
                  27
                },
                {
                  "Refill",
                  28
                },
                {
                  "Fisher",
                  29
                },
                {
                  "Cloud",
                  30
                },
                {
                  "Key",
                  31
                },
                {
                  "Box",
                  32
                },
                {
                  "Effect",
                  33
                },
                {
                  "Large",
                  34
                },
                {
                  "Medium",
                  34
                },
                {
                  "Small",
                  34
                },
                {
                  "Resource",
                  35
                },
                {
                  "Vehicle",
                  36
                },
                {
                  "Animal",
                  37
                },
                {
                  "Mythic",
                  38
                },
                {
                  "Skin",
                  39
                }
              };
            }
            int num;
            // ISSUE: reference to a compiler-generated field
            if (Assets.\u003C\u003Ef__switch\u0024map0.TryGetValue(key, out num))
            {
              switch (num)
              {
                case 0:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemHatAsset(bundle, data, id);
                  goto label_52;
                case 1:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemPantsAsset(bundle, data, id);
                  goto label_52;
                case 2:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemShirtAsset(bundle, data, id);
                  goto label_52;
                case 3:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemBackpackAsset(bundle, data, id);
                  goto label_52;
                case 4:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemVestAsset(bundle, data, id);
                  goto label_52;
                case 5:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemMaskAsset(bundle, data, id);
                  goto label_52;
                case 6:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemGlassesAsset(bundle, data, id);
                  goto label_52;
                case 7:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemGunAsset(bundle, data, id);
                  goto label_52;
                case 8:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemSightAsset(bundle, data, id);
                  goto label_52;
                case 9:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemTacticalAsset(bundle, data, id);
                  goto label_52;
                case 10:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemGripAsset(bundle, data, id);
                  goto label_52;
                case 11:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemBarrelAsset(bundle, data, id);
                  goto label_52;
                case 12:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemMagazineAsset(bundle, data, id);
                  goto label_52;
                case 13:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemFoodAsset(bundle, data, id);
                  goto label_52;
                case 14:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemWaterAsset(bundle, data, id);
                  goto label_52;
                case 15:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemMedicalAsset(bundle, data, id);
                  goto label_52;
                case 16:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemMeleeAsset(bundle, data, id);
                  goto label_52;
                case 17:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemFuelAsset(bundle, data, id);
                  goto label_52;
                case 18:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemToolAsset(bundle, data, id);
                  goto label_52;
                case 19:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemBarricadeAsset(bundle, data, id);
                  goto label_52;
                case 20:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemStorageAsset(bundle, data, id);
                  goto label_52;
                case 21:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemFarmAsset(bundle, data, id);
                  goto label_52;
                case 22:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemTrapAsset(bundle, data, id);
                  goto label_52;
                case 23:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemStructureAsset(bundle, data, id);
                  goto label_52;
                case 24:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemSupplyAsset(bundle, data, id);
                  goto label_52;
                case 25:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemThrowableAsset(bundle, data, id);
                  goto label_52;
                case 26:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemGrowerAsset(bundle, data, id);
                  goto label_52;
                case 27:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemOpticAsset(bundle, data, id);
                  goto label_52;
                case 28:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemRefillAsset(bundle, data, id);
                  goto label_52;
                case 29:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemFisherAsset(bundle, data, id);
                  goto label_52;
                case 30:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemCloudAsset(bundle, data, id);
                  goto label_52;
                case 31:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemKeyAsset(bundle, data, id);
                  goto label_52;
                case 32:
                  type = EAssetType.ITEM;
                  asset = (Asset) new ItemBoxAsset(bundle, data, id);
                  goto label_52;
                case 33:
                  type = EAssetType.EFFECT;
                  asset = (Asset) new EffectAsset(bundle, data, id);
                  goto label_52;
                case 34:
                  type = EAssetType.OBJECT;
                  asset = (Asset) new ObjectAsset(bundle, data, id);
                  goto label_52;
                case 35:
                  type = EAssetType.RESOURCE;
                  asset = (Asset) new ResourceAsset(bundle, data, id);
                  goto label_52;
                case 36:
                  type = EAssetType.VEHICLE;
                  asset = (Asset) new VehicleAsset(bundle, data, id);
                  goto label_52;
                case 37:
                  type = EAssetType.ANIMAL;
                  asset = (Asset) new AnimalAsset(bundle, data, id);
                  goto label_52;
                case 38:
                  type = EAssetType.MYTHIC;
                  asset = (Asset) new MythicAsset(bundle, data, id);
                  goto label_52;
                case 39:
                  type = EAssetType.SKIN;
                  asset = (Asset) new SkinAsset(bundle, data, id);
                  goto label_52;
              }
            }
          }
          UnityEngine.Debug.LogError((object) ("Failed to handle asset type: " + str));
label_52:
          if (asset != null)
            Assets.add(type, id, asset);
        }
        catch (Exception ex)
        {
          UnityEngine.Debug.LogError((object) ex.StackTrace);
          UnityEngine.Debug.LogError((object) ("Failed to load asset: " + bundle.name));
        }
      }
      foreach (string path1 in ReadWrite.getFolders(path, usePath))
        Assets.load(path + "/" + Path.GetFileName(path1), usePath);
    }

    [DebuggerHidden]
    public IEnumerator init()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Assets.\u003Cinit\u003Ec__Iterator0 initCIterator0 = new Assets.\u003Cinit\u003Ec__Iterator0();
      return (IEnumerator) initCIterator0;
    }

    private void Start()
    {
      Assets.refresh();
    }

    private void Awake()
    {
      Assets.asset = this;
    }
  }
}
