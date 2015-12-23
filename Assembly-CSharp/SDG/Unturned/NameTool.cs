// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.NameTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class NameTool
  {
    public static bool checkNames(string input, string name)
    {
      if (input.Length <= name.Length)
        return name.ToLower().IndexOf(input.ToLower()) != -1;
      return false;
    }

    public static bool isValid(string name)
    {
      foreach (int num in name.ToCharArray())
      {
        if (num <= 31 || num >= 126 || (num == 47 || num == 92) || num == 96)
          return false;
      }
      return true;
    }
  }
}
