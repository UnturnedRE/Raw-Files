// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.LevelInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class LevelInfo
  {
    private string _path;
    private string _name;
    private ELevelSize _size;
    private ELevelType _type;
    private bool _isEditable;

    public string path
    {
      get
      {
        return this._path;
      }
    }

    public string name
    {
      get
      {
        return this._name;
      }
    }

    public ELevelSize size
    {
      get
      {
        return this._size;
      }
    }

    public ELevelType type
    {
      get
      {
        return this._type;
      }
    }

    public bool isEditable
    {
      get
      {
        return this._isEditable;
      }
    }

    public LevelInfo(string newPath, string newName, ELevelSize newSize, ELevelType newType, bool newEditable)
    {
      this._path = newPath;
      this._name = newName;
      this._size = newSize;
      this._type = newType;
      this._isEditable = newEditable;
    }
  }
}
