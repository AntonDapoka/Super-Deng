using UnityEngine;

public class PlayerStateInteractorScript : MonoBehaviour
{
    [SerializeField] PlayerStatePresenterScript playerStatePresenter;
    [SerializeField]  FaceStateScript faceState;

    private int difficulty = 1;
    public int hp = 4;

    [SerializeField] private bool isLosing = false;
    [SerializeField] private bool inTakingDamage = false;
    [SerializeField] bool inBlinking = false;
    [SerializeField] bool isColored = false;
    [SerializeField] bool isInvincible = false;

    public void SetCurrentFace(GameObject face)
    {
        faceState = face.GetComponent<FaceStateScript>();
    }

    private void Update()
    {
        if (faceState == null) return;

        if ((faceState.Get(FaceProperty.IsColored) || faceState.Get(FaceProperty.IsKilling)) && !isColored)
        {
            isColored = true;
            playerStatePresenter.SetColoredState();
        }
        else if (!faceState.Get(FaceProperty.IsColored) && !faceState.Get(FaceProperty.IsKilling) && isColored)
        {
            isColored = false;
            playerStatePresenter.RemoveColoredState();
        }


        if (faceState.Get(FaceProperty.IsBlinking) && !inBlinking)
        {
            inBlinking = true;
            playerStatePresenter.SetBlinkingState();
        }
        else if (!faceState.Get(FaceProperty.IsBlinking) && inBlinking)
        {
            inBlinking = false;
            playerStatePresenter.RemoveBlinkingState();
        }

        
        if (faceState.Get(FaceProperty.IsKilling) && !inTakingDamage)
        {
            inTakingDamage = true;
            TakeDamage();
            playerStatePresenter.SetInvincibilityFramesState();
        } 
        else if (!faceState.Get(FaceProperty.IsKilling) && inTakingDamage)
        {
            inTakingDamage = false;
            playerStatePresenter.RemoveInvincibilityFramesState();
        }

        /*
        else if (faceCurrentFST.isKilling && !inTakingDamage)
        {
            inTakingDamage = true;
            TakeDamage();
            StartCoroutine(PlayAnimationTakeDamage());
        }
        */
    }

    public void TakeDamage()
    {
        if (hp > 1)
        {
            hp -= 1;
        }
        else if (!isLosing)
        {
            hp -= 1;
            //StartLosing();
        }
    }

    public void TakeHP()
    {
        if (hp < 4)
        {
            hp += 1;
        }
        /*if (hp == 2)
        {
            rendPartLeft.material = materialTurnOn;
        }
        if (hp == 3)
        {
            rendPartRight.material = materialTurnOn;
        }
        if (hp == 4)
        {
            rendPartTop.material = materialTurnOn;
        }*/
    }
}
