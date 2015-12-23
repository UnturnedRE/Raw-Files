// Decompiled with JetBrains decompiler
// Type: Steamworks.CallbackIdentities
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  internal class CallbackIdentities
  {
    public static int GetCallbackIdentity(Type callbackStruct)
    {
      object[] customAttributes = callbackStruct.GetCustomAttributes(typeof (CallbackIdentityAttribute), false);
      int index = 0;
      if (index < customAttributes.Length)
        return ((CallbackIdentityAttribute) customAttributes[index]).Identity;
      throw new Exception("Callback number not found for struct " + (object) callbackStruct);
    }
  }
}
