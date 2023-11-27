using TMPro;
using UnityEngine;

public class FontDictator : MonoBehaviour
{
    [SerializeField] TMP_FontAsset font;

    void Awake()
    {
        var textField = FindObjectsByType<TMP_Text>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach(var field in textField)
        {
            field.font = font;
        }
    }
}
