// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MenuSurvivorsUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class MenuSurvivorsUI
  {
    private static Sleek container;
    public static bool active;
    private static SleekButtonIcon characterButton;
    private static SleekButtonIcon appearanceButton;
    private static SleekButtonIcon groupButton;
    private static SleekButtonIcon clothingButton;

    public MenuSurvivorsUI()
    {
      Local local = Localization.read("/Menu/Survivors/MenuSurvivors.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Survivors/MenuSurvivors/MenuSurvivors.unity3d");
      MenuSurvivorsUI.container = new Sleek();
      MenuSurvivorsUI.container.positionOffset_X = 10;
      MenuSurvivorsUI.container.positionOffset_Y = 10;
      MenuSurvivorsUI.container.positionScale_Y = -1f;
      MenuSurvivorsUI.container.sizeOffset_X = -20;
      MenuSurvivorsUI.container.sizeOffset_Y = -20;
      MenuSurvivorsUI.container.sizeScale_X = 1f;
      MenuSurvivorsUI.container.sizeScale_Y = 1f;
      MenuUI.container.add(MenuSurvivorsUI.container);
      MenuSurvivorsUI.active = false;
      MenuSurvivorsUI.characterButton = new SleekButtonIcon((Texture2D) bundle.load("Character"));
      MenuSurvivorsUI.characterButton.positionOffset_X = -100;
      MenuSurvivorsUI.characterButton.positionOffset_Y = -115;
      MenuSurvivorsUI.characterButton.positionScale_X = 0.5f;
      MenuSurvivorsUI.characterButton.positionScale_Y = 0.5f;
      MenuSurvivorsUI.characterButton.sizeOffset_X = 200;
      MenuSurvivorsUI.characterButton.sizeOffset_Y = 50;
      MenuSurvivorsUI.characterButton.text = local.format("CharacterButtonText");
      MenuSurvivorsUI.characterButton.tooltip = local.format("CharacterButtonTooltip");
      MenuSurvivorsUI.characterButton.onClickedButton = new ClickedButton(MenuSurvivorsUI.onClickedCharacterButton);
      MenuSurvivorsUI.characterButton.fontSize = 14;
      MenuSurvivorsUI.container.add((Sleek) MenuSurvivorsUI.characterButton);
      MenuSurvivorsUI.appearanceButton = new SleekButtonIcon((Texture2D) bundle.load("Appearance"));
      MenuSurvivorsUI.appearanceButton.positionOffset_X = -100;
      MenuSurvivorsUI.appearanceButton.positionOffset_Y = -55;
      MenuSurvivorsUI.appearanceButton.positionScale_X = 0.5f;
      MenuSurvivorsUI.appearanceButton.positionScale_Y = 0.5f;
      MenuSurvivorsUI.appearanceButton.sizeOffset_X = 200;
      MenuSurvivorsUI.appearanceButton.sizeOffset_Y = 50;
      MenuSurvivorsUI.appearanceButton.text = local.format("AppearanceButtonText");
      MenuSurvivorsUI.appearanceButton.tooltip = local.format("AppearanceButtonTooltip");
      MenuSurvivorsUI.appearanceButton.onClickedButton = new ClickedButton(MenuSurvivorsUI.onClickedAppearanceButton);
      MenuSurvivorsUI.appearanceButton.fontSize = 14;
      MenuSurvivorsUI.container.add((Sleek) MenuSurvivorsUI.appearanceButton);
      MenuSurvivorsUI.groupButton = new SleekButtonIcon((Texture2D) bundle.load("Group"));
      MenuSurvivorsUI.groupButton.positionOffset_X = -100;
      MenuSurvivorsUI.groupButton.positionOffset_Y = 5;
      MenuSurvivorsUI.groupButton.positionScale_X = 0.5f;
      MenuSurvivorsUI.groupButton.positionScale_Y = 0.5f;
      MenuSurvivorsUI.groupButton.sizeOffset_X = 200;
      MenuSurvivorsUI.groupButton.sizeOffset_Y = 50;
      MenuSurvivorsUI.groupButton.text = local.format("GroupButtonText");
      MenuSurvivorsUI.groupButton.tooltip = local.format("GroupButtonTooltip");
      MenuSurvivorsUI.groupButton.onClickedButton = new ClickedButton(MenuSurvivorsUI.onClickedGroupButton);
      MenuSurvivorsUI.groupButton.fontSize = 14;
      MenuSurvivorsUI.container.add((Sleek) MenuSurvivorsUI.groupButton);
      MenuSurvivorsUI.clothingButton = new SleekButtonIcon((Texture2D) bundle.load("Clothing"));
      MenuSurvivorsUI.clothingButton.positionOffset_X = -100;
      MenuSurvivorsUI.clothingButton.positionOffset_Y = 65;
      MenuSurvivorsUI.clothingButton.positionScale_X = 0.5f;
      MenuSurvivorsUI.clothingButton.positionScale_Y = 0.5f;
      MenuSurvivorsUI.clothingButton.sizeOffset_X = 200;
      MenuSurvivorsUI.clothingButton.sizeOffset_Y = 50;
      MenuSurvivorsUI.clothingButton.text = local.format("ClothingButtonText");
      MenuSurvivorsUI.clothingButton.tooltip = local.format("ClothingButtonTooltip");
      MenuSurvivorsUI.clothingButton.onClickedButton = new ClickedButton(MenuSurvivorsUI.onClickedClothingButton);
      MenuSurvivorsUI.clothingButton.fontSize = 14;
      MenuSurvivorsUI.container.add((Sleek) MenuSurvivorsUI.clothingButton);
      bundle.unload();
      MenuSurvivorsCharacterUI survivorsCharacterUi = new MenuSurvivorsCharacterUI();
      MenuSurvivorsAppearanceUI survivorsAppearanceUi = new MenuSurvivorsAppearanceUI();
      MenuSurvivorsGroupUI survivorsGroupUi = new MenuSurvivorsGroupUI();
      MenuSurvivorsClothingUI survivorsClothingUi = new MenuSurvivorsClothingUI();
    }

    public static void open()
    {
      if (MenuSurvivorsUI.active)
      {
        MenuSurvivorsUI.close();
      }
      else
      {
        MenuSurvivorsUI.active = true;
        MenuSurvivorsUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!MenuSurvivorsUI.active)
        return;
      MenuSurvivorsUI.active = false;
      Characters.save();
      MenuSurvivorsUI.container.lerpPositionScale(0.0f, -1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onClickedCharacterButton(SleekButton button)
    {
      MenuSurvivorsCharacterUI.open();
      MenuSurvivorsUI.close();
    }

    private static void onClickedAppearanceButton(SleekButton button)
    {
      MenuSurvivorsAppearanceUI.open();
      MenuSurvivorsUI.close();
    }

    private static void onClickedGroupButton(SleekButton button)
    {
      MenuSurvivorsGroupUI.open();
      MenuSurvivorsUI.close();
    }

    private static void onClickedClothingButton(SleekButton button)
    {
      MenuSurvivorsClothingUI.open();
      MenuSurvivorsUI.close();
    }
  }
}
