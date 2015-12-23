// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ZombieClothing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class ZombieClothing
  {
    private static Texture2D megaTexture;
    private static Texture2D zombieTexture;
    private static Texture2D anomalyTexture;
    private static Shader shader;
    private static Material[][,] clothes;

    public static Material paint(ushort shirt, ushort pants, bool isMega)
    {
      Texture2D texture2D = Object.Instantiate<Texture2D>(!isMega ? ZombieClothing.zombieTexture : ZombieClothing.megaTexture);
      if ((int) shirt != 0)
      {
        ItemShirtAsset itemShirtAsset = (ItemShirtAsset) Assets.find(EAssetType.ITEM, shirt);
        if (itemShirtAsset != null)
        {
          for (int x = 0; x < itemShirtAsset.shirt.width; ++x)
          {
            for (int y = 0; y < itemShirtAsset.shirt.height; ++y)
            {
              if ((double) itemShirtAsset.shirt.GetPixel(x, y).a > 0.0)
                texture2D.SetPixel(x, y, itemShirtAsset.shirt.GetPixel(x, y));
            }
          }
        }
      }
      if ((int) pants != 0)
      {
        ItemPantsAsset itemPantsAsset = (ItemPantsAsset) Assets.find(EAssetType.ITEM, pants);
        if (itemPantsAsset != null)
        {
          for (int x = 0; x < itemPantsAsset.pants.width; ++x)
          {
            for (int y = 0; y < itemPantsAsset.pants.height; ++y)
            {
              if ((double) itemPantsAsset.pants.GetPixel(x, y).a > 0.0)
                texture2D.SetPixel(x, y, itemPantsAsset.pants.GetPixel(x, y));
            }
          }
        }
      }
      texture2D.Apply();
      Material material = new Material(ZombieClothing.shader);
      material.name = "Material";
      material.hideFlags = HideFlags.HideAndDontSave;
      material.mainTexture = (Texture) texture2D;
      material.SetTexture("_EmissionMap", (Texture) ZombieClothing.anomalyTexture);
      material.SetFloat("_Glossiness", 0.0f);
      return material;
    }

    public static void apply(Transform zombie, Renderer renderer_0, Renderer renderer_1, byte type, byte shirt, byte pants, byte hat, byte gear)
    {
      Transform child1 = zombie.FindChild("Skeleton").FindChild("Spine");
      Transform child2 = child1.FindChild("Skull");
      ZombieTable zombieTable = LevelZombies.tables[(int) type];
      if ((int) shirt == (int) byte.MaxValue)
        shirt = (byte) zombieTable.slots[0].table.Count;
      if ((int) pants == (int) byte.MaxValue)
        pants = (byte) zombieTable.slots[1].table.Count;
      Material material = ZombieClothing.clothes[(int) type][(int) shirt, (int) pants];
      if ((Object) renderer_0 != (Object) null)
        renderer_0.material = material;
      if ((Object) renderer_1 != (Object) null)
        renderer_1.material = material;
      Transform child3 = child2.FindChild("Hat");
      if ((Object) child3 != (Object) null)
        Object.Destroy((Object) child3.gameObject);
      if ((int) hat != (int) byte.MaxValue)
      {
        ItemHatAsset itemHatAsset = (ItemHatAsset) Assets.find(EAssetType.ITEM, zombieTable.slots[2].table[(int) hat].item);
        if (itemHatAsset != null)
        {
          Transform transform = Object.Instantiate<GameObject>(itemHatAsset.hat).transform;
          transform.name = "Hat";
          transform.transform.parent = child2;
          transform.transform.localPosition = Vector3.zero;
          transform.transform.localRotation = Quaternion.identity;
          transform.transform.localScale = Vector3.one;
          Object.Destroy((Object) transform.GetComponent<Collider>());
        }
      }
      Transform child4 = child1.FindChild("Backpack");
      if ((Object) child4 != (Object) null)
        Object.Destroy((Object) child4.gameObject);
      Transform child5 = child1.FindChild("Vest");
      if ((Object) child5 != (Object) null)
        Object.Destroy((Object) child5.gameObject);
      Transform child6 = child2.FindChild("Mask");
      if ((Object) child6 != (Object) null)
        Object.Destroy((Object) child6.gameObject);
      Transform child7 = child2.FindChild("Glasses");
      if ((Object) child7 != (Object) null)
        Object.Destroy((Object) child7.gameObject);
      if ((int) gear == (int) byte.MaxValue)
        return;
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, zombieTable.slots[3].table[(int) gear].item);
      if (itemAsset == null)
        return;
      if (itemAsset.type == EItemType.BACKPACK)
      {
        Transform transform = Object.Instantiate<GameObject>(((ItemBackpackAsset) itemAsset).backpack).transform;
        transform.name = "Backpack";
        transform.transform.parent = child1;
        transform.transform.localPosition = Vector3.zero;
        transform.transform.localRotation = Quaternion.identity;
        transform.transform.localScale = Vector3.one;
        Object.Destroy((Object) transform.GetComponent<Collider>());
      }
      else if (itemAsset.type == EItemType.VEST)
      {
        Transform transform = Object.Instantiate<GameObject>(((ItemVestAsset) itemAsset).vest).transform;
        transform.name = "Vest";
        transform.transform.parent = child1;
        transform.transform.localPosition = Vector3.zero;
        transform.transform.localRotation = Quaternion.identity;
        transform.transform.localScale = Vector3.one;
        Object.Destroy((Object) transform.GetComponent<Collider>());
      }
      else if (itemAsset.type == EItemType.MASK)
      {
        Transform transform = Object.Instantiate<GameObject>(((ItemMaskAsset) itemAsset).mask).transform;
        transform.name = "Mask";
        transform.transform.parent = child2;
        transform.transform.localPosition = Vector3.zero;
        transform.transform.localRotation = Quaternion.identity;
        transform.transform.localScale = Vector3.one;
        Object.Destroy((Object) transform.GetComponent<Collider>());
      }
      else
      {
        if (itemAsset.type != EItemType.GLASSES)
          return;
        Transform transform = Object.Instantiate<GameObject>(((ItemGlassesAsset) itemAsset).glasses).transform;
        transform.name = "Glasses";
        transform.transform.parent = child2;
        transform.transform.localPosition = Vector3.zero;
        transform.transform.localRotation = Quaternion.identity;
        transform.transform.localScale = Vector3.one;
        Object.Destroy((Object) transform.GetComponent<Collider>());
      }
    }

    public static void build()
    {
      ZombieClothing.megaTexture = (Texture2D) Resources.Load("Characters/Mega");
      ZombieClothing.zombieTexture = (Texture2D) Resources.Load("Characters/Zombie");
      ZombieClothing.anomalyTexture = (Texture2D) Resources.Load("Characters/Anomaly");
      ZombieClothing.shader = Shader.Find("Standard");
      ZombieClothing.clothes = new Material[LevelZombies.tables.Count][,];
      for (byte index1 = (byte) 0; (int) index1 < LevelZombies.tables.Count; ++index1)
      {
        ZombieTable zombieTable = LevelZombies.tables[(int) index1];
        ZombieClothing.clothes[(int) index1] = new Material[zombieTable.slots[0].table.Count + 1, zombieTable.slots[1].table.Count + 1];
        for (byte index2 = (byte) 0; (int) index2 < zombieTable.slots[0].table.Count + 1; ++index2)
        {
          ushort shirt = (ushort) 0;
          if ((int) index2 < zombieTable.slots[0].table.Count)
            shirt = zombieTable.slots[0].table[(int) index2].item;
          for (byte index3 = (byte) 0; (int) index3 < zombieTable.slots[1].table.Count + 1; ++index3)
          {
            ushort pants = (ushort) 0;
            if ((int) index3 < zombieTable.slots[1].table.Count)
              pants = zombieTable.slots[1].table[(int) index3].item;
            ZombieClothing.clothes[(int) index1][(int) index2, (int) index3] = ZombieClothing.paint(shirt, pants, zombieTable.isMega);
          }
        }
      }
    }
  }
}
