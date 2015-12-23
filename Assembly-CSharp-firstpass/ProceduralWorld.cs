// Decompiled with JetBrains decompiler
// Type: ProceduralWorld
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ProceduralWorld : MonoBehaviour
{
  public float tileSize = 100f;
  public int subTiles = 20;
  private Dictionary<Int2, ProceduralWorld.ProceduralTile> tiles = new Dictionary<Int2, ProceduralWorld.ProceduralTile>();
  public Transform target;
  public ProceduralWorld.ProceduralPrefab[] prefabs;
  public int range;

  private void Start()
  {
    this.Update();
    AstarPath.active.Scan();
  }

  private void Update()
  {
    Int2 int2 = new Int2(Mathf.RoundToInt((this.target.position.x - this.tileSize * 0.5f) / this.tileSize), Mathf.RoundToInt((this.target.position.z - this.tileSize * 0.5f) / this.tileSize));
    this.range = this.range >= 1 ? this.range : 1;
    bool flag = true;
    while (flag)
    {
      flag = false;
      using (Dictionary<Int2, ProceduralWorld.ProceduralTile>.Enumerator enumerator = this.tiles.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<Int2, ProceduralWorld.ProceduralTile> current = enumerator.Current;
          if (Mathf.Abs(current.Key.x - int2.x) > this.range || Mathf.Abs(current.Key.y - int2.y) > this.range)
          {
            current.Value.Destroy();
            this.tiles.Remove(current.Key);
            flag = true;
            break;
          }
        }
      }
    }
    for (int x = int2.x - this.range; x <= int2.x + this.range; ++x)
    {
      for (int index = int2.y - this.range; index <= int2.y + this.range; ++index)
      {
        if (!this.tiles.ContainsKey(new Int2(x, index)))
        {
          ProceduralWorld.ProceduralTile proceduralTile = new ProceduralWorld.ProceduralTile(this, x, index);
          this.StartCoroutine(proceduralTile.Generate());
          this.tiles.Add(new Int2(x, index), proceduralTile);
        }
      }
    }
    for (int x = int2.x - 1; x <= int2.x + 1; ++x)
    {
      for (int y = int2.y - 1; y <= int2.y + 1; ++y)
        this.tiles[new Int2(x, y)].ForceFinish();
    }
  }

  [Serializable]
  public class ProceduralPrefab
  {
    public float perlinPower = 1f;
    public Vector2 perlinOffset = Vector2.zero;
    public float perlinScale = 1f;
    public float random = 1f;
    public GameObject prefab;
    public float density;
    public float perlin;
    public bool singleFixed;
  }

  private class ProceduralTile
  {
    private int x;
    private int z;
    private System.Random rnd;
    private ProceduralWorld world;
    private Transform root;
    private IEnumerator ie;

    public ProceduralTile(ProceduralWorld world, int x, int z)
    {
      this.x = x;
      this.z = z;
      this.world = world;
      this.rnd = new System.Random(x * 10007 ^ z * 36007);
    }

    [DebuggerHidden]
    public IEnumerator Generate()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ProceduralWorld.ProceduralTile.\u003CGenerate\u003Ec__IteratorC()
      {
        \u003C\u003Ef__this = this
      };
    }

    public void ForceFinish()
    {
      do
        ;
      while (this.ie != null && (UnityEngine.Object) this.root != (UnityEngine.Object) null && this.ie.MoveNext());
      this.ie = (IEnumerator) null;
    }

    private Vector3 RandomInside()
    {
      return new Vector3()
      {
        x = ((float) this.x + (float) this.rnd.NextDouble()) * this.world.tileSize,
        z = ((float) this.z + (float) this.rnd.NextDouble()) * this.world.tileSize
      };
    }

    private Vector3 RandomInside(float px, float pz)
    {
      return new Vector3()
      {
        x = (px + (float) this.rnd.NextDouble() / (float) this.world.subTiles) * this.world.tileSize,
        z = (pz + (float) this.rnd.NextDouble() / (float) this.world.subTiles) * this.world.tileSize
      };
    }

    private Quaternion RandomYRot()
    {
      return Quaternion.Euler(360f * (float) this.rnd.NextDouble(), 0.0f, 360f * (float) this.rnd.NextDouble());
    }

    [DebuggerHidden]
    private IEnumerator InternalGenerate()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ProceduralWorld.ProceduralTile.\u003CInternalGenerate\u003Ec__IteratorD()
      {
        \u003C\u003Ef__this = this
      };
    }

    public void Destroy()
    {
      UnityEngine.Debug.Log((object) string.Concat(new object[4]
      {
        (object) "Destroying tile ",
        (object) this.x,
        (object) ", ",
        (object) this.z
      }));
      UnityEngine.Object.Destroy((UnityEngine.Object) this.root.gameObject);
      this.root = (Transform) null;
    }
  }
}
