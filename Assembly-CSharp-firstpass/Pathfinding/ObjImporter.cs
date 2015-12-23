// Decompiled with JetBrains decompiler
// Type: Pathfinding.ObjImporter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Pathfinding
{
  public class ObjImporter
  {
    public static Mesh ImportFile(string filePath)
    {
      if (!File.Exists(filePath))
      {
        Debug.LogError((object) ("No file was found at '" + filePath + "'"));
        return (Mesh) null;
      }
      ObjImporter.meshStruct meshStruct = ObjImporter.createMeshStruct(filePath);
      ObjImporter.populateMeshStruct(ref meshStruct);
      Vector3[] vector3Array1 = new Vector3[meshStruct.faceData.Length];
      Vector2[] vector2Array = new Vector2[meshStruct.faceData.Length];
      Vector3[] vector3Array2 = new Vector3[meshStruct.faceData.Length];
      int index = 0;
      foreach (Vector3 vector3 in meshStruct.faceData)
      {
        vector3Array1[index] = meshStruct.vertices[(int) vector3.x - 1];
        if ((double) vector3.y >= 1.0)
          vector2Array[index] = meshStruct.uv[(int) vector3.y - 1];
        if ((double) vector3.z >= 1.0)
          vector3Array2[index] = meshStruct.normals[(int) vector3.z - 1];
        ++index;
      }
      Mesh mesh = new Mesh();
      mesh.vertices = vector3Array1;
      mesh.uv = vector2Array;
      mesh.normals = vector3Array2;
      mesh.triangles = meshStruct.triangles;
      mesh.RecalculateBounds();
      return mesh;
    }

    private static ObjImporter.meshStruct createMeshStruct(string filename)
    {
      int length1 = 0;
      int length2 = 0;
      int length3 = 0;
      int length4 = 0;
      int length5 = 0;
      ObjImporter.meshStruct meshStruct = new ObjImporter.meshStruct();
      meshStruct.fileName = filename;
      StreamReader streamReader = File.OpenText(filename);
      string s = streamReader.ReadToEnd();
      streamReader.Dispose();
      using (StringReader stringReader = new StringReader(s))
      {
        string str = stringReader.ReadLine();
        char[] separator = new char[1]
        {
          ' '
        };
        while (str != null)
        {
          if (!str.StartsWith("f ") && !str.StartsWith("v ") && (!str.StartsWith("vt ") && !str.StartsWith("vn ")))
          {
            str = stringReader.ReadLine();
            if (str != null)
              str = str.Replace("  ", " ");
          }
          else
          {
            string[] strArray = str.Trim().Split(separator, 50);
            string key = strArray[0];
            if (key != null)
            {
              // ISSUE: reference to a compiler-generated field
              if (ObjImporter.\u003C\u003Ef__switch\u0024map0 == null)
              {
                // ISSUE: reference to a compiler-generated field
                ObjImporter.\u003C\u003Ef__switch\u0024map0 = new Dictionary<string, int>(4)
                {
                  {
                    "v",
                    0
                  },
                  {
                    "vt",
                    1
                  },
                  {
                    "vn",
                    2
                  },
                  {
                    "f",
                    3
                  }
                };
              }
              int num;
              // ISSUE: reference to a compiler-generated field
              if (ObjImporter.\u003C\u003Ef__switch\u0024map0.TryGetValue(key, out num))
              {
                switch (num)
                {
                  case 0:
                    ++length2;
                    break;
                  case 1:
                    ++length3;
                    break;
                  case 2:
                    ++length4;
                    break;
                  case 3:
                    length5 = length5 + strArray.Length - 1;
                    length1 += 3 * (strArray.Length - 2);
                    break;
                }
              }
            }
            str = stringReader.ReadLine();
            if (str != null)
              str = str.Replace("  ", " ");
          }
        }
      }
      meshStruct.triangles = new int[length1];
      meshStruct.vertices = new Vector3[length2];
      meshStruct.uv = new Vector2[length3];
      meshStruct.normals = new Vector3[length4];
      meshStruct.faceData = new Vector3[length5];
      return meshStruct;
    }

    private static void populateMeshStruct(ref ObjImporter.meshStruct mesh)
    {
      StreamReader streamReader = File.OpenText(mesh.fileName);
      string s = streamReader.ReadToEnd();
      streamReader.Close();
      using (StringReader stringReader = new StringReader(s))
      {
        string str = stringReader.ReadLine();
        char[] separator1 = new char[1]
        {
          ' '
        };
        char[] separator2 = new char[1]
        {
          '/'
        };
        int index1 = 0;
        int index2 = 0;
        int index3 = 0;
        int index4 = 0;
        int index5 = 0;
        int index6 = 0;
        int index7 = 0;
        while (str != null)
        {
          if (!str.StartsWith("f ") && !str.StartsWith("v ") && (!str.StartsWith("vt ") && !str.StartsWith("vn ")) && (!str.StartsWith("g ") && !str.StartsWith("usemtl ") && (!str.StartsWith("mtllib ") && !str.StartsWith("vt1 "))) && (!str.StartsWith("vt2 ") && !str.StartsWith("vc ") && !str.StartsWith("usemap ")))
          {
            str = stringReader.ReadLine();
            if (str != null)
              str = str.Replace("  ", " ");
          }
          else
          {
            string[] strArray1 = str.Trim().Split(separator1, 50);
            string key = strArray1[0];
            if (key != null)
            {
              // ISSUE: reference to a compiler-generated field
              if (ObjImporter.\u003C\u003Ef__switch\u0024map1 == null)
              {
                // ISSUE: reference to a compiler-generated field
                ObjImporter.\u003C\u003Ef__switch\u0024map1 = new Dictionary<string, int>(11)
                {
                  {
                    "g",
                    0
                  },
                  {
                    "usemtl",
                    1
                  },
                  {
                    "usemap",
                    2
                  },
                  {
                    "mtllib",
                    3
                  },
                  {
                    "v",
                    4
                  },
                  {
                    "vt",
                    5
                  },
                  {
                    "vt1",
                    6
                  },
                  {
                    "vt2",
                    7
                  },
                  {
                    "vn",
                    8
                  },
                  {
                    "vc",
                    9
                  },
                  {
                    "f",
                    10
                  }
                };
              }
              int num;
              // ISSUE: reference to a compiler-generated field
              if (ObjImporter.\u003C\u003Ef__switch\u0024map1.TryGetValue(key, out num))
              {
                switch (num)
                {
                  case 4:
                    mesh.vertices[index3] = new Vector3(Convert.ToSingle(strArray1[1]), Convert.ToSingle(strArray1[2]), Convert.ToSingle(strArray1[3]));
                    ++index3;
                    break;
                  case 5:
                    mesh.uv[index5] = new Vector2(Convert.ToSingle(strArray1[1]), Convert.ToSingle(strArray1[2]));
                    ++index5;
                    break;
                  case 6:
                    mesh.uv[index6] = new Vector2(Convert.ToSingle(strArray1[1]), Convert.ToSingle(strArray1[2]));
                    ++index6;
                    break;
                  case 7:
                    mesh.uv[index7] = new Vector2(Convert.ToSingle(strArray1[1]), Convert.ToSingle(strArray1[2]));
                    ++index7;
                    break;
                  case 8:
                    mesh.normals[index4] = new Vector3(Convert.ToSingle(strArray1[1]), Convert.ToSingle(strArray1[2]), Convert.ToSingle(strArray1[3]));
                    ++index4;
                    break;
                  case 10:
                    int index8 = 1;
                    List<int> list = new List<int>();
                    while (index8 < strArray1.Length && (string.Empty + strArray1[index8]).Length > 0)
                    {
                      Vector3 vector3 = new Vector3();
                      string[] strArray2 = strArray1[index8].Split(separator2, 3);
                      vector3.x = (float) Convert.ToInt32(strArray2[0]);
                      if (strArray2.Length > 1)
                      {
                        if (strArray2[1] != string.Empty)
                          vector3.y = (float) Convert.ToInt32(strArray2[1]);
                        vector3.z = (float) Convert.ToInt32(strArray2[2]);
                      }
                      ++index8;
                      mesh.faceData[index2] = vector3;
                      list.Add(index2);
                      ++index2;
                    }
                    for (int index9 = 1; index9 + 2 < strArray1.Length; ++index9)
                    {
                      mesh.triangles[index1] = list[0];
                      int index10 = index1 + 1;
                      mesh.triangles[index10] = list[index9];
                      int index11 = index10 + 1;
                      mesh.triangles[index11] = list[index9 + 1];
                      index1 = index11 + 1;
                    }
                    break;
                }
              }
            }
            str = stringReader.ReadLine();
            if (str != null)
              str = str.Replace("  ", " ");
          }
        }
      }
    }

    private struct meshStruct
    {
      public Vector3[] vertices;
      public Vector3[] normals;
      public Vector2[] uv;
      public int[] triangles;
      public Vector3[] faceData;
      public string fileName;
    }
  }
}
