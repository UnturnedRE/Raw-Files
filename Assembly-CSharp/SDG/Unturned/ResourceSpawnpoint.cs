// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ResourceSpawnpoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class ResourceSpawnpoint
  {
    public byte type;
    public ushort id;
    private float _lastDead;
    private Vector3 _point;
    private bool _isGenerated;
    private Quaternion _angle;
    private Vector3 _scale;
    private ResourceAsset _asset;
    private Transform _model;
    private Transform _stump;
    public ushort health;

    public float lastDead
    {
      get
      {
        return this._lastDead;
      }
    }

    public bool isDead
    {
      get
      {
        return (int) this.health == 0;
      }
    }

    public Vector3 point
    {
      get
      {
        return this._point;
      }
    }

    public bool isGenerated
    {
      get
      {
        return this._isGenerated;
      }
    }

    public Quaternion angle
    {
      get
      {
        return this._angle;
      }
    }

    public Vector3 scale
    {
      get
      {
        return this._scale;
      }
    }

    public ResourceAsset asset
    {
      get
      {
        return this._asset;
      }
    }

    public Transform model
    {
      get
      {
        return this._model;
      }
    }

    public Transform stump
    {
      get
      {
        return this._stump;
      }
    }

    public ResourceSpawnpoint(byte newType, ushort newID, Vector3 newPoint, bool newGenerated)
    {
      this.type = newType;
      this.id = newID;
      this._point = newPoint;
      this._isGenerated = newGenerated;
      this._asset = (ResourceAsset) Assets.find(EAssetType.RESOURCE, this.id);
      float num = Mathf.Sin((float) (((double) this.point.x + 4096.0) * 32.0 + ((double) this.point.z + 4096.0) * 32.0));
      this._angle = Quaternion.Euler(num * 5f, num * 360f, 0.0f);
      this._scale = new Vector3((float) (1.10000002384186 + (double) this.asset.scale + (double) num * (double) this.asset.scale), (float) (1.10000002384186 + (double) this.asset.scale + (double) num * (double) this.asset.scale), (float) (1.10000002384186 + (double) this.asset.scale + (double) num * (double) this.asset.scale));
      if (this.asset == null)
        return;
      if ((Object) this.asset.model != (Object) null)
      {
        this._model = Object.Instantiate<GameObject>(this.asset.model).transform;
        this.model.name = this.type.ToString();
        this.model.position = this.point + Vector3.down * this.scale.y * 0.75f;
        this.model.rotation = this.angle;
        this.model.localScale = this.scale;
        this.model.parent = LevelGround.models;
        if (!Dedicator.isDedicated)
        {
          this.model.gameObject.SetActive(false);
          if (!Level.isEditor && this.asset.isForage)
            this.model.FindChild("Forage").gameObject.AddComponent<InteractableForage>();
        }
      }
      this.health = this.asset.health;
    }

    public void askDamage(ushort amount)
    {
      if ((int) amount == 0 || this.isDead)
        return;
      if ((int) amount >= (int) this.health)
        this.health = (ushort) 0;
      else
        this.health -= amount;
    }

    public void wipe()
    {
      if (!((Object) this.model != (Object) null) || this.asset == null)
        return;
      if (this.asset.isForage)
      {
        if (Dedicator.isDedicated)
          return;
        this.model.FindChild("Forage").gameObject.SetActive(false);
      }
      else
      {
        if ((Object) this.asset.stump != (Object) null)
        {
          this._stump = Object.Instantiate<GameObject>(this.asset.stump).transform;
          this.stump.name = this.type.ToString();
          this.stump.position = this.point + Vector3.down * this.scale.y * 0.75f;
          this.stump.rotation = this.angle;
          this.stump.localScale = this.scale;
          this.stump.parent = LevelGround.models;
        }
        if (!((Object) this.model != (Object) null))
          return;
        Object.Destroy((Object) this.model.gameObject);
      }
    }

    public void revive()
    {
      if (this.asset == null)
        return;
      if (this.asset.isForage)
      {
        if (!Dedicator.isDedicated)
          this.model.FindChild("Forage").gameObject.SetActive(true);
        this.health = this.asset.health;
      }
      else
      {
        if ((Object) this.model != (Object) null)
          Object.Destroy((Object) this.model.gameObject);
        if ((Object) this.asset.model != (Object) null)
        {
          this._model = Object.Instantiate<GameObject>(this.asset.model).transform;
          this.model.name = this.type.ToString();
          this.model.position = this.point + Vector3.down * this.scale.y * 0.75f;
          this.model.rotation = this.angle;
          this.model.localScale = this.scale;
          this.model.parent = LevelGround.models;
          if (!Dedicator.isDedicated && (Object) this.stump != (Object) null)
            this.model.gameObject.SetActive(this.stump.gameObject.activeInHierarchy);
        }
        this.health = this.asset.health;
        if (!((Object) this.stump != (Object) null))
          return;
        Object.Destroy((Object) this.stump.gameObject);
      }
    }

    public void kill(Vector3 ragdoll)
    {
      this._lastDead = Time.realtimeSinceStartup;
      if (this.asset == null)
        return;
      if (this.asset.isForage)
      {
        if (Dedicator.isDedicated)
          return;
        this.model.FindChild("Forage").gameObject.SetActive(false);
      }
      else
      {
        if ((Object) this.asset.stump != (Object) null)
        {
          this._stump = Object.Instantiate<GameObject>(this.asset.stump).transform;
          this.stump.name = this.type.ToString();
          this.stump.position = this.point + Vector3.down * this.scale.y * 0.75f;
          this.stump.rotation = this.angle;
          this.stump.localScale = this.scale;
          this.stump.parent = LevelGround.models;
          if (!Dedicator.isDedicated && (Object) this.model != (Object) null)
            this.stump.gameObject.SetActive(this.model.gameObject.activeInHierarchy);
        }
        if (Dedicator.isDedicated || GraphicsSettings.effectQuality == EGraphicQuality.OFF)
        {
          if (!((Object) this.model != (Object) null))
            return;
          Object.Destroy((Object) this.model.gameObject);
        }
        else
        {
          ragdoll.y += 8f;
          ragdoll.x += Random.Range(-16f, 16f);
          ragdoll.z += Random.Range(-16f, 16f);
          ragdoll *= !((Object) Player.player != (Object) null) || Player.player.skills.boost != EPlayerBoost.FLIGHT ? 2f : 4f;
          if (!((Object) this.model != (Object) null))
            return;
          this.model.parent = Level.effects;
          this.model.tag = "Debris";
          this.model.gameObject.layer = LayerMasks.DEBRIS;
          this.model.gameObject.AddComponent<Rigidbody>();
          this.model.GetComponent<Rigidbody>().AddForce(ragdoll);
          this.model.GetComponent<Rigidbody>().drag = 0.5f;
          this.model.GetComponent<Rigidbody>().angularDrag = 0.1f;
          this.model.position = this.point + Vector3.up;
          this.model.rotation = this.angle;
          this.model.localScale = this.scale;
          Object.Destroy((Object) this.model.gameObject, 8f);
        }
      }
    }

    public void enable()
    {
      if ((Object) this.stump != (Object) null)
      {
        this.stump.gameObject.SetActive(true);
      }
      else
      {
        if (!((Object) this.model != (Object) null))
          return;
        this.model.gameObject.SetActive(true);
      }
    }

    public void disable()
    {
      if ((Object) this.stump != (Object) null)
      {
        this.stump.gameObject.SetActive(false);
      }
      else
      {
        if (!((Object) this.model != (Object) null))
          return;
        this.model.gameObject.SetActive(false);
      }
    }
  }
}
