// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerDashboardSkillsUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class PlayerDashboardSkillsUI
  {
    public static Local localization;
    public static Bundle icons;
    private static Sleek container;
    public static bool active;
    private static SleekBox backdropBox;
    private static Skill[] skills;
    private static SleekScrollBox skillsScrollBox;
    private static SleekButton boostButton;
    private static SleekBox experienceBox;
    private static byte selectedSpeciality;

    public PlayerDashboardSkillsUI()
    {
      if (PlayerDashboardSkillsUI.icons != null)
        PlayerDashboardSkillsUI.icons.unload();
      PlayerDashboardSkillsUI.localization = Localization.read("/Player/PlayerDashboardSkills.dat");
      PlayerDashboardSkillsUI.icons = Bundles.getBundle("/Bundles/Textures/Player/Icons/PlayerDashboardSkills/PlayerDashboardSkills.unity3d");
      PlayerDashboardSkillsUI.container = new Sleek();
      PlayerDashboardSkillsUI.container.positionScale_Y = 1f;
      PlayerDashboardSkillsUI.container.positionOffset_X = 10;
      PlayerDashboardSkillsUI.container.positionOffset_Y = 10;
      PlayerDashboardSkillsUI.container.sizeOffset_X = -20;
      PlayerDashboardSkillsUI.container.sizeOffset_Y = -20;
      PlayerDashboardSkillsUI.container.sizeScale_X = 1f;
      PlayerDashboardSkillsUI.container.sizeScale_Y = 1f;
      PlayerUI.container.add(PlayerDashboardSkillsUI.container);
      PlayerDashboardSkillsUI.active = false;
      PlayerDashboardSkillsUI.selectedSpeciality = byte.MaxValue;
      PlayerDashboardSkillsUI.backdropBox = new SleekBox();
      PlayerDashboardSkillsUI.backdropBox.positionOffset_Y = 60;
      PlayerDashboardSkillsUI.backdropBox.sizeOffset_Y = -60;
      PlayerDashboardSkillsUI.backdropBox.sizeScale_X = 1f;
      PlayerDashboardSkillsUI.backdropBox.sizeScale_Y = 1f;
      PlayerDashboardSkillsUI.backdropBox.backgroundColor = Palette.COLOR_W;
      PlayerDashboardSkillsUI.backdropBox.backgroundColor.a = 0.5f;
      PlayerDashboardSkillsUI.container.add((Sleek) PlayerDashboardSkillsUI.backdropBox);
      PlayerDashboardSkillsUI.experienceBox = new SleekBox();
      PlayerDashboardSkillsUI.experienceBox.positionOffset_X = 10;
      PlayerDashboardSkillsUI.experienceBox.positionOffset_Y = -90;
      PlayerDashboardSkillsUI.experienceBox.positionScale_Y = 1f;
      PlayerDashboardSkillsUI.experienceBox.sizeOffset_X = -15;
      PlayerDashboardSkillsUI.experienceBox.sizeOffset_Y = 80;
      PlayerDashboardSkillsUI.experienceBox.sizeScale_X = 0.5f;
      PlayerDashboardSkillsUI.experienceBox.fontSize = 14;
      PlayerDashboardSkillsUI.backdropBox.add((Sleek) PlayerDashboardSkillsUI.experienceBox);
      for (int index = 0; index < (int) PlayerSkills.SPECIALITIES; ++index)
      {
        SleekButtonIcon sleekButtonIcon = new SleekButtonIcon((Texture2D) PlayerDashboardSkillsUI.icons.load("Speciality_" + (object) index));
        sleekButtonIcon.positionOffset_X = index * 60 - 85;
        sleekButtonIcon.positionOffset_Y = 10;
        sleekButtonIcon.positionScale_X = 0.5f;
        sleekButtonIcon.sizeOffset_X = 50;
        sleekButtonIcon.sizeOffset_Y = 50;
        sleekButtonIcon.tooltip = PlayerDashboardSkillsUI.localization.format("Speciality_" + (object) index + "_Tooltip");
        sleekButtonIcon.onClickedButton = new ClickedButton(PlayerDashboardSkillsUI.onClickedSpecialityButton);
        PlayerDashboardSkillsUI.backdropBox.add((Sleek) sleekButtonIcon);
      }
      PlayerDashboardSkillsUI.skillsScrollBox = new SleekScrollBox();
      PlayerDashboardSkillsUI.skillsScrollBox.positionOffset_X = 10;
      PlayerDashboardSkillsUI.skillsScrollBox.positionOffset_Y = 70;
      PlayerDashboardSkillsUI.skillsScrollBox.sizeOffset_X = -20;
      PlayerDashboardSkillsUI.skillsScrollBox.sizeOffset_Y = -170;
      PlayerDashboardSkillsUI.skillsScrollBox.sizeScale_X = 1f;
      PlayerDashboardSkillsUI.skillsScrollBox.sizeScale_Y = 1f;
      PlayerDashboardSkillsUI.backdropBox.add((Sleek) PlayerDashboardSkillsUI.skillsScrollBox);
      PlayerDashboardSkillsUI.updateSelection((byte) 0);
      Player.player.skills.onExperienceUpdated += new ExperienceUpdated(PlayerDashboardSkillsUI.onExperienceUpdated);
      Player.player.skills.onBoostUpdated += new BoostUpdated(PlayerDashboardSkillsUI.onBoostUpdated);
      Player.player.skills.onSkillsUpdated += new SkillsUpdated(PlayerDashboardSkillsUI.onSkillsUpdated);
    }

    public static void open()
    {
      if (PlayerDashboardSkillsUI.active)
      {
        PlayerDashboardSkillsUI.close();
      }
      else
      {
        PlayerDashboardSkillsUI.active = true;
        PlayerDashboardSkillsUI.updateSelection(PlayerDashboardSkillsUI.selectedSpeciality);
        PlayerDashboardSkillsUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!PlayerDashboardSkillsUI.active)
        return;
      PlayerDashboardSkillsUI.active = false;
      PlayerDashboardSkillsUI.container.lerpPositionScale(0.0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void updateSelection(byte specialityIndex)
    {
      PlayerDashboardSkillsUI.skills = Player.player.skills.skills[(int) specialityIndex];
      PlayerDashboardSkillsUI.skillsScrollBox.remove();
      PlayerDashboardSkillsUI.skillsScrollBox.area = new Rect(0.0f, 0.0f, 5f, (float) (PlayerDashboardSkillsUI.skills.Length * 90 - 10));
      for (byte index = (byte) 0; (int) index < PlayerDashboardSkillsUI.skills.Length; ++index)
      {
        Skill skill = PlayerDashboardSkillsUI.skills[(int) index];
        SleekSkill sleekSkill = new SleekSkill(specialityIndex, index, skill);
        sleekSkill.positionOffset_Y = (int) index * 90;
        sleekSkill.sizeOffset_X = -30;
        sleekSkill.sizeOffset_Y = 80;
        sleekSkill.sizeScale_X = 1f;
        sleekSkill.onClickedButton = new ClickedButton(PlayerDashboardSkillsUI.onClickedSkillButton);
        PlayerDashboardSkillsUI.skillsScrollBox.add((Sleek) sleekSkill);
      }
      if (PlayerDashboardSkillsUI.boostButton != null)
        PlayerDashboardSkillsUI.backdropBox.remove((Sleek) PlayerDashboardSkillsUI.boostButton);
      PlayerDashboardSkillsUI.boostButton = (SleekButton) new SleekBoost((byte) Player.player.skills.boost);
      PlayerDashboardSkillsUI.boostButton.positionOffset_X = 5;
      PlayerDashboardSkillsUI.boostButton.positionOffset_Y = -90;
      PlayerDashboardSkillsUI.boostButton.positionScale_X = 0.5f;
      PlayerDashboardSkillsUI.boostButton.positionScale_Y = 1f;
      PlayerDashboardSkillsUI.boostButton.sizeOffset_X = -15;
      PlayerDashboardSkillsUI.boostButton.sizeOffset_Y = 80;
      PlayerDashboardSkillsUI.boostButton.sizeScale_X = 0.5f;
      PlayerDashboardSkillsUI.boostButton.onClickedButton = new ClickedButton(PlayerDashboardSkillsUI.onClickedBoostButton);
      PlayerDashboardSkillsUI.backdropBox.add((Sleek) PlayerDashboardSkillsUI.boostButton);
      PlayerDashboardSkillsUI.selectedSpeciality = specialityIndex;
    }

    private static void onClickedSpecialityButton(SleekButton button)
    {
      PlayerDashboardSkillsUI.updateSelection((byte) ((button.positionOffset_X + 85) / 60));
    }

    private static void onClickedBoostButton(SleekButton button)
    {
      if (Player.player.skills.experience < PlayerSkills.BOOST_COST)
        return;
      Player.player.skills.sendBoost();
    }

    private static void onClickedSkillButton(SleekButton button)
    {
      byte index = (byte) (button.positionOffset_Y / 90);
      if ((int) PlayerDashboardSkillsUI.skills[(int) index].level >= (int) PlayerDashboardSkillsUI.skills[(int) index].max || Player.player.skills.experience < Player.player.skills.cost((int) PlayerDashboardSkillsUI.selectedSpeciality, (int) index))
        return;
      Player.player.skills.sendUpgrade(PlayerDashboardSkillsUI.selectedSpeciality, index);
    }

    private static void onExperienceUpdated(uint newExperience)
    {
      PlayerDashboardSkillsUI.experienceBox.text = PlayerDashboardSkillsUI.localization.format("Experience", (object) newExperience.ToString());
    }

    private static void onBoostUpdated(EPlayerBoost newBoost)
    {
      PlayerDashboardSkillsUI.updateSelection(PlayerDashboardSkillsUI.selectedSpeciality);
    }

    private static void onSkillsUpdated()
    {
      PlayerDashboardSkillsUI.updateSelection(PlayerDashboardSkillsUI.selectedSpeciality);
    }
  }
}
