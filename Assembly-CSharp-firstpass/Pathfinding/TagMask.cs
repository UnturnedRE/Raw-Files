// Decompiled with JetBrains decompiler
// Type: Pathfinding.TagMask
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Pathfinding
{
  [Serializable]
  public class TagMask
  {
    public int tagsChange;
    public int tagsSet;

    public TagMask()
    {
    }

    public TagMask(int change, int set)
    {
      this.tagsChange = change;
      this.tagsSet = set;
    }

    public void SetValues(object boxedTagMask)
    {
      TagMask tagMask = (TagMask) boxedTagMask;
      this.tagsChange = tagMask.tagsChange;
      this.tagsSet = tagMask.tagsSet;
    }

    public override string ToString()
    {
      return string.Empty + Convert.ToString(this.tagsChange, 2) + "\n" + Convert.ToString(this.tagsSet, 2);
    }
  }
}
