// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EffectManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class EffectManager : SteamCaller
  {
    public static readonly float SMALL = 64f;
    public static readonly float MEDIUM = 128f;
    public static readonly float LARGE = 256f;
    public static readonly float INSANE = 512f;
    private static EffectManager manager;

    public static void sendEffect(ushort id, byte x, byte y, byte area, Vector3 point, Vector3 normal)
    {
      EffectManager.manager.channel.send("tellEffectPointNormal", ESteamCall.CLIENTS, x, y, area, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, (object) id, (object) point, (object) normal);
    }

    public static void sendEffect(ushort id, byte x, byte y, byte area, Vector3 point)
    {
      EffectManager.manager.channel.send("tellEffectPoint", ESteamCall.CLIENTS, x, y, area, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[2]
      {
        (object) id,
        (object) point
      });
    }

    public static void sendEffect(ushort id, float radius, Vector3 point, Vector3 normal)
    {
      EffectManager.manager.channel.send("tellEffectPointNormal", ESteamCall.CLIENTS, point, radius, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
      {
        (object) id,
        (object) point,
        (object) normal
      });
    }

    public static void sendEffect(ushort id, float radius, Vector3 point)
    {
      EffectManager.manager.channel.send("tellEffectPoint", ESteamCall.CLIENTS, point, radius, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[2]
      {
        (object) id,
        (object) point
      });
    }

    [SteamCall]
    public void tellEffectPointNormal(CSteamID steamID, ushort id, Vector3 point, Vector3 normal)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this.effect(id, point, normal);
    }

    [SteamCall]
    public void tellEffectPoint(CSteamID steamID, ushort id, Vector3 point)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this.effect(id, point, Vector3.up);
    }

    private void effect(ushort id, Vector3 point, Vector3 normal)
    {
      if (GraphicsSettings.effectQuality == EGraphicQuality.OFF)
        return;
      EffectAsset effectAsset = (EffectAsset) Assets.find(EAssetType.EFFECT, id);
      if (effectAsset == null)
        return;
      Transform transform1 = ((GameObject) Object.Instantiate((Object) effectAsset.effect, point, Quaternion.LookRotation(normal) * Quaternion.Euler(0.0f, 0.0f, (float) Random.Range(0, 360)))).transform;
      transform1.name = "Effect";
      transform1.parent = Level.effects;
      if (effectAsset.gore)
      {
        if (OptionsSettings.gore)
        {
          if ((int) effectAsset.splatter > 0)
          {
            for (int index = 0; index < (int) effectAsset.splatter * (!((Object) Player.player != (Object) null) || Player.player.skills.boost != EPlayerBoost.SPLATTERIFIC ? 1 : 8); ++index)
            {
              RaycastHit hitInfo;
              Physics.Raycast(point, -2f * normal + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)), out hitInfo, 8f, RayMasks.SPLATTER);
              if ((Object) hitInfo.transform != (Object) null && !PhysicsTool.isMaterialDynamic(DamageTool.getMaterial(hitInfo.point, hitInfo.transform, hitInfo.collider)))
              {
                float num = Random.Range(1f, 2f);
                Transform transform2 = ((GameObject) Object.Instantiate((Object) effectAsset.splatters[Random.Range(0, effectAsset.splatters.Length)], hitInfo.point + hitInfo.normal * Random.Range(0.04f, 0.06f), Quaternion.LookRotation(hitInfo.normal) * Quaternion.Euler(0.0f, 0.0f, (float) Random.Range(0, 360)))).transform;
                transform2.name = "Splatter";
                transform2.parent = Level.effects;
                transform2.localScale = new Vector3(num, num, num);
                transform2.gameObject.SetActive(true);
                Object.Destroy((Object) transform2.gameObject, GraphicsSettings.effect);
              }
            }
          }
        }
        else
          Object.Destroy((Object) transform1.GetComponent<ParticleSystem>());
      }
      if (!effectAsset.isStatic && (Object) transform1.GetComponent<AudioSource>() != (Object) null)
        transform1.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
      if ((double) effectAsset.lifetime > 1.0)
        Object.Destroy((Object) transform1.gameObject, Random.Range(effectAsset.lifetime - 4f, effectAsset.lifetime + 4f));
      else if ((Object) transform1.GetComponent<ParticleSystem>() != (Object) null)
        Object.Destroy((Object) transform1.gameObject, transform1.GetComponent<ParticleSystem>().startLifetime);
      else if ((Object) transform1.GetComponent<AudioSource>() != (Object) null && (Object) transform1.GetComponent<AudioSource>().clip != (Object) null)
        Object.Destroy((Object) transform1.gameObject, transform1.GetComponent<AudioSource>().clip.length);
      else
        Object.Destroy((Object) transform1.gameObject, GraphicsSettings.effect);
    }

    private void Start()
    {
      EffectManager.manager = this;
    }
  }
}
