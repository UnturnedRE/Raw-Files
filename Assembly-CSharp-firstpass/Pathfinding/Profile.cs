// Decompiled with JetBrains decompiler
// Type: Pathfinding.Profile
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Diagnostics;

namespace Pathfinding
{
  public class Profile
  {
    private int control = 1073741824;
    private const bool PROFILE_MEM = false;
    public string name;
    private Stopwatch w;
    private int counter;
    private long mem;
    private long smem;
    private bool dontCountFirst;

    public Profile(string name)
    {
      this.name = name;
      this.w = new Stopwatch();
    }

    public int ControlValue()
    {
      return this.control;
    }

    [Conditional("PROFILE")]
    public void Start()
    {
      if (this.dontCountFirst && this.counter == 1)
        return;
      this.w.Start();
    }

    [Conditional("PROFILE")]
    public void Stop()
    {
      ++this.counter;
      if (this.dontCountFirst && this.counter == 1)
        return;
      this.w.Stop();
    }

    [Conditional("PROFILE")]
    public void Log()
    {
      UnityEngine.Debug.Log((object) this.ToString());
    }

    [Conditional("PROFILE")]
    public void ConsoleLog()
    {
      Console.WriteLine(this.ToString());
    }

    [Conditional("PROFILE")]
    public void Stop(int control)
    {
      ++this.counter;
      if (this.dontCountFirst && this.counter == 1)
        return;
      this.w.Stop();
      if (this.control == 1073741824)
        this.control = control;
      else if (this.control != control)
        throw new Exception(string.Concat(new object[4]
        {
          (object) "Control numbers do not match ",
          (object) this.control,
          (object) " != ",
          (object) control
        }));
    }

    [Conditional("PROFILE")]
    public void Control(Profile other)
    {
      if (this.ControlValue() != other.ControlValue())
        throw new Exception("Control numbers do not match (" + (object) this.name + " " + other.name + ") " + (string) (object) this.ControlValue() + " != " + (string) (object) other.ControlValue());
    }

    public override string ToString()
    {
      return this.name + (object) " #" + (string) (object) this.counter + " " + this.w.Elapsed.TotalMilliseconds.ToString("0.0 ms") + " avg: " + (this.w.Elapsed.TotalMilliseconds / (double) this.counter).ToString("0.00 ms");
    }
  }
}
