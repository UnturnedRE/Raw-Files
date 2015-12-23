// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SteamChannelMethod
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Reflection;
using UnityEngine;

namespace SDG.Unturned
{
  public class SteamChannelMethod
  {
    private Component _component;
    private MethodInfo _method;
    private System.Type[] _types;

    public Component component
    {
      get
      {
        return this._component;
      }
    }

    public MethodInfo method
    {
      get
      {
        return this._method;
      }
    }

    public System.Type[] types
    {
      get
      {
        return this._types;
      }
    }

    public SteamChannelMethod(Component newComponent, MethodInfo newMethod, System.Type[] newTypes)
    {
      this._component = newComponent;
      this._method = newMethod;
      this._types = newTypes;
    }
  }
}
