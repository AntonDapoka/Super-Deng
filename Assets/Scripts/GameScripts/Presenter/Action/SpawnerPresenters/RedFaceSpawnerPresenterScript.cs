using UnityEngine;

public class RedFaceSpawnerPresenterScript : SpawnerActionPresenterScript
{
    public override void SetFaceActionMaterial(Material material)
    {
        materialFaceAction = material;
    }

    public override void ApplyFaceActionMaterial(GameObject face)
    {
        FaceScript faceScript = face.GetComponent<FaceScript>();
        faceScript.rend.material = materialFaceAction;
    }

    public override void ChangeFaceBackToDefault(GameObject face)
    {
        FaceScript faceScript = face.GetComponent<FaceScript>();
        FaceStateScript faceState = faceScript.FaceState;

        if (faceState.Get(FaceProperty.IsColored)
        || faceState.Get(FaceProperty.IsKilling)
        || faceState.Get(FaceProperty.IsBlinking)
        || faceState.Get(FaceProperty.IsBonusHealth) /*dd*/)
        {
            Debug.Log("Nah");
            return;
        }
        else if (faceState.Get(FaceProperty.IsRight))
            faceScript.rend.material = materialHolder._materials[MaterialType.Right];
        else if (faceState.Get(FaceProperty.IsLeft))
            faceScript.rend.material = materialHolder._materials[MaterialType.Left];
        else if (faceState.Get(FaceProperty.IsTop))
            faceScript.rend.material = materialHolder._materials[MaterialType.Top];
        else 
            faceScript.rend.material = materialHolder._materials[MaterialType.Default];
    }
}
