// Decompiled with JetBrains decompiler
// Type: Pathfinding.Serialization.GuidConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Serialization.JsonFx;
using System;
using System.Collections.Generic;

namespace Pathfinding.Serialization
{
  public class GuidConverter : JsonConverter
  {
    public override bool CanConvert(Type type)
    {
      return object.Equals((object) type, (object) typeof (Pathfinding.Util.Guid));
    }

    public override object ReadJson(Type objectType, Dictionary<string, object> values)
    {
      return (object) new Pathfinding.Util.Guid((string) values["value"]);
    }

    public override Dictionary<string, object> WriteJson(Type type, object value)
    {
      return new Dictionary<string, object>()
      {
        {
          "value",
          (object) ((Pathfinding.Util.Guid) value).ToString()
        }
      };
    }
  }
}
