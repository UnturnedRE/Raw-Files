// Decompiled with JetBrains decompiler
// Type: Pathfinding.AstarColor
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

namespace Pathfinding
{
  [Serializable]
  public class AstarColor
  {
    public static Color NodeConnection = new Color(1f, 1f, 1f, 0.9f);
    public static Color UnwalkableNode = new Color(1f, 0.0f, 0.0f, 0.5f);
    public static Color BoundsHandles = new Color(0.29f, 0.454f, 0.741f, 0.9f);
    public static Color ConnectionLowLerp = new Color(0.0f, 1f, 0.0f, 0.5f);
    public static Color ConnectionHighLerp = new Color(1f, 0.0f, 0.0f, 0.5f);
    public static Color MeshEdgeColor = new Color(0.0f, 0.0f, 0.0f, 0.5f);
    public static Color MeshColor = new Color(0.0f, 0.0f, 0.0f, 0.5f);
    public Color _NodeConnection;
    public Color _UnwalkableNode;
    public Color _BoundsHandles;
    public Color _ConnectionLowLerp;
    public Color _ConnectionHighLerp;
    public Color _MeshEdgeColor;
    public Color _MeshColor;
    public Color[] _AreaColors;
    private static Color[] AreaColors;

    public AstarColor()
    {
      this._NodeConnection = new Color(1f, 1f, 1f, 0.9f);
      this._UnwalkableNode = new Color(1f, 0.0f, 0.0f, 0.5f);
      this._BoundsHandles = new Color(0.29f, 0.454f, 0.741f, 0.9f);
      this._ConnectionLowLerp = new Color(0.0f, 1f, 0.0f, 0.5f);
      this._ConnectionHighLerp = new Color(1f, 0.0f, 0.0f, 0.5f);
      this._MeshEdgeColor = new Color(0.0f, 0.0f, 0.0f, 0.5f);
      this._MeshColor = new Color(0.125f, 0.686f, 0.0f, 0.19f);
    }

    public static Color GetAreaColor(uint area)
    {
      if (AstarColor.AreaColors == null || (long) area >= (long) AstarColor.AreaColors.Length)
        return AstarMath.IntToColor((int) area, 1f);
      return AstarColor.AreaColors[(int) area];
    }

    public void OnEnable()
    {
      AstarColor.NodeConnection = this._NodeConnection;
      AstarColor.UnwalkableNode = this._UnwalkableNode;
      AstarColor.BoundsHandles = this._BoundsHandles;
      AstarColor.ConnectionLowLerp = this._ConnectionLowLerp;
      AstarColor.ConnectionHighLerp = this._ConnectionHighLerp;
      AstarColor.MeshEdgeColor = this._MeshEdgeColor;
      AstarColor.MeshColor = this._MeshColor;
      AstarColor.AreaColors = this._AreaColors;
    }
  }
}
