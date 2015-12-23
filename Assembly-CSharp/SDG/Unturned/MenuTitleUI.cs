// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MenuTitleUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class MenuTitleUI
  {
    private static readonly byte STAT_COUNT = (byte) 7;
    private static Local localization;
    private static Sleek container;
    public static bool active;
    private static SleekBox titleBox;
    private static SleekLabel titleLabel;
    private static SleekLabel authorLabel;
    private static SleekButton statButton;
    private static EPlayerStat stat;

    public MenuTitleUI()
    {
      MenuTitleUI.localization = Localization.read("/Menu/MenuTitle.dat");
      MenuTitleUI.container = new Sleek();
      MenuTitleUI.container.positionOffset_X = 10;
      MenuTitleUI.container.positionOffset_Y = 10;
      MenuTitleUI.container.sizeOffset_X = -20;
      MenuTitleUI.container.sizeOffset_Y = -20;
      MenuTitleUI.container.sizeScale_X = 1f;
      MenuTitleUI.container.sizeScale_Y = 1f;
      MenuUI.container.add(MenuTitleUI.container);
      MenuTitleUI.active = true;
      MenuTitleUI.titleBox = new SleekBox();
      MenuTitleUI.titleBox.sizeOffset_Y = 100;
      MenuTitleUI.titleBox.sizeScale_X = 1f;
      MenuTitleUI.container.add((Sleek) MenuTitleUI.titleBox);
      MenuTitleUI.titleLabel = new SleekLabel();
      MenuTitleUI.titleLabel.sizeScale_X = 1f;
      MenuTitleUI.titleLabel.sizeOffset_Y = 60;
      MenuTitleUI.titleLabel.fontSize = 40;
      MenuTitleUI.titleLabel.text = Provider.APP_NAME + " " + Provider.APP_VERSION;
      MenuTitleUI.titleBox.add((Sleek) MenuTitleUI.titleLabel);
      MenuTitleUI.authorLabel = new SleekLabel();
      MenuTitleUI.authorLabel.positionOffset_Y = 60;
      MenuTitleUI.authorLabel.sizeScale_X = 1f;
      MenuTitleUI.authorLabel.sizeOffset_Y = 40;
      MenuTitleUI.authorLabel.text = MenuTitleUI.localization.format("Author_Label", (object) Provider.APP_AUTHOR);
      MenuTitleUI.titleBox.add((Sleek) MenuTitleUI.authorLabel);
      MenuTitleUI.statButton = new SleekButton();
      MenuTitleUI.statButton.positionOffset_Y = 110;
      MenuTitleUI.statButton.sizeOffset_Y = 50;
      MenuTitleUI.statButton.sizeScale_X = 1f;
      MenuTitleUI.statButton.onClickedButton = new ClickedButton(MenuTitleUI.onClickedStatButton);
      MenuTitleUI.container.add((Sleek) MenuTitleUI.statButton);
      MenuTitleUI.stat = EPlayerStat.NONE;
      MenuTitleUI.onClickedStatButton(MenuTitleUI.statButton);
      string pchName;
      bool has;
      if (!SteamApps.GetCurrentBetaName(out pchName, 1024) || !(pchName != "public") || (!Provider.provider.achievementsService.getAchievement("Preview", out has) || has))
        return;
      Provider.provider.achievementsService.setAchievement("Preview");
    }

    public static void open()
    {
      if (MenuTitleUI.active)
      {
        MenuTitleUI.close();
      }
      else
      {
        MenuTitleUI.active = true;
        MenuTitleUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!MenuTitleUI.active)
        return;
      MenuTitleUI.active = false;
      MenuTitleUI.container.lerpPositionScale(0.0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onClickedStatButton(SleekButton button)
    {
      byte num;
      do
      {
        num = (byte) Random.Range(1, (int) MenuTitleUI.STAT_COUNT + 1);
      }
      while ((int) num == (int) (byte) MenuTitleUI.stat);
      MenuTitleUI.stat = (EPlayerStat) num;
      if (MenuTitleUI.stat == EPlayerStat.KILLS_ZOMBIES_NORMAL)
      {
        int data1;
        Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Zombies_Normal", out data1);
        long data2;
        Provider.provider.statisticsService.globalStatisticsService.getStatistic("Kills_Zombies_Normal", out data2);
        MenuTitleUI.statButton.text = MenuTitleUI.localization.format("Stat_Kills_Zombies_Normal", (object) data1.ToString("n0"), (object) data2.ToString("n0"));
      }
      else if (MenuTitleUI.stat == EPlayerStat.KILLS_PLAYERS)
      {
        int data1;
        Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Players", out data1);
        long data2;
        Provider.provider.statisticsService.globalStatisticsService.getStatistic("Kills_Players", out data2);
        MenuTitleUI.statButton.text = MenuTitleUI.localization.format("Stat_Kills_Players", (object) data1.ToString("n0"), (object) data2.ToString("n0"));
      }
      else if (MenuTitleUI.stat == EPlayerStat.FOUND_ITEMS)
      {
        int data1;
        Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Items", out data1);
        long data2;
        Provider.provider.statisticsService.globalStatisticsService.getStatistic("Found_Items", out data2);
        MenuTitleUI.statButton.text = MenuTitleUI.localization.format("Stat_Found_Items", (object) data1.ToString("n0"), (object) data2.ToString("n0"));
      }
      else if (MenuTitleUI.stat == EPlayerStat.FOUND_RESOURCES)
      {
        int data1;
        Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Resources", out data1);
        long data2;
        Provider.provider.statisticsService.globalStatisticsService.getStatistic("Found_Resources", out data2);
        MenuTitleUI.statButton.text = MenuTitleUI.localization.format("Stat_Found_Resources", (object) data1.ToString("n0"), (object) data2.ToString("n0"));
      }
      else if (MenuTitleUI.stat == EPlayerStat.FOUND_EXPERIENCE)
      {
        int data1;
        Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Experience", out data1);
        long data2;
        Provider.provider.statisticsService.globalStatisticsService.getStatistic("Found_Experience", out data2);
        MenuTitleUI.statButton.text = MenuTitleUI.localization.format("Stat_Found_Experience", (object) data1.ToString("n0"), (object) data2.ToString("n0"));
      }
      else if (MenuTitleUI.stat == EPlayerStat.KILLS_ZOMBIES_MEGA)
      {
        int data1;
        Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Zombies_Mega", out data1);
        long data2;
        Provider.provider.statisticsService.globalStatisticsService.getStatistic("Kills_Zombies_Mega", out data2);
        MenuTitleUI.statButton.text = MenuTitleUI.localization.format("Stat_Kills_Zombies_Mega", (object) data1.ToString("n0"), (object) data2.ToString("n0"));
      }
      else if (MenuTitleUI.stat == EPlayerStat.DEATHS_PLAYERS)
      {
        int data1;
        Provider.provider.statisticsService.userStatisticsService.getStatistic("Deaths_Players", out data1);
        long data2;
        Provider.provider.statisticsService.globalStatisticsService.getStatistic("Deaths_Players", out data2);
        MenuTitleUI.statButton.text = MenuTitleUI.localization.format("Stat_Deaths_Players", (object) data1.ToString("n0"), (object) data2.ToString("n0"));
      }
      else
      {
        if (MenuTitleUI.stat != EPlayerStat.KILLS_ANIMALS)
          return;
        int data1;
        Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Animals", out data1);
        long data2;
        Provider.provider.statisticsService.globalStatisticsService.getStatistic("Kills_Animals", out data2);
        MenuTitleUI.statButton.text = MenuTitleUI.localization.format("Stat_Kills_Animals", (object) data1.ToString("n0"), (object) data2.ToString("n0"));
      }
    }
  }
}
