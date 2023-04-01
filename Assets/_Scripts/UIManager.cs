using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 


public class UIManager : Singleton<UIManager>
{
    [Header("Building Prefabs")]
    public GameObject villageHousePrefab;
    public GameObject arrowTowerPrefab;
    public GameObject catapultTowerPrefab;
    public GameObject wizardTowerPrefab;

    [Header("UI Stuff")]
    public Button VillageHouseButton;
    public Button ArrowTowerButton;
    public Button CatapultTowerButton;
    public Button WizardTowerButton;
    public Button SellBuildingButton;
    public Button UpgradeBuildingButton;
    public Button[] switchElementButtons;
    [Space]
    public GameObject PlacementTowerUIMenu1;
    public GameObject PlacementTowerUIMenu2;
    public GameObject UpgradeTowerUIMenu;
    public GameObject SwitchElementUIMenu;
    //private BaseTowerController btc;
    private TowerPlacementController tpc;
    //private TowerLevelSwitch tls;

    [Header("Player UI")]
    public TextMeshProUGUI moneyText;
    [SerializeField] private Image healthbar;
    [SerializeField] private Image manabar;

    [Header("Base Wall UI")]
    public GameObject[] WallUIObjects;
    [SerializeField] private Image[] wallHP;
    

    [Header("Debug")]
    public GameObject currentSelectedBuilding = null; 


    void Start()
    {
        SetupButtons();

        // Default health & mana to maximum in UI 
        healthbar.fillAmount = 1;
        manabar.fillAmount = 1;

        // Set default wall HP
        foreach (Image w in wallHP)
        {
            w.fillAmount = 1;
        }
    }

    void Update()
    {

    }

    // Manage healthbars & mana
    public void UpdateHealthBar(float currentHp, float maxHp)
    {
        healthbar.fillAmount = currentHp / maxHp;
    }
    public void UpdateManaBar(float currentMp, float maxMp)
    {
        manabar.fillAmount = currentMp / maxMp;
    }
    public void UpdateWallHP(int wallIndex, float currentHp, float maxHp)
    {
        wallHP[wallIndex].fillAmount = currentHp / maxHp;
    }

    // tower placement UI
    public void ShowBuildTowerMenuUI(TowerPlacementController placementPoint)
    {
        tpc = placementPoint;
        PlacementTowerUIMenu1.gameObject.SetActive(true);
    }
    public void CloseBuildTowerMenu()
    {
        PlacementTowerUIMenu1.gameObject.SetActive(false);
        PlacementTowerUIMenu2.gameObject.SetActive(false);
    }

    public void SwitchMenuPages()
    {
        if (PlacementTowerUIMenu1.activeSelf)
        {
            PlacementTowerUIMenu1.SetActive(false);
            PlacementTowerUIMenu2.SetActive(true);
        }
        else
        {
            PlacementTowerUIMenu1.SetActive(true);
            PlacementTowerUIMenu2.SetActive(false);
        }
    }
    //public void ShowUpgradeMenu(BaseTowerController baseTower)
    //{
    //    btc = baseTower;
    //    UpgradeTowerUIMenu.SetActive(true);
    //}
    public void ShowUpgradeMenu(TowerLevelSwitch towerLevelSwitch)
    {
        //tls = towerLevelSwitch;
        UpgradeTowerUIMenu.SetActive(true);
    }
    private void SetupButtons()
    {
        VillageHouseButton.onClick.AddListener(() =>
        {
            GameObject house = Instantiate(villageHousePrefab);
            house.transform.position = tpc.transform.position;
            tpc.TowerPlaced(house.GetComponent<BaseTowerController>());
            CloseBuildTowerMenu();
        });
        ArrowTowerButton.onClick.AddListener(() =>
        {
            GameObject tower = Instantiate(arrowTowerPrefab);
            tower.transform.position = tpc.transform.position;
            tpc.TowerPlaced(tower.GetComponent<BaseTowerController>());
            CloseBuildTowerMenu();
        });
        CatapultTowerButton.onClick.AddListener(() =>
        {
            GameObject tower = Instantiate(catapultTowerPrefab);
            tower.transform.position = tpc.transform.position;
            tpc.TowerPlaced(tower.GetComponent<BaseTowerController>());
            CloseBuildTowerMenu();
        });
        WizardTowerButton.onClick.AddListener(() =>
        {
            GameObject tower = Instantiate(wizardTowerPrefab);
            tower.transform.position = tpc.transform.position;
            tpc.TowerPlaced(tower.GetComponent<BaseTowerController>());
            CloseBuildTowerMenu();
        });

        // UPGRADES
        UpgradeBuildingButton.onClick.AddListener(() =>
        {
            currentSelectedBuilding.TryGetComponent(out TowerLevelSwitch tls);
            tls.UpgradeTowerPrefab();
            if (tls.WizardTower == true) { SwitchElementUIMenu.gameObject.SetActive(true); }
            else { SwitchElementUIMenu.gameObject.SetActive(false); }
            //print("Upgraded tower!");
            //tls.UpgradeTestMessage();
        });
        SellBuildingButton.onClick.AddListener(() =>
        {
            currentSelectedBuilding.TryGetComponent(out TowerLevelSwitch tls);
            tls.SellTowerPrefab();
            currentSelectedBuilding = null;
            //print("Sold tower!");
        });
        switchElementButtons[0].onClick.AddListener(() =>
        {
            currentSelectedBuilding.TryGetComponent(out TowerLevelSwitch tls);
            tls.ChangeWizardElementType("Fire");
        });
        switchElementButtons[1].onClick.AddListener(() =>
        {
            currentSelectedBuilding.TryGetComponent(out TowerLevelSwitch tls);
            tls.ChangeWizardElementType("Lightning");
        });
        switchElementButtons[2].onClick.AddListener(() =>
        {
            currentSelectedBuilding.TryGetComponent(out TowerLevelSwitch tls);
            tls.ChangeWizardElementType("Ice");
        });
    }
}
