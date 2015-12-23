// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.LevelGround
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace SDG.Unturned
{
  public class LevelGround : MonoBehaviour
  {
    public static readonly byte SAVEDATA_HEIGHTS_VERSION = (byte) 1;
    public static readonly byte SAVEDATA_HEIGHTS2_VERSION = (byte) 1;
    public static readonly byte SAVEDATA_MATERIALS_VERSION = (byte) 4;
    public static readonly byte SAVEDATA_DETAILS_VERSION = (byte) 3;
    public static readonly byte SAVEDATA_RESOURCES_VERSION = (byte) 6;
    public static readonly byte SAVEDATA_TREES_VERSION = (byte) 5;
    public static readonly byte RESOURCE_REGIONS = (byte) 4;
    public static readonly byte ALPHAMAPS = (byte) 2;
    private static Texture2D MASK;
    private static float[][,] reunHeight;
    private static float[][,,] reunMaterial;
    private static int frameHeight;
    private static int frameMaterial;
    private static float[,,] alphamap;
    private static float[,,] alphamap2;
    private static bool isBakingResources;
    private static byte bakeResources_X;
    private static byte bakeResources_Y;
    private static byte bakeResources_W;
    private static byte bakeResources_H;
    private static byte bakeResources_M;
    public static GroundMaterial[] _materials;
    public static GroundDetail[] _details;
    public static GroundResource[] _resources;
    private static byte[] _hash;
    private static byte[] hashHeights;
    private static byte[] hashTrees;
    private static Transform _models;
    private static Transform _models2;
    private static List<ResourceSpawnpoint>[,] _trees;
    private static bool[,] _regions;
    private static Terrain _terrain;
    private static Terrain _terrain2;
    private static TerrainData _data;
    private static TerrainData _data2;
    private static TerrainCollider collider;
    private static TerrainCollider collider2;

    public static GroundMaterial[] materials
    {
      get
      {
        return LevelGround._materials;
      }
    }

    public static GroundDetail[] details
    {
      get
      {
        return LevelGround._details;
      }
    }

    public static GroundResource[] resources
    {
      get
      {
        return LevelGround._resources;
      }
    }

    public static byte[] hash
    {
      get
      {
        return LevelGround._hash;
      }
    }

    public static Transform models
    {
      get
      {
        return LevelGround._models;
      }
    }

    public static Transform models2
    {
      get
      {
        return LevelGround._models2;
      }
    }

    public static List<ResourceSpawnpoint>[,] trees
    {
      get
      {
        return LevelGround._trees;
      }
    }

    public static bool[,] regions
    {
      get
      {
        return LevelGround._regions;
      }
    }

    public static Terrain terrain
    {
      get
      {
        return LevelGround._terrain;
      }
    }

    public static Terrain terrain2
    {
      get
      {
        return LevelGround._terrain2;
      }
    }

    public static TerrainData data
    {
      get
      {
        return LevelGround._data;
      }
    }

    public static TerrainData data2
    {
      get
      {
        return LevelGround._data2;
      }
    }

    public static Vector3 checkSafe(Vector3 point)
    {
      float height = LevelGround.getHeight(point);
      if ((double) point.y < (double) height - 1.0)
        point.y = height + 0.5f;
      return point;
    }

    public static void undoHeight()
    {
      if (LevelGround.frameHeight >= LevelGround.reunHeight.Length - 1)
        return;
      if (LevelGround.reunHeight[LevelGround.frameHeight + 1] != null)
        ++LevelGround.frameHeight;
      if (LevelGround.reunHeight[LevelGround.frameHeight] == null)
        return;
      LevelGround.data.SetHeights(0, 0, LevelGround.reunHeight[LevelGround.frameHeight]);
      LevelGround.terrain.Flush();
    }

    public static void redoHeight()
    {
      if (LevelGround.frameHeight <= 0)
        return;
      if (LevelGround.reunHeight[LevelGround.frameHeight - 1] != null)
        --LevelGround.frameHeight;
      if (LevelGround.reunHeight[LevelGround.frameHeight] == null)
        return;
      LevelGround.data.SetHeights(0, 0, LevelGround.reunHeight[LevelGround.frameHeight]);
      LevelGround.terrain.Flush();
    }

    public static void registerHeight()
    {
      if (LevelGround.frameHeight > 0)
      {
        LevelGround.reunHeight = new float[LevelGround.reunHeight.Length][,];
        LevelGround.frameHeight = 0;
      }
      for (int index = LevelGround.reunHeight.Length - 1; index > 0; --index)
        LevelGround.reunHeight[index] = LevelGround.reunHeight[index - 1];
      LevelGround.reunHeight[0] = LevelGround.data.GetHeights(0, 0, LevelGround.data.heightmapWidth, LevelGround.data.heightmapHeight);
    }

    public static void undoMaterial()
    {
      if (LevelGround.frameMaterial >= LevelGround.reunMaterial.Length - 1)
        return;
      if (LevelGround.reunMaterial[LevelGround.frameMaterial + 1] != null)
        ++LevelGround.frameMaterial;
      if (LevelGround.reunMaterial[LevelGround.frameMaterial] == null)
        return;
      LevelGround.data.SetAlphamaps(0, 0, LevelGround.reunMaterial[LevelGround.frameMaterial]);
    }

    public static void redoMaterial()
    {
      if (LevelGround.frameMaterial <= 0)
        return;
      if (LevelGround.reunMaterial[LevelGround.frameMaterial - 1] != null)
        --LevelGround.frameMaterial;
      if (LevelGround.reunMaterial[LevelGround.frameMaterial] == null)
        return;
      LevelGround.data.SetAlphamaps(0, 0, LevelGround.reunMaterial[LevelGround.frameMaterial]);
    }

    public static void registerMaterial()
    {
      if (LevelGround.frameMaterial > 0)
      {
        LevelGround.reunMaterial = new float[LevelGround.reunMaterial.Length][,,];
        LevelGround.frameMaterial = 0;
      }
      for (int index = LevelGround.reunMaterial.Length - 1; index > 0; --index)
        LevelGround.reunMaterial[index] = LevelGround.reunMaterial[index - 1];
      LevelGround.reunMaterial[0] = LevelGround.data.GetAlphamaps(0, 0, LevelGround.data.alphamapWidth, LevelGround.data.alphamapHeight);
    }

    public static GroundMaterial getMaterial(Vector3 point)
    {
      float[,,] alphamaps = LevelGround.data.GetAlphamaps(LevelGround.getAlphamap_X(point), LevelGround.getAlphamap_Y(point), 1, 1);
      float num = 0.0f;
      int index1 = 0;
      for (int index2 = 0; index2 < LevelGround.materials.Length; ++index2)
      {
        if ((double) alphamaps[0, 0, index2] > (double) num)
        {
          index1 = index2;
          num = alphamaps[0, 0, index2];
        }
      }
      return LevelGround.materials[index1];
    }

    public static float getHeight(Vector3 point)
    {
      return LevelGround.terrain.SampleHeight(point);
    }

    public static Vector3 getNormal(Vector3 point)
    {
      return LevelGround.data.GetInterpolatedNormal((point.x - LevelGround.terrain.transform.position.x) / LevelGround.data.size.x, (point.z - LevelGround.terrain.transform.position.z) / LevelGround.data.size.z);
    }

    public static int getDetail_X(Vector3 point)
    {
      return (int) (((double) point.x - (double) LevelGround.terrain.transform.position.x) / (double) LevelGround.data.size.x * (double) LevelGround.data.detailWidth);
    }

    public static int getDetail_Y(Vector3 point)
    {
      return (int) (((double) point.z - (double) LevelGround.terrain.transform.position.z) / (double) LevelGround.data.size.z * (double) LevelGround.data.detailHeight);
    }

    public static int getDetail2_X(Vector3 point)
    {
      return (int) (((double) point.x - (double) LevelGround.terrain2.transform.position.x) / (double) LevelGround.data2.size.x * (double) LevelGround.data2.detailWidth);
    }

    public static int getDetail2_Y(Vector3 point)
    {
      return (int) (((double) point.z - (double) LevelGround.terrain2.transform.position.z) / (double) LevelGround.data2.size.z * (double) LevelGround.data2.detailHeight);
    }

    public static int getAlphamap_X(Vector3 point)
    {
      return (int) (((double) point.x - (double) LevelGround.terrain.transform.position.x) / (double) LevelGround.data.size.x * (double) LevelGround.data.alphamapWidth);
    }

    public static int getAlphamap_Y(Vector3 point)
    {
      return (int) (((double) point.z - (double) LevelGround.terrain.transform.position.z) / (double) LevelGround.data.size.z * (double) LevelGround.data.alphamapHeight);
    }

    public static int getAlphamap2_X(Vector3 point)
    {
      return (int) (((double) point.x - (double) LevelGround.terrain2.transform.position.x) / (double) LevelGround.data2.size.x * (double) LevelGround.data2.alphamapWidth);
    }

    public static int getAlphamap2_Y(Vector3 point)
    {
      return (int) (((double) point.z - (double) LevelGround.terrain2.transform.position.z) / (double) LevelGround.data2.size.z * (double) LevelGround.data2.alphamapHeight);
    }

    public static int getHeightmap_X(Vector3 point)
    {
      return (int) (((double) point.x - (double) LevelGround.terrain.transform.position.x) / (double) LevelGround.data.size.x * (double) LevelGround.data.heightmapWidth);
    }

    public static int getHeightmap_Y(Vector3 point)
    {
      return (int) (((double) point.z - (double) LevelGround.terrain.transform.position.z) / (double) LevelGround.data.size.z * (double) LevelGround.data.heightmapHeight);
    }

    public static int getHeightmap2_X(Vector3 point)
    {
      return (int) (((double) point.x - (double) LevelGround.terrain2.transform.position.x) / (double) LevelGround.data2.size.x * (double) LevelGround.data2.heightmapWidth);
    }

    public static int getHeightmap2_Y(Vector3 point)
    {
      return (int) (((double) point.z - (double) LevelGround.terrain2.transform.position.z) / (double) LevelGround.data2.size.z * (double) LevelGround.data2.heightmapHeight);
    }

    public static void bewilder(Vector3 point)
    {
      if (Dedicator.isDedicated)
        return;
      int detailX = LevelGround.getDetail_X(point);
      int detailY = LevelGround.getDetail_Y(point);
      int[,] details = new int[4, 4];
      for (int layer = 0; layer < LevelGround.details.Length; ++layer)
        LevelGround.data.SetDetailLayer(detailX - 2, detailY - 2, layer, details);
    }

    public static void paint(Vector3 point, int size, int layer, bool edit2)
    {
      if (edit2)
        size = (int) ((double) size * 0.75);
      else
        size *= 3;
      if (size == 0)
        return;
      int num1;
      int num2;
      if (edit2)
      {
        num1 = LevelGround.getAlphamap2_X(point);
        num2 = LevelGround.getAlphamap2_Y(point);
      }
      else
      {
        num1 = LevelGround.getAlphamap_X(point);
        num2 = LevelGround.getAlphamap_Y(point);
      }
      int num3 = 0;
      int num4 = 0;
      int x = num1 - size / 2;
      if (x < 0)
      {
        num3 = -x;
        x = 0;
      }
      int y = num2 - size / 2;
      if (y < 0)
      {
        num4 = -y;
        y = 0;
      }
      int num5 = num1 + size / 2;
      int num6 = num2 + size / 2;
      if (edit2)
      {
        if (num5 > LevelGround.data2.alphamapWidth)
          num5 = LevelGround.data2.alphamapWidth;
        if (num6 > LevelGround.data2.alphamapHeight)
          num6 = LevelGround.data2.alphamapHeight;
      }
      else
      {
        if (num5 > LevelGround.data.alphamapWidth)
          num5 = LevelGround.data.alphamapWidth;
        if (num6 > LevelGround.data.alphamapHeight)
          num6 = LevelGround.data.alphamapHeight;
      }
      int width = num5 - x;
      int height = num6 - y;
      float[,,] map = !edit2 ? LevelGround.data.GetAlphamaps(x, y, width, height) : LevelGround.data2.GetAlphamaps(x, y, width, height);
      for (int index1 = 0; index1 < height; ++index1)
      {
        for (int index2 = 0; index2 < width; ++index2)
        {
          if ((double) LevelGround.MASK.GetPixel((int) ((double) (index1 + num4) / (double) size * (double) LevelGround.MASK.width), (int) ((double) (index2 + num3) / (double) size * (double) LevelGround.MASK.height)).r > 0.5)
          {
            for (int index3 = 0; index3 < LevelGround.materials.Length; ++index3)
              map[index1, index2, index3] = index3 != layer ? 0.0f : 1f;
          }
        }
      }
      if (edit2)
        LevelGround.data2.SetAlphamaps(x, y, map);
      else
        LevelGround.data.SetAlphamaps(x, y, map);
    }

    public static void adjust(Vector3 point, int size, float strength, bool edit2)
    {
      size = !edit2 ? (int) ((double) size * 1.5) : (int) ((double) size * 0.375);
      if (size == 0)
        return;
      int num1;
      int num2;
      if (edit2)
      {
        num1 = LevelGround.getHeightmap2_X(point);
        num2 = LevelGround.getHeightmap2_Y(point);
      }
      else
      {
        num1 = LevelGround.getHeightmap_X(point);
        num2 = LevelGround.getHeightmap_Y(point);
      }
      int num3 = 0;
      int num4 = 0;
      int xBase = num1 - size / 2;
      if (xBase < 0)
      {
        num3 = -xBase;
        xBase = 0;
      }
      int yBase = num2 - size / 2;
      if (yBase < 0)
      {
        num4 = -yBase;
        yBase = 0;
      }
      int num5 = num1 + size / 2;
      int num6 = num2 + size / 2;
      if (edit2)
      {
        if (num5 > LevelGround.data2.heightmapWidth)
          num5 = LevelGround.data2.heightmapWidth;
        if (num6 > LevelGround.data2.heightmapHeight)
          num6 = LevelGround.data2.heightmapHeight;
      }
      else
      {
        if (num5 > LevelGround.data.heightmapWidth)
          num5 = LevelGround.data.heightmapWidth;
        if (num6 > LevelGround.data.heightmapHeight)
          num6 = LevelGround.data.heightmapHeight;
      }
      int width = num5 - xBase;
      int height = num6 - yBase;
      float[,] heights = !edit2 ? LevelGround.data.GetHeights(xBase, yBase, width, height) : LevelGround.data2.GetHeights(xBase, yBase, width, height);
      for (int index1 = 0; index1 < height; ++index1)
      {
        for (int index2 = 0; index2 < width; ++index2)
        {
          float num7 = LevelGround.MASK.GetPixel((int) ((double) (index1 + num4) / (double) size * (double) LevelGround.MASK.width), (int) ((double) (index2 + num3) / (double) size * (double) LevelGround.MASK.height)).r;
          heights[index1, index2] = heights[index1, index2] + Time.deltaTime * 0.25f * num7 * strength;
          if ((double) heights[index1, index2] < 0.0)
            heights[index1, index2] = 0.0f;
          else if ((double) heights[index1, index2] > 1.0)
            heights[index1, index2] = 1f;
        }
      }
      if (edit2)
      {
        LevelGround.data2.SetHeights(xBase, yBase, heights);
        LevelGround.terrain2.Flush();
      }
      else
      {
        LevelGround.data.SetHeights(xBase, yBase, heights);
        LevelGround.terrain.Flush();
      }
    }

    public static void smooth(Vector3 point, int size, float strength, bool edit2)
    {
      size = !edit2 ? (int) ((double) size * 1.5) : (int) ((double) size * 0.375);
      if (size == 0)
        return;
      int num1;
      int num2;
      if (edit2)
      {
        num1 = LevelGround.getHeightmap2_X(point);
        num2 = LevelGround.getHeightmap2_Y(point);
      }
      else
      {
        num1 = LevelGround.getHeightmap_X(point);
        num2 = LevelGround.getHeightmap_Y(point);
      }
      int xBase = num1 - size / 2;
      if (xBase < 0)
        xBase = 0;
      int yBase = num2 - size / 2;
      if (yBase < 0)
        yBase = 0;
      int num3 = num1 + size / 2;
      int num4 = num2 + size / 2;
      if (edit2)
      {
        if (num3 > LevelGround.data2.heightmapWidth)
          num3 = LevelGround.data2.heightmapWidth;
        if (num4 > LevelGround.data2.heightmapHeight)
          num4 = LevelGround.data2.heightmapHeight;
      }
      else
      {
        if (num3 > LevelGround.data.heightmapWidth)
          num3 = LevelGround.data.heightmapWidth;
        if (num4 > LevelGround.data.heightmapHeight)
          num4 = LevelGround.data.heightmapHeight;
      }
      int width = num3 - xBase;
      int height = num4 - yBase;
      float[,] numArray = !edit2 ? LevelGround.data.GetHeights(xBase, yBase, width, height) : LevelGround.data2.GetHeights(xBase, yBase, width, height);
      float[,] heights = new float[height, width];
      for (int index1 = 0; index1 < height; ++index1)
      {
        for (int index2 = 0; index2 < width; ++index2)
        {
          float num5 = LevelGround.MASK.GetPixel((int) ((double) index1 / (double) size * (double) LevelGround.MASK.width), (int) ((double) index2 / (double) size * (double) LevelGround.MASK.height)).r;
          float num6 = 0.0f;
          int num7 = 1;
          float num8 = num6 + numArray[index1, index2];
          if (index1 > 0)
          {
            ++num7;
            num8 += numArray[index1 - 1, index2];
          }
          if (index2 > 0)
          {
            ++num7;
            num8 += numArray[index1, index2 - 1];
          }
          if (index1 < height - 1)
          {
            ++num7;
            num8 += numArray[index1 + 1, index2];
          }
          if (index2 < width - 1)
          {
            ++num7;
            num8 += numArray[index1, index2 + 1];
          }
          float b = num8 / (float) num7;
          heights[index1, index2] = Mathf.Lerp(numArray[index1, index2], b, num5 * strength);
        }
      }
      if (edit2)
      {
        LevelGround.data2.SetHeights(xBase, yBase, heights);
        LevelGround.terrain2.Flush();
      }
      else
      {
        LevelGround.data.SetHeights(xBase, yBase, heights);
        LevelGround.terrain.Flush();
      }
    }

    public static void flatten(Vector3 point, int size, float height, float strength, bool edit2)
    {
      size = !edit2 ? (int) ((double) size * 1.5) : (int) ((double) size * 0.375);
      if (size == 0)
        return;
      int num1;
      int num2;
      if (edit2)
      {
        num1 = LevelGround.getHeightmap2_X(point);
        num2 = LevelGround.getHeightmap2_Y(point);
      }
      else
      {
        num1 = LevelGround.getHeightmap_X(point);
        num2 = LevelGround.getHeightmap_Y(point);
      }
      int num3 = 0;
      int num4 = 0;
      int xBase = num1 - size / 2;
      if (xBase < 0)
      {
        num3 = -xBase;
        xBase = 0;
      }
      int yBase = num2 - size / 2;
      if (yBase < 0)
      {
        num4 = -yBase;
        yBase = 0;
      }
      int num5 = num1 + size / 2;
      int num6 = num2 + size / 2;
      if (edit2)
      {
        if (num5 > LevelGround.data2.heightmapWidth)
          num5 = LevelGround.data2.heightmapWidth;
        if (num6 > LevelGround.data2.heightmapHeight)
          num6 = LevelGround.data2.heightmapHeight;
      }
      else
      {
        if (num5 > LevelGround.data.heightmapWidth)
          num5 = LevelGround.data.heightmapWidth;
        if (num6 > LevelGround.data.heightmapHeight)
          num6 = LevelGround.data.heightmapHeight;
      }
      int width = num5 - xBase;
      int height1 = num6 - yBase;
      float[,] heights = !edit2 ? LevelGround.data.GetHeights(xBase, yBase, width, height1) : LevelGround.data2.GetHeights(xBase, yBase, width, height1);
      for (int index1 = 0; index1 < height1; ++index1)
      {
        for (int index2 = 0; index2 < width; ++index2)
        {
          float num7 = LevelGround.MASK.GetPixel((int) ((double) (index1 + num4) / (double) size * (double) LevelGround.MASK.width), (int) ((double) (index2 + num3) / (double) size * (double) LevelGround.MASK.height)).r;
          heights[index1, index2] = Mathf.Lerp(heights[index1, index2], height, num7 * strength);
        }
      }
      if (edit2)
      {
        LevelGround.data2.SetHeights(xBase, yBase, heights);
        LevelGround.terrain2.Flush();
      }
      else
      {
        LevelGround.data.SetHeights(xBase, yBase, heights);
        LevelGround.terrain.Flush();
      }
    }

    public static void bakeMaterials(bool quality)
    {
      if (LevelGround.data.alphamapLayers <= 0)
        return;
      int[] numArray1 = new int[LevelGround.materials.Length];
      List<int> list1 = new List<int>()
      {
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7
      };
      for (int index1 = 0; index1 < LevelGround.materials.Length; ++index1)
      {
        float num = 0.0f;
        int index2 = 0;
        for (int index3 = 0; index3 < list1.Count; ++index3)
        {
          if ((double) LevelGround.materials[list1[index3]].steepness >= (double) num)
          {
            num = LevelGround.materials[list1[index3]].steepness;
            index2 = index3;
          }
        }
        numArray1[index1] = list1[index2];
        list1.RemoveAt(index2);
      }
      int[] numArray2 = new int[LevelGround.materials.Length];
      List<int> list2 = new List<int>()
      {
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7
      };
      for (int index1 = 0; index1 < LevelGround.materials.Length; ++index1)
      {
        float num = 0.0f;
        int index2 = 0;
        for (int index3 = 0; index3 < list2.Count; ++index3)
        {
          if ((double) LevelGround.materials[list2[index3]].height >= (double) num)
          {
            num = LevelGround.materials[list2[index3]].height;
            index2 = index3;
          }
        }
        numArray2[index1] = list2[index2];
        list2.RemoveAt(index2);
      }
      LevelGround.alphamap = LevelGround.data.GetAlphamaps(0, 0, LevelGround.data.alphamapWidth, LevelGround.data.alphamapHeight);
      for (int index1 = 0; index1 < LevelGround.data.alphamapWidth; ++index1)
      {
        for (int index2 = 0; index2 < LevelGround.data.alphamapHeight; ++index2)
        {
          bool flag = false;
          for (int index3 = 0; index3 < LevelGround.materials.Length; ++index3)
          {
            if (!LevelGround.materials[index3].isGenerated && (double) LevelGround.alphamap[index1, index2, index3] > 0.5)
            {
              for (int index4 = 0; index4 < LevelGround.materials.Length; ++index4)
                LevelGround.alphamap[index1, index2, index4] = index4 != index3 ? 0.0f : 1f;
              flag = true;
              break;
            }
          }
          if (!flag)
          {
            float num1 = LevelGround.data.GetSteepness((float) index2 / (float) LevelGround.data.alphamapWidth, (float) index1 / (float) LevelGround.data.alphamapHeight) / 64f;
            for (int index3 = 0; index3 < LevelGround.materials.Length; ++index3)
            {
              if ((double) num1 >= (double) LevelGround.materials[numArray1[index3]].steepness && (double) LevelGround.materials[numArray1[index3]].steepness >= 0.00999999977648258)
              {
                for (int index4 = 0; index4 < LevelGround.materials.Length; ++index4)
                  LevelGround.alphamap[index1, index2, index4] = index4 != numArray1[index3] ? 0.0f : 1f;
                flag = true;
                break;
              }
            }
            if (!flag)
            {
              float num2 = LevelGround.data.GetInterpolatedHeight((float) index2 / (float) LevelGround.data.alphamapWidth, (float) index1 / (float) LevelGround.data.alphamapHeight) / LevelGround.data.size.y;
              for (int index3 = 0; index3 < LevelGround.materials.Length; ++index3)
              {
                if ((double) num2 >= (double) LevelGround.materials[numArray2[index3]].height && (double) LevelGround.materials[numArray2[index3]].height <= 0.990000009536743)
                {
                  for (int index4 = 0; index4 < LevelGround.materials.Length; ++index4)
                    LevelGround.alphamap[index1, index2, index4] = index4 != numArray2[index3] ? 0.0f : 1f;
                  flag = true;
                  break;
                }
              }
              if (!flag)
              {
                for (int index3 = 0; index3 < LevelGround.materials.Length; ++index3)
                  LevelGround.alphamap[index1, index2, index3] = 0.0f;
                LevelGround.alphamap[index1, index2, 0] = 1f;
              }
            }
          }
        }
      }
      if (quality)
      {
        for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
        {
          for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
          {
            if (!LevelObjects.regions[(int) index1, (int) index2])
            {
              List<LevelObject> list3 = LevelObjects.objects[(int) index1, (int) index2];
              for (int index3 = 0; index3 < list3.Count; ++index3)
                list3[index3].enable();
            }
          }
        }
        for (int index1 = 2; index1 < LevelGround.data.alphamapWidth - 2; ++index1)
        {
          for (int index2 = 2; index2 < LevelGround.data.alphamapHeight - 2; ++index2)
          {
            bool flag1 = false;
            for (int index3 = 0; index3 < LevelGround.materials.Length; ++index3)
            {
              if ((double) LevelGround.alphamap[index1, index2, index3] > 0.5 && !LevelGround.materials[index3].isGenerated)
              {
                flag1 = true;
                break;
              }
            }
            if (!flag1)
            {
              RaycastHit hitInfo;
              Physics.Raycast(new Vector3((float) (-(double) LevelGround.data.size.x / 2.0 + (double) index2 / (double) LevelGround.data.alphamapWidth * (double) LevelGround.data.size.x), 256f, (float) (-(double) LevelGround.data.size.z / 2.0 + (double) index1 / (double) LevelGround.data.alphamapHeight * (double) LevelGround.data.size.z)), Vector3.down, out hitInfo, 512f, RayMasks.BLOCK_GRASS);
              byte x;
              byte y;
              if ((UnityEngine.Object) hitInfo.transform != (UnityEngine.Object) null && ((double) LevelLighting.seaLevel > 0.990000009536743 || (double) hitInfo.point.y > (double) LevelLighting.seaLevel * (double) Level.TERRAIN) && ((hitInfo.transform.tag == "Large" || hitInfo.transform.tag == "Medium" || hitInfo.transform.tag == "Environment") && Regions.tryGetCoordinate(hitInfo.transform.position, out x, out y)))
              {
                bool flag2 = true;
                for (int index3 = 0; index3 < LevelObjects.objects[(int) x, (int) y].Count; ++index3)
                {
                  LevelObject levelObject = LevelObjects.objects[(int) x, (int) y][index3];
                  if ((UnityEngine.Object) levelObject.transform == (UnityEngine.Object) hitInfo.transform)
                  {
                    ObjectAsset objectAsset = (ObjectAsset) Assets.find(EAssetType.OBJECT, levelObject.id);
                    if (objectAsset == null || objectAsset.isSnowshoe)
                    {
                      flag2 = false;
                      break;
                    }
                    break;
                  }
                }
                if (flag2)
                {
                  for (int index3 = 0; index3 < LevelGround.materials.Length; ++index3)
                  {
                    if (LevelGround.materials[index3].isFoundation)
                    {
                      for (int index4 = -1; index4 < 2; ++index4)
                      {
                        for (int index5 = -1; index5 < 2; ++index5)
                        {
                          bool flag3 = true;
                          for (int index6 = 0; index6 < LevelGround.materials.Length; ++index6)
                          {
                            if ((double) LevelGround.alphamap[index1 + index4, index2 + index5, index6] > 0.5 && !LevelGround.materials[index6].isGenerated)
                            {
                              flag3 = false;
                              break;
                            }
                          }
                          if (flag3)
                          {
                            for (int index6 = 0; index6 < LevelGround.materials.Length; ++index6)
                              LevelGround.alphamap[index1 + index4, index2 + index5, index6] = index6 != index3 ? 0.0f : 1f;
                          }
                        }
                      }
                      break;
                    }
                  }
                }
              }
            }
          }
        }
        for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
        {
          for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
          {
            if (!LevelObjects.regions[(int) index1, (int) index2])
            {
              List<LevelObject> list3 = LevelObjects.objects[(int) index1, (int) index2];
              for (int index3 = 0; index3 < list3.Count; ++index3)
                list3[index3].disable();
            }
          }
        }
        float[,,] numArray3 = new float[LevelGround.data.alphamapWidth, LevelGround.data.alphamapHeight, LevelGround.materials.Length];
        for (int index1 = 0; index1 < LevelGround.materials.Length; ++index1)
        {
          for (int index2 = 0; index2 < LevelGround.data.alphamapWidth; ++index2)
          {
            for (int index3 = 0; index3 < LevelGround.data.alphamapHeight; ++index3)
              numArray3[index2, index3, index1] = index2 == 0 || index3 == 0 || (index2 == LevelGround.data.alphamapWidth - 1 || index3 == LevelGround.data.alphamapHeight - 1) ? LevelGround.alphamap[index2, index3, index1] : (float) (0.200000002980232 * (double) LevelGround.alphamap[index2, index3, index1] + 0.200000002980232 * (double) LevelGround.alphamap[index2 - 1, index3, index1] + 0.200000002980232 * (double) LevelGround.alphamap[index2, index3 - 1, index1] + 0.200000002980232 * (double) LevelGround.alphamap[index2 + 1, index3, index1] + 0.200000002980232 * (double) LevelGround.alphamap[index2, index3 + 1, index1]);
          }
        }
        float[,,] numArray4 = LevelGround.alphamap;
        for (int index1 = 0; index1 < LevelGround.materials.Length; ++index1)
        {
          for (int index2 = 1; index2 < LevelGround.data.alphamapWidth - 1; ++index2)
          {
            for (int index3 = 1; index3 < LevelGround.data.alphamapHeight - 1; ++index3)
              numArray4[index2, index3, index1] = (float) (0.200000002980232 * (double) numArray3[index2, index3, index1] + 0.200000002980232 * (double) numArray3[index2 - 1, index3, index1] + 0.200000002980232 * (double) numArray3[index2, index3 - 1, index1] + 0.200000002980232 * (double) numArray3[index2 + 1, index3, index1] + 0.200000002980232 * (double) numArray3[index2, index3 + 1, index1]);
          }
        }
        float[,,] map1 = numArray3;
        for (int index1 = 0; index1 < LevelGround.materials.Length; ++index1)
        {
          for (int index2 = 1; index2 < LevelGround.data.alphamapWidth - 1; ++index2)
          {
            for (int index3 = 1; index3 < LevelGround.data.alphamapHeight - 1; ++index3)
              map1[index2, index3, index1] = (float) (0.200000002980232 * (double) numArray4[index2, index3, index1] + 0.200000002980232 * (double) numArray4[index2 - 1, index3, index1] + 0.200000002980232 * (double) numArray4[index2, index3 - 1, index1] + 0.200000002980232 * (double) numArray4[index2 + 1, index3, index1] + 0.200000002980232 * (double) numArray4[index2, index3 + 1, index1]);
          }
        }
        LevelGround.data.SetAlphamaps(0, 0, map1);
        LevelGround.alphamap2 = LevelGround.data2.GetAlphamaps(0, 0, LevelGround.data2.alphamapWidth, LevelGround.data2.alphamapHeight);
        for (int index1 = 0; index1 < LevelGround.data2.alphamapWidth; ++index1)
        {
          for (int index2 = 0; index2 < LevelGround.data2.alphamapHeight; ++index2)
          {
            bool flag = false;
            for (int index3 = 0; index3 < LevelGround.materials.Length; ++index3)
            {
              if (!LevelGround.materials[index3].isGenerated && (double) LevelGround.alphamap2[index1, index2, index3] > 0.5)
              {
                for (int index4 = 0; index4 < LevelGround.materials.Length; ++index4)
                  LevelGround.alphamap2[index1, index2, index4] = index4 != index3 ? 0.0f : 1f;
                flag = true;
                break;
              }
            }
            if (!flag)
            {
              float num1 = LevelGround.data2.GetSteepness((float) index2 / (float) LevelGround.data2.alphamapWidth, (float) index1 / (float) LevelGround.data2.alphamapHeight) / 48f;
              for (int index3 = 0; index3 < LevelGround.materials.Length; ++index3)
              {
                if ((double) num1 >= (double) LevelGround.materials[numArray1[index3]].steepness && (double) LevelGround.materials[numArray1[index3]].steepness >= 0.00999999977648258)
                {
                  for (int index4 = 0; index4 < LevelGround.materials.Length; ++index4)
                    LevelGround.alphamap2[index1, index2, index4] = index4 != numArray1[index3] ? 0.0f : 1f;
                  flag = true;
                  break;
                }
              }
              if (!flag)
              {
                float num2 = LevelGround.data2.GetInterpolatedHeight((float) index2 / (float) LevelGround.data2.alphamapWidth, (float) index1 / (float) LevelGround.data2.alphamapHeight) / LevelGround.data2.size.y;
                for (int index3 = 0; index3 < LevelGround.materials.Length; ++index3)
                {
                  if ((double) num2 >= (double) LevelGround.materials[numArray2[index3]].height && (double) LevelGround.materials[numArray2[index3]].height <= 0.990000009536743)
                  {
                    for (int index4 = 0; index4 < LevelGround.materials.Length; ++index4)
                      LevelGround.alphamap2[index1, index2, index4] = index4 != numArray2[index3] ? 0.0f : 1f;
                    flag = true;
                    break;
                  }
                }
                if (!flag)
                {
                  for (int index3 = 0; index3 < LevelGround.materials.Length; ++index3)
                    LevelGround.alphamap2[index1, index2, index3] = 0.0f;
                  LevelGround.alphamap2[index1, index2, 0] = 1f;
                }
              }
            }
          }
        }
        float[,,] numArray5 = new float[LevelGround.data2.alphamapWidth, LevelGround.data2.alphamapHeight, LevelGround.materials.Length];
        for (int index1 = 0; index1 < LevelGround.materials.Length; ++index1)
        {
          for (int index2 = 0; index2 < LevelGround.data2.alphamapWidth; ++index2)
          {
            for (int index3 = 0; index3 < LevelGround.data2.alphamapHeight; ++index3)
              numArray5[index2, index3, index1] = index2 == 0 || index3 == 0 || (index2 == LevelGround.data2.alphamapWidth - 1 || index3 == LevelGround.data2.alphamapHeight - 1) ? LevelGround.alphamap2[index2, index3, index1] : (float) (0.200000002980232 * (double) LevelGround.alphamap2[index2, index3, index1] + 0.200000002980232 * (double) LevelGround.alphamap2[index2 - 1, index3, index1] + 0.200000002980232 * (double) LevelGround.alphamap2[index2, index3 - 1, index1] + 0.200000002980232 * (double) LevelGround.alphamap2[index2 + 1, index3, index1] + 0.200000002980232 * (double) LevelGround.alphamap2[index2, index3 + 1, index1]);
          }
        }
        float[,,] numArray6 = LevelGround.alphamap2;
        for (int index1 = 0; index1 < LevelGround.materials.Length; ++index1)
        {
          for (int index2 = 1; index2 < LevelGround.data2.alphamapWidth - 1; ++index2)
          {
            for (int index3 = 1; index3 < LevelGround.data2.alphamapHeight - 1; ++index3)
              numArray6[index2, index3, index1] = (float) (0.200000002980232 * (double) numArray5[index2, index3, index1] + 0.200000002980232 * (double) numArray5[index2 - 1, index3, index1] + 0.200000002980232 * (double) numArray5[index2, index3 - 1, index1] + 0.200000002980232 * (double) numArray5[index2 + 1, index3, index1] + 0.200000002980232 * (double) numArray5[index2, index3 + 1, index1]);
          }
        }
        float[,,] map2 = numArray5;
        for (int index1 = 0; index1 < LevelGround.materials.Length; ++index1)
        {
          for (int index2 = 1; index2 < LevelGround.data2.alphamapWidth - 1; ++index2)
          {
            for (int index3 = 1; index3 < LevelGround.data2.alphamapHeight - 1; ++index3)
              map2[index2, index3, index1] = (float) (0.200000002980232 * (double) numArray6[index2, index3, index1] + 0.200000002980232 * (double) numArray6[index2 - 1, index3, index1] + 0.200000002980232 * (double) numArray6[index2, index3 - 1, index1] + 0.200000002980232 * (double) numArray6[index2 + 1, index3, index1] + 0.200000002980232 * (double) numArray6[index2, index3 + 1, index1]);
          }
        }
        LevelGround.data2.SetAlphamaps(0, 0, map2);
      }
      else
        LevelGround.data.SetAlphamaps(0, 0, LevelGround.alphamap);
    }

    public static void bakeDetails()
    {
      if (LevelGround.data.alphamapLayers > 0)
      {
        LevelGround.alphamap = LevelGround.data.GetAlphamaps(0, 0, LevelGround.data.alphamapWidth, LevelGround.data.alphamapHeight);
        int[,] details = new int[LevelGround.data.detailWidth, LevelGround.data.detailHeight];
        for (int layer = 0; layer < LevelGround.details.Length; ++layer)
        {
          for (int index1 = 0; index1 < LevelGround.data.detailWidth; ++index1)
          {
            for (int index2 = 0; index2 < LevelGround.data.detailHeight; ++index2)
            {
              details[index1, index2] = 0;
              if (((double) LevelLighting.seaLevel > 0.990000009536743 || (double) LevelGround.data.GetInterpolatedHeight((float) index2 / (float) LevelGround.data.detailWidth, (float) index1 / (float) LevelGround.data.detailHeight) / (double) Level.TERRAIN > (double) LevelLighting.seaLevel * 0.800000011920929) && (double) UnityEngine.Random.value < (double) LevelGround.details[layer].chance)
              {
                for (int index3 = 0; index3 < LevelGround.materials.Length; ++index3)
                {
                  if ((double) LevelGround.alphamap[index1, index2, index3] > 0.5 && (LevelGround.materials[index3].isGrassy_0 && LevelGround.details[layer].isGrass_0 || LevelGround.materials[index3].isGrassy_1 && LevelGround.details[layer].isGrass_1 || (LevelGround.materials[index3].isFlowery && LevelGround.details[layer].isFlower || LevelGround.materials[index3].isRocky && LevelGround.details[layer].isRock) || LevelGround.materials[index3].isSnowy && LevelGround.details[layer].isSnow))
                  {
                    if (LevelGround.details[layer].isRock || LevelGround.details[layer].isSnow)
                    {
                      details[index1, index2] = (int) ((double) LevelGround.details[layer].density * (double) LevelGround.alphamap[index1, index2, index3] * 16.0);
                      break;
                    }
                    if ((double) UnityEngine.Random.value < (double) LevelGround.materials[index3].chance)
                    {
                      details[index1, index2] = (int) ((double) LevelGround.details[layer].density * (double) LevelGround.materials[index3].overgrowth * (double) LevelGround.alphamap[index1, index2, index3] * 16.0);
                      break;
                    }
                    break;
                  }
                }
              }
            }
          }
          LevelGround.data.SetDetailLayer(0, 0, layer, details);
        }
        LevelGround.terrain.Flush();
      }
      GC.Collect();
    }

    public static void addSpawn(Vector3 point)
    {
      byte x;
      byte y;
      if (!Regions.tryGetCoordinate(point, out x, out y))
        return;
      ResourceSpawnpoint resourceSpawnpoint = new ResourceSpawnpoint(EditorSpawns.selectedResource, LevelGround.resources[(int) EditorSpawns.selectedResource].id, point, false);
      resourceSpawnpoint.enable();
      LevelGround.trees[(int) x, (int) y].Add(resourceSpawnpoint);
    }

    public static void removeSpawn(Vector3 point, float radius)
    {
      radius *= radius;
      for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
        {
          List<ResourceSpawnpoint> list = new List<ResourceSpawnpoint>();
          for (int index3 = 0; index3 < LevelGround.trees[(int) index1, (int) index2].Count; ++index3)
          {
            ResourceSpawnpoint resourceSpawnpoint = LevelGround.trees[(int) index1, (int) index2][index3];
            if ((double) (resourceSpawnpoint.point - point).sqrMagnitude < (double) radius)
              UnityEngine.Object.Destroy((UnityEngine.Object) resourceSpawnpoint.model.gameObject);
            else
              list.Add(resourceSpawnpoint);
          }
          LevelGround._trees[(int) index1, (int) index2] = list;
        }
      }
    }

    public static void bakeGlobalResources()
    {
      if (LevelGround.isBakingResources)
      {
        LevelGround.isBakingResources = false;
        for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
        {
          for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
          {
            if (!LevelObjects.regions[(int) index1, (int) index2])
            {
              List<LevelObject> list = LevelObjects.objects[(int) index1, (int) index2];
              for (int index3 = 0; index3 < list.Count; ++index3)
                list[index3].disable();
            }
          }
        }
      }
      else
      {
        LevelGround.isBakingResources = true;
        LevelGround.bakeResources_X = (byte) 0;
        LevelGround.bakeResources_Y = (byte) 0;
        LevelGround.bakeResources_W = Regions.WORLD_SIZE;
        LevelGround.bakeResources_H = Regions.WORLD_SIZE;
        LevelGround.bakeResources_M = (byte) 0;
        for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
        {
          for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
          {
            if (!LevelObjects.regions[(int) index1, (int) index2])
            {
              List<LevelObject> list = LevelObjects.objects[(int) index1, (int) index2];
              for (int index3 = 0; index3 < list.Count; ++index3)
                list[index3].enable();
            }
          }
        }
        LevelGround.alphamap = LevelGround.data.GetAlphamaps(0, 0, LevelGround.data.alphamapWidth, LevelGround.data.alphamapHeight);
      }
    }

    public static void bakeLocalResources()
    {
      if (LevelGround.isBakingResources)
      {
        LevelGround.isBakingResources = false;
        for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
        {
          for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
          {
            if (!LevelObjects.regions[(int) LevelGround.bakeResources_X, (int) LevelGround.bakeResources_Y])
            {
              List<LevelObject> list = LevelObjects.objects[(int) index1, (int) index2];
              for (int index3 = 0; index3 < list.Count; ++index3)
                list[index3].disable();
            }
          }
        }
      }
      else
      {
        LevelGround.isBakingResources = true;
        LevelGround.bakeResources_X = (byte) Mathf.Max((int) Editor.editor.movement.region_x - 1, 0);
        LevelGround.bakeResources_Y = (byte) Mathf.Max((int) Editor.editor.movement.region_y - 1, 0);
        LevelGround.bakeResources_W = (byte) Mathf.Min((int) Editor.editor.movement.region_x + 2, (int) Regions.WORLD_SIZE);
        LevelGround.bakeResources_H = (byte) Mathf.Min((int) Editor.editor.movement.region_y + 2, (int) Regions.WORLD_SIZE);
        LevelGround.bakeResources_M = LevelGround.bakeResources_X;
        for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
        {
          for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
          {
            if (!LevelObjects.regions[(int) LevelGround.bakeResources_X, (int) LevelGround.bakeResources_Y])
            {
              List<LevelObject> list = LevelObjects.objects[(int) LevelGround.bakeResources_X, (int) LevelGround.bakeResources_Y];
              for (int index3 = 0; index3 < list.Count; ++index3)
                list[index3].enable();
            }
          }
        }
        LevelGround.alphamap = LevelGround.data.GetAlphamaps(0, 0, LevelGround.data.alphamapWidth, LevelGround.data.alphamapHeight);
      }
    }

    private static bool bakeResource()
    {
      List<ResourceSpawnpoint> list = new List<ResourceSpawnpoint>();
      for (int index = 0; index < LevelGround.trees[(int) LevelGround.bakeResources_X, (int) LevelGround.bakeResources_Y].Count; ++index)
      {
        ResourceSpawnpoint resourceSpawnpoint = LevelGround.trees[(int) LevelGround.bakeResources_X, (int) LevelGround.bakeResources_Y][index];
        if (resourceSpawnpoint.isGenerated)
          UnityEngine.Object.Destroy((UnityEngine.Object) resourceSpawnpoint.model.gameObject);
        else
          list.Add(resourceSpawnpoint);
      }
      LevelGround.trees[(int) LevelGround.bakeResources_X, (int) LevelGround.bakeResources_Y] = list;
      LevelGround.regions[(int) LevelGround.bakeResources_X, (int) LevelGround.bakeResources_Y] = false;
      float num1 = (float) ((int) LevelGround.bakeResources_X * (int) Regions.REGION_SIZE - 4096);
      float num2 = (float) ((int) LevelGround.bakeResources_Y * (int) Regions.REGION_SIZE - 4096);
      if ((double) num1 < (double) LevelGround.terrain.transform.position.x || (double) num2 < (double) LevelGround.terrain.transform.position.z || ((double) num1 >= (double) LevelGround.terrain.transform.position.x + (double) LevelGround.data.size.x || (double) num2 >= (double) LevelGround.terrain.transform.position.z + (double) LevelGround.data.size.z))
        return true;
      int num3 = 0;
      while (num3 < (int) Regions.REGION_SIZE)
      {
        int num4 = 0;
        while (num4 < (int) Regions.REGION_SIZE)
        {
          byte newType = (byte) UnityEngine.Random.Range(0, LevelGround.resources.Length);
          if ((double) UnityEngine.Random.value > 1.0 - (double) LevelGround.resources[(int) newType].density * 0.25 && (double) UnityEngine.Random.value < (double) LevelGround.resources[(int) newType].chance)
          {
            int index1 = (int) (((double) num2 + (double) num3 - (double) LevelGround.terrain.transform.position.z) / (double) LevelGround.data.size.z * (double) LevelGround.data.alphamapHeight);
            int index2 = (int) (((double) num1 + (double) num4 - (double) LevelGround.terrain.transform.position.x) / (double) LevelGround.data.size.x * (double) LevelGround.data.alphamapWidth);
            for (int index3 = 0; index3 < LevelGround.materials.Length; ++index3)
            {
              if ((double) LevelGround.alphamap[index1, index2, index3] > 0.75 && (LevelGround.materials[index3].isGrassy_0 && LevelGround.resources[(int) newType].isTree_0 || LevelGround.materials[index3].isGrassy_1 && LevelGround.resources[(int) newType].isTree_1 || (LevelGround.materials[index3].isFlowery && LevelGround.resources[(int) newType].isFlower || LevelGround.materials[index3].isRocky && LevelGround.resources[(int) newType].isRock) || LevelGround.materials[index3].isSnowy && LevelGround.resources[(int) newType].isSnow))
              {
                if ((double) UnityEngine.Random.value < (double) LevelGround.materials[index3].chance && (LevelGround.resources[(int) newType].isRock || LevelGround.resources[(int) newType].isSnow || (double) UnityEngine.Random.value < (double) LevelGround.materials[index3].overgrowth) && ((index1 == 0 || (double) LevelGround.alphamap[index1 - 1, index2, index3] > 0.75) && (index2 == 0 || (double) LevelGround.alphamap[index1, index2 - 1, index3] > 0.75) && ((index1 == LevelGround.data.alphamapWidth - 1 || (double) LevelGround.alphamap[index1 + 1, index2, index3] > 0.75) && (index2 == LevelGround.data.alphamapHeight - 1 || (double) LevelGround.alphamap[index1, index2 + 1, index3] > 0.75))))
                {
                  RaycastHit hitInfo;
                  Physics.Raycast(new Vector3(num1 + (float) num4, 256f, num2 + (float) num3), Vector3.down, out hitInfo, 256f);
                  if ((UnityEngine.Object) hitInfo.transform != (UnityEngine.Object) null && ((UnityEngine.Object) hitInfo.transform == (UnityEngine.Object) LevelGround.models || (UnityEngine.Object) hitInfo.transform == (UnityEngine.Object) LevelGround.models2))
                  {
                    ResourceAsset resourceAsset = (ResourceAsset) Assets.find(EAssetType.RESOURCE, LevelGround.resources[(int) newType].id);
                    if (resourceAsset != null)
                    {
                      bool flag1 = true;
                      for (byte index4 = (byte) 0; (int) index4 < LevelPlayers.spawns.Count; ++index4)
                      {
                        if ((double) (hitInfo.point - LevelPlayers.spawns[(int) index4].point).sqrMagnitude < (double) resourceAsset.radius * (double) resourceAsset.radius)
                        {
                          flag1 = false;
                          break;
                        }
                      }
                      if (flag1)
                      {
                        Collider[] colliderArray = Physics.OverlapSphere(hitInfo.point, resourceAsset.radius, RayMasks.BLOCK_RESOURCE);
                        bool flag2 = false;
                        for (int index4 = 0; index4 < colliderArray.Length; ++index4)
                        {
                          byte x;
                          byte y;
                          if (Regions.tryGetCoordinate(colliderArray[index4].transform.position, out x, out y))
                          {
                            bool flag3 = true;
                            for (int index5 = 0; index5 < LevelObjects.objects[(int) x, (int) y].Count; ++index5)
                            {
                              LevelObject levelObject = LevelObjects.objects[(int) x, (int) y][index5];
                              if ((UnityEngine.Object) levelObject.transform == (UnityEngine.Object) colliderArray[index4].transform)
                              {
                                ObjectAsset objectAsset = (ObjectAsset) Assets.find(EAssetType.OBJECT, levelObject.id);
                                if (resourceAsset == null || objectAsset.isSnowshoe)
                                {
                                  flag3 = false;
                                  break;
                                }
                                break;
                              }
                            }
                            if (flag3)
                            {
                              flag2 = true;
                              break;
                            }
                          }
                        }
                        if (!flag2)
                        {
                          LevelGround.trees[(int) LevelGround.bakeResources_X, (int) LevelGround.bakeResources_Y].Add(new ResourceSpawnpoint(newType, LevelGround.resources[(int) newType].id, hitInfo.point, true));
                          break;
                        }
                        break;
                      }
                      break;
                    }
                    break;
                  }
                  break;
                }
                break;
              }
            }
          }
          num4 += 2;
        }
        num3 += 2;
      }
      LevelGround.onRegionUpdated(byte.MaxValue, byte.MaxValue, Editor.editor.movement.region_x, Editor.editor.movement.region_y);
      return false;
    }

    public static void updateVisibility(bool isVisible)
    {
      if (isVisible)
        LevelGround.terrain.editorRenderFlags = (TerrainRenderFlags) 5;
      else
        LevelGround.terrain.editorRenderFlags = TerrainRenderFlags.heightmap;
    }

    public static void load(ushort size)
    {
      if (Level.isEditor)
        LevelGround.MASK = (Texture2D) Resources.Load("Edit/Mask");
      LevelGround.isBakingResources = false;
      LevelGround.bakeResources_X = (byte) 0;
      LevelGround.bakeResources_Y = (byte) 0;
      LevelGround.bakeResources_W = (byte) 0;
      LevelGround.bakeResources_H = (byte) 0;
      LevelGround.bakeResources_M = (byte) 0;
      LevelGround._models = new GameObject().transform;
      LevelGround.models.name = "Ground";
      LevelGround.models.parent = Level.level;
      LevelGround.models.tag = "Ground";
      LevelGround.models.gameObject.layer = LayerMasks.GROUND;
      LevelGround._terrain = LevelGround.models.gameObject.AddComponent<Terrain>();
      LevelGround.terrain.name = "Ground";
      LevelGround.terrain.heightmapPixelError = !Level.isEditor ? 16f : 1f;
      LevelGround.terrain.basemapDistance = 256f;
      LevelGround.terrain.transform.position = new Vector3((float) ((int) -size / 2), 0.0f, (float) ((int) -size / 2));
      LevelGround.terrain.reflectionProbeUsage = ReflectionProbeUsage.Off;
      LevelGround.terrain.treeDistance = 0.0f;
      LevelGround.terrain.treeBillboardDistance = 0.0f;
      LevelGround.terrain.treeCrossFadeLength = 0.0f;
      LevelGround.terrain.treeMaximumFullLODCount = 0;
      LevelGround._data = new TerrainData();
      LevelGround.data.name = "Ground";
      LevelGround.data.heightmapResolution = (int) size / 8;
      LevelGround.data.alphamapResolution = (int) size / 4;
      LevelGround.data.SetDetailResolution((int) size / 4, 32);
      LevelGround.data.baseMapResolution = (int) size / 16;
      LevelGround.data.size = new Vector3((float) size, Level.TERRAIN, (float) size);
      LevelGround.data.wavingGrassTint = Color.white;
      byte num1 = (byte) 0;
      byte num2 = (byte) 0;
      if (ReadWrite.fileExists(Level.info.path + "/Terrain/Heights.dat", false, false))
      {
        Block block = ReadWrite.readBlock(Level.info.path + "/Terrain/Heights.dat", false, false, (byte) 0);
        num1 = block.readByte();
        num2 = block.readByte();
      }
      if (ReadWrite.fileExists(Level.info.path + "/Terrain/Heightmap.png", false, false))
      {
        byte[] numArray1 = ReadWrite.readBytes(Level.info.path + "/Terrain/Heightmap.png", false, false);
        Texture2D texture2D = new Texture2D(LevelGround.data.heightmapWidth, LevelGround.data.heightmapHeight, TextureFormat.ARGB32, false);
        texture2D.name = "Texture";
        texture2D.hideFlags = HideFlags.HideAndDontSave;
        texture2D.LoadImage(numArray1);
        float[,] heights = new float[texture2D.width, texture2D.height];
        for (int x = 0; x < texture2D.width; ++x)
        {
          for (int y = 0; y < texture2D.height; ++y)
          {
            if ((int) num1 > 0)
            {
              byte[] numArray2 = new byte[4]
              {
                (byte) ((double) texture2D.GetPixel(x, y).r * (double) byte.MaxValue),
                (byte) ((double) texture2D.GetPixel(x, y).g * (double) byte.MaxValue),
                (byte) ((double) texture2D.GetPixel(x, y).b * (double) byte.MaxValue),
                (byte) ((double) texture2D.GetPixel(x, y).a * (double) byte.MaxValue)
              };
              heights[x, y] = BitConverter.ToSingle(numArray2, 0);
            }
            else
              heights[x, y] = texture2D.GetPixel(x, y).r;
          }
        }
        LevelGround.data.SetHeights(0, 0, heights);
        LevelGround.hashHeights = Hash.SHA1(numArray1);
      }
      else
      {
        float[,] heights = new float[LevelGround.data.heightmapWidth, LevelGround.data.heightmapHeight];
        for (int index1 = 0; index1 < LevelGround.data.heightmapWidth; ++index1)
        {
          for (int index2 = 0; index2 < LevelGround.data.heightmapHeight; ++index2)
            heights[index1, index2] = 0.03f;
        }
        LevelGround.data.SetHeights(0, 0, heights);
        LevelGround.hashHeights = new byte[20];
      }
      if (ReadWrite.fileExists(Level.info.path + "/Terrain/Materials.unity3d", false, false))
      {
        Bundle bundle = Bundles.getBundle(Level.info.path + "/Terrain/Materials.unity3d", false);
        UnityEngine.Object[] objectArray = bundle.load();
        bundle.unload();
        LevelGround._materials = new GroundMaterial[(int) LevelGround.ALPHAMAPS * 4];
        SplatPrototype[] splatPrototypeArray = new SplatPrototype[LevelGround.materials.Length];
        for (int index = 0; index < LevelGround.materials.Length; ++index)
        {
          splatPrototypeArray[index] = new SplatPrototype();
          splatPrototypeArray[index].texture = (Texture2D) objectArray[index];
          splatPrototypeArray[index].tileSize = new Vector2((float) splatPrototypeArray[index].texture.width / 4f, (float) splatPrototypeArray[index].texture.height / 4f);
          LevelGround.materials[index] = new GroundMaterial(splatPrototypeArray[index]);
        }
        LevelGround.data.splatPrototypes = splatPrototypeArray;
      }
      else
        LevelGround._materials = new GroundMaterial[0];
      if (ReadWrite.fileExists(Level.info.path + "/Terrain/Materials.dat", false, false))
      {
        Block block = ReadWrite.readBlock(Level.info.path + "/Terrain/Materials.dat", false, false, (byte) 0);
        byte num3 = block.readByte();
        if ((int) num3 > 1)
        {
          byte num4 = block.readByte();
          for (byte index = (byte) 0; (int) index < (int) num4; ++index)
          {
            if ((int) index >= LevelGround.materials.Length)
            {
              double num5 = (double) block.readSingle();
              double num6 = (double) block.readSingle();
              double num7 = (double) block.readSingle();
              double num8 = (double) block.readSingle();
              block.readBoolean();
              block.readBoolean();
              block.readBoolean();
              block.readBoolean();
              block.readBoolean();
              block.readBoolean();
              block.readBoolean();
            }
            else
            {
              LevelGround.materials[(int) index].overgrowth = block.readSingle();
              LevelGround.materials[(int) index].chance = block.readSingle();
              LevelGround.materials[(int) index].steepness = block.readSingle();
              if ((int) num3 > 3)
              {
                LevelGround.materials[(int) index].height = block.readSingle();
              }
              else
              {
                LevelGround.materials[(int) index].height = block.readSingle() / 2f;
                if ((double) LevelGround.materials[(int) index].height == 0.5)
                  LevelGround.materials[(int) index].height = 1f;
              }
              LevelGround.materials[(int) index].isGrassy_0 = block.readBoolean();
              LevelGround.materials[(int) index].isGrassy_1 = block.readBoolean();
              LevelGround.materials[(int) index].isFlowery = block.readBoolean();
              LevelGround.materials[(int) index].isRocky = block.readBoolean();
              if ((int) num3 > 2)
                LevelGround.materials[(int) index].isSnowy = block.readBoolean();
              LevelGround.materials[(int) index].isFoundation = block.readBoolean();
              LevelGround.materials[(int) index].isGenerated = block.readBoolean();
            }
          }
        }
      }
      LevelGround.alphamap = new float[LevelGround.data.alphamapWidth, LevelGround.data.alphamapHeight, LevelGround.materials.Length];
      for (int index = 0; index < (int) LevelGround.ALPHAMAPS; ++index)
      {
        if (ReadWrite.fileExists(string.Concat(new object[4]
        {
          (object) Level.info.path,
          (object) "/Terrain/Alphamap_",
          (object) index,
          (object) ".png"
        }), false, false))
        {
          byte[] data = ReadWrite.readBytes(string.Concat(new object[4]
          {
            (object) Level.info.path,
            (object) "/Terrain/Alphamap_",
            (object) index,
            (object) ".png"
          }), false, false);
          Texture2D texture2D = new Texture2D(LevelGround.data.heightmapWidth, LevelGround.data.heightmapHeight, TextureFormat.ARGB32, false);
          texture2D.name = "Texture";
          texture2D.hideFlags = HideFlags.HideAndDontSave;
          texture2D.LoadImage(data);
          for (int x = 0; x < texture2D.width; ++x)
          {
            for (int y = 0; y < texture2D.height; ++y)
            {
              Color pixel = texture2D.GetPixel(x, y);
              LevelGround.alphamap[x, y, index * 4] = pixel.r;
              LevelGround.alphamap[x, y, index * 4 + 1] = pixel.g;
              LevelGround.alphamap[x, y, index * 4 + 2] = pixel.b;
              LevelGround.alphamap[x, y, index * 4 + 3] = pixel.a;
            }
          }
        }
      }
      LevelGround.data.SetAlphamaps(0, 0, LevelGround.alphamap);
      if (!Dedicator.isDedicated)
      {
        if (ReadWrite.fileExists(Level.info.path + "/Terrain/Details.unity3d", false, false))
        {
          Bundle bundle = Bundles.getBundle(Level.info.path + "/Terrain/Details.unity3d", false);
          UnityEngine.Object[] objectArray1 = bundle.load(typeof (Texture2D));
          UnityEngine.Object[] objectArray2 = bundle.load(typeof (GameObject));
          bundle.unload();
          List<GroundDetail> list1 = new List<GroundDetail>();
          List<DetailPrototype> list2 = new List<DetailPrototype>();
          for (int index = 0; index < objectArray1.Length; ++index)
          {
            if (objectArray1[index].name.IndexOf("Texture_") == -1)
            {
              DetailPrototype newPrototype = new DetailPrototype();
              newPrototype.prototypeTexture = (Texture2D) objectArray1[index];
              newPrototype.renderMode = DetailRenderMode.Grass;
              newPrototype.dryColor = Color.white;
              newPrototype.healthyColor = Color.white;
              float num3 = (float) newPrototype.prototypeTexture.width / 20f;
              float num4 = (float) newPrototype.prototypeTexture.height / 20f;
              newPrototype.minWidth = num3 * 1.25f;
              newPrototype.maxWidth = num3 * 1.75f;
              newPrototype.minHeight = num4 * 1.25f;
              newPrototype.maxHeight = num4 * 1.75f;
              newPrototype.noiseSpread = 1f;
              list2.Add(newPrototype);
              list1.Add(new GroundDetail(newPrototype));
            }
          }
          for (int index = 0; index < objectArray2.Length; ++index)
          {
            if (objectArray2[index].name.IndexOf("Model_") == -1)
            {
              DetailPrototype newPrototype = new DetailPrototype();
              newPrototype.prototype = (GameObject) objectArray2[index];
              newPrototype.renderMode = DetailRenderMode.VertexLit;
              newPrototype.usePrototypeMesh = true;
              newPrototype.dryColor = new Color(0.95f, 0.95f, 0.95f);
              newPrototype.healthyColor = Color.white;
              newPrototype.maxWidth = 1.5f;
              newPrototype.maxHeight = 1.5f;
              newPrototype.noiseSpread = 1f;
              list2.Add(newPrototype);
              list1.Add(new GroundDetail(newPrototype));
            }
          }
          LevelGround.data.detailPrototypes = list2.ToArray();
          LevelGround._details = list1.ToArray();
        }
        else
          LevelGround._details = new GroundDetail[0];
        if (ReadWrite.fileExists(Level.info.path + "/Terrain/Details.dat", false, false))
        {
          Block block = ReadWrite.readBlock(Level.info.path + "/Terrain/Details.dat", false, false, (byte) 0);
          byte num3 = block.readByte();
          byte num4 = block.readByte();
          for (byte index = (byte) 0; (int) index < (int) num4; ++index)
          {
            if ((int) index >= LevelGround.details.Length)
            {
              double num5 = (double) block.readSingle();
              double num6 = (double) block.readSingle();
              block.readBoolean();
              block.readBoolean();
              block.readBoolean();
              block.readBoolean();
              block.readBoolean();
            }
            else
            {
              LevelGround.details[(int) index].density = block.readSingle();
              LevelGround.details[(int) index].chance = block.readSingle();
              LevelGround.details[(int) index].isGrass_0 = block.readBoolean();
              LevelGround.details[(int) index].isGrass_1 = block.readBoolean();
              LevelGround.details[(int) index].isFlower = block.readBoolean();
              LevelGround.details[(int) index].isRock = block.readBoolean();
              if ((int) num3 > 2)
                LevelGround.details[(int) index].isSnow = block.readBoolean();
            }
          }
        }
      }
      LevelGround.terrain.terrainData = LevelGround.data;
      LevelGround.terrain.detailObjectDensity = 0.0f;
      LevelGround.terrain.detailObjectDistance = 0.0f;
      LevelGround.terrain.terrainData.wavingGrassAmount = 0.0f;
      LevelGround.terrain.terrainData.wavingGrassSpeed = 1f;
      LevelGround.terrain.terrainData.wavingGrassStrength = 1f;
      LevelGround.collider = LevelGround.models.gameObject.AddComponent<TerrainCollider>();
      LevelGround.collider.terrainData = LevelGround.data;
      if (!Dedicator.isDedicated)
        LevelGround.bakeDetails();
      LevelGround._trees = new List<ResourceSpawnpoint>[(int) Regions.WORLD_SIZE, (int) Regions.WORLD_SIZE];
      LevelGround._regions = new bool[(int) Regions.WORLD_SIZE, (int) Regions.WORLD_SIZE];
      Asset[] assetArray = Assets.find(EAssetType.RESOURCE);
      if (ReadWrite.fileExists(Level.info.path + "/Terrain/Resources.dat", false, false))
      {
        Block block = ReadWrite.readBlock(Level.info.path + "/Terrain/Resources.dat", false, false, (byte) 0);
        byte num3 = block.readByte();
        if ((int) num3 > 3)
        {
          LevelGround._resources = new GroundResource[assetArray.Length];
          for (int index = 0; index < LevelGround.resources.Length; ++index)
            LevelGround.resources[index] = new GroundResource(assetArray[index].id);
        }
        else
        {
          LevelGround._resources = new GroundResource[assetArray.Length];
          for (int index = 0; index < 18; ++index)
            LevelGround.resources[index] = new GroundResource((ushort) (index + 1));
          for (int index = 18; index < LevelGround.resources.Length; ++index)
            LevelGround.resources[index] = new GroundResource(assetArray[index].id);
        }
        if ((int) num3 > 1)
        {
          byte num4 = block.readByte();
          for (byte index = (byte) 0; (int) index < (int) num4; ++index)
          {
            ushort num5 = (int) num3 < 6 ? LevelGround.resources[(int) index].id : block.readUInt16();
            float num6 = block.readSingle();
            float num7 = block.readSingle();
            bool flag1 = block.readBoolean();
            bool flag2 = block.readBoolean();
            bool flag3 = block.readBoolean();
            bool flag4 = block.readBoolean();
            bool flag5 = block.readBoolean();
            foreach (GroundResource groundResource in LevelGround.resources)
            {
              if ((int) groundResource.id == (int) num5)
              {
                groundResource.density = num6;
                groundResource.chance = num7;
                groundResource.isTree_0 = flag1;
                groundResource.isTree_1 = flag2;
                groundResource.isFlower = flag3;
                groundResource.isRock = flag4;
                groundResource.isSnow = flag5;
              }
            }
          }
        }
      }
      else
      {
        LevelGround._resources = new GroundResource[assetArray.Length];
        for (int index = 0; index < LevelGround.resources.Length; ++index)
          LevelGround.resources[index] = new GroundResource(assetArray[index].id);
      }
      for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
          LevelGround.trees[(int) index1, (int) index2] = new List<ResourceSpawnpoint>();
      }
      if (ReadWrite.fileExists(Level.info.path + "/Terrain/Trees.dat", false, false))
      {
        River river = new River(Level.info.path + "/Terrain/Trees.dat", false);
        byte num3 = river.readByte();
        bool flag = true;
        if ((int) num3 > 3)
        {
          for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
          {
            for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
            {
              ushort num4 = river.readUInt16();
              for (ushort index3 = (ushort) 0; (int) index3 < (int) num4; ++index3)
              {
                if ((int) num3 > 4)
                {
                  ushort newID = river.readUInt16();
                  Vector3 newPoint = river.readSingleVector3();
                  bool newGenerated = river.readBoolean();
                  byte newType = (byte) 0;
                  while ((int) newType < LevelGround.resources.Length && (int) LevelGround.resources[(int) newType].id != (int) newID)
                    ++newType;
                  ResourceSpawnpoint resourceSpawnpoint = new ResourceSpawnpoint(newType, newID, newPoint, newGenerated);
                  if (resourceSpawnpoint.asset == null)
                    flag = false;
                  LevelGround.trees[(int) index1, (int) index2].Add(resourceSpawnpoint);
                }
                else
                {
                  byte newType = river.readByte();
                  Vector3 newPoint = river.readSingleVector3();
                  bool newGenerated = river.readBoolean();
                  if ((int) newType < LevelGround.resources.Length)
                  {
                    ushort id = LevelGround.resources[(int) newType].id;
                    ResourceSpawnpoint resourceSpawnpoint = new ResourceSpawnpoint(newType, id, newPoint, newGenerated);
                    if (resourceSpawnpoint.asset == null)
                      flag = false;
                    LevelGround.trees[(int) index1, (int) index2].Add(resourceSpawnpoint);
                  }
                }
              }
            }
          }
        }
        LevelGround.hashTrees = !flag ? new byte[20] : river.getHash();
        river.closeRiver();
      }
      else
        LevelGround.hashTrees = new byte[20];
      LevelGround._hash = Hash.combine(LevelGround.hashHeights, LevelGround.hashTrees);
      if (!Dedicator.isDedicated)
      {
        LevelGround._models2 = new GameObject().transform;
        LevelGround.models2.name = "Ground2";
        LevelGround.models2.parent = Level.level;
        LevelGround.models2.tag = "Ground2";
        LevelGround.models2.gameObject.layer = LayerMasks.GROUND2;
        LevelGround._terrain2 = LevelGround.models2.gameObject.AddComponent<Terrain>();
        LevelGround.terrain2.name = "Ground2";
        LevelGround.terrain2.heightmapPixelError = !Level.isEditor ? 32f : 1f;
        LevelGround.terrain2.basemapDistance = 256f;
        LevelGround.terrain2.transform.position = new Vector3((float) -size, 0.0f, (float) -size);
        LevelGround.terrain2.reflectionProbeUsage = ReflectionProbeUsage.Off;
        LevelGround.terrain2.treeDistance = 0.0f;
        LevelGround.terrain2.treeBillboardDistance = 0.0f;
        LevelGround.terrain2.treeCrossFadeLength = 0.0f;
        LevelGround.terrain2.treeMaximumFullLODCount = 0;
        LevelGround._data2 = new TerrainData();
        LevelGround.data2.name = "Ground2";
        LevelGround.data2.heightmapResolution = (int) size / 16;
        LevelGround.data2.alphamapResolution = (int) size / 8;
        LevelGround.data2.SetDetailResolution((int) size / 8, 32);
        LevelGround.data2.baseMapResolution = (int) size / 16;
        LevelGround.data2.size = new Vector3((float) ((int) size * 2), Level.TERRAIN, (float) ((int) size * 2));
        LevelGround.data2.splatPrototypes = LevelGround.data.splatPrototypes;
        if (ReadWrite.fileExists(Level.info.path + "/Terrain/Heightmap2.png", false, false))
        {
          byte[] data = ReadWrite.readBytes(Level.info.path + "/Terrain/Heightmap2.png", false, false);
          Texture2D texture2D = new Texture2D(LevelGround.data2.heightmapWidth, LevelGround.data2.heightmapHeight, TextureFormat.ARGB32, false);
          texture2D.name = "Texture";
          texture2D.hideFlags = HideFlags.HideAndDontSave;
          texture2D.LoadImage(data);
          float[,] heights = new float[texture2D.width, texture2D.height];
          for (int x = 0; x < texture2D.width; ++x)
          {
            for (int y = 0; y < texture2D.height; ++y)
            {
              if ((int) num2 > 0)
              {
                byte[] numArray = new byte[4]
                {
                  (byte) ((double) texture2D.GetPixel(x, y).r * (double) byte.MaxValue),
                  (byte) ((double) texture2D.GetPixel(x, y).g * (double) byte.MaxValue),
                  (byte) ((double) texture2D.GetPixel(x, y).b * (double) byte.MaxValue),
                  (byte) ((double) texture2D.GetPixel(x, y).a * (double) byte.MaxValue)
                };
                heights[x, y] = BitConverter.ToSingle(numArray, 0);
              }
              else
                heights[x, y] = texture2D.GetPixel(x, y).r;
            }
          }
          LevelGround.data2.SetHeights(0, 0, heights);
        }
        else
        {
          float[,] heights = new float[LevelGround.data2.heightmapWidth, LevelGround.data2.heightmapHeight];
          for (int index1 = 0; index1 < LevelGround.data2.heightmapWidth; ++index1)
          {
            for (int index2 = 0; index2 < LevelGround.data2.heightmapHeight; ++index2)
              heights[index1, index2] = 0.0f;
          }
          LevelGround.data2.SetHeights(0, 0, heights);
        }
        LevelGround.alphamap2 = new float[LevelGround.data2.alphamapWidth, LevelGround.data2.alphamapHeight, LevelGround.materials.Length];
        for (int index = 0; index < (int) LevelGround.ALPHAMAPS; ++index)
        {
          if (ReadWrite.fileExists(string.Concat(new object[4]
          {
            (object) Level.info.path,
            (object) "/Terrain/Alphamap2_",
            (object) index,
            (object) ".png"
          }), false, false))
          {
            byte[] data = ReadWrite.readBytes(string.Concat(new object[4]
            {
              (object) Level.info.path,
              (object) "/Terrain/Alphamap2_",
              (object) index,
              (object) ".png"
            }), false, false);
            Texture2D texture2D = new Texture2D(LevelGround.data2.heightmapWidth, LevelGround.data2.heightmapHeight, TextureFormat.ARGB32, false);
            texture2D.name = "Texture";
            texture2D.hideFlags = HideFlags.HideAndDontSave;
            texture2D.LoadImage(data);
            for (int x = 0; x < texture2D.width; ++x)
            {
              for (int y = 0; y < texture2D.height; ++y)
              {
                Color pixel = texture2D.GetPixel(x, y);
                LevelGround.alphamap2[x, y, index * 4] = pixel.r;
                LevelGround.alphamap2[x, y, index * 4 + 1] = pixel.g;
                LevelGround.alphamap2[x, y, index * 4 + 2] = pixel.b;
                LevelGround.alphamap2[x, y, index * 4 + 3] = pixel.a;
              }
            }
          }
        }
        LevelGround.data2.SetAlphamaps(0, 0, LevelGround.alphamap2);
        LevelGround.terrain2.terrainData = LevelGround.data2;
        LevelGround.collider2 = LevelGround.models2.gameObject.AddComponent<TerrainCollider>();
        LevelGround.collider2.terrainData = LevelGround.data2;
        LevelGround.data2.wavingGrassTint = Color.white;
      }
      if (!Level.isEditor)
        return;
      LevelGround.reunHeight = new float[4][,];
      LevelGround.frameHeight = 0;
      LevelGround.reunMaterial = new float[4][,,];
      LevelGround.frameMaterial = 0;
      LevelGround.registerHeight();
      LevelGround.registerMaterial();
    }

    public static void save()
    {
      float[,] heights1 = LevelGround.data.GetHeights(0, 0, LevelGround.data.heightmapWidth, LevelGround.data.heightmapHeight);
      Texture2D texture2D1 = new Texture2D(LevelGround.data.heightmapWidth, LevelGround.data.heightmapHeight, TextureFormat.ARGB32, false);
      texture2D1.name = "Texture";
      texture2D1.hideFlags = HideFlags.HideAndDontSave;
      for (int x = 0; x < texture2D1.width; ++x)
      {
        for (int y = 0; y < texture2D1.height; ++y)
        {
          byte[] bytes = BitConverter.GetBytes(heights1[x, y]);
          texture2D1.SetPixel(x, y, new Color((float) bytes[0] / (float) byte.MaxValue, (float) bytes[1] / (float) byte.MaxValue, (float) bytes[2] / (float) byte.MaxValue, (float) bytes[3] / (float) byte.MaxValue));
        }
      }
      ReadWrite.writeBytes(Level.info.path + "/Terrain/Heightmap.png", false, false, texture2D1.EncodeToPNG());
      byte[] numArray = (byte[]) null;
      LevelGround.alphamap = LevelGround.data.GetAlphamaps(0, 0, LevelGround.data.alphamapWidth, LevelGround.data.alphamapHeight);
      Texture2D texture2D2 = new Texture2D(LevelGround.data.alphamapWidth, LevelGround.data.alphamapHeight, TextureFormat.ARGB32, false);
      texture2D2.name = "Texture";
      texture2D2.hideFlags = HideFlags.HideAndDontSave;
      for (int index = 0; index < (int) LevelGround.ALPHAMAPS; ++index)
      {
        for (int x = 0; x < texture2D2.width; ++x)
        {
          for (int y = 0; y < texture2D2.height; ++y)
            texture2D2.SetPixel(x, y, new Color(LevelGround.alphamap[x, y, index * 4], LevelGround.alphamap[x, y, index * 4 + 1], LevelGround.alphamap[x, y, index * 4 + 2], LevelGround.alphamap[x, y, index * 4 + 3]));
        }
        byte[] bytes = texture2D2.EncodeToPNG();
        ReadWrite.writeBytes(string.Concat(new object[4]
        {
          (object) Level.info.path,
          (object) "/Terrain/Alphamap_",
          (object) index,
          (object) ".png"
        }), false, false, bytes);
      }
      Block block1 = new Block();
      block1.writeByte(LevelGround.SAVEDATA_HEIGHTS_VERSION);
      block1.writeByte(LevelGround.SAVEDATA_HEIGHTS2_VERSION);
      ReadWrite.writeBlock(Level.info.path + "/Terrain/Heights.dat", false, false, block1);
      float[,] heights2 = LevelGround.data2.GetHeights(0, 0, LevelGround.data2.heightmapWidth, LevelGround.data2.heightmapHeight);
      Texture2D texture2D3 = new Texture2D(LevelGround.data2.heightmapWidth, LevelGround.data2.heightmapHeight, TextureFormat.ARGB32, false);
      texture2D3.name = "Texture";
      texture2D3.hideFlags = HideFlags.HideAndDontSave;
      for (int x = 0; x < texture2D3.width; ++x)
      {
        for (int y = 0; y < texture2D3.height; ++y)
        {
          byte[] bytes = BitConverter.GetBytes(heights2[x, y]);
          texture2D3.SetPixel(x, y, new Color((float) bytes[0] / (float) byte.MaxValue, (float) bytes[1] / (float) byte.MaxValue, (float) bytes[2] / (float) byte.MaxValue, (float) bytes[3] / (float) byte.MaxValue));
        }
      }
      ReadWrite.writeBytes(Level.info.path + "/Terrain/Heightmap2.png", false, false, texture2D3.EncodeToPNG());
      numArray = (byte[]) null;
      LevelGround.alphamap = LevelGround.data2.GetAlphamaps(0, 0, LevelGround.data2.alphamapWidth, LevelGround.data2.alphamapHeight);
      Texture2D texture2D4 = new Texture2D(LevelGround.data2.alphamapWidth, LevelGround.data2.alphamapHeight, TextureFormat.ARGB32, false);
      texture2D4.name = "Texture";
      texture2D4.hideFlags = HideFlags.HideAndDontSave;
      for (int index = 0; index < (int) LevelGround.ALPHAMAPS; ++index)
      {
        for (int x = 0; x < texture2D4.width; ++x)
        {
          for (int y = 0; y < texture2D4.height; ++y)
            texture2D4.SetPixel(x, y, new Color(LevelGround.alphamap[x, y, index * 4], LevelGround.alphamap[x, y, index * 4 + 1], LevelGround.alphamap[x, y, index * 4 + 2], LevelGround.alphamap[x, y, index * 4 + 3]));
        }
        byte[] bytes = texture2D4.EncodeToPNG();
        ReadWrite.writeBytes(string.Concat(new object[4]
        {
          (object) Level.info.path,
          (object) "/Terrain/Alphamap2_",
          (object) index,
          (object) ".png"
        }), false, false, bytes);
      }
      Block block2 = new Block();
      block2.writeByte(LevelGround.SAVEDATA_MATERIALS_VERSION);
      block2.writeByte((byte) LevelGround.materials.Length);
      for (byte index = (byte) 0; (int) index < LevelGround.materials.Length; ++index)
      {
        block2.writeSingle(LevelGround.materials[(int) index].overgrowth);
        block2.writeSingle(LevelGround.materials[(int) index].chance);
        block2.writeSingle(LevelGround.materials[(int) index].steepness);
        block2.writeSingle(LevelGround.materials[(int) index].height);
        block2.writeBoolean(LevelGround.materials[(int) index].isGrassy_0);
        block2.writeBoolean(LevelGround.materials[(int) index].isGrassy_1);
        block2.writeBoolean(LevelGround.materials[(int) index].isFlowery);
        block2.writeBoolean(LevelGround.materials[(int) index].isRocky);
        block2.writeBoolean(LevelGround.materials[(int) index].isSnowy);
        block2.writeBoolean(LevelGround.materials[(int) index].isFoundation);
        block2.writeBoolean(LevelGround.materials[(int) index].isGenerated);
      }
      ReadWrite.writeBlock(Level.info.path + "/Terrain/Materials.dat", false, false, block2);
      Block block3 = new Block();
      block3.writeByte(LevelGround.SAVEDATA_DETAILS_VERSION);
      block3.writeByte((byte) LevelGround.details.Length);
      for (byte index = (byte) 0; (int) index < LevelGround.details.Length; ++index)
      {
        block3.writeSingle(LevelGround.details[(int) index].density);
        block3.writeSingle(LevelGround.details[(int) index].chance);
        block3.writeBoolean(LevelGround.details[(int) index].isGrass_0);
        block3.writeBoolean(LevelGround.details[(int) index].isGrass_1);
        block3.writeBoolean(LevelGround.details[(int) index].isFlower);
        block3.writeBoolean(LevelGround.details[(int) index].isRock);
        block3.writeBoolean(LevelGround.details[(int) index].isSnow);
      }
      ReadWrite.writeBlock(Level.info.path + "/Terrain/Details.dat", false, false, block3);
      Block block4 = new Block();
      block4.writeByte(LevelGround.SAVEDATA_RESOURCES_VERSION);
      block4.writeByte((byte) LevelGround.resources.Length);
      for (byte index = (byte) 0; (int) index < LevelGround.resources.Length; ++index)
      {
        block4.writeUInt16(LevelGround.resources[(int) index].id);
        block4.writeSingle(LevelGround.resources[(int) index].density);
        block4.writeSingle(LevelGround.resources[(int) index].chance);
        block4.writeBoolean(LevelGround.resources[(int) index].isTree_0);
        block4.writeBoolean(LevelGround.resources[(int) index].isTree_1);
        block4.writeBoolean(LevelGround.resources[(int) index].isFlower);
        block4.writeBoolean(LevelGround.resources[(int) index].isRock);
        block4.writeBoolean(LevelGround.resources[(int) index].isSnow);
      }
      ReadWrite.writeBlock(Level.info.path + "/Terrain/Resources.dat", false, false, block4);
      River river = new River(Level.info.path + "/Terrain/Trees.dat", false);
      river.writeByte(LevelGround.SAVEDATA_TREES_VERSION);
      for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
        {
          List<ResourceSpawnpoint> list = LevelGround.trees[(int) index1, (int) index2];
          river.writeUInt16((ushort) list.Count);
          for (ushort index3 = (ushort) 0; (int) index3 < list.Count; ++index3)
          {
            ResourceSpawnpoint resourceSpawnpoint = list[(int) index3];
            river.writeUInt16(resourceSpawnpoint.id);
            river.writeSingleVector3(resourceSpawnpoint.point);
            river.writeBoolean(resourceSpawnpoint.isGenerated);
          }
        }
      }
      river.closeRiver();
    }

    private static void onRegionUpdated(byte old_x, byte old_y, byte new_x, byte new_y)
    {
      LevelGround.onRegionUpdated((Player) null, old_x, old_y, new_x, new_y);
    }

    private static void onRegionUpdated(Player player, byte old_x, byte old_y, byte new_x, byte new_y)
    {
      for (byte x_0 = (byte) 0; (int) x_0 < (int) Regions.WORLD_SIZE; ++x_0)
      {
        for (byte y_0 = (byte) 0; (int) y_0 < (int) Regions.WORLD_SIZE; ++y_0)
        {
          if (LevelGround.regions[(int) x_0, (int) y_0] && !Regions.checkArea(x_0, y_0, new_x, new_y, LevelGround.RESOURCE_REGIONS))
          {
            List<ResourceSpawnpoint> list = LevelGround.trees[(int) x_0, (int) y_0];
            for (int index = 0; index < list.Count; ++index)
              list[index].disable();
            LevelGround.regions[(int) x_0, (int) y_0] = false;
          }
        }
      }
      if (!Regions.checkSafe(new_x, new_y))
        return;
      for (int index1 = (int) new_x - (int) LevelGround.RESOURCE_REGIONS; index1 <= (int) new_x + (int) LevelGround.RESOURCE_REGIONS; ++index1)
      {
        for (int index2 = (int) new_y - (int) LevelGround.RESOURCE_REGIONS; index2 <= (int) new_y + (int) LevelGround.RESOURCE_REGIONS; ++index2)
        {
          if (Regions.checkSafe((byte) index1, (byte) index2) && !LevelGround.regions[index1, index2])
          {
            List<ResourceSpawnpoint> list = LevelGround.trees[index1, index2];
            for (int index3 = 0; index3 < list.Count; ++index3)
              list[index3].enable();
            LevelGround.regions[index1, index2] = true;
          }
        }
      }
    }

    private static void onPlayerCreated(Player player)
    {
      if (!player.channel.isOwner)
        return;
      Player.player.movement.onRegionUpdated += new PlayerRegionUpdated(LevelGround.onRegionUpdated);
    }

    private static void onEditorCreated()
    {
      Editor.editor.movement.onRegionUpdated += new EditorRegionUpdated(LevelGround.onRegionUpdated);
    }

    private void Update()
    {
      if (!Level.isLoaded || Dedicator.isDedicated || (!Level.isEditor || !LevelGround.isBakingResources))
        return;
      bool flag = true;
      while (flag)
      {
        flag = LevelGround.bakeResource();
        ++LevelGround.bakeResources_X;
        if ((int) LevelGround.bakeResources_X >= (int) LevelGround.bakeResources_W)
        {
          LevelGround.bakeResources_X = LevelGround.bakeResources_M;
          ++LevelGround.bakeResources_Y;
          if ((int) LevelGround.bakeResources_Y >= (int) LevelGround.bakeResources_H)
          {
            flag = false;
            LevelGround.isBakingResources = false;
            for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
            {
              for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
              {
                if (!LevelObjects.regions[(int) index1, (int) index2])
                {
                  List<LevelObject> list = LevelObjects.objects[(int) index1, (int) index2];
                  for (int index3 = 0; index3 < list.Count; ++index3)
                    list[index3].disable();
                }
              }
            }
          }
        }
      }
    }

    private void Start()
    {
      Player.onPlayerCreated += new PlayerCreated(LevelGround.onPlayerCreated);
      Editor.onEditorCreated += new EditorCreated(LevelGround.onEditorCreated);
    }
  }
}
