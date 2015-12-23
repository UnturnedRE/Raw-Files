// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.BarricadeTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class BarricadeTool : MonoBehaviour
  {
    public static Transform getBarricade(Transform parent, bool hasOwnership, Vector3 pos, Quaternion rot, ushort id, byte[] state)
    {
      ItemBarricadeAsset asset = (ItemBarricadeAsset) Assets.find(EAssetType.ITEM, id);
      return BarricadeTool.getBarricade(parent, hasOwnership, pos, rot, id, state, asset);
    }

    public static Transform getBarricade(Transform parent, bool hasOwnership, Vector3 pos, Quaternion rot, ushort id, byte[] state, ItemBarricadeAsset asset)
    {
      if (asset != null)
      {
        Transform parent1 = !Dedicator.isDedicated ? Object.Instantiate<GameObject>(asset.barricade).transform : Object.Instantiate<GameObject>(asset.clip).transform;
        parent1.name = id.ToString();
        parent1.parent = parent;
        parent1.localPosition = pos;
        parent1.localRotation = rot;
        if (Provider.isServer && (Object) asset.nav != (Object) null)
        {
          Transform transform = Object.Instantiate<GameObject>(asset.nav).transform;
          transform.name = "Nav";
          transform.parent = asset.build == EBuild.DOOR || asset.build == EBuild.GATE ? parent1.FindChild("Skeleton").FindChild("Hinge") : parent1;
          transform.localPosition = Vector3.zero;
          transform.localRotation = Quaternion.identity;
        }
        Transform childRecursive1 = TransformRecursiveFind.FindChildRecursive(parent1, "Burning");
        if ((Object) childRecursive1 != (Object) null)
          childRecursive1.gameObject.AddComponent<TemperatureTrigger>().temperature = EPlayerTemperature.BURNING;
        Transform childRecursive2 = TransformRecursiveFind.FindChildRecursive(parent1, "Warm");
        if ((Object) childRecursive2 != (Object) null)
          childRecursive2.gameObject.AddComponent<TemperatureTrigger>().temperature = EPlayerTemperature.WARM;
        if (asset.build == EBuild.DOOR || asset.build == EBuild.GATE)
          parent1.FindChild("Skeleton").FindChild("Hinge").gameObject.AddComponent<InteractableDoor>().updateState((Asset) asset, state);
        else if (asset.build == EBuild.BED)
          parent1.gameObject.AddComponent<InteractableBed>().updateState((Asset) asset, state);
        else if (asset.build == EBuild.STORAGE)
          parent1.gameObject.AddComponent<InteractableStorage>().updateState((Asset) asset, state);
        else if (asset.build == EBuild.FARM)
          parent1.gameObject.AddComponent<InteractableFarm>().updateState((Asset) asset, state);
        else if (asset.build == EBuild.TORCH || asset.build == EBuild.CAMPFIRE)
          parent1.gameObject.AddComponent<InteractableFire>().updateState((Asset) asset, state);
        else if (asset.build == EBuild.SPIKE || asset.build == EBuild.WIRE)
          parent1.FindChild("Trap").gameObject.AddComponent<InteractableTrap>().updateState((Asset) asset, state);
        else if (asset.build == EBuild.GENERATOR)
          parent1.gameObject.AddComponent<InteractableGenerator>().updateState((Asset) asset, state);
        else if (asset.build == EBuild.SPOT)
          parent1.gameObject.AddComponent<InteractableSpot>().updateState((Asset) asset, state);
        else if (asset.build == EBuild.SAFEZONE)
          parent1.gameObject.AddComponent<InteractableSafezone>().updateState((Asset) asset, state);
        else if (asset.build == EBuild.SIGN)
          parent1.gameObject.AddComponent<InteractableSign>().updateState((Asset) asset, state);
        if (hasOwnership)
        {
          if (asset.build == EBuild.DOOR || asset.build == EBuild.GATE)
            parent1.FindChild("Skeleton").FindChild("Hinge").gameObject.AddComponent<Interactable2SalvageBarricade>().root = parent1;
          else if (asset.build == EBuild.SPIKE || asset.build == EBuild.WIRE)
            parent1.FindChild("Trap").gameObject.AddComponent<Interactable2SalvageBarricade>().root = parent1;
          else
            parent1.gameObject.AddComponent<Interactable2SalvageBarricade>().root = parent1;
        }
        return parent1;
      }
      Transform transform1 = new GameObject().transform;
      transform1.name = id.ToString();
      transform1.tag = "Barricade";
      transform1.gameObject.layer = LayerMasks.BARRICADE;
      return transform1;
    }
  }
}
