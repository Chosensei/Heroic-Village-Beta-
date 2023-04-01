using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacementController : MonoBehaviour
{
    public bool occupied = false;
    private BaseTowerController tower;

    public BaseTowerController Tower { get => tower; set => tower = value; }

    private void OnMouseDown()
    {
        if (!occupied)
        {
            UIManager.Instance.ShowBuildTowerMenuUI(this);
        }
        // Also maybe want to pause the game here perhaps? 
    }
    public void TowerPlaced(BaseTowerController tower)
    {
        this.Tower = tower;
        occupied = true; 
    }
    public void TowerRemoved()
    {
        Tower = null; 
        occupied = false;
    }
}
