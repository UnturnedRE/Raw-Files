// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerInput
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class PlayerInput : PlayerCaller
  {
    public static readonly uint SAMPLES = 4U;
    public static readonly float RATE = Time.fixedDeltaTime * (float) PlayerInput.SAMPLES;
    private float _tick;
    private uint buffer;
    private uint consumed;
    private uint count;
    private uint _simulation;
    private uint _clock;
    private bool[] keys;
    private byte[] flags;
    public Queue<InputInfo> inputs;
    private List<PlayerInputPacket> clientsidePackets;
    private Queue<PlayerInputPacket> serversidePackets;
    private int sequence;
    private float lastInputed;
    private bool hasInputed;
    private bool isDismissed;

    public float tick
    {
      get
      {
        return this._tick;
      }
    }

    public uint simulation
    {
      get
      {
        return this._simulation;
      }
    }

    public uint clock
    {
      get
      {
        return this._clock;
      }
    }

    public void sendRaycast(RaycastInfo info)
    {
      if ((Object) info.player == (Object) null && (Object) info.zombie == (Object) null && ((Object) info.animal == (Object) null && (Object) info.vehicle == (Object) null) && (Object) info.transform == (Object) null)
        return;
      if (Provider.isServer)
      {
        this.inputs.Enqueue(new InputInfo()
        {
          animal = info.animal,
          direction = info.direction,
          limb = info.limb,
          material = info.material,
          normal = info.normal,
          player = info.player,
          point = info.point,
          transform = info.transform,
          vehicle = info.vehicle,
          zombie = info.zombie
        });
      }
      else
      {
        PlayerInputPacket playerInputPacket1 = this.clientsidePackets[this.clientsidePackets.Count - 1];
        if (!(playerInputPacket1 is WalkingPlayerInputPacket))
          return;
        WalkingPlayerInputPacket playerInputPacket2 = playerInputPacket1 as WalkingPlayerInputPacket;
        if (playerInputPacket2.clientsideInputs == null)
          playerInputPacket2.clientsideInputs = new List<RaycastInfo>();
        playerInputPacket2.clientsideInputs.Add(info);
      }
    }

    [SteamCall]
    public void askInput(CSteamID steamID)
    {
      if (!this.channel.checkOwner(steamID))
        return;
      int num1 = -1;
      byte num2 = (byte) this.channel.read(Types.BYTE_TYPE);
      for (byte index = (byte) 0; (int) index < (int) num2; ++index)
      {
        PlayerInputPacket playerInputPacket = (int) (byte) this.channel.read(Types.BYTE_TYPE) <= 0 ? (PlayerInputPacket) new WalkingPlayerInputPacket() : (PlayerInputPacket) new DrivingPlayerInputPacket();
        playerInputPacket.read(this.channel);
        if (playerInputPacket.sequence > this.sequence)
        {
          this.sequence = playerInputPacket.sequence;
          this.serversidePackets.Enqueue(playerInputPacket);
          num1 = playerInputPacket.sequence;
        }
      }
      if (num1 == -1)
        return;
      this.channel.send("askAck", ESteamCall.OWNER, ESteamPacket.UPDATE_UNRELIABLE_INSTANT, new object[1]
      {
        (object) num1
      });
      this.lastInputed = Time.realtimeSinceStartup;
      this.hasInputed = true;
    }

    [SteamCall]
    public void askAck(CSteamID steamID, int ack)
    {
      if (!this.channel.checkServer(steamID))
        return;
      for (int index = this.clientsidePackets.Count - 1; index >= 0; --index)
      {
        if (this.clientsidePackets[index].sequence <= ack)
          this.clientsidePackets.RemoveAt(index);
      }
    }

    private void FixedUpdate()
    {
      if (this.isDismissed)
        return;
      if (this.channel.isOwner)
      {
        if ((int) (this.count % PlayerInput.SAMPLES) == 0)
        {
          this._tick = Time.realtimeSinceStartup;
          this.keys[0] = this.player.movement.jump;
          this.keys[1] = this.player.equipment.primary;
          this.keys[2] = this.player.equipment.secondary;
          this.keys[3] = this.player.stance.crouch;
          this.keys[4] = this.player.stance.prone;
          this.keys[5] = this.player.stance.sprint;
          this.keys[6] = this.player.animator.leanLeft;
          this.keys[7] = this.player.animator.leanRight;
          this.player.life.simulate(this.simulation);
          this.player.stance.simulate(this.simulation, this.player.stance.crouch, this.player.stance.prone, this.player.stance.sprint);
          this.player.movement.simulate(this.simulation, (int) this.player.movement.horizontal - 1, (int) this.player.movement.vertical - 1, this.player.movement.jump, Vector3.zero, PlayerInput.RATE);
          if (Provider.isServer)
          {
            this.inputs.Clear();
          }
          else
          {
            ++this.sequence;
            if (this.player.stance.stance == EPlayerStance.DRIVING)
              this.clientsidePackets.Add((PlayerInputPacket) new DrivingPlayerInputPacket());
            else
              this.clientsidePackets.Add((PlayerInputPacket) new WalkingPlayerInputPacket());
            this.clientsidePackets[this.clientsidePackets.Count - 1].sequence = this.sequence;
          }
          this.player.equipment.simulate(this.simulation, this.player.equipment.primary, this.player.equipment.secondary);
          this.player.animator.simulate(this.simulation, this.player.animator.leanLeft, this.player.animator.leanRight);
          this.buffer += PlayerInput.SAMPLES;
          ++this._simulation;
        }
        if (this.consumed < this.buffer)
        {
          ++this.consumed;
          this.player.equipment.tock(this.clock);
          ++this._clock;
        }
        if ((int) this.consumed == (int) this.buffer && !Provider.isServer)
        {
          PlayerInputPacket playerInputPacket1 = this.clientsidePackets[this.clientsidePackets.Count - 1];
          if (playerInputPacket1 is DrivingPlayerInputPacket)
          {
            DrivingPlayerInputPacket playerInputPacket2 = playerInputPacket1 as DrivingPlayerInputPacket;
            playerInputPacket2.position = this.transform.parent.parent.parent.position;
            playerInputPacket2.angle_x = MeasurementTool.angleToByte(this.transform.parent.parent.parent.rotation.eulerAngles.x);
            playerInputPacket2.angle_y = MeasurementTool.angleToByte(this.transform.parent.parent.parent.rotation.eulerAngles.y);
            playerInputPacket2.angle_z = MeasurementTool.angleToByte(this.transform.parent.parent.parent.rotation.eulerAngles.z);
            playerInputPacket2.speed = (byte) ((double) Mathf.Clamp(this.player.movement.getVehicle().speed, -100f, 100f) + 128.0);
            playerInputPacket2.turn = (byte) (this.player.movement.getVehicle().turn + 1);
          }
          else
          {
            WalkingPlayerInputPacket playerInputPacket2 = playerInputPacket1 as WalkingPlayerInputPacket;
            byte num = (byte) 0;
            for (byte index = (byte) 0; (int) index < this.keys.Length; ++index)
            {
              if (this.keys[(int) index])
                num |= this.flags[(int) index];
            }
            playerInputPacket2.keys = num;
            playerInputPacket2.position = this.transform.localPosition;
            playerInputPacket2.yaw = this.player.look.yaw;
            playerInputPacket2.pitch = this.player.look.pitch;
          }
          this.channel.openWrite();
          this.channel.write((object) (byte) this.clientsidePackets.Count);
          using (List<PlayerInputPacket>.Enumerator enumerator = this.clientsidePackets.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              PlayerInputPacket current = enumerator.Current;
              if (current is DrivingPlayerInputPacket)
                this.channel.write((object) 1);
              else
                this.channel.write((object) 0);
              current.write(this.channel);
            }
          }
          this.channel.closeWrite("askInput", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_CHUNK_INSTANT);
        }
        ++this.count;
      }
      else
      {
        if (!Provider.isServer)
          return;
        if (this.serversidePackets.Count > 0)
        {
          PlayerInputPacket playerInputPacket1 = this.serversidePackets.Dequeue();
          if (playerInputPacket1 == null)
            return;
          if (this.simulation > (uint) (((double) Time.realtimeSinceStartup + 1.0 - (double) this.tick) / (double) PlayerInput.RATE))
          {
            ++this._simulation;
          }
          else
          {
            if (playerInputPacket1 is DrivingPlayerInputPacket)
            {
              DrivingPlayerInputPacket playerInputPacket2 = playerInputPacket1 as DrivingPlayerInputPacket;
              if (!this.player.life.isDead)
              {
                this.player.life.simulate(this.simulation);
                this.player.stance.simulate(this.simulation, false, false, false);
                this.player.look.simulate(0.0f, 0.0f, PlayerInput.RATE);
                this.player.movement.simulate(this.simulation, playerInputPacket2.position, (float) ((int) playerInputPacket2.angle_x * 2), (float) ((int) playerInputPacket2.angle_y * 2), (float) ((int) playerInputPacket2.angle_z * 2), (float) ((int) playerInputPacket2.speed - 128), (int) playerInputPacket2.turn - 1, PlayerInput.RATE);
                this.player.equipment.simulate(this.simulation, false, false);
                this.player.animator.simulate(this.simulation, false, false);
              }
            }
            else
            {
              WalkingPlayerInputPacket playerInputPacket2 = playerInputPacket1 as WalkingPlayerInputPacket;
              for (byte index = (byte) 0; (int) index < this.keys.Length; ++index)
                this.keys[(int) index] = ((int) playerInputPacket2.keys & (int) this.flags[(int) index]) == (int) this.flags[(int) index];
              this.inputs = playerInputPacket2.serversideInputs;
              if (!this.player.life.isDead)
              {
                this.player.life.simulate(this.simulation);
                this.player.stance.simulate(this.simulation, this.keys[3], this.keys[4], this.keys[5]);
                this.player.look.simulate(playerInputPacket2.yaw, playerInputPacket2.pitch, PlayerInput.RATE);
                this.player.movement.simulate(this.simulation, this.keys[0], playerInputPacket2.position, PlayerInput.RATE);
                this.player.equipment.simulate(this.simulation, this.keys[1], this.keys[2]);
                this.player.animator.simulate(this.simulation, this.keys[6], this.keys[7]);
              }
              this.buffer += PlayerInput.SAMPLES;
              while (this.consumed < this.buffer)
              {
                ++this.consumed;
                if (!this.player.life.isDead)
                  this.player.equipment.tock(this.clock);
                ++this._clock;
              }
            }
            ++this._simulation;
          }
        }
        else
        {
          this.player.movement.simulate();
          if (!this.hasInputed || (double) Time.realtimeSinceStartup - (double) this.lastInputed <= 10.0)
            return;
          Provider.dismiss(this.channel.owner.playerID.steamID);
          this.isDismissed = true;
        }
      }
    }

    private void Start()
    {
      this._tick = Time.realtimeSinceStartup;
      this._simulation = 0U;
      this._clock = 0U;
      if (this.channel.isOwner || Provider.isServer)
      {
        this.keys = new bool[8];
        this.flags = new byte[8];
        for (byte index = (byte) 0; (int) index < this.keys.Length; ++index)
          this.flags[(int) index] = (byte) (1 << 7 - (int) index);
      }
      if (this.channel.isOwner && Provider.isServer)
        this.inputs = new Queue<InputInfo>();
      if (this.channel.isOwner)
        this.clientsidePackets = new List<PlayerInputPacket>();
      else if (Provider.isServer)
        this.serversidePackets = new Queue<PlayerInputPacket>();
      this.sequence = -1;
    }
  }
}
