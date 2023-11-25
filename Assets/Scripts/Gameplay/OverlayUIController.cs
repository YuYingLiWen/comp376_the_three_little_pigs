﻿using UnityEngine;

public class OverlayUIController : MonoBehaviour
{
    [SerializeField] GameObject playerBuildMenu;
    [SerializeField] GameObject upgradeTowerUI;
    UpgradeTower towerUpgradeScript;

    private void OnEnable()
    {
        playerBuildMenu.SetActive(false);
        upgradeTowerUI.SetActive(false);
    }

    public void DisplayBuildMenu(bool active)
    {
        playerBuildMenu.SetActive(active);
    }

    public void DisplayUpgradeTowerMenu(bool active)
    {
        upgradeTowerUI.SetActive(active);

        if (!active) return;

        if (!towerUpgradeScript) towerUpgradeScript = upgradeTowerUI.GetComponent<UpgradeTower>();
    }

    public void DisplayUpgradeTowerMenu(bool active, GameObject selectedTower)
    {
        DisplayUpgradeTowerMenu(active);

        towerUpgradeScript.SetTower(selectedTower.GetComponent<Towers>());
    }
}