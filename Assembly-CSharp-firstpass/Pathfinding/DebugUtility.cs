// Decompiled with JetBrains decompiler
// Type: Pathfinding.DebugUtility
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

namespace Pathfinding
{
  public class DebugUtility : MonoBehaviour
  {
    public float offset = 0.2f;
    public Material defaultMaterial;
    public static DebugUtility active;
    public bool optimizeMeshes;

    public void Awake()
    {
      DebugUtility.active = this;
    }

    public static void DrawCubes(Vector3[] topVerts, Vector3[] bottomVerts, Color[] vertexColors, float width)
    {
      if ((UnityEngine.Object) DebugUtility.active == (UnityEngine.Object) null)
        DebugUtility.active = UnityEngine.Object.FindObjectOfType(typeof (DebugUtility)) as DebugUtility;
      if ((UnityEngine.Object) DebugUtility.active == (UnityEngine.Object) null)
        throw new NullReferenceException();
      if (topVerts.Length != bottomVerts.Length || topVerts.Length != vertexColors.Length)
      {
        Debug.LogError((object) "Array Lengths are not the same");
      }
      else
      {
        if (topVerts.Length > 2708)
        {
          Vector3[] topVerts1 = new Vector3[topVerts.Length - 2708];
          Vector3[] bottomVerts1 = new Vector3[topVerts.Length - 2708];
          Color[] vertexColors1 = new Color[topVerts.Length - 2708];
          for (int index = 2708; index < topVerts.Length; ++index)
          {
            topVerts1[index - 2708] = topVerts[index];
            bottomVerts1[index - 2708] = bottomVerts[index];
            vertexColors1[index - 2708] = vertexColors[index];
          }
          Vector3[] vector3Array1 = new Vector3[2708];
          Vector3[] vector3Array2 = new Vector3[2708];
          Color[] colorArray = new Color[2708];
          for (int index = 0; index < 2708; ++index)
          {
            vector3Array1[index] = topVerts[index];
            vector3Array2[index] = bottomVerts[index];
            colorArray[index] = vertexColors[index];
          }
          DebugUtility.DrawCubes(topVerts1, bottomVerts1, vertexColors1, width);
          topVerts = vector3Array1;
          bottomVerts = vector3Array2;
          vertexColors = colorArray;
        }
        width /= 2f;
        Vector3[] vector3Array = new Vector3[topVerts.Length * 4 * 6];
        int[] numArray = new int[topVerts.Length * 6 * 6];
        Color[] colorArray1 = new Color[topVerts.Length * 4 * 6];
        for (int index1 = 0; index1 < topVerts.Length; ++index1)
        {
          Vector3 vector3_1 = topVerts[index1] + new Vector3(0.0f, DebugUtility.active.offset, 0.0f);
          Vector3 vector3_2 = bottomVerts[index1] - new Vector3(0.0f, DebugUtility.active.offset, 0.0f);
          Vector3 vector3_3 = vector3_1 + new Vector3(-width, 0.0f, -width);
          Vector3 vector3_4 = vector3_1 + new Vector3(width, 0.0f, -width);
          Vector3 vector3_5 = vector3_1 + new Vector3(width, 0.0f, width);
          Vector3 vector3_6 = vector3_1 + new Vector3(-width, 0.0f, width);
          Vector3 vector3_7 = vector3_2 + new Vector3(-width, 0.0f, -width);
          Vector3 vector3_8 = vector3_2 + new Vector3(width, 0.0f, -width);
          Vector3 vector3_9 = vector3_2 + new Vector3(width, 0.0f, width);
          Vector3 vector3_10 = vector3_2 + new Vector3(-width, 0.0f, width);
          int index2 = index1 * 4 * 6;
          Color color = vertexColors[index1];
          for (int index3 = index2; index3 < index2 + 24; ++index3)
            colorArray1[index3] = color;
          vector3Array[index2] = vector3_3;
          vector3Array[index2 + 1] = vector3_6;
          vector3Array[index2 + 2] = vector3_5;
          vector3Array[index2 + 3] = vector3_4;
          int index4 = index1 * 6 * 6;
          numArray[index4] = index2;
          numArray[index4 + 1] = index2 + 1;
          numArray[index4 + 2] = index2 + 2;
          numArray[index4 + 3] = index2;
          numArray[index4 + 4] = index2 + 2;
          numArray[index4 + 5] = index2 + 3;
          int index5 = index2 + 4;
          vector3Array[index5 + 3] = vector3_7;
          vector3Array[index5 + 2] = vector3_10;
          vector3Array[index5 + 1] = vector3_9;
          vector3Array[index5] = vector3_8;
          int index6 = index4 + 6;
          numArray[index6] = index5;
          numArray[index6 + 1] = index5 + 1;
          numArray[index6 + 2] = index5 + 2;
          numArray[index6 + 3] = index5;
          numArray[index6 + 4] = index5 + 2;
          numArray[index6 + 5] = index5 + 3;
          int index7 = index5 + 4;
          vector3Array[index7] = vector3_8;
          vector3Array[index7 + 1] = vector3_4;
          vector3Array[index7 + 2] = vector3_5;
          vector3Array[index7 + 3] = vector3_9;
          int index8 = index6 + 6;
          numArray[index8] = index7;
          numArray[index8 + 1] = index7 + 1;
          numArray[index8 + 2] = index7 + 2;
          numArray[index8 + 3] = index7;
          numArray[index8 + 4] = index7 + 2;
          numArray[index8 + 5] = index7 + 3;
          int index9 = index7 + 4;
          vector3Array[index9 + 3] = vector3_7;
          vector3Array[index9 + 2] = vector3_3;
          vector3Array[index9 + 1] = vector3_6;
          vector3Array[index9] = vector3_10;
          int index10 = index8 + 6;
          numArray[index10] = index9;
          numArray[index10 + 1] = index9 + 1;
          numArray[index10 + 2] = index9 + 2;
          numArray[index10 + 3] = index9;
          numArray[index10 + 4] = index9 + 2;
          numArray[index10 + 5] = index9 + 3;
          int index11 = index9 + 4;
          vector3Array[index11 + 3] = vector3_9;
          vector3Array[index11 + 2] = vector3_10;
          vector3Array[index11 + 1] = vector3_6;
          vector3Array[index11] = vector3_5;
          int index12 = index10 + 6;
          numArray[index12] = index11;
          numArray[index12 + 1] = index11 + 1;
          numArray[index12 + 2] = index11 + 2;
          numArray[index12 + 3] = index11;
          numArray[index12 + 4] = index11 + 2;
          numArray[index12 + 5] = index11 + 3;
          int index13 = index11 + 4;
          vector3Array[index13] = vector3_8;
          vector3Array[index13 + 1] = vector3_7;
          vector3Array[index13 + 2] = vector3_3;
          vector3Array[index13 + 3] = vector3_4;
          int index14 = index12 + 6;
          numArray[index14] = index13;
          numArray[index14 + 1] = index13 + 1;
          numArray[index14 + 2] = index13 + 2;
          numArray[index14 + 3] = index13;
          numArray[index14 + 4] = index13 + 2;
          numArray[index14 + 5] = index13 + 3;
        }
        Mesh mesh = new Mesh();
        mesh.vertices = vector3Array;
        mesh.triangles = numArray;
        mesh.colors = colorArray1;
        mesh.name = "VoxelMesh";
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        if (DebugUtility.active.optimizeMeshes)
          mesh.Optimize();
        GameObject gameObject = new GameObject("DebugMesh");
        (gameObject.AddComponent(typeof (MeshRenderer)) as MeshRenderer).material = DebugUtility.active.defaultMaterial;
        (gameObject.AddComponent(typeof (MeshFilter)) as MeshFilter).mesh = mesh;
      }
    }

