// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System;
using UnityEngine;

namespace SDG.Unturned
{
  public class CommandWindow
  {
    private string _title;
    private static ConsoleInput input;
    private static ConsoleOutput output;

    public string title
    {
      get
      {
        return this._title;
      }
      set
      {
        this._title = value;
        if (CommandWindow.output == null)
          return;
        CommandWindow.output.title = this.title;
      }
    }

    public CommandWindow()
    {
      CommandWindow.input = new ConsoleInput();
      CommandWindow.input.onInputText = new InputText(CommandWindow.onInputText);
      CommandWindow.output = new ConsoleOutput();
      Application.logMessageReceived += new Application.LogCallback(CommandWindow.onOutputText);
    }

    private static void Log(object text, ConsoleColor color)
    {
      if (CommandWindow.output == null)
      {
        Debug.Log(text);
      }
      else
      {
        Console.ForegroundColor = color;
        if (Console.CursorLeft != 0)
          CommandWindow.input.clearLine();
        Console.WriteLine(text);
        CommandWindow.input.redrawInputLine();
      }
    }

    public static void Log(object text)
    {
      CommandWindow.Log(text, ConsoleColor.White);
    }

    public static void LogError(object text)
    {
      CommandWindow.Log(text, ConsoleColor.Red);
    }

    public static void LogWarning(object text)
    {
      CommandWindow.Log(text, ConsoleColor.Yellow);
    }

    private static void onOutputText(string text, string stack, LogType type)
    {
      if (type != LogType.Exception)
        return;
      CommandWindow.LogError((object) (text + " - " + stack));
    }

    private static void onInputText(string command)
    {
      if (Commander.execute(CSteamID.Nil, command))
        return;
      CommandWindow.LogError((object) "?");
    }

    public void update()
    {
      if (CommandWindow.input == null)
        return;
      CommandWindow.input.update();
    }

    public void shutdown()
    {
      if (CommandWindow.output == null)
        return;
      CommandWindow.output.shutdown();
    }
  }
}
