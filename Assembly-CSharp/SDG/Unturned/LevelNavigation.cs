// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.LevelNavigation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Pathfinding;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class LevelNavigation
  {
    public static readonly Vector3 BOUNDS_SIZE = new Vector3(64f, 64f, 64f);
    public static readonly byte SAVEDATA_BOUNDS_VERSION = (byte) 1;
    public static readonly byte SAVEDATA_FLAGS_VERSION = (byte) 4;
    public static readonly byte SAVEDATA_NAVIGATION_VERSION = (byte) 1;
    private static Transform _models;
    private static List<Flag> flags;
    private static List<Bounds> _bounds;

    public static Transform models
    {
      get
      {
        return LevelNavigation._models;
      }
    }

    public static List<Bounds> bounds
    {
      get
      {
        return LevelNavigation._bounds;
      }
    }

    public static bool tryGetBounds(Vector3 point, out byte bound)
    {
      bound = byte.MaxValue;
      for (byte index = (byte) 0; (int) index < LevelNavigation.bounds.Count; ++index)
      {
        if (LevelNavigation.bounds[(int) index].Contains(point))
        {
          bound = index;
          return true;
        }
      }
      return false;
    }

    public static bool tryGetNavigation(Vector3 point, out byte nav)
    {
      nav = byte.MaxValue;
      for (byte index = (byte) 0; (int) index < Mathf.Min(LevelNavigation.bounds.Count, AstarPath.active.graphs.Length); ++index)
      {
        if (AstarPath.active.graphs[(int) index] != null && ((RecastGraph) AstarPath.active.graphs[(int) index]).forcedBounds.Contains(point))
        {
          nav = index;
          return true;
        }
      }
      return false;
    }

    public static bool checkSafe(byte bound)
    {
      return (int) bound < LevelNavigation.bounds.Count;
    }

    public static bool checkSafe(Vector3 point)
    {
      for (byte index = (byte) 0; (int) index < LevelNavigation.bounds.Count; ++index)
      {
        if (LevelNavigation.bounds[(int) index].Contains(point))
          return true;
      }
      return false;
    }

    public static bool checkNavigation(Vector3 point)
    {
      for (byte index = (byte) 0; (int) index < AstarPath.active.graphs.Length; ++index)
      {
        if (AstarPath.active.graphs[(int) index] != null && ((RecastGraph) AstarPath.active.graphs[(int) index]).forcedBounds.Contains(point))
          return true;
      }
      return false;
    }

    public static void setEnabled(bool isEnabled)
    {
      if (LevelNavigation.flags == null)
        return;
      for (int index = 0; index < LevelNavigation.flags.Count; ++index)
        LevelNavigation.flags[index].setEnabled(isEnabled);
    }

    public static RecastGraph addGraph()
    {
      RecastGraph recastGraph = (RecastGraph) AstarPath.active.astarData.AddGraph(typeof (RecastGraph));
      recastGraph.cellSize = 0.1f;
      recastGraph.cellHeight = 0.1f;
      recastGraph.useTiles = true;
      recastGraph.editorTileSize = 128;
      recastGraph.minRegionSize = 64f;
      recastGraph.walkableHeight = 2f;
      recastGraph.walkableClimb = 0.75f;
      recastGraph.characterRadius = 0.75f;
      recastGraph.maxSlope = 75f;
      recastGraph.maxEdgeLength = 16f;
      recastGraph.contourMaxError = 2f;
      recastGraph.terrainSampleSize = 1;
      recastGraph.rasterizeTrees = false;
      recastGraph.rasterizeMeshes = false;
      recastGraph.rasterizeColliders = true;
      recastGraph.colliderRasterizeDetail = 4f;
      recastGraph.mask = (LayerMask) RayMasks.BLOCK_NAVMESH;
      return recastGraph;
    }

    public static void updateBounds()
    {
      LevelNavigation._bounds = new List<Bounds>();
      for (int index = 0; index < AstarPath.active.graphs.Length; ++index)
      {
        RecastGraph recastGraph = (RecastGraph) AstarPath.active.graphs[index];
        if (recastGraph != null)
        {
          RecastGraph.NavmeshTile[] tiles = recastGraph.GetTiles();
          if (tiles != null && tiles.Length > 0)
            LevelNavigation.bounds.Add(new Bounds(recastGraph.forcedBoundsCenter, recastGraph.forcedBoundsSize + LevelNavigation.BOUNDS_SIZE));
        }
      }
    }

    public static Transform addFlag(Vector3 point)
    {
      RecastGraph newGraph = LevelNavigation.addGraph();
      LevelNavigation.flags.Add(new Flag(point, newGraph));
      return LevelNavigation.flags[LevelNavigation.flags.Count - 1].model;
    }

    public static void removeFlag(Transform select)
    {
      for (int index1 = 0; index1 < LevelNavigation.flags.Count; ++index1)
      {
        if ((Object) LevelNavigation.flags[index1].model == (Object) select)
        {
          for (int index2 = index1 + 1; index2 < LevelNavigation.flags.Count; ++index2)
            LevelNavigation.flags[index2].needsNavigationSave = true;
          try
          {
            LevelNavigation.flags[index1].remove();
          }
          catch
          {
          }
          LevelNavigation.flags.RemoveAt(index1);
          break;
        }
      }
      LevelNavigation.updateBounds();
    }

    public static Flag getFlag(Transform select)
    {
      for (int index = 0; index < LevelNavigation.flags.Count; ++index)
      {
        if ((Object) LevelNavigation.flags[index].model == (Object) select)
          return LevelNavigation.flags[index];
      }
      return (Flag) null;
    }

    public static void load()
    {
      LevelNavigation._models = new GameObject().transform;
      LevelNavigation.models.name = "Navigation";
      LevelNavigation.models.parent = Level.level;
      LevelNavigation.models.tag = "Logic";
      LevelNavigation.models.gameObject.layer = LayerMasks.LOGIC;
      LevelNavigation._bounds = new List<Bounds>();
      if (ReadWrite.fileExists(Level.info.path + "/Environment/Bounds.dat", false, false))
      {
        River river = new River(Level.info.path + "/Environment/Bounds.dat", false);
        if ((int) river.readByte() > 0)
        {
          byte num = river.readByte();
          for (byte index = (byte) 0; (int) index < (int) num; ++index)
            LevelNavigation.bounds.Add(new Bounds(river.readSingleVector3(), river.readSingleVector3()));
        }
        river.closeRiver();
      }
      if (Level.isEditor)
      {
        LevelNavigation.flags = new List<Flag>();
        if (!ReadWrite.fileExists(Level.info.path + "/Environment/Flags.dat", false, false))
          return;
        River river1 = new River(Level.info.path + "/Environment/Flags.dat", false);
        byte num1 = river1.readByte();
        if ((int) num1 > 2)
        {
          byte num2 = river1.readByte();
          for (byte index = (byte) 0; (int) index < (int) num2; ++index)
          {
            Vector3 newPoint = river1.readSingleVector3();
            float newWidth = river1.readSingle();
            float newHeight = river1.readSingle();
            if ((int) num1 < 4)
            {
              newWidth *= 0.5f;
              newHeight *= 0.5f;
            }
            RecastGraph newGraph = (RecastGraph) null;
            if (ReadWrite.fileExists(string.Concat(new object[4]
            {
              (object) Level.info.path,
              (object) "/Environment/Navigation_",
              (object) index,
              (object) ".dat"
            }), false, false))
            {
              River river2 = new River(string.Concat(new object[4]
              {
                (object) Level.info.path,
                (object) "/Environment/Navigation_",
                (object) index,
                (object) ".dat"
              }), false);
              if ((int) river2.readByte() > 0)
                newGraph = LevelNavigation.buildGraph(river2);
              river2.closeRiver();
            }
            if (newGraph == null)
              newGraph = LevelNavigation.addGraph();
            LevelNavigation.flags.Add(new Flag(newPoint, newWidth, newHeight, newGraph));
          }
        }
        river1.closeRiver();
      }
      else
      {
        if (!Provider.isServer)
          return;
        byte num = (byte) 0;
        while (true)
        {
          if (ReadWrite.fileExists(string.Concat(new object[4]
          {
            (object) Level.info.path,
            (object) "/Environment/Navigation_",
            (object) num,
            (object) ".dat"
          }), false, false))
          {
            River river = new River(string.Concat(new object[4]
            {
              (object) Level.info.path,
              (object) "/Environment/Navigation_",
              (object) num,
              (object) ".dat"
            }), false);
            if ((int) river.readByte() > 0)
              LevelNavigation.buildGraph(river);
            river.closeRiver();
            ++num;
          }
          else
            break;
        }
      }
    }

    public static void save()
    {
      River river1 = new River(Level.info.path + "/Environment/Bounds.dat", false);
      river1.writeByte(LevelNavigation.SAVEDATA_BOUNDS_VERSION);
      river1.writeByte((byte) LevelNavigation.bounds.Count);
      for (byte index = (byte) 0; (int) index < LevelNavigation.bounds.Count; ++index)
      {
        river1.writeSingleVector3(LevelNavigation.bounds[(int) index].center);
        river1.writeSingleVector3(LevelNavigation.bounds[(int) index].size);
      }
      river1.closeRiver();
      River river2 = new River(Level.info.path + "/Environment/Flags.dat", false);
      river2.writeByte(LevelNavigation.SAVEDATA_FLAGS_VERSION);
      int count = LevelNavigation.flags.Count;
      while (true)
      {
        if (ReadWrite.fileExists(string.Concat(new object[4]
        {
          (object) Level.info.path,
          (object) "/Environment/Navigation_",
          (object) count,
          (object) ".dat"
        }), false, false))
        {
          ReadWrite.deleteFile(string.Concat(new object[4]
          {
            (object) Level.info.path,
            (object) "/Environment/Navigation_",
            (object) count,
            (object) ".dat"
          }), false, false);
          ++count;
        }
        else
          break;
      }
      river2.writeByte((byte) LevelNavigation.flags.Count);
      for (byte index1 = (byte) 0; (int) index1 < LevelNavigation.flags.Count; ++index1)
      {
        Flag flag = LevelNavigation.flags[(int) index1];
        river2.writeSingleVector3(flag.point);
        river2.writeSingle(flag.width);
        river2.writeSingle(flag.height);
        if (flag.needsNavigationSave)
        {
          River river3 = new River(string.Concat(new object[4]
          {
            (object) Level.info.path,
            (object) "/Environment/Navigation_",
            (object) index1,
            (object) ".dat"
          }), false);
          river3.writeByte(LevelNavigation.SAVEDATA_NAVIGATION_VERSION);
          RecastGraph graph = flag.graph;
          river3.writeSingleVector3(graph.forcedBoundsCenter);
          river3.writeSingleVector3(graph.forcedBoundsSize);
          river3.writeByte((byte) graph.tileXCount);
          river3.writeByte((byte) graph.tileZCount);
          RecastGraph.NavmeshTile[] tiles = graph.GetTiles();
          for (int index2 = 0; index2 < graph.tileZCount; ++index2)
          {
            for (int index3 = 0; index3 < graph.tileXCount; ++index3)
            {
              RecastGraph.NavmeshTile navmeshTile = tiles[index3 + index2 * graph.tileXCount];
              river3.writeUInt16((ushort) navmeshTile.tris.Length);
              for (int index4 = 0; index4 < navmeshTile.tris.Length; ++index4)
                river3.writeUInt16((ushort) navmeshTile.tris[index4]);
              river3.writeUInt16((ushort) navmeshTile.verts.Length);
              for (int index4 = 0; index4 < navmeshTile.verts.Length; ++index4)
              {
                Int3 int3 = navmeshTile.verts[index4];
                river3.writeInt32(int3.x);
                river3.writeInt32(int3.y);
                river3.writeInt32(int3.z);
              }
            }
          }
          river3.closeRiver();
          flag.needsNavigationSave = false;
        }
      }
      river2.closeRiver();
    }

    private static RecastGraph buildGraph(River river)
    {
      RecastGraph recastGraph = LevelNavigation.addGraph();
      int graphIndex = AstarPath.active.astarData.GetGraphIndex((NavGraph) recastGraph);
      TriangleMeshNode.SetNavmeshHolder(graphIndex, (INavmeshHolder) recastGraph);
      recastGraph.forcedBoundsCenter = river.readSingleVector3();
      recastGraph.forcedBoundsSize = river.readSingleVector3();
      recastGraph.tileXCount = (int) river.readByte();
      recastGraph.tileZCount = (int) river.readByte();
      RecastGraph.NavmeshTile[] newTiles = new RecastGraph.NavmeshTile[recastGraph.tileXCount * recastGraph.tileZCount];
      recastGraph.SetTiles(newTiles);
      for (int index1 = 0; index1 < recastGraph.tileZCount; ++index1)
      {
        for (int index2 = 0; index2 < recastGraph.tileXCount; ++index2)
        {
          RecastGraph.NavmeshTile navmeshTile = new RecastGraph.NavmeshTile();
          navmeshTile.x = index2;
          navmeshTile.z = index1;
          navmeshTile.w = 1;
          navmeshTile.d = 1;
          navmeshTile.bbTree = new BBTree((INavmeshHolder) navmeshTile);
          int index3 = index2 + index1 * recastGraph.tileXCount;
          newTiles[index3] = navmeshTile;
          navmeshTile.tris = new int[(int) river.readUInt16()];
          for (int index4 = 0; index4 < navmeshTile.tris.Length; ++index4)
            navmeshTile.tris[index4] = (int) river.readUInt16();
          navmeshTile.verts = new Int3[(int) river.readUInt16()];
          for (int index4 = 0; index4 < navmeshTile.verts.Length; ++index4)
            navmeshTile.verts[index4] = new Int3(river.readInt32(), river.readInt32(), river.readInt32());
          navmeshTile.nodes = new TriangleMeshNode[navmeshTile.tris.Length / 3];
          int num = index3 << 12;
          for (int index4 = 0; index4 < navmeshTile.nodes.Length; ++index4)
          {
            navmeshTile.nodes[index4] = new TriangleMeshNode(AstarPath.active);
            TriangleMeshNode triangleMeshNode = navmeshTile.nodes[index4];
            triangleMeshNode.GraphIndex = (uint) graphIndex;
            triangleMeshNode.Penalty = 0U;
            triangleMeshNode.Walkable = true;
            triangleMeshNode.v0 = navmeshTile.tris[index4 * 3] | num;
            triangleMeshNode.v1 = navmeshTile.tris[index4 * 3 + 1] | num;
            triangleMeshNode.v2 = navmeshTile.tris[index4 * 3 + 2] | num;
            triangleMeshNode.UpdatePositionFromVertices();
            navmeshTile.bbTree.Insert((MeshNode) triangleMeshNode);
          }
          recastGraph.CreateNodeConnections(navmeshTile.nodes);
        }
      }
      for (int index1 = 0; index1 < recastGraph.tileZCount; ++index1)
      {
        for (int index2 = 0; index2 < recastGraph.tileXCount; ++index2)
        {
          RecastGraph.NavmeshTile tile = newTiles[index2 + index1 * recastGraph.tileXCount];
          recastGraph.ConnectTileWithNeighbours(tile);
        }
      }
      return recastGraph;
    }
  }
}
