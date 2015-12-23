// Decompiled with JetBrains decompiler
// Type: Pathfinding.FleePath
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

namespace Pathfinding
{
  public class FleePath : RandomPath
  {
    [Obsolete("Please use the Construct method instead")]
    public FleePath(Vector3 start, Vector3 avoid, int length, OnPathDelegate callbackDelegate = null)
      : base(start, length, callbackDelegate)
    {
      throw new Exception("Please use the Construct method instead");
    }

    public FleePath()
    {
    }

    public static FleePath Construct(Vector3 start, Vector3 avoid, int searchLength, OnPathDelegate callback = null)
    {
      FleePath path = PathPool<FleePath>.GetPath();
      path.Setup(start, avoid, searchLength, callback);
      return path;
    }

    protected void Setup(Vector3 start, Vector3 avoid, int searchLength, OnPathDelegate callback)
    {
      this.Setup(start, searchLength, callback);
      this.aim = avoid - start;
      FleePath fleePath = this;
      Vector3 vector3 = fleePath.aim * 10f;
      fleePath.aim = vector3;
      this.aim = start - this.aim;
    }

    protected override void Recycle()
    {
      PathPool<FleePath>.Recycle(this);
    }
  }
}
