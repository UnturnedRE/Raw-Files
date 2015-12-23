// Decompiled with JetBrains decompiler
// Type: Pathfinding.UserConnection
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Serialization.JsonFx;
using UnityEngine;

namespace Pathfinding
{
  public class UserConnection
  {
    public bool enable = true;
    [JsonName("doOverWalkable")]
    public bool doOverrideWalkability = true;
    public Vector3 p1;
    public Vector3 p2;
    public ConnectionType type;
    [JsonName("doOverCost")]
    public bool doOverrideCost;
    [JsonName("overCost")]
    public int overrideCost;
    public bool oneWay;
    public float width;
    [JsonName("doOverCost")]
    public bool doOverridePenalty;
    [JsonName("overPenalty")]
    public uint overridePenalty;
  }
}
