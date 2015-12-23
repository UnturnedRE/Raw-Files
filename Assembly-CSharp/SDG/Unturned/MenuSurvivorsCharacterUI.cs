// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MenuSurvivorsCharacterUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class MenuSurvivorsCharacterUI
  {
    public static Local localization;
    public static Bundle icons;
    private static Sleek container;
    public static bool active;
    private static SleekScrollBox characterBox;
    private static SleekCharacter[] characterButtons;
    private static SleekField nameField;
    private static SleekBoxIcon specialityBox;
    private static SleekScrollBox specialitiesBox;

    public MenuSurvivorsCharacterUI()
    {
      if (MenuSurvivorsCharacterUI.icons != null)
        MenuSurvivorsCharacterUI.icons.unload();
      MenuSurvivorsCharacterUI.localization = Localization.read("/Menu/Survivors/MenuSurvivorsCharacter.dat");
      MenuSurvivorsCharacterUI.icons = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Survivors/MenuSurvivorsCharacter/MenuSurvivorsCharacter.unity3d");
      MenuSurvivorsCharacterUI.container = new Sleek();
      MenuSurvivorsCharacterUI.container.positionOffset_X = 10;
      MenuSurvivorsCharacterUI.container.positionOffset_Y = 10;
      MenuSurvivorsCharacterUI.container.positionScale_Y = 1f;
      MenuSurvivorsCharacterUI.container.sizeOffset_X = -20;
      MenuSurvivorsCharacterUI.container.sizeOffset_Y = -20;
      MenuSurvivorsCharacterUI.container.sizeScale_X = 1f;
      MenuSurvivorsCharacterUI.container.sizeScale_Y = 1f;
      MenuUI.container.add(MenuSurvivorsCharacterUI.container);
      MenuSurvivorsCharacterUI.active = false;
      MenuSurvivorsCharacterUI.characterBox = new SleekScrollBox();
      MenuSurvivorsCharacterUI.characterBox.positionOffset_X = -100;
      MenuSurvivorsCharacterUI.characterBox.positionOffset_Y = 300;
      MenuSurvivorsCharacterUI.characterBox.positionScale_X = 0.75f;
      MenuSurvivorsCharacterUI.characterBox.sizeOffset_X = 230;
      MenuSurvivorsCharacterUI.characterBox.sizeOffset_Y = -400;
      MenuSurvivorsCharacterUI.characterBox.sizeScale_Y = 1f;
      MenuSurvivorsCharacterUI.characterBox.area = new Rect(0.0f, 0.0f, 5f, (float) (((int) Customization.FREE_CHARACTERS + (int) Customization.PRO_CHARACTERS) * 110 - 10));
      MenuSurvivorsCharacterUI.container.add((Sleek) MenuSurvivorsCharacterUI.characterBox);
      MenuSurvivorsCharacterUI.characterButtons = new SleekCharacter[(int) Customization.FREE_CHARACTERS + (int) Customization.PRO_CHARACTERS];
      for (byte newIndex = (byte) 0; (int) newIndex < MenuSurvivorsCharacterUI.characterButtons.Length; ++newIndex)
      {
        SleekCharacter sleekCharacter = new SleekCharacter(newIndex);
        sleekCharacter.positionOffset_Y = (int) newIndex * 110;
        sleekCharacter.sizeOffset_X = 200;
        sleekCharacter.sizeOffset_Y = 100;
        sleekCharacter.onClickedCharacter = new ClickedCharacter(MenuSurvivorsCharacterUI.onClickedCharacter);
        MenuSurvivorsCharacterUI.characterBox.add((Sleek) sleekCharacter);
        MenuSurvivorsCharacterUI.characterButtons[(int) newIndex] = sleekCharacter;
      }
      MenuSurvivorsCharacterUI.nameField = new SleekField();
      MenuSurvivorsCharacterUI.nameField.positionOffset_X = -100;
      MenuSurvivorsCharacterUI.nameField.positionOffset_Y = 100;
      MenuSurvivorsCharacterUI.nameField.positionScale_X = 0.75f;
      MenuSurvivorsCharacterUI.nameField.sizeOffset_X = 200;
      MenuSurvivorsCharacterUI.nameField.sizeOffset_Y = 30;
      MenuSurvivorsCharacterUI.nameField.maxLength = 32;
      MenuSurvivorsCharacterUI.nameField.addLabel(MenuSurvivorsCharacterUI.localization.format("Name_Field_Label"), ESleekSide.LEFT);
      MenuSurvivorsCharacterUI.nameField.onTyped = new Typed(MenuSurvivorsCharacterUI.onTypedNameField);
      MenuSurvivorsCharacterUI.container.add((Sleek) MenuSurvivorsCharacterUI.nameField);
      MenuSurvivorsCharacterUI.specialityBox = new SleekBoxIcon((Texture2D) null);
      MenuSurvivorsCharacterUI.specialityBox.positionOffset_X = -100;
      MenuSurvivorsCharacterUI.specialityBox.positionOffset_Y = 140;
      MenuSurvivorsCharacterUI.specialityBox.positionScale_X = 0.75f;
      MenuSurvivorsCharacterUI.specialityBox.sizeOffset_X = 200;
      MenuSurvivorsCharacterUI.specialityBox.sizeOffset_Y = 30;
      MenuSurvivorsCharacterUI.specialityBox.addLabel(MenuSurvivorsCharacterUI.localization.format("Speciality_Box_Label"), ESleekSide.LEFT);
      MenuSurvivorsCharacterUI.container.add((Sleek) MenuSurvivorsCharacterUI.specialityBox);
      MenuSurvivorsCharacterUI.specialitiesBox = new SleekScrollBox();
      MenuSurvivorsCharacterUI.specialitiesBox.positionOffset_X = -100;
      MenuSurvivorsCharacterUI.specialitiesBox.positionOffset_Y = 180;
      MenuSurvivorsCharacterUI.specialitiesBox.positionScale_X = 0.75f;
      MenuSurvivorsCharacterUI.specialitiesBox.sizeOffset_X = 230;
      MenuSurvivorsCharacterUI.specialitiesBox.sizeOffset_Y = 110;
      MenuSurvivorsCharacterUI.specialitiesBox.area = new Rect(0.0f, 0.0f, 5f, (float) ((int) PlayerSkills.SPECIALITIES * 40 - 10));
      MenuSurvivorsCharacterUI.container.add((Sleek) MenuSurvivorsCharacterUI.specialitiesBox);
      for (int index = 0; index < (int) PlayerSkills.SPECIALITIES; ++index)
      {
        SleekButtonIcon sleekButtonIcon = new SleekButtonIcon((Texture2D) MenuSurvivorsCharacterUI.icons.load("Speciality_" + (object) index));
        sleekButtonIcon.positionOffset_Y = index * 40;
        sleekButtonIcon.sizeOffset_X = 200;
        sleekButtonIcon.sizeOffset_Y = 30;
        sleekButtonIcon.text = MenuSurvivorsCharacterUI.localization.format("Speciality_" + (object) index);
        sleekButtonIcon.onClickedButton = new ClickedButton(MenuSurvivorsCharacterUI.onClickedSpeciality);
        MenuSurvivorsCharacterUI.specialitiesBox.add((Sleek) sleekButtonIcon);
      }
      Characters.onCharacterUpdated += new CharacterUpdated(MenuSurvivorsCharacterUI.onCharacterUpdated);
    }

    public static void open()
    {
      if (MenuSurvivorsCharacterUI.active)
      {
        MenuSurvivorsCharacterUI.close();
      }
      else
      {
        MenuSurvivorsCharacterUI.active = true;
        MenuSurvivorsCharacterUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!MenuSurvivorsCharacterUI.active)
        return;
      MenuSurvivorsCharacterUI.active = false;
      MenuSurvivorsCharacterUI.container.lerpPositionScale(0.0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onCharacterUpdated(byte index, Character character)
    {
      if ((int) index == (int) Characters.selected)
      {
        MenuSurvivorsCharacterUI.nameField.text = character.name;
        MenuSurvivorsCharacterUI.specialityBox.icon = (Texture2D) MenuSurvivorsCharacterUI.icons.load("Speciality_" + (object) character.speciality);
        MenuSurvivorsCharacterUI.specialityBox.text = MenuSurvivorsCharacterUI.localization.format("Speciality_" + (object) (byte) character.speciality);
      }
      MenuSurvivorsCharacterUI.characterButtons[(int) index].updateCharacter(character);
    }

    private static void onTypedNameField(SleekField field, string text)
    {
      Characters.rename(text);
    }

    private static void onClickedCharacter(SleekCharacter character, byte index)
    {
      Characters.selected = index;
    }

    private static void onClickedSpeciality(SleekButton button)
    {
      Characters.specialize((EPlayerSpeciality) (button.positionOffset_Y / 40));
    }
  }
}
