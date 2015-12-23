// Decompiled with JetBrains decompiler
// Type: Steamworks.EAppType
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Flags]
  public enum EAppType
  {
    k_EAppType_Invalid = 0,
    k_EAppType_Game = 1,
    k_EAppType_Application = 2,
    k_EAppType_Tool = 4,
    k_EAppType_Demo = 8,
    k_EAppType_Media_DEPRECATED = 16,
    k_EAppType_DLC = 32,
    k_EAppType_Guide = 64,
    k_EAppType_Driver = 128,
    k_EAppType_Config = 256,
    k_EAppType_Hardware = 512,
    k_EAppType_Video = 2048,
    k_EAppType_Plugin = 4096,
    k_EAppType_Music = 8192,
    k_EAppType_Shortcut = 1073741824,
    k_EAppType_DepotOnly = -2147483647,
  }
}
