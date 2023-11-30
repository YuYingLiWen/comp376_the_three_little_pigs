using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OverlayUIController : MonoBehaviour
{
    [SerializeField] List<GameObject> menus;

    UpgradeTower towerUpgradeScript;

    private void Awake()
    {
        if (!instance) instance = this;
        else Destroy(gameObject);
    }

    private void OnDisable()
    {
        instance = null;
    }

    public void DisplayBuildMenu(bool active)
    {
        if (active) EnableMenu(Menus.BuildMenu);
        else DisableAllMenus();
    }

    public void DisplayUpgradeTowerMenu(bool active)
    {
        if(active)
        {
            if (!towerUpgradeScript)
            {
                var m = menus.Find(menu => (menu.name == Menus.TowerUpgradeMenu.ToString()));
                towerUpgradeScript = m.GetComponent<UpgradeTower>();
            }
        }
        else
        {
            DisableAllMenus();
        }
    }

    public void DisplayTC_Menu(bool active)
    {
        if (active) EnableMenu(Menus.UpgradeTCMenu);
        else DisableAllMenus();
    }

    void EnableMenu(Menus menu)
    {
        foreach (GameObject m in menus)
        {
            if(m.name == menu.ToString()) m.SetActive(true);
            else m.SetActive(false);
        }
    }

    public void DisplayUpgradeTowerMenu(bool active, GameObject selectedTower)
    {
        DisplayUpgradeTowerMenu(active);
        Debug.Log(selectedTower.GetComponent<Towers>());
        towerUpgradeScript.SetTower(selectedTower.GetComponent<Towers>());
        EnableMenu(Menus.TowerUpgradeMenu);

    }

    public void DisableAllMenus()
    {
        menus.ForEach(menu => menu.SetActive(false)); 
    }

    public void HandleStoneUpdate(int count)
    {
        stoneCountUI.text = count.ToString();
    }

    public void HandleWoodUpdate(int count)
    {
        woodCountUI.text = count.ToString();
    }

    public void HandleNightCycleUpdate(float fill)
    {
        waveImg.fillAmount = fill;
    }


    [SerializeField] TMP_Text woodCountUI;
    [SerializeField] TMP_Text stoneCountUI;
    [SerializeField] TMP_Text waveText;
    [SerializeField] Image waveImg;


    private static OverlayUIController instance = null;
    public static OverlayUIController Instance => instance;

    enum Menus { TowerUpgradeMenu, UpgradeTCMenu, BuildMenu };
}

