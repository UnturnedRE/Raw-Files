// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerLifeUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class PlayerLifeUI
  {
    public static Local localization;
    public static Bundle icons;
    private static Sleek _container;
    public static bool active;
    public static bool chatting;
    public static bool gesturing;
    private static SleekScrollBox chatBox;
    private static SleekLabel[] chatLabel;
    private static SleekLabel[] gameLabel;
    public static SleekField chatField;
    private static SleekBox modeBox;
    private static SleekBoxIcon voiceBox;
    private static SleekButton surrenderButton;
    private static SleekButton pointButton;
    private static SleekButton waveButton;
    public static SleekImageTexture painImage;
    public static SleekImageTexture scopeImage;
    public static SleekImageTexture scopeOverlay;
    public static SleekImageTexture scopeLeftOverlay;
    public static SleekImageTexture scopeRightOverlay;
    public static SleekImageTexture scopeUpOverlay;
    public static SleekImageTexture scopeDownOverlay;
    public static SleekImageTexture binocularsOverlay;
    public static SleekImageTexture hitEntitiyImage;
    public static SleekImageTexture hitCriticalImage;
    public static SleekImageTexture hitBuildImage;
    public static SleekImageTexture crosshairLeftImage;
    public static SleekImageTexture crosshairRightImage;
    public static SleekImageTexture crosshairDownImage;
    public static SleekImageTexture crosshairUpImage;
    public static SleekImageTexture dotImage;
    private static SleekBox lifeBox;
    private static SleekImageTexture healthIcon;
    private static SleekProgress healthProgress;
    private static SleekImageTexture foodIcon;
    private static SleekProgress foodProgress;
    private static SleekImageTexture waterIcon;
    private static SleekProgress waterProgress;
    private static SleekImageTexture virusIcon;
    private static SleekProgress virusProgress;
    private static SleekImageTexture staminaIcon;
    private static SleekProgress staminaProgress;
    private static SleekLabel waveLabel;
    private static SleekLabel scoreLabel;
    private static SleekBoxIcon oxygenBox;
    private static SleekProgress oxygenProgress;
    private static SleekBox vehicleBox;
    private static SleekImageTexture fuelIcon;
    private static SleekProgress fuelProgress;
    private static SleekImageTexture speedIcon;
    private static SleekProgress speedProgress;
    private static SleekBoxIcon bleedingBox;
    private static SleekBoxIcon brokenBox;
    private static SleekBoxIcon temperatureBox;
    private static SleekBoxIcon moonBox;

    public static Sleek container
    {
      get
      {
        return PlayerLifeUI._container;
      }
    }

    public PlayerLifeUI()
    {
      if (PlayerLifeUI.icons != null)
        PlayerLifeUI.icons.unload();
      PlayerLifeUI.localization = Localization.read("/Player/PlayerLife.dat");
      PlayerLifeUI.icons = Bundles.getBundle("/Bundles/Textures/Player/Icons/PlayerLife/PlayerLife.unity3d");
      PlayerLifeUI._container = new Sleek();
      PlayerLifeUI.container.positionOffset_X = 10;
      PlayerLifeUI.container.positionOffset_Y = 10;
      PlayerLifeUI.container.sizeOffset_X = -20;
      PlayerLifeUI.container.sizeOffset_Y = -20;
      PlayerLifeUI.container.sizeScale_X = 1f;
      PlayerLifeUI.container.sizeScale_Y = 1f;
      PlayerUI.container.add(PlayerLifeUI.container);
      PlayerLifeUI.active = true;
      PlayerLifeUI.chatting = false;
      PlayerLifeUI.chatBox = new SleekScrollBox();
      PlayerLifeUI.chatBox.sizeOffset_X = 430;
      PlayerLifeUI.chatBox.sizeOffset_Y = 160;
      PlayerLifeUI.chatBox.area = new Rect(0.0f, 0.0f, 5f, (float) (ChatManager.chat.Length * 40));
      PlayerLifeUI.chatBox.state = (Vector2) new Vector3(0.0f, float.MaxValue);
      PlayerLifeUI.container.add((Sleek) PlayerLifeUI.chatBox);
      PlayerLifeUI.chatBox.isVisible = false;
      PlayerLifeUI.chatLabel = new SleekLabel[ChatManager.chat.Length];
      for (int index = 0; index < PlayerLifeUI.chatLabel.Length; ++index)
      {
        SleekLabel sleekLabel = new SleekLabel();
        sleekLabel.positionOffset_Y = PlayerLifeUI.chatLabel.Length * 40 - 40 - index * 40;
        sleekLabel.sizeOffset_X = 400;
        sleekLabel.sizeOffset_Y = 40;
        sleekLabel.fontSize = 14;
        sleekLabel.fontAlignment = TextAnchor.UpperLeft;
        PlayerLifeUI.chatBox.add((Sleek) sleekLabel);
        PlayerLifeUI.chatLabel[index] = sleekLabel;
      }
      PlayerLifeUI.gameLabel = new SleekLabel[4];
      for (int index = 0; index < PlayerLifeUI.gameLabel.Length; ++index)
      {
        SleekLabel sleekLabel = new SleekLabel();
        sleekLabel.positionOffset_Y = 120 - index * 40;
        sleekLabel.sizeOffset_X = 400;
        sleekLabel.sizeOffset_Y = 40;
        sleekLabel.fontSize = 14;
        sleekLabel.fontAlignment = TextAnchor.UpperLeft;
        PlayerLifeUI.container.add((Sleek) sleekLabel);
        PlayerLifeUI.gameLabel[index] = sleekLabel;
      }
      PlayerLifeUI.chatField = new SleekField();
      PlayerLifeUI.chatField.positionOffset_X = -350;
      PlayerLifeUI.chatField.positionOffset_Y = 170;
      PlayerLifeUI.chatField.sizeOffset_X = 330;
      PlayerLifeUI.chatField.sizeOffset_Y = 30;
      PlayerLifeUI.chatField.fontAlignment = TextAnchor.MiddleLeft;
      PlayerLifeUI.chatField.maxLength = ChatManager.LENGTH;
      PlayerLifeUI.container.add((Sleek) PlayerLifeUI.chatField);
      PlayerLifeUI.modeBox = new SleekBox();
      PlayerLifeUI.modeBox.positionOffset_X = -100;
      PlayerLifeUI.modeBox.sizeOffset_X = 90;
      PlayerLifeUI.modeBox.sizeOffset_Y = 30;
      PlayerLifeUI.modeBox.fontAlignment = TextAnchor.MiddleCenter;
      PlayerLifeUI.chatField.add((Sleek) PlayerLifeUI.modeBox);
      PlayerLifeUI.voiceBox = new SleekBoxIcon((Texture2D) PlayerLifeUI.icons.load("Voice"));
      PlayerLifeUI.voiceBox.positionOffset_Y = 210;
      PlayerLifeUI.voiceBox.sizeOffset_X = 50;
      PlayerLifeUI.voiceBox.sizeOffset_Y = 50;
      PlayerLifeUI.container.add((Sleek) PlayerLifeUI.voiceBox);
      PlayerLifeUI.voiceBox.isVisible = false;
      PlayerLifeUI.painImage = new SleekImageTexture();
      PlayerLifeUI.painImage.sizeScale_X = 1f;
      PlayerLifeUI.painImage.sizeScale_Y = 1f;
      PlayerLifeUI.painImage.backgroundColor = Palette.COLOR_R;
      PlayerLifeUI.painImage.backgroundColor.a = 0.0f;
      PlayerLifeUI.painImage.texture = (Texture) Resources.Load("Materials/Pixel");
      PlayerUI.window.add((Sleek) PlayerLifeUI.painImage);
      PlayerLifeUI.scopeImage = new SleekImageTexture();
      PlayerLifeUI.scopeImage.positionScale_X = 0.5f;
      PlayerLifeUI.scopeImage.positionScale_Y = 0.5f;
      PlayerUI.window.add((Sleek) PlayerLifeUI.scopeImage);
      PlayerLifeUI.scopeImage.isVisible = false;
      PlayerLifeUI.scopeOverlay = new SleekImageTexture((Texture) Resources.Load("Overlay/Scope"));
      PlayerLifeUI.scopeOverlay.positionOffset_X = -256;
      PlayerLifeUI.scopeOverlay.positionOffset_Y = -256;
      PlayerLifeUI.scopeOverlay.positionScale_X = 0.5f;
      PlayerLifeUI.scopeOverlay.positionScale_Y = 0.5f;
      PlayerLifeUI.scopeOverlay.sizeOffset_X = 512;
      PlayerLifeUI.scopeOverlay.sizeOffset_Y = 512;
      PlayerUI.window.add((Sleek) PlayerLifeUI.scopeOverlay);
      PlayerLifeUI.scopeOverlay.isVisible = false;
      PlayerLifeUI.scopeLeftOverlay = new SleekImageTexture((Texture) Resources.Load("Materials/Pixel"));
      PlayerLifeUI.scopeLeftOverlay.positionOffset_Y = -256;
      PlayerLifeUI.scopeLeftOverlay.positionScale_Y = 0.5f;
      PlayerLifeUI.scopeLeftOverlay.sizeOffset_X = -256;
      PlayerLifeUI.scopeLeftOverlay.sizeOffset_Y = 512;
      PlayerLifeUI.scopeLeftOverlay.sizeScale_X = 0.5f;
      PlayerLifeUI.scopeLeftOverlay.backgroundColor = Color.black;
      PlayerUI.window.add((Sleek) PlayerLifeUI.scopeLeftOverlay);
      PlayerLifeUI.scopeLeftOverlay.isVisible = false;
      PlayerLifeUI.scopeRightOverlay = new SleekImageTexture((Texture) Resources.Load("Materials/Pixel"));
      PlayerLifeUI.scopeRightOverlay.positionOffset_Y = -256;
      PlayerLifeUI.scopeRightOverlay.positionScale_Y = 0.5f;
      PlayerLifeUI.scopeRightOverlay.positionOffset_X = 256;
      PlayerLifeUI.scopeRightOverlay.positionScale_X = 0.5f;
      PlayerLifeUI.scopeRightOverlay.sizeOffset_X = -256;
      PlayerLifeUI.scopeRightOverlay.sizeOffset_Y = 512;
      PlayerLifeUI.scopeRightOverlay.sizeScale_X = 0.5f;
      PlayerLifeUI.scopeRightOverlay.backgroundColor = Color.black;
      PlayerUI.window.add((Sleek) PlayerLifeUI.scopeRightOverlay);
      PlayerLifeUI.scopeRightOverlay.isVisible = false;
      PlayerLifeUI.scopeUpOverlay = new SleekImageTexture((Texture) Resources.Load("Materials/Pixel"));
      PlayerLifeUI.scopeUpOverlay.sizeOffset_Y = -256;
      PlayerLifeUI.scopeUpOverlay.sizeScale_X = 1f;
      PlayerLifeUI.scopeUpOverlay.sizeScale_Y = 0.5f;
      PlayerLifeUI.scopeUpOverlay.backgroundColor = Color.black;
      PlayerUI.window.add((Sleek) PlayerLifeUI.scopeUpOverlay);
      PlayerLifeUI.scopeUpOverlay.isVisible = false;
      PlayerLifeUI.scopeDownOverlay = new SleekImageTexture((Texture) Resources.Load("Materials/Pixel"));
      PlayerLifeUI.scopeDownOverlay.positionOffset_Y = 256;
      PlayerLifeUI.scopeDownOverlay.positionScale_Y = 0.5f;
      PlayerLifeUI.scopeDownOverlay.sizeOffset_Y = -256;
      PlayerLifeUI.scopeDownOverlay.sizeScale_X = 1f;
      PlayerLifeUI.scopeDownOverlay.sizeScale_Y = 0.5f;
      PlayerLifeUI.scopeDownOverlay.backgroundColor = Color.black;
      PlayerUI.window.add((Sleek) PlayerLifeUI.scopeDownOverlay);
      PlayerLifeUI.scopeDownOverlay.isVisible = false;
      PlayerLifeUI.binocularsOverlay = new SleekImageTexture((Texture) Resources.Load("Overlay/Binoculars"));
      PlayerLifeUI.binocularsOverlay.sizeScale_X = 1f;
      PlayerLifeUI.binocularsOverlay.sizeScale_Y = 1f;
      PlayerUI.window.add((Sleek) PlayerLifeUI.binocularsOverlay);
      PlayerLifeUI.binocularsOverlay.isVisible = false;
      PlayerLifeUI.surrenderButton = new SleekButton();
      PlayerLifeUI.surrenderButton.positionOffset_X = -210;
      PlayerLifeUI.surrenderButton.positionOffset_Y = -15;
      PlayerLifeUI.surrenderButton.positionScale_X = 0.5f;
      PlayerLifeUI.surrenderButton.positionScale_Y = 0.5f;
      PlayerLifeUI.surrenderButton.sizeOffset_X = 200;
      PlayerLifeUI.surrenderButton.sizeOffset_Y = 30;
      PlayerLifeUI.surrenderButton.text = PlayerLifeUI.localization.format("Surrender");
      PlayerLifeUI.surrenderButton.onClickedButton = new ClickedButton(PlayerLifeUI.onClickedSurrenderButton);
      PlayerLifeUI.container.add((Sleek) PlayerLifeUI.surrenderButton);
      PlayerLifeUI.surrenderButton.isVisible = false;
      PlayerLifeUI.pointButton = new SleekButton();
      PlayerLifeUI.pointButton.positionOffset_X = 10;
      PlayerLifeUI.pointButton.positionOffset_Y = -15;
      PlayerLifeUI.pointButton.positionScale_X = 0.5f;
      PlayerLifeUI.pointButton.positionScale_Y = 0.5f;
      PlayerLifeUI.pointButton.sizeOffset_X = 200;
      PlayerLifeUI.pointButton.sizeOffset_Y = 30;
      PlayerLifeUI.pointButton.text = PlayerLifeUI.localization.format("Point");
      PlayerLifeUI.pointButton.onClickedButton = new ClickedButton(PlayerLifeUI.onClickedPointButton);
      PlayerLifeUI.container.add((Sleek) PlayerLifeUI.pointButton);
      PlayerLifeUI.pointButton.isVisible = false;
      PlayerLifeUI.waveButton = new SleekButton();
      PlayerLifeUI.waveButton.positionOffset_X = -100;
      PlayerLifeUI.waveButton.positionOffset_Y = -55;
      PlayerLifeUI.waveButton.positionScale_X = 0.5f;
      PlayerLifeUI.waveButton.positionScale_Y = 0.5f;
      PlayerLifeUI.waveButton.sizeOffset_X = 200;
      PlayerLifeUI.waveButton.sizeOffset_Y = 30;
      PlayerLifeUI.waveButton.text = PlayerLifeUI.localization.format("Wave");
      PlayerLifeUI.waveButton.onClickedButton = new ClickedButton(PlayerLifeUI.onClickedWaveButton);
      PlayerLifeUI.container.add((Sleek) PlayerLifeUI.waveButton);
      PlayerLifeUI.waveButton.isVisible = false;
      PlayerLifeUI.hitEntitiyImage = new SleekImageTexture();
      PlayerLifeUI.hitEntitiyImage.positionOffset_X = -16;
      PlayerLifeUI.hitEntitiyImage.positionOffset_Y = -16;
      PlayerLifeUI.hitEntitiyImage.positionScale_X = 0.5f;
      PlayerLifeUI.hitEntitiyImage.positionScale_Y = 0.5f;
      PlayerLifeUI.hitEntitiyImage.sizeOffset_X = 32;
      PlayerLifeUI.hitEntitiyImage.sizeOffset_Y = 32;
      PlayerLifeUI.hitEntitiyImage.texture = (Texture) PlayerLifeUI.icons.load("Hit_Entity");
      PlayerLifeUI.hitEntitiyImage.backgroundColor = OptionsSettings.hitmarkerColor;
      PlayerUI.window.add((Sleek) PlayerLifeUI.hitEntitiyImage);
      PlayerLifeUI.hitEntitiyImage.isVisible = false;
      PlayerLifeUI.hitCriticalImage = new SleekImageTexture();
      PlayerLifeUI.hitCriticalImage.positionOffset_X = -16;
      PlayerLifeUI.hitCriticalImage.positionOffset_Y = -16;
      PlayerLifeUI.hitCriticalImage.positionScale_X = 0.5f;
      PlayerLifeUI.hitCriticalImage.positionScale_Y = 0.5f;
      PlayerLifeUI.hitCriticalImage.sizeOffset_X = 32;
      PlayerLifeUI.hitCriticalImage.sizeOffset_Y = 32;
      PlayerLifeUI.hitCriticalImage.texture = (Texture) PlayerLifeUI.icons.load("Hit_Critical");
      PlayerLifeUI.hitCriticalImage.backgroundColor = OptionsSettings.criticalHitmarkerColor;
      PlayerUI.window.add((Sleek) PlayerLifeUI.hitCriticalImage);
      PlayerLifeUI.hitCriticalImage.isVisible = false;
      PlayerLifeUI.hitBuildImage = new SleekImageTexture();
      PlayerLifeUI.hitBuildImage.positionOffset_X = -16;
      PlayerLifeUI.hitBuildImage.positionOffset_Y = -16;
      PlayerLifeUI.hitBuildImage.positionScale_X = 0.5f;
      PlayerLifeUI.hitBuildImage.positionScale_Y = 0.5f;
      PlayerLifeUI.hitBuildImage.sizeOffset_X = 32;
      PlayerLifeUI.hitBuildImage.sizeOffset_Y = 32;
      PlayerLifeUI.hitBuildImage.texture = (Texture) PlayerLifeUI.icons.load("Hit_Build");
      PlayerLifeUI.hitBuildImage.backgroundColor = OptionsSettings.crosshairColor;
      PlayerUI.window.add((Sleek) PlayerLifeUI.hitBuildImage);
      PlayerLifeUI.hitBuildImage.isVisible = false;
      PlayerLifeUI.dotImage = new SleekImageTexture();
      PlayerLifeUI.dotImage.positionOffset_X = -4;
      PlayerLifeUI.dotImage.positionOffset_Y = -4;
      PlayerLifeUI.dotImage.positionScale_X = 0.5f;
      PlayerLifeUI.dotImage.positionScale_Y = 0.5f;
      PlayerLifeUI.dotImage.sizeOffset_X = 8;
      PlayerLifeUI.dotImage.sizeOffset_Y = 8;
      PlayerLifeUI.dotImage.texture = (Texture) PlayerLifeUI.icons.load("Dot");
      PlayerLifeUI.dotImage.backgroundColor = OptionsSettings.crosshairColor;
      PlayerLifeUI.container.add((Sleek) PlayerLifeUI.dotImage);
      PlayerLifeUI.crosshairLeftImage = new SleekImageTexture();
      PlayerLifeUI.crosshairLeftImage.positionOffset_X = -4;
      PlayerLifeUI.crosshairLeftImage.positionOffset_Y = -4;
      PlayerLifeUI.crosshairLeftImage.positionScale_X = 0.5f;
      PlayerLifeUI.crosshairLeftImage.positionScale_Y = 0.5f;
      PlayerLifeUI.crosshairLeftImage.sizeOffset_X = 8;
      PlayerLifeUI.crosshairLeftImage.sizeOffset_Y = 8;
      PlayerLifeUI.crosshairLeftImage.texture = (Texture) PlayerLifeUI.icons.load("Crosshair_Right");
      PlayerLifeUI.crosshairLeftImage.backgroundColor = OptionsSettings.crosshairColor;
      PlayerLifeUI.container.add((Sleek) PlayerLifeUI.crosshairLeftImage);
      PlayerLifeUI.crosshairLeftImage.isVisible = false;
      PlayerLifeUI.crosshairRightImage = new SleekImageTexture();
      PlayerLifeUI.crosshairRightImage.positionOffset_X = -4;
      PlayerLifeUI.crosshairRightImage.positionOffset_Y = -4;
      PlayerLifeUI.crosshairRightImage.positionScale_X = 0.5f;
      PlayerLifeUI.crosshairRightImage.positionScale_Y = 0.5f;
      PlayerLifeUI.crosshairRightImage.sizeOffset_X = 8;
      PlayerLifeUI.crosshairRightImage.sizeOffset_Y = 8;
      PlayerLifeUI.crosshairRightImage.texture = (Texture) PlayerLifeUI.icons.load("Crosshair_Left");
      PlayerLifeUI.crosshairRightImage.backgroundColor = OptionsSettings.crosshairColor;
      PlayerLifeUI.container.add((Sleek) PlayerLifeUI.crosshairRightImage);
      PlayerLifeUI.crosshairRightImage.isVisible = false;
      PlayerLifeUI.crosshairDownImage = new SleekImageTexture();
      PlayerLifeUI.crosshairDownImage.positionOffset_X = -4;
      PlayerLifeUI.crosshairDownImage.positionOffset_Y = -4;
      PlayerLifeUI.crosshairDownImage.positionScale_X = 0.5f;
      PlayerLifeUI.crosshairDownImage.positionScale_Y = 0.5f;
      PlayerLifeUI.crosshairDownImage.sizeOffset_X = 8;
      PlayerLifeUI.crosshairDownImage.sizeOffset_Y = 8;
      PlayerLifeUI.crosshairDownImage.texture = (Texture) PlayerLifeUI.icons.load("Crosshair_Up");
      PlayerLifeUI.crosshairDownImage.backgroundColor = OptionsSettings.crosshairColor;
      PlayerLifeUI.container.add((Sleek) PlayerLifeUI.crosshairDownImage);
      PlayerLifeUI.crosshairDownImage.isVisible = false;
      PlayerLifeUI.crosshairUpImage = new SleekImageTexture();
      PlayerLifeUI.crosshairUpImage.positionOffset_X = -4;
      PlayerLifeUI.crosshairUpImage.positionOffset_Y = -4;
      PlayerLifeUI.crosshairUpImage.positionScale_X = 0.5f;
      PlayerLifeUI.crosshairUpImage.positionScale_Y = 0.5f;
      PlayerLifeUI.crosshairUpImage.sizeOffset_X = 8;
      PlayerLifeUI.crosshairUpImage.sizeOffset_Y = 8;
      PlayerLifeUI.crosshairUpImage.texture = (Texture) PlayerLifeUI.icons.load("Crosshair_Down");
      PlayerLifeUI.crosshairUpImage.backgroundColor = OptionsSettings.crosshairColor;
      PlayerLifeUI.container.add((Sleek) PlayerLifeUI.crosshairUpImage);
      PlayerLifeUI.crosshairUpImage.isVisible = false;
      PlayerLifeUI.lifeBox = new SleekBox();
      PlayerLifeUI.lifeBox.positionOffset_Y = -150;
      PlayerLifeUI.lifeBox.positionScale_Y = 1f;
      PlayerLifeUI.lifeBox.sizeOffset_Y = 150;
      PlayerLifeUI.lifeBox.sizeScale_X = 0.2f;
      PlayerLifeUI.container.add((Sleek) PlayerLifeUI.lifeBox);
      PlayerLifeUI.healthIcon = new SleekImageTexture();
      PlayerLifeUI.healthIcon.positionOffset_X = 5;
      PlayerLifeUI.healthIcon.positionOffset_Y = 5;
      PlayerLifeUI.healthIcon.sizeOffset_X = 20;
      PlayerLifeUI.healthIcon.sizeOffset_Y = 20;
      PlayerLifeUI.healthIcon.texture = (Texture) PlayerLifeUI.icons.load("Health");
      PlayerLifeUI.lifeBox.add((Sleek) PlayerLifeUI.healthIcon);
      PlayerLifeUI.healthProgress = new SleekProgress(string.Empty);
      PlayerLifeUI.healthProgress.positionOffset_X = 30;
      PlayerLifeUI.healthProgress.positionOffset_Y = 10;
      PlayerLifeUI.healthProgress.sizeOffset_X = -40;
      PlayerLifeUI.healthProgress.sizeOffset_Y = 10;
      PlayerLifeUI.healthProgress.sizeScale_X = 1f;
      PlayerLifeUI.healthProgress.color = Palette.COLOR_R;
      PlayerLifeUI.lifeBox.add((Sleek) PlayerLifeUI.healthProgress);
      PlayerLifeUI.foodIcon = new SleekImageTexture();
      PlayerLifeUI.foodIcon.positionOffset_X = 5;
      PlayerLifeUI.foodIcon.positionOffset_Y = 35;
      PlayerLifeUI.foodIcon.sizeOffset_X = 20;
      PlayerLifeUI.foodIcon.sizeOffset_Y = 20;
      PlayerLifeUI.foodIcon.texture = (Texture) PlayerLifeUI.icons.load("Food");
      PlayerLifeUI.lifeBox.add((Sleek) PlayerLifeUI.foodIcon);
      PlayerLifeUI.foodProgress = new SleekProgress(string.Empty);
      PlayerLifeUI.foodProgress.positionOffset_X = 30;
      PlayerLifeUI.foodProgress.positionOffset_Y = 40;
      PlayerLifeUI.foodProgress.sizeOffset_X = -40;
      PlayerLifeUI.foodProgress.sizeOffset_Y = 10;
      PlayerLifeUI.foodProgress.sizeScale_X = 1f;
      PlayerLifeUI.foodProgress.color = Palette.COLOR_O;
      PlayerLifeUI.lifeBox.add((Sleek) PlayerLifeUI.foodProgress);
      PlayerLifeUI.waterIcon = new SleekImageTexture();
      PlayerLifeUI.waterIcon.positionOffset_X = 5;
      PlayerLifeUI.waterIcon.positionOffset_Y = 65;
      PlayerLifeUI.waterIcon.sizeOffset_X = 20;
      PlayerLifeUI.waterIcon.sizeOffset_Y = 20;
      PlayerLifeUI.waterIcon.texture = (Texture) PlayerLifeUI.icons.load("Water");
      PlayerLifeUI.lifeBox.add((Sleek) PlayerLifeUI.waterIcon);
      PlayerLifeUI.waterProgress = new SleekProgress(string.Empty);
      PlayerLifeUI.waterProgress.positionOffset_X = 30;
      PlayerLifeUI.waterProgress.positionOffset_Y = 70;
      PlayerLifeUI.waterProgress.sizeOffset_X = -40;
      PlayerLifeUI.waterProgress.sizeOffset_Y = 10;
      PlayerLifeUI.waterProgress.sizeScale_X = 1f;
      PlayerLifeUI.waterProgress.color = Palette.COLOR_B;
      PlayerLifeUI.lifeBox.add((Sleek) PlayerLifeUI.waterProgress);
      PlayerLifeUI.virusIcon = new SleekImageTexture();
      PlayerLifeUI.virusIcon.positionOffset_X = 5;
      PlayerLifeUI.virusIcon.positionOffset_Y = 95;
      PlayerLifeUI.virusIcon.sizeOffset_X = 20;
      PlayerLifeUI.virusIcon.sizeOffset_Y = 20;
      PlayerLifeUI.virusIcon.texture = (Texture) PlayerLifeUI.icons.load("Virus");
      PlayerLifeUI.lifeBox.add((Sleek) PlayerLifeUI.virusIcon);
      PlayerLifeUI.virusProgress = new SleekProgress(string.Empty);
      PlayerLifeUI.virusProgress.positionOffset_X = 30;
      PlayerLifeUI.virusProgress.positionOffset_Y = 100;
      PlayerLifeUI.virusProgress.sizeOffset_X = -40;
      PlayerLifeUI.virusProgress.sizeOffset_Y = 10;
      PlayerLifeUI.virusProgress.sizeScale_X = 1f;
      PlayerLifeUI.virusProgress.color = Palette.COLOR_G;
      PlayerLifeUI.lifeBox.add((Sleek) PlayerLifeUI.virusProgress);
      PlayerLifeUI.staminaIcon = new SleekImageTexture();
      PlayerLifeUI.staminaIcon.positionOffset_X = 5;
      PlayerLifeUI.staminaIcon.positionOffset_Y = 125;
      PlayerLifeUI.staminaIcon.sizeOffset_X = 20;
      PlayerLifeUI.staminaIcon.sizeOffset_Y = 20;
      PlayerLifeUI.staminaIcon.texture = (Texture) PlayerLifeUI.icons.load("Stamina");
      PlayerLifeUI.lifeBox.add((Sleek) PlayerLifeUI.staminaIcon);
      PlayerLifeUI.staminaProgress = new SleekProgress(string.Empty);
      PlayerLifeUI.staminaProgress.positionOffset_X = 30;
      PlayerLifeUI.staminaProgress.positionOffset_Y = 130;
      PlayerLifeUI.staminaProgress.sizeOffset_X = -40;
      PlayerLifeUI.staminaProgress.sizeOffset_Y = 10;
      PlayerLifeUI.staminaProgress.sizeScale_X = 1f;
      PlayerLifeUI.staminaProgress.color = Palette.COLOR_Y;
      PlayerLifeUI.lifeBox.add((Sleek) PlayerLifeUI.staminaProgress);
      PlayerLifeUI.waveLabel = new SleekLabel();
      PlayerLifeUI.waveLabel.positionOffset_Y = 60;
      PlayerLifeUI.waveLabel.sizeOffset_Y = 30;
      PlayerLifeUI.waveLabel.sizeScale_X = 0.5f;
      PlayerLifeUI.lifeBox.add((Sleek) PlayerLifeUI.waveLabel);
      PlayerLifeUI.waveLabel.isVisible = false;
      PlayerLifeUI.scoreLabel = new SleekLabel();
      PlayerLifeUI.scoreLabel.positionOffset_Y = 60;
      PlayerLifeUI.scoreLabel.positionScale_X = 0.5f;
      PlayerLifeUI.scoreLabel.sizeOffset_Y = 30;
      PlayerLifeUI.scoreLabel.sizeScale_X = 0.5f;
      PlayerLifeUI.lifeBox.add((Sleek) PlayerLifeUI.scoreLabel);
      PlayerLifeUI.scoreLabel.isVisible = false;
      PlayerLifeUI.oxygenBox = new SleekBoxIcon((Texture2D) PlayerLifeUI.icons.load("Oxygen"));
      PlayerLifeUI.oxygenBox.positionOffset_Y = -30;
      PlayerLifeUI.oxygenBox.positionScale_X = 0.8f;
      PlayerLifeUI.oxygenBox.positionScale_Y = 1f;
      PlayerLifeUI.oxygenBox.sizeOffset_Y = 30;
      PlayerLifeUI.oxygenBox.sizeScale_X = 0.2f;
      PlayerLifeUI.container.add((Sleek) PlayerLifeUI.oxygenBox);
      PlayerLifeUI.oxygenBox.isVisible = false;
      PlayerLifeUI.oxygenProgress = new SleekProgress(string.Empty);
      PlayerLifeUI.oxygenProgress.positionOffset_X = 30;
      PlayerLifeUI.oxygenProgress.positionOffset_Y = 10;
      PlayerLifeUI.oxygenProgress.sizeOffset_X = -40;
      PlayerLifeUI.oxygenProgress.sizeOffset_Y = 10;
      PlayerLifeUI.oxygenProgress.sizeScale_X = 1f;
      PlayerLifeUI.oxygenProgress.color = Palette.COLOR_W;
      PlayerLifeUI.oxygenBox.add((Sleek) PlayerLifeUI.oxygenProgress);
      PlayerLifeUI.vehicleBox = new SleekBox();
      PlayerLifeUI.vehicleBox.positionOffset_Y = -60;
      PlayerLifeUI.vehicleBox.positionScale_X = 0.8f;
      PlayerLifeUI.vehicleBox.positionScale_Y = 1f;
      PlayerLifeUI.vehicleBox.sizeOffset_Y = 60;
      PlayerLifeUI.vehicleBox.sizeScale_X = 0.2f;
      PlayerLifeUI.container.add((Sleek) PlayerLifeUI.vehicleBox);
      PlayerLifeUI.vehicleBox.isVisible = false;
      PlayerLifeUI.fuelIcon = new SleekImageTexture();
      PlayerLifeUI.fuelIcon.positionOffset_X = 5;
      PlayerLifeUI.fuelIcon.positionOffset_Y = 5;
      PlayerLifeUI.fuelIcon.sizeOffset_X = 20;
      PlayerLifeUI.fuelIcon.sizeOffset_Y = 20;
      PlayerLifeUI.fuelIcon.texture = (Texture) PlayerLifeUI.icons.load("Fuel");
      PlayerLifeUI.vehicleBox.add((Sleek) PlayerLifeUI.fuelIcon);
      PlayerLifeUI.fuelProgress = new SleekProgress(string.Empty);
      PlayerLifeUI.fuelProgress.positionOffset_X = 30;
      PlayerLifeUI.fuelProgress.positionOffset_Y = 10;
      PlayerLifeUI.fuelProgress.sizeOffset_X = -40;
      PlayerLifeUI.fuelProgress.sizeOffset_Y = 10;
      PlayerLifeUI.fuelProgress.sizeScale_X = 1f;
      PlayerLifeUI.fuelProgress.color = Palette.COLOR_Y;
      PlayerLifeUI.vehicleBox.add((Sleek) PlayerLifeUI.fuelProgress);
      PlayerLifeUI.speedIcon = new SleekImageTexture();
      PlayerLifeUI.speedIcon.positionOffset_X = 5;
      PlayerLifeUI.speedIcon.positionOffset_Y = 35;
      PlayerLifeUI.speedIcon.sizeOffset_X = 20;
      PlayerLifeUI.speedIcon.sizeOffset_Y = 20;
      PlayerLifeUI.speedIcon.texture = (Texture) PlayerLifeUI.icons.load("Speed");
      PlayerLifeUI.vehicleBox.add((Sleek) PlayerLifeUI.speedIcon);
      PlayerLifeUI.speedProgress = new SleekProgress(!OptionsSettings.metric ? " mph" : " kph");
      PlayerLifeUI.speedProgress.positionOffset_X = 30;
      PlayerLifeUI.speedProgress.positionOffset_Y = 40;
      PlayerLifeUI.speedProgress.sizeOffset_X = -40;
      PlayerLifeUI.speedProgress.sizeOffset_Y = 10;
      PlayerLifeUI.speedProgress.sizeScale_X = 1f;
      PlayerLifeUI.speedProgress.color = Palette.COLOR_P;
      PlayerLifeUI.vehicleBox.add((Sleek) PlayerLifeUI.speedProgress);
      PlayerLifeUI.bleedingBox = new SleekBoxIcon((Texture2D) PlayerLifeUI.icons.load("Bleeding"));
      PlayerLifeUI.bleedingBox.positionOffset_Y = -210;
      PlayerLifeUI.bleedingBox.positionScale_Y = 1f;
      PlayerLifeUI.bleedingBox.sizeOffset_X = 50;
      PlayerLifeUI.bleedingBox.sizeOffset_Y = 50;
      PlayerLifeUI.container.add((Sleek) PlayerLifeUI.bleedingBox);
      PlayerLifeUI.bleedingBox.isVisible = false;
      PlayerLifeUI.brokenBox = new SleekBoxIcon((Texture2D) PlayerLifeUI.icons.load("Broken"));
      PlayerLifeUI.brokenBox.positionOffset_Y = -210;
      PlayerLifeUI.brokenBox.positionScale_Y = 1f;
      PlayerLifeUI.brokenBox.sizeOffset_X = 50;
      PlayerLifeUI.brokenBox.sizeOffset_Y = 50;
      PlayerLifeUI.container.add((Sleek) PlayerLifeUI.brokenBox);
      PlayerLifeUI.brokenBox.isVisible = false;
      PlayerLifeUI.temperatureBox = new SleekBoxIcon((Texture2D) null);
      PlayerLifeUI.temperatureBox.positionOffset_Y = -210;
      PlayerLifeUI.temperatureBox.positionScale_Y = 1f;
      PlayerLifeUI.temperatureBox.sizeOffset_X = 50;
      PlayerLifeUI.temperatureBox.sizeOffset_Y = 50;
      PlayerLifeUI.container.add((Sleek) PlayerLifeUI.temperatureBox);
      PlayerLifeUI.temperatureBox.isVisible = false;
      PlayerLifeUI.moonBox = new SleekBoxIcon((Texture2D) PlayerLifeUI.icons.load("Moon"));
      PlayerLifeUI.moonBox.positionOffset_Y = -210;
      PlayerLifeUI.moonBox.positionScale_Y = 1f;
      PlayerLifeUI.moonBox.sizeOffset_X = 50;
      PlayerLifeUI.moonBox.sizeOffset_Y = 50;
      PlayerLifeUI.container.add((Sleek) PlayerLifeUI.moonBox);
      PlayerLifeUI.moonBox.isVisible = false;
      if (Level.info.type == ELevelType.HORDE)
      {
        PlayerLifeUI.foodIcon.isVisible = false;
        PlayerLifeUI.foodProgress.isVisible = false;
        PlayerLifeUI.waterIcon.isVisible = false;
        PlayerLifeUI.waterProgress.isVisible = false;
        PlayerLifeUI.virusIcon.isVisible = false;
        PlayerLifeUI.virusProgress.isVisible = false;
        PlayerLifeUI.waveLabel.isVisible = true;
        PlayerLifeUI.scoreLabel.isVisible = true;
        PlayerLifeUI.staminaIcon.positionOffset_Y = 35;
        PlayerLifeUI.staminaProgress.positionOffset_Y = 40;
        PlayerLifeUI.lifeBox.positionOffset_Y = -90;
        PlayerLifeUI.lifeBox.sizeOffset_Y = 90;
      }
      else
      {
        PlayerLifeUI.moonBox.isVisible = LightingManager.isFullMoon;
        PlayerLifeUI.updateIcons();
      }
      Player.player.life.onDamaged += new Damaged(PlayerLifeUI.onDamaged);
      Player.player.life.onHealthUpdated = new HealthUpdated(PlayerLifeUI.onHealthUpdated);
      Player.player.life.onFoodUpdated = new FoodUpdated(PlayerLifeUI.onFoodUpdated);
      Player.player.life.onWaterUpdated = new WaterUpdated(PlayerLifeUI.onWaterUpdated);
      Player.player.life.onVirusUpdated = new VirusUpdated(PlayerLifeUI.onVirusUpdated);
      Player.player.life.onStaminaUpdated = new StaminaUpdated(PlayerLifeUI.onStaminaUpdated);
      Player.player.life.onOxygenUpdated = new OxygenUpdated(PlayerLifeUI.onOxygenUpdated);
      Player.player.life.onBleedingUpdated = new BleedingUpdated(PlayerLifeUI.onBleedingUpdated);
      Player.player.life.onBrokenUpdated = new BrokenUpdated(PlayerLifeUI.onBrokenUpdated);
      Player.player.life.onTemperatureUpdated = new TemperatureUpdated(PlayerLifeUI.onTemperatureUpdated);
      Player.player.look.onPerspectiveUpdated += new PerspectiveUpdated(PlayerLifeUI.onPerspectiveUpdated);
      Player.player.movement.onSeated += new Seated(PlayerLifeUI.onSeated);
      Player.player.movement.onVehicleUpdated += new VehicleUpdated(PlayerLifeUI.onVehicleUpdated);
      Player.player.movement.onSafetyUpdated += new SafetyUpdated(PlayerLifeUI.onSafetyUpdated);
      Player.player.voice.onTalked = new Talked(PlayerLifeUI.onTalked);
      Player.player.skills.onExperienceUpdated += new ExperienceUpdated(PlayerLifeUI.onExperienceUpdated);
      LightingManager.onMoonUpdated += new MoonUpdated(PlayerLifeUI.onMoonUpdated);
      ZombieManager.onWaveUpdated += new WaveUpdated(PlayerLifeUI.onWaveUpdated);
      PlayerLifeUI.onListed();
      ChatManager.onListed += new Listed(PlayerLifeUI.onListed);
    }

    public static void open()
    {
      if (PlayerLifeUI.active)
      {
        PlayerLifeUI.close();
      }
      else
      {
        PlayerLifeUI.active = true;
        PlayerLifeUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!PlayerLifeUI.active)
        return;
      PlayerLifeUI.active = false;
      PlayerLifeUI.closeChat();
      PlayerLifeUI.closeGestures();
      PlayerLifeUI.container.lerpPositionScale(0.0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    public static void openChat()
    {
      if (PlayerLifeUI.chatting)
        return;
      PlayerLifeUI.chatting = true;
      PlayerLifeUI.chatField.text = string.Empty;
      PlayerLifeUI.chatField.lerpPositionOffset(100, 170, ESleekLerp.EXPONENTIAL, 20f);
      if (PlayerUI.chat == EChatMode.GLOBAL)
        PlayerLifeUI.modeBox.text = PlayerLifeUI.localization.format("Mode_Global");
      else if (PlayerUI.chat == EChatMode.LOCAL)
        PlayerLifeUI.modeBox.text = PlayerLifeUI.localization.format("Mode_Local");
      else if (PlayerUI.chat == EChatMode.GROUP)
        PlayerLifeUI.modeBox.text = PlayerLifeUI.localization.format("Mode_Group");
      else
        PlayerLifeUI.modeBox.text = "?";
      if (Player.player.channel.owner.isAdmin && !Provider.isServer)
      {
        PlayerLifeUI.modeBox.backgroundColor = Palette.ADMIN;
        PlayerLifeUI.modeBox.foregroundColor = Palette.ADMIN;
        PlayerLifeUI.chatField.backgroundColor = Palette.ADMIN;
        PlayerLifeUI.chatField.foregroundColor = Palette.ADMIN;
      }
      else if (Provider.isPro)
      {
        PlayerLifeUI.modeBox.backgroundColor = Palette.PRO;
        PlayerLifeUI.modeBox.foregroundColor = Palette.PRO;
        PlayerLifeUI.chatField.backgroundColor = Palette.PRO;
        PlayerLifeUI.chatField.foregroundColor = Palette.PRO;
      }
      else
      {
        PlayerLifeUI.modeBox.backgroundColor = Color.white;
        PlayerLifeUI.modeBox.foregroundColor = Color.white;
        PlayerLifeUI.chatField.backgroundColor = Color.white;
        PlayerLifeUI.chatField.foregroundColor = Color.white;
      }
      PlayerLifeUI.chatBox.state = new Vector2(0.0f, float.MaxValue);
      PlayerLifeUI.chatBox.isVisible = true;
      for (int index = 0; index < 4; ++index)
        PlayerLifeUI.gameLabel[index].isVisible = false;
    }

    public static void closeChat()
    {
      if (!PlayerLifeUI.chatting)
        return;
      PlayerLifeUI.chatting = false;
      PlayerLifeUI.chatField.lerpPositionOffset(-350, 170, ESleekLerp.EXPONENTIAL, 20f);
      PlayerLifeUI.chatBox.isVisible = false;
      for (int index = 0; index < 4; ++index)
        PlayerLifeUI.gameLabel[index].isVisible = true;
    }

    public static void openGestures()
    {
      if (PlayerLifeUI.gesturing)
        return;
      PlayerLifeUI.gesturing = true;
      PlayerLifeUI.surrenderButton.isVisible = true;
      PlayerLifeUI.pointButton.isVisible = true;
      PlayerLifeUI.waveButton.isVisible = true;
    }

    public static void closeGestures()
    {
      if (!PlayerLifeUI.gesturing)
        return;
      PlayerLifeUI.gesturing = false;
      PlayerLifeUI.surrenderButton.isVisible = false;
      PlayerLifeUI.pointButton.isVisible = false;
      PlayerLifeUI.waveButton.isVisible = false;
    }

    private static void onDamaged(byte damage)
    {
      if ((int) damage <= 5)
        return;
      PlayerUI.pain(Mathf.Clamp((float) damage / 40f, 0.0f, 1f));
    }

    public static void updateGrayscale()
    {
      GrayscaleEffect component1 = Player.player.animator.view.GetComponent<GrayscaleEffect>();
      GrayscaleEffect component2 = Camera.main.GetComponent<GrayscaleEffect>();
      GrayscaleEffect component3 = Player.player.look.characterCamera.GetComponent<GrayscaleEffect>();
      if (Player.player.look.perspective == EPlayerPerspective.FIRST)
      {
        component1.enabled = true;
        component2.enabled = false;
      }
      else
      {
        component1.enabled = false;
        component2.enabled = true;
      }
      component1.blend = LevelLighting.vision != ELightingVision.CIVILIAN ? ((int) Player.player.life.health >= 50 ? 0.0f : (float) ((1.0 - (double) Player.player.life.health / 50.0) * (1.0 - (double) Player.player.skills.mastery(1, 3) * 0.75))) : 1f;
      component2.blend = component1.blend;
      component3.blend = component1.blend;
    }

    private static void onPerspectiveUpdated(EPlayerPerspective newPerspective)
    {
      PlayerLifeUI.updateGrayscale();
    }

    private static void onHealthUpdated(byte newHealth)
    {
      PlayerLifeUI.healthProgress.state = (float) newHealth / 100f;
      PlayerLifeUI.onPerspectiveUpdated(Player.player.look.perspective);
    }

    private static void onFoodUpdated(byte newFood)
    {
      PlayerLifeUI.foodProgress.state = (float) newFood / 100f;
    }

    private static void onWaterUpdated(byte newWater)
    {
      PlayerLifeUI.waterProgress.state = (float) newWater / 100f;
    }

    private static void onVirusUpdated(byte newVirus)
    {
      PlayerLifeUI.virusProgress.state = (float) newVirus / 100f;
    }

    private static void onStaminaUpdated(byte newStamina)
    {
      PlayerLifeUI.staminaProgress.state = (float) newStamina / 100f;
    }

    private static void onOxygenUpdated(byte newOxygen)
    {
      PlayerLifeUI.oxygenProgress.state = (float) newOxygen / 100f;
      PlayerLifeUI.oxygenBox.isVisible = (int) newOxygen != 100;
    }

    private static void updateIcons()
    {
      int num = 0;
      if (PlayerLifeUI.bleedingBox.isVisible)
        num += 60;
      PlayerLifeUI.brokenBox.positionOffset_X = num;
      if (PlayerLifeUI.brokenBox.isVisible)
        num += 60;
      PlayerLifeUI.temperatureBox.positionOffset_X = num;
      if (PlayerLifeUI.temperatureBox.isVisible)
        num += 60;
      PlayerLifeUI.moonBox.positionOffset_X = num;
    }

    private static void onBleedingUpdated(bool newBleeding)
    {
      PlayerLifeUI.bleedingBox.isVisible = newBleeding;
      PlayerLifeUI.updateIcons();
    }

    private static void onBrokenUpdated(bool newBroken)
    {
      PlayerLifeUI.brokenBox.isVisible = newBroken;
      PlayerLifeUI.updateIcons();
    }

    private static void onTemperatureUpdated(EPlayerTemperature newTemperature)
    {
      PlayerLifeUI.temperatureBox.isVisible = newTemperature != EPlayerTemperature.NONE;
      switch (newTemperature)
      {
        case EPlayerTemperature.FREEZING:
          PlayerLifeUI.temperatureBox.icon = (Texture2D) PlayerLifeUI.icons.load("Freezing");
          break;
        case EPlayerTemperature.COLD:
          PlayerLifeUI.temperatureBox.icon = (Texture2D) PlayerLifeUI.icons.load("Cold");
          break;
        case EPlayerTemperature.WARM:
          PlayerLifeUI.temperatureBox.icon = (Texture2D) PlayerLifeUI.icons.load("Warm");
          break;
        case EPlayerTemperature.BURNING:
          PlayerLifeUI.temperatureBox.icon = (Texture2D) PlayerLifeUI.icons.load("Burning");
          break;
        case EPlayerTemperature.COVERED:
          PlayerLifeUI.temperatureBox.icon = (Texture2D) PlayerLifeUI.icons.load("Covered");
          break;
        default:
          PlayerLifeUI.temperatureBox.icon = (Texture2D) null;
          break;
      }
      PlayerLifeUI.updateIcons();
    }

    private static void onMoonUpdated(bool isFullMoon)
    {
      PlayerLifeUI.moonBox.isVisible = isFullMoon;
      PlayerLifeUI.updateIcons();
    }

    private static void onExperienceUpdated(uint newExperience)
    {
      PlayerLifeUI.scoreLabel.text = PlayerLifeUI.localization.format("Score", (object) newExperience.ToString());
    }

    private static void onWaveUpdated(bool newWaveReady, int newWaveIndex)
    {
      PlayerLifeUI.waveLabel.text = PlayerLifeUI.localization.format("Round", (object) newWaveIndex);
      if (newWaveReady)
        PlayerUI.message(EPlayerMessage.WAVE_ON, string.Empty);
      else
        PlayerUI.message(EPlayerMessage.WAVE_OFF, string.Empty);
    }

    private static void onSeated(bool isDriver, bool inVehicle, bool wasVehicle)
    {
      PlayerLifeUI.vehicleBox.isVisible = isDriver && inVehicle;
    }

    private static void onVehicleUpdated(bool isDriveable, ushort newFuel, ushort maxFuel, float newSpeed, float minSpeed, float maxSpeed)
    {
      if (isDriveable)
      {
        PlayerLifeUI.fuelProgress.state = (float) newFuel / (float) maxFuel;
        float num1 = Mathf.Clamp(newSpeed, minSpeed, maxSpeed);
        float num2 = (double) num1 <= 0.0 ? num1 / minSpeed : num1 / maxSpeed;
        PlayerLifeUI.speedProgress.state = num2;
        PlayerLifeUI.speedProgress.measure = !OptionsSettings.metric ? (int) MeasurementTool.KPHToMPH(MeasurementTool.speedToKPH(Mathf.Abs(newSpeed))) : (int) MeasurementTool.speedToKPH(Mathf.Abs(newSpeed));
      }
      PlayerLifeUI.vehicleBox.isVisible = isDriveable;
    }

    private static void onSafetyUpdated(bool isSafe)
    {
      if (isSafe)
        PlayerUI.message(EPlayerMessage.SAFEZONE_ON, string.Empty);
      else
        PlayerUI.message(EPlayerMessage.SAFEZONE_OFF, string.Empty);
    }

    private static void onTalked(bool isTalking)
    {
      PlayerLifeUI.voiceBox.isVisible = isTalking;
    }

    private static void onListed()
    {
      for (int index = 0; index < ChatManager.chat.Length; ++index)
      {
        Chat chat = ChatManager.chat[index];
        if (chat != null)
        {
          PlayerLifeUI.chatLabel[index].foregroundColor = chat.color;
          if (chat.mode == EChatMode.GLOBAL)
            PlayerLifeUI.chatLabel[index].text = PlayerLifeUI.localization.format("Chat_Global", (object) chat.speaker, (object) chat.text);
          else if (chat.mode == EChatMode.LOCAL)
            PlayerLifeUI.chatLabel[index].text = PlayerLifeUI.localization.format("Chat_Local", (object) chat.speaker, (object) chat.text);
          else if (chat.mode == EChatMode.GROUP)
            PlayerLifeUI.chatLabel[index].text = PlayerLifeUI.localization.format("Chat_Group", (object) chat.speaker, (object) chat.text);
          else if (chat.mode == EChatMode.WELCOME)
            PlayerLifeUI.chatLabel[index].text = PlayerLifeUI.localization.format("Chat_Welcome", (object) chat.speaker, (object) chat.text);
          else if (chat.mode == EChatMode.SAY)
            PlayerLifeUI.chatLabel[index].text = PlayerLifeUI.localization.format("Chat_Say", (object) chat.speaker, (object) chat.text);
          else
            PlayerLifeUI.chatLabel[index].text = "?";
        }
      }
      for (int index = 0; index < 4; ++index)
      {
        PlayerLifeUI.gameLabel[index].foregroundColor = PlayerLifeUI.chatLabel[index].foregroundColor;
        PlayerLifeUI.gameLabel[index].text = PlayerLifeUI.chatLabel[index].text;
      }
    }

    private static void onClickedSurrenderButton(SleekButton button)
    {
      if (Player.player.animator.gesture == EPlayerGesture.SURRENDER_START)
        Player.player.animator.sendGesture(EPlayerGesture.SURRENDER_STOP, true);
      else
        Player.player.animator.sendGesture(EPlayerGesture.SURRENDER_START, true);
      PlayerLifeUI.closeGestures();
    }

    private static void onClickedPointButton(SleekButton button)
    {
      Player.player.animator.sendGesture(EPlayerGesture.POINT, true);
      PlayerLifeUI.closeGestures();
    }

    private static void onClickedWaveButton(SleekButton button)
    {
      Player.player.animator.sendGesture(EPlayerGesture.WAVE, true);
      PlayerLifeUI.closeGestures();
    }
  }
}
