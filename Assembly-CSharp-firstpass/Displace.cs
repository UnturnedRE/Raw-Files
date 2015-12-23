// Decompiled with JetBrains decompiler
// Type: Displace
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

[RequireComponent(typeof (WaterBase))]
[ExecuteInEditMode]
public class Displace : MonoBehaviour
{
  public void Awake()
  {
    if (this.enabled)
      this.OnEnable();
    else
      this.OnDisable();
  }

  public void OnEnable()
  {
    Shader.EnableKeyword("WATER_VERTEX_DISPLACEMENT_ON");
    Shader.DisableKeyword("WATER_VERTEX_DISPLACEMENT_OFF");
  }

  public void OnDisable()
  {
    Shader.EnableKeyword("WATER_VERTEX_DISPLACEMENT_OFF");
    Shader.DisableKeyword("WATER_VERTEX_DISPLACEMENT_ON");
  }
}
