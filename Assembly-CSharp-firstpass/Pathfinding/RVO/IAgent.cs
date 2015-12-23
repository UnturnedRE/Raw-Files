// Decompiled with JetBrains decompiler
// Type: Pathfinding.RVO.IAgent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.RVO
{
  public interface IAgent
  {
    Vector3 InterpolatedPosition { get; }

    Vector3 Position { get; }

    Vector3 DesiredVelocity { get; set; }

    Vector3 Velocity { get; set; }

    bool Locked { get; set; }

    float Radius { get; set; }

    float Height { get; set; }

    float MaxSpeed { get; set; }

    float NeighbourDist { get; set; }

    float AgentTimeHorizon { get; set; }

    float ObstacleTimeHorizon { get; set; }

    RVOLayer Layer { get; set; }

    RVOLayer CollidesWith { get; set; }

    bool DebugDraw { get; set; }

    int MaxNeighbours { get; set; }

    List<ObstacleVertex> NeighbourObstacles { get; }

    void SetYPosition(float yCoordinate);

    void Teleport(Vector3 pos);
  }
}
