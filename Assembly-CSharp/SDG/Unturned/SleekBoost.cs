// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekBoost
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SleekBoost : SleekButton
  {
    private SleekLabel infoLabel;
    private SleekLabel descriptionLabel;
    private SleekLabel costLabel;

    public SleekBoost(byte boost)
    {
      this.init();
      this.fontStyle = FontStyle.Bold;
      this.fontAlignment = TextAnchor.MiddleCenter;
      this.fontSize = SleekRender.FONT_SIZE;
      this.calculateContent();
      this.infoLabel = new SleekLabel();
      this.infoLabel.positionOffset_X = 5;
      this.infoLabel.positionOffset_Y = 5;
      this.infoLabel.sizeOffset_X = -10;
      this.infoLabel.sizeOffset_Y = -5;
      this.infoLabel.sizeScale_X = 0.5f;
      this.infoLabel.sizeScale_Y = 0.5f;
      this.infoLabel.fontAlignment = TextAnchor.MiddleLeft;
      this.infoLabel.text = PlayerDashboardSkillsUI.localization.format("Boost_" + (object) boost);
      this.infoLabel.foregroundColor = Palette.COLOR_Y;
      this.infoLabel.fontSize = 14;
      this.add((Sleek) this.infoLabel);
      this.descriptionLabel = new SleekLabel();
      this.descriptionLabel.positionOffset_X = 5;
      this.descriptionLabel.positionOffset_Y = 5;
      this.descriptionLabel.positionScale_Y = 0.5f;
      this.descriptionLabel.sizeOffset_X = -10;
      this.descriptionLabel.sizeOffset_Y = -5;
      this.descriptionLabel.sizeScale_X = 0.5f;
      this.descriptionLabel.sizeScale_Y = 0.5f;
      this.descriptionLabel.fontAlignment = TextAnchor.MiddleLeft;
      this.descriptionLabel.text = PlayerDashboardSkillsUI.localization.format("Boost_" + (object) boost + "_Tooltip");
      this.add((Sleek) this.descriptionLabel);
      if ((int) boost > 0)
      {
        SleekLabel sleekLabel = new SleekLabel();
        sleekLabel.positionOffset_X = 5;
        sleekLabel.positionOffset_Y = 5;
        sleekLabel.positionScale_X = 0.25f;
        sleekLabel.sizeOffset_X = -10;
        sleekLabel.sizeOffset_Y = -10;
        sleekLabel.sizeScale_X = 0.5f;
        sleekLabel.sizeScale_Y = 1f;
        sleekLabel.fontAlignment = TextAnchor.MiddleCenter;
        sleekLabel.text = PlayerDashboardSkillsUI.localization.format("Boost_" + (object) boost + "_Bonus");
        sleekLabel.foregroundColor = Palette.COLOR_G;
        this.add((Sleek) sleekLabel);
      }
      this.costLabel = new SleekLabel();
      this.costLabel.positionOffset_X = 5;
      this.costLabel.positionOffset_Y = 5;
      this.costLabel.positionScale_X = 0.5f;
      this.costLabel.sizeOffset_X = -10;
      this.costLabel.sizeOffset_Y = -10;
      this.costLabel.sizeScale_X = 0.5f;
      this.costLabel.sizeScale_Y = 1f;
      this.costLabel.fontAlignment = TextAnchor.MiddleRight;
      this.costLabel.text = PlayerDashboardSkillsUI.localization.format("Cost", (object) PlayerSkills.BOOST_COST);
      this.add((Sleek) this.costLabel);
      this.tooltip = PlayerDashboardSkillsUI.localization.format("Boost_" + (object) boost + "_Tooltip");
    }
  }
}
