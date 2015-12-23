// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerVoice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System;
using UnityEngine;

namespace SDG.Unturned
{
  public class PlayerVoice : PlayerCaller
  {
    private static readonly uint FREQUENCY = 8000U;
    private static readonly uint LENGTH = 10U;
    private static readonly uint SAMPLES = PlayerVoice.FREQUENCY * PlayerVoice.LENGTH;
    public Talked onTalked;
    private float[] received;
    private byte[] bufferReceive;
    private byte[] bufferSend;
    private float playback;
    private int write;
    private bool needsPlay;
    private float delayPlay;
    private float lastPlay;
    private float played;
    private float lastTalk;
    private bool isTalking;

    [SteamCall]
    public void tellVoice(CSteamID steamID, byte[] data, int length)
    {
      if (!this.channel.checkOwner(steamID) || Provider.isServer || (!OptionsSettings.chatVoice || this.player.life.isDead) || length <= 4)
        return;
      for (int index = 0; index < length; ++index)
        data[index] = data[index + 4];
      uint nBytesWritten;
      if (SteamUser.DecompressVoice(data, (uint) length, this.bufferReceive, (uint) this.bufferReceive.Length, out nBytesWritten, PlayerVoice.FREQUENCY) != EVoiceResult.k_EVoiceResultOK)
        return;
      this.playback += (float) (nBytesWritten / 2U) / (float) PlayerVoice.FREQUENCY;
      int startIndex = 0;
      while ((long) startIndex < (long) nBytesWritten)
      {
        this.received[this.write] = (float) BitConverter.ToInt16(this.bufferReceive, startIndex) / (float) short.MaxValue;
        ++this.write;
        if ((long) this.write >= (long) PlayerVoice.SAMPLES)
          this.write = 0;
        startIndex += 2;
      }
      this.GetComponent<AudioSource>().clip.SetData(this.received, 0);
      if (this.GetComponent<AudioSource>().isPlaying)
        return;
      this.needsPlay = true;
      if ((double) this.delayPlay > 0.0)
        return;
      this.delayPlay = 0.3f;
    }

    private void Update()
    {
      if (this.channel.isOwner)
      {
        if (!OptionsSettings.chatVoice)
          return;
        if (Input.GetKey(ControlsSettings.voice) && !this.player.life.isDead)
        {
          if (!this.isTalking)
          {
            this.isTalking = true;
            this.lastTalk = Time.realtimeSinceStartup;
            SteamUser.StartVoiceRecording();
            SteamFriends.SetInGameVoiceSpeaking(Provider.user, this.isTalking);
            if (this.onTalked != null)
              this.onTalked(this.isTalking);
          }
        }
        else if ((!Input.GetKey(ControlsSettings.voice) || this.player.life.isDead) && this.isTalking)
        {
          this.isTalking = false;
          SteamUser.StopVoiceRecording();
          SteamFriends.SetInGameVoiceSpeaking(Provider.user, this.isTalking);
          if (this.onTalked != null)
            this.onTalked(this.isTalking);
        }
        if ((double) Time.realtimeSinceStartup - (double) this.lastTalk <= 0.1)
          return;
        this.lastTalk = Time.realtimeSinceStartup;
        uint cbDestBufferSize;
        uint cbUncompressedDestBufferSize;
        if (SteamUser.GetAvailableVoice(out cbDestBufferSize, out cbUncompressedDestBufferSize, 0U) != EVoiceResult.k_EVoiceResultOK || cbDestBufferSize <= 0U)
          return;
        int num = (int) SteamUser.GetVoice(true, this.bufferSend, cbDestBufferSize, out cbDestBufferSize, false, (byte[]) null, cbUncompressedDestBufferSize, out cbUncompressedDestBufferSize, PlayerVoice.FREQUENCY);
        if (cbDestBufferSize <= 0U)
          return;
        for (int index = (int) cbDestBufferSize + 3; index > 3; --index)
          this.bufferSend[index] = this.bufferSend[index - 4];
        this.channel.send("tellVoice", ESteamCall.PEERS, this.transform.position, EffectManager.MEDIUM, ESteamPacket.UPDATE_VOICE, this.bufferSend, (int) cbDestBufferSize);
      }
      else
      {
        if (Provider.isServer || !OptionsSettings.chatVoice)
          return;
        if (this.GetComponent<AudioSource>().isPlaying)
        {
          if ((double) this.lastPlay > (double) this.GetComponent<AudioSource>().time)
            this.played += this.GetComponent<AudioSource>().clip.length;
          this.lastPlay = this.GetComponent<AudioSource>().time;
          if ((double) this.played + (double) this.GetComponent<AudioSource>().time < (double) this.playback)
            return;
          this.GetComponent<AudioSource>().Stop();
          this.GetComponent<AudioSource>().time = 0.0f;
          this.write = 0;
          this.playback = 0.0f;
          this.played = 0.0f;
          this.lastPlay = 0.0f;
          this.needsPlay = false;
        }
        else
        {
          if (!this.needsPlay)
            return;
          this.delayPlay -= Time.deltaTime;
          if ((double) this.delayPlay > 0.0)
            return;
          this.GetComponent<AudioSource>().Play();
        }
      }
    }

    private void Start()
    {
      if (!OptionsSettings.chatVoice)
        return;
      if (this.channel.isOwner)
      {
        this.bufferSend = new byte[8004];
      }
      else
      {
        if (Provider.isServer)
          return;
        this.GetComponent<AudioSource>().clip = AudioClip.Create("Voice", (int) PlayerVoice.SAMPLES, 1, (int) PlayerVoice.FREQUENCY, true, false);
        this.received = new float[(IntPtr) PlayerVoice.SAMPLES];
        this.bufferReceive = new byte[22000];
      }
    }
  }
}
