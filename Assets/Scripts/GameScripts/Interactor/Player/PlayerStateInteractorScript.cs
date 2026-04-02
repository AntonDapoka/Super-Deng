using System;
using System.Collections;
using UnityEngine;

public class PlayerStateInteractorScript : MonoBehaviour
{
    [SerializeField] PlayerStatePresenterScript playerStatePresenter;
    [SerializeField] FaceStateScript faceState;

    [SerializeField] private int hp = 4;
    [SerializeField] private float durationSecondsInvincible = 1f;

    [SerializeField] private bool isLosing = false;
    [SerializeField] private bool isTakingDamage = false;
    [SerializeField] private bool isTakingHealth = false;
    [SerializeField] private bool isBlinking = false;
    [SerializeField] private bool isColored = false;
    [SerializeField] private bool isInvincible = false;

    [SerializeField] private bool isFaceKilling = false;
    [SerializeField] private bool isFaceBonus = false;
    [SerializeField] private bool isFaceBlinking = false;
    [SerializeField] private bool isFaceColored = false;

    public void SetCurrentFace(GameObject face)
    {
        faceState = face.GetComponent<FaceStateScript>();
        playerStatePresenter.SetNewHP(hp); // Move to Initialize()
    }

    private void Update()
    {
        if (faceState == null) return;

        isFaceColored = faceState.GetFaceState(FaceProperty.IsColored);
        isFaceKilling = faceState.GetFaceState(FaceProperty.IsKilling);
        isFaceBlinking = faceState.GetFaceState(FaceProperty.IsBlinking);
        isFaceBonus = faceState.GetFaceState(FaceProperty.IsBonus);

        if (!isFaceColored &&
            !isFaceBlinking &&
            !isFaceKilling &&
            !isFaceBonus)
        {
            playerStatePresenter.DisplayHP();
            isColored = false;
            isBlinking = false;
            isTakingDamage = false;
            return;
        }

        HandleState(
            isFaceKilling || isFaceColored,
            ref isColored,
            playerStatePresenter.SetColoredState,
            playerStatePresenter.RemoveColoredState
        );

        HandleState(
            isFaceBlinking,
            ref isBlinking,
            playerStatePresenter.SetBlinkingState,
            playerStatePresenter.RemoveBlinkingState
        );

        if (isFaceBonus && faceState.GetBonusType(BonusType.Health) && !isTakingHealth) 
        { 
            isTakingHealth = true; 
            TakeHP(); 
            playerStatePresenter.SetTakingHealthState();
        } 
        if (!isFaceBonus && !faceState.GetBonusType(BonusType.Health) && isTakingHealth) 
        { 
            isTakingHealth = false; 
            playerStatePresenter.RemoveTakingHealthState();
        }

        if (isFaceKilling && !isTakingDamage && !isInvincible) 
        { 
            isTakingDamage = true; 
            TakeDamage(); 
            playerStatePresenter.SetInvincibilityFramesState(); 
        } 
        if (!isFaceKilling && isTakingDamage) 
        { 
            isTakingDamage = false; 
        }
    }

    private void HandleState(bool condition, ref bool state, Action onEnter, Action onExit)
    {
        if (condition == state) return;

        state = condition;

        if (state) onEnter?.Invoke();
        else onExit?.Invoke();
    }

    private void TakeDamage()
    {
        hp = Mathf.Max(0, hp - 1);

        if (hp > 0)
            StartCoroutine(UsingInvincibilityFrames());
        else if (!isLosing)
        {
            // StartLosing();
        }

        playerStatePresenter.SetNewHP(hp);
    }

    private IEnumerator UsingInvincibilityFrames()
    {
        isInvincible = true;
        yield return new WaitForSeconds(durationSecondsInvincible);
        playerStatePresenter.RemoveInvincibilityFramesState(); 
        isInvincible = false;
    }

    private void TakeHP()
    {
        if (hp < 4)
        {
            hp += 1;
        }
        playerStatePresenter.SetNewHP(hp);
    }
}
