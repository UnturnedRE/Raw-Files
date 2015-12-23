﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.InteropHelp
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Steamworks
{
  public class InteropHelp
  {
    public static void TestIfPlatformSupported()
    {
    }

    public static void TestIfAvailableClient()
    {
      InteropHelp.TestIfPlatformSupported();
      if (NativeMethods.SteamClient() == IntPtr.Zero)
        throw new InvalidOperationException("Steamworks is not initialized.");
    }

    public static void TestIfAvailableGameServer()
    {
      InteropHelp.TestIfPlatformSupported();
      if (NativeMethods.SteamClientGameServer() == IntPtr.Zero)
        throw new InvalidOperationException("Steamworks is not initialized.");
    }

    public static string PtrToStringUTF8(IntPtr nativeUtf8)
    {
      if (nativeUtf8 == IntPtr.Zero)
        return string.Empty;
      int ofs = 0;
      while ((int) Marshal.ReadByte(nativeUtf8, ofs) != 0)
        ++ofs;
      if (ofs == 0)
        return string.Empty;
      byte[] numArray = new byte[ofs];
      Marshal.Copy(nativeUtf8, numArray, 0, numArray.Length);
      return Encoding.UTF8.GetString(numArray);
    }

    public class UTF8StringHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
      public UTF8StringHandle(string str)
        : base(true)
      {
        if (str == null)
        {
          this.SetHandle(IntPtr.Zero);
        }
        else
        {
          byte[] numArray = new byte[Encoding.UTF8.GetByteCount(str) + 1];
          Encoding.UTF8.GetBytes(str, 0, str.Length, numArray, 0);
          IntPtr num = Marshal.AllocHGlobal(numArray.Length);
          Marshal.Copy(numArray, 0, num, numArray.Length);
          this.SetHandle(num);
        }
      }

      protected override bool ReleaseHandle()
      {
        if (!this.IsInvalid)
          Marshal.FreeHGlobal(this.handle);
        return true;
      }
    }

    public class SteamParamStringArray
    {
      private IntPtr[] m_Strings;
      private IntPtr m_ptrStrings;
      private IntPtr m_pSteamParamStringArray;

      public SteamParamStringArray(IList<string> strings)
      {
        if (strings == null)
        {
          this.m_pSteamParamStringArray = IntPtr.Zero;
        }
        else
        {
          this.m_Strings = new IntPtr[strings.Count];
          for (int index = 0; index < strings.Count; ++index)
          {
            byte[] numArray = new byte[Encoding.UTF8.GetByteCount(strings[index]) + 1];
            Encoding.UTF8.GetBytes(strings[index], 0, strings[index].Length, numArray, 0);
            this.m_Strings[index] = Marshal.AllocHGlobal(numArray.Length);
            Marshal.Copy(numArray, 0, this.m_Strings[index], numArray.Length);
          }
          this.m_ptrStrings = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (IntPtr)) * this.m_Strings.Length);
          SteamParamStringArray_t paramStringArrayT = new SteamParamStringArray_t()
          {
            m_ppStrings = this.m_ptrStrings,
            m_nNumStrings = this.m_Strings.Length
          };
          Marshal.Copy(this.m_Strings, 0, paramStringArrayT.m_ppStrings, this.m_Strings.Length);
          this.m_pSteamParamStringArray = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (SteamParamStringArray_t)));
          Marshal.StructureToPtr((object) paramStringArrayT, this.m_pSteamParamStringArray, false);
        }
      }

      ~SteamParamStringArray()
      {
        foreach (IntPtr hglobal in this.m_Strings)
          Marshal.FreeHGlobal(hglobal);
        if (this.m_ptrStrings != IntPtr.Zero)
          Marshal.FreeHGlobal(this.m_ptrStrings);
        if (!(this.m_pSteamParamStringArray != IntPtr.Zero))
          return;
        Marshal.FreeHGlobal(this.m_pSteamParamStringArray);
      }

      public static implicit operator IntPtr(InteropHelp.SteamParamStringArray that)
      {
        return that.m_pSteamParamStringArray;
      }
    }
  }
}
