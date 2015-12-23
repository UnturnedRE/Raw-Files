// Decompiled with JetBrains decompiler
// Type: WaterTile
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

[ExecuteInEditMode]
public class WaterTile : MonoBehaviour
{
  public PlanarReflection reflection;
  public WaterBase waterBase;

  public void Start()
  {
    this.AcquireComponents();
  }

  private void AcquireComponents()
  {
    if ((bool) ((Object) this.reflection))
      return;
    if ((bool) ((Object) this.transform.parent))
      this.reflection = this.transform.parent.GetComponent<PlanarReflection>();
    else
      this.reflection = this.transform.GetComponent<PlanarReflection>();
  }

  public void OnWillRenderObject()
  {
    if ((bool) ((Object) this.reflection))
      this.reflection.WaterTileBeingRendered(this.transform, Camera.current);
    if (!(bool) ((Object) this.waterBase))
      return;
    this.waterBase.WaterTileBeingRendered(this.transform, Camera.current);
  }
}
