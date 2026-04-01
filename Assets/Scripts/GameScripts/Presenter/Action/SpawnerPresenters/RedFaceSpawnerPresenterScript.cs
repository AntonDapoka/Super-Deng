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

        if (faceState.Get(FaceProperty.IsColored)
        || faceState.Get(FaceProperty.IsKilling)
        || faceState.Get(FaceProperty.IsBlinking)
        || faceState.Get(FaceProperty.IsBonusHealth) /*dd*/)
        {
            return;
        }
        else if (faceState.Get(FaceProperty.IsRight))
            materialHolder.SetMaterial(faceScript.rend, MaterialType.Right);
        else if (faceState.Get(FaceProperty.IsLeft))
            materialHolder.SetMaterial(faceScript.rend, MaterialType.Left);
        else if (faceState.Get(FaceProperty.IsTop))
            materialHolder.SetMaterial(faceScript.rend, MaterialType.Top);
        else 
            materialHolder.SetMaterial(faceScript.rend, MaterialType.Default);
    }
}
