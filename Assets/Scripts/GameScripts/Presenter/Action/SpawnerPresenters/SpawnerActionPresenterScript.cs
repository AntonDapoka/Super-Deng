using UnityEngine;

public abstract class SpawnerActionPresenterScript : MonoBehaviour
{
    [SerializeField] protected FaceMaterialViewScript materialHolder; //REPLACE REPLACE REPLACE

    public virtual void ApplyFaceActionMaterial(GameObject face, Material material) {}

    public virtual void ChangeFaceBackToDefault(GameObject face) {}
}