    public static void DrawQuads(Vector3[] verts, float width)
    {
      if (verts.Length >= 16250)
      {
        Vector3[] verts1 = new Vector3[verts.Length - 16250];
        for (int index = 16250; index < verts.Length; ++index)
          verts1[index - 16250] = verts[index];
        Vector3[] vector3Array = new Vector3[16250];
        for (int index = 0; index < 16250; ++index)
          vector3Array[index] = verts[index];
        DebugUtility.DrawQuads(verts1, width);
        verts = vector3Array;
      }
      width /= 2f;
      Vector3[] vector3Array1 = new Vector3[verts.Length * 4];
      int[] numArray = new int[verts.Length * 6];
      for (int index1 = 0; index1 < verts.Length; ++index1)
      {
        Vector3 vector3 = verts[index1];
        int index2 = index1 * 4;
        vector3Array1[index2] = vector3 + new Vector3(-width, 0.0f, -width);
        vector3Array1[index2 + 1] = vector3 + new Vector3(-width, 0.0f, width);
        vector3Array1[index2 + 2] = vector3 + new Vector3(width, 0.0f, width);
        vector3Array1[index2 + 3] = vector3 + new Vector3(width, 0.0f, -width);
        int index3 = index1 * 6;
        numArray[index3] = index2;
        numArray[index3 + 1] = index2 + 1;
        numArray[index3 + 2] = index2 + 2;
        numArray[index3 + 3] = index2;
        numArray[index3 + 4] = index2 + 2;
        numArray[index3 + 5] = index2 + 3;
      }
      Mesh mesh = new Mesh();
      mesh.vertices = vector3Array1;
      mesh.triangles = numArray;
      mesh.RecalculateNormals();
      mesh.RecalculateBounds();
      GameObject gameObject = new GameObject("DebugMesh");
      (gameObject.AddComponent(typeof (MeshRenderer)) as MeshRenderer).material = DebugUtility.active.defaultMaterial;
      (gameObject.AddComponent(typeof (MeshFilter)) as MeshFilter).mesh = mesh;
    }

    public static void TestMeshLimit()
    {
      Vector3[] vector3Array = new Vector3[64000];
      int[] numArray = new int[96000];
      for (int index1 = 0; index1 < 16000; ++index1)
      {
        Vector3 vector3 = UnityEngine.Random.onUnitSphere * 10f;
        int index2 = index1 * 4;
        vector3Array[index2] = vector3 + new Vector3(-0.1f, 0.0f, -0.1f);
        vector3Array[index2 + 1] = vector3 + new Vector3(-0.1f, 0.0f, 0.1f);
        vector3Array[index2 + 2] = vector3 + new Vector3(0.1f, 0.0f, 0.1f);
        vector3Array[index2 + 3] = vector3 + new Vector3(0.1f, 0.0f, -0.1f);
        int index3 = index1 * 6;
        numArray[index3] = index2;
        numArray[index3 + 1] = index2 + 1;
        numArray[index3 + 2] = index2 + 2;
        numArray[index3 + 3] = index2;
        numArray[index3 + 4] = index2 + 2;
        numArray[index3 + 5] = index2 + 3;
      }
      Mesh mesh = new Mesh();
      mesh.vertices = vector3Array;
      mesh.triangles = numArray;
      mesh.RecalculateNormals();
      mesh.RecalculateBounds();
      GameObject gameObject = new GameObject("DebugMesh");
      gameObject.AddComponent(typeof (MeshRenderer));
      (gameObject.AddComponent(typeof (MeshFilter)) as MeshFilter).mesh = mesh;
    }
  }
}
