
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] Image bar;
    [SerializeField] TMP_Text txt;

    public void OnHpUpdate(int current, int max)
    {
        txt.text = $"{current} / {max}";
        bar.fillAmount = (float)current / (float)max;
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
