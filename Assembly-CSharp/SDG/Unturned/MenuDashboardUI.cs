// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MenuDashboardUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Newtonsoft.Json;
using SDG.Provider.Services.Store;
using SDG.Provider.Services.Web;
using SDG.SteamworksProvider.Services.Store;
using System;
using System.Text;
using UnityEngine;

namespace SDG.Unturned
{
  public class MenuDashboardUI
  {
    private static Local localization;
    private static Sleek container;
    public static bool active;
    private static SleekButtonIcon playButton;
    private static SleekButtonIcon survivorsButton;
    private static SleekButtonIcon configurationButton;
    private static SleekButtonIcon workshopButton;
    private static SleekButtonIcon exitButton;
    private static SleekButton proButton;
    private static SleekLabel proLabel;
    private static SleekLabel featureLabel;
    private static SleekScrollBox newsBox;
    private static NewsResponse newsResponse;
    private static IWebRequestHandle newsRequestHandle;

    public MenuDashboardUI()
    {
      MenuDashboardUI.newsRequestHandle = Provider.provider.webService.createRequest("http://api.steampowered.com/ISteamNews/GetNewsForApp/v0002/", ERequestType.GET, new WebRequestReadyCallback(MenuDashboardUI.onWebRequestReady));
      Provider.provider.webService.updateRequest(MenuDashboardUI.newsRequestHandle, "appid", "304930");
      Provider.provider.webService.updateRequest(MenuDashboardUI.newsRequestHandle, "count", "10");
      Provider.provider.webService.updateRequest(MenuDashboardUI.newsRequestHandle, "feeds", "steam_community_announcements");
      Provider.provider.webService.submitRequest(MenuDashboardUI.newsRequestHandle);
      MenuDashboardUI.localization = Localization.read("/Menu/MenuDashboard.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/MenuDashboard/MenuDashboard.unity3d");
      MenuDashboardUI.container = new Sleek();
      MenuDashboardUI.container.positionOffset_X = 10;
      MenuDashboardUI.container.positionOffset_Y = 10;
      MenuDashboardUI.container.sizeOffset_X = -20;
      MenuDashboardUI.container.sizeOffset_Y = -20;
      MenuDashboardUI.container.sizeScale_X = 1f;
      MenuDashboardUI.container.sizeScale_Y = 1f;
      MenuUI.container.add(MenuDashboardUI.container);
      MenuDashboardUI.active = true;
      MenuDashboardUI.playButton = new SleekButtonIcon((Texture2D) bundle.load("Play"));
      MenuDashboardUI.playButton.positionOffset_Y = 170;
      MenuDashboardUI.playButton.sizeOffset_X = 200;
      MenuDashboardUI.playButton.sizeOffset_Y = 50;
      MenuDashboardUI.playButton.text = MenuDashboardUI.localization.format("PlayButtonText");
      MenuDashboardUI.playButton.tooltip = MenuDashboardUI.localization.format("PlayButtonTooltip");
      MenuDashboardUI.playButton.onClickedButton = new ClickedButton(MenuDashboardUI.onClickedPlayButton);
      MenuDashboardUI.playButton.fontSize = 14;
      MenuDashboardUI.container.add((Sleek) MenuDashboardUI.playButton);
      MenuDashboardUI.survivorsButton = new SleekButtonIcon((Texture2D) bundle.load("Survivors"));
      MenuDashboardUI.survivorsButton.positionOffset_Y = 230;
      MenuDashboardUI.survivorsButton.sizeOffset_X = 200;
      MenuDashboardUI.survivorsButton.sizeOffset_Y = 50;
      MenuDashboardUI.survivorsButton.text = MenuDashboardUI.localization.format("SurvivorsButtonText");
      MenuDashboardUI.survivorsButton.tooltip = MenuDashboardUI.localization.format("SurvivorsButtonTooltip");
      MenuDashboardUI.survivorsButton.onClickedButton = new ClickedButton(MenuDashboardUI.onClickedSurvivorsButton);
      MenuDashboardUI.survivorsButton.fontSize = 14;
      MenuDashboardUI.container.add((Sleek) MenuDashboardUI.survivorsButton);
      MenuDashboardUI.configurationButton = new SleekButtonIcon((Texture2D) bundle.load("Configuration"));
      MenuDashboardUI.configurationButton.positionOffset_Y = 290;
      MenuDashboardUI.configurationButton.sizeOffset_X = 200;
      MenuDashboardUI.configurationButton.sizeOffset_Y = 50;
      MenuDashboardUI.configurationButton.text = MenuDashboardUI.localization.format("ConfigurationButtonText");
      MenuDashboardUI.configurationButton.tooltip = MenuDashboardUI.localization.format("ConfigurationButtonTooltip");
      MenuDashboardUI.configurationButton.onClickedButton = new ClickedButton(MenuDashboardUI.onClickedConfigurationButton);
      MenuDashboardUI.configurationButton.fontSize = 14;
      MenuDashboardUI.container.add((Sleek) MenuDashboardUI.configurationButton);
      MenuDashboardUI.workshopButton = new SleekButtonIcon((Texture2D) bundle.load("Workshop"));
      MenuDashboardUI.workshopButton.positionOffset_Y = 350;
      MenuDashboardUI.workshopButton.sizeOffset_X = 200;
      MenuDashboardUI.workshopButton.sizeOffset_Y = 50;
      MenuDashboardUI.workshopButton.text = MenuDashboardUI.localization.format("WorkshopButtonText");
      MenuDashboardUI.workshopButton.tooltip = MenuDashboardUI.localization.format("WorkshopButtonTooltip");
      MenuDashboardUI.workshopButton.onClickedButton = new ClickedButton(MenuDashboardUI.onClickedWorkshopButton);
      MenuDashboardUI.workshopButton.fontSize = 14;
      MenuDashboardUI.container.add((Sleek) MenuDashboardUI.workshopButton);
      MenuDashboardUI.exitButton = new SleekButtonIcon((Texture2D) bundle.load("Exit"));
      MenuDashboardUI.exitButton.positionOffset_Y = -50;
      MenuDashboardUI.exitButton.positionScale_Y = 1f;
      MenuDashboardUI.exitButton.sizeOffset_X = 200;
      MenuDashboardUI.exitButton.sizeOffset_Y = 50;
      MenuDashboardUI.exitButton.text = MenuDashboardUI.localization.format("ExitButtonText");
      MenuDashboardUI.exitButton.tooltip = MenuDashboardUI.localization.format("ExitButtonTooltip");
      MenuDashboardUI.exitButton.onClickedButton = new ClickedButton(MenuDashboardUI.onClickedExitButton);
      MenuDashboardUI.exitButton.fontSize = 14;
      MenuDashboardUI.container.add((Sleek) MenuDashboardUI.exitButton);
      MenuDashboardUI.newsBox = new SleekScrollBox();
      MenuDashboardUI.newsBox.positionOffset_X = 210;
      MenuDashboardUI.newsBox.positionOffset_Y = 170;
      MenuDashboardUI.newsBox.sizeOffset_X = -210;
      MenuDashboardUI.newsBox.sizeOffset_Y = -280;
      MenuDashboardUI.newsBox.sizeScale_X = 1f;
      MenuDashboardUI.newsBox.sizeScale_Y = 1f;
      MenuDashboardUI.container.add((Sleek) MenuDashboardUI.newsBox);
      if (!Provider.isPro)
      {
        MenuDashboardUI.proButton = new SleekButton();
        MenuDashboardUI.proButton.positionOffset_X = 210;
        MenuDashboardUI.proButton.positionOffset_Y = -100;
        MenuDashboardUI.proButton.positionScale_Y = 1f;
        MenuDashboardUI.proButton.sizeOffset_Y = 100;
        MenuDashboardUI.proButton.sizeOffset_X = -210;
        MenuDashboardUI.proButton.sizeScale_X = 1f;
        MenuDashboardUI.proButton.tooltip = MenuDashboardUI.localization.format("Pro_Button_Tooltip");
        MenuDashboardUI.proButton.backgroundColor = Palette.PRO;
        MenuDashboardUI.proButton.foregroundColor = Palette.PRO;
        MenuDashboardUI.proButton.onClickedButton = new ClickedButton(MenuDashboardUI.onClickedProButton);
        MenuDashboardUI.container.add((Sleek) MenuDashboardUI.proButton);
        MenuDashboardUI.proLabel = new SleekLabel();
        MenuDashboardUI.proLabel.sizeScale_X = 1f;
        MenuDashboardUI.proLabel.sizeOffset_Y = 50;
        MenuDashboardUI.proLabel.text = MenuDashboardUI.localization.format("Pro_Title");
        MenuDashboardUI.proLabel.foregroundColor = Palette.PRO;
        MenuDashboardUI.proLabel.fontSize = 18;
        MenuDashboardUI.proButton.add((Sleek) MenuDashboardUI.proLabel);
        MenuDashboardUI.featureLabel = new SleekLabel();
        MenuDashboardUI.featureLabel.positionOffset_Y = 50;
        MenuDashboardUI.featureLabel.sizeOffset_Y = -50;
        MenuDashboardUI.featureLabel.sizeScale_X = 1f;
        MenuDashboardUI.featureLabel.sizeScale_Y = 1f;
        MenuDashboardUI.featureLabel.text = MenuDashboardUI.localization.format("Pro_Button");
        MenuDashboardUI.featureLabel.foregroundColor = Palette.PRO;
        MenuDashboardUI.proButton.add((Sleek) MenuDashboardUI.featureLabel);
      }
      else
        MenuDashboardUI.newsBox.sizeOffset_Y = -170;
      bundle.unload();
      MenuPauseUI menuPauseUi = new MenuPauseUI();
      MenuTitleUI menuTitleUi = new MenuTitleUI();
      MenuPlayUI menuPlayUi = new MenuPlayUI();
      MenuSurvivorsUI menuSurvivorsUi = new MenuSurvivorsUI();
      MenuConfigurationUI menuConfigurationUi = new MenuConfigurationUI();
      MenuWorkshopUI menuWorkshopUi = new MenuWorkshopUI();
      if (Provider.connectionFailureInfo == ESteamConnectionFailureInfo.NONE)
        return;
      ESteamConnectionFailureInfo connectionFailureInfo = Provider.connectionFailureInfo;
      string connectionFailureReason = Provider.connectionFailureReason;
      uint connectionFailureDuration = Provider.connectionFailureDuration;
      Provider.resetConnectionFailure();
      if (connectionFailureInfo == ESteamConnectionFailureInfo.BANNED)
        MenuUI.alert(MenuDashboardUI.localization.format("Banned", (object) connectionFailureDuration, (object) connectionFailureReason));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.KICKED)
        MenuUI.alert(MenuDashboardUI.localization.format("Kicked", (object) connectionFailureReason));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.WHITELISTED)
        MenuUI.alert(MenuDashboardUI.localization.format("Whitelisted"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.PASSWORD)
        MenuUI.alert(MenuDashboardUI.localization.format("Password"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.FULL)
        MenuUI.alert(MenuDashboardUI.localization.format("Full"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.HASH)
        MenuUI.alert(MenuDashboardUI.localization.format("Hash"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.VERSION)
        MenuUI.alert(MenuDashboardUI.localization.format("Version"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.PRO)
        MenuUI.alert(MenuDashboardUI.localization.format("Pro"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.AUTH_VERIFICATION)
        MenuUI.alert(MenuDashboardUI.localization.format("Auth_Verification"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.AUTH_NO_STEAM)
        MenuUI.alert(MenuDashboardUI.localization.format("Auth_No_Steam"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.AUTH_LICENSE_EXPIRED)
        MenuUI.alert(MenuDashboardUI.localization.format("Auth_License_Expired"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.AUTH_VAC_BAN)
        MenuUI.alert(MenuDashboardUI.localization.format("Auth_VAC_Ban"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.AUTH_ELSEWHERE)
        MenuUI.alert(MenuDashboardUI.localization.format("Auth_Elsewhere"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.AUTH_TIMED_OUT)
        MenuUI.alert(MenuDashboardUI.localization.format("Auth_Timed_Out"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.AUTH_USED)
        MenuUI.alert(MenuDashboardUI.localization.format("Auth_Used"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.AUTH_NO_USER)
        MenuUI.alert(MenuDashboardUI.localization.format("Auth_No_User"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.AUTH_PUB_BAN)
        MenuUI.alert(MenuDashboardUI.localization.format("Auth_Pub_Ban"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.AUTH_ECON)
        MenuUI.alert(MenuDashboardUI.localization.format("Auth_Econ"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.AUTH_EMPTY)
        MenuUI.alert(MenuDashboardUI.localization.format("Auth_Empty"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.PENDING)
        MenuUI.alert(MenuDashboardUI.localization.format("Pending"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.NAME_PLAYER_SHORT)
        MenuUI.alert(MenuDashboardUI.localization.format("Name_Player_Short"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.NAME_PLAYER_LONG)
        MenuUI.alert(MenuDashboardUI.localization.format("Name_Player_Long"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.NAME_PLAYER_INVALID)
        MenuUI.alert(MenuDashboardUI.localization.format("Name_Player_Invalid"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.NAME_PLAYER_NUMBER)
        MenuUI.alert(MenuDashboardUI.localization.format("Name_Player_Number"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.NAME_CHARACTER_SHORT)
        MenuUI.alert(MenuDashboardUI.localization.format("Name_Character_Short"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.NAME_CHARACTER_LONG)
        MenuUI.alert(MenuDashboardUI.localization.format("Name_Character_Long"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.NAME_CHARACTER_INVALID)
        MenuUI.alert(MenuDashboardUI.localization.format("Name_Character_Invalid"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.NAME_CHARACTER_NUMBER)
        MenuUI.alert(MenuDashboardUI.localization.format("Name_Character_Number"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.TIMED_OUT)
        MenuUI.alert(MenuDashboardUI.localization.format("Timed_Out"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.MAP)
        MenuUI.alert(MenuDashboardUI.localization.format("Map"));
      else if (connectionFailureInfo == ESteamConnectionFailureInfo.SHUTDOWN)
      {
        MenuUI.alert(MenuDashboardUI.localization.format("Shutdown"));
      }
      else
      {
        if (connectionFailureInfo != ESteamConnectionFailureInfo.PING)
          return;
        MenuUI.alert(MenuDashboardUI.localization.format("Ping"));
      }
    }

    public static void open()
    {
      if (MenuDashboardUI.active)
      {
        MenuDashboardUI.close();
      }
      else
      {
        MenuDashboardUI.active = true;
        MenuDashboardUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!MenuDashboardUI.active)
        return;
      MenuDashboardUI.active = false;
      MenuDashboardUI.container.lerpPositionScale(0.0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onClickedPlayButton(SleekButton button)
    {
      MenuPlayUI.open();
      MenuDashboardUI.close();
      MenuTitleUI.close();
    }

    private static void onClickedSurvivorsButton(SleekButton button)
    {
      MenuSurvivorsUI.open();
      MenuDashboardUI.close();
      MenuTitleUI.close();
    }

    private static void onClickedConfigurationButton(SleekButton button)
    {
      MenuConfigurationUI.open();
      MenuDashboardUI.close();
      MenuTitleUI.close();
    }

    private static void onClickedWorkshopButton(SleekButton button)
    {
      MenuWorkshopUI.open();
      MenuDashboardUI.close();
      MenuTitleUI.close();
    }

    private static void onClickedExitButton(SleekButton button)
    {
      MenuPauseUI.open();
      MenuDashboardUI.close();
      MenuTitleUI.close();
    }

    private static void onClickedProButton(SleekButton button)
    {
      if (!Provider.provider.storeService.canOpenStore)
        MenuUI.alert(MenuSurvivorsCharacterUI.localization.format("Overlay"));
      else
        Provider.provider.storeService.open((IStorePackageID) new SteamworksStorePackageID(Provider.PRO_ID.m_AppId));
    }

    private static void onClickedNewsButton(SleekButton button)
    {
      int index = MenuDashboardUI.newsBox.search((Sleek) button);
      if (!Provider.provider.browserService.canOpenBrowser)
        MenuUI.alert(MenuDashboardUI.localization.format("Overlay"));
      else
        Provider.provider.browserService.open(MenuDashboardUI.newsResponse.AppNews.NewsItems[index].URL);
    }

    private static void filterContent(string header, string source, ref string contents, ref int lines)
    {
      int startIndex1 = source.IndexOf("[b]" + header + ":[/b]");
      if (startIndex1 == -1)
        return;
      contents = contents + "<i>" + header + "</i>:\n";
      lines = lines + 2;
      int startIndex2 = source.IndexOf("[list]", startIndex1);
      int num1 = source.IndexOf("[/list]", startIndex2);
      string str1 = source.Substring(startIndex2 + 6, num1 - (startIndex2 + 6));
      string[] separator = new string[1]
      {
        "[*]"
      };
      int num2 = 1;
      foreach (string str2 in str1.Split(separator, (StringSplitOptions) num2))
      {
        if (str2.Length > 0)
        {
          contents = contents + str2;
          lines = lines + 1;
        }
      }
      contents = contents + "\n";
      lines = lines + 1;
    }

    private static void onWebRequestReady(IWebRequestHandle webRequestHandle)
    {
      if (webRequestHandle != MenuDashboardUI.newsRequestHandle)
        return;
      uint responseBodySize = Provider.provider.webService.getResponseBodySize(MenuDashboardUI.newsRequestHandle);
      byte[] numArray = new byte[(IntPtr) responseBodySize];
      Provider.provider.webService.getResponseBodyData(MenuDashboardUI.newsRequestHandle, numArray, responseBodySize);
      MenuDashboardUI.newsResponse = JsonConvert.DeserializeObject<NewsResponse>(Encoding.UTF8.GetString(numArray));
      int num = 0;
      for (int index = 0; index < MenuDashboardUI.newsResponse.AppNews.NewsItems.Length; ++index)
      {
        NewsItem newsItem = MenuDashboardUI.newsResponse.AppNews.NewsItems[index];
        string contents = string.Empty;
        int lines = 0;
        MenuDashboardUI.filterContent("Additions", newsItem.Contents, ref contents, ref lines);
        MenuDashboardUI.filterContent("Improvements", newsItem.Contents, ref contents, ref lines);
        MenuDashboardUI.filterContent("Updates", newsItem.Contents, ref contents, ref lines);
        MenuDashboardUI.filterContent("Tweaks", newsItem.Contents, ref contents, ref lines);
        MenuDashboardUI.filterContent("Fixes", newsItem.Contents, ref contents, ref lines);
        SleekButton sleekButton = new SleekButton();
        sleekButton.sizeScale_X = 1f;
        sleekButton.sizeOffset_X = -30;
        sleekButton.sizeOffset_Y = 80 + lines * 12;
        sleekButton.positionOffset_Y = num;
        sleekButton.tooltip = MenuDashboardUI.localization.format("NewsTooltip");
        MenuDashboardUI.newsBox.add((Sleek) sleekButton);
        sleekButton.onClickedButton = new ClickedButton(MenuDashboardUI.onClickedNewsButton);
        SleekLabel sleekLabel1 = new SleekLabel();
        sleekLabel1.sizeScale_X = 1f;
        sleekLabel1.sizeOffset_Y = 50;
        sleekLabel1.text = newsItem.Title;
        sleekLabel1.fontSize = 40;
        sleekLabel1.fontAlignment = TextAnchor.MiddleLeft;
        sleekButton.add((Sleek) sleekLabel1);
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds((double) newsItem.Date).ToLocalTime();
        SleekLabel sleekLabel2 = new SleekLabel();
        sleekLabel2.sizeScale_X = 1f;
        sleekLabel2.sizeOffset_Y = 20;
        sleekLabel2.positionOffset_Y = 50;
        sleekLabel2.text = (string) (object) dateTime + (object) " - " + newsItem.Author;
        sleekLabel2.fontAlignment = TextAnchor.MiddleLeft;
        sleekButton.add((Sleek) sleekLabel2);
        SleekLabel sleekLabel3 = new SleekLabel();
        sleekLabel3.sizeScale_X = 1f;
        sleekLabel3.positionOffset_Y = 70;
        sleekLabel3.sizeScale_Y = 1f;
        sleekLabel3.sizeOffset_Y = -70;
        sleekLabel3.text = contents;
        sleekLabel3.fontAlignment = TextAnchor.UpperLeft;
        sleekLabel3.isRich = true;
        sleekButton.add((Sleek) sleekLabel3);
        num += sleekButton.sizeOffset_Y + 10;
      }
      MenuDashboardUI.newsBox.area = new Rect(0.0f, 0.0f, 5f, (float) (num - 10));
      Provider.provider.webService.releaseRequest(MenuDashboardUI.newsRequestHandle);
    }
  }
}
