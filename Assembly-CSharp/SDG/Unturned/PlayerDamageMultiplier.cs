// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerDamageMultiplier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class PlayerDamageMultiplier : IDamageMultiplier
  {
    public float damage;
    private float leg;
    private float arm;
    private float spine;
    private float skull;

    public PlayerDamageMultiplier(float newDamage, float newLeg, float newArm, float newSpine, float newSkull)
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
          return this.damage * this.leg;
        case ELimb.LEFT_LEG:
          return this.damage * this.leg;
        case ELimb.RIGHT_FOOT:
          return this.damage * this.leg;
        case ELimb.RIGHT_LEG:
          return this.damage * this.leg;
        case ELimb.LEFT_HAND:
          return this.damage * this.arm;
        case ELimb.LEFT_ARM:
          return this.damage * this.arm;
        case ELimb.RIGHT_HAND:
          return this.damage * this.arm;
        case ELimb.RIGHT_ARM:
          return this.damage * this.arm;
        case ELimb.SPINE:
          return this.damage * this.spine;
        case ELimb.SKULL:
          return this.damage * this.skull;
        default:
          return this.damage;
      }
    }

    public float armor(ELimb limb, Player player)
    {
      if (limb == ELimb.LEFT_FOOT || limb == ELimb.LEFT_LEG || (limb == ELimb.RIGHT_FOOT || limb == ELimb.RIGHT_LEG))
      {
        if ((int) player.clothing.pants != 0)
        {
          ItemClothingAsset itemClothingAsset = (ItemClothingAsset) Assets.find(EAssetType.ITEM, player.clothing.pants);
          if (itemClothingAsset != null)
          {
            if (Provider.mode != EGameMode.EASY && (int) player.clothing.pantsQuality > 0)
            {
              --player.clothing.pantsQuality;
              player.clothing.sendUpdatePantsQuality();
            }
            return itemClothingAsset.armor + (float) ((1.0 - (double) itemClothingAsset.armor) * (1.0 - (double) player.clothing.pantsQuality / 100.0));
          }
        }
      }
      else if (limb == ELimb.LEFT_HAND || limb == ELimb.LEFT_ARM || (limb == ELimb.RIGHT_HAND || limb == ELimb.RIGHT_ARM))
      {
        if ((int) player.clothing.shirt != 0)
        {
          ItemClothingAsset itemClothingAsset = (ItemClothingAsset) Assets.find(EAssetType.ITEM, player.clothing.shirt);
          if (itemClothingAsset != null)
          {
            if (Provider.mode != EGameMode.EASY && (int) player.clothing.shirtQuality > 0)
            {
              --player.clothing.shirtQuality;
              player.clothing.sendUpdateShirtQuality();
            }
            return itemClothingAsset.armor + (float) ((1.0 - (double) itemClothingAsset.armor) * (1.0 - (double) player.clothing.shirtQuality / 100.0));
          }
        }
      }
      else if (limb == ELimb.SPINE)
      {
        if ((int) player.clothing.vest != 0)
        {
          ItemClothingAsset itemClothingAsset = (ItemClothingAsset) Assets.find(EAssetType.ITEM, player.clothing.vest);
          if (itemClothingAsset != null)
          {
            if (Provider.mode != EGameMode.EASY && (int) player.clothing.vestQuality > 0)
            {
              --player.clothing.vestQuality;
              player.clothing.sendUpdateVestQuality();
            }
            return itemClothingAsset.armor + (float) ((1.0 - (double) itemClothingAsset.armor) * (1.0 - (double) player.clothing.vestQuality / 100.0));
          }
        }
        if ((int) player.clothing.shirt != 0)
        {
          ItemClothingAsset itemClothingAsset = (ItemClothingAsset) Assets.find(EAssetType.ITEM, player.clothing.shirt);
          if (itemClothingAsset != null)
          {
            if (Provider.mode != EGameMode.EASY && (int) player.clothing.shirtQuality > 0)
            {
              --player.clothing.shirtQuality;
              player.clothing.sendUpdateShirtQuality();
            }
            return itemClothingAsset.armor + (float) ((1.0 - (double) itemClothingAsset.armor) * (1.0 - (double) player.clothing.shirtQuality / 100.0));
          }
        }
      }
      else if (limb == ELimb.SKULL && (int) player.clothing.hat != 0)
      {
        ItemClothingAsset itemClothingAsset = (ItemClothingAsset) Assets.find(EAssetType.ITEM, player.clothing.hat);
        if (itemClothingAsset != null)
        {
          if (Provider.mode != EGameMode.EASY && (int) player.clothing.hatQuality > 0)
          {
            --player.clothing.hatQuality;
            player.clothing.sendUpdateHatQuality();
          }
          return itemClothingAsset.armor + (float) ((1.0 - (double) itemClothingAsset.armor) * (1.0 - (double) player.clothing.hatQuality / 100.0));
        }
      }
      return 1f;
    }
  }
}
