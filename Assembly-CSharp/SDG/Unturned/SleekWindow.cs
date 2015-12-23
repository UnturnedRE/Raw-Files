// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SleekWindow : Sleek
  {
    private static SleekWindow _active;
    public ClickedMouse onClickedMouse;
    public ClickedMouseStarted onClickedMouseStarted;
    public ClickedMouseStopped onClickedMouseStopped;
    public MovedMouse onMovedMouse;
    public bool showCursor;
    public bool isEnabled;
    private GUISkin style;
    private Rect cursorRect;
    private Rect tooltipRect;
    private Rect debugRect;
    private Texture cursor;
    private string lastTooltip;
    private float startedTooltip;
    private float _mouse_x;
    private float _mouse_y;
    private int fps;
    private float lastFrame;
    private int frames;

    public static SleekWindow active
    {
      get
      {
        return SleekWindow._active;
      }
    }

    public float mouse_x
    {
      get
      {
        return this._mouse_x;
      }
    }

    public float mouse_y
    {
      get
      {
        return (float) Screen.height - this._mouse_y;
      }
    }

    public SleekWindow()
    {
      SleekWindow._active = this;
      Cursor.visible = false;
      this.style = !Provider.isPro ? (GUISkin) Resources.Load("UI/Free/Skin") : (GUISkin) Resources.Load("UI/Pro/Skin");
      this.cursor = (Texture) Resources.Load("UI/Cursor");
      this.showCursor = true;
      this.isEnabled = true;
      this.cursorRect = new Rect(0.0f, 0.0f, 20f, 20f);
      this.tooltipRect = new Rect(0.0f, 0.0f, 400f, 60f);
      this.debugRect = new Rect(0.0f, 0.0f, 800f, 30f);
      this.init();
      this.sizeScale_X = 1f;
      this.sizeScale_Y = 1f;
      this.fps = 0;
      this.frames = 0;
      this.lastFrame = Time.realtimeSinceStartup;
      SleekRender.allowInput = true;
    }

    public void updateDebug()
    {
      ++this.frames;
      if ((double) Time.realtimeSinceStartup - (double) this.lastFrame <= 1.0)
        return;
      this.fps = (int) ((double) this.frames / ((double) Time.realtimeSinceStartup - (double) this.lastFrame));
      this.lastFrame = Time.realtimeSinceStartup;
      this.frames = 0;
    }

    public override void draw(bool ignoreCulling)
    {
      Cursor.visible = false;
      GUI.skin = this.style;
      if (this.isEnabled)
      {
        if ((double) Input.mousePosition.x != (double) this._mouse_x || (double) Input.mousePosition.y != (double) this._mouse_y)
        {
          this._mouse_x = Input.mousePosition.x - this.frame.x;
          this._mouse_y = Input.mousePosition.y - this.frame.y;
          if (this.onMovedMouse != null)
            this.onMovedMouse(this.mouse_x, this.mouse_y);
        }
        this.update();
        this.drawChildren(ignoreCulling);
        if (this.showCursor)
        {
          this.cursorRect.x = Input.mousePosition.x;
          this.cursorRect.y = (float) Screen.height - Input.mousePosition.y;
          if ((Object) this.cursor != (Object) null)
          {
            GUI.color = OptionsSettings.cursorColor;
            GUI.DrawTexture(this.cursorRect, this.cursor);
            GUI.color = Color.white;
          }
          if (Event.current.type == EventType.Repaint)
          {
            if (GUI.tooltip != this.lastTooltip)
            {
              this.lastTooltip = GUI.tooltip;
              this.startedTooltip = Time.realtimeSinceStartup;
            }
            if (GUI.tooltip != string.Empty && (double) Time.realtimeSinceStartup - (double) this.startedTooltip > 0.5)
            {
              this.tooltipRect.y = (float) ((double) Screen.height - (double) Input.mousePosition.y - 30.0);
              if ((double) Input.mousePosition.x > (double) Screen.width - (double) this.tooltipRect.width - 30.0)
              {
                this.tooltipRect.x = Input.mousePosition.x - 30f - this.tooltipRect.width;
                SleekRender.drawLabel(this.tooltipRect, FontStyle.Bold, TextAnchor.MiddleRight, 12, false, SleekRender.tooltip, GUI.tooltip);
              }
              else
              {
                this.tooltipRect.x = Input.mousePosition.x + 30f;
                SleekRender.drawLabel(this.tooltipRect, FontStyle.Bold, TextAnchor.MiddleLeft, 12, false, SleekRender.tooltip, GUI.tooltip);
              }
            }
          }
          Cursor.lockState = CursorLockMode.None;
        }
        else
          Cursor.lockState = CursorLockMode.Locked;
      }
      if (Event.current.type == EventType.MouseDown)
      {
        if (this.onClickedMouse != null)
          this.onClickedMouse();
        if (this.onClickedMouseStarted != null)
          this.onClickedMouseStarted();
      }
      else if (Event.current.type == EventType.MouseUp && this.onClickedMouseStopped != null)
        this.onClickedMouseStopped();
      if (!OptionsSettings.debug || this.fps == 0)
        return;
      if (Provider.isConnected)
      {
        if ((Object) Player.player != (Object) null && Player.player.channel.owner.isAdmin)
          SleekRender.drawLabel(this.debugRect, FontStyle.Normal, TextAnchor.UpperLeft, 12, 0 != 0, Color.green, (string) (object) this.fps + (object) "/s - " + (string) (object) (int) ((double) Provider.ping * 1000.0) + "ms - " + (string) (object) Provider.bytesSent + " " + (string) (object) Provider.bytesReceived + " " + (string) (object) Provider.packetsSent + " " + (string) (object) Provider.packetsReceived + " " + (!Player.player.look.isOrbiting ? "F1" : "Orbiting") + " " + (!Player.player.look.isTracking ? "F2" : "Tracking") + " " + (!Player.player.look.isLocking ? "F3" : "Locking") + " " + (!Player.player.look.isFocusing ? "F4" : "Focusing") + " + Shift");
        else
          SleekRender.drawLabel(this.debugRect, FontStyle.Normal, TextAnchor.UpperLeft, 12, 0 != 0, Color.green, (string) (object) this.fps + (object) "/s - " + (string) (object) (int) ((double) Provider.ping * 1000.0) + "ms - " + (string) (object) Provider.bytesSent + " " + (string) (object) Provider.bytesReceived + " " + (string) (object) Provider.packetsSent + " " + (string) (object) Provider.packetsReceived);
      }
      else
        SleekRender.drawLabel(this.debugRect, FontStyle.Normal, TextAnchor.UpperLeft, 12, false, Color.green, (string) (object) this.fps + (object) "/s");
    }
  }
}
