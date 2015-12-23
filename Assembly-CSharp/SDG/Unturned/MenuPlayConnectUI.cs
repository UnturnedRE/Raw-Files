// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MenuPlayConnectUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider;
using System;
using System.Net;
using UnityEngine;

namespace SDG.Unturned
{
  public class MenuPlayConnectUI
  {
    private static Local localization;
    private static Sleek container;
    public static bool active;
    private static SleekField ipField;
    private static SleekUInt16Field portField;
    private static SleekField passwordField;
    private static SleekButtonIcon connectButton;
    private static bool isLaunched;

    public MenuPlayConnectUI()
    {
      MenuPlayConnectUI.localization = Localization.read("/Menu/Play/MenuPlayConnect.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Play/MenuPlayConnect/MenuPlayConnect.unity3d");
      MenuPlayConnectUI.container = new Sleek();
      MenuPlayConnectUI.container.positionOffset_X = 10;
      MenuPlayConnectUI.container.positionOffset_Y = 10;
      MenuPlayConnectUI.container.positionScale_Y = 1f;
      MenuPlayConnectUI.container.sizeOffset_X = -20;
      MenuPlayConnectUI.container.sizeOffset_Y = -20;
      MenuPlayConnectUI.container.sizeScale_X = 1f;
      MenuPlayConnectUI.container.sizeScale_Y = 1f;
      MenuUI.container.add(MenuPlayConnectUI.container);
      MenuPlayConnectUI.active = false;
      MenuPlayConnectUI.ipField = new SleekField();
      MenuPlayConnectUI.ipField.positionOffset_X = -100;
      MenuPlayConnectUI.ipField.positionOffset_Y = -75;
      MenuPlayConnectUI.ipField.positionScale_X = 0.5f;
      MenuPlayConnectUI.ipField.positionScale_Y = 0.5f;
      MenuPlayConnectUI.ipField.sizeOffset_X = 200;
      MenuPlayConnectUI.ipField.sizeOffset_Y = 30;
      MenuPlayConnectUI.ipField.maxLength = 64;
      MenuPlayConnectUI.ipField.addLabel(MenuPlayConnectUI.localization.format("IP_Field_Label"), ESleekSide.RIGHT);
      MenuPlayConnectUI.ipField.text = PlaySettings.connectIP;
      MenuPlayConnectUI.ipField.onTyped = new Typed(MenuPlayConnectUI.onTypedIPField);
      MenuPlayConnectUI.container.add((Sleek) MenuPlayConnectUI.ipField);
      MenuPlayConnectUI.portField = new SleekUInt16Field();
      MenuPlayConnectUI.portField.positionOffset_X = -100;
      MenuPlayConnectUI.portField.positionOffset_Y = -35;
      MenuPlayConnectUI.portField.positionScale_X = 0.5f;
      MenuPlayConnectUI.portField.positionScale_Y = 0.5f;
      MenuPlayConnectUI.portField.sizeOffset_X = 200;
      MenuPlayConnectUI.portField.sizeOffset_Y = 30;
      MenuPlayConnectUI.portField.addLabel(MenuPlayConnectUI.localization.format("Port_Field_Label"), ESleekSide.RIGHT);
      MenuPlayConnectUI.portField.state = PlaySettings.connectPort;
      MenuPlayConnectUI.portField.onTypedUInt16 = new TypedUInt16(MenuPlayConnectUI.onTypedPortField);
      MenuPlayConnectUI.container.add((Sleek) MenuPlayConnectUI.portField);
      MenuPlayConnectUI.passwordField = new SleekField();
      MenuPlayConnectUI.passwordField.positionOffset_X = -100;
      MenuPlayConnectUI.passwordField.positionOffset_Y = 5;
      MenuPlayConnectUI.passwordField.positionScale_X = 0.5f;
      MenuPlayConnectUI.passwordField.positionScale_Y = 0.5f;
      MenuPlayConnectUI.passwordField.sizeOffset_X = 200;
      MenuPlayConnectUI.passwordField.sizeOffset_Y = 30;
      MenuPlayConnectUI.passwordField.addLabel(MenuPlayConnectUI.localization.format("Password_Field_Label"), ESleekSide.RIGHT);
      MenuPlayConnectUI.passwordField.replace = MenuPlayConnectUI.localization.format("Password_Field_Replace").ToCharArray()[0];
      MenuPlayConnectUI.passwordField.text = PlaySettings.connectPassword;
      MenuPlayConnectUI.passwordField.onTyped = new Typed(MenuPlayConnectUI.onTypedPasswordField);
      MenuPlayConnectUI.container.add((Sleek) MenuPlayConnectUI.passwordField);
      MenuPlayConnectUI.connectButton = new SleekButtonIcon((Texture2D) bundle.load("Connect"));
      MenuPlayConnectUI.connectButton.positionOffset_X = -100;
      MenuPlayConnectUI.connectButton.positionOffset_Y = 45;
      MenuPlayConnectUI.connectButton.positionScale_X = 0.5f;
      MenuPlayConnectUI.connectButton.positionScale_Y = 0.5f;
      MenuPlayConnectUI.connectButton.sizeOffset_X = 200;
      MenuPlayConnectUI.connectButton.sizeOffset_Y = 30;
      MenuPlayConnectUI.connectButton.text = MenuPlayConnectUI.localization.format("Connect_Button");
      MenuPlayConnectUI.connectButton.tooltip = MenuPlayConnectUI.localization.format("Connect_Button_Tooltip");
      MenuPlayConnectUI.connectButton.onClickedButton = new ClickedButton(MenuPlayConnectUI.onClickedConnectButton);
      MenuPlayConnectUI.container.add((Sleek) MenuPlayConnectUI.connectButton);
      Provider.provider.matchmakingService.onAttemptUpdated = new TempSteamworksMatchmaking.AttemptUpdated(MenuPlayConnectUI.onAttemptUpdated);
      Provider.provider.matchmakingService.onTimedOut = new TempSteamworksMatchmaking.TimedOut(MenuPlayConnectUI.onTimedOut);
      if (!MenuPlayConnectUI.isLaunched)
      {
        MenuPlayConnectUI.isLaunched = true;
        uint ip;
        ushort port;
        string pass;
        if (CommandLine.tryGetConnect(Environment.CommandLine, out ip, out port, out pass))
        {
          Provider.provider.matchmakingService.connect(new SteamConnectionInfo(ip, port, pass));
          MenuUI.openAlert(MenuPlayConnectUI.localization.format("Connecting"));
        }
      }
      bundle.unload();
    }

