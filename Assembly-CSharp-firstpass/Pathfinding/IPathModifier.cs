// Decompiled with JetBrains decompiler
// Type: Pathfinding.IPathModifier
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Pathfinding
{
  public interface IPathModifier
  {
    int Priority { get; set; }

    ModifierData input { get; }

    ModifierData output { get; }

    void ApplyOriginal(Path p);

    void Apply(Path p, ModifierData source);

    void PreProcess(Path p);
  }
}
