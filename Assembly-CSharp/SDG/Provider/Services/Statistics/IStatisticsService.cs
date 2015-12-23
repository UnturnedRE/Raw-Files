// Decompiled with JetBrains decompiler
// Type: SDG.Provider.Services.Statistics.IStatisticsService
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services;
using SDG.Provider.Services.Statistics.Global;
using SDG.Provider.Services.Statistics.User;

namespace SDG.Provider.Services.Statistics
{
  public interface IStatisticsService : IService
  {
    IUserStatisticsService userStatisticsService { get; }

    IGlobalStatisticsService globalStatisticsService { get; }
  }
}
