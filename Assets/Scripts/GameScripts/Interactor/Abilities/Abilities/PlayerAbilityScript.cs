using UnityEngine;

public abstract class PlayerAbilityScript : MonoBehaviour, IAbilityScript
{
   public abstract void Activate(GameObject face);
}
