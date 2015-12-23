// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekLevel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SleekLevel : Sleek
  {
    private SleekButton button;
    private SleekLabel nameLabel;
    private SleekLabel infoLabel;
    public ClickedLevel onClickedLevel;

    public SleekLevel(LevelInfo level, bool isEditor)
    {
      this.init();
      this.sizeOffset_X = 400;
      this.sizeOffset_Y = 100;
      this.button = new SleekButton();
      this.button.sizeOffset_X = 0;
      this.button.sizeOffset_Y = 0;
      this.button.sizeScale_X = 1f;
      this.button.sizeScale_Y = 1f;
      if (level.isEditable || !isEditor)
        this.button.onClickedButton = new ClickedButton(this.onClickedButton);
      this.add((Sleek) this.button);
      if (ReadWrite.fileExists("/Maps/" + level.name + "/Icon.png", false))
      {
        byte[] data = ReadWrite.readBytes("/Maps/" + level.name + "/Icon.png", false);
        Texture2D texture2D = new Texture2D(380, 80, TextureFormat.ARGB32, false, true);
        texture2D.name = "Texture";
        texture2D.hideFlags = HideFlags.HideAndDontSave;
        texture2D.LoadImage(data);
        SleekImageTexture sleekImageTexture = new SleekImageTexture();
        sleekImageTexture.positionOffset_X = 10;
        sleekImageTexture.positionOffset_Y = 10;
        sleekImageTexture.sizeOffset_X = -20;
        sleekImageTexture.sizeOffset_Y = -20;
        sleekImageTexture.sizeScale_X = 1f;
        sleekImageTexture.sizeScale_Y = 1f;
        sleekImageTexture.texture = (Texture) texture2D;
        this.button.add((Sleek) sleekImageTexture);
      }
      this.nameLabel = new SleekLabel();
      this.nameLabel.positionOffset_Y = 10;
      this.nameLabel.sizeScale_X = 1f;
      this.nameLabel.sizeOffset_Y = 50;
      this.nameLabel.fontAlignment = TextAnchor.MiddleCenter;
      this.nameLabel.text = level.name;
      this.nameLabel.fontSize = 14;
      this.button.add((Sleek) this.nameLabel);
      this.infoLabel = new SleekLabel();
      this.infoLabel.positionOffset_Y = 60;
      this.infoLabel.sizeScale_X = 1f;
      this.infoLabel.sizeOffset_Y = 30;
      this.infoLabel.fontAlignment = TextAnchor.MiddleCenter;
      string str1 = "#SIZE";
      if (level.size == ELevelSize.TINY)
        str1 = MenuPlaySingleplayerUI.localization.format("Tiny");
      else if (level.size == ELevelSize.SMALL)
        str1 = MenuPlaySingleplayerUI.localization.format("Small");
      else if (level.size == ELevelSize.MEDIUM)
        str1 = MenuPlaySingleplayerUI.localization.format("Medium");
      else if (level.size == ELevelSize.LARGE)
        str1 = MenuPlaySingleplayerUI.localization.format("Large");
      else if (level.size == ELevelSize.INSANE)
        str1 = MenuPlaySingleplayerUI.localization.format("Insane");
      string str2 = "#TYPE";
      if (level.type == ELevelType.SURVIVAL)
        str2 = MenuPlaySingleplayerUI.localization.format("Survival");
      else if (level.type == ELevelType.HORDE)
        str2 = MenuPlaySingleplayerUI.localization.format("Horde");
      this.infoLabel.text = MenuPlaySingleplayerUI.localization.format("Info", (object) str1, (object) str2);
      this.button.add((Sleek) this.infoLabel);
      if (level.isEditable || !isEditor)
        return;
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Workshop/MenuWorkshopEditor/MenuWorkshopEditor.unity3d");
      SleekImageTexture sleekImageTexture1 = new SleekImageTexture();
      sleekImageTexture1.positionOffset_X = 20;
      sleekImageTexture1.positionOffset_Y = -20;
      sleekImageTexture1.positionScale_Y = 0.5f;
      sleekImageTexture1.sizeOffset_X = 40;
      sleekImageTexture1.sizeOffset_Y = 40;
      sleekImageTexture1.texture = (Texture) bundle.load("Lock");
      this.button.add((Sleek) sleekImageTexture1);
      bundle.unload();
    }

    private void onClickedButton(SleekButton button)
    {
      if (this.onClickedLevel == null)
        return;
      this.onClickedLevel(this, (byte) (this.positionOffset_Y / 110));
    }

    public override void draw(bool ignoreCulling)
    {
      this.drawChildren(ignoreCulling);
    }
  }
}
