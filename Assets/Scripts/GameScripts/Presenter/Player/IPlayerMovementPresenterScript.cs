using System.Collections.Generic;
using UnityEngine;

public interface IPlayerMovementPresenterScript 
{
    void UpdatePlayerSides(Dictionary<string, GameObject> sides, GameObject playerFace);
    void UpdateNonPlayerSide(GameObject side);
}
