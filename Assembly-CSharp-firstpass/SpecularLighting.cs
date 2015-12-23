// Decompiled with JetBrains decompiler
// Type: SpecularLighting
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof (WaterBase))]
public class SpecularLighting : MonoBehaviour
{
  public Transform specularLight;
  private WaterBase waterBase;

  public void Start()
  {
    this.waterBase = (WaterBase) this.gameObject.GetComponent(typeof (WaterBase));
  }

  public void Update()
  {
    if (!(bool) ((Object) this.waterBase))
      this.waterBase = (WaterBase) this.gameObject.GetComponent(typeof (WaterBase));
    if (!(bool) ((Object) this.specularLight) || !(bool) ((Object) this.waterBase.sharedMaterial))
      return;
    this.waterBase.sharedMaterial.SetVector("_WorldLightDir", (Vector4) this.specularLight.transform.forward);
  }
}
