// Decompiled with JetBrains decompiler
// Type: Pathfinding.Serialization.BoundsConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Serialization.JsonFx;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Serialization
{
  public class BoundsConverter : JsonConverter
  {
    public override bool CanConvert(System.Type type)
    {
      return object.Equals((object) type, (object) typeof (Bounds));
    }

    public override object ReadJson(System.Type objectType, Dictionary<string, object> values)
    {
      return (object) new Bounds()
      {
        center = new Vector3(this.CastFloat(values["cx"]), this.CastFloat(values["cy"]), this.CastFloat(values["cz"])),
        extents = new Vector3(this.CastFloat(values["ex"]), this.CastFloat(values["ey"]), this.CastFloat(values["ez"]))
      };
    }

    public override Dictionary<string, object> WriteJson(System.Type type, object value)
    {
      Bounds bounds = (Bounds) value;
      return new Dictionary<string, object>()
      {
        {
          "cx",
          (object) bounds.center.x
        },
        {
          "cy",
          (object) bounds.center.y
        },
        {
          "cz",
          (object) bounds.center.z
        },
        {
          "ex",
          (object) bounds.extents.x
        },
        {
          "ey",
          (object) bounds.extents.y
        },
        {
          "ez",
          (object) bounds.extents.z
        }
      };
    }
  }
}
