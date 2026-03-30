using UnityEngine;

public abstract class SpawnerActionPresenterScript : MonoBehaviour
{
    [SerializeField] protected FaceMaterialViewScript materialHolder; //REPLACE REPLACE REPLACE

    [SerializeField] protected Material materialFaceAction;
    
    public virtual void SetFaceActionMaterial(Material material) {}

    public virtual void ApplyFaceActionMaterial(GameObject face) {}

    public virtual void ChangeFaceBackToDefault(GameObject face) {}
}