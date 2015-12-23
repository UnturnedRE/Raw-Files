// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorMovement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorMovement : MonoBehaviour
  {
    public EditorRegionUpdated onRegionUpdated;
    public EditorBoundUpdated onBoundUpdated;
    private byte _region_x;
    private byte _region_y;
    private byte _bound;
    private static bool _isMoving;
    private Vector3 input;
    private CharacterController controller;

    public byte region_x
    {
      get
      {
        return this._region_x;
      }
    }

    public byte region_y
    {
      get
      {
        return this._region_y;
      }
    }

    public byte bound
    {
      get
      {
        return this._bound;
      }
    }

    public static bool isMoving
    {
      get
      {
        return EditorMovement._isMoving;
      }
    }

    private void FixedUpdate()
    {
      byte x;
      byte y;
      if (Regions.tryGetCoordinate(this.transform.position, out x, out y) && ((int) x != (int) this.region_x || (int) y != (int) this.region_y))
      {
        byte regionX = this.region_x;
        byte regionY = this.region_y;
        this._region_x = x;
        this._region_y = y;
        if (this.onRegionUpdated != null)
          this.onRegionUpdated(regionX, regionY, x, y);
      }
      byte bound1;
      LevelNavigation.tryGetBounds(this.transform.position, out bound1);
      if ((int) bound1 == (int) this.bound)
        return;
      byte bound2 = this.bound;
      this._bound = bound1;
      if (this.onBoundUpdated == null)
        return;
      this.onBoundUpdated(bound2, bound1);
    }

    private void Update()
    {
      if (EditorInteract.isFlying)
      {
        this.input.x = !Input.GetKey(ControlsSettings.left) ? (!Input.GetKey(ControlsSettings.right) ? 0.0f : 1f) : -1f;
        this.input.z = !Input.GetKey(ControlsSettings.up) ? (!Input.GetKey(ControlsSettings.down) ? 0.0f : -1f) : 1f;
        EditorMovement._isMoving = (double) this.input.x != 0.0 || (double) this.input.z != 0.0;
        int num = (int) this.controller.Move(Camera.main.transform.rotation * this.input * (!Input.GetKey(ControlsSettings.modify) ? 32f : 128f) * Time.deltaTime + (!Input.GetKey(ControlsSettings.modify) ? 32f : 128f) * Camera.main.transform.forward * Input.GetAxis("mouse_z"));
        Vector3 position = this.transform.position;
        position.x = Mathf.Clamp(position.x, (float) -Level.size, (float) Level.size);
        position.y = Mathf.Clamp(position.y, 0.0f, Level.HEIGHT);
        position.z = Mathf.Clamp(position.z, (float) -Level.size, (float) Level.size);
        this.transform.position = position;
      }
      LevelLighting.updateLocal(Camera.main.transform.position);
    }

    private void Start()
    {
      this._region_x = byte.MaxValue;
      this._region_y = byte.MaxValue;
      this._bound = byte.MaxValue;
      this.controller = this.transform.GetComponent<CharacterController>();
    }
  }
}
