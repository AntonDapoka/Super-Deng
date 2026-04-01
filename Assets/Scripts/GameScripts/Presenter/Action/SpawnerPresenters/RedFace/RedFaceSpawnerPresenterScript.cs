using UnityEngine;

public class RedFaceSpawnerPresenterScript : SpawnerActionPresenterScript
{
    public override void ApplyFaceActionMaterial(GameObject face, Material material)
    {
        FaceScript faceScript = face.GetComponent<FaceScript>();
        faceScript.rend.material = material;
    }

    public override void ChangeFaceBackToDefault(GameObject face)
    {
        FaceScript faceScript = face.GetComponent<FaceScript>();
        FaceStateScript faceState = faceScript.FaceState;

        if (faceState.GetFaceState(FaceProperty.IsColored)
        || faceState.GetFaceState(FaceProperty.IsKilling)
        || faceState.GetFaceState(FaceProperty.IsBlinking)
        || faceState.GetFaceState(FaceProperty.IsBonus) /*dd*/)
        {
            return;
        }
        else if (faceState.GetFaceState(FaceProperty.IsRight))
            materialHolder.SetMaterial(faceScript.rend, MaterialType.Right);
        else if (faceState.GetFaceState(FaceProperty.IsLeft))
            materialHolder.SetMaterial(faceScript.rend, MaterialType.Left);
        else if (faceState.GetFaceState(FaceProperty.IsTop))
            materialHolder.SetMaterial(faceScript.rend, MaterialType.Top);
        else 
            materialHolder.SetMaterial(faceScript.rend, MaterialType.Default);
    }
}
