// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ChatManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class ChatManager : SteamCaller
  {
    public static readonly int LENGTH = 90;
    public static string welcomeText = string.Empty;
    public static Color welcomeColor = Palette.SERVER;
    public static float chatrate = 0.25f;
    private static readonly string[] SWEARS = new string[20]
    {
      "cunt",
      "dick",
      "pussy",
      "penis",
      "vagina",
      "fuck",
      "fucking",
      "fucked",
      "shit",
      "shitting",
      "shat",
      "damn",
      "damned",
      "hell",
      "cock",
      "whore",
      "fag",
      "faggot",
      "fag",
      "nigger"
    };
    public static Listed onListed;
    public static Chatted onChatted;
    private static ChatManager manager;
    private static Chat[] _chat;

    public static Chat[] chat
    {
      get
      {
        return ChatManager._chat;
      }
    }

    public static string replace(string text, int index, int count, char mask)
    {
      string str = text.Substring(0, index);
      for (int index1 = 0; index1 < count; ++index1)
        str += (string) (object) mask;
      return str + text.Substring(index + count, text.Length - index - count);
    }

    public static string filter(string text)
    {
      string text1 = text.ToLower();
      if (text.Length > 0)
      {
        bool flag = text.IndexOf(' ') != -1;
        for (int index = 0; index < ChatManager.SWEARS.Length; ++index)
        {
          string str = ChatManager.SWEARS[index];
          int num = text1.IndexOf(str, 0);
          while (num != -1)
          {
            if (!flag || (num == 0 || !char.IsLetterOrDigit(text1[num - 1])) && (num == text1.Length - str.Length || !char.IsLetterOrDigit(text1[num + str.Length])))
            {
              text1 = ChatManager.replace(text1, num, str.Length, '#');
              text = ChatManager.replace(text, num, str.Length, '#');
              num = text1.IndexOf(str, num);
            }
            else
              num = text1.IndexOf(str, num + 1);
          }
        }
      }
      return text;
    }

    public static void list(CSteamID steamID, EChatMode mode, Color color, string text)
    {
      text = text.Trim();
      if (OptionsSettings.filter)
        text = ChatManager.filter(text);
      string newSpeaker;
      if (steamID == CSteamID.Nil)
      {
        newSpeaker = Provider.localization.format("Say");
      }
      else
      {
        SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(steamID);
        if (steamPlayer == null || !OptionsSettings.chatText && steamPlayer.playerID.steamID != Provider.client)
          return;
        newSpeaker = !(Characters.active.group != CSteamID.Nil) || !(steamPlayer.playerID.group == Characters.active.group) ? steamPlayer.playerID.characterName : (!(steamPlayer.playerID.nickName != string.Empty) || !(steamPlayer.playerID.steamID != Provider.client) ? steamPlayer.playerID.characterName : steamPlayer.playerID.nickName);
      }
      for (int index = ChatManager.chat.Length - 1; index > 0; --index)
      {
        if (ChatManager.chat[index - 1] != null)
        {
          if (ChatManager.chat[index] == null)
          {
            ChatManager.chat[index] = new Chat(ChatManager.chat[index - 1].mode, ChatManager.chat[index - 1].color, ChatManager.chat[index - 1].speaker, ChatManager.chat[index - 1].text);
          }
          else
          {
            ChatManager.chat[index].mode = ChatManager.chat[index - 1].mode;
            ChatManager.chat[index].color = ChatManager.chat[index - 1].color;
            ChatManager.chat[index].speaker = ChatManager.chat[index - 1].speaker;
            ChatManager.chat[index].text = ChatManager.chat[index - 1].text;
          }
        }
      }
      if (ChatManager.chat[0] == null)
      {
        ChatManager.chat[0] = new Chat(mode, color, newSpeaker, text);
      }
      else
      {
        ChatManager.chat[0].mode = mode;
        ChatManager.chat[0].color = color;
        ChatManager.chat[0].speaker = newSpeaker;
        ChatManager.chat[0].text = text;
      }
      if (ChatManager.onListed == null)
        return;
      ChatManager.onListed();
    }

    public static bool process(SteamPlayer player, string text)
    {
      if (!(text.Substring(0, 1) == "@"))
        return true;
      if (player.isAdmin)
        Commander.execute(player.playerID.steamID, text.Substring(1));
      return false;
    }

    [SteamCall]
    public void tellChat(CSteamID steamID, CSteamID owner, byte mode, Color color, string text)
    {
      if (!this.channel.checkServer(steamID))
        return;
      ChatManager.list(owner, (EChatMode) mode, color, text);
    }

    [SteamCall]
    public void askChat(CSteamID steamID, byte mode, string text)
    {
      if (!Provider.isServer)
        return;
      SteamPlayer steamPlayer1 = PlayerTool.getSteamPlayer(steamID);
      if (steamPlayer1 == null)
        return;
      EChatMode mode1 = (EChatMode) mode;
      if (text.Length < 2)
        return;
      if (text.Length > ChatManager.LENGTH)
        text = text.Substring(0, ChatManager.LENGTH);
      text = text.Trim();
      if (mode1 == EChatMode.GLOBAL)
        CommandWindow.Log((object) Provider.localization.format("Global", (object) steamPlayer1.playerID.characterName, (object) steamPlayer1.playerID.playerName, (object) text));
      else if (mode1 == EChatMode.LOCAL)
      {
        CommandWindow.Log((object) Provider.localization.format("Local", (object) steamPlayer1.playerID.characterName, (object) steamPlayer1.playerID.playerName, (object) text));
      }
      else
      {
        if (mode1 != EChatMode.GROUP)
          return;
        CommandWindow.Log((object) Provider.localization.format("Group", (object) steamPlayer1.playerID.characterName, (object) steamPlayer1.playerID.playerName, (object) text));
      }
      Color chatted = Color.white;
      if (steamPlayer1.isAdmin)
        chatted = Palette.ADMIN;
      else if (steamPlayer1.isPro)
        chatted = Palette.PRO;
      bool isVisible = true;
      if (ChatManager.onChatted != null)
        ChatManager.onChatted(steamPlayer1, mode1, ref chatted, text, ref isVisible);
      if (!ChatManager.process(steamPlayer1, text) || !isVisible || (double) Time.realtimeSinceStartup - (double) steamPlayer1.lastChat < (double) ChatManager.chatrate)
        return;
      steamPlayer1.lastChat = Time.realtimeSinceStartup;
      if (mode1 == EChatMode.GLOBAL)
        this.channel.send("tellChat", ESteamCall.OTHERS, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[4]
        {
          (object) steamID,
          (object) mode,
          (object) chatted,
          (object) text
        });
      else if (mode1 == EChatMode.LOCAL)
      {
        this.channel.send("tellChat", ESteamCall.OTHERS, steamPlayer1.player.transform.position, EffectManager.MEDIUM, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, (object) steamID, (object) mode, (object) chatted, (object) text);
      }
      else
      {
        if (mode1 != EChatMode.GROUP || !(steamPlayer1.playerID.group != CSteamID.Nil))
          return;
        for (int index = 0; index < Provider.clients.Count; ++index)
        {
          SteamPlayer steamPlayer2 = Provider.clients[index];
          if (steamPlayer2.playerID.group == steamPlayer1.playerID.group)
            this.channel.send("tellChat", steamPlayer2.playerID.steamID, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[4]
            {
              (object) steamID,
              (object) mode,
              (object) chatted,
              (object) text
            });
        }
      }
    }

    public static void sendChat(EChatMode mode, string text)
    {
      if (!Provider.isServer)
      {
        ChatManager.manager.channel.send("askChat", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[2]
        {
          (object) (byte) mode,
          (object) text
        });
      }
      else
      {
        if (Dedicator.isDedicated)
          return;
        SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(Provider.client);
        if (steamPlayer == null || !ChatManager.process(steamPlayer, text))
          return;
        ChatManager.list(Provider.client, mode, !Provider.isPro ? Color.white : Palette.PRO, text);
      }
    }

    public static void say(CSteamID target, string text, Color color)
    {
      if (!Provider.isServer)
        return;
      if (text.Length > ChatManager.LENGTH)
        text = text.Substring(0, ChatManager.LENGTH);
      if (Dedicator.isDedicated)
        ChatManager.manager.channel.send("tellChat", target, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[4]
        {
          (object) CSteamID.Nil,
          (object) 3,
          (object) color,
          (object) text
        });
      else
        ChatManager.list(CSteamID.Nil, EChatMode.WELCOME, color, text);
    }

    public static void say(string text, Color color)
    {
      if (!Provider.isServer)
        return;
      if (text.Length > ChatManager.LENGTH)
        text = text.Substring(0, ChatManager.LENGTH);
      if (Dedicator.isDedicated)
        ChatManager.manager.channel.send("tellChat", ESteamCall.OTHERS, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[4]
        {
          (object) CSteamID.Nil,
          (object) 4,
          (object) color,
          (object) text
        });
      else
        ChatManager.list(CSteamID.Nil, EChatMode.SAY, color, text);
    }

    private void onLevelLoaded(int level)
    {
      if (level <= Level.SETUP)
        return;
      for (int index = 0; index < ChatManager.chat.Length; ++index)
        ChatManager.chat[index] = (Chat) null;
    }

    private void onServerConnected(CSteamID steamID)
    {
      if (!Provider.isServer || !(ChatManager.welcomeText != string.Empty))
        return;
      SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(steamID);
      ChatManager.say(steamPlayer.playerID.steamID, string.Format(ChatManager.welcomeText, (object) steamPlayer.playerID.characterName), ChatManager.welcomeColor);
    }

    private void Start()
    {
      ChatManager.manager = this;
      ChatManager._chat = new Chat[16];
      Level.onLevelLoaded += new LevelLoaded(this.onLevelLoaded);
      Provider.onServerConnected += new Provider.ServerConnected(this.onServerConnected);
    }
  }
}
