// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorCopy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorCopy
  {
    private Vector3 _position;
    private Quaternion _rotation;
    private ushort _id;

    public Vector3 position
    {
      get
      {
        return this._position;
      }
    }

    public Quaternion rotation
    {
      get
      {
        return this._rotation;
      }
    }

    public ushort id
    {
      get
      {
        return this._id;
      }
    }

    public EditorCopy(Vector3 newPosition, Quaternion newRotation, ushort newID)
    {
      this._position = newPosition;
      this._rotation = newRotation;
      this._id = newID;
    }
  }
}
