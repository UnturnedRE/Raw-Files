// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Passenger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class Passenger
  {
    public SteamPlayer player;
    private Transform _seat;

    public Transform seat
    {
      get
      {
        return this._seat;
      }
    }

    public Passenger(Transform newSeat)
    {
      this._seat = newSeat;
    }
  }
}
