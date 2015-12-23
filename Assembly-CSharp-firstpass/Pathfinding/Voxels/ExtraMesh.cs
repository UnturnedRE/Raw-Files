// Decompiled with JetBrains decompiler
// Type: Pathfinding.Voxels.ExtraMesh
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace Pathfinding.Voxels
{
  public struct ExtraMesh
  {
    public MeshFilter original;
    public int area;
    public Vector3[] vertices;
    public int[] triangles;
    public Bounds bounds;
    public Matrix4x4 matrix;

    public ExtraMesh(Vector3[] v, int[] t, Bounds b)
    {
      this.matrix = Matrix4x4.identity;
      this.vertices = v;
      this.triangles = t;
      this.bounds = b;
      this.original = (MeshFilter) null;
      this.area = 0;
    }

    public ExtraMesh(Vector3[] v, int[] t, Bounds b, Matrix4x4 matrix)
    {
      this.matrix = matrix;
      this.vertices = v;
      this.triangles = t;
      this.bounds = b;
      this.original = (MeshFilter) null;
      this.area = 0;
    }

    public void RecalculateBounds()
    {
      Bounds bounds = new Bounds(this.matrix.MultiplyPoint3x4(this.vertices[0]), Vector3.zero);
      for (int index = 1; index < this.vertices.Length; ++index)
        bounds.Encapsulate(this.matrix.MultiplyPoint3x4(this.vertices[index]));
      this.bounds = bounds;
    }
  }
}
