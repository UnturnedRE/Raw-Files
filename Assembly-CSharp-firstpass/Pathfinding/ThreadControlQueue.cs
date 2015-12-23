// Decompiled with JetBrains decompiler
// Type: Pathfinding.ThreadControlQueue
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Threading;

namespace Pathfinding
{
  public class ThreadControlQueue
  {
    private object lockObj = new object();
    private ManualResetEvent block = new ManualResetEvent(true);
    private Path head;
    private Path tail;
    private int numReceivers;
    private bool blocked;
    private int blockedReceivers;
    private bool starving;
    private bool terminate;

    public bool IsEmpty
    {
      get
      {
        return this.head == null;
      }
    }

    public bool IsTerminating
    {
      get
      {
        return this.terminate;
      }
    }

    public bool AllReceiversBlocked
    {
      get
      {
        if (this.blocked)
          return this.blockedReceivers == this.numReceivers;
        return false;
      }
    }

    public ThreadControlQueue(int numReceivers)
    {
      this.numReceivers = numReceivers;
    }

    public void Block()
    {
      lock (this.lockObj)
      {
        this.blocked = true;
        this.block.Reset();
      }
    }

    public void Unblock()
    {
      lock (this.lockObj)
      {
        this.blocked = false;
        this.block.Set();
      }
    }

    public void Lock()
    {
      Monitor.Enter(this.lockObj);
    }

    public void Unlock()
    {
      Monitor.Exit(this.lockObj);
    }

    public void PushFront(Path p)
    {
      if (this.terminate)
        return;
      lock (this.lockObj)
      {
        if (this.tail == null)
        {
          this.head = p;
          this.tail = p;
          if (this.starving && !this.blocked)
          {
            this.starving = false;
            this.block.Set();
          }
          else
            this.starving = false;
        }
        else
        {
          p.next = this.head;
          this.head = p;
        }
      }
    }

    public void Push(Path p)
    {
      if (this.terminate)
        return;
      lock (this.lockObj)
      {
        if (this.tail == null)
        {
          this.head = p;
          this.tail = p;
          if (this.starving && !this.blocked)
          {
            this.starving = false;
            this.block.Set();
          }
          else
            this.starving = false;
        }
        else
        {
          this.tail.next = p;
          this.tail = p;
        }
      }
    }

    private void Starving()
    {
      this.starving = true;
      this.block.Reset();
    }

    public void TerminateReceivers()
    {
      this.terminate = true;
      this.block.Set();
    }

    public Path Pop()
    {
      Monitor.Enter(this.lockObj);
      try
      {
        if (this.terminate)
        {
          ++this.blockedReceivers;
          throw new ThreadControlQueue.QueueTerminationException();
        }
        if (this.head == null)
          this.Starving();
        while (this.blocked || this.starving)
        {
          ++this.blockedReceivers;
          if (this.terminate)
            throw new ThreadControlQueue.QueueTerminationException();
          if (this.blockedReceivers != this.numReceivers && this.blockedReceivers > this.numReceivers)
            throw new InvalidOperationException("More receivers are blocked than specified in constructor (" + (object) this.blockedReceivers + " > " + (string) (object) this.numReceivers + ")");
          Monitor.Exit(this.lockObj);
          this.block.WaitOne();
          Monitor.Enter(this.lockObj);
          --this.blockedReceivers;
          if (this.head == null)
            this.Starving();
        }
        Path path = this.head;
        if (this.head.next == null)
          this.tail = (Path) null;
        this.head = this.head.next;
        return path;
      }
      finally
      {
        Monitor.Exit(this.lockObj);
      }
    }

    public void ReceiverTerminated()
    {
      Monitor.Enter(this.lockObj);
      ++this.blockedReceivers;
      Monitor.Exit(this.lockObj);
    }

    public Path PopNoBlock(bool blockedBefore)
    {
      Monitor.Enter(this.lockObj);
      try
      {
        if (this.terminate)
        {
          ++this.blockedReceivers;
          throw new ThreadControlQueue.QueueTerminationException();
        }
        if (this.head == null)
          this.Starving();
        if (this.blocked || this.starving)
        {
          if (!blockedBefore)
          {
            ++this.blockedReceivers;
            if (this.terminate)
              throw new ThreadControlQueue.QueueTerminationException();
            if (this.blockedReceivers != this.numReceivers && this.blockedReceivers > this.numReceivers)
              throw new InvalidOperationException("More receivers are blocked than specified in constructor (" + (object) this.blockedReceivers + " > " + (string) (object) this.numReceivers + ")");
          }
          return (Path) null;
        }
        if (blockedBefore)
          --this.blockedReceivers;
        Path path = this.head;
        if (this.head.next == null)
          this.tail = (Path) null;
        this.head = this.head.next;
        return path;
      }
      finally
      {
        Monitor.Exit(this.lockObj);
      }
    }

    public class QueueTerminationException : Exception
    {
    }
  }
}
