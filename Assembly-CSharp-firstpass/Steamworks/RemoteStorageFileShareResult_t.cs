﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.RemoteStorageFileShareResult_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1307)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct RemoteStorageFileShareResult_t
  {
    public const int k_iCallback = 1307;
    public EResult m_eResult;
    public UGCHandle_t m_hFile;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string m_rgchFilename;
  }
}
