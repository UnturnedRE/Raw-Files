// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ConsoleInput
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace SDG.Unturned
{
  public class ConsoleInput
  {
    private string text = string.Empty;
    public InputText onInputText;

    public void clearLine()
    {
      Console.CursorLeft = 0;
      Console.Write(new string(' ', Console.BufferWidth));
      --Console.CursorTop;
      Console.CursorLeft = 0;
    }

    public void redrawInputLine()
    {
      if (Console.CursorLeft > 0)
        this.clearLine();
      Console.ForegroundColor = ConsoleColor.White;
      Console.Write(this.text);
    }

    internal void onBackspace()
    {
      if (this.text.Length < 1)
        return;
      this.text = this.text.Substring(0, this.text.Length - 1);
      this.redrawInputLine();
    }

    internal void onEscape()
    {
      this.clearLine();
      this.text = string.Empty;
    }

    internal void onEnter()
    {
      this.clearLine();
      Console.ForegroundColor = ConsoleColor.White;
      Console.WriteLine(">" + this.text);
      string command = this.text;
      this.text = string.Empty;
      if (command.Length <= 0 || this.onInputText == null)
        return;
      this.onInputText(command);
    }

    public void update()
    {
      if (!Console.KeyAvailable)
        return;
      ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();
      if (consoleKeyInfo.Key == ConsoleKey.Enter)
        this.onEnter();
      else if (consoleKeyInfo.Key == ConsoleKey.Backspace)
        this.onBackspace();
      else if (consoleKeyInfo.Key == ConsoleKey.Escape)
      {
        this.onEscape();
      }
      else
      {
        if ((int) consoleKeyInfo.KeyChar == 0)
          return;
        this.text += (string) (object) consoleKeyInfo.KeyChar;
        this.redrawInputLine();
      }
    }
  }
}
