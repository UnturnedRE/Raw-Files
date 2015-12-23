﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.CallResult`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
  public sealed class CallResult<T>
  {
    private IntPtr m_pVTable = IntPtr.Zero;
    private SteamAPICall_t m_hAPICall = SteamAPICall_t.Invalid;
    private readonly int m_size = Marshal.SizeOf(typeof (T));
    private CCallbackBaseVTable VTable;
    private CCallbackBase m_CCallbackBase;
    private GCHandle m_pCCallbackBase;

    public SteamAPICall_t Handle
    {
      get
      {
        return this.m_hAPICall;
      }
    }

    private event CallResult<T>.APIDispatchDelegate m_Func;

    public CallResult(CallResult<T>.APIDispatchDelegate func = null)
    {
      this.m_Func = func;
      this.BuildCCallbackBase();
    }

    ~CallResult()
    {
      this.Cancel();
      if (this.m_pVTable != IntPtr.Zero)
        Marshal.FreeHGlobal(this.m_pVTable);
      if (!this.m_pCCallbackBase.IsAllocated)
        return;
      this.m_pCCallbackBase.Free();
    }

    public static CallResult<T> Create(CallResult<T>.APIDispatchDelegate func = null)
    {
      return new CallResult<T>(func);
    }

    public void Set(SteamAPICall_t hAPICall, CallResult<T>.APIDispatchDelegate func = null)
    {
      if (func != null)
        this.m_Func = func;
      if (this.m_Func == null)
        throw new Exception("CallResult function was null, you must either set it in the CallResult Constructor or in Set()");
      if (this.m_hAPICall != SteamAPICall_t.Invalid)
        NativeMethods.SteamAPI_UnregisterCallResult(this.m_pCCallbackBase.AddrOfPinnedObject(), (ulong) this.m_hAPICall);
      this.m_hAPICall = hAPICall;
      if (!(hAPICall != SteamAPICall_t.Invalid))
        return;
      NativeMethods.SteamAPI_RegisterCallResult(this.m_pCCallbackBase.AddrOfPinnedObject(), (ulong) hAPICall);
    }

    public bool IsActive()
    {
      return this.m_hAPICall != SteamAPICall_t.Invalid;
    }

    public void Cancel()
    {
      if (!(this.m_hAPICall != SteamAPICall_t.Invalid))
        return;
      NativeMethods.SteamAPI_UnregisterCallResult(this.m_pCCallbackBase.AddrOfPinnedObject(), (ulong) this.m_hAPICall);
      this.m_hAPICall = SteamAPICall_t.Invalid;
    }

    public void SetGameserverFlag()
    {
      this.m_CCallbackBase.m_nCallbackFlags |= (byte) 2;
    }

    private void OnRunCallback(IntPtr thisptr, IntPtr pvParam)
    {
      this.m_hAPICall = SteamAPICall_t.Invalid;
      try
      {
        this.m_Func((T) Marshal.PtrToStructure(pvParam, typeof (T)), false);
      }
      catch (Exception ex)
      {
        CallbackDispatcher.ExceptionHandler(ex);
      }
    }

    private void OnRunCallResult(IntPtr thisptr, IntPtr pvParam, bool bFailed, ulong hSteamAPICall)
    {
      SteamAPICall_t steamApiCallT = (SteamAPICall_t) hSteamAPICall;
      if (!(steamApiCallT == this.m_hAPICall))
        return;
      try
      {
        this.m_Func((T) Marshal.PtrToStructure(pvParam, typeof (T)), bFailed);
      }
      catch (Exception ex)
      {
        CallbackDispatcher.ExceptionHandler(ex);
      }
      if (!(steamApiCallT == this.m_hAPICall))
        return;
      this.m_hAPICall = SteamAPICall_t.Invalid;
    }

    private int OnGetCallbackSizeBytes(IntPtr thisptr)
    {
      return this.m_size;
    }

    private void BuildCCallbackBase()
    {
      this.VTable = new CCallbackBaseVTable()
      {
        m_RunCallback = new CCallbackBaseVTable.RunCBDel(this.OnRunCallback),
        m_RunCallResult = new CCallbackBaseVTable.RunCRDel(this.OnRunCallResult),
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

    public delegate void APIDispatchDelegate(T param, bool bIOFailure);
  }
}
