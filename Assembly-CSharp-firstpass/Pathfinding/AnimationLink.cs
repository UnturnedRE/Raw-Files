// Decompiled with JetBrains decompiler
// Type: Pathfinding.AnimationLink
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  public class AnimationLink : NodeLink2
  {
    public float animSpeed = 1f;
    public bool reverseAnim = true;
    public string boneRoot = "bn_COG_Root";
    public string clip;
    public GameObject referenceMesh;
    public AnimationLink.LinkClip[] sequence;

    private static Transform SearchRec(Transform tr, string name)
    {
      int childCount = tr.childCount;
      for (int index = 0; index < childCount; ++index)
      {
        Transform child = tr.GetChild(index);
        if (child.name == name)
          return child;
        Transform transform = AnimationLink.SearchRec(child, name);
        if ((UnityEngine.Object) transform != (UnityEngine.Object) null)
          return transform;
      }
      return (Transform) null;
    }

    public void CalculateOffsets(List<Vector3> trace, out Vector3 endPosition)
    {
      endPosition = this.transform.position;
      if ((UnityEngine.Object) this.referenceMesh == (UnityEngine.Object) null)
        return;
      GameObject gameObject = UnityEngine.Object.Instantiate((UnityEngine.Object) this.referenceMesh, this.transform.position, this.transform.rotation) as GameObject;
      gameObject.hideFlags = HideFlags.HideAndDontSave;
      Transform transform = AnimationLink.SearchRec(gameObject.transform, this.boneRoot);
      if ((UnityEngine.Object) transform == (UnityEngine.Object) null)
        throw new Exception("Could not find root transform");
      Animation animation = gameObject.GetComponent<Animation>();
      if ((UnityEngine.Object) animation == (UnityEngine.Object) null)
        animation = gameObject.AddComponent<Animation>();
      for (int index = 0; index < this.sequence.Length; ++index)
        animation.AddClip(this.sequence[index].clip, this.sequence[index].clip.name);
      Vector3 vector3_1 = Vector3.zero;
      Vector3 position = this.transform.position;
      Vector3 vector3_2 = Vector3.zero;
      for (int index1 = 0; index1 < this.sequence.Length; ++index1)
      {
        AnimationLink.LinkClip linkClip = this.sequence[index1];
        if (linkClip == null)
        {
          endPosition = position;
          return;
        }
        animation[linkClip.clip.name].enabled = true;
        animation[linkClip.clip.name].weight = 1f;
        for (int index2 = 0; index2 < linkClip.loopCount; ++index2)
        {
          animation[linkClip.clip.name].normalizedTime = 0.0f;
          animation.Sample();
          Vector3 vector3_3 = transform.position - this.transform.position;
          if (index1 > 0)
            position += vector3_1 - vector3_3;
          else
            vector3_2 = vector3_3;
          for (int index3 = 0; index3 <= 20; ++index3)
          {
            float num = (float) index3 / 20f;
            animation[linkClip.clip.name].normalizedTime = num;
            animation.Sample();
            Vector3 vector3_4 = position + transform.position - this.transform.position + linkClip.velocity * num * linkClip.clip.length;
            trace.Add(vector3_4);
          }
          position += linkClip.velocity * 1f * linkClip.clip.length;
          animation[linkClip.clip.name].normalizedTime = 1f;
          animation.Sample();
          vector3_1 = transform.position - this.transform.position;
        }
        animation[linkClip.clip.name].enabled = false;
        animation[linkClip.clip.name].weight = 0.0f;
      }
      Vector3 vector3_5 = position + vector3_1 - vector3_2;
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) gameObject);
      endPosition = vector3_5;
    }

    public override void OnDrawGizmosSelected()
    {
      base.OnDrawGizmosSelected();
      List<Vector3> trace = ListPool<Vector3>.Claim();
      Vector3 endPosition = Vector3.zero;
      this.CalculateOffsets(trace, out endPosition);
      Gizmos.color = Color.blue;
      for (int index = 0; index < trace.Count - 1; ++index)
        Gizmos.DrawLine(trace[index], trace[index + 1]);
    }

    [Serializable]
    public class LinkClip
    {
      public int loopCount = 1;
      public AnimationClip clip;
      public Vector3 velocity;

      public string name
      {
        get
        {
          if ((UnityEngine.Object) this.clip != (UnityEngine.Object) null)
            return this.clip.name;
          return string.Empty;
        }
      }
    }
  }
}
