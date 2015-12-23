// Decompiled with JetBrains decompiler
// Type: Pathfinding.AstarProfiler
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Pathfinding
{
  public class AstarProfiler
  {
    private static Dictionary<string, AstarProfiler.ProfilePoint> profiles = new Dictionary<string, AstarProfiler.ProfilePoint>();
    private static DateTime startTime = DateTime.UtcNow;
    public static AstarProfiler.ProfilePoint[] fastProfiles;
    public static string[] fastProfileNames;

    private AstarProfiler()
    {
    }

    [Conditional("ProfileAstar")]
    public static void InitializeFastProfile(string[] profileNames)
    {
      AstarProfiler.fastProfileNames = new string[profileNames.Length + 2];
      Array.Copy((Array) profileNames, (Array) AstarProfiler.fastProfileNames, profileNames.Length);
      AstarProfiler.fastProfileNames[AstarProfiler.fastProfileNames.Length - 2] = "__Control1__";
      AstarProfiler.fastProfileNames[AstarProfiler.fastProfileNames.Length - 1] = "__Control2__";
      AstarProfiler.fastProfiles = new AstarProfiler.ProfilePoint[AstarProfiler.fastProfileNames.Length];
      for (int index = 0; index < AstarProfiler.fastProfiles.Length; ++index)
        AstarProfiler.fastProfiles[index] = new AstarProfiler.ProfilePoint();
    }

    [Conditional("ProfileAstar")]
    public static void StartFastProfile(int tag)
    {
      AstarProfiler.fastProfiles[tag].watch.Start();
    }

    [Conditional("ProfileAstar")]
    public static void EndFastProfile(int tag)
    {
      AstarProfiler.ProfilePoint profilePoint = AstarProfiler.fastProfiles[tag];
      ++profilePoint.totalCalls;
      profilePoint.watch.Stop();
    }

    [Conditional("UNITY_PRO_PROFILER")]
    public static void EndProfile()
    {
    }

    [Conditional("ProfileAstar")]
    public static void StartProfile(string tag)
    {
      AstarProfiler.ProfilePoint profilePoint;
      AstarProfiler.profiles.TryGetValue(tag, out profilePoint);
      if (profilePoint == null)
      {
        profilePoint = new AstarProfiler.ProfilePoint();
        AstarProfiler.profiles[tag] = profilePoint;
      }
      profilePoint.tmpBytes = GC.GetTotalMemory(false);
      profilePoint.watch.Start();
    }

    [Conditional("ProfileAstar")]
    public static void EndProfile(string tag)
    {
      if (!AstarProfiler.profiles.ContainsKey(tag))
      {
        UnityEngine.Debug.LogError((object) ("Can only end profiling for a tag which has already been started (tag was " + tag + ")"));
      }
      else
      {
        AstarProfiler.ProfilePoint profilePoint = AstarProfiler.profiles[tag];
        ++profilePoint.totalCalls;
        profilePoint.watch.Stop();
        profilePoint.totalBytes += GC.GetTotalMemory(false) - profilePoint.tmpBytes;
      }
    }

    [Conditional("ProfileAstar")]
    public static void Reset()
    {
      AstarProfiler.profiles.Clear();
      AstarProfiler.startTime = DateTime.UtcNow;
      if (AstarProfiler.fastProfiles == null)
        return;
      for (int index = 0; index < AstarProfiler.fastProfiles.Length; ++index)
        AstarProfiler.fastProfiles[index] = new AstarProfiler.ProfilePoint();
    }

    [Conditional("ProfileAstar")]
    public static void PrintFastResults()
    {
      int num1 = 0;
      while (num1 < 1000)
        ++num1;
      double num2 = AstarProfiler.fastProfiles[AstarProfiler.fastProfiles.Length - 2].watch.Elapsed.TotalMilliseconds / 1000.0;
      TimeSpan timeSpan = DateTime.UtcNow - AstarProfiler.startTime;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("============================\n\t\t\t\tProfile results:\n============================\n");
      stringBuilder.Append("Name\t\t|\tTotal Time\t|\tTotal Calls\t|\tAvg/Call\t|\tBytes");
      for (int index = 0; index < AstarProfiler.fastProfiles.Length; ++index)
      {
        string str = AstarProfiler.fastProfileNames[index];
        AstarProfiler.ProfilePoint profilePoint = AstarProfiler.fastProfiles[index];
        int num3 = profilePoint.totalCalls;
        double num4 = profilePoint.watch.Elapsed.TotalMilliseconds - num2 * (double) num3;
        if (num3 >= 1)
        {
          stringBuilder.Append("\n").Append(str.PadLeft(10)).Append("|   ");
          stringBuilder.Append(num4.ToString("0.0 ").PadLeft(10)).Append(profilePoint.watch.Elapsed.TotalMilliseconds.ToString("(0.0)").PadLeft(10)).Append("|   ");
          stringBuilder.Append(num3.ToString().PadLeft(10)).Append("|   ");
          stringBuilder.Append((num4 / (double) num3).ToString("0.000").PadLeft(10));
        }
      }
      stringBuilder.Append("\n\n============================\n\t\tTotal runtime: ");
      stringBuilder.Append(timeSpan.TotalSeconds.ToString("F3"));
      stringBuilder.Append(" seconds\n============================");
      UnityEngine.Debug.Log((object) stringBuilder.ToString());
    }

    [Conditional("ProfileAstar")]
    public static void PrintResults()
    {
      TimeSpan timeSpan = DateTime.UtcNow - AstarProfiler.startTime;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("============================\n\t\t\t\tProfile results:\n============================\n");
      int num1 = 5;
      using (Dictionary<string, AstarProfiler.ProfilePoint>.Enumerator enumerator = AstarProfiler.profiles.GetEnumerator())
      {
        while (enumerator.MoveNext())
          num1 = Math.Max(enumerator.Current.Key.Length, num1);
      }
      stringBuilder.Append(" Name ".PadRight(num1)).Append("|").Append(" Total Time\t".PadRight(20)).Append("|").Append(" Total Calls ".PadRight(20)).Append("|").Append(" Avg/Call ".PadRight(20));
      using (Dictionary<string, AstarProfiler.ProfilePoint>.Enumerator enumerator = AstarProfiler.profiles.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<string, AstarProfiler.ProfilePoint> current = enumerator.Current;
          double totalMilliseconds = current.Value.watch.Elapsed.TotalMilliseconds;
          int num2 = current.Value.totalCalls;
          if (num2 >= 1)
          {
            string key = current.Key;
            stringBuilder.Append("\n").Append(key.PadRight(num1)).Append("| ");
            stringBuilder.Append(totalMilliseconds.ToString("0.0").PadRight(20)).Append("| ");
            stringBuilder.Append(num2.ToString().PadRight(20)).Append("| ");
            stringBuilder.Append((totalMilliseconds / (double) num2).ToString("0.000").PadRight(20));
            stringBuilder.Append(AstarMath.FormatBytesBinary((int) current.Value.totalBytes).PadLeft(10));
          }
        }
      }
      stringBuilder.Append("\n\n============================\n\t\tTotal runtime: ");
      stringBuilder.Append(timeSpan.TotalSeconds.ToString("F3"));
      stringBuilder.Append(" seconds\n============================");
      UnityEngine.Debug.Log((object) stringBuilder.ToString());
    }

    public class ProfilePoint
    {
      public Stopwatch watch = new Stopwatch();
      public int totalCalls;
      public long tmpBytes;
      public long totalBytes;
    }
  }
}
