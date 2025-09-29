using System.Collections;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public bool isTutorial = false;
    [SerializeField] private int hp = 4;
    [SerializeField] private GameObject faceCurrent;
    private FaceScript faceCurrentFS;
    private TutorialFaceScript faceCurrentFST;
    [Space]
    [SerializeField] private GameObject glowingPartTop;
    [SerializeField] private GameObject glowingPartMiddle;
    [SerializeField] private GameObject glowingPartLeft;
    [SerializeField] private GameObject glowingPartRight;
    [Space]
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip animClipBlink;
    [SerializeField] private Material materialTurnOn;
    [SerializeField] private Material materialTurnOff;
    [Space]
    public MeshRenderer rendPartTop;
    public MeshRenderer rendPartMiddle;
    public MeshRenderer rendPartLeft;
    public MeshRenderer rendPartRight;

    [SerializeField] private LoseScript LS;

    private bool inBlinking = false;
    private bool isLosing = false;
    private bool inTakingDamage = false;


    private void Awake()
    {
        rendPartTop = glowingPartTop.GetComponent<MeshRenderer>();
        rendPartMiddle = glowingPartMiddle.GetComponent<MeshRenderer>();
        rendPartLeft = glowingPartLeft.GetComponent<MeshRenderer>();
        rendPartRight = glowingPartRight.GetComponent<MeshRenderer>();
        animator = gameObject.GetComponent<Animator>();
        animator.enabled = false;
    }

    private void Start()
    {
        if (!isTutorial)
        {
            faceCurrentFS = faceCurrent.GetComponent<FaceScript>();
        }
        else faceCurrentFST = faceCurrent.GetComponent<TutorialFaceScript>();
    }

    private void Update()
    {
        
        if (!isTutorial)
        {
            if (faceCurrentFS != null && faceCurrentFS.isKilling && !inTakingDamage)
            {
                inTakingDamage = true;
                TakeDamage();
                StartCoroutine(PlayAnimationTakeDamage());
            }
            if (faceCurrentFS != null && faceCurrentFS.isBlinking && !inBlinking)
            {
                inBlinking = true;
                if (animator != null && animClipBlink != null)
                {
                    animator.enabled = true;
                    animator.Play(animClipBlink.name);
                }
            }
            else if (faceCurrentFS != null && !faceCurrentFS.isBlinking && inBlinking)
            {
                inBlinking = false;
                animator.enabled = false;
                ResetMaterials();
            }
        }
        else if (faceCurrentFST.isKilling && !inTakingDamage)
        {
            inTakingDamage = true;
            TakeDamage();
            StartCoroutine(PlayAnimationTakeDamage());
        }
    }

    public void ResetMaterials()
    {
        Material[] materials = new Material[] { materialTurnOff, materialTurnOn };
        Material[] parts = new Material[] { rendPartTop.material, rendPartMiddle.material, rendPartLeft.material, rendPartRight.material };

        for (int i = 0; i < 4; i++)
        {
            parts[i] = materials[(hp >= i + 1) ? 1 : 0];
        }

        rendPartTop.material = parts[0];
        rendPartMiddle.material = parts[1];
        rendPartLeft.material = parts[2];
        rendPartRight.material = parts[3];
    }

    public void SetCurrentFace(GameObject face)
    {
        faceCurrent = face;
        faceCurrentFS = face.GetComponent<FaceScript>();
    }
    
    public GameObject GetCurrentFace()
    {
        return faceCurrent;
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
            StartLosing();
        }
        
        if (hp <= 3)
        {
            rendPartTop.material = materialTurnOff;
        }
        if (hp <= 2)
        {
            rendPartRight.material = materialTurnOff;
        }
        if (hp <= 1)
        {
            rendPartLeft.material = materialTurnOff;
        }

    }

    public void TakeHP()
    {
        if (hp < 4)
        {
            hp += 1;
        }
        if (hp == 2)
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
        }
    }

    private IEnumerator PlayAnimationTakeDamage()
    {
        if (animator != null && animClipBlink != null)
        {
            animator.enabled = true;
            animator.Play(animClipBlink.name);
            yield return new WaitForSeconds(animClipBlink.length); 
        }
        ResetMaterials();
        animator.enabled = false;
        inTakingDamage = false;
    }

    public void StartLosing()
    {
        isLosing = true;
        rendPartMiddle.material = materialTurnOff;
        if (LS != null)
        {
            LS.Lose();
        }
        
    }

    public void SetPartsMaterial(Material material)
    {
        rendPartTop.material = material;
        rendPartMiddle.material = material;
        rendPartLeft.material = material;
        rendPartRight.material = material;
    }
}
