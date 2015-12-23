// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekSkill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SleekSkill : SleekButton
  {
    public SleekSkill(byte speciality, byte index, Skill skill)
    {
      this.init();
      this.fontStyle = FontStyle.Bold;
      this.fontAlignment = TextAnchor.MiddleCenter;
      this.fontSize = SleekRender.FONT_SIZE;
      this.calculateContent();
      for (byte index1 = (byte) 0; (int) index1 < (int) skill.max; ++index1)
      {
        SleekImageTexture sleekImageTexture = new SleekImageTexture();
        sleekImageTexture.positionOffset_X = -20 - (int) index1 * 20;
        sleekImageTexture.positionOffset_Y = 10;
        sleekImageTexture.positionScale_X = 1f;
        sleekImageTexture.sizeOffset_X = 10;
        sleekImageTexture.sizeOffset_Y = -10;
        sleekImageTexture.sizeScale_Y = 0.5f;
        sleekImageTexture.texture = (int) index1 >= (int) skill.level ? (Texture) PlayerDashboardSkillsUI.icons.load("Locked") : (Texture) PlayerDashboardSkillsUI.icons.load("Unlocked");
        this.add((Sleek) sleekImageTexture);
      }
      SleekLabel sleekLabel1 = new SleekLabel();
      sleekLabel1.positionOffset_X = 5;
      sleekLabel1.positionOffset_Y = 5;
      sleekLabel1.sizeOffset_X = -10;
      sleekLabel1.sizeOffset_Y = -5;
      sleekLabel1.sizeScale_X = 0.5f;
      sleekLabel1.sizeScale_Y = 0.5f;
      sleekLabel1.fontAlignment = TextAnchor.MiddleLeft;
      sleekLabel1.text = PlayerDashboardSkillsUI.localization.format("Skill", (object) PlayerDashboardSkillsUI.localization.format(string.Concat(new object[4]
      {
        (object) "Speciality_",
        (object) speciality,
        (object) "_Skill_",
        (object) index
      })), (object) PlayerDashboardSkillsUI.localization.format("Level_" + (object) skill.level));
      sleekLabel1.foregroundColor = Palette.COLOR_Y;
      sleekLabel1.fontSize = 14;
      this.add((Sleek) sleekLabel1);
      SleekLabel sleekLabel2 = new SleekLabel();
      sleekLabel2.positionOffset_X = 5;
      sleekLabel2.positionOffset_Y = 5;
      sleekLabel2.positionScale_Y = 0.5f;
      sleekLabel2.sizeOffset_X = -10;
      sleekLabel2.sizeOffset_Y = -5;
      sleekLabel2.sizeScale_X = 0.5f;
      sleekLabel2.sizeScale_Y = 0.5f;
      sleekLabel2.fontAlignment = TextAnchor.MiddleLeft;
      sleekLabel2.text = PlayerDashboardSkillsUI.localization.format("Speciality_" + (object) speciality + "_Skill_" + (string) (object) index + "_Tooltip");
      this.add((Sleek) sleekLabel2);
      if ((int) skill.level > 0)
      {
        SleekLabel sleekLabel3 = new SleekLabel();
        sleekLabel3.positionOffset_X = 5;
        sleekLabel3.positionOffset_Y = 5;
        sleekLabel3.positionScale_X = 0.25f;
        sleekLabel3.sizeOffset_X = -10;
        sleekLabel3.sizeOffset_Y = -10;
        sleekLabel3.sizeScale_X = 0.5f;
        sleekLabel3.sizeScale_Y = 0.5f;
        sleekLabel3.fontAlignment = TextAnchor.MiddleCenter;
        sleekLabel3.text = PlayerDashboardSkillsUI.localization.format("Bonus_Current", (object) PlayerDashboardSkillsUI.localization.format("Speciality_" + (object) speciality + "_Skill_" + (string) (object) index + "_Level_" + (string) (object) skill.level));
        sleekLabel3.foregroundColor = Palette.COLOR_G;
        this.add((Sleek) sleekLabel3);
      }
      if ((int) skill.level < (int) skill.max)
      {
        SleekLabel sleekLabel3 = new SleekLabel();
        sleekLabel3.positionOffset_X = 5;
        sleekLabel3.positionOffset_Y = 5;
        sleekLabel3.positionScale_X = 0.25f;
        sleekLabel3.positionScale_Y = 0.5f;
        sleekLabel3.sizeOffset_X = -10;
        sleekLabel3.sizeOffset_Y = -10;
        sleekLabel3.sizeScale_X = 0.5f;
        sleekLabel3.sizeScale_Y = 0.5f;
        sleekLabel3.fontAlignment = TextAnchor.MiddleCenter;
        sleekLabel3.text = PlayerDashboardSkillsUI.localization.format("Bonus_Next", (object) PlayerDashboardSkillsUI.localization.format("Speciality_" + (object) speciality + "_Skill_" + (string) (object) index + "_Level_" + (string) (object) ((int) skill.level + 1)));
        sleekLabel3.foregroundColor = Palette.COLOR_G;
        this.add((Sleek) sleekLabel3);
      }
      SleekLabel sleekLabel4 = new SleekLabel();
      sleekLabel4.positionOffset_X = 5;
      sleekLabel4.positionOffset_Y = 5;
      sleekLabel4.positionScale_X = 0.5f;
      sleekLabel4.positionScale_Y = 0.5f;
      sleekLabel4.sizeOffset_X = -10;
      sleekLabel4.sizeOffset_Y = -10;
      sleekLabel4.sizeScale_X = 0.5f;
      sleekLabel4.sizeScale_Y = 0.5f;
      sleekLabel4.fontAlignment = TextAnchor.MiddleRight;
      if ((int) skill.level < (int) skill.max)
        sleekLabel4.text = PlayerDashboardSkillsUI.localization.format("Cost", (object) Player.player.skills.cost((int) speciality, (int) index));
      else
        sleekLabel4.text = PlayerDashboardSkillsUI.localization.format("Full");
      this.add((Sleek) sleekLabel4);
      this.tooltip = PlayerDashboardSkillsUI.localization.format("Speciality_" + (object) speciality + "_Skill_" + (string) (object) index + "_Tooltip");
    }
  }
}
