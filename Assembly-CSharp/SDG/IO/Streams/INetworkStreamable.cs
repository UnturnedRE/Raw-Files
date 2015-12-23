// Decompiled with JetBrains decompiler
// Type: SDG.IO.Streams.INetworkStreamable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.IO.Streams
{
  public interface INetworkStreamable
  {
    void readFromStream(NetworkStream networkStream);

    void writeToStream(NetworkStream networkStream);
  }
}
