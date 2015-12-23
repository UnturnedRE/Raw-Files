// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Command
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System;

namespace SDG.Unturned
{
  public class Command : IComparable<Command>
  {
    protected Local localization;
    protected string _command;
    protected string _info;
    protected string _help;

    public string command
    {
      get
      {
        return this._command;
      }
    }

    public string info
    {
      get
      {
        return this._info;
      }
    }

    public string help
    {
      get
      {
        return this._help;
      }
    }

    protected virtual void execute(CSteamID executorID, string parameter)
    {
    }

    public virtual bool check(CSteamID executorID, string method, string parameter)
    {
      if (!(method.ToLower() == this.command.ToLower()))
        return false;
      this.execute(executorID, parameter);
      return true;
    }

    public int CompareTo(Command other)
    {
      return this.command.CompareTo(other.command);
    }
  }
}
