// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Dedicator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class Dedicator : MonoBehaviour
  {
    public static ESteamSecurity security;
    public static string serverID;
    private static bool _isDedicated;

    public static CommandWindow commandWindow { get; protected set; }

    public static bool isDedicated
    {
      get
      {
        return Dedicator._isDedicated;
      }
    }

    private void Update()
    {
      if (!Dedicator.isDedicated || Dedicator.commandWindow == null)
        return;
      Dedicator.commandWindow.update();
    }

    private void Awake()
    {
      Dedicator._isDedicated = CommandLine.tryGetServer(out Dedicator.security, out Dedicator.serverID);
      if (!Dedicator.isDedicated)
        return;
      Dedicator.commandWindow = new CommandWindow();
    }

    private void OnApplicationQuit()
    {
      if (!Dedicator.isDedicated || Dedicator.commandWindow == null)
        return;
      Dedicator.commandWindow.shutdown();
    }
  }
}
