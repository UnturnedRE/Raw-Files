// Decompiled with JetBrains decompiler
// Type: Pathfinding.PathModifier
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Pathfinding
{
  [Serializable]
  public abstract class PathModifier : IPathModifier
  {
    public int priority;
    [NonSerialized]
    public Seeker seeker;

    public abstract ModifierData input { get; }

    public abstract ModifierData output { get; }

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

    public void Awake(Seeker s)
    {
      this.seeker = s;
      if (!((UnityEngine.Object) s != (UnityEngine.Object) null))
        return;
      s.RegisterModifier((IPathModifier) this);
    }

    public void OnDestroy(Seeker s)
    {
      if (!((UnityEngine.Object) s != (UnityEngine.Object) null))
        return;
      s.DeregisterModifier((IPathModifier) this);
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
  }
}
