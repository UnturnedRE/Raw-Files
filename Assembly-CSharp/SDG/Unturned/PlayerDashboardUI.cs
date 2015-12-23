// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerDashboardUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class PlayerDashboardUI
  {
    public static Sleek container;
    public static bool active;
    private static SleekButtonIcon inventoryButton;
    private static SleekButtonIcon craftingButton;
    private static SleekButtonIcon skillsButton;
    private static SleekButtonIcon informationButton;

    public PlayerDashboardUI()
    {
      Local local = Localization.read("/Player/PlayerDashboard.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Player/Icons/PlayerDashboard/PlayerDashboard.unity3d");
      PlayerDashboardUI.container = new Sleek();
      PlayerDashboardUI.container.positionScale_Y = -1f;
      PlayerDashboardUI.container.positionOffset_X = 10;
      PlayerDashboardUI.container.positionOffset_Y = 10;
      PlayerDashboardUI.container.sizeOffset_X = -20;
      PlayerDashboardUI.container.sizeOffset_Y = -20;
      PlayerDashboardUI.container.sizeScale_X = 1f;
      PlayerDashboardUI.container.sizeScale_Y = 1f;
      PlayerUI.container.add(PlayerDashboardUI.container);
      PlayerDashboardUI.inventoryButton = new SleekButtonIcon((Texture2D) bundle.load("Inventory"));
      PlayerDashboardUI.inventoryButton.sizeOffset_X = -5;
      PlayerDashboardUI.inventoryButton.sizeOffset_Y = 50;
      PlayerDashboardUI.inventoryButton.sizeScale_X = 0.25f;
      PlayerDashboardUI.inventoryButton.text = local.format("Inventory", (object) ControlsSettings.inventory);
      PlayerDashboardUI.inventoryButton.tooltip = local.format("Inventory_Tooltip");
      PlayerDashboardUI.inventoryButton.onClickedButton = new ClickedButton(PlayerDashboardUI.onClickedInventoryButton);
      PlayerDashboardUI.inventoryButton.fontSize = 14;
      PlayerDashboardUI.container.add((Sleek) PlayerDashboardUI.inventoryButton);
      PlayerDashboardUI.craftingButton = new SleekButtonIcon((Texture2D) bundle.load("Crafting"));
      PlayerDashboardUI.craftingButton.positionOffset_X = 5;
      PlayerDashboardUI.craftingButton.positionScale_X = 0.25f;
      PlayerDashboardUI.craftingButton.sizeOffset_X = -10;
      PlayerDashboardUI.craftingButton.sizeOffset_Y = 50;
      PlayerDashboardUI.craftingButton.sizeScale_X = 0.25f;
      PlayerDashboardUI.craftingButton.text = local.format("Crafting", (object) ControlsSettings.crafting);
      PlayerDashboardUI.craftingButton.tooltip = local.format("Crafting_Tooltip");
      PlayerDashboardUI.craftingButton.onClickedButton = new ClickedButton(PlayerDashboardUI.onClickedCraftingButton);
      PlayerDashboardUI.craftingButton.fontSize = 14;
      PlayerDashboardUI.container.add((Sleek) PlayerDashboardUI.craftingButton);
      PlayerDashboardUI.skillsButton = new SleekButtonIcon((Texture2D) bundle.load("Skills"));
      PlayerDashboardUI.skillsButton.positionOffset_X = 5;
      PlayerDashboardUI.skillsButton.positionScale_X = 0.5f;
      PlayerDashboardUI.skillsButton.sizeOffset_X = -10;
      PlayerDashboardUI.skillsButton.sizeOffset_Y = 50;
      PlayerDashboardUI.skillsButton.sizeScale_X = 0.25f;
      PlayerDashboardUI.skillsButton.text = local.format("Skills", (object) ControlsSettings.skills);
      PlayerDashboardUI.skillsButton.tooltip = local.format("Skills_Tooltip");
      PlayerDashboardUI.skillsButton.onClickedButton = new ClickedButton(PlayerDashboardUI.onClickedSkillsButton);
      PlayerDashboardUI.skillsButton.fontSize = 14;
      PlayerDashboardUI.container.add((Sleek) PlayerDashboardUI.skillsButton);
      PlayerDashboardUI.informationButton = new SleekButtonIcon((Texture2D) bundle.load("Information"));
      PlayerDashboardUI.informationButton.positionOffset_X = 5;
      PlayerDashboardUI.informationButton.positionScale_X = 0.75f;
      PlayerDashboardUI.informationButton.sizeOffset_X = -5;
      PlayerDashboardUI.informationButton.sizeOffset_Y = 50;
      PlayerDashboardUI.informationButton.sizeScale_X = 0.25f;
      PlayerDashboardUI.informationButton.text = local.format("Information", (object) ControlsSettings.map);
      PlayerDashboardUI.informationButton.tooltip = local.format("Information_Tooltip");
      PlayerDashboardUI.informationButton.onClickedButton = new ClickedButton(PlayerDashboardUI.onClickedInformationButton);
      PlayerDashboardUI.informationButton.fontSize = 14;
      PlayerDashboardUI.container.add((Sleek) PlayerDashboardUI.informationButton);
      if (Level.info.type == ELevelType.HORDE)
      {
        PlayerDashboardUI.inventoryButton.sizeScale_X = 0.5f;
        PlayerDashboardUI.craftingButton.isVisible = false;
        PlayerDashboardUI.skillsButton.isVisible = false;
        PlayerDashboardUI.informationButton.positionScale_X = 0.5f;
        PlayerDashboardUI.informationButton.sizeScale_X = 0.5f;
      }
      bundle.unload();
      PlayerDashboardInventoryUI dashboardInventoryUi = new PlayerDashboardInventoryUI();
      PlayerDashboardCraftingUI dashboardCraftingUi = new PlayerDashboardCraftingUI();
      PlayerDashboardSkillsUI dashboardSkillsUi = new PlayerDashboardSkillsUI();
      PlayerDashboardInformationUI dashboardInformationUi = new PlayerDashboardInformationUI();
    }

    public static void open()
    {
      if (PlayerDashboardUI.active)
      {
        PlayerDashboardUI.close();
      }
      else
      {
        PlayerDashboardUI.active = true;
        if (PlayerDashboardInventoryUI.active)
        {
          PlayerDashboardInventoryUI.active = false;
          PlayerDashboardInventoryUI.open();
        }
        else if (PlayerDashboardCraftingUI.active)
        {
          PlayerDashboardCraftingUI.active = false;
          PlayerDashboardCraftingUI.open();
        }
        else if (PlayerDashboardSkillsUI.active)
        {
          PlayerDashboardSkillsUI.active = false;
          PlayerDashboardSkillsUI.open();
        }
        else if (PlayerDashboardInformationUI.active)
        {
          PlayerDashboardInformationUI.active = false;
          PlayerDashboardInformationUI.open();
        }
        PlayerDashboardUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!PlayerDashboardUI.active)
        return;
      PlayerDashboardUI.active = false;
      if (PlayerDashboardInventoryUI.active)
      {
        PlayerDashboardInventoryUI.close();
        PlayerDashboardInventoryUI.active = true;
      }
      else if (PlayerDashboardCraftingUI.active)
      {
        PlayerDashboardCraftingUI.close();
        PlayerDashboardCraftingUI.active = true;
      }
      else if (PlayerDashboardSkillsUI.active)
      {
        PlayerDashboardSkillsUI.close();
        PlayerDashboardSkillsUI.active = true;
      }
      else if (PlayerDashboardInformationUI.active)
      {
        PlayerDashboardInformationUI.close();
        PlayerDashboardInformationUI.active = true;
      }
      PlayerDashboardUI.container.lerpPositionScale(0.0f, -1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onClickedInventoryButton(SleekButton button)
    {
      PlayerDashboardCraftingUI.close();
      PlayerDashboardSkillsUI.close();
      PlayerDashboardInformationUI.close();
      if (PlayerDashboardInventoryUI.active)
      {
        PlayerDashboardUI.close();
        PlayerLifeUI.open();
      }
      else
        PlayerDashboardInventoryUI.open();
    }

    private static void onClickedCraftingButton(SleekButton button)
    {
      PlayerDashboardInventoryUI.close();
      PlayerDashboardSkillsUI.close();
      PlayerDashboardInformationUI.close();
      if (PlayerDashboardCraftingUI.active)
      {
        PlayerDashboardUI.close();
        PlayerLifeUI.open();
      }
      else
        PlayerDashboardCraftingUI.open();
    }

    private static void onClickedSkillsButton(SleekButton button)
    {
      PlayerDashboardInventoryUI.close();
      PlayerDashboardCraftingUI.close();
      PlayerDashboardInformationUI.close();
      if (PlayerDashboardSkillsUI.active)
      {
        PlayerDashboardUI.close();
        PlayerLifeUI.open();
      }
      else
        PlayerDashboardSkillsUI.open();
    }

    private static void onClickedInformationButton(SleekButton button)
    {
      PlayerDashboardInventoryUI.close();
      PlayerDashboardCraftingUI.close();
      PlayerDashboardSkillsUI.close();
      if (PlayerDashboardInformationUI.active)
      {
        PlayerDashboardUI.close();
        PlayerLifeUI.open();
      }
      else
        PlayerDashboardInformationUI.open();
    }
  }
}
