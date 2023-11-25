internal interface IInteractable
{
    /// <summary>
    /// Called when object has been clicked on.
    /// </summary>
    void OnClick();

    /// <summary>
    /// Called when object is no longer the selected.
    /// </summary>
    void Deselect();
}