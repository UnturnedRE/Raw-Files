// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Local
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class Local
  {
    private Data data;

    public Local(Data newData)
    {
      this.data = newData;
    }

    public Local()
    {
      this.data = (Data) null;
    }

    public string format(string key)
    {
      if (this.data != null)
        return this.data.readString(key) ?? "#" + key.ToUpper();
      return "#" + key.ToUpper();
    }

    public string format(string key, params object[] values)
    {
      if (this.data == null || values == null)
        return "#" + key.ToUpper();
      string format = this.data.readString(key);
      if (format != null)
        return string.Format(format, values);
      return "#" + key.ToUpper();
    }
  }
}
