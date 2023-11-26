
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTC : MonoBehaviour
{
    [SerializeField] Button upgradeButton;
    [SerializeField] TownCenter tc;

    private void OnEnable()
    {
        upgradeButton.onClick.AddListener(() => { tc.Upgrade(); });
    }

    private void OnDisable()
    {
        upgradeButton.onClick.RemoveAllListeners();
    }
}