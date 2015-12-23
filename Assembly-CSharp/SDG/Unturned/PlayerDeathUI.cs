// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerDeathUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class PlayerDeathUI
  {
    private static Sleek container;
    public static Local localization;
    public static bool active;
    private static SleekBox causeBox;
    public static SleekButtonIcon homeButton;
    public static SleekButtonIcon respawnButton;

    public PlayerDeathUI()
    {
      PlayerDeathUI.localization = Localization.read("/Player/PlayerDeath.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Player/Icons/PlayerDeath/PlayerDeath.unity3d");
      PlayerDeathUI.container = new Sleek();
      PlayerDeathUI.container.positionScale_Y = 1f;
      PlayerDeathUI.container.positionOffset_X = 10;
      PlayerDeathUI.container.positionOffset_Y = 10;
      PlayerDeathUI.container.sizeOffset_X = -20;
      PlayerDeathUI.container.sizeOffset_Y = -20;
      PlayerDeathUI.container.sizeScale_X = 1f;
      PlayerDeathUI.container.sizeScale_Y = 1f;
      PlayerUI.container.add(PlayerDeathUI.container);
      PlayerDeathUI.active = false;
      PlayerDeathUI.causeBox = new SleekBox();
      PlayerDeathUI.causeBox.positionOffset_Y = -25;
      PlayerDeathUI.causeBox.positionScale_Y = 0.8f;
      PlayerDeathUI.causeBox.sizeOffset_Y = 50;
      PlayerDeathUI.causeBox.sizeScale_X = 1f;
      PlayerDeathUI.container.add((Sleek) PlayerDeathUI.causeBox);
      PlayerDeathUI.homeButton = new SleekButtonIcon((Texture2D) bundle.load("Home"));
      PlayerDeathUI.homeButton.positionOffset_X = -205;
      PlayerDeathUI.homeButton.positionOffset_Y = 35;
      PlayerDeathUI.homeButton.positionScale_X = 0.5f;
      PlayerDeathUI.homeButton.positionScale_Y = 0.8f;
      PlayerDeathUI.homeButton.sizeOffset_X = 200;
      PlayerDeathUI.homeButton.sizeOffset_Y = 30;
      PlayerDeathUI.homeButton.text = PlayerDeathUI.localization.format("Home_Button");
      PlayerDeathUI.homeButton.tooltip = PlayerDeathUI.localization.format("Home_Button_Tooltip");
      PlayerDeathUI.homeButton.onClickedButton = new ClickedButton(PlayerDeathUI.onClickedHomeButton);
      PlayerDeathUI.container.add((Sleek) PlayerDeathUI.homeButton);
      PlayerDeathUI.respawnButton = new SleekButtonIcon((Texture2D) bundle.load("Respawn"));
      PlayerDeathUI.respawnButton.positionOffset_X = 5;
      PlayerDeathUI.respawnButton.positionOffset_Y = 35;
      PlayerDeathUI.respawnButton.positionScale_X = 0.5f;
      PlayerDeathUI.respawnButton.positionScale_Y = 0.8f;
      PlayerDeathUI.respawnButton.sizeOffset_X = 200;
      PlayerDeathUI.respawnButton.sizeOffset_Y = 30;
      PlayerDeathUI.respawnButton.text = PlayerDeathUI.localization.format("Respawn_Button");
      PlayerDeathUI.respawnButton.tooltip = PlayerDeathUI.localization.format("Respawn_Button_Tooltip");
      PlayerDeathUI.respawnButton.onClickedButton = new ClickedButton(PlayerDeathUI.onClickedRespawnButton);
      PlayerDeathUI.container.add((Sleek) PlayerDeathUI.respawnButton);
      bundle.unload();
    }

    public static void open()
    {
      if (PlayerDeathUI.active)
      {
        PlayerDeathUI.close();
      }
      else
      {
        PlayerDeathUI.active = true;
        PlayerLifeUI.close();
        if (PlayerLife.deathCause == EDeathCause.BLEEDING)
          PlayerDeathUI.causeBox.text = PlayerDeathUI.localization.format("Bleeding");
        else if (PlayerLife.deathCause == EDeathCause.BONES)
          PlayerDeathUI.causeBox.text = PlayerDeathUI.localization.format("Bones");
        else if (PlayerLife.deathCause == EDeathCause.FREEZING)
          PlayerDeathUI.causeBox.text = PlayerDeathUI.localization.format("Freezing");
        else if (PlayerLife.deathCause == EDeathCause.BURNING)
          PlayerDeathUI.causeBox.text = PlayerDeathUI.localization.format("Burning");
        else if (PlayerLife.deathCause == EDeathCause.FOOD)
          PlayerDeathUI.causeBox.text = PlayerDeathUI.localization.format("Food");
        else if (PlayerLife.deathCause == EDeathCause.WATER)
          PlayerDeathUI.causeBox.text = PlayerDeathUI.localization.format("Water");
        else if (PlayerLife.deathCause == EDeathCause.GUN || PlayerLife.deathCause == EDeathCause.MELEE || (PlayerLife.deathCause == EDeathCause.PUNCH || PlayerLife.deathCause == EDeathCause.ROADKILL))
        {
          SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(PlayerLife.deathKiller);
          string str1;
          string str2;
          if (steamPlayer != null)
          {
            str1 = steamPlayer.playerID.characterName;
            str2 = steamPlayer.playerID.playerName;
          }
          else
          {
            str1 = "?";
            str2 = "?";
          }
          string str3 = string.Empty;
          if (PlayerLife.deathLimb == ELimb.LEFT_FOOT || PlayerLife.deathLimb == ELimb.LEFT_LEG || (PlayerLife.deathLimb == ELimb.RIGHT_FOOT || PlayerLife.deathLimb == ELimb.RIGHT_LEG))
            str3 = PlayerDeathUI.localization.format("Leg");
          else if (PlayerLife.deathLimb == ELimb.LEFT_HAND || PlayerLife.deathLimb == ELimb.LEFT_ARM || (PlayerLife.deathLimb == ELimb.RIGHT_HAND || PlayerLife.deathLimb == ELimb.RIGHT_ARM))
            str3 = PlayerDeathUI.localization.format("Arm");
          else if (PlayerLife.deathLimb == ELimb.SPINE)
            str3 = PlayerDeathUI.localization.format("Spine");
          else if (PlayerLife.deathLimb == ELimb.SKULL)
            str3 = PlayerDeathUI.localization.format("Skull");
          if (PlayerLife.deathCause == EDeathCause.GUN)
            PlayerDeathUI.causeBox.text = PlayerDeathUI.localization.format("Gun", (object) str3, (object) str1, (object) str2);
          else if (PlayerLife.deathCause == EDeathCause.MELEE)
            PlayerDeathUI.causeBox.text = PlayerDeathUI.localization.format("Melee", (object) str3, (object) str1, (object) str2);
          else if (PlayerLife.deathCause == EDeathCause.PUNCH)
            PlayerDeathUI.causeBox.text = PlayerDeathUI.localization.format("Punch", (object) str3, (object) str1, (object) str2);
          else if (PlayerLife.deathCause == EDeathCause.ROADKILL)
            PlayerDeathUI.causeBox.text = PlayerDeathUI.localization.format("Roadkill", (object) str1, (object) str2);
        }
        else if (PlayerLife.deathCause == EDeathCause.ZOMBIE)
          PlayerDeathUI.causeBox.text = PlayerDeathUI.localization.format("Zombie");
        else if (PlayerLife.deathCause == EDeathCause.ANIMAL)
          PlayerDeathUI.causeBox.text = PlayerDeathUI.localization.format("Animal");
        else if (PlayerLife.deathCause == EDeathCause.SUICIDE)
          PlayerDeathUI.causeBox.text = PlayerDeathUI.localization.format("Suicide");
        else if (PlayerLife.deathCause == EDeathCause.KILL)
          PlayerDeathUI.causeBox.text = PlayerDeathUI.localization.format("Kill");
        else if (PlayerLife.deathCause == EDeathCause.INFECTION)
          PlayerDeathUI.causeBox.text = PlayerDeathUI.localization.format("Infection");
        else if (PlayerLife.deathCause == EDeathCause.BREATH)
          PlayerDeathUI.causeBox.text = PlayerDeathUI.localization.format("Breath");
        else if (PlayerLife.deathCause == EDeathCause.ZOMBIE)
          PlayerDeathUI.causeBox.text = PlayerDeathUI.localization.format("Zombie");
        else if (PlayerLife.deathCause == EDeathCause.VEHICLE)
          PlayerDeathUI.causeBox.text = PlayerDeathUI.localization.format("Vehicle");
        else if (PlayerLife.deathCause == EDeathCause.GRENADE)
          PlayerDeathUI.causeBox.text = PlayerDeathUI.localization.format("Grenade");
        else if (PlayerLife.deathCause == EDeathCause.SHRED)
          PlayerDeathUI.causeBox.text = PlayerDeathUI.localization.format("Shred");
        else if (PlayerLife.deathCause == EDeathCause.LANDMINE)
          PlayerDeathUI.causeBox.text = PlayerDeathUI.localization.format("Landmine");
        if (PlayerLife.deathCause != EDeathCause.SUICIDE && OptionsSettings.music)
          Camera.main.GetComponent<AudioSource>().enabled = true;
        PlayerDeathUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!PlayerDeathUI.active)
        return;
      PlayerDeathUI.active = false;
      PlayerLifeUI.open();
      if (OptionsSettings.music)
        Camera.main.GetComponent<AudioSource>().enabled = false;
      PlayerDeathUI.container.lerpPositionScale(0.0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onClickedHomeButton(SleekButton button)
    {
      if (!Provider.isServer && Provider.isPvP)
      {
        if ((double) Time.realtimeSinceStartup - (double) Player.player.life.lastDeath < (double) PlayerLife.TIMER_HOME)
          return;
      }
      else if ((double) Time.realtimeSinceStartup - (double) Player.player.life.lastRespawn < (double) PlayerLife.TIMER_RESPAWN)
        return;
      Player.player.life.sendRespawn(true);
    }

    private static void onClickedRespawnButton(SleekButton button)
    {
      if ((double) Time.realtimeSinceStartup - (double) Player.player.life.lastRespawn < (double) PlayerLife.TIMER_RESPAWN)
        return;
      Player.player.life.sendRespawn(false);
    }
  }
}
