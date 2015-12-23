// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.NetworkSnapshotBuffer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class NetworkSnapshotBuffer
  {
    private NetworkSnapshot[] snapshots;
    private int readIndex;
    private int writeIndex;
    private Vector3 lastPos;
    private Quaternion lastRot;
    private float readLast;
    private float writeLast;
    private float readDuration;
    private float readDelay;

    public NetworkSnapshotBuffer(float newDuration, float newDelay)
    {
      this.snapshots = new NetworkSnapshot[16];
      this.readIndex = 0;
      this.writeIndex = 0;
      this.readDuration = newDuration;
      this.readDelay = newDelay;
    }

    private int getReadWriteSpace()
    {
      if (this.readIndex <= this.writeIndex)
        return this.writeIndex - this.readIndex;
      return this.writeIndex + (this.snapshots.Length - this.readIndex);
    }

    public void getCurrentSnapshot(out Vector3 pos, out Quaternion rot)
    {
      int readWriteSpace = this.getReadWriteSpace();
      if (readWriteSpace <= 0)
      {
        this.readLast = Time.realtimeSinceStartup;
        pos = this.lastPos;
        rot = this.lastRot;
      }
      else if (readWriteSpace > this.snapshots.Length - 2)
      {
        this.readIndex = this.writeIndex;
        pos = this.lastPos;
        rot = this.lastRot;
      }
      else if ((double) Mathf.Max(this.writeLast, Time.realtimeSinceStartup) - (double) this.snapshots[this.readIndex].timestamp < (double) this.readDelay)
      {
        this.readLast = Time.realtimeSinceStartup;
        pos = this.lastPos;
        rot = this.lastRot;
      }
      else
      {
        if ((double) Time.realtimeSinceStartup - (double) this.readLast > (double) this.readDuration)
        {
          this.lastPos = this.snapshots[this.readIndex].pos;
          this.lastRot = this.snapshots[this.readIndex].rot;
          this.incrementReadIndex();
          this.readLast = Time.realtimeSinceStartup;
        }
        float t = Mathf.Clamp01((Time.realtimeSinceStartup - this.readLast) / this.readDuration);
        pos = Vector3.Lerp(this.lastPos, this.snapshots[this.readIndex].pos, t);
        rot = Quaternion.Slerp(this.lastRot, this.snapshots[this.readIndex].rot, t);
      }
    }

    public void updateLastSnapshot(Vector3 pos, Quaternion rot)
    {
      this.readIndex = 0;
      this.writeIndex = 0;
      this.lastPos = pos;
      this.lastRot = rot;
      this.readLast = Time.realtimeSinceStartup;
    }

    public void addNewSnapshot(Vector3 pos, Quaternion rot)
    {
      this.snapshots[this.writeIndex].pos = pos;
      this.snapshots[this.writeIndex].rot = rot;
      this.snapshots[this.writeIndex].timestamp = Time.realtimeSinceStartup;
      this.incrementWriteIndex();
      this.writeLast = Time.realtimeSinceStartup;
    }

    private void incrementReadIndex()
    {
      ++this.readIndex;
      if (this.readIndex != this.snapshots.Length)
        return;
      this.readIndex = 0;
    }

    private void incrementWriteIndex()
    {
      ++this.writeIndex;
      if (this.writeIndex != this.snapshots.Length)
        return;
      this.writeIndex = 0;
    }
  }
}
