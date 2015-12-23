// Decompiled with JetBrains decompiler
// Type: Pathfinding.Serialization.IntKeyDictionaryConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Serialization.JsonFx;
using System;
using System.Collections.Generic;

namespace Pathfinding.Serialization
{
  public class IntKeyDictionaryConverter : JsonConverter
  {
    public override bool CanConvert(Type type)
    {
      if (!object.Equals((object) type, (object) typeof (Dictionary<int, int>)))
        return object.Equals((object) type, (object) typeof (SortedDictionary<int, int>));
      return true;
    }

    public override object ReadJson(Type type, Dictionary<string, object> values)
    {
      Dictionary<int, int> dictionary = new Dictionary<int, int>();
      using (Dictionary<string, object>.Enumerator enumerator = values.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<string, object> current = enumerator.Current;
          dictionary.Add(Convert.ToInt32(current.Key), Convert.ToInt32(current.Value));
        }
      }
      return (object) dictionary;
    }

    public override Dictionary<string, object> WriteJson(Type type, object value)
    {
      Dictionary<string, object> dictionary = new Dictionary<string, object>();
      using (Dictionary<int, int>.Enumerator enumerator = ((Dictionary<int, int>) value).GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, int> current = enumerator.Current;
          dictionary.Add(current.Key.ToString(), (object) current.Value);
        }
      }
      return dictionary;
    }
  }
}
