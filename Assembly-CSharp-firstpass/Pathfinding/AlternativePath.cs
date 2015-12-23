// Decompiled with JetBrains decompiler
// Type: Pathfinding.AlternativePath
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

namespace Pathfinding
{
  [AddComponentMenu("Pathfinding/Modifiers/Alternative Path")]
  [Serializable]
  public class AlternativePath : MonoModifier
  {
    public int penalty = 1000;
    public int randomStep = 10;
    private object lockObject = new object();
    private System.Random rnd = new System.Random();
    private System.Random seedGenerator = new System.Random();
    private GraphNode[] prevNodes;
    private int prevSeed;
    private int prevPenalty;
    private bool waitingForApply;
    private bool destroyed;
    private GraphNode[] toBeApplied;

    public override ModifierData input
    {
      get
      {
        return ModifierData.Original;
      }
    }

    public override ModifierData output
    {
      get
      {
        return ModifierData.All;
      }
    }

    public override void Apply(Path p, ModifierData source)
    {
      if ((UnityEngine.Object) this == (UnityEngine.Object) null)
        return;
      lock (this.lockObject)
      {
        this.toBeApplied = p.path.ToArray();
        if (this.waitingForApply)
          return;
        this.waitingForApply = true;
        AstarPath.OnPathPreSearch += new OnPathDelegate(this.ApplyNow);
      }
    }

    public new void OnDestroy()
    {
      this.destroyed = true;
      lock (this.lockObject)
      {
        if (!this.waitingForApply)
        {
          this.waitingForApply = true;
          AstarPath.OnPathPreSearch += new OnPathDelegate(this.ClearOnDestroy);
        }
      }
      base.OnDestroy();
    }

    private void ClearOnDestroy(Path p)
    {
      lock (this.lockObject)
      {
        AstarPath.OnPathPreSearch -= new OnPathDelegate(this.ClearOnDestroy);
        this.waitingForApply = false;
        this.InversePrevious();
      }
    }

    private void InversePrevious()
    {
      this.rnd = new System.Random(this.prevSeed);
      if (this.prevNodes == null)
        return;
      bool flag = false;
      int index = this.rnd.Next(this.randomStep);
      while (index < this.prevNodes.Length)
      {
        if ((long) this.prevNodes[index].Penalty < (long) this.prevPenalty)
          flag = true;
        this.prevNodes[index].Penalty = (uint) ((ulong) this.prevNodes[index].Penalty - (ulong) this.prevPenalty);
        index += this.rnd.Next(1, this.randomStep);
      }
      if (!flag)
        return;
      Debug.LogWarning((object) "Penalty for some nodes has been reset while this modifier was active. Penalties might not be correctly set.");
    }

    private void ApplyNow(Path somePath)
    {
      lock (this.lockObject)
      {
        this.waitingForApply = false;
        AstarPath.OnPathPreSearch -= new OnPathDelegate(this.ApplyNow);
        this.InversePrevious();
        if (this.destroyed)
          return;
        int local_1 = this.seedGenerator.Next();
        this.rnd = new System.Random(local_1);
        if (this.toBeApplied != null)
        {
          int local_3 = this.rnd.Next(this.randomStep);
          while (local_3 < this.toBeApplied.Length)
          {
            this.toBeApplied[local_3].Penalty = (uint) ((ulong) this.toBeApplied[local_3].Penalty + (ulong) this.penalty);
            local_3 += this.rnd.Next(1, this.randomStep);
          }
        }
        this.prevPenalty = this.penalty;
        this.prevSeed = local_1;
        this.prevNodes = this.toBeApplied;
      }
    }
  }
}
