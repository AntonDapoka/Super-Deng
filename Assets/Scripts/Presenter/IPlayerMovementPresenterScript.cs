using System.Collections.Generic;
using UnityEngine;

public interface IPlayerMovementPresenterScript 
{
    void UpdatePlayerSides(Dictionary<string, GameObject> sides);
    void UpdateNonPlayerSide(GameObject side);
}