    public static void connect(SteamConnectionInfo info)
    {
      Provider.provider.matchmakingService.connect(info);
    }

    public static void open()
    {
      if (MenuPlayConnectUI.active)
      {
        MenuPlayConnectUI.close();
      }
      else
      {
        MenuPlayConnectUI.active = true;
        MenuPlayConnectUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!MenuPlayConnectUI.active)
        return;
      MenuPlayConnectUI.active = false;
      MenuSettings.save();
      MenuPlayConnectUI.container.lerpPositionScale(0.0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onClickedConnectButton(SleekButton button)
    {
      if (!(MenuPlayConnectUI.ipField.text != string.Empty) || (int) MenuPlayConnectUI.portField.state == 0)
        return;
      if (Parser.checkIP(MenuPlayConnectUI.ipField.text))
      {
        SteamConnectionInfo info = new SteamConnectionInfo(MenuPlayConnectUI.ipField.text, MenuPlayConnectUI.portField.state, MenuPlayConnectUI.passwordField.text);
        MenuSettings.save();
        MenuPlayConnectUI.connect(info);
      }
      else
      {
        IPAddress[] hostAddresses = Dns.GetHostAddresses(MenuPlayConnectUI.ipField.text);
        if (hostAddresses.Length <= 0 || !Parser.checkIP(hostAddresses[0].ToString()))
          return;
        SteamConnectionInfo info = new SteamConnectionInfo(hostAddresses[0].ToString(), MenuPlayConnectUI.portField.state, MenuPlayConnectUI.passwordField.text);
        MenuSettings.save();
        MenuPlayConnectUI.connect(info);
      }
    }

    private static void onTypedIPField(SleekField field, string text)
    {
      PlaySettings.connectIP = text;
    }

    private static void onTypedPortField(SleekUInt16Field field, ushort state)
    {
      PlaySettings.connectPort = state;
    }

    private static void onTypedPasswordField(SleekField field, string text)
    {
      PlaySettings.connectPassword = text;
    }

    private static void onAttemptUpdated(int attempt)
    {
      MenuUI.openAlert(MenuPlayConnectUI.localization.format("Connecting", (object) attempt));
    }

    private static void onTimedOut()
    {
      if (Provider.connectionFailureInfo == ESteamConnectionFailureInfo.NONE)
        return;
      ESteamConnectionFailureInfo connectionFailureInfo = Provider.connectionFailureInfo;
      Provider.resetConnectionFailure();
      if (connectionFailureInfo == ESteamConnectionFailureInfo.PRO)
        MenuUI.alert(MenuPlayConnectUI.localization.format("Pro"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.PASSWORD)
        MenuUI.alert(MenuPlayConnectUI.localization.format("Password"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.FULL)
      {
        MenuUI.alert(MenuPlayConnectUI.localization.format("Full"));
      }
      else
      {
        if (connectionFailureInfo != ESteamConnectionFailureInfo.TIMED_OUT)
          return;
        MenuUI.alert(MenuPlayConnectUI.localization.format("Timed_Out"));
      }
    }
  }
}
