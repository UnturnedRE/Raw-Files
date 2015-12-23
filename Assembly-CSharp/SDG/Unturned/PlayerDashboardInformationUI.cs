// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerDashboardInformationUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class PlayerDashboardInformationUI
  {
    private static Local localization;
    public static Bundle icons;
    private static Sleek container;
    public static bool active;
    private static byte zoom;
    private static SleekBox backdropBox;
    private static bool isDragging;
    private static Vector2 dragOrigin;
    private static Vector2 dragOffset;
    private static Sleek mapInspect;
    private static SleekViewBox mapBox;
    private static SleekImageTexture mapImage;
    private static SleekButtonIcon zoomInButton;
    private static SleekButtonIcon zoomOutButton;
    private static SleekBox countBox;
    private static SleekScrollBox playersBox;

    public PlayerDashboardInformationUI()
    {
      if (PlayerDashboardInformationUI.icons != null)
        PlayerDashboardInformationUI.icons.unload();
      PlayerDashboardInformationUI.localization = Localization.read("/Player/PlayerDashboardInformation.dat");
      PlayerDashboardInformationUI.icons = Bundles.getBundle("/Bundles/Textures/Player/Icons/PlayerDashboardInformation/PlayerDashboardInformation.unity3d");
      PlayerDashboardInformationUI.container = new Sleek();
      PlayerDashboardInformationUI.container.positionScale_Y = 1f;
      PlayerDashboardInformationUI.container.positionOffset_X = 10;
      PlayerDashboardInformationUI.container.positionOffset_Y = 10;
      PlayerDashboardInformationUI.container.sizeOffset_X = -20;
      PlayerDashboardInformationUI.container.sizeOffset_Y = -20;
      PlayerDashboardInformationUI.container.sizeScale_X = 1f;
      PlayerDashboardInformationUI.container.sizeScale_Y = 1f;
      PlayerUI.container.add(PlayerDashboardInformationUI.container);
      PlayerDashboardInformationUI.active = false;
      PlayerDashboardInformationUI.zoom = (byte) 1;
      PlayerDashboardInformationUI.isDragging = false;
      PlayerDashboardInformationUI.dragOrigin = Vector2.zero;
      PlayerDashboardInformationUI.dragOffset = Vector2.zero;
      PlayerDashboardInformationUI.backdropBox = new SleekBox();
      PlayerDashboardInformationUI.backdropBox.positionOffset_Y = 60;
      PlayerDashboardInformationUI.backdropBox.sizeOffset_Y = -60;
      PlayerDashboardInformationUI.backdropBox.sizeScale_X = 1f;
      PlayerDashboardInformationUI.backdropBox.sizeScale_Y = 1f;
      PlayerDashboardInformationUI.backdropBox.backgroundColor = Palette.COLOR_W;
      PlayerDashboardInformationUI.backdropBox.backgroundColor.a = 0.5f;
      PlayerDashboardInformationUI.container.add((Sleek) PlayerDashboardInformationUI.backdropBox);
      PlayerDashboardInformationUI.mapInspect = new Sleek();
      PlayerDashboardInformationUI.mapInspect.positionOffset_X = 10;
      PlayerDashboardInformationUI.mapInspect.positionOffset_Y = 10;
      PlayerDashboardInformationUI.mapInspect.sizeOffset_X = -15;
      PlayerDashboardInformationUI.mapInspect.sizeOffset_Y = -20;
      PlayerDashboardInformationUI.mapInspect.sizeScale_X = 0.75f;
      PlayerDashboardInformationUI.mapInspect.sizeScale_Y = 1f;
      PlayerDashboardInformationUI.backdropBox.add(PlayerDashboardInformationUI.mapInspect);
      PlayerDashboardInformationUI.mapBox = new SleekViewBox();
      PlayerDashboardInformationUI.mapBox.sizeOffset_Y = -40;
      PlayerDashboardInformationUI.mapBox.sizeScale_X = 1f;
      PlayerDashboardInformationUI.mapBox.sizeScale_Y = 1f;
      PlayerDashboardInformationUI.mapBox.constraint = ESleekConstraint.XY;
      PlayerDashboardInformationUI.mapBox.constrain_X = 542;
      PlayerDashboardInformationUI.mapBox.constrain_Y = 542;
      PlayerDashboardInformationUI.mapInspect.add((Sleek) PlayerDashboardInformationUI.mapBox);
      PlayerDashboardInformationUI.mapImage = new SleekImageTexture();
      PlayerDashboardInformationUI.mapBox.add((Sleek) PlayerDashboardInformationUI.mapImage);
      PlayerDashboardInformationUI.updateZoom();
      if (ReadWrite.fileExists(Level.info.path + "/Map.png", false, false) && Provider.mode != EGameMode.HARD)
      {
        byte[] data = ReadWrite.readBytes(Level.info.path + "/Map.png", false, false);
        Texture2D texture2D = new Texture2D((int) Level.size / 2, (int) Level.size / 2, TextureFormat.ARGB32, false, true);
        texture2D.name = "Texture";
        texture2D.hideFlags = HideFlags.HideAndDontSave;
        texture2D.filterMode = FilterMode.Trilinear;
        texture2D.LoadImage(data);
        PlayerDashboardInformationUI.mapImage.texture = (Texture) texture2D;
      }
      else
        PlayerDashboardInformationUI.mapImage.texture = (Texture) Resources.Load("Level/Map");
      PlayerDashboardInformationUI.zoomInButton = new SleekButtonIcon((Texture2D) PlayerDashboardInformationUI.icons.load("Zoom_In"));
      PlayerDashboardInformationUI.zoomInButton.positionOffset_Y = -30;
      PlayerDashboardInformationUI.zoomInButton.positionScale_Y = 1f;
      PlayerDashboardInformationUI.zoomInButton.sizeOffset_X = -5;
      PlayerDashboardInformationUI.zoomInButton.sizeOffset_Y = 30;
      PlayerDashboardInformationUI.zoomInButton.sizeScale_X = 0.5f;
      PlayerDashboardInformationUI.zoomInButton.text = PlayerDashboardInformationUI.localization.format("Zoom_In_Button");
      PlayerDashboardInformationUI.zoomInButton.tooltip = PlayerDashboardInformationUI.localization.format("Zoom_In_Button_Tooltip");
      PlayerDashboardInformationUI.zoomInButton.onClickedButton = new ClickedButton(PlayerDashboardInformationUI.onClickedZoomInButton);
      PlayerDashboardInformationUI.mapInspect.add((Sleek) PlayerDashboardInformationUI.zoomInButton);
      PlayerDashboardInformationUI.zoomOutButton = new SleekButtonIcon((Texture2D) PlayerDashboardInformationUI.icons.load("Zoom_Out"));
      PlayerDashboardInformationUI.zoomOutButton.positionOffset_X = 5;
      PlayerDashboardInformationUI.zoomOutButton.positionOffset_Y = -30;
      PlayerDashboardInformationUI.zoomOutButton.positionScale_X = 0.5f;
      PlayerDashboardInformationUI.zoomOutButton.positionScale_Y = 1f;
      PlayerDashboardInformationUI.zoomOutButton.sizeOffset_X = -5;
      PlayerDashboardInformationUI.zoomOutButton.sizeOffset_Y = 30;
      PlayerDashboardInformationUI.zoomOutButton.sizeScale_X = 0.5f;
      PlayerDashboardInformationUI.zoomOutButton.text = PlayerDashboardInformationUI.localization.format("Zoom_Out_Button");
      PlayerDashboardInformationUI.zoomOutButton.tooltip = PlayerDashboardInformationUI.localization.format("Zoom_Out_Button_Tooltip");
      PlayerDashboardInformationUI.zoomOutButton.onClickedButton = new ClickedButton(PlayerDashboardInformationUI.onClickedZoomOutButton);
      PlayerDashboardInformationUI.mapInspect.add((Sleek) PlayerDashboardInformationUI.zoomOutButton);
      PlayerDashboardInformationUI.countBox = new SleekBox();
      PlayerDashboardInformationUI.countBox.positionOffset_X = 5;
      PlayerDashboardInformationUI.countBox.positionOffset_Y = 10;
      PlayerDashboardInformationUI.countBox.positionScale_X = 0.75f;
      PlayerDashboardInformationUI.countBox.sizeOffset_X = -15;
      PlayerDashboardInformationUI.countBox.sizeOffset_Y = 30;
      PlayerDashboardInformationUI.countBox.sizeScale_X = 0.25f;
      PlayerDashboardInformationUI.backdropBox.add((Sleek) PlayerDashboardInformationUI.countBox);
      PlayerDashboardInformationUI.playersBox = new SleekScrollBox();
      PlayerDashboardInformationUI.playersBox.positionOffset_X = 5;
      PlayerDashboardInformationUI.playersBox.positionOffset_Y = 50;
      PlayerDashboardInformationUI.playersBox.positionScale_X = 0.75f;
      PlayerDashboardInformationUI.playersBox.sizeOffset_X = -15;
      PlayerDashboardInformationUI.playersBox.sizeOffset_Y = -60;
      PlayerDashboardInformationUI.playersBox.sizeScale_X = 0.25f;
      PlayerDashboardInformationUI.playersBox.sizeScale_Y = 1f;
      PlayerDashboardInformationUI.backdropBox.add((Sleek) PlayerDashboardInformationUI.playersBox);
      PlayerUI.window.onClickedMouseStarted += new ClickedMouseStarted(PlayerDashboardInformationUI.onClickedMouseStarted);
      PlayerUI.window.onClickedMouseStopped += new ClickedMouseStopped(PlayerDashboardInformationUI.onClickedMouseStopped);
      PlayerUI.window.onMovedMouse += new MovedMouse(PlayerDashboardInformationUI.onMovedMouse);
    }

    public static void open()
    {
      if (PlayerDashboardInformationUI.active)
      {
        PlayerDashboardInformationUI.close();
      }
      else
      {
        PlayerDashboardInformationUI.active = true;
        if (Provider.mode != EGameMode.HARD)
        {
          PlayerDashboardInformationUI.mapImage.remove();
          for (int index = 0; index < LevelNodes.nodes.Count; ++index)
          {
            Node node = LevelNodes.nodes[index];
            if (node.type == ENodeType.LOCATION)
            {
              SleekLabel sleekLabel = new SleekLabel();
              sleekLabel.positionOffset_X = -200;
              sleekLabel.positionOffset_Y = -30;
              sleekLabel.positionScale_X = (float) ((double) node.point.x / (double) ((int) Level.size - (int) Level.border) + 0.5);
              sleekLabel.positionScale_Y = (float) (0.5 - (double) node.point.z / (double) ((int) Level.size - (int) Level.border));
              sleekLabel.sizeOffset_X = 400;
              sleekLabel.sizeOffset_Y = 60;
              sleekLabel.text = ((LocationNode) node).name;
              PlayerDashboardInformationUI.mapImage.add((Sleek) sleekLabel);
            }
          }
          if ((Object) Player.player != (Object) null)
          {
            SleekImageTexture sleekImageTexture = new SleekImageTexture();
            sleekImageTexture.positionOffset_X = -10;
            sleekImageTexture.positionOffset_Y = -10;
            sleekImageTexture.positionScale_X = (float) ((double) Player.player.transform.position.x / (double) ((int) Level.size - (int) Level.border) + 0.5);
            sleekImageTexture.positionScale_Y = (float) (0.5 - (double) Player.player.transform.position.z / (double) ((int) Level.size - (int) Level.border));
            sleekImageTexture.sizeOffset_X = 20;
            sleekImageTexture.sizeOffset_Y = 20;
            sleekImageTexture.isAngled = true;
            sleekImageTexture.angle = Player.player.transform.rotation.eulerAngles.y;
            sleekImageTexture.texture = (Texture) PlayerDashboardInformationUI.icons.load("Player");
            sleekImageTexture.addLabel(Characters.active.name, ESleekSide.RIGHT);
            PlayerDashboardInformationUI.mapImage.add((Sleek) sleekImageTexture);
          }
          if (Characters.active.group != CSteamID.Nil)
          {
            for (int index = 0; index < PlayerGroupUI.groups.Count; ++index)
            {
              SteamPlayer steamPlayer = Provider.clients[index];
              if (steamPlayer.playerID.steamID != Provider.client && steamPlayer.playerID.group == Characters.active.group && (Object) steamPlayer.model != (Object) null)
              {
                SleekImageTexture sleekImageTexture = new SleekImageTexture();
                sleekImageTexture.positionOffset_X = -10;
                sleekImageTexture.positionOffset_Y = -10;
                sleekImageTexture.positionScale_X = (float) ((double) steamPlayer.player.transform.position.x / (double) ((int) Level.size - (int) Level.border) + 0.5);
                sleekImageTexture.positionScale_Y = (float) (0.5 - (double) steamPlayer.player.transform.position.z / (double) ((int) Level.size - (int) Level.border));
                sleekImageTexture.sizeOffset_X = 20;
                sleekImageTexture.sizeOffset_Y = 20;
                sleekImageTexture.texture = (Texture) Provider.provider.communityService.getIcon(steamPlayer.playerID.steamID);
                if (steamPlayer.playerID.nickName == string.Empty)
                  sleekImageTexture.addLabel(steamPlayer.playerID.characterName, ESleekSide.RIGHT);
                else
                  sleekImageTexture.addLabel(steamPlayer.playerID.nickName, ESleekSide.RIGHT);
                PlayerDashboardInformationUI.mapImage.add((Sleek) sleekImageTexture);
              }
            }
          }
        }
        PlayerDashboardInformationUI.countBox.text = PlayerDashboardInformationUI.localization.format("Count", (object) Provider.clients.Count, (object) Provider.maxPlayers);
        PlayerDashboardInformationUI.playersBox.remove();
        for (int index = 0; index < Provider.clients.Count; ++index)
        {
          SleekPlayer sleekPlayer = new SleekPlayer(Provider.clients[index]);
          sleekPlayer.positionOffset_Y = index * 40;
          sleekPlayer.sizeOffset_X = -30;
          sleekPlayer.sizeOffset_Y = 30;
          sleekPlayer.sizeScale_X = 1f;
          PlayerDashboardInformationUI.playersBox.add((Sleek) sleekPlayer);
        }
        PlayerDashboardInformationUI.playersBox.area = new Rect(0.0f, 0.0f, 5f, (float) (Provider.clients.Count * 40 - 10));
        PlayerDashboardInformationUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!PlayerDashboardInformationUI.active)
        return;
      PlayerDashboardInformationUI.active = false;
      PlayerDashboardInformationUI.isDragging = false;
      PlayerDashboardInformationUI.container.lerpPositionScale(0.0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void updateZoom()
    {
      PlayerDashboardInformationUI.mapBox.area = new Rect(0.0f, 0.0f, (float) (512 * (int) PlayerDashboardInformationUI.zoom), (float) (512 * (int) PlayerDashboardInformationUI.zoom));
      PlayerDashboardInformationUI.mapImage.sizeOffset_X = 512 * (int) PlayerDashboardInformationUI.zoom;
      PlayerDashboardInformationUI.mapImage.sizeOffset_Y = 512 * (int) PlayerDashboardInformationUI.zoom;
    }

    private static void onClickedMouseStarted()
    {
      if ((double) PlayerUI.window.mouse_x <= (double) PlayerDashboardInformationUI.mapBox.frame.xMin || (double) PlayerUI.window.mouse_x >= (double) PlayerDashboardInformationUI.mapBox.frame.xMax || ((double) PlayerUI.window.mouse_y <= (double) PlayerDashboardInformationUI.mapBox.frame.yMin || (double) PlayerUI.window.mouse_y >= (double) PlayerDashboardInformationUI.mapBox.frame.yMax))
        return;
      PlayerDashboardInformationUI.isDragging = true;
      PlayerDashboardInformationUI.dragOrigin.x = PlayerUI.window.mouse_x;
      PlayerDashboardInformationUI.dragOrigin.y = PlayerUI.window.mouse_y;
      PlayerDashboardInformationUI.dragOffset.x = PlayerDashboardInformationUI.mapBox.state.x;
      PlayerDashboardInformationUI.dragOffset.y = PlayerDashboardInformationUI.mapBox.state.y;
    }

    private static void onClickedMouseStopped()
    {
      if (!PlayerDashboardInformationUI.isDragging)
        return;
      PlayerDashboardInformationUI.isDragging = false;
      PlayerDashboardInformationUI.dragOrigin = Vector2.zero;
      PlayerDashboardInformationUI.dragOffset = Vector2.zero;
    }

    private static void onMovedMouse(float x, float y)
    {
      if (!PlayerDashboardInformationUI.isDragging)
        return;
      PlayerDashboardInformationUI.mapBox.state.x = PlayerDashboardInformationUI.dragOffset.x - x + PlayerDashboardInformationUI.dragOrigin.x;
      PlayerDashboardInformationUI.mapBox.state.y = PlayerDashboardInformationUI.dragOffset.y - y + PlayerDashboardInformationUI.dragOrigin.y;
    }

    private static void onClickedZoomInButton(SleekButton button)
    {
      if ((int) PlayerDashboardInformationUI.zoom >= (int) Level.size / 1024)
        return;
      ++PlayerDashboardInformationUI.zoom;
      PlayerDashboardInformationUI.updateZoom();
      PlayerDashboardInformationUI.mapBox.state += new Vector2(256f, 256f);
      PlayerDashboardInformationUI.isDragging = false;
      PlayerDashboardInformationUI.dragOrigin = Vector2.zero;
      PlayerDashboardInformationUI.dragOffset = Vector2.zero;
    }

    private static void onClickedZoomOutButton(SleekButton button)
    {
      if ((int) PlayerDashboardInformationUI.zoom <= 1)
        return;
      --PlayerDashboardInformationUI.zoom;
      PlayerDashboardInformationUI.updateZoom();
      PlayerDashboardInformationUI.mapBox.state -= new Vector2(256f, 256f);
      PlayerDashboardInformationUI.isDragging = false;
      PlayerDashboardInformationUI.dragOrigin = Vector2.zero;
      PlayerDashboardInformationUI.dragOffset = Vector2.zero;
    }
  }
}
