// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Commander
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System.Collections.Generic;

namespace SDG.Unturned
{
  public class Commander
  {
    private static List<Command> _commands;

    public static List<Command> commands
    {
      get
      {
        return Commander._commands;
      }
    }

    public static void register(Command command)
    {
      int index = Commander.commands.BinarySearch(command);
      if (index < 0)
        index = ~index;
      Commander.commands.Insert(index, command);
    }

    public static void deregister(Command command)
    {
      Commander.commands.Remove(command);
    }

    public static bool execute(CSteamID executorID, string command)
    {
      string method = command;
      string parameter = string.Empty;
      int length = command.IndexOf(' ');
      if (length != -1)
      {
        method = command.Substring(0, length);
        parameter = command.Substring(length + 1, command.Length - length - 1);
      }
      for (int index = 0; index < Commander.commands.Count; ++index)
      {
        if (Commander.commands[index].check(executorID, method, parameter))
          return true;
      }
      return false;
    }

    public static void init()
    {
      Commander._commands = new List<Command>();
      Commander.register((Command) new CommandHelp(Localization.read("/Server/ServerCommandHelp.dat")));
      Commander.register((Command) new CommandName(Localization.read("/Server/ServerCommandName.dat")));
      Commander.register((Command) new CommandPort(Localization.read("/Server/ServerCommandPort.dat")));
      Commander.register((Command) new CommandPassword(Localization.read("/Server/ServerCommandPassword.dat")));
      Commander.register((Command) new CommandMaxPlayers(Localization.read("/Server/ServerCommandMaxPlayers.dat")));
      Commander.register((Command) new CommandMap(Localization.read("/Server/ServerCommandMap.dat")));
      Commander.register((Command) new CommandPvE(Localization.read("/Server/ServerCommandPvE.dat")));
      Commander.register((Command) new CommandFilter(Localization.read("/Server/ServerCommandFilter.dat")));
      Commander.register((Command) new CommandMode(Localization.read("/Server/ServerCommandMode.dat")));
      Commander.register((Command) new CommandCamera(Localization.read("/Server/ServerCommandCamera.dat")));
      Commander.register((Command) new CommandCycle(Localization.read("/Server/ServerCommandCycle.dat")));
      Commander.register((Command) new CommandTime(Localization.read("/Server/ServerCommandTime.dat")));
      Commander.register((Command) new CommandDay(Localization.read("/Server/ServerCommandDay.dat")));
      Commander.register((Command) new CommandNight(Localization.read("/Server/ServerCommandNight.dat")));
      Commander.register((Command) new CommandKick(Localization.read("/Server/ServerCommandKick.dat")));
      Commander.register((Command) new CommandBan(Localization.read("/Server/ServerCommandBan.dat")));
      Commander.register((Command) new CommandUnban(Localization.read("/Server/ServerCommandUnban.dat")));
      Commander.register((Command) new CommandBans(Localization.read("/Server/ServerCommandBans.dat")));
      Commander.register((Command) new CommandAdmin(Localization.read("/Server/ServerCommandAdmin.dat")));
      Commander.register((Command) new CommandUnadmin(Localization.read("/Server/ServerCommandUnadmin.dat")));
      Commander.register((Command) new CommandAdmins(Localization.read("/Server/ServerCommandAdmins.dat")));
      Commander.register((Command) new CommandOwner(Localization.read("/Server/ServerCommandOwner.dat")));
      Commander.register((Command) new CommandPermit(Localization.read("/Server/ServerCommandPermit.dat")));
      Commander.register((Command) new CommandUnpermit(Localization.read("/Server/ServerCommandUnpermit.dat")));
      Commander.register((Command) new CommandPermits(Localization.read("/Server/ServerCommandPermits.dat")));
      Commander.register((Command) new CommandPlayers(Localization.read("/Server/ServerCommandPlayers.dat")));
      Commander.register((Command) new CommandSay(Localization.read("/Server/ServerCommandSay.dat")));
      Commander.register((Command) new CommandWelcome(Localization.read("/Server/ServerCommandWelcome.dat")));
      Commander.register((Command) new CommandSlay(Localization.read("/Server/ServerCommandSlay.dat")));
      Commander.register((Command) new CommandGive(Localization.read("/Server/ServerCommandGive.dat")));
      Commander.register((Command) new CommandLoadout(Localization.read("/Server/ServerCommandLoadout.dat")));
      Commander.register((Command) new CommandExperience(Localization.read("/Server/ServerCommandExperience.dat")));
      Commander.register((Command) new CommandVehicle(Localization.read("/Server/ServerCommandVehicle.dat")));
      Commander.register((Command) new CommandTeleport(Localization.read("/Server/ServerCommandTeleport.dat")));
      Commander.register((Command) new CommandTimeout(Localization.read("/Server/ServerCommandTimeout.dat")));
      Commander.register((Command) new CommandChatrate(Localization.read("/Server/ServerCommandChatrate.dat")));
      Commander.register((Command) new CommandDebug(Localization.read("/Server/ServerCommandDebug.dat")));
      Commander.register((Command) new CommandBind(Localization.read("/Server/ServerCommandBind.dat")));
      Commander.register((Command) new CommandSave(Localization.read("/Server/ServerCommandSave.dat")));
      Commander.register((Command) new CommandShutdown(Localization.read("/Server/ServerCommandShutdown.dat")));
    }
  }
}
