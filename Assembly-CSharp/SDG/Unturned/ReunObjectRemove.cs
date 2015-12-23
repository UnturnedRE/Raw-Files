// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ReunObjectRemove
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class ReunObjectRemove : IReun
  {
    private Transform model;
    private ushort id;
    private Vector3 position;
    private Quaternion rotation;

    public ReunObjectRemove(Transform newModel, ushort newID, Vector3 newPosition, Quaternion newRotation)
    {
      this.model = newModel;
      this.id = newID;
      this.position = newPosition;
      this.rotation = newRotation;
    }

    public Transform redo()
    {
      if ((Object) this.model != (Object) null)
      {
        LevelObjects.removeObject(this.model);
        this.model = (Transform) null;
      }
      return (Transform) null;
    }

    public void undo()
    {
      if (!((Object) this.model == (Object) null))
        return;
      this.model = LevelObjects.addObject(this.position, this.rotation, this.id);
    }
  }
}
