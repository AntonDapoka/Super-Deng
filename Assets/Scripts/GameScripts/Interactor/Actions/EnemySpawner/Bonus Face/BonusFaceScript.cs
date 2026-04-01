using UnityEngine;

public class BonusFaceScript
{
    private int id;
    private float timer; //Local
    private bool isTime = true;
    private readonly GameObject face;
    private readonly FaceScript faceScript;
    private readonly FaceStateScript faceState;
    private readonly BonusFaceSpawnerPresenterScript presenter;

    private State state;

    private enum State
    {
        Living,
        Dying,
        Done
    }

    private readonly float lifeDuration;
    private readonly float deathDuration;
    private readonly Material materialAction;

    public bool IsFinished => state == State.Done;
    private bool isBroken;

    public BonusFaceScript(
        GameObject face,
        BonusFaceSettings settings,
        BonusFaceBasicSettings settingsBasic,
        BonusFaceSpawnerPresenterScript presenter,
        BonusType bonusType)
    {
        this.face = face;
        this.presenter = presenter;

        bool isChange = settings.isBasicSettingsChange;

        if (isChange && settings.isMaterialChange)
            materialAction = settings.material;
        else materialAction = settingsBasic.materialBasic;

        faceScript = face.GetComponent<FaceScript>();
        faceState = face.GetComponent<FaceStateScript>();

        StartLiving(bonusType);
    }

    public void Update()
    {
        if (state == State.Done || isBroken) return;

        switch (state)
        {
            case State.Living: UpdateLiving(); break;
            case State.Dying: UpdateDying(); break;
        }
    }

    private void StartLiving(BonusType bonusType)
    {
        if (state == State.Done || isBroken) return;

        timer = 0f;
        state = State.Living;
        faceState.SetFaceState(FaceProperty.IsBonus, true);
        faceState.SetBonusType(bonusType, true);
    }

    private void UpdateLiving()
    {
        ApplyBonusFaceMaterial();
        AdvanceTimer();
        if (TimerExpired(lifeDuration)) UpdateDying();
    }

    private void UpdateDying()
    {
        timer = 0f;
        state = State.Dying;

        faceState.SetFaceState(FaceProperty.IsKilling, true);
        faceState.SetFaceState(FaceProperty.IsColored, false);
    }

    private void Finish()
    {        
        faceState.SetFaceState(FaceProperty.IsKilling, false);
        presenter.ChangeFaceBackToDefault(face);
        state = State.Done;
    }

    private void ApplyBonusFaceMaterial()
    {
        presenter.ApplyFaceActionMaterial(face, materialAction);
    }

    private void AdvanceTimer()
    {
        if (isTime) timer += Time.deltaTime;
    }

    private bool TimerExpired(float duration)
    {
        return timer >= duration;
    }

    public void ForcedBreak()
    {
        if (isBroken || state == State.Done) return;

        isBroken = true;

        faceState.SetFaceState(FaceProperty.IsBonus, false);
        faceState.SetFaceState(FaceProperty.IsColored, false);

        presenter.ChangeFaceBackToDefault(face);

        state = State.Done;
    }
}