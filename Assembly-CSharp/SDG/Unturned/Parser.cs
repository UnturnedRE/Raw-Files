// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Parser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace SDG.Unturned
{
  public class Parser
  {
    public static bool trySplitStart(string serial, out string start, out string end)
    {
      start = string.Empty;
      end = string.Empty;
      int length = serial.IndexOf(' ');
      if (length == -1)
        return false;
      start = serial.Substring(0, length);
      end = serial.Substring(length + 1, serial.Length - length - 1);
      return true;
    }

    public static bool trySplitEnd(string serial, out string start, out string end)
    {
      start = string.Empty;
      end = string.Empty;
      int length = serial.LastIndexOf(' ');
      if (length == -1)
        return false;
      start = serial.Substring(0, length);
      end = serial.Substring(length + 1, serial.Length - length - 1);
      return true;
    }

    public static string[] getComponentsFromSerial(string serial, char delimiter)
    {
      List<string> list = new List<string>();
      int num;
      for (int startIndex = 0; startIndex < serial.Length; startIndex = num + 1)
      {
        num = serial.IndexOf(delimiter, startIndex);
        if (num == -1)
        {
          list.Add(serial.Substring(startIndex, serial.Length - startIndex));
          break;
        }
        list.Add(serial.Substring(startIndex, num - startIndex));
      }
      return list.ToArray();
    }

    public static string getSerialFromComponents(char delimiter, params object[] components)
    {
      string str = string.Empty;
      for (int index = 0; index < components.Length; ++index)
      {
        str += components[index].ToString();
        if (index < components.Length - 1)
          str += (string) (object) delimiter;
      }
      return str;
    }

    public static bool checkIP(string ip)
    {
      int num1 = ip.IndexOf('.');
      if (num1 == -1)
        return false;
      int num2 = ip.IndexOf('.', num1 + 1);
      if (num2 == -1)
        return false;
      int num3 = ip.IndexOf('.', num2 + 1);
      return num3 != -1 && ip.IndexOf('.', num3 + 1) == -1;
    }

    public static uint getUInt32FromIP(string ip)
    {
      string[] componentsFromSerial = Parser.getComponentsFromSerial(ip, '.');
      return (uint) ((int) uint.Parse(componentsFromSerial[0]) << 24 | (int) uint.Parse(componentsFromSerial[1]) << 16 | (int) uint.Parse(componentsFromSerial[2]) << 8) | uint.Parse(componentsFromSerial[3]);
    }

    public static string getIPFromUInt32(uint ip)
    {
      return (string) (object) (uint) ((int) (ip >> 24) & (int) byte.MaxValue) + (object) "." + (string) (object) (uint) ((int) (ip >> 16) & (int) byte.MaxValue) + "." + (string) (object) (uint) ((int) (ip >> 8) & (int) byte.MaxValue) + "." + (string) (object) (uint) ((int) ip & (int) byte.MaxValue);
    }
  }
}
