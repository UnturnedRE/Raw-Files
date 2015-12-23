// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Useable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class Useable : PlayerCaller
  {
    public virtual bool canInspect
    {
      get
      {
        return true;
      }
    }

    public virtual void startPrimary()
    {
    }

    public virtual void stopPrimary()
    {
    }

    public virtual void startSecondary()
    {
    }

    public virtual void stopSecondary()
    {
    }

    public virtual void equip()
    {
    }

    public virtual void dequip()
    {
    }

    public virtual void tick()
    {
    }

    public virtual void simulate(uint simulation)
    {
    }

    public virtual void tock(uint clock)
    {
    }

    public virtual void updateState(byte[] newState)
    {
    }
  }
}
