// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Editor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class Editor : MonoBehaviour
  {
    public static EditorCreated onEditorCreated;
    private static Editor _editor;
    private EditorMovement _movement;

    public static Editor editor
    {
      get
      {
        return Editor._editor;
      }
    }

    public EditorMovement movement
    {
      get
      {
        return this._movement;
      }
    }

    private void Start()
    {
      this._movement = this.GetComponent<EditorMovement>();
      Editor._editor = this;
      if (Editor.onEditorCreated == null)
        return;
      Editor.onEditorCreated();
    }

    public static void save()
    {
      EditorInteract.save();
      EditorTerrainHeight.save();
      EditorTerrainMaterials.save();
      EditorObjects.save();
      EditorSpawns.save();
    }
  }
}
