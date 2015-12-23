// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerSkills
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System;

namespace SDG.Unturned
{
  public class PlayerSkills : PlayerCaller
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 6;
    public static readonly byte SPECIALITIES = (byte) 3;
    public static readonly byte BOOST_COUNT = (byte) 4;
    public static readonly uint BOOST_COST = 25U;
    public ExperienceUpdated onExperienceUpdated;
    public BoostUpdated onBoostUpdated;
    public SkillsUpdated onSkillsUpdated;
    private Skill[][] _skills;
    private EPlayerBoost _boost;
    private uint _experience;
    private bool wasLoaded;

    public Skill[][] skills
    {
      get
      {
        return this._skills;
      }
    }

    public EPlayerBoost boost
    {
      get
      {
        return this._boost;
      }
    }

    public uint experience
    {
      get
      {
        return this._experience;
      }
    }

    [SteamCall]
    public void tellExperience(CSteamID steamID, uint newExperience)
    {
      if (!this.channel.checkServer(steamID))
        return;
      if (this.channel.isOwner && newExperience > this.experience && Level.info.type != ELevelType.HORDE)
      {
        int data;
        if (this.wasLoaded && Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Experience", out data))
          Provider.provider.statisticsService.userStatisticsService.setStatistic("Found_Experience", data + ((int) newExperience - (int) this.experience));
        PlayerUI.message(EPlayerMessage.EXPERIENCE, (newExperience - this.experience).ToString());
      }
      this.wasLoaded = true;
      this._experience = newExperience;
      if (this.onExperienceUpdated == null)
        return;
      this.onExperienceUpdated(this.experience);
    }

    [SteamCall]
    public void tellBoost(CSteamID steamID, byte newBoost)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this._boost = (EPlayerBoost) newBoost;
      if (this.onBoostUpdated == null)
        return;
      this.onBoostUpdated(this.boost);
    }

    [SteamCall]
    public void tellSkill(CSteamID steamID, byte speciality, byte index, byte level)
    {
      if (!this.channel.checkServer(steamID) || (int) index >= this.skills[(int) speciality].Length)
        return;
      this.skills[(int) speciality][(int) index].level = level;
      if (this.channel.isOwner)
      {
        bool flag1 = true;
        bool flag2 = true;
        bool flag3 = true;
        for (int index1 = 0; index1 < this.skills[0].Length; ++index1)
        {
          if ((int) this.skills[0][index1].level < (int) this.skills[0][index1].max)
          {
            flag1 = false;
            break;
          }
        }
        for (int index1 = 0; index1 < this.skills[1].Length; ++index1)
        {
          if ((int) this.skills[1][index1].level < (int) this.skills[1][index1].max)
          {
            flag2 = false;
            break;
          }
        }
        for (int index1 = 0; index1 < this.skills[2].Length; ++index1)
        {
          if ((int) this.skills[2][index1].level < (int) this.skills[2][index1].max)
          {
            flag3 = false;
            break;
          }
        }
        bool has1;
        if (flag1 && Provider.provider.achievementsService.getAchievement("Offense", out has1) && !has1)
          Provider.provider.achievementsService.setAchievement("Offense");
        bool has2;
        if (flag2 && Provider.provider.achievementsService.getAchievement("Defense", out has2) && !has2)
          Provider.provider.achievementsService.setAchievement("Defense");
        bool has3;
        if (flag3 && Provider.provider.achievementsService.getAchievement("Support", out has3) && !has3)
          Provider.provider.achievementsService.setAchievement("Support");
        bool has4;
        if (flag1 && flag2 && (flag3 && Provider.provider.achievementsService.getAchievement("Mastermind", out has4)) && !has4)
          Provider.provider.achievementsService.setAchievement("Mastermind");
      }
      if (this.onSkillsUpdated == null)
        return;
      this.onSkillsUpdated();
    }

    public float mastery(int speciality, int index)
    {
      return this.skills[speciality][index].mastery;
    }

    public uint cost(int speciality, int index)
    {
      return this.skills[speciality][index].cost;
    }

    public void askAward(uint award)
    {
      if (Provider.mode == EGameMode.PRO)
        award *= 2U;
      if (this.channel.isOwner)
      {
        this.channel.send("tellExperience", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
        {
          (object) (uint) ((int) this.experience + (int) award)
        });
      }
      else
      {
        this._experience += award;
        this.channel.send("tellExperience", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
        {
          (object) this.experience
        });
      }
    }

