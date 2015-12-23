// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MenuPlayServersUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider;
using Steamworks;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class MenuPlayServersUI
  {
    public static Local localization;
    public static Bundle icons;
    private static Sleek container;
    public static bool active;
    private static LevelInfo[] levels;
    private static SleekScrollBox serverBox;
    private static SleekBox infoBox;
    private static List<SleekServer> serverButtons;
    private static SleekButton sortName;
    private static SleekButton sortPlayers;
    private static SleekButton sortPing;
    private static SleekLabel passwordLabel;
    private static SleekField passwordField;
    private static SleekButtonIcon refreshInternetButton;
    private static SleekButtonIcon refreshLANButton;
    private static SleekButtonIcon refreshHistoryButton;
    private static SleekButtonIcon refreshFavoritesButton;
    private static SleekButtonIcon refreshFriendsButton;
    private static SleekButtonState mapButtonState;
    private static SleekButtonState passwordButtonState;
    private static SleekButtonState workshopButtonState;
    private static SleekButtonState attendanceButtonState;
    private static SleekButtonState protectionButtonState;
    private static SleekButtonState combatButtonState;
    private static SleekButtonState modeButtonState;
    private static SleekButtonState cameraButtonState;

    public MenuPlayServersUI()
    {
      if (MenuPlayServersUI.icons != null)
        MenuPlayServersUI.icons.unload();
      MenuPlayServersUI.localization = Localization.read("/Menu/Play/MenuPlayServers.dat");
      MenuPlayServersUI.icons = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Play/MenuPlayServers/MenuPlayServers.unity3d");
      MenuPlayServersUI.container = new Sleek();
      MenuPlayServersUI.container.positionOffset_X = 10;
      MenuPlayServersUI.container.positionOffset_Y = 10;
      MenuPlayServersUI.container.positionScale_Y = 1f;
      MenuPlayServersUI.container.sizeOffset_X = -20;
      MenuPlayServersUI.container.sizeOffset_Y = -20;
      MenuPlayServersUI.container.sizeScale_X = 1f;
      MenuPlayServersUI.container.sizeScale_Y = 1f;
      MenuUI.container.add(MenuPlayServersUI.container);
      MenuPlayServersUI.active = false;
      MenuPlayServersUI.serverBox = new SleekScrollBox();
      MenuPlayServersUI.serverBox.positionOffset_Y = 40;
      MenuPlayServersUI.serverBox.sizeOffset_Y = -160;
      MenuPlayServersUI.serverBox.sizeScale_X = 1f;
      MenuPlayServersUI.serverBox.sizeScale_Y = 1f;
      MenuPlayServersUI.serverBox.area = new Rect(0.0f, 0.0f, 5f, 0.0f);
      MenuPlayServersUI.container.add((Sleek) MenuPlayServersUI.serverBox);
      MenuPlayServersUI.sortName = new SleekButton();
      MenuPlayServersUI.sortName.sizeOffset_X = -250;
      MenuPlayServersUI.sortName.sizeOffset_Y = 30;
      MenuPlayServersUI.sortName.sizeScale_X = 1f;
      MenuPlayServersUI.sortName.text = MenuPlayServersUI.localization.format("Sort_Name");
      MenuPlayServersUI.sortName.tooltip = MenuPlayServersUI.localization.format("Sort_Name_Tooltip");
      MenuPlayServersUI.sortName.onClickedButton = new ClickedButton(MenuPlayServersUI.onClickedSortNameButton);
      MenuPlayServersUI.container.add((Sleek) MenuPlayServersUI.sortName);
      MenuPlayServersUI.sortPlayers = new SleekButton();
      MenuPlayServersUI.sortPlayers.positionOffset_X = -240;
      MenuPlayServersUI.sortPlayers.positionScale_X = 1f;
      MenuPlayServersUI.sortPlayers.sizeOffset_X = 100;
      MenuPlayServersUI.sortPlayers.sizeOffset_Y = 30;
      MenuPlayServersUI.sortPlayers.text = MenuPlayServersUI.localization.format("Sort_Players");
      MenuPlayServersUI.sortPlayers.tooltip = MenuPlayServersUI.localization.format("Sort_Players_Tooltip");
      MenuPlayServersUI.sortPlayers.onClickedButton = new ClickedButton(MenuPlayServersUI.onClickedSortPlayersButton);
      MenuPlayServersUI.container.add((Sleek) MenuPlayServersUI.sortPlayers);
      MenuPlayServersUI.sortPing = new SleekButton();
      MenuPlayServersUI.sortPing.positionOffset_X = -130;
      MenuPlayServersUI.sortPing.positionScale_X = 1f;
      MenuPlayServersUI.sortPing.sizeOffset_X = 100;
      MenuPlayServersUI.sortPing.sizeOffset_Y = 30;
      MenuPlayServersUI.sortPing.text = MenuPlayServersUI.localization.format("Sort_Ping");
      MenuPlayServersUI.sortPing.tooltip = MenuPlayServersUI.localization.format("Sort_Ping_Tooltip");
      MenuPlayServersUI.sortPing.onClickedButton = new ClickedButton(MenuPlayServersUI.onClickedSortPingButton);
      MenuPlayServersUI.container.add((Sleek) MenuPlayServersUI.sortPing);
      MenuPlayServersUI.infoBox = new SleekBox();
      MenuPlayServersUI.infoBox.positionOffset_Y = 40;
      MenuPlayServersUI.infoBox.sizeOffset_X = -30;
      MenuPlayServersUI.infoBox.sizeScale_X = 1f;
      MenuPlayServersUI.infoBox.sizeOffset_Y = 50;
      MenuPlayServersUI.infoBox.text = MenuPlayServersUI.localization.format("No_Servers", (object) Provider.APP_VERSION);
      MenuPlayServersUI.infoBox.fontSize = 14;
      MenuPlayServersUI.container.add((Sleek) MenuPlayServersUI.infoBox);
      MenuPlayServersUI.infoBox.isVisible = false;
      MenuPlayServersUI.serverButtons = new List<SleekServer>();
      Provider.provider.matchmakingService.onMasterServerAdded = new TempSteamworksMatchmaking.MasterServerAdded(MenuPlayServersUI.onMasterServerAdded);
      Provider.provider.matchmakingService.onMasterServerRemoved = new TempSteamworksMatchmaking.MasterServerRemoved(MenuPlayServersUI.onMasterServerRemoved);
      Provider.provider.matchmakingService.onMasterServerResorted = new TempSteamworksMatchmaking.MasterServerResorted(MenuPlayServersUI.onMasterServerResorted);
      Provider.provider.matchmakingService.onMasterServerRefreshed = new TempSteamworksMatchmaking.MasterServerRefreshed(MenuPlayServersUI.onMasterServerRefreshed);
      MenuPlayServersUI.passwordLabel = new SleekLabel();
      MenuPlayServersUI.passwordLabel.positionOffset_X = 5;
      MenuPlayServersUI.passwordLabel.positionOffset_Y = -70;
      MenuPlayServersUI.passwordLabel.positionScale_X = 0.8f;
      MenuPlayServersUI.passwordLabel.positionScale_Y = 1f;
      MenuPlayServersUI.passwordLabel.sizeOffset_Y = 30;
      MenuPlayServersUI.passwordLabel.sizeScale_X = 0.2f;
      MenuPlayServersUI.passwordLabel.text = MenuPlayServersUI.localization.format("Password_Field_Label");
      MenuPlayServersUI.container.add((Sleek) MenuPlayServersUI.passwordLabel);
      MenuPlayServersUI.passwordField = new SleekField();
      MenuPlayServersUI.passwordField.positionOffset_X = 5;
      MenuPlayServersUI.passwordField.positionOffset_Y = -30;
      MenuPlayServersUI.passwordField.positionScale_X = 0.8f;
      MenuPlayServersUI.passwordField.positionScale_Y = 1f;
      MenuPlayServersUI.passwordField.sizeOffset_Y = 30;
      MenuPlayServersUI.passwordField.sizeScale_X = 0.2f;
      MenuPlayServersUI.passwordField.replace = MenuPlayServersUI.localization.format("Password_Field_Replace").ToCharArray()[0];
      MenuPlayServersUI.passwordField.text = PlaySettings.serversPassword;
      MenuPlayServersUI.passwordField.onTyped = new Typed(MenuPlayServersUI.onTypedPasswordField);
      MenuPlayServersUI.container.add((Sleek) MenuPlayServersUI.passwordField);
      MenuPlayServersUI.refreshInternetButton = new SleekButtonIcon((Texture2D) MenuPlayServersUI.icons.load("Refresh_Internet"));
      MenuPlayServersUI.refreshInternetButton.sizeOffset_X = -5;
      MenuPlayServersUI.refreshInternetButton.positionOffset_Y = -110;
      MenuPlayServersUI.refreshInternetButton.positionScale_Y = 1f;
      MenuPlayServersUI.refreshInternetButton.sizeOffset_Y = 30;
      MenuPlayServersUI.refreshInternetButton.sizeScale_X = 0.2f;
      MenuPlayServersUI.refreshInternetButton.text = MenuPlayServersUI.localization.format("Refresh_Internet_Button");
      MenuPlayServersUI.refreshInternetButton.tooltip = MenuPlayServersUI.localization.format("Refresh_Internet_Button_Tooltip");
      MenuPlayServersUI.refreshInternetButton.onClickedButton = new ClickedButton(MenuPlayServersUI.onClickedRefreshInternetButton);
      MenuPlayServersUI.container.add((Sleek) MenuPlayServersUI.refreshInternetButton);
      MenuPlayServersUI.refreshLANButton = new SleekButtonIcon((Texture2D) MenuPlayServersUI.icons.load("Refresh_LAN"));
      MenuPlayServersUI.refreshLANButton.positionOffset_X = 5;
      MenuPlayServersUI.refreshLANButton.positionOffset_Y = -110;
      MenuPlayServersUI.refreshLANButton.positionScale_X = 0.2f;
      MenuPlayServersUI.refreshLANButton.positionScale_Y = 1f;
      MenuPlayServersUI.refreshLANButton.sizeOffset_X = -10;
      MenuPlayServersUI.refreshLANButton.sizeOffset_Y = 30;
      MenuPlayServersUI.refreshLANButton.sizeScale_X = 0.2f;
      MenuPlayServersUI.refreshLANButton.text = MenuPlayServersUI.localization.format("Refresh_LAN_Button");
      MenuPlayServersUI.refreshLANButton.tooltip = MenuPlayServersUI.localization.format("Refresh_LAN_Button_Tooltip");
      MenuPlayServersUI.refreshLANButton.onClickedButton = new ClickedButton(MenuPlayServersUI.onClickedRefreshLANButton);
      MenuPlayServersUI.container.add((Sleek) MenuPlayServersUI.refreshLANButton);
      MenuPlayServersUI.refreshHistoryButton = new SleekButtonIcon((Texture2D) MenuPlayServersUI.icons.load("Refresh_History"));
      MenuPlayServersUI.refreshHistoryButton.positionOffset_X = 5;
      MenuPlayServersUI.refreshHistoryButton.sizeOffset_X = -10;
      MenuPlayServersUI.refreshHistoryButton.positionOffset_Y = -110;
      MenuPlayServersUI.refreshHistoryButton.positionScale_Y = 1f;
      MenuPlayServersUI.refreshHistoryButton.positionScale_X = 0.4f;
      MenuPlayServersUI.refreshHistoryButton.sizeOffset_Y = 30;
      MenuPlayServersUI.refreshHistoryButton.sizeScale_X = 0.2f;
      MenuPlayServersUI.refreshHistoryButton.text = MenuPlayServersUI.localization.format("Refresh_History_Button");
      MenuPlayServersUI.refreshHistoryButton.tooltip = MenuPlayServersUI.localization.format("Refresh_History_Button_Tooltip");
      MenuPlayServersUI.refreshHistoryButton.onClickedButton = new ClickedButton(MenuPlayServersUI.onClickedRefreshHistoryButton);
      MenuPlayServersUI.container.add((Sleek) MenuPlayServersUI.refreshHistoryButton);
      MenuPlayServersUI.refreshFavoritesButton = new SleekButtonIcon((Texture2D) MenuPlayServersUI.icons.load("Refresh_Favorites"));
      MenuPlayServersUI.refreshFavoritesButton.positionOffset_X = 5;
      MenuPlayServersUI.refreshFavoritesButton.sizeOffset_X = -10;
      MenuPlayServersUI.refreshFavoritesButton.positionOffset_Y = -110;
      MenuPlayServersUI.refreshFavoritesButton.positionScale_Y = 1f;
      MenuPlayServersUI.refreshFavoritesButton.positionScale_X = 0.6f;
      MenuPlayServersUI.refreshFavoritesButton.sizeOffset_Y = 30;
      MenuPlayServersUI.refreshFavoritesButton.sizeScale_X = 0.2f;
      MenuPlayServersUI.refreshFavoritesButton.text = MenuPlayServersUI.localization.format("Refresh_Favorites_Button");
      MenuPlayServersUI.refreshFavoritesButton.tooltip = MenuPlayServersUI.localization.format("Refresh_Favorites_Button_Tooltip");
      MenuPlayServersUI.refreshFavoritesButton.onClickedButton = new ClickedButton(MenuPlayServersUI.onClickedRefreshFavoritesButton);
      MenuPlayServersUI.container.add((Sleek) MenuPlayServersUI.refreshFavoritesButton);
      MenuPlayServersUI.refreshFriendsButton = new SleekButtonIcon((Texture2D) MenuPlayServersUI.icons.load("Refresh_Friends"));
      MenuPlayServersUI.refreshFriendsButton.positionOffset_X = 5;
      MenuPlayServersUI.refreshFriendsButton.sizeOffset_X = -5;
      MenuPlayServersUI.refreshFriendsButton.positionOffset_Y = -110;
      MenuPlayServersUI.refreshFriendsButton.positionScale_Y = 1f;
      MenuPlayServersUI.refreshFriendsButton.positionScale_X = 0.8f;
      MenuPlayServersUI.refreshFriendsButton.sizeOffset_Y = 30;
      MenuPlayServersUI.refreshFriendsButton.sizeScale_X = 0.2f;
      MenuPlayServersUI.refreshFriendsButton.text = MenuPlayServersUI.localization.format("Refresh_Friends_Button");
      MenuPlayServersUI.refreshFriendsButton.tooltip = MenuPlayServersUI.localization.format("Refresh_Friends_Button_Tooltip");
      MenuPlayServersUI.refreshFriendsButton.onClickedButton = new ClickedButton(MenuPlayServersUI.onClickedRefreshFriendsButton);
      MenuPlayServersUI.container.add((Sleek) MenuPlayServersUI.refreshFriendsButton);
      MenuPlayServersUI.mapButtonState = new SleekButtonState(new GUIContent[0]);
      MenuPlayServersUI.mapButtonState.positionOffset_X = 5;
      MenuPlayServersUI.mapButtonState.positionOffset_Y = -70;
      MenuPlayServersUI.mapButtonState.positionScale_X = 0.4f;
      MenuPlayServersUI.mapButtonState.positionScale_Y = 1f;
      MenuPlayServersUI.mapButtonState.sizeOffset_X = -10;
      MenuPlayServersUI.mapButtonState.sizeOffset_Y = 30;
      MenuPlayServersUI.mapButtonState.sizeScale_X = 0.2f;
      MenuPlayServersUI.mapButtonState.onSwappedState = new SwappedState(MenuPlayServersUI.onSwappedMapState);
      MenuPlayServersUI.container.add((Sleek) MenuPlayServersUI.mapButtonState);
      MenuPlayServersUI.passwordButtonState = new SleekButtonState(new GUIContent[3]
      {
        new GUIContent(MenuPlayServersUI.localization.format("No_Password_Button")),
        new GUIContent(MenuPlayServersUI.localization.format("Yes_Password_Button")),
        new GUIContent(MenuPlayServersUI.localization.format("Any_Password_Button"))
      });
      MenuPlayServersUI.passwordButtonState.positionOffset_Y = -70;
      MenuPlayServersUI.passwordButtonState.positionScale_Y = 1f;
      MenuPlayServersUI.passwordButtonState.sizeOffset_X = -5;
      MenuPlayServersUI.passwordButtonState.sizeOffset_Y = 30;
      MenuPlayServersUI.passwordButtonState.sizeScale_X = 0.2f;
      MenuPlayServersUI.passwordButtonState.state = (int) FilterSettings.filterPassword;
      MenuPlayServersUI.passwordButtonState.onSwappedState = new SwappedState(MenuPlayServersUI.onSwappedPasswordState);
      MenuPlayServersUI.container.add((Sleek) MenuPlayServersUI.passwordButtonState);
      MenuPlayServersUI.workshopButtonState = new SleekButtonState(new GUIContent[3]
      {
        new GUIContent(MenuPlayServersUI.localization.format("No_Workshop_Button")),
        new GUIContent(MenuPlayServersUI.localization.format("Yes_Workshop_Button")),
        new GUIContent(MenuPlayServersUI.localization.format("Any_Workshop_Button"))
      });
      MenuPlayServersUI.workshopButtonState.positionOffset_Y = -30;
      MenuPlayServersUI.workshopButtonState.positionScale_Y = 1f;
      MenuPlayServersUI.workshopButtonState.sizeOffset_X = -5;
      MenuPlayServersUI.workshopButtonState.sizeOffset_Y = 30;
      MenuPlayServersUI.workshopButtonState.sizeScale_X = 0.2f;
      MenuPlayServersUI.workshopButtonState.state = (int) FilterSettings.filterWorkshop;
      MenuPlayServersUI.workshopButtonState.onSwappedState = new SwappedState(MenuPlayServersUI.onSwappedWorkshopState);
      MenuPlayServersUI.container.add((Sleek) MenuPlayServersUI.workshopButtonState);
      MenuPlayServersUI.attendanceButtonState = new SleekButtonState(new GUIContent[3]
      {
        new GUIContent(MenuPlayServersUI.localization.format("Empty_Button"), (Texture) MenuPlayServersUI.icons.load("Empty")),
        new GUIContent(MenuPlayServersUI.localization.format("Space_Button"), (Texture) MenuPlayServersUI.icons.load("Space")),
        new GUIContent(MenuPlayServersUI.localization.format("Any_Attendance_Button"))
      });
      MenuPlayServersUI.attendanceButtonState.positionOffset_X = 5;
      MenuPlayServersUI.attendanceButtonState.positionOffset_Y = -30;
      MenuPlayServersUI.attendanceButtonState.positionScale_X = 0.4f;
      MenuPlayServersUI.attendanceButtonState.positionScale_Y = 1f;
      MenuPlayServersUI.attendanceButtonState.sizeOffset_X = -10;
      MenuPlayServersUI.attendanceButtonState.sizeOffset_Y = 30;
      MenuPlayServersUI.attendanceButtonState.sizeScale_X = 0.2f;
      MenuPlayServersUI.attendanceButtonState.state = (int) FilterSettings.filterAttendance;
      MenuPlayServersUI.attendanceButtonState.onSwappedState = new SwappedState(MenuPlayServersUI.onSwappedAttendanceState);
      MenuPlayServersUI.container.add((Sleek) MenuPlayServersUI.attendanceButtonState);
      MenuPlayServersUI.protectionButtonState = new SleekButtonState(new GUIContent[3]
      {
        new GUIContent(MenuPlayServersUI.localization.format("Secure_Button"), (Texture) MenuPlayServersUI.icons.load("Secure")),
        new GUIContent(MenuPlayServersUI.localization.format("Insecure_Button"), (Texture) MenuPlayServersUI.icons.load("Insecure")),
        new GUIContent(MenuPlayServersUI.localization.format("Any_Protection_Button"))
      });
      MenuPlayServersUI.protectionButtonState.positionOffset_X = 5;
      MenuPlayServersUI.protectionButtonState.positionOffset_Y = -70;
      MenuPlayServersUI.protectionButtonState.positionScale_X = 0.6f;
      MenuPlayServersUI.protectionButtonState.positionScale_Y = 1f;
      MenuPlayServersUI.protectionButtonState.sizeOffset_X = -10;
      MenuPlayServersUI.protectionButtonState.sizeOffset_Y = 30;
      MenuPlayServersUI.protectionButtonState.sizeScale_X = 0.2f;
      MenuPlayServersUI.protectionButtonState.state = (int) FilterSettings.filterProtection;
      MenuPlayServersUI.protectionButtonState.onSwappedState = new SwappedState(MenuPlayServersUI.onSwappedProtectionState);
      MenuPlayServersUI.container.add((Sleek) MenuPlayServersUI.protectionButtonState);
      MenuPlayServersUI.combatButtonState = new SleekButtonState(new GUIContent[3]
      {
        new GUIContent(MenuPlayServersUI.localization.format("PvP_Button"), (Texture) MenuPlayServersUI.icons.load("PvP")),
        new GUIContent(MenuPlayServersUI.localization.format("PvE_Button"), (Texture) MenuPlayServersUI.icons.load("PvE")),
        new GUIContent(MenuPlayServersUI.localization.format("Any_Combat_Button"))
      });
      MenuPlayServersUI.combatButtonState.positionOffset_X = 5;
      MenuPlayServersUI.combatButtonState.positionOffset_Y = -70;
      MenuPlayServersUI.combatButtonState.positionScale_X = 0.2f;
      MenuPlayServersUI.combatButtonState.positionScale_Y = 1f;
      MenuPlayServersUI.combatButtonState.sizeOffset_X = -10;
      MenuPlayServersUI.combatButtonState.sizeOffset_Y = 30;
      MenuPlayServersUI.combatButtonState.sizeScale_X = 0.2f;
      MenuPlayServersUI.combatButtonState.state = (int) FilterSettings.filterCombat;
      MenuPlayServersUI.combatButtonState.onSwappedState = new SwappedState(MenuPlayServersUI.onSwappedCombatState);
      MenuPlayServersUI.container.add((Sleek) MenuPlayServersUI.combatButtonState);
      MenuPlayServersUI.modeButtonState = new SleekButtonState(new GUIContent[5]
      {
        new GUIContent(MenuPlayServersUI.localization.format("Easy_Button"), (Texture) MenuPlayServersUI.icons.load("Easy")),
        new GUIContent(MenuPlayServersUI.localization.format("Normal_Button"), (Texture) MenuPlayServersUI.icons.load("Normal")),
        new GUIContent(MenuPlayServersUI.localization.format("Hard_Button"), (Texture) MenuPlayServersUI.icons.load("Hard")),
        new GUIContent(MenuPlayServersUI.localization.format("Pro_Button"), (Texture) MenuPlayServersUI.icons.load("Pro")),
        new GUIContent(MenuPlayServersUI.localization.format("Any_Mode_Button"))
      });
      MenuPlayServersUI.modeButtonState.positionOffset_X = 5;
      MenuPlayServersUI.modeButtonState.positionOffset_Y = -30;
      MenuPlayServersUI.modeButtonState.positionScale_X = 0.6f;
      MenuPlayServersUI.modeButtonState.positionScale_Y = 1f;
      MenuPlayServersUI.modeButtonState.sizeOffset_X = -10;
      MenuPlayServersUI.modeButtonState.sizeOffset_Y = 30;
      MenuPlayServersUI.modeButtonState.sizeScale_X = 0.2f;
      MenuPlayServersUI.modeButtonState.state = (int) FilterSettings.filterMode;
      MenuPlayServersUI.modeButtonState.onSwappedState = new SwappedState(MenuPlayServersUI.onSwappedModeState);
      MenuPlayServersUI.container.add((Sleek) MenuPlayServersUI.modeButtonState);
      MenuPlayServersUI.cameraButtonState = new SleekButtonState(new GUIContent[4]
      {
        new GUIContent(MenuPlayServersUI.localization.format("First_Button"), (Texture) MenuPlayServersUI.icons.load("First")),
        new GUIContent(MenuPlayServersUI.localization.format("Third_Button"), (Texture) MenuPlayServersUI.icons.load("Third")),
        new GUIContent(MenuPlayServersUI.localization.format("Both_Button"), (Texture) MenuPlayServersUI.icons.load("Both")),
        new GUIContent(MenuPlayServersUI.localization.format("Any_Camera_Button"))
      });
      MenuPlayServersUI.cameraButtonState.positionOffset_X = 5;
      MenuPlayServersUI.cameraButtonState.positionOffset_Y = -30;
      MenuPlayServersUI.cameraButtonState.positionScale_X = 0.2f;
      MenuPlayServersUI.cameraButtonState.positionScale_Y = 1f;
      MenuPlayServersUI.cameraButtonState.sizeOffset_X = -10;
      MenuPlayServersUI.cameraButtonState.sizeOffset_Y = 30;
      MenuPlayServersUI.cameraButtonState.sizeScale_X = 0.2f;
      MenuPlayServersUI.cameraButtonState.state = (int) FilterSettings.filterCamera;
      MenuPlayServersUI.cameraButtonState.onSwappedState = new SwappedState(MenuPlayServersUI.onSwappedCameraState);
      MenuPlayServersUI.container.add((Sleek) MenuPlayServersUI.cameraButtonState);
      MenuPlayServersUI.onLevelsRefreshed();
      Level.onLevelsRefreshed += new LevelsRefreshed(MenuPlayServersUI.onLevelsRefreshed);
    }

    public static void open()
    {
      if (MenuPlayServersUI.active)
      {
        MenuPlayServersUI.close();
      }
      else
      {
        MenuPlayServersUI.active = true;
        MenuPlayServersUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!MenuPlayServersUI.active)
        return;
      MenuPlayServersUI.active = false;
      MenuSettings.save();
      MenuPlayServersUI.container.lerpPositionScale(0.0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onClickedServer(SleekServer server, SteamServerInfo info)
    {
      if (info.mode == EGameMode.PRO && !Provider.isPro || info.players >= info.maxPlayers || info.isPassworded && MenuPlayServersUI.passwordField.text == string.Empty)
        return;
      MenuSettings.save();
      Provider.connect(info, MenuPlayServersUI.passwordField.text);
    }

    private static void onMasterServerAdded(int insert, SteamServerInfo info)
    {
      if (insert > MenuPlayServersUI.serverButtons.Count)
        return;
      SleekServer sleekServer = new SleekServer(info);
      sleekServer.positionOffset_Y = insert * 40;
      sleekServer.sizeOffset_X = -30;
      sleekServer.sizeOffset_Y = 30;
      sleekServer.sizeScale_X = 1f;
      sleekServer.onClickedServer = new ClickedServer(MenuPlayServersUI.onClickedServer);
      MenuPlayServersUI.serverBox.add((Sleek) sleekServer);
      for (int index = insert; index < MenuPlayServersUI.serverButtons.Count; ++index)
        MenuPlayServersUI.serverButtons[index].positionOffset_Y += 40;
      MenuPlayServersUI.serverButtons.Insert(insert, sleekServer);
      MenuPlayServersUI.serverBox.area = new Rect(0.0f, 0.0f, 5f, (float) (MenuPlayServersUI.serverButtons.Count * 40 - 10));
    }

    private static void onMasterServerRemoved()
    {
      MenuPlayServersUI.infoBox.isVisible = false;
      MenuPlayServersUI.serverBox.remove();
      MenuPlayServersUI.serverButtons.Clear();
    }

    private static void onMasterServerResorted()
    {
      MenuPlayServersUI.infoBox.isVisible = false;
      MenuPlayServersUI.serverBox.remove();
      MenuPlayServersUI.serverButtons.Clear();
      for (int index = 0; index < Provider.provider.matchmakingService.serverList.Count; ++index)
      {
        SleekServer sleekServer = new SleekServer(Provider.provider.matchmakingService.serverList[index]);
        sleekServer.positionOffset_Y = index * 40;
        sleekServer.sizeOffset_X = -30;
        sleekServer.sizeOffset_Y = 30;
        sleekServer.sizeScale_X = 1f;
        sleekServer.onClickedServer = new ClickedServer(MenuPlayServersUI.onClickedServer);
        MenuPlayServersUI.serverBox.add((Sleek) sleekServer);
        MenuPlayServersUI.serverButtons.Add(sleekServer);
      }
      MenuPlayServersUI.serverBox.area = new Rect(0.0f, 0.0f, 5f, (float) (Provider.provider.matchmakingService.serverList.Count * 40 - 10));
    }

    private static void onMasterServerRefreshed(EMatchMakingServerResponse response)
    {
      if (MenuPlayServersUI.serverButtons.Count != 0)
        return;
      MenuPlayServersUI.infoBox.isVisible = true;
    }

    private static void onClickedSortNameButton(SleekButton button)
    {
      Provider.provider.matchmakingService.sortMasterServer((IComparer<SteamServerInfo>) new SteamServerInfoNameComparator());
    }

    private static void onClickedSortPlayersButton(SleekButton button)
    {
      Provider.provider.matchmakingService.sortMasterServer((IComparer<SteamServerInfo>) new SteamServerInfoPlayersComparator());
    }

    private static void onClickedSortPingButton(SleekButton button)
    {
      Provider.provider.matchmakingService.sortMasterServer((IComparer<SteamServerInfo>) new SteamServerInfoPingComparator());
    }

    private static void onClickedRefreshInternetButton(SleekButton button)
    {
      Provider.provider.matchmakingService.refreshMasterServer(ESteamServerList.INTERNET, FilterSettings.filterMap, FilterSettings.filterPassword, FilterSettings.filterWorkshop, FilterSettings.filterAttendance, FilterSettings.filterProtection, FilterSettings.filterCombat, FilterSettings.filterMode, FilterSettings.filterCamera);
    }

    private static void onClickedRefreshLANButton(SleekButton button)
    {
      Provider.provider.matchmakingService.refreshMasterServer(ESteamServerList.LAN, FilterSettings.filterMap, FilterSettings.filterPassword, FilterSettings.filterWorkshop, FilterSettings.filterAttendance, FilterSettings.filterProtection, FilterSettings.filterCombat, FilterSettings.filterMode, FilterSettings.filterCamera);
    }

    private static void onClickedRefreshHistoryButton(SleekButton button)
    {
      Provider.provider.matchmakingService.refreshMasterServer(ESteamServerList.HISTORY, FilterSettings.filterMap, FilterSettings.filterPassword, FilterSettings.filterWorkshop, FilterSettings.filterAttendance, FilterSettings.filterProtection, FilterSettings.filterCombat, FilterSettings.filterMode, FilterSettings.filterCamera);
    }

    private static void onClickedRefreshFavoritesButton(SleekButton button)
    {
      Provider.provider.matchmakingService.refreshMasterServer(ESteamServerList.FAVORITES, FilterSettings.filterMap, FilterSettings.filterPassword, FilterSettings.filterWorkshop, FilterSettings.filterAttendance, FilterSettings.filterProtection, FilterSettings.filterCombat, FilterSettings.filterMode, FilterSettings.filterCamera);
    }

    private static void onClickedRefreshFriendsButton(SleekButton button)
    {
      Provider.provider.matchmakingService.refreshMasterServer(ESteamServerList.FRIENDS, FilterSettings.filterMap, FilterSettings.filterPassword, FilterSettings.filterWorkshop, FilterSettings.filterAttendance, FilterSettings.filterProtection, FilterSettings.filterCombat, FilterSettings.filterMode, FilterSettings.filterCamera);
    }

    private static void onTypedPasswordField(SleekField field, string text)
    {
      PlaySettings.serversPassword = text;
    }

    private static void onSwappedMapState(SleekButtonState button, int index)
    {
      if (index > 0)
        FilterSettings.filterMap = MenuPlayServersUI.levels[index - 1].name;
      else
        FilterSettings.filterMap = string.Empty;
    }

    private static void onSwappedPasswordState(SleekButtonState button, int index)
    {
      FilterSettings.filterPassword = (EPassword) index;
    }

    private static void onSwappedWorkshopState(SleekButtonState button, int index)
    {
      FilterSettings.filterWorkshop = (EWorkshop) index;
    }

    private static void onSwappedAttendanceState(SleekButtonState button, int index)
    {
      FilterSettings.filterAttendance = (EAttendance) index;
    }

    private static void onSwappedProtectionState(SleekButtonState button, int index)
    {
      FilterSettings.filterProtection = (EProtection) index;
    }

    private static void onSwappedCombatState(SleekButtonState button, int index)
    {
      FilterSettings.filterCombat = (ECombat) index;
    }

    private static void onSwappedModeState(SleekButtonState button, int index)
    {
      FilterSettings.filterMode = (EGameMode) index;
    }

    private static void onSwappedCameraState(SleekButtonState button, int index)
    {
      FilterSettings.filterCamera = (ECameraMode) index;
    }

    private static void onLevelsRefreshed()
    {
      MenuPlayServersUI.levels = Level.getLevels();
      GUIContent[] guiContentArray = new GUIContent[MenuPlayServersUI.levels.Length + 1];
      guiContentArray[0] = new GUIContent(MenuPlayServersUI.localization.format("Any_Map"));
      for (int index = 0; index < MenuPlayServersUI.levels.Length; ++index)
      {
        LevelInfo levelInfo = MenuPlayServersUI.levels[index];
        if (levelInfo != null)
          guiContentArray[index + 1] = new GUIContent(levelInfo.name);
      }
      int index1 = -1;
      for (int index2 = 0; index2 < MenuPlayServersUI.levels.Length; ++index2)
      {
        LevelInfo levelInfo = MenuPlayServersUI.levels[index2];
        if (levelInfo != null && levelInfo.name == FilterSettings.filterMap)
        {
          index1 = index2;
          break;
        }
      }
      int num;
      if (index1 != -1 && MenuPlayServersUI.levels[index1] != null)
      {
        FilterSettings.filterMap = MenuPlayServersUI.levels[index1].name;
        num = index1 + 1;
      }
      else
      {
        FilterSettings.filterMap = string.Empty;
        num = 0;
      }
      MenuPlayServersUI.mapButtonState.setContent(guiContentArray);
      MenuPlayServersUI.mapButtonState.state = num;
    }
  }
}
