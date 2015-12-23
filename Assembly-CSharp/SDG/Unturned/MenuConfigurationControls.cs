// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MenuConfigurationControls
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class MenuConfigurationControls : MonoBehaviour
  {
    private static byte _binding;

    public static byte binding
    {
      get
      {
        return MenuConfigurationControls._binding;
      }
      set
      {
        MenuConfigurationControls._binding = value;
        SleekRender.allowInput = (int) MenuConfigurationControls.binding == (int) byte.MaxValue;
      }
    }

    private static void cancel()
    {
      MenuConfigurationControlsUI.cancel();
      MenuConfigurationControls.binding = byte.MaxValue;
    }

    private static void bind(KeyCode key)
    {
      MenuConfigurationControlsUI.bind(key);
      MenuConfigurationControls.binding = byte.MaxValue;
    }

    private void OnGUI()
    {
      if ((int) MenuConfigurationControls.binding == (int) byte.MaxValue)
        return;
      if (Event.current.type == EventType.KeyDown)
      {
        if (Event.current.keyCode == KeyCode.Backspace || Event.current.keyCode == KeyCode.Escape)
          MenuConfigurationControls.cancel();
        else
          MenuConfigurationControls.bind(Event.current.keyCode);
      }
      else if (Event.current.type == EventType.MouseDown)
      {
        if (Event.current.button == 0)
          MenuConfigurationControls.bind(KeyCode.Mouse0);
        else if (Event.current.button == 1)
          MenuConfigurationControls.bind(KeyCode.Mouse1);
        else if (Event.current.button == 2)
          MenuConfigurationControls.bind(KeyCode.Mouse2);
        else if (Event.current.button == 3)
          MenuConfigurationControls.bind(KeyCode.Mouse3);
        else if (Event.current.button == 4)
          MenuConfigurationControls.bind(KeyCode.Mouse4);
        else if (Event.current.button == 5)
        {
          MenuConfigurationControls.bind(KeyCode.Mouse5);
        }
        else
        {
          if (Event.current.button != 6)
            return;
          MenuConfigurationControls.bind(KeyCode.Mouse6);
        }
      }
      else
      {
        if (!Event.current.shift)
          return;
        MenuConfigurationControls.bind(KeyCode.LeftShift);
      }
    }

    private void Awake()
    {
      MenuConfigurationControls.binding = byte.MaxValue;
    }
  }
}
