// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ConsoleOutput
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace SDG.Unturned
{
  public class ConsoleOutput
  {
    private const int STD_OUTPUT_HANDLE = -11;
    private TextWriter text;

    public string title
    {
      set
      {
        ConsoleOutput.SetConsoleTitle(value);
      }
    }

    public ConsoleOutput()
    {
      ConsoleOutput.AllocConsole();
      this.text = Console.Out;
      try
      {
        Console.SetOut((TextWriter) new StreamWriter((Stream) new FileStream(new SafeFileHandle(ConsoleOutput.GetStdHandle(-11), true), FileAccess.Write), Encoding.UTF8)
        {
          AutoFlush = true
        });
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ex);
      }
    }

    public void shutdown()
    {
      Console.SetOut(this.text);
      ConsoleOutput.FreeConsole();
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool AttachConsole(uint dwProcessId);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool AllocConsole();

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool FreeConsole();

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll")]
    private static extern bool SetConsoleTitle(string lpConsoleTitle);
  }
}
