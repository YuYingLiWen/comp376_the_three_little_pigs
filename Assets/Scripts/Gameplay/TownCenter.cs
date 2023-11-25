
using UnityEngine;

public class TownCenter : MonoBehaviour, IInteractable
{
    [SerializeField] Transform night_fov;

    private void Start()
    {
        night_fov.localScale = Vector3.one * fovRange;
    }

    void Update()
    {
        night_fov.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    public void OnClick()
    {
        Debug.Log("Clicked " + name);
    }

    public void Deselect()
    {
        Debug.Log("Deseelect " + name);
    }

    float fovRange = 3.0f;
}
