// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ControlsSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class ControlsSettings
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 13;
    public static readonly byte LEFT = (byte) 0;
    public static readonly byte RIGHT = (byte) 1;
    public static readonly byte UP = (byte) 2;
    public static readonly byte DOWN = (byte) 3;
    public static readonly byte JUMP = (byte) 4;
    public static readonly byte LEAN_LEFT = (byte) 5;
    public static readonly byte LEAN_RIGHT = (byte) 6;
    public static readonly byte PRIMARY = (byte) 7;
    public static readonly byte SECONDARY = (byte) 8;
    public static readonly byte INTERACT = (byte) 9;
    public static readonly byte CROUCH = (byte) 10;
    public static readonly byte PRONE = (byte) 11;
    public static readonly byte SPRINT = (byte) 12;
    public static readonly byte RELOAD = (byte) 13;
    public static readonly byte ATTACH = (byte) 14;
    public static readonly byte FIREMODE = (byte) 15;
    public static readonly byte DASHBOARD = (byte) 16;
    public static readonly byte INVENTORY = (byte) 17;
    public static readonly byte CRAFTING = (byte) 18;
    public static readonly byte SKILLS = (byte) 19;
    public static readonly byte MAP = (byte) 20;
    public static readonly byte PLAYERS = (byte) 21;
    public static readonly byte VOICE = (byte) 22;
    public static readonly byte MODIFY = (byte) 23;
    public static readonly byte SNAP = (byte) 24;
    public static readonly byte FOCUS = (byte) 25;
    public static readonly byte TOOL_0 = (byte) 26;
    public static readonly byte TOOL_1 = (byte) 27;
    public static readonly byte TOOL_2 = (byte) 28;
    public static readonly byte HUD = (byte) 29;
    public static readonly byte OTHER = (byte) 30;
    public static readonly byte GLOBAL = (byte) 31;
    public static readonly byte LOCAL = (byte) 32;
    public static readonly byte GROUP = (byte) 33;
    public static readonly byte GESTURE = (byte) 34;
    public static readonly byte VISION = (byte) 35;
    public static readonly byte TACTICAL = (byte) 36;
    public static readonly byte PERSPECTIVE = (byte) 37;
    public static readonly byte DEQUIP = (byte) 38;
    public static readonly byte STANCE = (byte) 39;
    public static readonly byte MIN_SENSITIVITY = (byte) 1;
    public static readonly byte MAX_SENSITIVITY = (byte) 6;
    private static ControlBinding[] _bindings = new ControlBinding[40];
    private static float _sensitvity;
    private static float _look;
    public static bool invert;
    public static EControlMode aiming;
    public static EControlMode crouching;
    public static EControlMode proning;
    public static EControlMode sprinting;

    public static float sensitivity
    {
      get
      {
        return ControlsSettings._sensitvity;
      }
      set
      {
        ControlsSettings._sensitvity = value;
        ControlsSettings._look = (float) ControlsSettings.MIN_SENSITIVITY + (float) ControlsSettings.MAX_SENSITIVITY * value;
      }
    }

    public static float look
    {
      get
      {
        return ControlsSettings._look;
      }
    }

    public static ControlBinding[] bindings
    {
      get
      {
        return ControlsSettings._bindings;
      }
    }

    public static KeyCode left
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.LEFT].key;
      }
    }

    public static KeyCode up
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.UP].key;
      }
    }

    public static KeyCode right
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.RIGHT].key;
      }
    }

    public static KeyCode down
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.DOWN].key;
      }
    }

    public static KeyCode jump
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.JUMP].key;
      }
    }

    public static KeyCode leanLeft
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.LEAN_LEFT].key;
      }
    }

    public static KeyCode leanRight
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.LEAN_RIGHT].key;
      }
    }

    public static KeyCode primary
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.PRIMARY].key;
      }
    }

    public static KeyCode secondary
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.SECONDARY].key;
      }
    }

    public static KeyCode reload
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.RELOAD].key;
      }
    }

    public static KeyCode attach
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.ATTACH].key;
      }
    }

    public static KeyCode firemode
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.FIREMODE].key;
      }
    }

    public static KeyCode dashboard
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.DASHBOARD].key;
      }
    }

    public static KeyCode inventory
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.INVENTORY].key;
      }
    }

    public static KeyCode crafting
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.CRAFTING].key;
      }
    }

    public static KeyCode skills
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.SKILLS].key;
      }
    }

    public static KeyCode map
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.MAP].key;
      }
    }

    public static KeyCode players
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.PLAYERS].key;
      }
    }

    public static KeyCode voice
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.VOICE].key;
      }
    }

    public static KeyCode interact
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.INTERACT].key;
      }
    }

    public static KeyCode crouch
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.CROUCH].key;
      }
    }

    public static KeyCode prone
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.PRONE].key;
      }
    }

    public static KeyCode stance
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.STANCE].key;
      }
    }

    public static KeyCode sprint
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.SPRINT].key;
      }
    }

    public static KeyCode modify
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.MODIFY].key;
      }
    }

    public static KeyCode snap
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.SNAP].key;
      }
    }

    public static KeyCode focus
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.FOCUS].key;
      }
    }

    public static KeyCode tool_0
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.TOOL_0].key;
      }
    }

    public static KeyCode tool_1
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.TOOL_1].key;
      }
    }

    public static KeyCode tool_2
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.TOOL_2].key;
      }
    }

    public static KeyCode hud
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.HUD].key;
      }
    }

    public static KeyCode other
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.OTHER].key;
      }
    }

    public static KeyCode global
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.GLOBAL].key;
      }
    }

    public static KeyCode local
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.LOCAL].key;
      }
    }

    public static KeyCode group
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.GROUP].key;
      }
    }

    public static KeyCode gesture
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.GESTURE].key;
      }
    }

    public static KeyCode vision
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.VISION].key;
      }
    }

    public static KeyCode tactical
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.TACTICAL].key;
      }
    }

    public static KeyCode perspective
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.PERSPECTIVE].key;
      }
    }

    public static KeyCode dequip
    {
      get
      {
        return ControlsSettings.bindings[(int) ControlsSettings.DEQUIP].key;
      }
    }

    public static void bind(byte index, KeyCode key)
    {
      if (ControlsSettings.bindings[(int) index] == null)
        ControlsSettings.bindings[(int) index] = new ControlBinding(key);
      else
        ControlsSettings.bindings[(int) index].key = key;
    }

    public static void load()
    {
      ControlsSettings.bind(ControlsSettings.LEFT, KeyCode.A);
      ControlsSettings.bind(ControlsSettings.RIGHT, KeyCode.D);
      ControlsSettings.bind(ControlsSettings.UP, KeyCode.W);
      ControlsSettings.bind(ControlsSettings.DOWN, KeyCode.S);
      ControlsSettings.bind(ControlsSettings.JUMP, KeyCode.Space);
      ControlsSettings.bind(ControlsSettings.LEAN_LEFT, KeyCode.Q);
      ControlsSettings.bind(ControlsSettings.LEAN_RIGHT, KeyCode.E);
      ControlsSettings.bind(ControlsSettings.PRIMARY, KeyCode.Mouse0);
      ControlsSettings.bind(ControlsSettings.SECONDARY, KeyCode.Mouse1);
      ControlsSettings.bind(ControlsSettings.INTERACT, KeyCode.F);
      ControlsSettings.bind(ControlsSettings.CROUCH, KeyCode.X);
      ControlsSettings.bind(ControlsSettings.PRONE, KeyCode.Z);
      ControlsSettings.bind(ControlsSettings.STANCE, KeyCode.O);
      ControlsSettings.bind(ControlsSettings.SPRINT, KeyCode.LeftShift);
      ControlsSettings.bind(ControlsSettings.RELOAD, KeyCode.R);
      ControlsSettings.bind(ControlsSettings.ATTACH, KeyCode.T);
      ControlsSettings.bind(ControlsSettings.FIREMODE, KeyCode.V);
      ControlsSettings.bind(ControlsSettings.DASHBOARD, KeyCode.Tab);
      ControlsSettings.bind(ControlsSettings.INVENTORY, KeyCode.G);
      ControlsSettings.bind(ControlsSettings.CRAFTING, KeyCode.Y);
      ControlsSettings.bind(ControlsSettings.SKILLS, KeyCode.U);
      ControlsSettings.bind(ControlsSettings.MAP, KeyCode.M);
      ControlsSettings.bind(ControlsSettings.PLAYERS, KeyCode.P);
      ControlsSettings.bind(ControlsSettings.VOICE, KeyCode.LeftAlt);
      ControlsSettings.bind(ControlsSettings.MODIFY, KeyCode.LeftShift);
      ControlsSettings.bind(ControlsSettings.SNAP, KeyCode.LeftControl);
      ControlsSettings.bind(ControlsSettings.FOCUS, KeyCode.F);
      ControlsSettings.bind(ControlsSettings.TOOL_0, KeyCode.Q);
      ControlsSettings.bind(ControlsSettings.TOOL_1, KeyCode.W);
      ControlsSettings.bind(ControlsSettings.TOOL_2, KeyCode.E);
      ControlsSettings.bind(ControlsSettings.HUD, KeyCode.Home);
      ControlsSettings.bind(ControlsSettings.OTHER, KeyCode.LeftControl);
      ControlsSettings.bind(ControlsSettings.GLOBAL, KeyCode.J);
      ControlsSettings.bind(ControlsSettings.LOCAL, KeyCode.K);
      ControlsSettings.bind(ControlsSettings.GROUP, KeyCode.L);
      ControlsSettings.bind(ControlsSettings.GESTURE, KeyCode.C);
      ControlsSettings.bind(ControlsSettings.VISION, KeyCode.N);
      ControlsSettings.bind(ControlsSettings.TACTICAL, KeyCode.B);
      ControlsSettings.bind(ControlsSettings.PERSPECTIVE, KeyCode.H);
      ControlsSettings.bind(ControlsSettings.DEQUIP, KeyCode.BackQuote);
      ControlsSettings.aiming = EControlMode.HOLD;
      ControlsSettings.crouching = EControlMode.TOGGLE;
      ControlsSettings.proning = EControlMode.TOGGLE;
      ControlsSettings.sprinting = EControlMode.HOLD;
      if (ReadWrite.fileExists("/Controls.dat", true))
      {
        Block block = ReadWrite.readBlock("/Controls.dat", true, (byte) 0);
        if (block != null)
        {
          byte num1 = block.readByte();
          if ((int) num1 > 10)
          {
            ControlsSettings.sensitivity = block.readSingle();
            ControlsSettings.invert = block.readBoolean();
            if ((int) num1 > 11)
            {
              ControlsSettings.aiming = (EControlMode) block.readByte();
              ControlsSettings.crouching = (EControlMode) block.readByte();
              ControlsSettings.proning = (EControlMode) block.readByte();
            }
            else
            {
              ControlsSettings.aiming = EControlMode.HOLD;
              ControlsSettings.crouching = EControlMode.TOGGLE;
              ControlsSettings.proning = EControlMode.TOGGLE;
            }
            ControlsSettings.sprinting = (int) num1 <= 12 ? EControlMode.HOLD : (EControlMode) block.readByte();
            byte num2 = block.readByte();
            for (byte index = (byte) 0; (int) index < (int) num2; ++index)
            {
              if ((int) index >= ControlsSettings.bindings.Length)
              {
                int num3 = (int) block.readByte();
              }
              else
              {
                ushort num4 = block.readUInt16();
                ControlsSettings.bindings[(int) index] = new ControlBinding((KeyCode) num4);
              }
            }
            return;
          }
        }
      }
      ControlsSettings.sensitivity = 0.5f;
      ControlsSettings.invert = false;
    }

    public static void save()
    {
      Block block = new Block();
      block.writeByte(ControlsSettings.SAVEDATA_VERSION);
      block.writeSingle(ControlsSettings.sensitivity);
      block.writeBoolean(ControlsSettings.invert);
      block.writeByte((byte) ControlsSettings.aiming);
      block.writeByte((byte) ControlsSettings.crouching);
      block.writeByte((byte) ControlsSettings.proning);
      block.writeByte((byte) ControlsSettings.sprinting);
      block.writeByte((byte) ControlsSettings.bindings.Length);
      for (byte index = (byte) 0; (int) index < ControlsSettings.bindings.Length; ++index)
      {
        ControlBinding controlBinding = ControlsSettings.bindings[(int) index];
        block.writeUInt16((ushort) controlBinding.key);
      }
      ReadWrite.writeBlock("/Controls.dat", true, block);
    }
  }
}
