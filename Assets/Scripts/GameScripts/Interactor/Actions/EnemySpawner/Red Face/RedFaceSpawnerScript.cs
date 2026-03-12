using System.Collections.Generic;
using UnityEngine;

public class RedFaceSpawnerScript : SpawnerActionScript
{
    private List<RedFaceScript> redFaces = new();
    private RedFaceSettings redFaceSettings;
    [SerializeField] private RedFaceBasicSettings redFaceBasicSettings;
    [SerializeField] private RedFaceSpawnerPresenterScript presenter;

    public override void SetSettings(ActionSettingsScript settings)
    {
        if (settings == null || faces == null || settings as RedFaceSettings == null)
        {
            Debug.LogError($"RedFaceSpawner REQUIRES RedFaceSettings, but received {settings?.GetType().Name ?? "null"}");
            return;
        }
        redFaceSettings = settings as RedFaceSettings;
    }

    public override void SetBasicSettings(ActionBasicSettingsScript actionBasicSettings)
    {
        if (actionBasicSettings is not RedFaceBasicSettings redFaceSettings)
        {
            Debug.LogError("actionBasicSettings must be of type RedFaceBasicSettingsScript");
            return;
        }

        redFaceBasicSettings = redFaceSettings;
    }

    public override bool IsSuitableSpecialRequirements()
    {
        return true;
    }

    public override void SetActionFace(GameObject face)
    {
        if (isTurnOn) redFaces.Add(CreateRedFace(face));
    }

    private RedFaceScript CreateRedFace(GameObject face)
    {
        if (face == null)
            Debug.Log("Face null");

        if (redFaceSettings == null)
            Debug.Log("redFaceSettings null");

        if (redFaceBasicSettings == null)
            Debug.Log("redFaceBasicSettings null");

        if (presenter == null)
            Debug.Log("presenter null");

        return new RedFaceScript(face, redFaceSettings, redFaceBasicSettings, presenter);
    }

    private void Update()
    {
        for (int i = redFaces.Count - 1; i >= 0; i--)
        {
            redFaces[i].Update();

            if (redFaces[i].IsFinished)
                redFaces.RemoveAt(i);
        }
    }
}