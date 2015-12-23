// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerBarricadeSignUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class PlayerBarricadeSignUI
  {
    private static Sleek container;
    public static bool active;
    private static InteractableSign sign;
    private static SleekField textField;
    private static SleekBox textBox;
    private static SleekButton yesButton;
    private static SleekButton noButton;

    public PlayerBarricadeSignUI()
    {
      Local local = Localization.read("/Player/PlayerBarricadeSign.dat");
      PlayerBarricadeSignUI.container = new Sleek();
      PlayerBarricadeSignUI.container.positionScale_Y = 1f;
      PlayerBarricadeSignUI.container.positionOffset_X = 10;
      PlayerBarricadeSignUI.container.positionOffset_Y = 10;
      PlayerBarricadeSignUI.container.sizeOffset_X = -20;
      PlayerBarricadeSignUI.container.sizeOffset_Y = -20;
      PlayerBarricadeSignUI.container.sizeScale_X = 1f;
      PlayerBarricadeSignUI.container.sizeScale_Y = 1f;
      PlayerUI.container.add(PlayerBarricadeSignUI.container);
      PlayerBarricadeSignUI.active = false;
      PlayerBarricadeSignUI.sign = (InteractableSign) null;
      PlayerBarricadeSignUI.textField = new SleekField();
      PlayerBarricadeSignUI.textField.positionOffset_X = -200;
      PlayerBarricadeSignUI.textField.positionScale_X = 0.5f;
      PlayerBarricadeSignUI.textField.positionScale_Y = 0.1f;
      PlayerBarricadeSignUI.textField.sizeOffset_X = 400;
      PlayerBarricadeSignUI.textField.sizeOffset_Y = -40;
      PlayerBarricadeSignUI.textField.sizeScale_Y = 0.8f;
      PlayerBarricadeSignUI.textField.maxLength = 200;
      PlayerBarricadeSignUI.textField.multiline = true;
      PlayerBarricadeSignUI.container.add((Sleek) PlayerBarricadeSignUI.textField);
      PlayerBarricadeSignUI.textBox = new SleekBox();
      PlayerBarricadeSignUI.textBox.positionOffset_X = -200;
      PlayerBarricadeSignUI.textBox.positionScale_X = 0.5f;
      PlayerBarricadeSignUI.textBox.positionScale_Y = 0.1f;
      PlayerBarricadeSignUI.textBox.sizeOffset_X = 400;
      PlayerBarricadeSignUI.textBox.sizeOffset_Y = -40;
      PlayerBarricadeSignUI.textBox.sizeScale_Y = 0.8f;
      PlayerBarricadeSignUI.container.add((Sleek) PlayerBarricadeSignUI.textBox);
      PlayerBarricadeSignUI.yesButton = new SleekButton();
      PlayerBarricadeSignUI.yesButton.positionOffset_X = -200;
      PlayerBarricadeSignUI.yesButton.positionOffset_Y = -30;
      PlayerBarricadeSignUI.yesButton.positionScale_X = 0.5f;
      PlayerBarricadeSignUI.yesButton.positionScale_Y = 0.9f;
      PlayerBarricadeSignUI.yesButton.sizeOffset_X = 195;
      PlayerBarricadeSignUI.yesButton.sizeOffset_Y = 30;
      PlayerBarricadeSignUI.yesButton.text = local.format("Yes_Button");
      PlayerBarricadeSignUI.yesButton.tooltip = local.format("Yes_Button_Tooltip");
      PlayerBarricadeSignUI.yesButton.onClickedButton = new ClickedButton(PlayerBarricadeSignUI.onClickedYesButton);
      PlayerBarricadeSignUI.container.add((Sleek) PlayerBarricadeSignUI.yesButton);
      PlayerBarricadeSignUI.noButton = new SleekButton();
      PlayerBarricadeSignUI.noButton.positionOffset_X = 5;
      PlayerBarricadeSignUI.noButton.positionOffset_Y = -30;
      PlayerBarricadeSignUI.noButton.positionScale_X = 0.5f;
      PlayerBarricadeSignUI.noButton.positionScale_Y = 0.9f;
      PlayerBarricadeSignUI.noButton.sizeOffset_X = 195;
      PlayerBarricadeSignUI.noButton.sizeOffset_Y = 30;
      PlayerBarricadeSignUI.noButton.text = local.format("No_Button");
      PlayerBarricadeSignUI.noButton.tooltip = local.format("No_Button_Tooltip");
      PlayerBarricadeSignUI.noButton.onClickedButton = new ClickedButton(PlayerBarricadeSignUI.onClickedNoButton);
      PlayerBarricadeSignUI.container.add((Sleek) PlayerBarricadeSignUI.noButton);
    }

    public static void open(string newText)
    {
      if (PlayerBarricadeSignUI.active)
      {
        PlayerBarricadeSignUI.close();
      }
      else
      {
        PlayerBarricadeSignUI.active = true;
        PlayerBarricadeSignUI.sign = (InteractableSign) null;
        PlayerBarricadeSignUI.yesButton.isVisible = false;
        PlayerBarricadeSignUI.noButton.positionOffset_X = -200;
        PlayerBarricadeSignUI.noButton.sizeOffset_X = 400;
        PlayerBarricadeSignUI.textBox.text = newText;
        PlayerBarricadeSignUI.textField.isVisible = false;
        PlayerBarricadeSignUI.textBox.isVisible = true;
        PlayerBarricadeSignUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void open(InteractableSign newSign)
    {
      if (PlayerBarricadeSignUI.active)
      {
        PlayerBarricadeSignUI.close();
      }
      else
      {
        PlayerBarricadeSignUI.active = true;
        PlayerBarricadeSignUI.sign = newSign;
        PlayerBarricadeSignUI.yesButton.isVisible = true;
        PlayerBarricadeSignUI.noButton.positionOffset_X = -5;
        PlayerBarricadeSignUI.noButton.sizeOffset_X = 195;
        PlayerBarricadeSignUI.textField.text = PlayerBarricadeSignUI.sign.text;
        PlayerBarricadeSignUI.textField.isVisible = true;
        PlayerBarricadeSignUI.textBox.isVisible = false;
        PlayerBarricadeSignUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!PlayerBarricadeSignUI.active)
        return;
      PlayerBarricadeSignUI.active = false;
      PlayerBarricadeSignUI.sign = (InteractableSign) null;
      PlayerBarricadeSignUI.container.lerpPositionScale(0.0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onClickedYesButton(SleekButton button)
    {
      BarricadeManager.updateSign(PlayerBarricadeSignUI.sign.transform, PlayerBarricadeSignUI.textField.text);
      PlayerLifeUI.open();
      PlayerBarricadeSignUI.close();
    }

    private static void onClickedNoButton(SleekButton button)
    {
      PlayerLifeUI.open();
      PlayerBarricadeSignUI.close();
    }
  }
}
