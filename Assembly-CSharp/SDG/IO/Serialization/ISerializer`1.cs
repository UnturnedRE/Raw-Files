// Decompiled with JetBrains decompiler
// Type: SDG.IO.Serialization.ISerializer`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.IO.Serialization
{
  public interface ISerializer<T>
  {
    void serialize(T instance, byte[] data, int index, out int size, bool isFormatted);

    void serialize(T instance, string path, bool isFormatted);
  }
}
