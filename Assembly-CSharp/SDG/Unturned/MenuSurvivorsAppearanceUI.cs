// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MenuSurvivorsAppearanceUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class MenuSurvivorsAppearanceUI
  {
    private static Local localization;
    private static Sleek container;
    public static bool active;
    private static SleekScrollBox customizationBox;
    private static SleekBox faceBox;
    private static SleekBox hairBox;
    private static SleekBox beardBox;
    private static SleekButton[] faceButtons;
    private static SleekButton[] hairButtons;
    private static SleekButton[] beardButtons;
    private static SleekBox skinBox;
    private static SleekBox colorBox;
    private static SleekButton[] skinButtons;
    private static SleekButton[] colorButtons;
    private static SleekColorPicker skinColorPicker;
    private static SleekColorPicker colorColorPicker;
    private static SleekButtonState handState;
    private static SleekSlider characterSlider;

    public MenuSurvivorsAppearanceUI()
    {
      MenuSurvivorsAppearanceUI.localization = Localization.read("/Menu/Survivors/MenuSurvivorsAppearance.dat");
      MenuSurvivorsAppearanceUI.container = new Sleek();
      MenuSurvivorsAppearanceUI.container.positionOffset_X = 10;
      MenuSurvivorsAppearanceUI.container.positionOffset_Y = 10;
      MenuSurvivorsAppearanceUI.container.positionScale_Y = 1f;
      MenuSurvivorsAppearanceUI.container.sizeOffset_X = -20;
      MenuSurvivorsAppearanceUI.container.sizeOffset_Y = -20;
      MenuSurvivorsAppearanceUI.container.sizeScale_X = 1f;
      MenuSurvivorsAppearanceUI.container.sizeScale_Y = 1f;
      MenuUI.container.add(MenuSurvivorsAppearanceUI.container);
      MenuSurvivorsAppearanceUI.active = false;
      MenuSurvivorsAppearanceUI.customizationBox = new SleekScrollBox();
      MenuSurvivorsAppearanceUI.customizationBox.positionOffset_X = -140;
      MenuSurvivorsAppearanceUI.customizationBox.positionOffset_Y = 100;
      MenuSurvivorsAppearanceUI.customizationBox.positionScale_X = 0.75f;
      MenuSurvivorsAppearanceUI.customizationBox.sizeOffset_X = 270;
      MenuSurvivorsAppearanceUI.customizationBox.sizeOffset_Y = -270;
      MenuSurvivorsAppearanceUI.customizationBox.sizeScale_Y = 1f;
      MenuSurvivorsAppearanceUI.container.add((Sleek) MenuSurvivorsAppearanceUI.customizationBox);
      MenuSurvivorsAppearanceUI.faceBox = new SleekBox();
      MenuSurvivorsAppearanceUI.faceBox.sizeOffset_X = 240;
      MenuSurvivorsAppearanceUI.faceBox.sizeOffset_Y = 30;
      MenuSurvivorsAppearanceUI.faceBox.text = MenuSurvivorsAppearanceUI.localization.format("Face_Box");
      MenuSurvivorsAppearanceUI.faceBox.tooltip = MenuSurvivorsAppearanceUI.localization.format("Face_Box_Tooltip");
      MenuSurvivorsAppearanceUI.customizationBox.add((Sleek) MenuSurvivorsAppearanceUI.faceBox);
      MenuSurvivorsAppearanceUI.faceButtons = new SleekButton[(int) Customization.FACES_FREE + (int) Customization.FACES_PRO];
      for (int index = 0; index < MenuSurvivorsAppearanceUI.faceButtons.Length; ++index)
      {
        SleekButton sleekButton = new SleekButton();
        sleekButton.positionOffset_X = index % 5 * 50;
        sleekButton.positionOffset_Y = 40 + Mathf.FloorToInt((float) index / 5f) * 50;
        sleekButton.sizeOffset_X = 40;
        sleekButton.sizeOffset_Y = 40;
        MenuSurvivorsAppearanceUI.faceBox.add((Sleek) sleekButton);
        SleekImageTexture sleekImageTexture1 = new SleekImageTexture();
        sleekImageTexture1.positionOffset_X = 10;
        sleekImageTexture1.positionOffset_Y = 10;
        sleekImageTexture1.sizeOffset_X = 20;
        sleekImageTexture1.sizeOffset_Y = 20;
        sleekImageTexture1.texture = (Texture) Resources.Load("Materials/Pixel");
        sleekButton.add((Sleek) sleekImageTexture1);
        SleekImageTexture sleekImageTexture2 = new SleekImageTexture();
        sleekImageTexture2.positionOffset_X = 2;
        sleekImageTexture2.positionOffset_Y = 2;
        sleekImageTexture2.sizeOffset_X = 16;
        sleekImageTexture2.sizeOffset_Y = 16;
        sleekImageTexture2.texture = (Texture) Resources.Load("Faces/" + (object) index + "/Texture");
        sleekImageTexture1.add((Sleek) sleekImageTexture2);
        if (index >= (int) Customization.FACES_FREE)
        {
          if (Provider.isPro)
          {
            sleekButton.onClickedButton = new ClickedButton(MenuSurvivorsAppearanceUI.onClickedFaceButton);
          }
          else
          {
            sleekButton.backgroundColor = Palette.PRO;
            Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Pro.unity3d");
            SleekImageTexture sleekImageTexture3 = new SleekImageTexture();
            sleekImageTexture3.positionOffset_X = -10;
            sleekImageTexture3.positionOffset_Y = -10;
            sleekImageTexture3.positionScale_X = 0.5f;
            sleekImageTexture3.positionScale_Y = 0.5f;
            sleekImageTexture3.sizeOffset_X = 20;
            sleekImageTexture3.sizeOffset_Y = 20;
            sleekImageTexture3.texture = (Texture) bundle.load("Lock_Small");
            sleekButton.add((Sleek) sleekImageTexture3);
            bundle.unload();
          }
        }
        else
          sleekButton.onClickedButton = new ClickedButton(MenuSurvivorsAppearanceUI.onClickedFaceButton);
        MenuSurvivorsAppearanceUI.faceButtons[index] = sleekButton;
      }
      MenuSurvivorsAppearanceUI.hairBox = new SleekBox();
      MenuSurvivorsAppearanceUI.hairBox.positionOffset_Y = 80 + Mathf.CeilToInt((float) MenuSurvivorsAppearanceUI.faceButtons.Length / 5f) * 50;
      MenuSurvivorsAppearanceUI.hairBox.sizeOffset_X = 240;
      MenuSurvivorsAppearanceUI.hairBox.sizeOffset_Y = 30;
      MenuSurvivorsAppearanceUI.hairBox.text = MenuSurvivorsAppearanceUI.localization.format("Hair_Box");
      MenuSurvivorsAppearanceUI.hairBox.tooltip = MenuSurvivorsAppearanceUI.localization.format("Hair_Box_Tooltip");
      MenuSurvivorsAppearanceUI.customizationBox.add((Sleek) MenuSurvivorsAppearanceUI.hairBox);
      MenuSurvivorsAppearanceUI.hairButtons = new SleekButton[(int) Customization.HAIRS_FREE + (int) Customization.HAIRS_PRO];
      for (int index = 0; index < MenuSurvivorsAppearanceUI.hairButtons.Length; ++index)
      {
        SleekButton sleekButton = new SleekButton();
        sleekButton.positionOffset_X = index % 5 * 50;
        sleekButton.positionOffset_Y = 40 + Mathf.FloorToInt((float) index / 5f) * 50;
        sleekButton.sizeOffset_X = 40;
        sleekButton.sizeOffset_Y = 40;
        MenuSurvivorsAppearanceUI.hairBox.add((Sleek) sleekButton);
        SleekImageTexture sleekImageTexture1 = new SleekImageTexture();
        sleekImageTexture1.positionOffset_X = 10;
        sleekImageTexture1.positionOffset_Y = 10;
        sleekImageTexture1.sizeOffset_X = 20;
        sleekImageTexture1.sizeOffset_Y = 20;
        sleekImageTexture1.texture = (Texture) Resources.Load("Hairs/" + (object) index + "/Texture");
        sleekButton.add((Sleek) sleekImageTexture1);
        if (index >= (int) Customization.HAIRS_FREE)
        {
          if (Provider.isPro)
          {
            sleekButton.onClickedButton = new ClickedButton(MenuSurvivorsAppearanceUI.onClickedHairButton);
          }
          else
          {
            sleekButton.backgroundColor = Palette.PRO;
            Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Pro.unity3d");
            SleekImageTexture sleekImageTexture2 = new SleekImageTexture();
            sleekImageTexture2.positionOffset_X = -10;
            sleekImageTexture2.positionOffset_Y = -10;
            sleekImageTexture2.positionScale_X = 0.5f;
            sleekImageTexture2.positionScale_Y = 0.5f;
            sleekImageTexture2.sizeOffset_X = 20;
            sleekImageTexture2.sizeOffset_Y = 20;
            sleekImageTexture2.texture = (Texture) bundle.load("Lock_Small");
            sleekButton.add((Sleek) sleekImageTexture2);
            bundle.unload();
          }
        }
        else
          sleekButton.onClickedButton = new ClickedButton(MenuSurvivorsAppearanceUI.onClickedHairButton);
        MenuSurvivorsAppearanceUI.hairButtons[index] = sleekButton;
      }
      MenuSurvivorsAppearanceUI.beardBox = new SleekBox();
      MenuSurvivorsAppearanceUI.beardBox.positionOffset_Y = 160 + Mathf.CeilToInt((float) MenuSurvivorsAppearanceUI.faceButtons.Length / 5f) * 50 + Mathf.CeilToInt((float) MenuSurvivorsAppearanceUI.hairButtons.Length / 5f) * 50;
      MenuSurvivorsAppearanceUI.beardBox.sizeOffset_X = 240;
      MenuSurvivorsAppearanceUI.beardBox.sizeOffset_Y = 30;
      MenuSurvivorsAppearanceUI.beardBox.text = MenuSurvivorsAppearanceUI.localization.format("Beard_Box");
      MenuSurvivorsAppearanceUI.beardBox.tooltip = MenuSurvivorsAppearanceUI.localization.format("Beard_Box_Tooltip");
      MenuSurvivorsAppearanceUI.customizationBox.add((Sleek) MenuSurvivorsAppearanceUI.beardBox);
      MenuSurvivorsAppearanceUI.beardButtons = new SleekButton[(int) Customization.BEARDS_FREE + (int) Customization.BEARDS_PRO];
      for (int index = 0; index < MenuSurvivorsAppearanceUI.beardButtons.Length; ++index)
      {
        SleekButton sleekButton = new SleekButton();
        sleekButton.positionOffset_X = index % 5 * 50;
        sleekButton.positionOffset_Y = 40 + Mathf.FloorToInt((float) index / 5f) * 50;
        sleekButton.sizeOffset_X = 40;
        sleekButton.sizeOffset_Y = 40;
        MenuSurvivorsAppearanceUI.beardBox.add((Sleek) sleekButton);
        SleekImageTexture sleekImageTexture1 = new SleekImageTexture();
        sleekImageTexture1.positionOffset_X = 10;
        sleekImageTexture1.positionOffset_Y = 10;
        sleekImageTexture1.sizeOffset_X = 20;
        sleekImageTexture1.sizeOffset_Y = 20;
        sleekImageTexture1.texture = (Texture) Resources.Load("Beards/" + (object) index + "/Texture");
        sleekButton.add((Sleek) sleekImageTexture1);
        if (index >= (int) Customization.BEARDS_FREE)
        {
          if (Provider.isPro)
          {
            sleekButton.onClickedButton = new ClickedButton(MenuSurvivorsAppearanceUI.onClickedBeardButton);
          }
          else
          {
            sleekButton.backgroundColor = Palette.PRO;
            Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Pro.unity3d");
            SleekImageTexture sleekImageTexture2 = new SleekImageTexture();
            sleekImageTexture2.positionOffset_X = -10;
            sleekImageTexture2.positionOffset_Y = -10;
            sleekImageTexture2.positionScale_X = 0.5f;
            sleekImageTexture2.positionScale_Y = 0.5f;
            sleekImageTexture2.sizeOffset_X = 20;
            sleekImageTexture2.sizeOffset_Y = 20;
            sleekImageTexture2.texture = (Texture) bundle.load("Lock_Small");
            sleekButton.add((Sleek) sleekImageTexture2);
            bundle.unload();
          }
        }
        else
          sleekButton.onClickedButton = new ClickedButton(MenuSurvivorsAppearanceUI.onClickedBeardButton);
        MenuSurvivorsAppearanceUI.beardButtons[index] = sleekButton;
      }
      MenuSurvivorsAppearanceUI.skinBox = new SleekBox();
      MenuSurvivorsAppearanceUI.skinBox.positionOffset_Y = 240 + Mathf.CeilToInt((float) MenuSurvivorsAppearanceUI.faceButtons.Length / 5f) * 50 + Mathf.CeilToInt((float) MenuSurvivorsAppearanceUI.hairButtons.Length / 5f) * 50 + Mathf.CeilToInt((float) MenuSurvivorsAppearanceUI.beardButtons.Length / 5f) * 50;
      MenuSurvivorsAppearanceUI.skinBox.sizeOffset_X = 240;
      MenuSurvivorsAppearanceUI.skinBox.sizeOffset_Y = 30;
      MenuSurvivorsAppearanceUI.skinBox.text = MenuSurvivorsAppearanceUI.localization.format("Skin_Box");
      MenuSurvivorsAppearanceUI.skinBox.tooltip = MenuSurvivorsAppearanceUI.localization.format("Skin_Box_Tooltip");
      MenuSurvivorsAppearanceUI.customizationBox.add((Sleek) MenuSurvivorsAppearanceUI.skinBox);
      MenuSurvivorsAppearanceUI.skinButtons = new SleekButton[Customization.SKINS.Length];
      for (int index = 0; index < MenuSurvivorsAppearanceUI.skinButtons.Length; ++index)
      {
        SleekButton sleekButton = new SleekButton();
        sleekButton.positionOffset_X = index % 5 * 50;
        sleekButton.positionOffset_Y = 40 + Mathf.FloorToInt((float) index / 5f) * 50;
        sleekButton.sizeOffset_X = 40;
        sleekButton.sizeOffset_Y = 40;
        sleekButton.onClickedButton = new ClickedButton(MenuSurvivorsAppearanceUI.onClickedSkinButton);
        MenuSurvivorsAppearanceUI.skinBox.add((Sleek) sleekButton);
        SleekImageTexture sleekImageTexture = new SleekImageTexture();
        sleekImageTexture.positionOffset_X = 10;
        sleekImageTexture.positionOffset_Y = 10;
        sleekImageTexture.sizeOffset_X = 20;
        sleekImageTexture.sizeOffset_Y = 20;
        sleekImageTexture.texture = (Texture) Resources.Load("Materials/Pixel");
        sleekImageTexture.backgroundColor = Customization.SKINS[index];
        sleekButton.add((Sleek) sleekImageTexture);
        MenuSurvivorsAppearanceUI.skinButtons[index] = sleekButton;
      }
      MenuSurvivorsAppearanceUI.skinColorPicker = new SleekColorPicker();
      MenuSurvivorsAppearanceUI.skinColorPicker.positionOffset_Y = 280 + Mathf.CeilToInt((float) MenuSurvivorsAppearanceUI.faceButtons.Length / 5f) * 50 + Mathf.CeilToInt((float) MenuSurvivorsAppearanceUI.hairButtons.Length / 5f) * 50 + Mathf.CeilToInt((float) MenuSurvivorsAppearanceUI.beardButtons.Length / 5f) * 50 + Mathf.CeilToInt((float) MenuSurvivorsAppearanceUI.skinButtons.Length / 5f) * 50;
      MenuSurvivorsAppearanceUI.customizationBox.add((Sleek) MenuSurvivorsAppearanceUI.skinColorPicker);
      if (Provider.isPro)
      {
        MenuSurvivorsAppearanceUI.skinColorPicker.onColorPicked = new ColorPicked(MenuSurvivorsAppearanceUI.onSkinColorPicked);
      }
      else
      {
        Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Pro.unity3d");
        SleekImageTexture sleekImageTexture = new SleekImageTexture();
        sleekImageTexture.positionOffset_X = -40;
        sleekImageTexture.positionOffset_Y = -40;
        sleekImageTexture.positionScale_X = 0.5f;
        sleekImageTexture.positionScale_Y = 0.5f;
        sleekImageTexture.sizeOffset_X = 80;
        sleekImageTexture.sizeOffset_Y = 80;
        sleekImageTexture.texture = (Texture) bundle.load("Lock_Large");
        MenuSurvivorsAppearanceUI.skinColorPicker.add((Sleek) sleekImageTexture);
        bundle.unload();
      }
      MenuSurvivorsAppearanceUI.colorBox = new SleekBox();
      MenuSurvivorsAppearanceUI.colorBox.positionOffset_Y = 440 + Mathf.CeilToInt((float) MenuSurvivorsAppearanceUI.faceButtons.Length / 5f) * 50 + Mathf.CeilToInt((float) MenuSurvivorsAppearanceUI.hairButtons.Length / 5f) * 50 + Mathf.CeilToInt((float) MenuSurvivorsAppearanceUI.beardButtons.Length / 5f) * 50 + Mathf.CeilToInt((float) MenuSurvivorsAppearanceUI.skinButtons.Length / 5f) * 50;
      MenuSurvivorsAppearanceUI.colorBox.sizeOffset_X = 240;
      MenuSurvivorsAppearanceUI.colorBox.sizeOffset_Y = 30;
      MenuSurvivorsAppearanceUI.colorBox.text = MenuSurvivorsAppearanceUI.localization.format("Color_Box");
      MenuSurvivorsAppearanceUI.colorBox.tooltip = MenuSurvivorsAppearanceUI.localization.format("Color_Box_Tooltip");
      MenuSurvivorsAppearanceUI.customizationBox.add((Sleek) MenuSurvivorsAppearanceUI.colorBox);
      MenuSurvivorsAppearanceUI.colorButtons = new SleekButton[Customization.COLORS.Length];
      for (int index = 0; index < MenuSurvivorsAppearanceUI.colorButtons.Length; ++index)
      {
        SleekButton sleekButton = new SleekButton();
        sleekButton.positionOffset_X = index % 5 * 50;
        sleekButton.positionOffset_Y = 40 + Mathf.FloorToInt((float) index / 5f) * 50;
        sleekButton.sizeOffset_X = 40;
        sleekButton.sizeOffset_Y = 40;
        sleekButton.onClickedButton = new ClickedButton(MenuSurvivorsAppearanceUI.onClickedColorButton);
        MenuSurvivorsAppearanceUI.colorBox.add((Sleek) sleekButton);
        SleekImageTexture sleekImageTexture = new SleekImageTexture();
        sleekImageTexture.positionOffset_X = 10;
        sleekImageTexture.positionOffset_Y = 10;
        sleekImageTexture.sizeOffset_X = 20;
        sleekImageTexture.sizeOffset_Y = 20;
        sleekImageTexture.texture = (Texture) Resources.Load("Materials/Pixel");
        sleekImageTexture.backgroundColor = Customization.COLORS[index];
        sleekButton.add((Sleek) sleekImageTexture);
        MenuSurvivorsAppearanceUI.colorButtons[index] = sleekButton;
      }
      MenuSurvivorsAppearanceUI.colorColorPicker = new SleekColorPicker();
      MenuSurvivorsAppearanceUI.colorColorPicker.positionOffset_Y = 480 + Mathf.CeilToInt((float) MenuSurvivorsAppearanceUI.faceButtons.Length / 5f) * 50 + Mathf.CeilToInt((float) MenuSurvivorsAppearanceUI.hairButtons.Length / 5f) * 50 + Mathf.CeilToInt((float) MenuSurvivorsAppearanceUI.beardButtons.Length / 5f) * 50 + Mathf.CeilToInt((float) MenuSurvivorsAppearanceUI.skinButtons.Length / 5f) * 50 + Mathf.CeilToInt((float) MenuSurvivorsAppearanceUI.colorButtons.Length / 5f) * 50;
      MenuSurvivorsAppearanceUI.customizationBox.add((Sleek) MenuSurvivorsAppearanceUI.colorColorPicker);
      if (Provider.isPro)
      {
        MenuSurvivorsAppearanceUI.colorColorPicker.onColorPicked = new ColorPicked(MenuSurvivorsAppearanceUI.onColorColorPicked);
      }
      else
      {
        Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Pro.unity3d");
        SleekImageTexture sleekImageTexture = new SleekImageTexture();
        sleekImageTexture.positionOffset_X = -40;
        sleekImageTexture.positionOffset_Y = -40;
        sleekImageTexture.positionScale_X = 0.5f;
        sleekImageTexture.positionScale_Y = 0.5f;
        sleekImageTexture.sizeOffset_X = 80;
        sleekImageTexture.sizeOffset_Y = 80;
        sleekImageTexture.texture = (Texture) bundle.load("Lock_Large");
        MenuSurvivorsAppearanceUI.colorColorPicker.add((Sleek) sleekImageTexture);
        bundle.unload();
      }
      MenuSurvivorsAppearanceUI.customizationBox.area = new Rect(0.0f, 0.0f, 5f, (float) (600 + Mathf.CeilToInt((float) MenuSurvivorsAppearanceUI.faceButtons.Length / 5f) * 50 + Mathf.CeilToInt((float) MenuSurvivorsAppearanceUI.hairButtons.Length / 5f) * 50 + Mathf.CeilToInt((float) MenuSurvivorsAppearanceUI.beardButtons.Length / 5f) * 50 + Mathf.CeilToInt((float) MenuSurvivorsAppearanceUI.skinButtons.Length / 5f) * 50 + Mathf.CeilToInt((float) MenuSurvivorsAppearanceUI.colorButtons.Length / 5f) * 50));
      Characters.onCharacterUpdated += new CharacterUpdated(MenuSurvivorsAppearanceUI.onCharacterUpdated);
      MenuSurvivorsAppearanceUI.handState = new SleekButtonState(new GUIContent[2]
      {
        new GUIContent(MenuSurvivorsAppearanceUI.localization.format("Right")),
        new GUIContent(MenuSurvivorsAppearanceUI.localization.format("Left"))
      });
      MenuSurvivorsAppearanceUI.handState.positionOffset_X = -140;
      MenuSurvivorsAppearanceUI.handState.positionOffset_Y = -160;
      MenuSurvivorsAppearanceUI.handState.positionScale_X = 0.75f;
      MenuSurvivorsAppearanceUI.handState.positionScale_Y = 1f;
      MenuSurvivorsAppearanceUI.handState.sizeOffset_X = 240;
      MenuSurvivorsAppearanceUI.handState.sizeOffset_Y = 30;
      MenuSurvivorsAppearanceUI.handState.onSwappedState = new SwappedState(MenuSurvivorsAppearanceUI.onSwappedHandState);
      MenuSurvivorsAppearanceUI.container.add((Sleek) MenuSurvivorsAppearanceUI.handState);
      MenuSurvivorsAppearanceUI.characterSlider = new SleekSlider();
      MenuSurvivorsAppearanceUI.characterSlider.positionOffset_X = -140;
      MenuSurvivorsAppearanceUI.characterSlider.positionOffset_Y = -120;
      MenuSurvivorsAppearanceUI.characterSlider.positionScale_X = 0.75f;
      MenuSurvivorsAppearanceUI.characterSlider.positionScale_Y = 1f;
      MenuSurvivorsAppearanceUI.characterSlider.sizeOffset_X = 240;
      MenuSurvivorsAppearanceUI.characterSlider.sizeOffset_Y = 20;
      MenuSurvivorsAppearanceUI.characterSlider.orientation = ESleekOrientation.HORIZONTAL;
      MenuSurvivorsAppearanceUI.characterSlider.onDragged = new Dragged(MenuSurvivorsAppearanceUI.onDraggedCharacterSlider);
      MenuSurvivorsAppearanceUI.container.add((Sleek) MenuSurvivorsAppearanceUI.characterSlider);
    }

    public static void open()
    {
      if (MenuSurvivorsAppearanceUI.active)
      {
        MenuSurvivorsAppearanceUI.close();
      }
      else
      {
        MenuSurvivorsAppearanceUI.active = true;
        MenuSurvivorsAppearanceUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!MenuSurvivorsAppearanceUI.active)
        return;
      MenuSurvivorsAppearanceUI.active = false;
      MenuSurvivorsAppearanceUI.container.lerpPositionScale(0.0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void updateFaces(Color color)
    {
      for (int index = 0; index < MenuSurvivorsAppearanceUI.faceButtons.Length; ++index)
        MenuSurvivorsAppearanceUI.faceButtons[index].children[0].backgroundColor = color;
    }

    private static void updateColors(Color color)
    {
      for (int index = 1; index < MenuSurvivorsAppearanceUI.hairButtons.Length; ++index)
        MenuSurvivorsAppearanceUI.hairButtons[index].children[0].backgroundColor = color;
      for (int index = 1; index < MenuSurvivorsAppearanceUI.beardButtons.Length; ++index)
        MenuSurvivorsAppearanceUI.beardButtons[index].children[0].backgroundColor = color;
    }

    private static void onCharacterUpdated(byte index, Character character)
    {
      if ((int) index != (int) Characters.selected)
        return;
      MenuSurvivorsAppearanceUI.skinColorPicker.state = character.skin;
      MenuSurvivorsAppearanceUI.colorColorPicker.state = character.color;
      MenuSurvivorsAppearanceUI.updateFaces(character.skin);
      MenuSurvivorsAppearanceUI.updateColors(character.color);
      MenuSurvivorsAppearanceUI.handState.state = !character.hand ? 0 : 1;
    }

    private static void onClickedFaceButton(SleekButton button)
    {
      Characters.growFace((byte) (button.positionOffset_X / 50 + (button.positionOffset_Y - 40) / 50 * 5));
    }

    private static void onClickedHairButton(SleekButton button)
    {
      Characters.growHair((byte) (button.positionOffset_X / 50 + (button.positionOffset_Y - 40) / 50 * 5));
    }

    private static void onClickedBeardButton(SleekButton button)
    {
      Characters.growBeard((byte) (button.positionOffset_X / 50 + (button.positionOffset_Y - 40) / 50 * 5));
    }

    private static void onClickedSkinButton(SleekButton button)
    {
      int index = button.positionOffset_X / 50 + (button.positionOffset_Y - 40) / 50 * 5;
      Color color = Customization.SKINS[index];
      Characters.paintSkin(color);
      MenuSurvivorsAppearanceUI.skinColorPicker.state = color;
      MenuSurvivorsAppearanceUI.updateFaces(color);
    }

    private static void onSkinColorPicked(SleekColorPicker picker, Color color)
    {
      Characters.paintSkin(color);
      MenuSurvivorsAppearanceUI.updateFaces(color);
    }

    private static void onClickedColorButton(SleekButton button)
    {
      int index = button.positionOffset_X / 50 + (button.positionOffset_Y - 40) / 50 * 5;
      Color color = Customization.COLORS[index];
      Characters.paintColor(color);
      MenuSurvivorsAppearanceUI.colorColorPicker.state = color;
      MenuSurvivorsAppearanceUI.updateColors(color);
    }

    private static void onColorColorPicked(SleekColorPicker picker, Color color)
    {
      Characters.paintColor(color);
      MenuSurvivorsAppearanceUI.updateColors(color);
    }

    private static void onSwappedHandState(SleekButtonState button, int index)
    {
      Characters.hand(index != 0);
    }

    private static void onDraggedCharacterSlider(SleekSlider slider, float state)
    {
      Characters.characterYaw = state * 360f;
    }
  }
}
