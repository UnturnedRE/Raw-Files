// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandLine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
  public class CommandLine
  {
    public static bool tryGetConnect(string line, out uint ip, out ushort port, out string pass)
    {
      ip = 0U;
      port = (ushort) 0;
      pass = string.Empty;
      int num1 = line.ToLower().IndexOf("+connect");
      if (num1 == -1)
        return false;
      int num2 = line.IndexOf(':', num1 + 9);
      string str = line.Substring(num1 + 9, num2 - num1 - 9);
      if (Parser.checkIP(str))
        ip = Parser.getUInt32FromIP(str);
      else if (!uint.TryParse(str, out ip))
        return false;
      int num3 = line.IndexOf(' ', num2 + 1);
      if (num3 == -1)
      {
        if (!ushort.TryParse(line.Substring(num2 + 1, line.Length - num2 - 1), out port))
          return false;
        int num4 = line.ToLower().IndexOf("+password");
        if (num4 != -1)
          pass = line.Substring(num4 + 10, line.Length - num4 - 10);
        return true;
      }
      if (!ushort.TryParse(line.Substring(num2 + 1, num3 - num2 - 1), out port))
        return false;
      int num5 = line.ToLower().IndexOf("+password");
      if (num5 != -1)
        pass = line.Substring(num5 + 10, line.Length - num5 - 10);
      return true;
    }

    public static bool tryGetLanguage(out string local, out string path)
    {
      local = string.Empty;
      path = string.Empty;
      string[] commandLineArgs = Environment.GetCommandLineArgs();
      for (int index1 = 0; index1 < commandLineArgs.Length; ++index1)
      {
        if (commandLineArgs[index1].Substring(0, 1) == "+")
        {
          local = commandLineArgs[index1].Substring(1, commandLineArgs[index1].Length - 1);
          if (Provider.provider.workshopService.ugc != null)
          {
            for (int index2 = 0; index2 < Provider.provider.workshopService.ugc.Count; ++index2)
            {
              SteamContent steamContent = Provider.provider.workshopService.ugc[index2];
              if (steamContent.type == ESteamUGCType.LOCALIZATION && ReadWrite.folderExists(steamContent.path + "/" + local, false))
              {
                path = steamContent.path + "/";
                return true;
              }
            }
          }
          if (ReadWrite.folderExists("/Localization/" + local))
          {
            path = ReadWrite.PATH + "/Localization/";
            return true;
          }
        }
      }
      return false;
    }

    public static bool tryGetServer(out ESteamSecurity security, out string id)
    {
      security = ESteamSecurity.LAN;
      id = string.Empty;
      string commandLine = Environment.CommandLine;
      int num1 = commandLine.ToLower().IndexOf("+secureserver");
      if (num1 != -1)
      {
        security = ESteamSecurity.SECURE;
        id = commandLine.Substring(num1 + 14, commandLine.Length - num1 - 14);
        return !(id == "Singleplayer");
      }
      int num2 = commandLine.ToLower().IndexOf("+insecureserver");
      if (num2 != -1)
      {
        security = ESteamSecurity.INSECURE;
        id = commandLine.Substring(num2 + 16, commandLine.Length - num2 - 16);
        return !(id == "Singleplayer");
      }
      int num3 = commandLine.ToLower().IndexOf("+lanserver");
      if (num3 == -1)
        return false;
      security = ESteamSecurity.LAN;
      id = commandLine.Substring(num3 + 11, commandLine.Length - num3 - 11);
      return !(id == "Singleplayer");
    }

    public static string[] getCommands()
    {
      string[] commandLineArgs = Environment.GetCommandLineArgs();
      List<string> list1 = new List<string>();
      bool flag = false;
      for (int index1 = 0; index1 < commandLineArgs.Length; ++index1)
      {
        if (commandLineArgs[index1].Substring(0, 1) == "+")
          flag = true;
        else if (commandLineArgs[index1].Substring(0, 1) == "-")
        {
          list1.Add(commandLineArgs[index1].Substring(1, commandLineArgs[index1].Length - 1));
          flag = false;
        }
        else if (list1.Count > 0 && !flag)
        {
          List<string> list2;
          int index2;
          (list2 = list1)[index2 = list1.Count - 1] = list2[index2] + " " + commandLineArgs[index1];
        }
      }
      return list1.ToArray();
    }
  }
}
