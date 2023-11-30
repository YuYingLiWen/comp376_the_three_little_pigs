
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public sealed class Objectives : MonoBehaviour
{
    [SerializeField] Image objective1Checkbox;
    [SerializeField]TMP_Text currentHouseLevelText;

    [SerializeField] Image objective2Checkbox;
    [SerializeField] TMP_Text wavesLeftText;

    public void UpdateHouseLevel(int level)
    {
        currentHouseLevelText.text = $"CURRENT LEVEL: {level} / 3";
    }

    public void UpdateWavesLeft(int wavesLeft)
    {
        if (wavesLeft < 0) return;
        wavesLeftText.text = $"WAVES LEFT: {wavesLeft}";
    }

    public void OnObjective1Completed()
    {
        objective1Checkbox.color = Color.green;
    }

    public void OnObjective2Completed()
    {
        objective2Checkbox.color = Color.green;
    }
}
