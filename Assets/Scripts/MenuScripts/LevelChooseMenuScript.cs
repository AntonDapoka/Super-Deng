using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class LevelChooseMenuScript : MonoBehaviour
{
    [SerializeField] GameObject holder;
    [SerializeField] private GameObject[] objects;
    [SerializeField] private Transform[] points;
    [SerializeField] private int[] scenes;
    public int currentIndex = 2;

    public float moveDuration = 1f;
    public AnimationCurve movementCurve;
    [Header("UI")]
    [SerializeField] private Button buttonRight;
    [SerializeField] private Button buttonLeft;
    [SerializeField] private Button buttonChoose;
    [SerializeField] private GameObject wall;
    [SerializeField] private SecretPasswordInputScript SPIS;

    public void TurnOnAndOff(bool isTurn)
    {
        StartCoroutine(TurningOnAndOff(isTurn));
    }

    private IEnumerator TurningOnAndOff(bool isTurn)
    {
        yield return new WaitForSeconds(!isTurn ? 0.3f : 0f);
        for (int i = 0; i < objects.Length; i++)
        {
            if (i != currentIndex)
                objects[i].SetActive(!isTurn);
        }
        buttonLeft.gameObject.SetActive(!isTurn);
        buttonRight.gameObject.SetActive(!isTurn);
    }
    /*
    [SerializeField] private Button button—hoose;
    [SerializeField] private LevelButtonTransition LBT;

    [SerializeField] private Vector3 positionChosen;
    [SerializeField] private Vector3 positionUnchosen;
    [SerializeField] private RectTransform bannerLevelDesc;
    [SerializeField] private RectTransform[] uiElements;
    [SerializeField] private GameObject wall;
    [SerializeField] private int[] scenes;

    public float biasBanner = 700f;
    public float biasUI = 400f;
    public float moveDuration = 1f; 
    public AnimationCurve movementCurve;
    */
    private void Start()
    {
        //buttonChoose.onClick.AddListener(LoadMenuScene);
        buttonRight.onClick.AddListener(OnRightButtonClick);
        buttonLeft.onClick.AddListener(OnLeftButtonClick);
        //positionUnchosen = Vector3.zero;
        //wall.SetActive(false);
        SPIS.SetCurrentIndex(currentIndex);
        SPIS.SetAmountOfLevels(objects.Length - 1);
    }

    public void OnLeftButtonClick()
    {

        if (currentIndex != 0)
        {
            StartCoroutine(MoveObjectAndUI(currentIndex + 1));
            
        }
        else
        {
            //Play sound
        }
    }

    public void OnRightButtonClick()
    {
        if (currentIndex != objects.Length - 1)
        {
            StartCoroutine(MoveObjectAndUI(currentIndex - 1));
        }
        else
        {
            //PlaySound
        }
    }

    private IEnumerator MoveObjectAndUI(int newIndex)
    {
        wall.SetActive(true);
        float elapsedTime = 0f;
        int multiplier = newIndex > currentIndex ? 1 : -1;

        //while (currentIndex != newIndex)
        //{
            //Debug.Log(currentIndex.ToString() +  "‚‚‚"  + newIndex.ToString());
        while (elapsedTime < moveDuration / Mathf.Abs(newIndex - currentIndex))
        {
            float curveProgress = movementCurve.Evaluate(elapsedTime / (moveDuration / Mathf.Abs(newIndex - currentIndex)));
            //Debug.Log(multiplier);
            objects[currentIndex].transform.position = Vector3.Lerp(points[2].position, points[2 + multiplier].position, curveProgress);

            if (multiplier >= 0) {
                if (currentIndex > 0)
                    objects[currentIndex - 1].transform.position = Vector3.Lerp(points[1].position, points[2].position, curveProgress);
                if (currentIndex > 1)
                    objects[currentIndex - 2].transform.position = Vector3.Lerp(points[0].position, points[1].position, curveProgress);
                if (currentIndex < objects.Length - 1)
                    objects[currentIndex + 1].transform.position = Vector3.Lerp(points[3].position, points[4].position, curveProgress);
            }
            else
            {
                if (currentIndex < objects.Length- 1)
                    objects[currentIndex + 1].transform.position = Vector3.Lerp(points[3].position, points[2].position, curveProgress);
                if (currentIndex < objects.Length - 2)
                    objects[currentIndex + 2].transform.position = Vector3.Lerp(points[4].position, points[3].position, curveProgress);
                if (currentIndex > 0)
                    objects[currentIndex - 1].transform.position = Vector3.Lerp(points[1].position, points[0].position, curveProgress);
                //if (currentIndex > 1 && multiplier < 0)
                //  objects[currentIndex - 2].transform.position = Vector3.Lerp(points[currentIndex - 2].position, points[currentIndex - 3].position, curveProgress);
            }


            

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        currentIndex -= multiplier;
        //}
        SPIS.SetCurrentIndex(currentIndex);
        SPIS.SetAcrivePasswordUI(currentIndex == objects.Length - 1);
        //Debug.Log(currentIndex == objects.Length - 1);


        wall.SetActive(false);

    }

}
