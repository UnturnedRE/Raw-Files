// Decompiled with JetBrains decompiler
// Type: Pathfinding.UnityReferenceHelper
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
  [ExecuteInEditMode]
  public class UnityReferenceHelper : MonoBehaviour
  {
    [HideInInspector]
    [SerializeField]
    private string guid;

    public string GetGUID()
    {
      return this.guid;
    }

    public void Awake()
    {
      this.Reset();
    }

    public void Reset()
    {
      if (this.guid == null || this.guid == string.Empty)
      {
        this.guid = Guid.NewGuid().ToString();
        Debug.Log((object) ("Created new GUID - " + this.guid));
      }
      else
      {
        foreach (UnityReferenceHelper unityReferenceHelper in Object.FindObjectsOfType(typeof (UnityReferenceHelper)) as UnityReferenceHelper[])
        {
          if ((Object) unityReferenceHelper != (Object) this && this.guid == unityReferenceHelper.guid)
          {
            this.guid = Guid.NewGuid().ToString();
            Debug.Log((object) ("Created new GUID - " + this.guid));
            break;
          }
        }
      }
    }
  }
}
