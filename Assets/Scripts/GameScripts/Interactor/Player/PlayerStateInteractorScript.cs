using Newtonsoft.Json.Converters;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateInteractorScript : MonoBehaviour
{
    [SerializeField] private int hp = 4;
    [SerializeField] private GameObject faceCurrent;
    private FaceScript faceScript;
    private FaceStateScript faceState;

    /*[Space]
    public MeshRenderer rendPartTop;
    public MeshRenderer rendPartMiddle;
    public MeshRenderer rendPartLeft;
    public MeshRenderer rendPartRight;*/

    [SerializeField] private bool isLosing = false;
    [SerializeField] private bool inTakingDamage = false;
    [SerializeField] bool inBlinking = false;


    private void Awake()
    {/*
        rendPartTop = glowingPartTop.GetComponent<MeshRenderer>();
        rendPartMiddle = glowingPartMiddle.GetComponent<MeshRenderer>();
        rendPartLeft = glowingPartLeft.GetComponent<MeshRenderer>();
        rendPartRight = glowingPartRight.GetComponent<MeshRenderer>();*/
    }

    private void Start()
    {
        if (faceScript != null)
        {
            faceScript = faceCurrent.GetComponent<FaceScript>();
            faceState = faceCurrent.GetComponent<FaceStateScript>();
        }
        
    }

    private void Update()
    {
        if (faceState != null && faceState.Get(FaceProperty.IsKilling) && !inTakingDamage)
        {
            inTakingDamage = true;
            TakeDamage();
        }
        if (faceState != null && !faceState.Get(FaceProperty.IsKilling) && inTakingDamage)
        {
            inTakingDamage = false;
        }
        if (faceState != null && faceState.Get(FaceProperty.IsBlinking) && !inBlinking)
        {
            inBlinking = true;
            /*if (animator != null && animClipBlink != null)
            {
                animator.enabled = true;
                animator.Play(animClipBlink.name);
            }*/
        }
        else if (faceState != null && !faceState.Get(FaceProperty.IsBlinking) && inBlinking)
        {
            inBlinking = false;
            //animator.enabled = false;
            //ResetMaterials();
        }

        //*/
        // Commented out - field is commented in TutorialFaceScript
        /*
        else if (faceCurrentFST.isKilling && !inTakingDamage)
        {
            inTakingDamage = true;
            TakeDamage();
            StartCoroutine(PlayAnimationTakeDamage());
        }
        */
    }
    /*
    public void ResetMaterials()
    {
        //Material[] materials = new Material[] { materialTurnOff, materialTurnOn };
        Material[] parts = new Material[] { rendPartTop.material, rendPartMiddle.material, rendPartLeft.material, rendPartRight.material };

        for (int i = 0; i < 4; i++)
        {
            parts[i] = materials[(hp >= i + 1) ? 1 : 0];
        }

        rendPartTop.material = parts[0];
        rendPartMiddle.material = parts[1];
        rendPartLeft.material = parts[2];
        rendPartRight.material = parts[3];
    }*/


    public void SetCurrentFace(GameObject face)
    {
        faceCurrent = face;
        faceScript = face.GetComponent<FaceScript>();
        faceState = face.GetComponent<FaceStateScript>();
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
    /*
    public void SetPartsMaterial(Material material)
    {
        rendPartTop.material = material;
        rendPartMiddle.material = material;
        rendPartLeft.material = material;
        rendPartRight.material = material;
    }*/
}
