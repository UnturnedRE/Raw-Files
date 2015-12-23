// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.HumanAnimator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class HumanAnimator : CharacterAnimator
  {
    public static readonly float LEAN = 20f;
    public float _lean;
    public float lean;
    public float _pitch;
    public float pitch;
    public float _offset;
    public float offset;

    public void apply()
    {
      bool animationPlaying = this.getAnimationPlaying();
      if (animationPlaying)
      {
        this.leftShoulder.parent = this.skull;
        this.rightShoulder.parent = this.skull;
      }
      this.spine.Rotate(0.0f, this._pitch * 0.5f, this._lean * HumanAnimator.LEAN);
      this.skull.Rotate(0.0f, this._pitch * 0.5f, 0.0f);
      this.skull.position += this.skull.forward * this.offset;
      if (!animationPlaying)
        return;
      this.skull.Rotate(0.0f, (float) (-(double) this.spine.localRotation.eulerAngles.x + (double) this._pitch * 0.5), 0.0f);
      this.leftShoulder.parent = this.spine;
      this.rightShoulder.parent = this.spine;
      this.skull.Rotate(0.0f, this.spine.localRotation.eulerAngles.x - this._pitch * 0.5f, 0.0f);
    }

    private void LateUpdate()
    {
      this._lean = Mathf.LerpAngle(this._lean, this.lean, 4f * Time.deltaTime);
      this._pitch = Mathf.LerpAngle(this._pitch, this.pitch - 90f, 4f * Time.deltaTime);
      this._offset = Mathf.Lerp(this._offset, this.offset, 4f * Time.deltaTime);
      this.apply();
    }

    private void Awake()
    {
      this.init();
    }
  }
}
