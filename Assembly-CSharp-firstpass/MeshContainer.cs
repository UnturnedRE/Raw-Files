// Decompiled with JetBrains decompiler
// Type: MeshContainer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public class MeshContainer
{
  public Mesh mesh;
  public Vector3[] vertices;
  public Vector3[] normals;

  public MeshContainer(Mesh m)
  {
    this.mesh = m;
    this.vertices = m.vertices;
    this.normals = m.normals;
  }

  public void Update()
  {
    this.mesh.vertices = this.vertices;
    this.mesh.normals = this.normals;
  }
}
