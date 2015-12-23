// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekJars
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class SleekJars : Sleek
  {
    public ClickedJar onClickedJar;

    public SleekJars(float radius, List<InventorySearch> search)
    {
      this.init();
      float num = 6.283185f / (float) search.Count;
      for (int index = 0; index < search.Count; ++index)
      {
        ItemJar jar = search[index].jar;
        if ((ItemAsset) Assets.find(EAssetType.ITEM, jar.item.id) != null)
        {
          SleekItem sleekItem = new SleekItem(jar);
          sleekItem.positionOffset_X = (int) ((double) Mathf.Cos(num * (float) index) * (double) radius) - sleekItem.sizeOffset_X / 2;
          sleekItem.positionOffset_Y = (int) ((double) Mathf.Sin(num * (float) index) * (double) radius) - sleekItem.sizeOffset_Y / 2;
          sleekItem.positionScale_X = 0.5f;
          sleekItem.positionScale_Y = 0.5f;
          sleekItem.onClickedItem = new ClickedItem(this.onClickedButton);
          sleekItem.onDraggedItem = new DraggedItem(this.onClickedButton);
          this.add((Sleek) sleekItem);
        }
      }
    }

    public override void draw(bool ignoreCulling)
    {
      this.drawChildren(ignoreCulling);
    }

    private void onClickedButton(SleekItem item)
    {
      int index = this.search((Sleek) item);
      if (index == -1 || this.onClickedJar == null)
        return;
      this.onClickedJar(this, index);
    }
  }
}
