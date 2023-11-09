using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public sealed class NightBehavior : MonoBehaviour
{

    [SerializeField] Image night;

    [SerializeField] Color day_color;
    [SerializeField] Color night_color;

    [SerializeField] float time = 5.0f;


    private void Start()
    {
        night = GetComponent<Image>();  
    }

    [ContextMenu("ToNight")]
    public void ToNight()
    {
        StartCoroutine(ToNightRoutine());
    }

    IEnumerator ToNightRoutine()
    {
        float elapsedTime = 0.0f;
        while (elapsedTime <= time)
        {
            night.color = Color.Lerp(day_color, night_color, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    [ContextMenu("ToDay")]
    public void ToDay()
    {
        StartCoroutine(ToDayRoutine());
    }

    IEnumerator ToDayRoutine()
    {
        float elapsedTime = 0.0f;
        while (elapsedTime <= time)
        {
            night.color = Color.Lerp(night_color, day_color,elapsedTime/time );
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
