// Decompiled with JetBrains decompiler
// Type: Pathfinding.GraphModifier
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace Pathfinding
{
  public abstract class GraphModifier : MonoBehaviour
  {
    private static GraphModifier root;
    private GraphModifier prev;
    private GraphModifier next;

    public static void FindAllModifiers()
    {
      foreach (GraphModifier graphModifier in Object.FindObjectsOfType(typeof (GraphModifier)) as GraphModifier[])
        graphModifier.OnEnable();
    }

    public static void TriggerEvent(GraphModifier.EventType type)
    {
      if (!Application.isPlaying)
        GraphModifier.FindAllModifiers();
      GraphModifier graphModifier = GraphModifier.root;
      GraphModifier.EventType eventType = type;
      switch (eventType)
      {
        case GraphModifier.EventType.PostScan:
          for (; (Object) graphModifier != (Object) null; graphModifier = graphModifier.next)
            graphModifier.OnPostScan();
          break;
        case GraphModifier.EventType.PreScan:
          for (; (Object) graphModifier != (Object) null; graphModifier = graphModifier.next)
            graphModifier.OnPreScan();
          break;
        case GraphModifier.EventType.LatePostScan:
          for (; (Object) graphModifier != (Object) null; graphModifier = graphModifier.next)
            graphModifier.OnLatePostScan();
          break;
        case GraphModifier.EventType.PreUpdate:
          for (; (Object) graphModifier != (Object) null; graphModifier = graphModifier.next)
            graphModifier.OnGraphsPreUpdate();
          break;
        default:
          if (eventType != GraphModifier.EventType.PostUpdate)
          {
            if (eventType != GraphModifier.EventType.PostCacheLoad)
              break;
            for (; (Object) graphModifier != (Object) null; graphModifier = graphModifier.next)
              graphModifier.OnPostCacheLoad();
            break;
          }
          for (; (Object) graphModifier != (Object) null; graphModifier = graphModifier.next)
            graphModifier.OnGraphsPostUpdate();
          break;
      }
    }

    protected virtual void OnEnable()
    {
      this.OnDisable();
      if ((Object) GraphModifier.root == (Object) null)
      {
        GraphModifier.root = this;
      }
      else
      {
        this.next = GraphModifier.root;
        GraphModifier.root.prev = this;
        GraphModifier.root = this;
      }
    }

    protected virtual void OnDisable()
    {
      if ((Object) GraphModifier.root == (Object) this)
      {
        GraphModifier.root = this.next;
        if ((Object) GraphModifier.root != (Object) null)
          GraphModifier.root.prev = (GraphModifier) null;
      }
      else
      {
        if ((Object) this.prev != (Object) null)
          this.prev.next = this.next;
        if ((Object) this.next != (Object) null)
          this.next.prev = this.prev;
      }
      this.prev = (GraphModifier) null;
      this.next = (GraphModifier) null;
    }

    public virtual void OnPostScan()
    {
    }

    public virtual void OnPreScan()
    {
    }

    public virtual void OnLatePostScan()
    {
    }

    public virtual void OnPostCacheLoad()
    {
    }

    public virtual void OnGraphsPreUpdate()
    {
    }

    public virtual void OnGraphsPostUpdate()
    {
    }

    public enum EventType
    {
      PostScan = 1,
      PreScan = 2,
      LatePostScan = 4,
      PreUpdate = 8,
      PostUpdate = 16,
      PostCacheLoad = 32,
    }
  }
}
