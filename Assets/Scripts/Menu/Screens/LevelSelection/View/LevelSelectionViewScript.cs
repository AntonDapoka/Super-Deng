using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using Menu.Effects.Flickering;

namespace Menu.Screens.LevelSelection
{
    public class LevelSelectionViewScript : MonoBehaviour
    {
        [SerializeField] private Color colorButtonTurnOn = Color.white;
        [SerializeField] private Color colorButtonTurnOff = Color.clear;
        [SerializeField] private float durationButtonChange = 0.25f;

        public float moveDuration = 1f;
        public AnimationCurve movementCurve;

        public void ShowLevelIcons(Transform[] objects)
        {
            foreach (Transform obj in objects)
            {
                obj.gameObject.SetActive(true);
            }
        }

        public void HideLevelIcons(Transform[] objects)
        {
            foreach (Transform obj in objects)
            {
                obj.gameObject.SetActive(false);
            }
        }

        public void ChangeButtonStateInstant(Button button, bool isTurningOn)
        {
            button.image.color = isTurningOn ? colorButtonTurnOn : colorButtonTurnOff;
        }

        public void ChangeButtonState(
            Button button, 
            FlickeringSettings settings, 
            FlickeringPresenterScript flickeringPresenter, 
            bool isTurningOn)
        {
            Color colorInitial = isTurningOn ? colorButtonTurnOff : colorButtonTurnOn;
            Color colorTarget = isTurningOn ? colorButtonTurnOn : colorButtonTurnOff;
            FlickeringRequestScript request = new(button, 
            settings, 
            colorInitial, 
            colorTarget, 
            durationButtonChange, 
            isTurningOn, 
            isBlinking: true);
            flickeringPresenter.Flicker(in request);
        }

        public void MoveLevelIcons(Transform[] iconsTransform, Transform[] points, int indexNew, int indexCurrent)
        {
            StartCoroutine(MovingLevelIcons(iconsTransform, points, indexNew, indexCurrent));
        }

        private IEnumerator MovingLevelIcons(Transform[] iconsTransform, Transform[] points, int indexNew, int indexCurrent)
        {
            float elapsedTime = 0f;
            int multiplier = indexNew > indexCurrent ? 1 : -1;

            while (elapsedTime < moveDuration / Mathf.Abs(indexNew - indexCurrent))
            {
                float curveProgress = movementCurve.Evaluate(elapsedTime / (moveDuration / Mathf.Abs(indexNew - indexCurrent)));

                iconsTransform[indexCurrent].transform.position = Vector3.Lerp(points[2].position, points[2 + multiplier].position, curveProgress);

                if (multiplier >= 0) {
                    if (indexCurrent > 0)
                        iconsTransform[indexCurrent - 1].transform.position = Vector3.Lerp(points[1].position, points[2].position, curveProgress);
                    if (indexCurrent > 1)
                        iconsTransform[indexCurrent - 2].transform.position = Vector3.Lerp(points[0].position, points[1].position, curveProgress);
                    if (indexCurrent < iconsTransform.Length - 1)
                        iconsTransform[indexCurrent + 1].transform.position = Vector3.Lerp(points[3].position, points[4].position, curveProgress);
                }
                else
                {
                    if (indexCurrent < iconsTransform.Length- 1)
                        iconsTransform[indexCurrent + 1].transform.position = Vector3.Lerp(points[3].position, points[2].position, curveProgress);
                    if (indexCurrent < iconsTransform.Length - 2)
                        iconsTransform[indexCurrent + 2].transform.position = Vector3.Lerp(points[4].position, points[3].position, curveProgress);
                    if (indexCurrent > 0)
                        iconsTransform[indexCurrent - 1].transform.position = Vector3.Lerp(points[1].position, points[0].position, curveProgress);
                    //if (currentIndex > 1 && multiplier < 0)
                    //  objects[currentIndex - 2].transform.position = Vector3.Lerp(points[currentIndex - 2].position, points[currentIndex - 3].position, curveProgress);
                }

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            indexCurrent -= multiplier;
            //}

            //Debug.Log(currentIndex == objects.Length - 1);


            //wall.SetActive(false);

        }

    }
}