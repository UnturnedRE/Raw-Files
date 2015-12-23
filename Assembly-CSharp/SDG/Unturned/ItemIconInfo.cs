// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemIconInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class ItemIconInfo
  {
    public ushort id;
    public ushort skin;
    public byte quality;
    public byte[] state;
    public ItemAsset itemAsset;
    public SkinAsset skinAsset;
    public int x;
    public int y;
    public bool scale;
    public ItemIconReady callback;
  }
}
