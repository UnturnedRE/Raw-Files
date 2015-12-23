// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ReunObjectAdd
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class ReunObjectAdd : IReun
  {
    private Transform model;
    private ushort id;
    private Vector3 position;
    private Quaternion rotation;

    public ReunObjectAdd(ushort newID, Vector3 newPosition, Quaternion newRotation)
    {
      this.model = (Transform) null;
      this.id = newID;
      this.position = newPosition;
      this.rotation = newRotation;
    }

    public Transform redo()
    {
      if ((Object) this.model == (Object) null)
        this.model = LevelObjects.addObject(this.position, this.rotation, this.id);
      return this.model;
    }

    public void undo()
    {
      if (!((Object) this.model != (Object) null))
        return;
      LevelObjects.removeObject(this.model);
      this.model = (Transform) null;
    }
  }
}
