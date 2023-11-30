
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTC : MonoBehaviour
{
    [SerializeField] Button upgradeButton;
    [SerializeField] TownCenter tc;

    [SerializeField] InfoPanel infoPanel;

    private void OnEnable()
    {
        upgradeButton.onClick.AddListener(() => { tc.Upgrade(); });

        infoPanel.SetActive(true);
        infoPanel.DisplayCost(tc.UpgradeCostWood, tc.UpgradeCostStone);// TODO: When stone is used put stone here
    }

    private void OnDisable()
    {
        upgradeButton.onClick.RemoveAllListeners();

        infoPanel.SetActive(false);
    }
}