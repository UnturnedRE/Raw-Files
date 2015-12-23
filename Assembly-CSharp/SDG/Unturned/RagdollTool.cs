// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.RagdollTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class RagdollTool
  {
    private static void applySkeleton(Transform skeleton_0, Transform skeleton_1)
    {
      for (int index1 = 0; index1 < skeleton_1.childCount; ++index1)
      {
        Transform skeleton_0_1 = (Transform) null;
        Transform child = skeleton_1.GetChild(index1);
        for (int index2 = index1; index2 < skeleton_0.childCount; ++index2)
        {
          skeleton_0_1 = skeleton_0.GetChild(index2);
          if (skeleton_0_1.name == child.name)
            break;
        }
        if ((Object) skeleton_0_1 != (Object) null)
        {
          child.localPosition = skeleton_0_1.localPosition;
          child.localRotation = skeleton_0_1.localRotation;
          if (skeleton_0_1.childCount > 0 && child.childCount > 0)
            RagdollTool.applySkeleton(skeleton_0_1, child);
        }
      }
    }

    public static void ragdollPlayer(Vector3 point, Quaternion rotation, Transform skeleton, Vector3 ragdoll, PlayerClothing clothes)
    {
      if (GraphicsSettings.effectQuality == EGraphicQuality.OFF)
        return;
      ragdoll.y += 8f;
      ragdoll.x += Random.Range(-16f, 16f);
      ragdoll.z += Random.Range(-16f, 16f);
      ragdoll *= !((Object) Player.player != (Object) null) || Player.player.skills.boost != EPlayerBoost.FLIGHT ? 32f : 256f;
      Transform transform = ((GameObject) Object.Instantiate(Resources.Load("Characters/Ragdoll_Player"), point + Vector3.up * 0.1f, rotation * Quaternion.Euler(90f, 0.0f, 0.0f))).transform;
      transform.name = "Ragdoll";
      transform.parent = Level.effects;
      if ((Object) skeleton != (Object) null)
        RagdollTool.applySkeleton(skeleton, transform.FindChild("Skeleton"));
      transform.FindChild("Skeleton").FindChild("Spine").GetComponent<Rigidbody>().AddForce(ragdoll);
      Object.Destroy((Object) transform.gameObject, GraphicsSettings.effect);
      HumanClothes component = transform.GetComponent<HumanClothes>();
      component.skin = clothes.skin;
      component.color = clothes.color;
      component.face = clothes.face;
      component.hair = clothes.hair;
      component.beard = clothes.beard;
      component.shirt = clothes.shirt;
      component.pants = clothes.pants;
      component.hat = clothes.hat;
      component.backpack = clothes.backpack;
      component.vest = clothes.vest;
      component.mask = clothes.mask;
      component.glasses = clothes.glasses;
      component.visualShirt = clothes.visualShirt;
      component.visualPants = clothes.visualPants;
      component.visualHat = clothes.visualHat;
      component.visualBackpack = clothes.visualBackpack;
      component.visualVest = clothes.visualVest;
      component.visualMask = clothes.visualMask;
      component.visualGlasses = clothes.visualGlasses;
      component.isVisual = clothes.isVisual;
      component.apply();
    }

    public static void ragdollZombie(Vector3 point, Quaternion rotation, Transform skeleton, Vector3 ragdoll, byte type, byte shirt, byte pants, byte hat, byte gear)
    {
      if (GraphicsSettings.effectQuality == EGraphicQuality.OFF)
        return;
      ragdoll.y += 8f;
      ragdoll.x += Random.Range(-16f, 16f);
      ragdoll.z += Random.Range(-16f, 16f);
      ragdoll *= !((Object) Player.player != (Object) null) || Player.player.skills.boost != EPlayerBoost.FLIGHT ? 32f : 256f;
      Transform transform = ((GameObject) Object.Instantiate(Resources.Load("Characters/Ragdoll_Zombie"), point + Vector3.up * 0.1f, rotation * Quaternion.Euler(90f, 0.0f, 0.0f))).transform;
      transform.name = "Ragdoll";
      transform.parent = Level.effects;
      transform.localScale = !LevelZombies.tables[(int) type].isMega ? Vector3.one : Vector3.one * 1.5f;
      if ((Object) skeleton != (Object) null)
        RagdollTool.applySkeleton(skeleton, transform.FindChild("Skeleton"));
      transform.FindChild("Skeleton").FindChild("Spine").GetComponent<Rigidbody>().AddForce(ragdoll);
      Object.Destroy((Object) transform.gameObject, GraphicsSettings.effect);
      ZombieClothing.apply(transform, (Renderer) null, transform.FindChild("Model_1").GetComponent<Renderer>(), type, shirt, pants, hat, gear);
    }

    public static void ragdollAnimal(Vector3 point, Quaternion rotation, Transform skeleton, Vector3 ragdoll, ushort id)
    {
      if (GraphicsSettings.effectQuality == EGraphicQuality.OFF)
        return;
      ragdoll.y += 8f;
      ragdoll.x += Random.Range(-16f, 16f);
      ragdoll.z += Random.Range(-16f, 16f);
      ragdoll *= !((Object) Player.player != (Object) null) || Player.player.skills.boost != EPlayerBoost.FLIGHT ? 32f : 256f;
      AnimalAsset animalAsset = (AnimalAsset) Assets.find(EAssetType.ANIMAL, id);
      if (animalAsset == null)
        return;
      Transform transform = ((GameObject) Object.Instantiate((Object) animalAsset.ragdoll, point + Vector3.up * 0.1f, rotation * Quaternion.Euler(0.0f, 90f, 0.0f))).transform;
      transform.name = "Ragdoll";
      transform.parent = Level.effects;
      if ((Object) skeleton != (Object) null)
        RagdollTool.applySkeleton(skeleton, transform.FindChild("Skeleton"));
      transform.FindChild("Skeleton").FindChild("Spine").GetComponent<Rigidbody>().AddForce(ragdoll);
      Object.Destroy((Object) transform.gameObject, GraphicsSettings.effect);
    }
  }
}
