// Decompiled with JetBrains decompiler
// Type: Pathfinding.Serialization.MatrixConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Serialization.JsonFx;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Serialization
{
  public class MatrixConverter : JsonConverter
  {
    private float[] values = new float[16];

    public override bool CanConvert(System.Type type)
    {
      return object.Equals((object) type, (object) typeof (Matrix4x4));
    }

    public override object ReadJson(System.Type objectType, Dictionary<string, object> values)
    {
      Matrix4x4 matrix4x4 = new Matrix4x4();
      Array array = (Array) values["values"];
      if (array.Length != 16)
      {
        Debug.LogError((object) ("Number of elements in matrix was not 16 (got " + (object) array.Length + ")"));
        return (object) matrix4x4;
      }
      for (int index = 0; index < 16; ++index)
        matrix4x4[index] = Convert.ToSingle(array.GetValue(new int[1]
        {
          index
        }));
      return (object) matrix4x4;
    }

    public override Dictionary<string, object> WriteJson(System.Type type, object value)
    {
      Matrix4x4 matrix4x4 = (Matrix4x4) value;
      for (int index = 0; index < this.values.Length; ++index)
        this.values[index] = matrix4x4[index];
      return new Dictionary<string, object>()
      {
        {
          "values",
          (object) this.values
        }
      };
    }
  }
}
