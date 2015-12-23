// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.LoadingUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class LoadingUI : MonoBehaviour
  {
    private static bool _isInitialized;
    public static SleekWindow window;
    private static Local localization;
    private static SleekImageTexture backgroundImage;
    private static SleekBox loadingBox;
    private static SleekImageTexture loadingImage;
    private static SleekLabel loadingLabel;
    private static float lastLoading;

    public static bool isInitialized
    {
      get
      {
        return LoadingUI._isInitialized;
      }
    }

    public static bool isBlocked
    {
      get
      {
        return (double) Time.realtimeSinceStartup - (double) LoadingUI.lastLoading < 0.100000001490116;
      }
    }

    public static void updateProgress(float progress)
    {
      if (Dedicator.isDedicated || LoadingUI.loadingImage == null)
        return;
      LoadingUI.loadingImage.sizeScale_X = progress;
      LoadingUI.loadingImage.sizeOffset_X = (int) (-20.0 * (double) progress);
    }

    public static void updateLabel(string key)
    {
      if (Dedicator.isDedicated || LoadingUI.loadingLabel == null)
        return;
      LoadingUI.loadingLabel.text = LoadingUI.localization.format(key);
    }

    public static void updateScene()
    {
      if (Dedicator.isDedicated || LoadingUI.backgroundImage == null || LoadingUI.loadingImage == null)
        return;
      LoadingUI.updateProgress(0.0f);
      if (Level.info != null && ReadWrite.fileExists(Level.info.path + "/Level.png", false, false))
      {
        byte[] data = ReadWrite.readBytes(Level.info.path + "/Level.png", false, false);
        Texture2D texture2D = new Texture2D(1280, 720, TextureFormat.ARGB32, false, true);
        texture2D.name = "Texture";
        texture2D.hideFlags = HideFlags.HideAndDontSave;
        texture2D.LoadImage(data);
        LoadingUI.backgroundImage.texture = (Texture) texture2D;
        Local local = Localization.tryRead(Level.info.path, false);
        if (local != null)
        {
          if (Provider.isConnected)
          {
            LoadingUI.loadingLabel.text = local.format("Loading_Server", (object) Provider.currentServerInfo.name, (object) (!Provider.currentServerInfo.isSecure ? local.format("Insecure") : local.format("Secure")));
            if (Provider.mode == EGameMode.EASY)
            {
              LoadingUI.loadingImage.backgroundColor = Palette.COLOR_G;
              LoadingUI.loadingImage.foregroundColor = Palette.COLOR_G;
            }
            else if (Provider.mode == EGameMode.HARD)
            {
              LoadingUI.loadingImage.backgroundColor = Palette.COLOR_R;
              LoadingUI.loadingImage.foregroundColor = Palette.COLOR_R;
            }
            else if (Provider.mode == EGameMode.PRO)
            {
              LoadingUI.loadingImage.backgroundColor = Palette.PRO;
              LoadingUI.loadingImage.foregroundColor = Palette.PRO;
            }
            else
            {
              LoadingUI.loadingImage.backgroundColor = Color.white;
              LoadingUI.loadingImage.foregroundColor = Color.white;
            }
          }
          else
          {
            LoadingUI.loadingLabel.text = local.format("Loading_Editor");
            LoadingUI.loadingImage.backgroundColor = Color.white;
            LoadingUI.loadingImage.foregroundColor = Color.white;
          }
        }
        else
        {
          LoadingUI.loadingLabel.text = string.Empty;
          LoadingUI.loadingImage.backgroundColor = Color.white;
          LoadingUI.loadingImage.foregroundColor = Color.white;
        }
      }
      else
      {
        LoadingUI.backgroundImage.texture = (Texture) Resources.Load("UI/Loading/Images/Background");
        LoadingUI.localization = Localization.read("/Menu/MenuLoading.dat");
        LoadingUI.loadingLabel.text = LoadingUI.localization.format("Loading");
        LoadingUI.loadingImage.backgroundColor = Color.white;
        LoadingUI.loadingImage.foregroundColor = Color.white;
      }
    }

    public static void rebuild()
    {
      LoadingUI.window.build();
    }

    private void OnGUI()
    {
      if (!Dedicator.isDedicated && (Application.isLoadingLevel || Assets.isLoading || (Provider.isLoading || Level.isLoading) || Player.isLoading))
        LoadingUI.lastLoading = Time.realtimeSinceStartup;
      if (!LoadingUI.isBlocked)
        return;
      LoadingUI.window.draw(false);
    }

    private void Awake()
    {
      if (LoadingUI.isInitialized)
      {
        Object.Destroy((Object) this.gameObject);
      }
      else
      {
        LoadingUI._isInitialized = true;
        Object.DontDestroyOnLoad((Object) this.gameObject);
      }
    }

    private void Start()
    {
      if (!Dedicator.isDedicated)
      {
        LoadingUI.window = new SleekWindow();
        LoadingUI.backgroundImage = new SleekImageTexture();
        LoadingUI.backgroundImage.sizeScale_X = 1f;
        LoadingUI.backgroundImage.sizeScale_Y = 1f;
        LoadingUI.window.add((Sleek) LoadingUI.backgroundImage);
        LoadingUI.loadingBox = new SleekBox();
        LoadingUI.loadingBox.positionOffset_X = 10;
        LoadingUI.loadingBox.positionOffset_Y = -60;
        LoadingUI.loadingBox.positionScale_Y = 1f;
        LoadingUI.loadingBox.sizeOffset_X = -20;
        LoadingUI.loadingBox.sizeOffset_Y = 50;
        LoadingUI.loadingBox.sizeScale_X = 1f;
        LoadingUI.window.add((Sleek) LoadingUI.loadingBox);
        LoadingUI.loadingImage = new SleekImageTexture();
        LoadingUI.loadingImage.positionOffset_X = 10;
        LoadingUI.loadingImage.positionOffset_Y = 10;
        LoadingUI.loadingImage.sizeOffset_X = -20;
        LoadingUI.loadingImage.sizeOffset_Y = -20;
        LoadingUI.loadingImage.sizeScale_X = 1f;
        LoadingUI.loadingImage.sizeScale_Y = 1f;
        LoadingUI.loadingImage.texture = (Texture) Resources.Load("Materials/Pixel");
        LoadingUI.loadingBox.add((Sleek) LoadingUI.loadingImage);
        LoadingUI.loadingLabel = new SleekLabel();
        LoadingUI.loadingLabel.positionOffset_X = 10;
        LoadingUI.loadingLabel.positionOffset_Y = -15;
        LoadingUI.loadingLabel.positionScale_Y = 0.5f;
        LoadingUI.loadingLabel.sizeOffset_X = -20;
        LoadingUI.loadingLabel.sizeOffset_Y = 30;
        LoadingUI.loadingLabel.sizeScale_X = 1f;
        LoadingUI.loadingLabel.fontSize = 14;
        LoadingUI.loadingBox.add((Sleek) LoadingUI.loadingLabel);
        LoadingUI.updateScene();
      }
      else
        Object.Destroy((Object) this.gameObject);
    }
  }
}
