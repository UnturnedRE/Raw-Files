// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorLook
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorLook : MonoBehaviour
  {
    private static float _pitch;
    private static float _yaw;
    private Camera highlightCamera;

    public static float pitch
    {
      get
      {
        return EditorLook._pitch;
      }
    }

    public static float yaw
    {
      get
      {
        return EditorLook._yaw;
      }
    }

    private void Update()
    {
      if (!EditorInteract.isFlying)
        return;
      Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, OptionsSettings.view + (!EditorMovement.isMoving || !Input.GetKey(ControlsSettings.modify) ? 0.0f : 10f), 8f * Time.deltaTime);
      this.highlightCamera.fieldOfView = Camera.main.fieldOfView;
      EditorLook._yaw += ControlsSettings.look * Input.GetAxis("mouse_x");
      if (ControlsSettings.invert)
        EditorLook._pitch += ControlsSettings.look * Input.GetAxis("mouse_y");
      else
        EditorLook._pitch -= ControlsSettings.look * Input.GetAxis("mouse_y");
      if ((double) EditorLook.pitch > 90.0)
        EditorLook._pitch = 90f;
      else if ((double) EditorLook.pitch < -90.0)
        EditorLook._pitch = -90f;
      Camera.main.transform.localRotation = Quaternion.Euler(EditorLook.pitch, 0.0f, 0.0f);
      this.transform.rotation = Quaternion.Euler(0.0f, EditorLook.yaw, 0.0f);
    }

    private void Start()
    {
      Camera.main.fieldOfView = OptionsSettings.view;
      this.highlightCamera = Camera.main.transform.FindChild("HighlightCamera").GetComponent<Camera>();
      this.highlightCamera.fieldOfView = OptionsSettings.view;
      EditorLook._pitch = Camera.main.transform.localRotation.eulerAngles.x;
      if ((double) EditorLook.pitch > 90.0)
        EditorLook._pitch = EditorLook.pitch - 360f;
      EditorLook._yaw = this.transform.rotation.eulerAngles.y;
      LevelLighting.updateLighting();
      Camera.main.transform.FindChild("HighlightCamera").GetComponent<AntialiasingAsPostEffect>().enabled = GraphicsSettings.antiAliasingType == EAntiAliasingType.FXAA;
    }
  }
}
