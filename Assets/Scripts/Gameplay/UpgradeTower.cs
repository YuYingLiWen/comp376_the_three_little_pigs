
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTower : MonoBehaviour
{
    [SerializeField] Button upgradeButton;
    [SerializeField] Button sellButton;

    private Towers aTower;

    private void OnEnable()
    {
        upgradeButton.onClick.AddListener(() => { aTower.Upgrade();});
        sellButton.onClick.AddListener(() => { aTower.Sell();});
    }

    private void OnDisable()
    {
        upgradeButton.onClick.RemoveAllListeners();
        sellButton.onClick.RemoveAllListeners();

        aTower = null;
    }

    public void SetTower(Towers tower) => this.aTower = tower;
}
