
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTower : MonoBehaviour
{
    [SerializeField] Button upgradeButton;
    [SerializeField] Button sellButton;

    private ITower tower;

    private void OnEnable()
    {
        upgradeButton.onClick.AddListener(() => { tower.Upgrade();});
        sellButton.onClick.AddListener(() => { tower.Sell();});
    }

    private void OnDisable()
    {
        upgradeButton.onClick.RemoveAllListeners();
        sellButton.onClick.RemoveAllListeners();

        tower = null;
    }

    public void SetTower(ITower tower) => this.tower = tower;
}
