// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Tools
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class Tools : MonoBehaviour
  {
    private static bool _isInitialized;

    public static bool isInitialized
    {
      get
      {
        return Tools._isInitialized;
      }
    }

    private void Awake()
    {
      if (Tools.isInitialized)
      {
        Object.Destroy((Object) this.gameObject);
      }
      else
      {
        Tools._isInitialized = true;
        Object.DontDestroyOnLoad((Object) this.gameObject);
      }
    }
  }
}
