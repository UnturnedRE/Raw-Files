// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.AnimalDamageMultiplier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class AnimalDamageMultiplier : IDamageMultiplier
  {
    public static readonly float MULTIPLIER_EASY = 1.25f;
    public static readonly float MULTIPLIER_HARD = 0.75f;
    public float damage;
    private float leg;
    private float spine;
    private float skull;

    public AnimalDamageMultiplier(float newDamage, float newLeg, float newSpine, float newSkull)
    {
      this.damage = newDamage;
      this.leg = newLeg;
      this.spine = newSpine;
      this.skull = newSkull;
    }

    public float multiply(ELimb limb)
    {
      switch (limb)
      {
        case ELimb.LEFT_BACK:
          return (float) ((double) this.damage * (double) this.leg * (Provider.mode != EGameMode.EASY ? (Provider.mode != EGameMode.HARD ? 1.0 : (double) AnimalDamageMultiplier.MULTIPLIER_HARD) : (double) AnimalDamageMultiplier.MULTIPLIER_EASY));
        case ELimb.RIGHT_BACK:
          return (float) ((double) this.damage * (double) this.leg * (Provider.mode != EGameMode.EASY ? (Provider.mode != EGameMode.HARD ? 1.0 : (double) AnimalDamageMultiplier.MULTIPLIER_HARD) : (double) AnimalDamageMultiplier.MULTIPLIER_EASY));
        case ELimb.LEFT_FRONT:
          return (float) ((double) this.damage * (double) this.leg * (Provider.mode != EGameMode.EASY ? (Provider.mode != EGameMode.HARD ? 1.0 : (double) AnimalDamageMultiplier.MULTIPLIER_HARD) : (double) AnimalDamageMultiplier.MULTIPLIER_EASY));
        case ELimb.RIGHT_FRONT:
          return (float) ((double) this.damage * (double) this.leg * (Provider.mode != EGameMode.EASY ? (Provider.mode != EGameMode.HARD ? 1.0 : (double) AnimalDamageMultiplier.MULTIPLIER_HARD) : (double) AnimalDamageMultiplier.MULTIPLIER_EASY));
        case ELimb.SPINE:
          return (float) ((double) this.damage * (double) this.spine * (Provider.mode != EGameMode.EASY ? (Provider.mode != EGameMode.HARD ? 1.0 : (double) AnimalDamageMultiplier.MULTIPLIER_HARD) : (double) AnimalDamageMultiplier.MULTIPLIER_EASY));
        case ELimb.SKULL:
          return (float) ((double) this.damage * (double) this.skull * (Provider.mode != EGameMode.EASY ? (Provider.mode != EGameMode.HARD ? 1.0 : (double) AnimalDamageMultiplier.MULTIPLIER_HARD) : (double) AnimalDamageMultiplier.MULTIPLIER_EASY));
        default:
          return this.damage;
      }
    }
  }
}
