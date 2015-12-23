// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekCharacter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services.Store;
using SDG.SteamworksProvider.Services.Store;
using UnityEngine;

namespace SDG.Unturned
{
  public class SleekCharacter : Sleek
  {
    private byte index;
    private SleekButton button;
    private SleekLabel nameLabel;
    private SleekLabel nickLabel;
    private SleekLabel specialityLabel;
    public ClickedCharacter onClickedCharacter;

    public SleekCharacter(byte newIndex)
    {
      this.init();
      this.index = newIndex;
      this.button = new SleekButton();
      this.button.sizeScale_X = 1f;
      this.button.sizeScale_Y = 1f;
      this.button.onClickedButton = new ClickedButton(this.onClickedButton);
      this.add((Sleek) this.button);
      this.nameLabel = new SleekLabel();
      this.nameLabel.sizeScale_X = 1f;
      this.nameLabel.sizeScale_Y = 0.33f;
      this.button.add((Sleek) this.nameLabel);
      this.nickLabel = new SleekLabel();
      this.nickLabel.positionScale_Y = 0.33f;
      this.nickLabel.sizeScale_X = 1f;
      this.nickLabel.sizeScale_Y = 0.33f;
      this.button.add((Sleek) this.nickLabel);
      this.specialityLabel = new SleekLabel();
      this.specialityLabel.positionScale_Y = 0.66f;
      this.specialityLabel.sizeScale_X = 1f;
      this.specialityLabel.sizeScale_Y = 0.33f;
      this.button.add((Sleek) this.specialityLabel);
      if (Provider.isPro || (int) this.index < (int) Customization.FREE_CHARACTERS)
        return;
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Pro/Pro.unity3d");
      SleekImageTexture sleekImageTexture = new SleekImageTexture();
      sleekImageTexture.positionOffset_X = -20;
      sleekImageTexture.positionOffset_Y = -20;
      sleekImageTexture.positionScale_X = 0.5f;
      sleekImageTexture.positionScale_Y = 0.5f;
      sleekImageTexture.sizeOffset_X = 40;
      sleekImageTexture.sizeOffset_Y = 40;
      sleekImageTexture.texture = (Texture) bundle.load("Lock_Medium");
      this.button.add((Sleek) sleekImageTexture);
      bundle.unload();
    }

    public void updateCharacter(Character character)
    {
      this.nameLabel.text = MenuSurvivorsCharacterUI.localization.format("Name_Label", (object) character.name);
      this.nickLabel.text = MenuSurvivorsCharacterUI.localization.format("Nick_Label", (object) character.nick);
      this.specialityLabel.text = MenuSurvivorsCharacterUI.localization.format("Speciality_" + (object) (byte) character.speciality);
    }

    private void onClickedButton(SleekButton button)
    {
      if (!Provider.isPro && (int) this.index >= (int) Customization.FREE_CHARACTERS)
      {
        if (!Provider.provider.storeService.canOpenStore)
        {
          MenuUI.alert(MenuSurvivorsCharacterUI.localization.format("Overlay"));
          return;
        }
        Provider.provider.storeService.open((IStorePackageID) new SteamworksStorePackageID(Provider.PRO_ID.m_AppId));
      }
      if (this.onClickedCharacter == null)
        return;
      this.onClickedCharacter(this, this.index);
    }

    public override void draw(bool ignoreCulling)
    {
      this.drawChildren(ignoreCulling);
    }
  }
}
