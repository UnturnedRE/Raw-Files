// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Character
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class Character
  {
    public ushort shirt;
    public ushort pants;
    public ushort hat;
    public ushort backpack;
    public ushort vest;
    public ushort mask;
    public ushort glasses;
    public ulong packageShirt;
    public ulong packagePants;
    public ulong packageHat;
    public ulong packageBackpack;
    public ulong packageVest;
    public ulong packageMask;
    public ulong packageGlasses;
    public ushort primaryItem;
    public byte[] primaryState;
    public ushort secondaryItem;
    public byte[] secondaryState;
    public byte face;
    public byte hair;
    public byte beard;
    public Color skin;
    public Color color;
    public bool hand;
    public string name;
    public string nick;
    public CSteamID group;
    public EPlayerSpeciality speciality;

    public Character()
    {
      this.face = (byte) Random.Range(0, (int) Customization.FACES_FREE);
      this.hair = (byte) Random.Range(0, (int) Customization.HAIRS_FREE);
      this.beard = (byte) 0;
      this.skin = Customization.SKINS[Random.Range(0, Customization.SKINS.Length)];
      this.color = Customization.COLORS[Random.Range(0, Customization.COLORS.Length)];
      this.hand = false;
      this.primaryState = new byte[0];
      this.secondaryState = new byte[0];
      this.name = Provider.clientName;
      this.nick = Provider.clientName;
      this.group = CSteamID.Nil;
      this.speciality = (EPlayerSpeciality) Random.Range(0, (int) PlayerSkills.SPECIALITIES);
    }

    public Character(ushort newShirt, ushort newPants, ushort newHat, ushort newBackpack, ushort newVest, ushort newMask, ushort newGlasses, ulong newPackageShirt, ulong newPackagePants, ulong newPackageHat, ulong newPackageBackpack, ulong newPackageVest, ulong newPackageMask, ulong newPackageGlasses, ushort newPrimaryItem, byte[] newPrimaryState, ushort newSecondaryItem, byte[] newSecondaryState, byte newFace, byte newHair, byte newBeard, Color newSkin, Color newColor, bool newHand, string newName, string newNick, CSteamID newGroup, EPlayerSpeciality newSpeciality)
    {
      this.shirt = newShirt;
      this.pants = newPants;
      this.hat = newHat;
      this.backpack = newBackpack;
      this.vest = newVest;
      this.mask = newMask;
      this.glasses = newGlasses;
      this.packageShirt = newPackageShirt;
      this.packagePants = newPackagePants;
      this.packageHat = newPackageHat;
      this.packageBackpack = newPackageBackpack;
      this.packageVest = newPackageVest;
      this.packageMask = newPackageMask;
      this.packageGlasses = newPackageGlasses;
      this.primaryItem = newPrimaryItem;
      this.secondaryItem = newSecondaryItem;
      this.primaryState = newPrimaryState;
      this.secondaryState = newSecondaryState;
      this.face = newFace;
      this.hair = newHair;
      this.beard = newBeard;
      this.skin = newSkin;
      this.color = newColor;
      this.hand = newHand;
      this.name = newName;
      this.nick = newNick;
      this.group = newGroup;
      this.speciality = newSpeciality;
    }
  }
}
