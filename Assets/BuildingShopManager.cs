using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BuildingShopManager : Singleton<BuildingShopManager>
{
    [Header("Building Menu variables")]
    public Image currentLevelSprite;
    public Image nextLevelSprite;
    public TMP_Text BuildingName1;
    public TMP_Text BuildingName2;
    public TMP_Text CurrentLevelValue;
    public TMP_Text StatText1;
    public TMP_Text StatText2;
    public TMP_Text StatText3;
    public TMP_Text StatText4;
    public TMP_Text StatValue1;
    public TMP_Text StatValue2;
    public TMP_Text StatValue3;
    public TMP_Text StatValue4;
    public TMP_Text SellPriceValue;
    public TMP_Text NextLevelValue;
    public TMP_Text UpgradedStatText1;
    public TMP_Text UpgradedStatText2;
    public TMP_Text UpgradedStatText3;
    public TMP_Text UpgradedStatText4;
    public TMP_Text UpgradedStatValue1;
    public TMP_Text UpgradedStatValue2;
    public TMP_Text UpgradedStatValue3;
    public TMP_Text UpgradedStatValue4;
    public TMP_Text UpgradeCostValue;

}
