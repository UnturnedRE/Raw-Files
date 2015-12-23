// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.HumanHitboxState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class HumanHitboxState
  {
    public float angle;
    public HumanBoneState[] bones;
    public float net;

    public HumanHitboxState(int size)
    {
      this.bones = new HumanBoneState[size];
      for (int index = 0; index < size; ++index)
        this.bones[index] = new HumanBoneState();
    }

    public void update(Transform[] newBones)
    {
      for (int index = 0; index < this.bones.Length; ++index)
      {
        this.bones[index].position = newBones[index].localPosition;
        this.bones[index].rotation = newBones[index].localRotation;
      }
    }
  }
}
