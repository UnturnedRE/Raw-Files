// Decompiled with JetBrains decompiler
// Type: RVOAgentPlacer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class RVOAgentPlacer : MonoBehaviour
{
  public int agents = 100;
  public float ringSize = 100f;
  public float repathRate = 1f;
  private const float rad2Deg = 57.29578f;
  public LayerMask mask;
  public GameObject prefab;
  public Vector3 goalOffset;

  [DebuggerHidden]
  private IEnumerator Start()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new RVOAgentPlacer.\u003CStart\u003Ec__IteratorA()
    {
      \u003C\u003Ef__this = this
    };
  }

  public Color GetColor(float angle)
  {
    return RVOAgentPlacer.HSVToRGB(angle * 57.29578f, 0.8f, 0.6f);
  }

  private static Color HSVToRGB(float h, float s, float v)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = 0.0f;
    float num4 = s * v;
    float num5 = h / 60f;
    float num6 = num4 * (1f - Math.Abs((float) ((double) num5 % 2.0 - 1.0)));
    if ((double) num5 < 1.0)
    {
      num1 = num4;
      num2 = num6;
    }
    else if ((double) num5 < 2.0)
    {
      num1 = num6;
      num2 = num4;
    }
    else if ((double) num5 < 3.0)
    {
      num2 = num4;
      num3 = num6;
    }
    else if ((double) num5 < 4.0)
    {
      num2 = num6;
      num3 = num4;
    }
    else if ((double) num5 < 5.0)
    {
      num1 = num6;
      num3 = num4;
    }
    else if ((double) num5 < 6.0)
    {
      num1 = num4;
      num3 = num6;
    }
    float num7 = v - num4;
    return new Color(num1 + num7, num2 + num7, num3 + num7);
  }
}
