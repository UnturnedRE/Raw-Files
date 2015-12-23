// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.StructureTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Pathfinding;
using UnityEngine;

namespace SDG.Unturned
{
  public class StructureTool : MonoBehaviour
  {
    public static Transform getStructure(ushort id, bool hasOwnership)
    {
      ItemStructureAsset asset = (ItemStructureAsset) Assets.find(EAssetType.ITEM, id);
      return StructureTool.getStructure(id, hasOwnership, asset);
    }

    public static Transform getStructure(ushort id, bool hasOwnership, ItemStructureAsset asset)
    {
      if (asset != null)
      {
        Transform transform1 = !Dedicator.isDedicated ? Object.Instantiate<GameObject>(asset.structure).transform : Object.Instantiate<GameObject>(asset.clip).transform;
        transform1.name = id.ToString();
        if (Provider.isServer)
        {
          Transform transform2 = Object.Instantiate<GameObject>(asset.nav).transform;
          transform2.name = "Nav";
          transform2.parent = transform1;
          transform2.localPosition = Vector3.zero;
          transform2.localRotation = Quaternion.identity;
          if (asset.construct == EConstruct.FLOOR)
          {
            BoxCollider boxCollider = (BoxCollider) transform2.GetComponent<Collider>();
            Transform transform3 = new GameObject().transform;
            transform3.name = "Cutter";
            transform3.parent = transform1;
            transform3.localPosition = Vector3.zero;
            transform3.localRotation = Quaternion.Euler(90f, 0.0f, 0.0f);
            NavmeshCut navmeshCut = transform3.gameObject.AddComponent<NavmeshCut>();
            navmeshCut.center = new Vector3(boxCollider.center.x, boxCollider.center.z, boxCollider.center.y);
            navmeshCut.rectangleSize = new Vector2(boxCollider.size.x + 1f, boxCollider.size.y + 1f);
            navmeshCut.height = boxCollider.size.z + 1f;
            navmeshCut.updateDistance = 1f;
            navmeshCut.updateRotationDistance = 15f;
            navmeshCut.useRotation = true;
          }
        }
        if (hasOwnership)
          transform1.gameObject.AddComponent<Interactable2SalvageStructure>();
        return transform1;
      }
      Transform transform = new GameObject().transform;
      transform.name = id.ToString();
      transform.tag = "Structure";
      transform.gameObject.layer = LayerMasks.STRUCTURE;
      return transform;
    }
  }
}
