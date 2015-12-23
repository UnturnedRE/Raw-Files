// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.GraphicsSettingsData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class GraphicsSettingsData
  {
    public GraphicsSettingsResolution Resolution { get; set; }

    public bool IsFullscreenEnabled { get; set; }

    public bool IsVSyncEnabled { get; set; }

    public bool IsMotionBlurEnabled { get; set; }

    public bool IsSSAOEnabled { get; set; }

    public bool IsSunShaftsEnabled { get; set; }

    public bool IsBloomEnabled { get; set; }

    public bool IsCloudEnabled { get; set; }

    public bool IsTerrainEnabled { get; set; }

    public bool IsFogEnabled { get; set; }

    public float DrawDistance { get; set; }

    public EAntiAliasingType AntiAliasingType { get; set; }

    public EGraphicQuality EffectQuality { get; set; }

    public EGraphicQuality FoliageQuality { get; set; }

    public EGraphicQuality LightingQuality { get; set; }

    public EGraphicQuality WaterQuality { get; set; }

    public EGraphicQuality ScopeQuality { get; set; }

    public EGraphicQuality OutlineQuality { get; set; }

    public GraphicsSettingsData()
    {
      this.Resolution = new GraphicsSettingsResolution();
      this.IsFullscreenEnabled = false;
      this.IsVSyncEnabled = false;
      this.IsMotionBlurEnabled = false;
      this.IsSSAOEnabled = false;
      this.IsSunShaftsEnabled = false;
      this.IsBloomEnabled = false;
      this.IsCloudEnabled = true;
      this.IsTerrainEnabled = true;
      this.IsFogEnabled = true;
      this.DrawDistance = 0.0f;
      this.AntiAliasingType = EAntiAliasingType.OFF;
      this.EffectQuality = EGraphicQuality.LOW;
      this.FoliageQuality = EGraphicQuality.LOW;
      this.LightingQuality = EGraphicQuality.LOW;
      this.WaterQuality = EGraphicQuality.LOW;
      this.ScopeQuality = EGraphicQuality.ULTRA;
      this.OutlineQuality = EGraphicQuality.LOW;
    }
  }
}
