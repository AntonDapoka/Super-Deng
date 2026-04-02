using UnityEngine;

public class BonusFaceScript
{
    private int id;
    private float timer; //Local
    private bool isTime = true;
    private readonly GameObject face;
    private readonly FaceScript faceScript;
    private readonly FaceStateScript faceState;
    private readonly BonusFaceSettings settings;
    private readonly BonusFaceBasicSettings settingsBasic;
    private readonly BonusFaceSpawnerPresenterScript presenter;

    private State state;

    private enum State
    {
        Living,
        Dying,
        Done
    }

    private GameObject bonus;
    private BonusType bonusType;
    private readonly float lifeDuration;
    private readonly float deathDuration;

    public bool IsFinished => state == State.Done;
    private bool isBroken = false;

    public BonusFaceScript(
        GameObject face,
        BonusFaceSettings settings,
        BonusFaceBasicSettings settingsBasic,
        BonusFaceSpawnerPresenterScript presenter,
        BonusType bonusType)
    {
        this.face = face;
        this.settings = settings;
        this.settingsBasic = settingsBasic;
        this.presenter = presenter;
        this.bonusType = bonusType;

        if (settings.isLifeDuration)
            lifeDuration = settings.lifeDurationSeconds;

        if (settings.isDeathDuration)
            deathDuration = settings.deathDurationSeconds;

        faceScript = face.GetComponent<FaceScript>();
        faceState = face.GetComponent<FaceStateScript>();

        StartLiving();
    }

    public void Update()
    {
        if (state == State.Done || isBroken) return;
        if (faceState.GetFaceState(FaceProperty.HavePlayer))
        {
            ForcedBreak();
            return;
        }
        

        switch (state)
        {
            case State.Living: UpdateLiving(); break;
            case State.Dying: UpdateDying(); break;
        }
    }

    private void StartLiving()
    {
        if (state == State.Done || isBroken) return;

        timer = 0f;
        if (settings.isLifeDuration) state = State.Living;
        faceState.SetFaceState(FaceProperty.IsBonus, true);
        faceState.SetBonusType(bonusType, true);
        bonus = null;
        presenter.PresentBonusType(face, ref bonus, settings, settingsBasic);
    }

    private void UpdateLiving()
    {
        AdvanceTimer();
        if (TimerExpired(lifeDuration)) 
        {
            if (settings.isDeathDuration)
                StartDying();
            else Finish();
        }
    }

    private void StartDying()
    {
        timer = 0f;
        state = State.Dying;
    }

    private void UpdateDying()
    {
        AdvanceTimer();
        presenter.PlayBonusDyingAnimation(face, bonus, settings, settingsBasic, timer, deathDuration);
        if (TimerExpired(deathDuration)) Finish();
    }

    private void Finish()
    {        
        faceState.SetFaceState(FaceProperty.IsBonus, false);
        faceState.SetBonusType(bonusType, false);
        presenter.ChangeFaceBackToDefault(face);
        presenter.DestroyBonus(bonus);
        bonus = null;
        state = State.Done;
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
        faceState.SetBonusType(bonusType, false);

        presenter.ChangeFaceBackToDefault(face);
        presenter.DestroyBonus(bonus);
        bonus = null;
        state = State.Done;
    }
}