using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class TowerManager : Singleton<TowerManager>
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
    //public Button[] switchElementButtons = null;
    [Space]
    public Image PlacementTowerUIMenu;
    public Image UpgradeTowerUIMenu;
    //public Image SwitchElementUIMenu = null;

    private BaseTowerController btc;
    private TowerPlacementController tpc;
    private TowerLevelSwitch tls;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetupButtons();
    }
    // tower placement UI
    public void ShowBuildTowerMenu(TowerPlacementController placementPoint)
    {
        //print("Show tower");
        tpc = placementPoint;
        PlacementTowerUIMenu.gameObject.SetActive(true);
    }
    public void CloseBuildTowerMenu()
    {
        PlacementTowerUIMenu.gameObject.SetActive(false);
    }
    public void ShowBuildingUpgradeMenu(BaseTowerController baseTower)
    {
        btc = baseTower;
        UpgradeTowerUIMenu.gameObject.SetActive(true);
    }

    private void SetupButtons()
    {
        VillageHouseButton.onClick.AddListener(() =>
        {
            //GameObject house = Instantiate(villageHousePrefab);
            Transform house = Instantiate(villageHousePrefab.transform.GetChild(0));
            house.transform.position = tpc.transform.position;
            tpc.TowerPlaced(house.GetComponent<BaseTowerController>());
            CloseBuildTowerMenu();
        });
        ArrowTowerButton.onClick.AddListener(() =>
        {
            //GameObject tower = Instantiate(arrowTowerPrefab);
            Transform tower = Instantiate(arrowTowerPrefab.transform.GetChild(0));
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
            tls.UpgradeTowerPrefab();
            UpgradeTowerUIMenu.gameObject.SetActive(false);

            //if (tls.isWizard == true) { SwitchElementUIMenu.gameObject.SetActive(true); }
            //else { SwitchElementUIMenu.gameObject.SetActive(false); }
        });
        SellBuildingButton.onClick.AddListener(() =>
        {
            //TODO
            Debug.Log("Nothing implemented yet!");
        }); 
        //switchElementButtons[0].onClick.AddListener(() =>
        //{
        //    tls.ChangeWizardElementType("Fire");
        //});
        //switchElementButtons[1].onClick.AddListener(() =>
        //{
        //    tls.ChangeWizardElementType("Lightning");
        //});
        //switchElementButtons[2].onClick.AddListener(() =>
        //{
        //    tls.ChangeWizardElementType("Ice");
        //});
    }
}
