// Decompiled with JetBrains decompiler
// Type: Pathfinding.Serialization.GraphMeta
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Pathfinding.Serialization
{
  internal class GraphMeta
  {
    public Version version;
    public int graphs;
    public string[] guids;
    public string[] typeNames;
    public int[] nodeCounts;

    public Type GetGraphType(int i)
    {
      if (this.typeNames[i] == null)
        return (Type) null;
      Type type = Type.GetType(this.typeNames[i]);
      if (!object.Equals((object) type, (object) null))
        return type;
      throw new Exception("No graph of type '" + this.typeNames[i] + "' could be created, type does not exist");
    }
  }
}
