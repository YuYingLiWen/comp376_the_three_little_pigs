
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTower : MonoBehaviour
{
    [SerializeField] Button upgradeButton;
    [SerializeField] Button sellButton;

    [SerializeField] InfoPanel infoPanel;

    private Towers aTower;

    private void OnEnable()
    {
        upgradeButton.onClick.AddListener(() => { aTower.Upgrade();});
        sellButton.onClick.AddListener(() => { aTower.Sell();});

        if (!aTower) return;
        infoPanel.SetActive(true);
        infoPanel.DisplayCost(aTower.UpgradeCostWood, aTower.UpgradeCostStone);
    }

    private void OnDisable()
    {
        upgradeButton.onClick.RemoveAllListeners();
        sellButton.onClick.RemoveAllListeners();

        infoPanel.SetActive(false);

        aTower = null;
    }

    public void SetTower(Towers tower) => this.aTower = tower;
}
