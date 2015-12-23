// Decompiled with JetBrains decompiler
// Type: Pathfinding.MonoModifier
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

namespace Pathfinding
{
  [Serializable]
  public abstract class MonoModifier : MonoBehaviour, IPathModifier
  {
    [NonSerialized]
    public Seeker seeker;
    public int priority;

    public int Priority
    {
      get
      {
        return this.priority;
      }
      set
      {
        this.priority = value;
      }
    }

    public abstract ModifierData input { get; }

    public abstract ModifierData output { get; }

    public void OnEnable()
    {
    }

    public void OnDisable()
    {
    }

    public void Awake()
    {
      this.seeker = this.GetComponent<Seeker>();
      if (!((UnityEngine.Object) this.seeker != (UnityEngine.Object) null))
        return;
      this.seeker.RegisterModifier((IPathModifier) this);
    }

    public void OnDestroy()
    {
      if (!((UnityEngine.Object) this.seeker != (UnityEngine.Object) null))
        return;
      this.seeker.DeregisterModifier((IPathModifier) this);
    }

    [Obsolete]
    public virtual void ApplyOriginal(Path p)
    {
    }

    public abstract void Apply(Path p, ModifierData source);

    [Obsolete]
    public virtual void PreProcess(Path p)
    {
    }

    [Obsolete]
    public virtual Vector3[] Apply(GraphNode[] path, Vector3 start, Vector3 end, int startIndex, int endIndex, NavGraph graph)
    {
      Vector3[] vector3Array = new Vector3[endIndex - startIndex];
      for (int index = startIndex; index < endIndex; ++index)
        vector3Array[index - startIndex] = (Vector3) path[index].position;
      return vector3Array;
    }

    [Obsolete]
    public virtual Vector3[] Apply(Vector3[] path, Vector3 start, Vector3 end)
    {
      return path;
    }
  }
}
