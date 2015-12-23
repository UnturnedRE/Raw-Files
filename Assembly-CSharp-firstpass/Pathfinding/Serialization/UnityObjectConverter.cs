// Decompiled with JetBrains decompiler
// Type: Pathfinding.Serialization.UnityObjectConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding;
using Pathfinding.Serialization.JsonFx;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Serialization
{
  public class UnityObjectConverter : JsonConverter
  {
    public override bool CanConvert(System.Type type)
    {
      return typeof (UnityEngine.Object).IsAssignableFrom(type);
    }

    public override object ReadJson(System.Type objectType, Dictionary<string, object> values)
    {
      if (values == null)
        return (object) null;
      string path = (string) values["Name"];
      if (path == null)
        return (object) null;
      string typeName = (string) values["Type"];
      System.Type type = System.Type.GetType(typeName);
      if (object.Equals((object) type, (object) null))
      {
        Debug.LogError((object) ("Could not find type '" + typeName + "'. Cannot deserialize Unity reference"));
        return (object) null;
      }
      if (values.ContainsKey("GUID"))
      {
        string str = (string) values["GUID"];
        UnityReferenceHelper[] unityReferenceHelperArray = UnityEngine.Object.FindObjectsOfType(typeof (UnityReferenceHelper)) as UnityReferenceHelper[];
        for (int index = 0; index < unityReferenceHelperArray.Length; ++index)
        {
          if (unityReferenceHelperArray[index].GetGUID() == str)
          {
            if (object.Equals((object) type, (object) typeof (GameObject)))
              return (object) unityReferenceHelperArray[index].gameObject;
            return (object) unityReferenceHelperArray[index].GetComponent(type);
          }
        }
      }
      UnityEngine.Object[] objectArray = Resources.LoadAll(path, type);
      for (int index = 0; index < objectArray.Length; ++index)
      {
        if (objectArray[index].name == path || objectArray.Length == 1)
          return (object) objectArray[index];
      }
      return (object) null;
    }

    public override Dictionary<string, object> WriteJson(System.Type type, object value)
    {
      UnityEngine.Object @object = (UnityEngine.Object) value;
      Dictionary<string, object> dictionary = new Dictionary<string, object>();
      if (value == null)
      {
        dictionary.Add("Name", (object) null);
        return dictionary;
      }
      dictionary.Add("Name", (object) @object.name);
      dictionary.Add("Type", (object) @object.GetType().AssemblyQualifiedName);
      Component component = value as Component;
      GameObject gameObject = value as GameObject;
      if ((UnityEngine.Object) component != (UnityEngine.Object) null || (UnityEngine.Object) gameObject != (UnityEngine.Object) null)
      {
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && (UnityEngine.Object) gameObject == (UnityEngine.Object) null)
          gameObject = component.gameObject;
        UnityReferenceHelper unityReferenceHelper = gameObject.GetComponent<UnityReferenceHelper>();
        if ((UnityEngine.Object) unityReferenceHelper == (UnityEngine.Object) null)
        {
          Debug.Log((object) ("Adding UnityReferenceHelper to Unity Reference '" + @object.name + "'"));
          unityReferenceHelper = gameObject.AddComponent<UnityReferenceHelper>();
        }
        unityReferenceHelper.Reset();
        dictionary.Add("GUID", (object) unityReferenceHelper.GetGUID());
      }
      return dictionary;
    }
  }
}
