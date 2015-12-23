// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SteamChannel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SDG.Unturned
{
  public class SteamChannel : MonoBehaviour
  {
    private static object[] voice = new object[3];
    private SteamChannelMethod[] _calls;
    public int id;
    public SteamPlayer owner;
    public bool isOwner;

    public SteamChannelMethod[] calls
    {
      get
      {
        return this._calls;
      }
    }

    public bool checkServer(CSteamID steamID)
    {
      return steamID == Provider.server;
    }

    public bool checkOwner(CSteamID steamID)
    {
      if (this.owner == null)
        return false;
      return steamID == this.owner.playerID.steamID;
    }

    public void receive(CSteamID steamID, byte[] packet, int offset, int size)
    {
      if (size < 2)
        return;
      int index = (int) packet[offset + 1];
      if (index < 0 || index >= this.calls.Length)
        return;
      ESteamPacket esteamPacket = (ESteamPacket) packet[offset];
      if (esteamPacket == ESteamPacket.UPDATE_VOICE && size < 4)
        return;
      if (esteamPacket == ESteamPacket.UPDATE_UNRELIABLE_CHUNK_BUFFER || esteamPacket == ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER)
      {
        SteamPacker.openRead(offset + 2, packet);
        this.calls[index].method.Invoke((object) this.calls[index].component, new object[1]
        {
          (object) steamID
        });
        SteamPacker.closeRead();
      }
      else if (this.calls[index].types.Length > 0)
      {
        if (esteamPacket == ESteamPacket.UPDATE_VOICE)
        {
          SteamChannel.voice[0] = (object) steamID;
          SteamChannel.voice[1] = (object) packet;
          SteamChannel.voice[2] = (object) (int) BitConverter.ToUInt16(packet, offset + 2);
          this.calls[index].method.Invoke((object) this.calls[index].component, SteamChannel.voice);
        }
        else
        {
          object[] objects = SteamPacker.getObjects(steamID, offset, 2, packet, this.calls[index].types);
          if (objects == null)
            return;
          this.calls[index].method.Invoke((object) this.calls[index].component, objects);
        }
      }
      else
        this.calls[index].method.Invoke((object) this.calls[index].component, (object[]) null);
    }

    public object read(System.Type type)
    {
      return SteamPacker.read(type);
    }

    public object[] read(params System.Type[] types)
    {
      return SteamPacker.read(types);
    }

    public void write(object objects)
    {
      SteamPacker.write(objects);
    }

    public void write(params object[] objects)
    {
      SteamPacker.write(objects);
    }

    public void openWrite()
    {
      SteamPacker.openWrite(2);
    }

    public void closeWrite(string name, CSteamID steamID, ESteamPacket type)
    {
      if (!Provider.isChunk(type))
      {
        Debug.LogError((object) "Failed to stream non chunk.");
      }
      else
      {
        int call = this.getCall(name);
        if (call == -1)
          return;
        int size;
        byte[] packet;
        this.getPacket(type, call, out size, out packet);
        if (this.isOwner && steamID == Provider.client)
          this.receive(Provider.client, packet, 0, size);
        else if (Provider.isServer && steamID == Provider.server)
          this.receive(Provider.server, packet, 0, size);
        else
          Provider.send(steamID, type, packet, size, this.id);
      }
    }

    public void closeWrite(string name, ESteamCall mode, byte bound, ESteamPacket type)
    {
      if (!Provider.isChunk(type))
      {
        Debug.LogError((object) "Failed to stream non chunk.");
      }
      else
      {
        int call = this.getCall(name);
        if (call == -1)
          return;
        int size;
        byte[] packet;
        this.getPacket(type, call, out size, out packet);
        this.send(mode, bound, type, size, packet);
      }
    }

    public void closeWrite(string name, ESteamCall mode, byte x, byte y, byte area, ESteamPacket type)
    {
      if (!Provider.isChunk(type))
      {
        Debug.LogError((object) "Failed to stream non chunk.");
      }
      else
      {
        int call = this.getCall(name);
        if (call == -1)
          return;
        int size;
        byte[] packet;
        this.getPacket(type, call, out size, out packet);
        this.send(mode, x, y, area, type, size, packet);
      }
    }

    public void closeWrite(string name, ESteamCall mode, ESteamPacket type)
    {
      if (!Provider.isChunk(type))
      {
        Debug.LogError((object) "Failed to stream non chunk.");
      }
      else
      {
        int call = this.getCall(name);
        if (call == -1)
          return;
        int size;
        byte[] packet;
        this.getPacket(type, call, out size, out packet);
        this.send(mode, type, size, packet);
      }
    }

    public void send(string name, CSteamID steamID, ESteamPacket type, params object[] arguments)
    {
      int call = this.getCall(name);
      if (call == -1)
        return;
      int size;
      byte[] packet;
      this.getPacket(type, call, out size, out packet, arguments);
      if (this.isOwner && steamID == Provider.client)
        this.receive(Provider.client, packet, 0, size);
      else if (Provider.isServer && steamID == Provider.server)
        this.receive(Provider.server, packet, 0, size);
      else
        Provider.send(steamID, type, packet, size, this.id);
    }

    public void sendAside(string name, CSteamID steamID, ESteamPacket type, params object[] arguments)
    {
      int call = this.getCall(name);
      if (call == -1)
        return;
      int size;
      byte[] packet;
      this.getPacket(type, call, out size, out packet, arguments);
      for (int index = 0; index < Provider.clients.Count; ++index)
      {
        if (Provider.clients[index].playerID.steamID != steamID)
          Provider.send(Provider.clients[index].playerID.steamID, type, packet, size, this.id);
      }
    }

    public void send(ESteamCall mode, byte bound, ESteamPacket type, int size, byte[] packet)
    {
      if (mode == ESteamCall.SERVER)
      {
        if (Provider.isServer)
          this.receive(Provider.server, packet, 0, size);
        else
          Provider.send(Provider.server, type, packet, size, this.id);
      }
      else if (mode == ESteamCall.ALL)
      {
        if (!Provider.isServer)
          Provider.send(Provider.server, type, packet, size, this.id);
        for (int index = 0; index < Provider.clients.Count; ++index)
        {
          if (Provider.clients[index].playerID.steamID != Provider.client && (UnityEngine.Object) Provider.clients[index].player != (UnityEngine.Object) null && (int) Provider.clients[index].player.movement.bound == (int) bound)
            Provider.send(Provider.clients[index].playerID.steamID, type, packet, size, this.id);
        }
        if (Provider.isServer)
          this.receive(Provider.server, packet, 0, size);
        else
          this.receive(Provider.client, packet, 0, size);
      }
      else if (mode == ESteamCall.OTHERS)
      {
        if (!Provider.isServer)
          Provider.send(Provider.server, type, packet, size, this.id);
        for (int index = 0; index < Provider.clients.Count; ++index)
        {
          if (Provider.clients[index].playerID.steamID != Provider.client && (UnityEngine.Object) Provider.clients[index].player != (UnityEngine.Object) null && (int) Provider.clients[index].player.movement.bound == (int) bound)
            Provider.send(Provider.clients[index].playerID.steamID, type, packet, size, this.id);
        }
      }
      else if (mode == ESteamCall.OWNER)
      {
        if (this.isOwner)
          this.receive(this.owner.playerID.steamID, packet, 0, size);
        else
          Provider.send(this.owner.playerID.steamID, type, packet, size, this.id);
      }
      else if (mode == ESteamCall.NOT_OWNER)
      {
        if (!Provider.isServer)
          Provider.send(Provider.server, type, packet, size, this.id);
        for (int index = 0; index < Provider.clients.Count; ++index)
        {
          if (Provider.clients[index].playerID.steamID != this.owner.playerID.steamID && (UnityEngine.Object) Provider.clients[index].player != (UnityEngine.Object) null && (int) Provider.clients[index].player.movement.bound == (int) bound)
            Provider.send(Provider.clients[index].playerID.steamID, type, packet, size, this.id);
        }
      }
      else if (mode == ESteamCall.CLIENTS)
      {
        for (int index = 0; index < Provider.clients.Count; ++index)
        {
          if (Provider.clients[index].playerID.steamID != Provider.client && (UnityEngine.Object) Provider.clients[index].player != (UnityEngine.Object) null && (int) Provider.clients[index].player.movement.bound == (int) bound)
            Provider.send(Provider.clients[index].playerID.steamID, type, packet, size, this.id);
        }
        if (!Provider.isClient)
          return;
        this.receive(Provider.client, packet, 0, size);
      }
      else
      {
        if (mode != ESteamCall.PEERS)
          return;
        for (int index = 0; index < Provider.clients.Count; ++index)
        {
          if (Provider.clients[index].playerID.steamID != Provider.client && (UnityEngine.Object) Provider.clients[index].player != (UnityEngine.Object) null && (int) Provider.clients[index].player.movement.bound == (int) bound)
            Provider.send(Provider.clients[index].playerID.steamID, type, packet, size, this.id);
        }
      }
    }

    public void send(string name, ESteamCall mode, byte bound, ESteamPacket type, byte[] bytes, int length)
    {
      int call = this.getCall(name);
      if (call == -1)
        return;
      int size;
      byte[] packet;
      this.getPacket(type, call, out size, out packet, bytes, length);
      this.send(mode, bound, type, size, packet);
    }

    public void send(string name, ESteamCall mode, byte bound, ESteamPacket type, params object[] arguments)
    {
      int call = this.getCall(name);
      if (call == -1)
        return;
      int size;
      byte[] packet;
      this.getPacket(type, call, out size, out packet, arguments);
      this.send(mode, bound, type, size, packet);
    }

    public void send(ESteamCall mode, byte x, byte y, byte area, ESteamPacket type, int size, byte[] packet)
    {
      if (mode == ESteamCall.SERVER)
      {
        if (Provider.isServer)
          this.receive(Provider.server, packet, 0, size);
        else
          Provider.send(Provider.server, type, packet, size, this.id);
      }
      else if (mode == ESteamCall.ALL)
      {
        if (!Provider.isServer)
          Provider.send(Provider.server, type, packet, size, this.id);
        for (int index = 0; index < Provider.clients.Count; ++index)
        {
          if (Provider.clients[index].playerID.steamID != Provider.client && (UnityEngine.Object) Provider.clients[index].player != (UnityEngine.Object) null && Regions.checkArea(x, y, Provider.clients[index].player.movement.region_x, Provider.clients[index].player.movement.region_y, area))
            Provider.send(Provider.clients[index].playerID.steamID, type, packet, size, this.id);
        }
        if (Provider.isServer)
          this.receive(Provider.server, packet, 0, size);
        else
          this.receive(Provider.client, packet, 0, size);
      }
      else if (mode == ESteamCall.OTHERS)
      {
        if (!Provider.isServer)
          Provider.send(Provider.server, type, packet, size, this.id);
        for (int index = 0; index < Provider.clients.Count; ++index)
        {
          if (Provider.clients[index].playerID.steamID != Provider.client && (UnityEngine.Object) Provider.clients[index].player != (UnityEngine.Object) null && Regions.checkArea(x, y, Provider.clients[index].player.movement.region_x, Provider.clients[index].player.movement.region_y, area))
            Provider.send(Provider.clients[index].playerID.steamID, type, packet, size, this.id);
        }
      }
      else if (mode == ESteamCall.OWNER)
      {
        if (this.isOwner)
          this.receive(this.owner.playerID.steamID, packet, 0, size);
        else
          Provider.send(this.owner.playerID.steamID, type, packet, size, this.id);
      }
      else if (mode == ESteamCall.NOT_OWNER)
      {
        if (!Provider.isServer)
          Provider.send(Provider.server, type, packet, size, this.id);
        for (int index = 0; index < Provider.clients.Count; ++index)
        {
          if (Provider.clients[index].playerID.steamID != this.owner.playerID.steamID && (UnityEngine.Object) Provider.clients[index].player != (UnityEngine.Object) null && Regions.checkArea(x, y, Provider.clients[index].player.movement.region_x, Provider.clients[index].player.movement.region_y, area))
            Provider.send(Provider.clients[index].playerID.steamID, type, packet, size, this.id);
        }
      }
      else if (mode == ESteamCall.CLIENTS)
      {
        for (int index = 0; index < Provider.clients.Count; ++index)
        {
          if (Provider.clients[index].playerID.steamID != Provider.client && (UnityEngine.Object) Provider.clients[index].player != (UnityEngine.Object) null && Regions.checkArea(x, y, Provider.clients[index].player.movement.region_x, Provider.clients[index].player.movement.region_y, area))
            Provider.send(Provider.clients[index].playerID.steamID, type, packet, size, this.id);
        }
        if (!Provider.isClient)
          return;
        this.receive(Provider.client, packet, 0, size);
      }
      else
      {
        if (mode != ESteamCall.PEERS)
          return;
        for (int index = 0; index < Provider.clients.Count; ++index)
        {
          if (Provider.clients[index].playerID.steamID != Provider.client && (UnityEngine.Object) Provider.clients[index].player != (UnityEngine.Object) null && Regions.checkArea(x, y, Provider.clients[index].player.movement.region_x, Provider.clients[index].player.movement.region_y, area))
            Provider.send(Provider.clients[index].playerID.steamID, type, packet, size, this.id);
        }
      }
    }

    public void send(string name, ESteamCall mode, byte x, byte y, byte area, ESteamPacket type, byte[] bytes, int length)
    {
      int call = this.getCall(name);
      if (call == -1)
        return;
      int size;
      byte[] packet;
      this.getPacket(type, call, out size, out packet, bytes, length);
      this.send(mode, x, y, area, type, size, packet);
    }

    public void send(string name, ESteamCall mode, byte x, byte y, byte area, ESteamPacket type, params object[] arguments)
    {
      int call = this.getCall(name);
      if (call == -1)
        return;
      int size;
      byte[] packet;
      this.getPacket(type, call, out size, out packet, arguments);
      this.send(mode, x, y, area, type, size, packet);
    }

    public void send(ESteamCall mode, ESteamPacket type, int size, byte[] packet)
    {
      if (mode == ESteamCall.SERVER)
      {
        if (Provider.isServer)
          this.receive(Provider.server, packet, 0, size);
        else
          Provider.send(Provider.server, type, packet, size, this.id);
      }
      else if (mode == ESteamCall.ALL)
      {
        if (!Provider.isServer)
          Provider.send(Provider.server, type, packet, size, this.id);
        for (int index = 0; index < Provider.clients.Count; ++index)
        {
          if (Provider.clients[index].playerID.steamID != Provider.client)
            Provider.send(Provider.clients[index].playerID.steamID, type, packet, size, this.id);
        }
        if (Provider.isServer)
          this.receive(Provider.server, packet, 0, size);
        else
          this.receive(Provider.client, packet, 0, size);
      }
      else if (mode == ESteamCall.OTHERS)
      {
        if (!Provider.isServer)
          Provider.send(Provider.server, type, packet, size, this.id);
        for (int index = 0; index < Provider.clients.Count; ++index)
        {
          if (Provider.clients[index].playerID.steamID != Provider.client)
            Provider.send(Provider.clients[index].playerID.steamID, type, packet, size, this.id);
        }
      }
      else if (mode == ESteamCall.OWNER)
      {
        if (this.isOwner)
          this.receive(this.owner.playerID.steamID, packet, 0, size);
        else
          Provider.send(this.owner.playerID.steamID, type, packet, size, this.id);
      }
      else if (mode == ESteamCall.NOT_OWNER)
      {
        if (!Provider.isServer)
          Provider.send(Provider.server, type, packet, size, this.id);
        for (int index = 0; index < Provider.clients.Count; ++index)
        {
          if (Provider.clients[index].playerID.steamID != this.owner.playerID.steamID)
            Provider.send(Provider.clients[index].playerID.steamID, type, packet, size, this.id);
        }
      }
      else if (mode == ESteamCall.CLIENTS)
      {
        for (int index = 0; index < Provider.clients.Count; ++index)
        {
          if (Provider.clients[index].playerID.steamID != Provider.client)
            Provider.send(Provider.clients[index].playerID.steamID, type, packet, size, this.id);
        }
        if (!Provider.isClient)
          return;
        this.receive(Provider.client, packet, 0, size);
      }
      else
      {
        if (mode != ESteamCall.PEERS)
          return;
        for (int index = 0; index < Provider.clients.Count; ++index)
        {
          if (Provider.clients[index].playerID.steamID != Provider.client)
            Provider.send(Provider.clients[index].playerID.steamID, type, packet, size, this.id);
        }
      }
    }

    public void send(string name, ESteamCall mode, ESteamPacket type, params object[] arguments)
    {
      int call = this.getCall(name);
      if (call == -1)
        return;
      int size;
      byte[] packet;
      this.getPacket(type, call, out size, out packet, arguments);
      this.send(mode, type, size, packet);
    }

    public void send(string name, ESteamCall mode, ESteamPacket type, byte[] bytes, int length)
    {
      int call = this.getCall(name);
      if (call == -1)
        return;
      int size;
      byte[] packet;
      this.getPacket(type, call, out size, out packet, bytes, length);
      this.send(mode, type, size, packet);
    }

    public void send(ESteamCall mode, Vector3 point, float radius, ESteamPacket type, int size, byte[] packet)
    {
      radius *= radius;
      if (mode == ESteamCall.SERVER)
      {
        if (Provider.isServer)
          this.receive(Provider.server, packet, 0, size);
        else
          Provider.send(Provider.server, type, packet, size, this.id);
      }
      else if (mode == ESteamCall.ALL)
      {
        if (!Provider.isServer)
          Provider.send(Provider.server, type, packet, size, this.id);
        for (int index = 0; index < Provider.clients.Count; ++index)
        {
          if (Provider.clients[index].playerID.steamID != Provider.client && (UnityEngine.Object) Provider.clients[index].player != (UnityEngine.Object) null && (double) (Provider.clients[index].player.transform.position - point).sqrMagnitude < (double) radius)
            Provider.send(Provider.clients[index].playerID.steamID, type, packet, size, this.id);
        }
        if (Provider.isServer)
          this.receive(Provider.server, packet, 0, size);
        else
          this.receive(Provider.client, packet, 0, size);
      }
      else if (mode == ESteamCall.OTHERS)
      {
        if (!Provider.isServer)
          Provider.send(Provider.server, type, packet, size, this.id);
        for (int index = 0; index < Provider.clients.Count; ++index)
        {
          if (Provider.clients[index].playerID.steamID != Provider.client && (UnityEngine.Object) Provider.clients[index].player != (UnityEngine.Object) null && (double) (Provider.clients[index].player.transform.position - point).sqrMagnitude < (double) radius)
            Provider.send(Provider.clients[index].playerID.steamID, type, packet, size, this.id);
        }
      }
      else if (mode == ESteamCall.OWNER)
      {
        if (this.isOwner)
          this.receive(this.owner.playerID.steamID, packet, 0, size);
        else
          Provider.send(this.owner.playerID.steamID, type, packet, size, this.id);
      }
      else if (mode == ESteamCall.NOT_OWNER)
      {
        if (!Provider.isServer)
          Provider.send(Provider.server, type, packet, size, this.id);
        for (int index = 0; index < Provider.clients.Count; ++index)
        {
          if (Provider.clients[index].playerID.steamID != this.owner.playerID.steamID && (UnityEngine.Object) Provider.clients[index].player != (UnityEngine.Object) null && (double) (Provider.clients[index].player.transform.position - point).sqrMagnitude < (double) radius)
            Provider.send(Provider.clients[index].playerID.steamID, type, packet, size, this.id);
        }
      }
      else if (mode == ESteamCall.CLIENTS)
      {
        for (int index = 0; index < Provider.clients.Count; ++index)
        {
          if (Provider.clients[index].playerID.steamID != Provider.client && (UnityEngine.Object) Provider.clients[index].player != (UnityEngine.Object) null && (double) (Provider.clients[index].player.transform.position - point).sqrMagnitude < (double) radius)
            Provider.send(Provider.clients[index].playerID.steamID, type, packet, size, this.id);
        }
        if (!Provider.isClient)
          return;
        this.receive(Provider.client, packet, 0, size);
      }
      else
      {
        if (mode != ESteamCall.PEERS)
          return;
        for (int index = 0; index < Provider.clients.Count; ++index)
        {
          if (Provider.clients[index].playerID.steamID != Provider.client && (UnityEngine.Object) Provider.clients[index].player != (UnityEngine.Object) null && (double) (Provider.clients[index].player.transform.position - point).sqrMagnitude < (double) radius)
            Provider.send(Provider.clients[index].playerID.steamID, type, packet, size, this.id);
        }
      }
    }

    public void send(string name, ESteamCall mode, Vector3 point, float radius, ESteamPacket type, byte[] bytes, int length)
    {
      int call = this.getCall(name);
      if (call == -1)
        return;
      int size;
      byte[] packet;
      this.getPacket(type, call, out size, out packet, bytes, length);
      this.send(mode, point, radius, type, size, packet);
    }

    public void send(string name, ESteamCall mode, Vector3 point, float radius, ESteamPacket type, params object[] arguments)
    {
      int call = this.getCall(name);
      if (call == -1)
        return;
      int size;
      byte[] packet;
      this.getPacket(type, call, out size, out packet, arguments);
      this.send(mode, point, radius, type, size, packet);
    }

    public void build()
    {
      List<SteamChannelMethod> list = new List<SteamChannelMethod>();
      Component[] components = this.GetComponents(typeof (Component));
      for (int index1 = 0; index1 < components.Length; ++index1)
      {
        MemberInfo[] members = components[index1].GetType().GetMembers();
        for (int index2 = 0; index2 < members.Length; ++index2)
        {
          if (members[index2].MemberType == MemberTypes.Method)
          {
            MethodInfo newMethod = (MethodInfo) members[index2];
            if (newMethod.GetCustomAttributes(typeof (SteamCall), true).Length > 0)
            {
              ParameterInfo[] parameters = newMethod.GetParameters();
              System.Type[] newTypes = new System.Type[parameters.Length];
              for (int index3 = 0; index3 < parameters.Length; ++index3)
                newTypes[index3] = parameters[index3].ParameterType;
              list.Add(new SteamChannelMethod(components[index1], newMethod, newTypes));
            }
          }
        }
      }
      this._calls = list.ToArray();
    }

    public virtual void init()
    {
    }

    public void setup()
    {
      Provider.openChannel(this);
    }

    private void getPacket(ESteamPacket type, int index, out int size, out byte[] packet)
    {
      packet = SteamPacker.closeWrite(out size);
      packet[0] = (byte) type;
      packet[1] = (byte) index;
    }

    private void getPacket(ESteamPacket type, int index, out int size, out byte[] packet, byte[] bytes, int length)
    {
      size = 4 + length;
      packet = bytes;
      packet[0] = (byte) type;
      packet[1] = (byte) index;
      byte[] bytes1 = BitConverter.GetBytes((ushort) length);
      packet[2] = bytes1[0];
      packet[3] = bytes1[1];
    }

    private void getPacket(ESteamPacket type, int index, out int size, out byte[] packet, params object[] arguments)
    {
      packet = SteamPacker.getBytes(2, out size, arguments);
      packet[0] = (byte) type;
      packet[1] = (byte) index;
    }

    private int getCall(string name)
    {
      for (int index = 0; index < this.calls.Length; ++index)
      {
        if (this.calls[index].method.Name == name)
          return index;
      }
      return -1;
    }

    private void Awake()
    {
      this.build();
      this.init();
    }

    private void OnDestroy()
    {
      if (this.id == 0)
        return;
      Provider.closeChannel(this);
    }
  }
}
