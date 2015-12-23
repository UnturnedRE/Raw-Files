// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Bundles
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class Bundles : MonoBehaviour
  {
    private static bool _isInitialized;

    public static bool isInitialized
    {
      get
      {
        return Bundles._isInitialized;
      }
    }

    public static Bundle getBundle(string path)
    {
      return Bundles.getBundle(path, true);
    }

    public static Bundle getBundle(string path, bool usePath)
    {
      return new Bundle(path, usePath);
    }

    private void Awake()
    {
      if (Bundles.isInitialized)
      {
        Object.Destroy((Object) this.gameObject);
      }
      else
      {
        Bundles._isInitialized = true;
        Object.DontDestroyOnLoad((Object) this.gameObject);
      }
    }
  }
}
