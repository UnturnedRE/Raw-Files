// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class PlayerUI : MonoBehaviour
  {
    public static readonly float MESSAGE_TIME = 2f;
    public static readonly float HIT_TIME = 0.15f;
    public static SleekWindow window;
    public static Sleek container;
    private static PlayerUI ui;
    private static SleekBox messageBox;
    private static SleekLabel messageLabel;
    private static SleekProgress messageProgress;
    private static SleekImageTexture messageIcon;
    private static SleekImageTexture messageQualityImage;
    private static SleekLabel messageAmountLabel;
    private static SleekBox messageBox2;
    private static SleekLabel messageLabel2;
    public static bool isLocked;
    private BlurEffect blur;
    private AudioReverbZone zone;
    private TwirlEffect twirl;
    private Vignetting vignetting;
    private ColorCorrectionCurves colors;
    private Fisheye fish;
    private MotionBlur motion;
    private ContrastEnhance contrast;
    private float twirlScale;
    private float twirlSize;
    private float twirlSpeed;
    private float vignetteScale;
    private float vignetteSize;
    private float vignetteSpeed;
    private float blurScale;
    private float blurSize;
    private float blurSpeed;
    private float spreadScale;
    private float spreadSize;
    private float spreadSpeed;
    private float chromaScale;
    private float chromaSize;
    private float chromaSpeed;
    private float fishScale;
    private float fishSize_X;
    private float fishSize_Y;
    private float fishSpeed;
    private float motionScale;
    private float motionSize;
    private float motionSpeed;
    private float contrastScale;
    private float contrastSize;
    private float contrastSpeed;
    private static float lastHit;
    private static EPlayerHit hit;
    private static float lastMessage;
    private static bool isMessaged;
    private static bool lastHinted;
    private static bool isHinted;
    private static bool lastHinted2;
    private static bool isHinted2;
    private static bool isOverlayed;
    private static bool isReverting;
    private static EChatMode _chat;

    public static EChatMode chat
    {
      get
      {
        return PlayerUI._chat;
      }
    }

    public static void rebuild()
    {
      PlayerUI.ui.Invoke("init", 0.1f);
    }

    public void build()
    {
      PlayerUI.window.build();
      LoadingUI.rebuild();
    }

    public void init()
    {
      GraphicsSettings.resize();
      this.Invoke("build", 0.1f);
    }

    public static void pain(float amount)
    {
      PlayerLifeUI.painImage.backgroundColor.a = amount * 0.75f;
    }

    public static void hitmark(EPlayerHit newHit)
    {
      if (!PlayerUI.window.isEnabled || PlayerUI.isOverlayed && !PlayerUI.isReverting)
        return;
      PlayerUI.lastHit = Time.realtimeSinceStartup;
      PlayerUI.hit = newHit;
      if (PlayerUI.hit != EPlayerHit.CRITICAL)
        return;
      Player.player.playSound((AudioClip) Resources.Load("Sounds/General/Hit"), 0.5f);
    }

    public static void enableDot()
    {
      PlayerLifeUI.dotImage.isVisible = true;
    }

    public static void disableDot()
    {
      PlayerLifeUI.dotImage.isVisible = false;
    }

    public static void updateScope(bool isScoped)
    {
      PlayerLifeUI.scopeImage.isVisible = isScoped;
      PlayerLifeUI.scopeOverlay.isVisible = isScoped;
      PlayerLifeUI.scopeLeftOverlay.isVisible = isScoped;
      PlayerLifeUI.scopeRightOverlay.isVisible = isScoped;
      PlayerLifeUI.scopeUpOverlay.isVisible = isScoped;
      PlayerLifeUI.scopeDownOverlay.isVisible = isScoped;
      PlayerUI.container.isVisible = !isScoped;
      PlayerUI.isOverlayed = isScoped;
      if (PlayerUI.isOverlayed)
      {
        PlayerUI.isReverting = PlayerUI.window.isEnabled;
        PlayerUI.window.isEnabled = true;
      }
      else
      {
        if (PlayerUI.window.isEnabled)
          return;
        PlayerUI.window.isEnabled = PlayerUI.isReverting;
      }
    }

    public static void updateBinoculars(bool isBinoculars)
    {
      PlayerLifeUI.binocularsOverlay.isVisible = isBinoculars;
      PlayerUI.container.isVisible = !isBinoculars;
      PlayerUI.isOverlayed = isBinoculars;
      if (PlayerUI.isOverlayed)
      {
        PlayerUI.isReverting = PlayerUI.window.isEnabled;
        PlayerUI.window.isEnabled = true;
      }
      else
      {
        if (PlayerUI.window.isEnabled)
          return;
        PlayerUI.window.isEnabled = PlayerUI.isReverting;
      }
    }

    public static void resetCrosshair()
    {
      if (Provider.mode == EGameMode.HARD)
        return;
      PlayerLifeUI.crosshairLeftImage.positionOffset_X = -4;
      PlayerLifeUI.crosshairLeftImage.positionOffset_Y = -4;
      PlayerLifeUI.crosshairRightImage.positionOffset_X = -4;
      PlayerLifeUI.crosshairRightImage.positionOffset_Y = -4;
      PlayerLifeUI.crosshairDownImage.positionOffset_X = -4;
      PlayerLifeUI.crosshairDownImage.positionOffset_Y = -4;
      PlayerLifeUI.crosshairUpImage.positionOffset_X = -4;
      PlayerLifeUI.crosshairUpImage.positionOffset_Y = -4;
    }

    public static void updateCrosshair(float spread)
    {
      if (Provider.mode == EGameMode.HARD)
        return;
      PlayerLifeUI.crosshairLeftImage.lerpPositionOffset((int) (-(double) spread * 400.0 - 4.0), -4, ESleekLerp.EXPONENTIAL, 10f);
      PlayerLifeUI.crosshairRightImage.lerpPositionOffset((int) ((double) spread * 400.0 - 4.0), -4, ESleekLerp.EXPONENTIAL, 10f);
      PlayerLifeUI.crosshairDownImage.lerpPositionOffset(-4, (int) ((double) spread * 400.0 - 4.0), ESleekLerp.EXPONENTIAL, 10f);
      PlayerLifeUI.crosshairUpImage.lerpPositionOffset(-4, (int) (-(double) spread * 400.0 - 4.0), ESleekLerp.EXPONENTIAL, 10f);
    }

    public static void enableCrosshair()
    {
      if (Provider.mode == EGameMode.HARD)
        return;
      PlayerLifeUI.crosshairLeftImage.isVisible = true;
      PlayerLifeUI.crosshairRightImage.isVisible = true;
      PlayerLifeUI.crosshairDownImage.isVisible = true;
      PlayerLifeUI.crosshairUpImage.isVisible = true;
    }

    public static void disableCrosshair()
    {
      if (Provider.mode == EGameMode.HARD)
        return;
      PlayerLifeUI.crosshairLeftImage.isVisible = false;
      PlayerLifeUI.crosshairRightImage.isVisible = false;
      PlayerLifeUI.crosshairDownImage.isVisible = false;
      PlayerLifeUI.crosshairUpImage.isVisible = false;
    }

    public static void hint(Transform transform, EPlayerMessage message)
    {
      PlayerUI.hint(transform, message, string.Empty, Color.white);
    }

    public static void hint(Transform transform, EPlayerMessage message, string text, Color color, params object[] objects)
    {
      if (PlayerUI.isMessaged)
        return;
      if ((Object) transform != (Object) null && (Object) Camera.main != (Object) null)
      {
        Vector3 vector3 = Camera.main.WorldToScreenPoint(transform.position);
        PlayerUI.messageBox.positionOffset_X = (int) ((double) vector3.x - 150.0);
        PlayerUI.messageBox.positionOffset_Y = (int) ((double) Screen.height - (double) vector3.y + 10.0);
        PlayerUI.messageBox.positionScale_X = 0.0f;
        PlayerUI.messageBox.positionScale_Y = 0.0f;
      }
      else
      {
        PlayerUI.messageBox.positionOffset_X = -150;
        PlayerUI.messageBox.positionOffset_Y = -25;
        PlayerUI.messageBox.positionScale_X = 0.5f;
        PlayerUI.messageBox.positionScale_Y = 0.9f;
      }
      PlayerUI.messageBox.isVisible = true;
      PlayerUI.lastHinted = true;
      PlayerUI.isHinted = true;
      if (message == EPlayerMessage.GENERATOR_ON || message == EPlayerMessage.GENERATOR_OFF || (message == EPlayerMessage.GROW || message == EPlayerMessage.VEHICLE_ENTER))
      {
        PlayerUI.messageBox.sizeOffset_Y = 70;
        PlayerUI.messageProgress.isVisible = true;
        PlayerUI.messageIcon.isVisible = true;
        if (message == EPlayerMessage.GENERATOR_ON || message == EPlayerMessage.GENERATOR_OFF)
        {
          InteractableGenerator interactableGenerator = (InteractableGenerator) PlayerInteract.interactable;
          PlayerUI.messageProgress.state = (float) interactableGenerator.fuel / (float) InteractableGenerator.FUEL;
          PlayerUI.messageIcon.texture = (Texture) PlayerLifeUI.icons.load("Fuel");
        }
        else if (message == EPlayerMessage.GROW)
        {
          InteractableFarm interactableFarm = (InteractableFarm) PlayerInteract.interactable;
          PlayerUI.messageProgress.state = (float) (Provider.time - interactableFarm.planted) / (float) interactableFarm.growth;
          PlayerUI.messageIcon.texture = (Texture) PlayerLifeUI.icons.load("Grow");
        }
        else if (message == EPlayerMessage.VEHICLE_ENTER)
        {
          InteractableVehicle interactableVehicle = (InteractableVehicle) PlayerInteract.interactable;
          PlayerUI.messageProgress.state = (float) interactableVehicle.fuel / (float) interactableVehicle.asset.fuel;
          PlayerUI.messageIcon.texture = (Texture) PlayerLifeUI.icons.load("Fuel");
        }
        PlayerUI.messageProgress.color = message != EPlayerMessage.GROW ? Palette.COLOR_Y : Palette.COLOR_G;
        PlayerUI.messageQualityImage.isVisible = false;
        PlayerUI.messageAmountLabel.isVisible = false;
      }
      else if (message == EPlayerMessage.ITEM && (((ItemAsset) objects[1]).showQuality || (int) ((Item) objects[0]).amount > 1))
      {
        PlayerUI.messageBox.sizeOffset_Y = 70;
        PlayerUI.messageQualityImage.backgroundColor = color;
        PlayerUI.messageQualityImage.foregroundColor = color;
        PlayerUI.messageAmountLabel.backgroundColor = color;
        PlayerUI.messageAmountLabel.foregroundColor = color;
        if (((ItemAsset) objects[1]).showQuality)
        {
          PlayerUI.messageAmountLabel.text = (string) (object) ((Item) objects[0]).quality + (object) "%";
          PlayerUI.messageQualityImage.isVisible = true;
          PlayerUI.messageAmountLabel.isVisible = true;
        }
        else
        {
          PlayerUI.messageAmountLabel.text = "x" + (object) ((Item) objects[0]).amount;
          PlayerUI.messageQualityImage.isVisible = false;
          PlayerUI.messageAmountLabel.isVisible = true;
        }
        PlayerUI.messageProgress.isVisible = false;
        PlayerUI.messageIcon.isVisible = false;
      }
      else
      {
        PlayerUI.messageBox.sizeOffset_Y = 50;
        PlayerUI.messageQualityImage.isVisible = false;
        PlayerUI.messageAmountLabel.isVisible = false;
        PlayerUI.messageProgress.isVisible = false;
        PlayerUI.messageIcon.isVisible = false;
      }
      if (message == EPlayerMessage.ITEM)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Item", (object) text, (object) ControlsSettings.interact);
      else if (message == EPlayerMessage.VEHICLE_ENTER)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Vehicle_Enter", (object) text, (object) ControlsSettings.interact);
      else if (message == EPlayerMessage.ENEMY)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Enemy", (object) text);
      else if (message == EPlayerMessage.DOOR_OPEN)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Door_Open", (object) ControlsSettings.interact);
      else if (message == EPlayerMessage.DOOR_CLOSE)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Door_Close", (object) ControlsSettings.interact);
      else if (message == EPlayerMessage.LOCKED)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Locked");
      else if (message == EPlayerMessage.BLOCKED)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Blocked");
      else if (message == EPlayerMessage.PILLAR)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Pillar");
      else if (message == EPlayerMessage.POST)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Post");
      else if (message == EPlayerMessage.ROOF)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Roof");
      else if (message == EPlayerMessage.WALL)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Wall");
      else if (message == EPlayerMessage.CORNER)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Corner");
      else if (message == EPlayerMessage.GROUND)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Ground");
      else if (message == EPlayerMessage.DOORWAY)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Doorway");
      else if (message == EPlayerMessage.DOORWAY)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Doorway");
      else if (message == EPlayerMessage.GARAGE)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Garage");
      else if (message == EPlayerMessage.BED_ON)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Bed_On", (object) ControlsSettings.interact, (object) text);
      else if (message == EPlayerMessage.BED_OFF)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Bed_Off", (object) ControlsSettings.interact, (object) text);
      else if (message == EPlayerMessage.BED_CLAIMED)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Bed_Claimed");
      else if (message == EPlayerMessage.BOUNDS)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Bounds");
      else if (message == EPlayerMessage.STORAGE)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Storage", (object) ControlsSettings.interact);
      else if (message == EPlayerMessage.FARM)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Farm", (object) ControlsSettings.interact);
      else if (message == EPlayerMessage.GROW)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Grow");
      else if (message == EPlayerMessage.SOIL)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Soil");
      else if (message == EPlayerMessage.FIRE_ON)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Fire_On", (object) ControlsSettings.interact);
      else if (message == EPlayerMessage.FIRE_OFF)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Fire_Off", (object) ControlsSettings.interact);
      else if (message == EPlayerMessage.FORAGE)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Forage", (object) ControlsSettings.interact);
      else if (message == EPlayerMessage.GENERATOR_ON)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Generator_On", (object) ControlsSettings.interact);
      else if (message == EPlayerMessage.GENERATOR_OFF)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Generator_Off", (object) ControlsSettings.interact);
      else if (message == EPlayerMessage.SPOT_ON)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Spot_On", (object) ControlsSettings.interact);
      else if (message == EPlayerMessage.SPOT_OFF)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Spot_Off", (object) ControlsSettings.interact);
      else if (message == EPlayerMessage.PURCHASE)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Purchase", objects[0], objects[1], (object) ControlsSettings.interact);
      else if (message == EPlayerMessage.POWER)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Power");
      else if (message == EPlayerMessage.USE)
        PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Use", (object) ControlsSettings.interact);
      PlayerUI.messageBox.backgroundColor = color;
      PlayerUI.messageBox.foregroundColor = color;
      PlayerUI.messageLabel.backgroundColor = color;
      PlayerUI.messageLabel.foregroundColor = color;
    }

    public static void hint2(EPlayerMessage message)
    {
      PlayerUI.messageBox2.isVisible = true;
      PlayerUI.lastHinted2 = true;
      PlayerUI.isHinted2 = true;
      if (message != EPlayerMessage.SALVAGE)
        return;
      PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Salvage", (object) ControlsSettings.interact);
    }

    public static void message(EPlayerMessage message, string text)
    {
      if (!OptionsSettings.hints)
        return;
      if (message == EPlayerMessage.NONE)
      {
        PlayerUI.messageBox.isVisible = false;
        PlayerUI.lastMessage = -999f;
        PlayerUI.isMessaged = false;
      }
      else
      {
        PlayerUI.messageBox.positionOffset_X = -150;
        PlayerUI.messageBox.positionOffset_Y = -25;
        PlayerUI.messageBox.positionScale_X = 0.5f;
        PlayerUI.messageBox.positionScale_Y = 0.9f;
        PlayerUI.messageBox.isVisible = true;
        PlayerUI.messageBox.sizeOffset_Y = 50;
        PlayerUI.messageProgress.isVisible = false;
        PlayerUI.messageIcon.isVisible = false;
        PlayerUI.messageQualityImage.isVisible = false;
        PlayerUI.messageAmountLabel.isVisible = false;
        PlayerUI.lastMessage = Time.realtimeSinceStartup;
        PlayerUI.isMessaged = true;
        if (message == EPlayerMessage.SPACE)
          PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Space");
        if (message == EPlayerMessage.RELOAD)
          PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Reload", (object) ControlsSettings.reload);
        else if (message == EPlayerMessage.SAFETY)
          PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Safety", (object) ControlsSettings.firemode);
        else if (message == EPlayerMessage.VEHICLE_EXIT)
          PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Vehicle_Exit", (object) ControlsSettings.interact);
        else if (message == EPlayerMessage.VEHICLE_SWAP)
          PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Vehicle_Swap", (object) Player.player.movement.getVehicle().passengers.Length);
        else if (message == EPlayerMessage.LIGHT)
          PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Light", (object) ControlsSettings.tactical);
        else if (message == EPlayerMessage.LASER)
          PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Laser", (object) ControlsSettings.tactical);
        else if (message == EPlayerMessage.LASER)
          PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Laser", (object) ControlsSettings.tactical);
        else if (message == EPlayerMessage.RANGEFINDER)
          PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Rangefinder", (object) ControlsSettings.tactical);
        else if (message == EPlayerMessage.EXPERIENCE)
          PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Experience", (object) text);
        else if (message == EPlayerMessage.EMPTY)
          PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Empty");
        else if (message == EPlayerMessage.FULL)
          PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Full");
        else if (message == EPlayerMessage.MOON_ON)
          PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Moon_On");
        else if (message == EPlayerMessage.MOON_OFF)
          PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Moon_Off");
        else if (message == EPlayerMessage.SAFEZONE_ON)
          PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Safezone_On");
        else if (message == EPlayerMessage.SAFEZONE_OFF)
          PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Safezone_Off");
        else if (message == EPlayerMessage.WAVE_ON)
          PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Wave_On");
        else if (message == EPlayerMessage.WAVE_OFF)
          PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Wave_Off");
      }
      PlayerUI.messageBox.backgroundColor = Color.white;
      PlayerUI.messageBox.foregroundColor = Color.white;
      PlayerUI.messageLabel.backgroundColor = Color.white;
      PlayerUI.messageLabel.foregroundColor = Color.white;
    }

    private void onVisionUpdated(bool isViewing)
    {
      if (isViewing && (double) Random.value < 0.5)
      {
        float num = Random.value;
        this.zone.reverbPreset = (double) num >= 0.25 ? ((double) num >= 0.5 ? ((double) num >= 0.75 ? AudioReverbPreset.SewerPipe : AudioReverbPreset.Arena) : AudioReverbPreset.Psychotic) : AudioReverbPreset.Drugged;
        this.zone.enabled = true;
      }
      else
        this.zone.enabled = false;
      if (isViewing && (double) Random.value < 0.5)
      {
        this.twirlScale = Random.Range(2f, 8f);
        this.twirlSize = Random.Range(2f, 32f);
        this.twirlSpeed = Random.Range(0.25f, 1f);
        this.twirl.enabled = true;
      }
      else
      {
        this.twirl.enabled = false;
        this.twirl.angle = 0.0f;
      }
      if (isViewing && (double) Random.value < 0.5)
      {
        this.vignetteScale = Random.Range(2f, 8f);
        this.vignetteSize = Random.Range(0.0f, 16f);
        this.vignetteSpeed = Random.Range(0.25f, 1f);
        this.blurScale = Random.Range(2f, 8f);
        this.blurSize = Random.Range(0.0f, 64f);
        this.blurSpeed = Random.Range(0.25f, 1f);
        this.spreadScale = Random.Range(2f, 8f);
        this.spreadSize = Random.Range(0.0f, 2f);
        this.spreadSpeed = Random.Range(0.25f, 1f);
        this.chromaScale = Random.Range(2f, 8f);
        this.chromaSize = Random.Range(0.0f, 64f);
        this.chromaSpeed = Random.Range(0.25f, 1f);
        this.vignetting.enabled = true;
      }
      else
      {
        this.vignetting.enabled = false;
        this.vignetting.intensity = 0.0f;
        this.vignetting.blur = 0.0f;
        this.vignetting.blurSpread = 0.0f;
        this.vignetting.chromaticAberration = 0.0f;
      }
      if (isViewing && (double) Random.value < 0.5)
      {
        this.colors.saturation = Random.Range(1f, 2f);
        float num = Random.value;
        if ((double) num < 0.25)
        {
          this.colors.redChannel = AnimationCurve.Linear(0.0f, Random.Range(0.0f, 1f), 1f, Random.Range(0.0f, 1f));
          this.colors.greenChannel = AnimationCurve.Linear(0.0f, 0.0f, 1f, 1f);
          this.colors.blueChannel = AnimationCurve.Linear(0.0f, 0.0f, 1f, 1f);
        }
        else if ((double) num < 0.5)
        {
          this.colors.redChannel = AnimationCurve.Linear(0.0f, 0.0f, 1f, 1f);
          this.colors.greenChannel = AnimationCurve.Linear(0.0f, Random.Range(0.0f, 1f), 1f, Random.Range(0.0f, 1f));
          this.colors.blueChannel = AnimationCurve.Linear(0.0f, 0.0f, 1f, 1f);
        }
        else if ((double) num < 0.75)
        {
          this.colors.redChannel = AnimationCurve.Linear(0.0f, 0.0f, 1f, 1f);
          this.colors.greenChannel = AnimationCurve.Linear(0.0f, 0.0f, 1f, 1f);
          this.colors.blueChannel = AnimationCurve.Linear(0.0f, Random.Range(0.0f, 1f), 1f, Random.Range(0.0f, 1f));
        }
        else
        {
          this.colors.redChannel = AnimationCurve.Linear(0.0f, Random.Range(0.0f, 1f), 1f, Random.Range(0.0f, 1f));
          this.colors.greenChannel = AnimationCurve.Linear(0.0f, Random.Range(0.0f, 1f), 1f, Random.Range(0.0f, 1f));
          this.colors.blueChannel = AnimationCurve.Linear(0.0f, Random.Range(0.0f, 1f), 1f, Random.Range(0.0f, 1f));
        }
        this.colors.UpdateParameters();
        this.colors.enabled = true;
      }
      else
        this.colors.enabled = false;
      if (isViewing && (double) Random.value < 0.5)
      {
        this.fishScale = Random.Range(2f, 8f);
        this.fishSize_X = Random.Range(0.1f, 0.6f);
        this.fishSize_Y = Random.Range(0.1f, 0.6f);
        this.fishSpeed = Random.Range(0.25f, 1f);
        this.fish.enabled = true;
      }
      else
      {
        this.fish.enabled = false;
        this.fish.strengthX = 0.0f;
        this.fish.strengthY = 0.0f;
      }
      if (isViewing && (double) Random.value < 0.5)
      {
        this.motionScale = Random.Range(2f, 8f);
        this.motionSize = Random.Range(0.1f, 0.92f);
        this.motionSpeed = Random.Range(0.25f, 1f);
        this.motion.enabled = true;
      }
      else
      {
        this.motion.enabled = false;
        this.motion.blurAmount = 0.0f;
      }
      if (isViewing && (double) Random.value < 0.5)
      {
        this.contrastScale = Random.Range(2f, 8f);
        this.contrastSize = Random.Range(-3f, 3f);
        this.contrastSpeed = Random.Range(0.25f, 1f);
        this.contrast.enabled = true;
      }
      else
      {
        this.contrast.enabled = false;
        this.contrast.intensity = 0.0f;
      }
    }

    private void onLifeUpdated(bool isDead)
    {
      PlayerUI.isLocked = false;
      if (isDead)
      {
        PlayerLifeUI.close();
        PlayerDashboardUI.close();
        PlayerBarricadeSignUI.close();
        PlayerDeathUI.open();
      }
      else
        PlayerDeathUI.close();
    }

    private void onMoonUpdated(bool isFullMoon)
    {
      if (isFullMoon)
        PlayerUI.message(EPlayerMessage.MOON_ON, string.Empty);
      else
        PlayerUI.message(EPlayerMessage.MOON_OFF, string.Empty);
    }

    private void OnGUI()
    {
      if (PlayerUI.window == null)
        return;
      if (Event.current.isKey && Event.current.type == EventType.KeyUp)
      {
        if (Event.current.keyCode == KeyCode.Return)
        {
          if (PlayerLifeUI.chatting)
          {
            if (PlayerLifeUI.chatField.text != string.Empty)
              ChatManager.sendChat(PlayerUI.chat, PlayerLifeUI.chatField.text);
            PlayerLifeUI.closeChat();
          }
          else if (PlayerLifeUI.active && !PlayerUI.window.showCursor)
            PlayerLifeUI.openChat();
        }
        else if (Event.current.keyCode == ControlsSettings.global)
        {
          if (PlayerLifeUI.active && !PlayerUI.window.showCursor && !PlayerLifeUI.chatting)
          {
            PlayerUI._chat = EChatMode.GLOBAL;
            PlayerLifeUI.openChat();
          }
        }
        else if (Event.current.keyCode == ControlsSettings.local)
        {
          if (PlayerLifeUI.active && !PlayerUI.window.showCursor && !PlayerLifeUI.chatting)
          {
            PlayerUI._chat = EChatMode.LOCAL;
            PlayerLifeUI.openChat();
          }
        }
        else if (Event.current.keyCode == ControlsSettings.group && PlayerLifeUI.active && (!PlayerUI.window.showCursor && !PlayerLifeUI.chatting))
        {
          PlayerUI._chat = EChatMode.GROUP;
          PlayerLifeUI.openChat();
        }
      }
      if (PlayerLifeUI.chatting)
        GUI.SetNextControlName("Chat");
      PlayerUI.window.draw(false);
      if (PlayerLifeUI.chatting && GUI.GetNameOfFocusedControl() != "Chat")
        GUI.FocusControl("Chat");
      MenuConfigurationControlsUI.update();
    }

    private void Update()
    {
      if (PlayerUI.window == null)
        return;
      if (Characters.active.group != CSteamID.Nil)
      {
        for (int index = 0; index < PlayerGroupUI.groups.Count; ++index)
        {
          SleekLabel sleekLabel = PlayerGroupUI.groups[index];
          SteamPlayer steamPlayer = Provider.clients[index];
          if (steamPlayer.playerID.steamID != Provider.client && steamPlayer.playerID.group == Characters.active.group && (Object) steamPlayer.model != (Object) null)
          {
            Vector3 vector3 = Camera.main.WorldToScreenPoint(steamPlayer.model.position + Vector3.up * 3f);
            if ((double) vector3.z > 0.0 && (double) vector3.z < 1024.0)
            {
              sleekLabel.positionOffset_X = (int) ((double) vector3.x - 100.0);
              sleekLabel.positionOffset_Y = (int) ((double) Screen.height - (double) vector3.y - 15.0);
              sleekLabel.isVisible = true;
            }
            else
              sleekLabel.isVisible = false;
          }
          else
            sleekLabel.isVisible = false;
        }
      }
      PlayerLifeUI.painImage.backgroundColor.a = Mathf.Lerp(PlayerLifeUI.painImage.backgroundColor.a, 0.0f, 2f * Time.deltaTime);
      if (PlayerUI.hit == EPlayerHit.ENTITIY)
      {
        PlayerLifeUI.hitEntitiyImage.isVisible = (double) Time.realtimeSinceStartup - (double) PlayerUI.lastHit < (double) PlayerUI.HIT_TIME;
        PlayerLifeUI.hitCriticalImage.isVisible = false;
        PlayerLifeUI.hitBuildImage.isVisible = false;
      }
      else if (PlayerUI.hit == EPlayerHit.CRITICAL)
      {
        PlayerLifeUI.hitEntitiyImage.isVisible = false;
        PlayerLifeUI.hitCriticalImage.isVisible = (double) Time.realtimeSinceStartup - (double) PlayerUI.lastHit < (double) PlayerUI.HIT_TIME;
        PlayerLifeUI.hitBuildImage.isVisible = false;
      }
      else if (PlayerUI.hit == EPlayerHit.BUILD)
      {
        PlayerLifeUI.hitEntitiyImage.isVisible = false;
        PlayerLifeUI.hitCriticalImage.isVisible = false;
        PlayerLifeUI.hitBuildImage.isVisible = (double) Time.realtimeSinceStartup - (double) PlayerUI.lastHit < (double) PlayerUI.HIT_TIME;
      }
      if (PlayerUI.isMessaged)
      {
        if ((double) Time.realtimeSinceStartup - (double) PlayerUI.lastMessage > (double) PlayerUI.MESSAGE_TIME)
        {
          PlayerUI.isMessaged = false;
          if (!PlayerUI.isHinted)
            PlayerUI.messageBox.isVisible = false;
        }
      }
      else if (PlayerUI.isHinted)
      {
        if (!PlayerUI.lastHinted)
        {
          PlayerUI.isHinted = false;
          PlayerUI.messageBox.isVisible = false;
        }
        PlayerUI.lastHinted = false;
      }
      if (PlayerUI.isHinted2)
      {
        if (!PlayerUI.lastHinted2)
        {
          PlayerUI.isHinted2 = false;
          PlayerUI.messageBox2.isVisible = false;
        }
        PlayerUI.lastHinted2 = false;
      }
      Time.timeScale = !Provider.isServer || !MenuConfigurationOptionsUI.active && !MenuConfigurationDisplayUI.active && (!MenuConfigurationGraphicsUI.active && !MenuConfigurationControlsUI.active) && !PlayerPauseUI.active ? 1f : 0.0f;
      if ((int) MenuConfigurationControlsUI.binding == (int) byte.MaxValue)
      {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
          if (PlayerDashboardUI.active)
          {
            PlayerDashboardUI.close();
            if (!Player.player.life.isDead)
              PlayerLifeUI.open();
          }
          else if (!Player.player.life.isDead)
          {
            if (PlayerBarricadeSignUI.active)
            {
              PlayerBarricadeSignUI.close();
              PlayerLifeUI.open();
            }
            else if (MenuConfigurationOptionsUI.active)
            {
              MenuConfigurationOptionsUI.close();
              PlayerPauseUI.open();
            }
            else if (MenuConfigurationDisplayUI.active)
            {
              MenuConfigurationDisplayUI.close();
              PlayerPauseUI.open();
            }
            else if (MenuConfigurationGraphicsUI.active)
            {
              MenuConfigurationGraphicsUI.close();
              PlayerPauseUI.open();
            }
            else if (MenuConfigurationControlsUI.active)
            {
              MenuConfigurationControlsUI.close();
              PlayerPauseUI.open();
            }
            else if (PlayerPauseUI.active)
            {
              PlayerPauseUI.close();
              if (!Player.player.life.isDead)
                PlayerLifeUI.open();
            }
            else if (PlayerLifeUI.chatting)
            {
              PlayerLifeUI.closeChat();
            }
            else
            {
              PlayerLifeUI.close();
              PlayerDashboardUI.close();
              PlayerPauseUI.open();
            }
          }
        }
        if (PlayerDeathUI.active)
        {
          if (!Provider.isServer && Provider.isPvP)
          {
            if ((double) Time.realtimeSinceStartup - (double) Player.player.life.lastDeath < (double) PlayerLife.TIMER_HOME)
              PlayerDeathUI.homeButton.text = PlayerDeathUI.localization.format("Home_Button_Timer", (object) Mathf.Ceil(PlayerLife.TIMER_HOME - (Time.realtimeSinceStartup - Player.player.life.lastDeath)));
            else
              PlayerDeathUI.homeButton.text = PlayerDeathUI.localization.format("Home_Button");
          }
          else if ((double) Time.realtimeSinceStartup - (double) Player.player.life.lastRespawn < (double) PlayerLife.TIMER_RESPAWN)
            PlayerDeathUI.homeButton.text = PlayerDeathUI.localization.format("Home_Button_Timer", (object) Mathf.Ceil(PlayerLife.TIMER_RESPAWN - (Time.realtimeSinceStartup - Player.player.life.lastRespawn)));
          else
            PlayerDeathUI.homeButton.text = PlayerDeathUI.localization.format("Home_Button");
          if ((double) Time.realtimeSinceStartup - (double) Player.player.life.lastRespawn < (double) PlayerLife.TIMER_RESPAWN)
            PlayerDeathUI.respawnButton.text = PlayerDeathUI.localization.format("Respawn_Button_Timer", (object) Mathf.Ceil(PlayerLife.TIMER_RESPAWN - (Time.realtimeSinceStartup - Player.player.life.lastRespawn)));
          else
            PlayerDeathUI.respawnButton.text = PlayerDeathUI.localization.format("Respawn_Button");
        }
        if (PlayerPauseUI.active)
        {
          if (!Provider.isServer && Provider.isPvP && (double) Time.realtimeSinceStartup - (double) PlayerPauseUI.lastLeave < (double) PlayerPauseUI.TIMER_LEAVE)
            PlayerPauseUI.exitButton.text = PlayerPauseUI.localization.format("Exit_Button_Timer", (object) Mathf.Ceil(PlayerPauseUI.TIMER_LEAVE - (Time.realtimeSinceStartup - PlayerPauseUI.lastLeave)));
          else
            PlayerPauseUI.exitButton.text = PlayerPauseUI.localization.format("Exit_Button_Text");
        }
        if (!Player.player.life.isDead)
        {
          if (Input.GetKeyDown(ControlsSettings.dashboard))
          {
            if (PlayerDashboardUI.active)
            {
              PlayerDashboardUI.close();
              PlayerLifeUI.open();
            }
            else if (!PlayerUI.window.showCursor && !PlayerLifeUI.chatting)
            {
              PlayerLifeUI.close();
              PlayerPauseUI.close();
              PlayerDashboardUI.open();
            }
          }
          if (Input.GetKeyDown(ControlsSettings.inventory))
          {
            if (PlayerDashboardUI.active && PlayerDashboardInventoryUI.active)
            {
              PlayerDashboardUI.close();
              PlayerLifeUI.open();
            }
            else if (PlayerDashboardUI.active)
            {
              PlayerDashboardCraftingUI.close();
              PlayerDashboardSkillsUI.close();
              PlayerDashboardInformationUI.close();
              PlayerDashboardInventoryUI.open();
            }
            else if (!PlayerUI.window.showCursor && !PlayerLifeUI.chatting)
            {
              PlayerLifeUI.close();
              PlayerPauseUI.close();
              PlayerDashboardInventoryUI.active = true;
              PlayerDashboardCraftingUI.active = false;
              PlayerDashboardSkillsUI.active = false;
              PlayerDashboardInformationUI.active = false;
              PlayerDashboardUI.open();
            }
          }
          if (Input.GetKeyDown(ControlsSettings.crafting) && Level.info.type != ELevelType.HORDE)
          {
            if (PlayerDashboardUI.active && PlayerDashboardCraftingUI.active)
            {
              PlayerDashboardUI.close();
              PlayerLifeUI.open();
            }
            else if (PlayerDashboardUI.active)
            {
              PlayerDashboardInventoryUI.close();
              PlayerDashboardSkillsUI.close();
              PlayerDashboardInformationUI.close();
              PlayerDashboardCraftingUI.open();
            }
            else if (!PlayerUI.window.showCursor && !PlayerLifeUI.chatting)
            {
              PlayerLifeUI.close();
              PlayerPauseUI.close();
              PlayerDashboardInventoryUI.active = false;
              PlayerDashboardCraftingUI.active = true;
              PlayerDashboardSkillsUI.active = false;
              PlayerDashboardInformationUI.active = false;
              PlayerDashboardUI.open();
            }
          }
          if (Input.GetKeyDown(ControlsSettings.skills) && Level.info.type != ELevelType.HORDE)
          {
            if (PlayerDashboardUI.active && PlayerDashboardSkillsUI.active)
            {
              PlayerDashboardUI.close();
              PlayerLifeUI.open();
            }
            else if (PlayerDashboardUI.active)
            {
              PlayerDashboardInventoryUI.close();
              PlayerDashboardCraftingUI.close();
              PlayerDashboardInformationUI.close();
              PlayerDashboardSkillsUI.open();
            }
            else if (!PlayerUI.window.showCursor && !PlayerLifeUI.chatting)
            {
              PlayerLifeUI.close();
              PlayerPauseUI.close();
              PlayerDashboardInventoryUI.active = false;
              PlayerDashboardCraftingUI.active = false;
              PlayerDashboardSkillsUI.active = true;
              PlayerDashboardInformationUI.active = false;
              PlayerDashboardUI.open();
            }
          }
          if (Input.GetKeyDown(ControlsSettings.map) || Input.GetKeyDown(ControlsSettings.players))
          {
            if (PlayerDashboardUI.active && PlayerDashboardInformationUI.active)
            {
              PlayerDashboardUI.close();
              PlayerLifeUI.open();
            }
            else if (PlayerDashboardUI.active)
            {
              PlayerDashboardInventoryUI.close();
              PlayerDashboardCraftingUI.close();
              PlayerDashboardSkillsUI.close();
              PlayerDashboardInformationUI.open();
            }
            else if (!PlayerUI.window.showCursor && !PlayerLifeUI.chatting)
            {
              PlayerLifeUI.close();
              PlayerPauseUI.close();
              PlayerDashboardInventoryUI.active = false;
              PlayerDashboardCraftingUI.active = false;
              PlayerDashboardSkillsUI.active = false;
              PlayerDashboardInformationUI.active = true;
              PlayerDashboardUI.open();
            }
          }
          if (Input.GetKeyDown(ControlsSettings.gesture))
          {
            if (!Player.player.equipment.isSelected && Player.player.stance.stance != EPlayerStance.PRONE && (Player.player.stance.stance != EPlayerStance.SWIM && Player.player.stance.stance != EPlayerStance.DRIVING) && (Player.player.stance.stance != EPlayerStance.SITTING && PlayerLifeUI.active && (!PlayerUI.window.showCursor && !PlayerLifeUI.chatting)))
              PlayerLifeUI.openGestures();
          }
          else if (Input.GetKeyUp(ControlsSettings.gesture) && PlayerLifeUI.active)
            PlayerLifeUI.closeGestures();
        }
        if (Input.GetKeyDown(ControlsSettings.hud) && !PlayerUI.isOverlayed)
          PlayerUI.window.isEnabled = !PlayerUI.window.isEnabled;
        if (Input.GetKeyDown(KeyCode.Insert))
          Assets.refresh();
      }
      PlayerUI.window.showCursor = PlayerPauseUI.active || MenuConfigurationOptionsUI.active || (MenuConfigurationDisplayUI.active || MenuConfigurationGraphicsUI.active) || (MenuConfigurationControlsUI.active || PlayerDashboardUI.active || (PlayerDeathUI.active || PlayerLifeUI.chatting)) || (PlayerLifeUI.gesturing || PlayerBarricadeSignUI.active) || PlayerUI.isLocked;
      if (PlayerUI.window.showCursor && !MenuConfigurationGraphicsUI.active || ((double) Camera.main.transform.position.y < (double) LevelLighting.seaLevel * (double) Level.TERRAIN || Player.player.look.scopeCamera.enabled && (Object) Player.player.look.scopeCamera.targetTexture != (Object) null && (Player.player.look.perspective == EPlayerPerspective.FIRST && (Object) Player.player.equipment.useable != (Object) null) && ((UseableGun) Player.player.equipment.useable).isAiming))
      {
        if (!this.blur.enabled)
          this.blur.enabled = true;
      }
      else if (this.blur.enabled)
        this.blur.enabled = false;
      if (this.twirl.enabled)
        this.twirl.angle = Mathf.Lerp(this.twirl.angle, Mathf.Sin(Time.realtimeSinceStartup / this.twirlScale) * this.twirlSize, Time.deltaTime * this.twirlSpeed);
      if (this.vignetting.enabled)
      {
        this.vignetting.intensity = Mathf.Lerp(this.vignetting.intensity, Mathf.Sin(Time.realtimeSinceStartup / this.vignetteScale) * this.vignetteSize, Time.deltaTime * this.vignetteSpeed);
        this.vignetting.blur = Mathf.Lerp(this.vignetting.blur, Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup / this.blurScale)) * this.blurSize, Time.deltaTime * this.blurSpeed);
        this.vignetting.blurSpread = Mathf.Lerp(this.vignetting.blurSpread, Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup / this.spreadScale)) * this.spreadSize, Time.deltaTime * this.spreadSpeed);
        this.vignetting.chromaticAberration = Mathf.Lerp(this.vignetting.chromaticAberration, Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup / this.chromaScale)) * this.chromaSize, Time.deltaTime * this.chromaSpeed);
      }
      if (this.fish.enabled)
      {
        this.fish.strengthX = Mathf.Lerp(this.fish.strengthX, (float) (0.400000005960464 + (double) Mathf.Sin(Time.realtimeSinceStartup / this.fishScale) * (double) this.fishSize_X), Time.deltaTime * this.fishSpeed);
        this.fish.strengthY = Mathf.Lerp(this.fish.strengthY, (float) (0.400000005960464 + (double) Mathf.Cos(Time.realtimeSinceStartup / this.fishScale) * (double) this.fishSize_Y), Time.deltaTime * this.fishSpeed);
      }
      if (this.motion.enabled)
        this.motion.blurAmount = Mathf.Lerp(this.motion.blurAmount, Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup / this.motionScale)) * this.motionSize, Time.deltaTime * this.motionSpeed);
      if (this.contrast.enabled)
        this.contrast.intensity = Mathf.Lerp(this.contrast.intensity, Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup / this.contrastScale)) * this.contrastSize, Time.deltaTime * this.contrastSpeed);
      PlayerUI.window.updateDebug();
    }

    private void Start()
    {
      PlayerUI.isLocked = false;
      PlayerUI.lastHit = float.MinValue;
      PlayerUI._chat = EChatMode.GLOBAL;
      PlayerUI.window = new SleekWindow();
      PlayerUI.ui = this;
      PlayerUI.container = new Sleek();
      PlayerUI.container.sizeScale_X = 1f;
      PlayerUI.container.sizeScale_Y = 1f;
      PlayerUI.window.add(PlayerUI.container);
      GraphicsSettings.apply();
      PlayerGroupUI playerGroupUi = new PlayerGroupUI();
      PlayerDashboardUI playerDashboardUi = new PlayerDashboardUI();
      PlayerPauseUI playerPauseUi = new PlayerPauseUI();
      PlayerLifeUI playerLifeUi = new PlayerLifeUI();
      PlayerDeathUI playerDeathUi = new PlayerDeathUI();
      PlayerBarricadeSignUI playerBarricadeSignUi = new PlayerBarricadeSignUI();
      PlayerUI.messageBox = new SleekBox();
      PlayerUI.messageBox.positionOffset_X = -150;
      PlayerUI.messageBox.positionOffset_Y = -25;
      PlayerUI.messageBox.positionScale_X = 0.5f;
      PlayerUI.messageBox.positionScale_Y = 0.9f;
      PlayerUI.messageBox.sizeOffset_X = 300;
      PlayerUI.messageBox.sizeOffset_Y = 50;
      PlayerUI.container.add((Sleek) PlayerUI.messageBox);
      PlayerUI.messageBox.isVisible = false;
      PlayerUI.messageLabel = new SleekLabel();
      PlayerUI.messageLabel.positionOffset_X = 10;
      PlayerUI.messageLabel.positionOffset_Y = 10;
      PlayerUI.messageLabel.sizeOffset_X = -20;
      PlayerUI.messageLabel.sizeOffset_Y = 30;
      PlayerUI.messageLabel.sizeScale_X = 1f;
      PlayerUI.messageLabel.fontSize = 14;
      PlayerUI.messageBox.add((Sleek) PlayerUI.messageLabel);
      PlayerUI.messageIcon = new SleekImageTexture();
      PlayerUI.messageIcon.positionOffset_X = 5;
      PlayerUI.messageIcon.positionOffset_Y = 45;
      PlayerUI.messageIcon.sizeOffset_X = 20;
      PlayerUI.messageIcon.sizeOffset_Y = 20;
      PlayerUI.messageBox.add((Sleek) PlayerUI.messageIcon);
      PlayerUI.messageIcon.isVisible = false;
      PlayerUI.messageProgress = new SleekProgress(string.Empty);
      PlayerUI.messageProgress.positionOffset_X = 30;
      PlayerUI.messageProgress.positionOffset_Y = 50;
      PlayerUI.messageProgress.sizeOffset_X = -40;
      PlayerUI.messageProgress.sizeOffset_Y = 10;
      PlayerUI.messageProgress.sizeScale_X = 1f;
      PlayerUI.messageBox.add((Sleek) PlayerUI.messageProgress);
      PlayerUI.messageProgress.isVisible = false;
      PlayerUI.messageQualityImage = new SleekImageTexture((Texture) PlayerDashboardInventoryUI.icons.load("Quality_0"));
      PlayerUI.messageQualityImage.positionOffset_X = -30;
      PlayerUI.messageQualityImage.positionOffset_Y = -30;
      PlayerUI.messageQualityImage.positionScale_X = 1f;
      PlayerUI.messageQualityImage.positionScale_Y = 1f;
      PlayerUI.messageQualityImage.sizeOffset_X = 20;
      PlayerUI.messageQualityImage.sizeOffset_Y = 20;
      PlayerUI.messageBox.add((Sleek) PlayerUI.messageQualityImage);
      PlayerUI.messageQualityImage.isVisible = false;
      PlayerUI.messageAmountLabel = new SleekLabel();
      PlayerUI.messageAmountLabel.positionOffset_X = 10;
      PlayerUI.messageAmountLabel.positionOffset_Y = -40;
      PlayerUI.messageAmountLabel.positionScale_Y = 1f;
      PlayerUI.messageAmountLabel.sizeOffset_X = -20;
      PlayerUI.messageAmountLabel.sizeOffset_Y = 30;
      PlayerUI.messageAmountLabel.sizeScale_X = 1f;
      PlayerUI.messageAmountLabel.fontAlignment = TextAnchor.LowerLeft;
      PlayerUI.messageBox.add((Sleek) PlayerUI.messageAmountLabel);
      PlayerUI.messageAmountLabel.isVisible = false;
      PlayerUI.messageBox2 = new SleekBox();
      PlayerUI.messageBox2.positionOffset_X = -150;
      PlayerUI.messageBox2.positionOffset_Y = -85;
      PlayerUI.messageBox2.positionScale_X = 0.5f;
      PlayerUI.messageBox2.positionScale_Y = 0.9f;
      PlayerUI.messageBox2.sizeOffset_X = 300;
      PlayerUI.messageBox2.sizeOffset_Y = 50;
      PlayerUI.container.add((Sleek) PlayerUI.messageBox2);
      PlayerUI.messageBox2.isVisible = false;
      PlayerUI.messageLabel2 = new SleekLabel();
      PlayerUI.messageLabel2.positionOffset_X = 10;
      PlayerUI.messageLabel2.positionOffset_Y = 10;
      PlayerUI.messageLabel2.sizeOffset_X = -20;
      PlayerUI.messageLabel2.sizeOffset_Y = 30;
      PlayerUI.messageLabel2.sizeScale_X = 1f;
      PlayerUI.messageLabel2.fontSize = 14;
      PlayerUI.messageBox2.add((Sleek) PlayerUI.messageLabel2);
      Player.player.life.onVisionUpdated += new VisionUpdated(this.onVisionUpdated);
      Player.player.life.onLifeUpdated += new LifeUpdated(this.onLifeUpdated);
      LightingManager.onMoonUpdated += new MoonUpdated(this.onMoonUpdated);
      this.blur = this.GetComponent<BlurEffect>();
      this.zone = this.GetComponent<AudioReverbZone>();
      this.twirl = Player.player.animator.view.GetComponent<TwirlEffect>();
      this.vignetting = Player.player.animator.view.GetComponent<Vignetting>();
      this.colors = Player.player.animator.view.GetComponent<ColorCorrectionCurves>();
      this.fish = Player.player.animator.view.GetComponent<Fisheye>();
      this.motion = Player.player.animator.view.GetComponent<MotionBlur>();
      this.contrast = Player.player.animator.view.GetComponent<ContrastEnhance>();
    }
  }
}
