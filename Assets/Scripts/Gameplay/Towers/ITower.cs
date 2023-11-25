using UnityEngine;

public interface ITower
{
    /// <summary>
    /// To have man the tower.
    /// </summary>
    void Garrison(GameObject pig);
    
    /// <summary>
    /// To have ALL units abandon tower.
    /// </summary>
    void Abandon();

    /// <summary>
    /// Sell the tower.
    /// </summary>
    void Sell();
}
