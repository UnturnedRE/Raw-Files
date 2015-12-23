// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Chat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class Chat
  {
    public EChatMode mode;
    public Color color;
    public string speaker;
    public string text;

    public Chat(EChatMode newMode, Color newColor, string newSpeaker, string newText)
    {
      this.mode = newMode;
      this.color = newColor;
      this.speaker = newSpeaker;
      this.text = newText;
    }
  }
}
