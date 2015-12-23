// Decompiled with JetBrains decompiler
// Type: LocalSpaceGraph
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public class LocalSpaceGraph : MonoBehaviour
{
  protected Matrix4x4 originalMatrix;

  private void Start()
  {
    this.originalMatrix = this.transform.localToWorldMatrix;
  }

  public Matrix4x4 GetMatrix()
  {
    return this.transform.worldToLocalMatrix * this.originalMatrix;
  }
}
