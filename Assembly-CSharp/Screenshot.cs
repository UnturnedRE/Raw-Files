// Decompiled with JetBrains decompiler
// Type: Screenshot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Screenshot : MonoBehaviour
{
  private void Update()
  {
    if (!Input.GetKeyDown(KeyCode.F))
      return;
    Debug.Log((object) "A");
    Application.CaptureScreenshot("Screenshot.png", 4);
    Debug.Log((object) "B");
  }
}
