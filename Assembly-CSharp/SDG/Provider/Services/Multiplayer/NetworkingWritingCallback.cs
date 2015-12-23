// Decompiled with JetBrains decompiler
// Type: SDG.Provider.Services.Multiplayer.NetworkingWritingCallback
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.IO;

namespace SDG.Provider.Services.Multiplayer
{
  public delegate void NetworkingWritingCallback(MemoryStream bufferStream, BinaryWriter bufferWriter);
}
