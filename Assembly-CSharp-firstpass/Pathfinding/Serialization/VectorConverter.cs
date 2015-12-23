// Decompiled with JetBrains decompiler
// Type: Pathfinding.Serialization.VectorConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Serialization.JsonFx;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Serialization
{
  public class VectorConverter : JsonConverter
  {
    public override bool CanConvert(System.Type type)
    {
      if (!object.Equals((object) type, (object) typeof (Vector2)) && !object.Equals((object) type, (object) typeof (Vector3)))
        return object.Equals((object) type, (object) typeof (Vector4));
      return true;
    }

    public override object ReadJson(System.Type type, Dictionary<string, object> values)
    {
      if (object.Equals((object) type, (object) typeof (Vector2)))
        return (object) new Vector2(this.CastFloat(values["x"]), this.CastFloat(values["y"]));
      if (object.Equals((object) type, (object) typeof (Vector3)))
        return (object) new Vector3(this.CastFloat(values["x"]), this.CastFloat(values["y"]), this.CastFloat(values["z"]));
      if (object.Equals((object) type, (object) typeof (Vector4)))
        return (object) new Vector4(this.CastFloat(values["x"]), this.CastFloat(values["y"]), this.CastFloat(values["z"]), this.CastFloat(values["w"]));
      throw new NotImplementedException("Can only read Vector2,3,4. Not objects of type " + (object) type);
    }

    public override Dictionary<string, object> WriteJson(System.Type type, object value)
    {
      if (object.Equals((object) type, (object) typeof (Vector2)))
      {
        Vector2 vector2 = (Vector2) value;
        return new Dictionary<string, object>()
        {
          {
            "x",
            (object) vector2.x
          },
          {
            "y",
            (object) vector2.y
          }
        };
      }
      if (object.Equals((object) type, (object) typeof (Vector3)))
      {
        Vector3 vector3 = (Vector3) value;
        return new Dictionary<string, object>()
        {
          {
            "x",
            (object) vector3.x
          },
          {
            "y",
            (object) vector3.y
          },
          {
            "z",
            (object) vector3.z
          }
        };
      }
      if (!object.Equals((object) type, (object) typeof (Vector4)))
        throw new NotImplementedException("Can only write Vector2,3,4. Not objects of type " + (object) type);
      Vector4 vector4 = (Vector4) value;
      return new Dictionary<string, object>()
      {
        {
          "x",
          (object) vector4.x
        },
        {
          "y",
          (object) vector4.y
        },
        {
          "z",
          (object) vector4.z
        },
        {
          "w",
          (object) vector4.w
        }
      };
    }
  }
}
