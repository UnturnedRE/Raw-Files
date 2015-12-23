// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.LevelObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class LevelObject
  {
    private Transform _transform;
    private ushort _id;
    public byte[] state;
    private ObjectAsset _asset;

    public Transform transform
    {
      get
      {
        return this._transform;
      }
    }

    public ushort id
    {
      get
      {
        return this._id;
      }
    }

    public ObjectAsset asset
    {
      get
      {
        return this._asset;
      }
    }

    public bool isEnabled
    {
      get
      {
        return this.transform.gameObject.activeSelf;
      }
    }

    public LevelObject(Vector3 newPoint, Quaternion newRotation, ushort newID)
    {
      this._id = newID;
      this._asset = (ObjectAsset) Assets.find(EAssetType.OBJECT, this.id);
      if (this.asset == null)
        return;
      this.state = this.asset.getState();
      if (Dedicator.isDedicated)
      {
        if (this.asset.type != EObjectType.SMALL && (Object) this.asset.model != (Object) null)
        {
          this._transform = Object.Instantiate<GameObject>(this.asset.model).transform;
          this.transform.name = this.id.ToString();
          this.transform.parent = LevelObjects.models;
          this.transform.position = newPoint;
          this.transform.rotation = newRotation;
        }
      }
      else if ((Object) this.asset.model != (Object) null)
      {
        this._transform = Object.Instantiate<GameObject>(this.asset.model).transform;
        this.transform.name = this.id.ToString();
        this.transform.parent = LevelObjects.models;
        this.transform.position = newPoint;
        this.transform.rotation = newRotation;
        this.transform.gameObject.SetActive(false);
      }
      if (!((Object) this.transform != (Object) null))
        return;
      if ((Level.isEditor || Provider.isServer) && (this.asset.type != EObjectType.SMALL && (Object) this.asset.nav != (Object) null))
      {
        Transform transform = Object.Instantiate<GameObject>(this.asset.nav).transform;
        transform.name = "Nav";
        transform.parent = this.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
      }
      if (this.asset.type != EObjectType.SMALL)
      {
        if (Level.isEditor)
        {
          Transform child = this.transform.FindChild("Block");
          if ((Object) child != (Object) null && (Object) this.transform.GetComponent<Collider>() == (Object) null)
          {
            BoxCollider boxCollider1 = (BoxCollider) child.GetComponent<Collider>();
            BoxCollider boxCollider2 = this.transform.gameObject.AddComponent<BoxCollider>();
            boxCollider2.center = boxCollider1.center;
            boxCollider2.size = boxCollider1.size;
          }
        }
        else if (Provider.isClient && (Object) this.asset.slots != (Object) null)
        {
          Transform transform = Object.Instantiate<GameObject>(this.asset.slots).transform;
          transform.name = "Slots";
          transform.parent = this.transform;
          transform.localPosition = Vector3.zero;
          transform.localRotation = Quaternion.identity;
        }
        if (!((Object) this.asset.slots != (Object) null))
          ;
      }
      if (this.asset.interactability == EObjectInteractability.NONE)
        return;
      InteractableObject interactableObject = (InteractableObject) null;
      if (this.asset.interactability == EObjectInteractability.BINARY_STATE)
        interactableObject = (InteractableObject) this.transform.gameObject.AddComponent<InteractableObjectBinaryState>();
      else if (this.asset.interactability == EObjectInteractability.DROPPER)
        interactableObject = (InteractableObject) this.transform.gameObject.AddComponent<InteractableObjectDropper>();
      else if (this.asset.interactability == EObjectInteractability.NOTE)
        interactableObject = (InteractableObject) this.transform.gameObject.AddComponent<InteractableObjectNote>();
      if (!((Object) interactableObject != (Object) null))
        return;
      interactableObject.updateState((Asset) this.asset, this.state);
    }

    public void enable()
    {
      if (!((Object) this.transform != (Object) null))
        return;
      this.transform.gameObject.SetActive(true);
    }

    public void disable()
    {
      if (!((Object) this.transform != (Object) null))
        return;
      this.transform.gameObject.SetActive(false);
    }
  }
}
