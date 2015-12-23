// Decompiled with JetBrains decompiler
// Type: Pathfinding.Serialization.SerializeSettings
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Pathfinding.Serialization
{
  public class SerializeSettings
  {
    public bool nodes = true;
    public bool prettyPrint;
    public bool editorSettings;

    public static SerializeSettings Settings
    {
      get
      {
        return new SerializeSettings()
        {
          nodes = false
        };
      }
    }

    public static SerializeSettings All
    {
      get
      {
        return new SerializeSettings()
        {
          nodes = true
        };
      }
    }
  }
}
