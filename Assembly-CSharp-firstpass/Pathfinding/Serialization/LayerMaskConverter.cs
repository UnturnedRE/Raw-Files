// Decompiled with JetBrains decompiler
// Type: Pathfinding.Serialization.LayerMaskConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Serialization.JsonFx;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Serialization
{
  public class LayerMaskConverter : JsonConverter
  {
    public override bool CanConvert(System.Type type)
    {
      return object.Equals((object) type, (object) typeof (LayerMask));
    }

    public override object ReadJson(System.Type type, Dictionary<string, object> values)
    {
      return (object) (LayerMask) ((int) values["value"]);
    }

    public override Dictionary<string, object> WriteJson(System.Type type, object value)
    {
      return new Dictionary<string, object>()
      {
        {
          "value",
          (object) ((LayerMask) value).value
        }
      };
    }
  }
}
