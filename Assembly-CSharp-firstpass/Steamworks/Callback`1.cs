// Decompiled with JetBrains decompiler
// Type: Steamworks.Callback`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
  public sealed class Callback<T>
  {
    private IntPtr m_pVTable = IntPtr.Zero;
    private readonly int m_size = Marshal.SizeOf(typeof (T));
    private CCallbackBaseVTable VTable;
    private CCallbackBase m_CCallbackBase;
    private GCHandle m_pCCallbackBase;
    private bool m_bGameServer;

    private event Callback<T>.DispatchDelegate m_Func;

    public Callback(Callback<T>.DispatchDelegate func, bool bGameServer = false)
    {
      this.m_bGameServer = bGameServer;
      this.BuildCCallbackBase();
      this.Register(func);
    }

    ~Callback()
    {
      this.Unregister();
      if (this.m_pVTable != IntPtr.Zero)
        Marshal.FreeHGlobal(this.m_pVTable);
      if (!this.m_pCCallbackBase.IsAllocated)
        return;
      this.m_pCCallbackBase.Free();
    }

    public static Callback<T> Create(Callback<T>.DispatchDelegate func)
    {
      return new Callback<T>(func, false);
    }

    public static Callback<T> CreateGameServer(Callback<T>.DispatchDelegate func)
    {
      return new Callback<T>(func, true);
    }

    public void Register(Callback<T>.DispatchDelegate func)
    {
      if (func == null)
        throw new Exception("Callback function must not be null.");
      if (((int) this.m_CCallbackBase.m_nCallbackFlags & 1) == 1)
        this.Unregister();
      if (this.m_bGameServer)
        this.SetGameserverFlag();
      this.m_Func = func;
      NativeMethods.SteamAPI_RegisterCallback(this.m_pCCallbackBase.AddrOfPinnedObject(), CallbackIdentities.GetCallbackIdentity(typeof (T)));
    }

    public void Unregister()
    {
      NativeMethods.SteamAPI_UnregisterCallback(this.m_pCCallbackBase.AddrOfPinnedObject());
    }

    public void SetGameserverFlag()
    {
      this.m_CCallbackBase.m_nCallbackFlags |= (byte) 2;
    }

    private void OnRunCallback(IntPtr thisptr, IntPtr pvParam)
    {
      try
      {
        this.m_Func((T) Marshal.PtrToStructure(pvParam, typeof (T)));
      }
      catch (Exception ex)
      {
        CallbackDispatcher.ExceptionHandler(ex);
      }
    }

    private void OnRunCallResult(IntPtr thisptr, IntPtr pvParam, bool bFailed, ulong hSteamAPICall)
    {
      try
      {
        this.m_Func((T) Marshal.PtrToStructure(pvParam, typeof (T)));
      }
      catch (Exception ex)
      {
        CallbackDispatcher.ExceptionHandler(ex);
      }
    }

    private int OnGetCallbackSizeBytes(IntPtr thisptr)
    {
      return this.m_size;
    }

    private void BuildCCallbackBase()
    {
      this.VTable = new CCallbackBaseVTable()
      {
        m_RunCallResult = new CCallbackBaseVTable.RunCRDel(this.OnRunCallResult),
        m_RunCallback = new CCallbackBaseVTable.RunCBDel(this.OnRunCallback),
        m_GetCallbackSizeBytes = new CCallbackBaseVTable.GetCallbackSizeBytesDel(this.OnGetCallbackSizeBytes)
      };
      this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (CCallbackBaseVTable)));
      Marshal.StructureToPtr((object) this.VTable, this.m_pVTable, false);
      this.m_CCallbackBase = new CCallbackBase()
      {
        m_vfptr = this.m_pVTable,
        m_nCallbackFlags = (byte) 0,
        m_iCallback = CallbackIdentities.GetCallbackIdentity(typeof (T))
      };
      this.m_pCCallbackBase = GCHandle.Alloc((object) this.m_CCallbackBase, GCHandleType.Pinned);
    }

    public delegate void DispatchDelegate(T param);
  }
}