    [SteamCall]
    public void askUpgrade(CSteamID steamID, byte speciality, byte index)
    {
      if (!this.channel.checkOwner(steamID) || !Provider.isServer || ((int) speciality >= (int) PlayerSkills.SPECIALITIES || (int) index >= this.skills[(int) speciality].Length))
        return;
      Skill skill = this.skills[(int) speciality][(int) index];
      if (this.experience < this.cost((int) speciality, (int) index) || (int) skill.level >= (int) skill.max)
        return;
      this._experience -= this.cost((int) speciality, (int) index);
      ++skill.level;
      this.channel.send("tellExperience", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) this.experience
      });
      this.channel.send("tellSkill", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[3]
      {
        (object) speciality,
        (object) index,
        (object) skill.level
      });
    }

    [SteamCall]
    public void askBoost(CSteamID steamID)
    {
      if (!this.channel.checkOwner(steamID) || !Provider.isServer || this.experience < PlayerSkills.BOOST_COST)
        return;
      this._experience -= PlayerSkills.BOOST_COST;
      byte num;
      do
      {
        num = (byte) UnityEngine.Random.Range(1, (int) PlayerSkills.BOOST_COUNT + 1);
      }
      while ((int) num == (int) (byte) this.boost);
      this._boost = (EPlayerBoost) num;
      this.channel.send("tellExperience", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) this.experience
      });
      this.channel.send("tellBoost", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) (byte) this.boost
      });
    }

    [SteamCall]
    public void askPurchase(CSteamID steamID, byte index)
    {
      if (!this.channel.checkOwner(steamID) || !Provider.isServer || (int) index >= LevelNodes.nodes.Count)
        return;
      PurchaseNode purchaseNode;
      try
      {
        purchaseNode = (PurchaseNode) LevelNodes.nodes[(int) index];
      }
      catch
      {
        return;
      }
      if (this.experience < purchaseNode.cost)
        return;
      this._experience -= purchaseNode.cost;
      this.channel.send("tellExperience", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) this.experience
      });
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, purchaseNode.id);
      if (itemAsset.type == EItemType.GUN && this.player.inventory.has(purchaseNode.id) != null)
        this.player.inventory.tryAddItem(new Item(((ItemGunAsset) itemAsset).magazineID, true), true);
      else
        this.player.inventory.tryAddItem(new Item(purchaseNode.id, true), true);
    }

    public void sendUpgrade(byte speciality, byte index)
    {
      this.channel.send("askUpgrade", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[2]
      {
        (object) speciality,
        (object) index
      });
    }

    public void sendBoost()
    {
      this.channel.send("askBoost", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
    }

    public void sendPurchase(PurchaseNode node)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      this.channel.send("askPurchase", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[1]
      {
        (object) (byte) LevelNodes.nodes.FindIndex(new Predicate<Node>(new PlayerSkills.\u003CsendPurchase\u003Ec__AnonStoreyE()
        {
          node = node
        }.\u003C\u003Em__B))
      });
    }

    [SteamCall]
    public void tellSkills(CSteamID steamID, byte speciality, byte[] newLevels)
    {
      if (!this.channel.checkServer(steamID))
        return;
      for (byte index = (byte) 0; (int) index < newLevels.Length && (int) index < this.skills[(int) speciality].Length; ++index)
        this.skills[(int) speciality][(int) index].level = newLevels[(int) index];
      if (this.onSkillsUpdated == null)
        return;
      this.onSkillsUpdated();
    }

    [SteamCall]
    public void askSkills(CSteamID steamID)
    {
      if (!Provider.isServer)
        return;
      for (byte index1 = (byte) 0; (int) index1 < this.skills.Length; ++index1)
      {
        byte[] numArray = new byte[this.skills[(int) index1].Length];
        for (byte index2 = (byte) 0; (int) index2 < numArray.Length; ++index2)
          numArray[(int) index2] = this.skills[(int) index1][(int) index2].level;
        this.channel.send("tellSkills", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[2]
        {
          (object) index1,
          (object) numArray
        });
      }
      this.channel.send("tellExperience", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) this.experience
      });
      this.channel.send("tellBoost", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) (byte) this.boost
      });
    }

    private void onLifeUpdated(bool isDead)
    {
      if (!isDead || !Provider.isServer)
        return;
      for (byte index1 = (byte) 0; (int) index1 < this.skills.Length; ++index1)
      {
        if ((int) index1 != (int) (byte) this.channel.owner.speciality)
        {
          byte[] numArray = new byte[this.skills[(int) index1].Length];
          for (byte index2 = (byte) 0; (int) index2 < numArray.Length; ++index2)
            numArray[(int) index2] = (byte) ((double) this.skills[(int) index1][(int) index2].level * 0.75);
          this.channel.send("tellSkills", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[2]
          {
            (object) index1,
            (object) numArray
          });
        }
      }
      this._experience = (uint) ((double) this.experience * 0.75);
      this._boost = EPlayerBoost.NONE;
      this.channel.send("tellExperience", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) this.experience
      });
      this.channel.send("tellBoost", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) (byte) this.boost
      });
    }

    public void init()
    {
      this.channel.send("askSkills", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER);
    }

    private void Start()
    {
      this._skills = new Skill[(int) PlayerSkills.SPECIALITIES][];
      this.skills[0] = new Skill[7];
      this.skills[0][0] = new Skill((byte) 0, (byte) 7, 10U);
      this.skills[0][1] = new Skill((byte) 0, (byte) 7, 10U);
      this.skills[0][2] = new Skill((byte) 0, (byte) 5, 10U);
      this.skills[0][3] = new Skill((byte) 0, (byte) 5, 10U);
      this.skills[0][4] = new Skill((byte) 0, (byte) 5, 10U);
      this.skills[0][5] = new Skill((byte) 0, (byte) 5, 10U);
      this.skills[0][6] = new Skill((byte) 0, (byte) 5, 20U);
      this.skills[1] = new Skill[7];
      this.skills[1][0] = new Skill((byte) 0, (byte) 7, 10U);
      this.skills[1][1] = new Skill((byte) 0, (byte) 5, 10U);
      this.skills[1][2] = new Skill((byte) 0, (byte) 5, 10U);
      this.skills[1][3] = new Skill((byte) 0, (byte) 5, 10U);
      this.skills[1][4] = new Skill((byte) 0, (byte) 5, 10U);
      this.skills[1][5] = new Skill((byte) 0, (byte) 5, 10U);
      this.skills[1][6] = new Skill((byte) 0, (byte) 5, 10U);
      this.skills[2] = new Skill[8];
      this.skills[2][0] = new Skill((byte) 0, (byte) 7, 10U);
      this.skills[2][1] = new Skill((byte) 0, (byte) 3, 20U);
      this.skills[2][2] = new Skill((byte) 0, (byte) 5, 10U);
      this.skills[2][3] = new Skill((byte) 0, (byte) 3, 20U);
      this.skills[2][4] = new Skill((byte) 0, (byte) 5, 10U);
      this.skills[2][5] = new Skill((byte) 0, (byte) 7, 10U);
      this.skills[2][6] = new Skill((byte) 0, (byte) 5, 10U);
      this.skills[2][7] = new Skill((byte) 0, (byte) 3, 20U);
      if (Provider.isServer)
      {
        this.load();
        this.player.life.onLifeUpdated += new LifeUpdated(this.onLifeUpdated);
      }
      else
        this._experience = uint.MaxValue;
      this.Invoke("init", 0.1f);
    }

    public void load()
    {
      if (!PlayerSavedata.fileExists(this.channel.owner.playerID, "/Player/Skills.dat") || Level.info.type != ELevelType.SURVIVAL)
        return;
      Block block = PlayerSavedata.readBlock(this.channel.owner.playerID, "/Player/Skills.dat", (byte) 0);
      byte num = block.readByte();
      if ((int) num <= 4)
        return;
      this._experience = block.readUInt32();
      this._boost = (EPlayerBoost) block.readByte();
      if ((int) num < 6)
        return;
      for (byte index1 = (byte) 0; (int) index1 < this.skills.Length; ++index1)
      {
        if (this.skills[(int) index1] != null)
        {
          for (byte index2 = (byte) 0; (int) index2 < this.skills[(int) index1].Length; ++index2)
          {
            this.skills[(int) index1][(int) index2].level = block.readByte();
            if ((int) this.skills[(int) index1][(int) index2].level > (int) this.skills[(int) index1][(int) index2].max)
              this.skills[(int) index1][(int) index2].level = this.skills[(int) index1][(int) index2].max;
          }
        }
      }
    }

    public void save()
    {
      Block block = new Block();
      block.writeByte(PlayerSkills.SAVEDATA_VERSION);
      block.writeUInt32(this.experience);
      block.writeByte((byte) this.boost);
      for (byte index1 = (byte) 0; (int) index1 < this.skills.Length; ++index1)
      {
        if (this.skills[(int) index1] != null)
        {
          for (byte index2 = (byte) 0; (int) index2 < this.skills[(int) index1].Length; ++index2)
            block.writeByte(this.skills[(int) index1][(int) index2].level);
        }
      }
      PlayerSavedata.writeBlock(this.channel.owner.playerID, "/Player/Skills.dat", block);
    }
  }
}
