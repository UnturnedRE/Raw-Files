// Decompiled with JetBrains decompiler
// Type: Pathfinding.RVO.RVOLayer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Pathfinding.RVO
{
  [Flags]
  public enum RVOLayer
  {
    DefaultAgent = 1,
    DefaultObstacle = 2,
    Layer2 = 4,
    Layer3 = 8,
    Layer4 = 16,
    Layer5 = 32,
    Layer6 = 64,
    Layer7 = 128,
    Layer8 = 256,
    Layer9 = 512,
    Layer10 = 1024,
    Layer11 = 2048,
    Layer12 = 4096,
    Layer13 = 8192,
    Layer14 = 16384,
    Layer15 = 32768,
    Layer16 = 65536,
    Layer17 = 131072,
    Layer18 = 262144,
    Layer19 = 524288,
    Layer20 = 1048576,
    Layer21 = 2097152,
    Layer22 = 4194304,
    Layer23 = 8388608,
    Layer24 = 16777216,
    Layer25 = 33554432,
    Layer26 = 67108864,
    Layer27 = 134217728,
    Layer28 = 268435456,
    Layer29 = 536870912,
    Layer30 = 1073741824,
  }
}
