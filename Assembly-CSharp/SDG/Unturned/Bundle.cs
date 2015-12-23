// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Bundle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class Bundle
  {
    private AssetBundle asset;
    private string resource;
    private string _name;
    private string _path;
    private bool _usePath;

    public string name
    {
      get
      {
        return this._name;
      }
    }

    public string path
    {
      get
      {
        return this._path;
      }
    }

    public bool usePath
    {
      get
      {
        return this._usePath;
      }
    }

    public bool hasResource
    {
      get
      {
        return (UnityEngine.Object) this.asset == (UnityEngine.Object) null;
      }
    }

    public Bundle(string path)
      : this(path, true)
    {
    }

    public Bundle(string path, bool usePath)
    {
      this._usePath = usePath;
      this.asset = !ReadWrite.fileExists(path, false, usePath) ? (AssetBundle) null : AssetBundle.CreateFromFile(!usePath ? path : ReadWrite.PATH + path);
      this._name = ReadWrite.fileName(path);
      this._path = ReadWrite.folderPath(path);
      if (!((UnityEngine.Object) this.asset == (UnityEngine.Object) null))
        return;
      this.resource = this._path.Substring(1);
    }

    public Bundle()
    {
      this.asset = (AssetBundle) null;
      this._name = "#BUNDLE_NAME";
      this._path = "#BUNDLE_PATH";
    }

    public UnityEngine.Object load(string name)
    {
      if (!((UnityEngine.Object) this.asset != (UnityEngine.Object) null))
        return Resources.Load(this.resource + "/" + name);
      if (this.asset.Contains(name))
        return this.asset.LoadAsset(name);
      return (UnityEngine.Object) null;
    }

    public UnityEngine.Object[] load()
    {
      if ((UnityEngine.Object) this.asset != (UnityEngine.Object) null)
        return this.asset.LoadAllAssets();
      return (UnityEngine.Object[]) null;
    }

    public UnityEngine.Object[] load(System.Type type)
    {
      if ((UnityEngine.Object) this.asset != (UnityEngine.Object) null)
        return this.asset.LoadAllAssets(type);
      return (UnityEngine.Object[]) null;
    }

    public void unload()
    {
      if (!((UnityEngine.Object) this.asset != (UnityEngine.Object) null))
        return;
      this.asset.Unload(false);
    }
  }
}
