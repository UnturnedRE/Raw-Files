// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MenuSurvivorsGroupUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class MenuSurvivorsGroupUI
  {
    private static Local localization;
    private static Sleek container;
    public static bool active;
    private static SteamGroup[] groups;
    private static SleekField nickField;
    private static SleekButtonIcon groupButton;
    private static SleekScrollBox groupsBox;

    public MenuSurvivorsGroupUI()
    {
      MenuSurvivorsGroupUI.localization = Localization.read("/Menu/Survivors/MenuSurvivorsGroup.dat");
      MenuSurvivorsGroupUI.container = new Sleek();
      MenuSurvivorsGroupUI.container.positionOffset_X = 10;
      MenuSurvivorsGroupUI.container.positionOffset_Y = 10;
      MenuSurvivorsGroupUI.container.positionScale_Y = 1f;
      MenuSurvivorsGroupUI.container.sizeOffset_X = -20;
      MenuSurvivorsGroupUI.container.sizeOffset_Y = -20;
      MenuSurvivorsGroupUI.container.sizeScale_X = 1f;
      MenuSurvivorsGroupUI.container.sizeScale_Y = 1f;
      MenuUI.container.add(MenuSurvivorsGroupUI.container);
      MenuSurvivorsGroupUI.active = false;
      MenuSurvivorsGroupUI.groups = Provider.provider.communityService.getGroups();
      MenuSurvivorsGroupUI.nickField = new SleekField();
      MenuSurvivorsGroupUI.nickField.positionOffset_X = -100;
      MenuSurvivorsGroupUI.nickField.positionOffset_Y = 100;
      MenuSurvivorsGroupUI.nickField.positionScale_X = 0.75f;
      MenuSurvivorsGroupUI.nickField.sizeOffset_X = 200;
      MenuSurvivorsGroupUI.nickField.sizeOffset_Y = 30;
      MenuSurvivorsGroupUI.nickField.maxLength = 32;
      MenuSurvivorsGroupUI.nickField.addLabel(MenuSurvivorsGroupUI.localization.format("Nick_Field_Label"), ESleekSide.LEFT);
      MenuSurvivorsGroupUI.nickField.onTyped = new Typed(MenuSurvivorsGroupUI.onTypedNickField);
      MenuSurvivorsGroupUI.container.add((Sleek) MenuSurvivorsGroupUI.nickField);
      MenuSurvivorsGroupUI.groupButton = new SleekButtonIcon((Texture2D) null, 20);
      MenuSurvivorsGroupUI.groupButton.positionOffset_X = -100;
      MenuSurvivorsGroupUI.groupButton.positionOffset_Y = 140;
      MenuSurvivorsGroupUI.groupButton.positionScale_X = 0.75f;
      MenuSurvivorsGroupUI.groupButton.sizeOffset_X = 200;
      MenuSurvivorsGroupUI.groupButton.sizeOffset_Y = 30;
      MenuSurvivorsGroupUI.groupButton.addLabel(MenuSurvivorsGroupUI.localization.format("Group_Box_Label"), ESleekSide.LEFT);
      MenuSurvivorsGroupUI.groupButton.onClickedButton = new ClickedButton(MenuSurvivorsGroupUI.onClickedUngroupButton);
      MenuSurvivorsGroupUI.container.add((Sleek) MenuSurvivorsGroupUI.groupButton);
      MenuSurvivorsGroupUI.groupsBox = new SleekScrollBox();
      MenuSurvivorsGroupUI.groupsBox.positionOffset_X = -100;
      MenuSurvivorsGroupUI.groupsBox.positionOffset_Y = 180;
      MenuSurvivorsGroupUI.groupsBox.positionScale_X = 0.75f;
      MenuSurvivorsGroupUI.groupsBox.sizeOffset_X = 230;
      MenuSurvivorsGroupUI.groupsBox.sizeOffset_Y = -280;
      MenuSurvivorsGroupUI.groupsBox.sizeScale_Y = 1f;
      MenuSurvivorsGroupUI.groupsBox.area = new Rect(0.0f, 0.0f, 5f, (float) (MenuSurvivorsGroupUI.groups.Length * 40 - 10));
      MenuSurvivorsGroupUI.container.add((Sleek) MenuSurvivorsGroupUI.groupsBox);
      for (int index = 0; index < MenuSurvivorsGroupUI.groups.Length; ++index)
      {
        SleekButtonIcon sleekButtonIcon = new SleekButtonIcon(MenuSurvivorsGroupUI.groups[index].icon, 20);
        sleekButtonIcon.positionOffset_Y = index * 40;
        sleekButtonIcon.sizeOffset_X = 200;
        sleekButtonIcon.sizeOffset_Y = 30;
        sleekButtonIcon.text = MenuSurvivorsGroupUI.groups[index].name;
        sleekButtonIcon.onClickedButton = new ClickedButton(MenuSurvivorsGroupUI.onClickedGroupButton);
        MenuSurvivorsGroupUI.groupsBox.add((Sleek) sleekButtonIcon);
      }
      Characters.onCharacterUpdated += new CharacterUpdated(MenuSurvivorsGroupUI.onCharacterUpdated);
    }

    public static void open()
    {
      if (MenuSurvivorsGroupUI.active)
      {
        MenuSurvivorsGroupUI.close();
      }
      else
      {
        MenuSurvivorsGroupUI.active = true;
        MenuSurvivorsGroupUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!MenuSurvivorsGroupUI.active)
        return;
      MenuSurvivorsGroupUI.active = false;
      MenuSurvivorsGroupUI.container.lerpPositionScale(0.0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onCharacterUpdated(byte index, Character character)
    {
      if ((int) index != (int) Characters.selected)
        return;
      MenuSurvivorsGroupUI.nickField.text = character.nick;
      for (int index1 = 0; index1 < MenuSurvivorsGroupUI.groups.Length; ++index1)
      {
        if (MenuSurvivorsGroupUI.groups[index1].steamID == character.group)
        {
          MenuSurvivorsGroupUI.groupButton.text = MenuSurvivorsGroupUI.groups[index1].name;
          MenuSurvivorsGroupUI.groupButton.icon = MenuSurvivorsGroupUI.groups[index1].icon;
          return;
        }
      }
      MenuSurvivorsGroupUI.groupButton.text = MenuSurvivorsGroupUI.localization.format("Group_Box");
      MenuSurvivorsGroupUI.groupButton.icon = (Texture2D) null;
    }

    private static void onTypedNickField(SleekField field, string text)
    {
      Characters.renick(text);
    }

    private static void onClickedGroupButton(SleekButton button)
    {
      Characters.group(MenuSurvivorsGroupUI.groups[button.positionOffset_Y / 40].steamID);
    }

    private static void onClickedUngroupButton(SleekButton button)
    {
      Characters.ungroup();
    }
  }
}
