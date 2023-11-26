
using UnityEngine;
using TMPro;

public sealed class InfoPanel : MonoBehaviour
{
    [SerializeField] TMP_Text woodCost;
    [SerializeField] TMP_Text stoneCost;

    public void DisplayWoodCost(int cost)
    {
        woodCost.text = cost.ToString();
    }

    public void DisplayStoneCost(int cost)
    {
        stoneCost.text = cost.ToString();
    }

    public void DisplayCost(int wood, int stone)
    {
        DisplayStoneCost(stone);
        DisplayWoodCost(wood);
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

}
