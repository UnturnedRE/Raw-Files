// Decompiled with JetBrains decompiler
// Type: Steamworks.DllCheck
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Steamworks
{
  public class DllCheck
  {
    [DllImport("kernel32.dll")]
    public static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    private static extern int GetModuleFileName(IntPtr hModule, StringBuilder strFullPath, int nSize);

    public static bool Test()
    {
      return true;
    }

    private static bool CheckSteamAPIDLL()
    {
      string lpModuleName;
      int num;
      if (IntPtr.Size == 4)
      {
        lpModuleName = "steam_api.dll";
        num = 186560;
      }
      else
      {
        lpModuleName = "steam_api64.dll";
        num = 206760;
      }
      IntPtr moduleHandle = DllCheck.GetModuleHandle(lpModuleName);
      if (moduleHandle == IntPtr.Zero)
        return true;
      StringBuilder strFullPath = new StringBuilder(256);
      DllCheck.GetModuleFileName(moduleHandle, strFullPath, strFullPath.Capacity);
      string str = strFullPath.ToString();
      return !File.Exists(str) || new FileInfo(str).Length == (long) num && !(FileVersionInfo.GetVersionInfo(str).FileVersion != "02.89.45.04");
    }
  }
}
