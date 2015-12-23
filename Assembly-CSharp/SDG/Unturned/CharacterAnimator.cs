// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CharacterAnimator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class CharacterAnimator : MonoBehaviour
  {
    public static readonly float BLEND = 0.25f;
    protected Transform spine;
    protected Transform skull;
    protected Transform leftShoulder;
    protected Transform rightShoulder;
    protected string clip;

    public void sample()
    {
      this.GetComponent<Animation>().Sample();
    }

    public void mixAnimation(string name, bool mixLeftShoulder, bool mixRightShoulder)
    {
      this.mixAnimation(name, mixLeftShoulder, mixRightShoulder, false);
    }

    public void mixAnimation(string name, bool mixLeftShoulder, bool mixRightShoulder, bool mixSkull)
    {
      if (mixLeftShoulder)
        this.GetComponent<Animation>()[name].AddMixingTransform(this.leftShoulder, true);
      if (mixRightShoulder)
        this.GetComponent<Animation>()[name].AddMixingTransform(this.rightShoulder, true);
      if (mixSkull)
        this.GetComponent<Animation>()[name].AddMixingTransform(this.skull, true);
      this.GetComponent<Animation>()[name].layer = 1;
    }

    public void addAnimation(AnimationClip clip)
    {
      this.GetComponent<Animation>().AddClip(clip, clip.name);
      this.mixAnimation(clip.name, true, true);
    }

    public void removeAnimation(AnimationClip clip)
    {
      if (!((TrackedReference) this.GetComponent<Animation>()[clip.name] != (TrackedReference) null))
        return;
      this.GetComponent<Animation>().RemoveClip(clip);
    }

    public void setAnimationSpeed(string name, float speed)
    {
      if (!((TrackedReference) this.GetComponent<Animation>()[name] != (TrackedReference) null))
        return;
      this.GetComponent<Animation>()[name].speed = speed;
    }

    public float getAnimationLength(string name)
    {
      if ((TrackedReference) this.GetComponent<Animation>()[name] != (TrackedReference) null)
        return this.GetComponent<Animation>()[name].clip.length / this.GetComponent<Animation>()[name].speed;
      return 0.0f;
    }

    public void getAnimationSample(string name, float point)
    {
      this.GetComponent<Animation>()[name].clip.SampleAnimation(this.gameObject, point);
    }

    public bool getAnimationPlaying()
    {
      if (this.clip != string.Empty)
        return this.GetComponent<Animation>().IsPlaying(this.clip);
      return false;
    }

    public void state(string name)
    {
      if ((TrackedReference) this.GetComponent<Animation>()[name] == (TrackedReference) null)
        return;
      this.GetComponent<Animation>().CrossFade(name, CharacterAnimator.BLEND);
    }

    public bool checkExists(string name)
    {
      return (TrackedReference) this.GetComponent<Animation>()[name] != (TrackedReference) null;
    }

    public void play(string name, bool smooth)
    {
      if ((TrackedReference) this.GetComponent<Animation>()[name] == (TrackedReference) null)
        return;
      if (this.clip != string.Empty)
        this.GetComponent<Animation>().Stop(this.clip);
      this.clip = name;
      if (smooth)
        this.GetComponent<Animation>().CrossFade(name, CharacterAnimator.BLEND);
      else
        this.GetComponent<Animation>().Play(name);
    }

    public void stop(string name)
    {
      if ((TrackedReference) this.GetComponent<Animation>()[name] == (TrackedReference) null || !(name == this.clip))
        return;
      this.GetComponent<Animation>().Stop(name);
      this.clip = string.Empty;
    }

    protected void init()
    {
      this.clip = string.Empty;
      this.spine = this.transform.FindChild("Skeleton").FindChild("Spine");
      this.skull = this.spine.FindChild("Skull");
      this.leftShoulder = this.spine.FindChild("Left_Shoulder");
      this.rightShoulder = this.spine.FindChild("Right_Shoulder");
    }

    private void Awake()
    {
      this.init();
    }
  }
}
