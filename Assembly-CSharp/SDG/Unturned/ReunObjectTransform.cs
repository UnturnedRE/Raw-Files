// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ReunObjectTransform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class ReunObjectTransform : IReun
  {
    private Transform model;
    private Vector3 fromPosition;
    private Quaternion fromRotation;
    private Vector3 toPosition;
    private Quaternion toRotation;

    public ReunObjectTransform(Transform newModel, Vector3 newFromPosition, Quaternion newFromRotation, Vector3 newToPosition, Quaternion newToRotation)
    {
      this.model = newModel;
      this.fromPosition = newFromPosition;
      this.fromRotation = newFromRotation;
      this.toPosition = newToPosition;
      this.toRotation = newToRotation;
    }

    public Transform redo()
    {
      if ((Object) this.model != (Object) null)
        LevelObjects.transformObject(this.model, this.toPosition, this.toRotation, this.fromPosition, this.fromRotation);
      return this.model;
    }

    public void undo()
    {
      if (!((Object) this.model != (Object) null))
        return;
      LevelObjects.transformObject(this.model, this.fromPosition, this.fromRotation, this.toPosition, this.toRotation);
    }
  }
}
