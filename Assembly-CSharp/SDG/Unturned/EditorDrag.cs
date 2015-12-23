// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorDrag
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorDrag
  {
    private Transform _transform;
    private Vector3 _screen;

    public Transform transform
    {
      get
      {
        return this._transform;
      }
    }

    public Vector3 screen
    {
      get
      {
        return this._screen;
      }
    }

    public EditorDrag(Transform newTransform, Vector3 newScreen)
    {
      this._transform = newTransform;
      this._screen = newScreen;
    }
  }
}
