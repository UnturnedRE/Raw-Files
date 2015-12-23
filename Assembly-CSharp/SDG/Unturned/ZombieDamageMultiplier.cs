// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ZombieDamageMultiplier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class ZombieDamageMultiplier : IDamageMultiplier
  {
    public static readonly float MULTIPLIER_EASY = 1.25f;
    public static readonly float MULTIPLIER_HARD = 0.75f;
    public float damage;
    private float leg;
    private float arm;
    private float spine;
    private float skull;

    public ZombieDamageMultiplier(float newDamage, float newLeg, float newArm, float newSpine, float newSkull)
    {
      this.damage = newDamage;
      this.leg = newLeg;
      this.arm = newArm;
      this.spine = newSpine;
      this.skull = newSkull;
    }

    public float multiply(ELimb limb)
    {
      switch (limb)
      {
        case ELimb.LEFT_FOOT:
          return (float) ((double) this.damage * (double) this.leg * (Provider.mode != EGameMode.EASY ? (Provider.mode != EGameMode.HARD ? 1.0 : (double) ZombieDamageMultiplier.MULTIPLIER_HARD) : (double) ZombieDamageMultiplier.MULTIPLIER_EASY));
        case ELimb.LEFT_LEG:
          return (float) ((double) this.damage * (double) this.leg * (Provider.mode != EGameMode.EASY ? (Provider.mode != EGameMode.HARD ? 1.0 : (double) ZombieDamageMultiplier.MULTIPLIER_HARD) : (double) ZombieDamageMultiplier.MULTIPLIER_EASY));
        case ELimb.RIGHT_FOOT:
          return (float) ((double) this.damage * (double) this.leg * (Provider.mode != EGameMode.EASY ? (Provider.mode != EGameMode.HARD ? 1.0 : (double) ZombieDamageMultiplier.MULTIPLIER_HARD) : (double) ZombieDamageMultiplier.MULTIPLIER_EASY));
        case ELimb.RIGHT_LEG:
          return (float) ((double) this.damage * (double) this.leg * (Provider.mode != EGameMode.EASY ? (Provider.mode != EGameMode.HARD ? 1.0 : (double) ZombieDamageMultiplier.MULTIPLIER_HARD) : (double) ZombieDamageMultiplier.MULTIPLIER_EASY));
        case ELimb.LEFT_HAND:
          return (float) ((double) this.damage * (double) this.arm * (Provider.mode != EGameMode.EASY ? (Provider.mode != EGameMode.HARD ? 1.0 : (double) ZombieDamageMultiplier.MULTIPLIER_HARD) : (double) ZombieDamageMultiplier.MULTIPLIER_EASY));
        case ELimb.LEFT_ARM:
          return (float) ((double) this.damage * (double) this.arm * (Provider.mode != EGameMode.EASY ? (Provider.mode != EGameMode.HARD ? 1.0 : (double) ZombieDamageMultiplier.MULTIPLIER_HARD) : (double) ZombieDamageMultiplier.MULTIPLIER_EASY));
        case ELimb.RIGHT_HAND:
          return (float) ((double) this.damage * (double) this.arm * (Provider.mode != EGameMode.EASY ? (Provider.mode != EGameMode.HARD ? 1.0 : (double) ZombieDamageMultiplier.MULTIPLIER_HARD) : (double) ZombieDamageMultiplier.MULTIPLIER_EASY));
        case ELimb.RIGHT_ARM:
          return (float) ((double) this.damage * (double) this.arm * (Provider.mode != EGameMode.EASY ? (Provider.mode != EGameMode.HARD ? 1.0 : (double) ZombieDamageMultiplier.MULTIPLIER_HARD) : (double) ZombieDamageMultiplier.MULTIPLIER_EASY));
        case ELimb.SPINE:
          return (float) ((double) this.damage * (double) this.spine * (Provider.mode != EGameMode.EASY ? (Provider.mode != EGameMode.HARD ? 1.0 : (double) ZombieDamageMultiplier.MULTIPLIER_HARD) : (double) ZombieDamageMultiplier.MULTIPLIER_EASY));
        case ELimb.SKULL:
          return (float) ((double) this.damage * (double) this.skull * (Provider.mode != EGameMode.EASY ? (Provider.mode != EGameMode.HARD ? 1.0 : (double) ZombieDamageMultiplier.MULTIPLIER_HARD) : (double) ZombieDamageMultiplier.MULTIPLIER_EASY));
        default:
          return this.damage;
      }
    }

    public float armor(ELimb limb, Zombie zombie)
    {
      if ((int) zombie.type < LevelZombies.tables.Count)
      {
        if (limb == ELimb.LEFT_FOOT || limb == ELimb.LEFT_LEG || (limb == ELimb.RIGHT_FOOT || limb == ELimb.RIGHT_LEG))
        {
          if ((int) zombie.pants != (int) byte.MaxValue && (int) zombie.pants < LevelZombies.tables[(int) zombie.type].slots[1].table.Count)
          {
            ItemClothingAsset itemClothingAsset = (ItemClothingAsset) Assets.find(EAssetType.ITEM, LevelZombies.tables[(int) zombie.type].slots[1].table[(int) zombie.pants].item);
            if (itemClothingAsset != null)
              return itemClothingAsset.armor;
          }
        }
        else if (limb == ELimb.LEFT_HAND || limb == ELimb.LEFT_ARM || (limb == ELimb.RIGHT_HAND || limb == ELimb.RIGHT_ARM))
        {
          if ((int) zombie.shirt != (int) byte.MaxValue && (int) zombie.shirt < LevelZombies.tables[(int) zombie.type].slots[0].table.Count)
          {
            ItemClothingAsset itemClothingAsset = (ItemClothingAsset) Assets.find(EAssetType.ITEM, LevelZombies.tables[(int) zombie.type].slots[0].table[(int) zombie.shirt].item);
            if (itemClothingAsset != null)
              return itemClothingAsset.armor;
          }
        }
        else if (limb == ELimb.SPINE)
        {
          if ((int) zombie.gear != (int) byte.MaxValue && (int) zombie.gear < LevelZombies.tables[(int) zombie.type].slots[3].table.Count)
          {
            ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, LevelZombies.tables[(int) zombie.type].slots[3].table[(int) zombie.gear].item);
            if (itemAsset != null && itemAsset.type == EItemType.VEST)
              return ((ItemClothingAsset) itemAsset).armor;
          }
          if ((int) zombie.shirt != (int) byte.MaxValue && (int) zombie.shirt < LevelZombies.tables[(int) zombie.type].slots[0].table.Count)
          {
            ItemClothingAsset itemClothingAsset = (ItemClothingAsset) Assets.find(EAssetType.ITEM, LevelZombies.tables[(int) zombie.type].slots[0].table[(int) zombie.shirt].item);
            if (itemClothingAsset != null)
              return itemClothingAsset.armor;
          }
        }
        else if (limb == ELimb.SKULL && (int) zombie.hat != (int) byte.MaxValue && (int) zombie.hat < LevelZombies.tables[(int) zombie.type].slots[2].table.Count)
        {
          ItemClothingAsset itemClothingAsset = (ItemClothingAsset) Assets.find(EAssetType.ITEM, LevelZombies.tables[(int) zombie.type].slots[2].table[(int) zombie.hat].item);
          if (itemClothingAsset != null)
            return itemClothingAsset.armor;
        }
      }
      return 1f;
    }
  }
}
