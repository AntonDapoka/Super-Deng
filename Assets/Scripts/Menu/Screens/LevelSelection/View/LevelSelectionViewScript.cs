using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using Menu.Effects.Flickering;
using Menu.Effects.Flickering.Logo;
using System.Linq;

namespace Menu.Screens.LevelSelection
{
    public class LevelSelectionViewScript : MonoBehaviour
    {
        [SerializeField] private LevelIconMaterialManagerScript iconMaterialManager;
        [SerializeField] private Color colorButtonTurnOn = Color.white;
        [SerializeField] private Color colorButtonTurnOff = Color.clear;
        [SerializeField] private Material materialGlowingTurnOn;
        [SerializeField] private Material materialGlowingTurnOff;
        [SerializeField] private float durationButtonChange = 0.25f;
        [SerializeField] private float durationIconChangeTurnOn = 0.25f;
        [SerializeField] private float durationIconChangeTurnOff = 0.25f;

        public float moveDuration = 1f;
        public AnimationCurve movementCurve;

        public void ShowLevelIcons(Transform[] objects, int numberCurrent)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].gameObject.SetActive(true);
                if (i != numberCurrent)
                {
                    iconMaterialManager.SwapMaterial(objects[i].gameObject, materialGlowingTurnOff, 0f, 0.25f);
                }
            }
        }

        public void HideLevelIcons(Transform[] objects)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].gameObject.SetActive(false);
            }
        }

        public void HideSideLevelIcons(Transform[] objects, int numberCurrent)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                if (i != numberCurrent) objects[i].gameObject.SetActive(false);
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

        public Task MoveLevelIcons(Transform[] iconsTransform, Transform[] points, int numberNew, int numberCurrent)
        {
            iconMaterialManager.SwapMaterial(iconsTransform[numberNew].gameObject, materialGlowingTurnOn, durationIconChangeTurnOn, 0.25f);
            iconMaterialManager.SwapMaterial(iconsTransform[numberCurrent].gameObject, materialGlowingTurnOff, durationIconChangeTurnOff, 0.25f);
            Spark[] sparks = iconsTransform[numberNew].GetComponent<LevelIconScript>().GetSparks();
            int count = Random.Range(Random.Range(0,1), sparks.Length);

            foreach (Spark spark in sparks.OrderBy(x => Random.value).Take(count))
            {
                spark.GetComponent<ParticleSystem>().Play();
            }
            return this.RunAsync(MovingLevelIcons(iconsTransform, points, numberNew, numberCurrent));
        }

        private IEnumerator MovingLevelIcons(Transform[] iconsTransform, Transform[] points, int numberNew, int numberCurrent)
        {
            int stepsTotal = Mathf.Abs(numberNew - numberCurrent);
            if (stepsTotal == 0) yield break;

            int direction = (int)Mathf.Sign(numberNew - numberCurrent);
            float stepDuration = moveDuration / stepsTotal;
            int centerSlot = points.Length / 2;

            for (int step = 0; step < stepsTotal; step++)
            {
                int numberFrom = numberCurrent + direction * step;
                int numberTo = numberFrom + direction;

                float elapsedTime = 0f;
                while (elapsedTime < stepDuration)
                {
                    float curveProgress = movementCurve.Evaluate(elapsedTime / stepDuration);

                    for (int i = 0; i < iconsTransform.Length; i++)
                    {
                        Vector3 positionFrom = GetPointPositionByOffset(points, centerSlot, i - numberFrom);
                        Vector3 positionTo = GetPointPositionByOffset(points, centerSlot, i - numberTo);
                        iconsTransform[i].position = Vector3.Lerp(positionFrom, positionTo, curveProgress);
                    }

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                for (int i = 0; i < iconsTransform.Length; i++)
                    iconsTransform[i].position = GetPointPositionByOffset(points, centerSlot, i - numberTo);
            }
        }

        private static Vector3 GetPointPositionByOffset(Transform[] points, int centerSlot, int offset)
        {
            int slot = Mathf.Clamp(centerSlot + offset, 0, points.Length - 1);
            return points[slot].position;
        }

    }
}