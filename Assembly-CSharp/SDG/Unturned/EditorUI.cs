// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorUI : MonoBehaviour
  {
    public static readonly float MESSAGE_TIME = 2f;
    public static readonly float HINT_TIME = 0.15f;
    public static SleekWindow window;
    private static SleekBox messageBox;
    private static float lastMessage;
    private static bool isMessaged;
    private static bool lastHinted;
    private static bool isHinted;

    public static void hint(EEditorMessage message, string text)
    {
      if (EditorUI.isMessaged)
        return;
      EditorUI.messageBox.isVisible = true;
      EditorUI.lastHinted = true;
      EditorUI.isHinted = true;
      if (message != EEditorMessage.FOCUS)
        return;
      EditorUI.messageBox.text = text;
    }

    public static void message(EEditorMessage message)
    {
      if (!OptionsSettings.hints)
        return;
      EditorUI.messageBox.isVisible = true;
      EditorUI.lastMessage = Time.realtimeSinceStartup;
      EditorUI.isMessaged = true;
      if (message == EEditorMessage.HEIGHTS)
        EditorUI.messageBox.text = EditorDashboardUI.localization.format("Heights", (object) ControlsSettings.tool_2);
      else if (message == EEditorMessage.ROADS)
        EditorUI.messageBox.text = EditorDashboardUI.localization.format("Roads", (object) ControlsSettings.tool_1, (object) ControlsSettings.tool_2);
      else if (message == EEditorMessage.NAVIGATION)
        EditorUI.messageBox.text = EditorDashboardUI.localization.format("Navigation", (object) ControlsSettings.tool_2);
      else if (message == EEditorMessage.OBJECTS)
        EditorUI.messageBox.text = EditorDashboardUI.localization.format("Objects", (object) ControlsSettings.other, (object) ControlsSettings.tool_2, (object) ControlsSettings.tool_2);
      else if (message == EEditorMessage.NODES)
      {
        EditorUI.messageBox.text = EditorDashboardUI.localization.format("Nodes", (object) ControlsSettings.tool_2);
      }
      else
      {
        if (message != EEditorMessage.VISIBILITY)
          return;
        EditorUI.messageBox.text = EditorDashboardUI.localization.format("Visibility");
      }
    }

    private void OnGUI()
    {
      if (EditorUI.window == null)
        return;
      EditorUI.window.draw(false);
    }

    private void Update()
    {
      if (EditorUI.window == null)
        return;
      if (Input.GetKeyDown(KeyCode.Escape))
        EditorPauseUI.open();
      if (Input.GetKeyDown(ControlsSettings.hud))
        EditorUI.window.isEnabled = !EditorUI.window.isEnabled;
      if (Input.GetKeyDown(KeyCode.Insert))
        Assets.refresh();
      EditorUI.window.showCursor = !EditorInteract.isFlying;
      EditorUI.window.updateDebug();
      if (EditorUI.isMessaged)
      {
        if ((double) Time.realtimeSinceStartup - (double) EditorUI.lastMessage <= (double) EditorUI.MESSAGE_TIME)
          return;
        EditorUI.isMessaged = false;
        if (EditorUI.isHinted)
          return;
        EditorUI.messageBox.isVisible = false;
      }
      else
      {
        if (!EditorUI.isHinted)
          return;
        if (!EditorUI.lastHinted)
        {
          EditorUI.isHinted = false;
          EditorUI.messageBox.isVisible = false;
        }
        EditorUI.lastHinted = false;
      }
    }

    private void Start()
    {
      EditorUI.window = new SleekWindow();
      EditorUI.messageBox = new SleekBox();
      EditorUI.messageBox.positionOffset_X = -150;
      EditorUI.messageBox.positionOffset_Y = -60;
      EditorUI.messageBox.positionScale_X = 0.5f;
      EditorUI.messageBox.positionScale_Y = 1f;
      EditorUI.messageBox.sizeOffset_X = 300;
      EditorUI.messageBox.sizeOffset_Y = 50;
      EditorUI.messageBox.fontSize = 14;
      EditorUI.window.add((Sleek) EditorUI.messageBox);
      EditorUI.messageBox.isVisible = false;
      OptionsSettings.apply();
      GraphicsSettings.apply();
      EditorDashboardUI editorDashboardUi = new EditorDashboardUI();
    }
  }
}
