// Decompiled with JetBrains decompiler
// Type: Steamworks.EAppOwnershipFlags
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Flags]
  public enum EAppOwnershipFlags
  {
    k_EAppOwnershipFlags_None = 0,
    k_EAppOwnershipFlags_OwnsLicense = 1,
    k_EAppOwnershipFlags_FreeLicense = 2,
    k_EAppOwnershipFlags_RegionRestricted = 4,
    k_EAppOwnershipFlags_LowViolence = 8,
    k_EAppOwnershipFlags_InvalidPlatform = 16,
    k_EAppOwnershipFlags_SharedLicense = 32,
    k_EAppOwnershipFlags_FreeWeekend = 64,
    k_EAppOwnershipFlags_RetailLicense = 128,
    k_EAppOwnershipFlags_LicenseLocked = 256,
    k_EAppOwnershipFlags_LicensePending = 512,
    k_EAppOwnershipFlags_LicenseExpired = 1024,
    k_EAppOwnershipFlags_LicensePermanent = 2048,
    k_EAppOwnershipFlags_LicenseRecurring = 4096,
    k_EAppOwnershipFlags_LicenseCanceled = 8192,
    k_EAppOwnershipFlags_AutoGrant = 16384,
  }
}
