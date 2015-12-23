// Decompiled with JetBrains decompiler
// Type: Pathfinding.ModifierData
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Pathfinding
{
  [Flags]
  public enum ModifierData
  {
    All = -1,
    StrictNodePath = 1,
    NodePath = 2,
    StrictVectorPath = 4,
    VectorPath = 8,
    Original = 16,
    None = 0,
    Nodes = NodePath | StrictNodePath,
    Vector = VectorPath | StrictVectorPath,
  }
}
