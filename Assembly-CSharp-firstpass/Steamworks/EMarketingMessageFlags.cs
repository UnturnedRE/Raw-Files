// Decompiled with JetBrains decompiler
// Type: Steamworks.EMarketingMessageFlags
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Flags]
  public enum EMarketingMessageFlags
  {
    k_EMarketingMessageFlagsNone = 0,
    k_EMarketingMessageFlagsHighPriority = 1,
    k_EMarketingMessageFlagsPlatformWindows = 2,
    k_EMarketingMessageFlagsPlatformMac = 4,
    k_EMarketingMessageFlagsPlatformLinux = 8,
    k_EMarketingMessageFlagsPlatformRestrictions = k_EMarketingMessageFlagsPlatformLinux | k_EMarketingMessageFlagsPlatformMac | k_EMarketingMessageFlagsPlatformWindows,
  }
}
