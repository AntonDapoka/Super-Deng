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

        public void ShowLevelIcons(Transform[] objects, int indexCurrent)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].gameObject.SetActive(true);
                if (i != indexCurrent)
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

        public void HideSideLevelIcons(Transform[] objects, int indexCurrent)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                if (i != indexCurrent) objects[i].gameObject.SetActive(false);
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

        public Task MoveLevelIcons(Transform[] iconsTransform, Transform[] points, int indexNew, int indexCurrent)
        {
            iconMaterialManager.SwapMaterial(iconsTransform[indexNew].gameObject, materialGlowingTurnOn, durationIconChangeTurnOn, 0.25f);
            iconMaterialManager.SwapMaterial(iconsTransform[indexCurrent].gameObject, materialGlowingTurnOff, durationIconChangeTurnOff, 0.25f);
            Spark[] sparks = iconsTransform[indexNew].GetComponent<LevelIconScript>().GetSparks();
            int count = Random.Range(Random.Range(0,1), sparks.Length);

            foreach (Spark spark in sparks.OrderBy(x => Random.value).Take(count))
            {
                spark.GetComponent<ParticleSystem>().Play();
            }
            return this.RunAsync(MovingLevelIcons(iconsTransform, points, indexNew, indexCurrent));
        }

        private IEnumerator MovingLevelIcons(Transform[] iconsTransform, Transform[] points, int indexNew, int indexCurrent)
        {
            int stepsTotal = Mathf.Abs(indexNew - indexCurrent);
            if (stepsTotal == 0) yield break;

            int direction = (int)Mathf.Sign(indexNew - indexCurrent);
            float stepDuration = moveDuration / stepsTotal;
            int centerSlot = points.Length / 2;

            for (int step = 0; step < stepsTotal; step++)
            {
                int indexFrom = indexCurrent + direction * step;
                int indexTo = indexFrom + direction;

                float elapsedTime = 0f;
                while (elapsedTime < stepDuration)
                {
                    float curveProgress = movementCurve.Evaluate(elapsedTime / stepDuration);

                    for (int i = 0; i < iconsTransform.Length; i++)
                    {
                        Vector3 positionFrom = GetPointPositionByOffset(points, centerSlot, i - indexFrom);
                        Vector3 positionTo = GetPointPositionByOffset(points, centerSlot, i - indexTo);
                        iconsTransform[i].position = Vector3.Lerp(positionFrom, positionTo, curveProgress);
                    }

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                for (int i = 0; i < iconsTransform.Length; i++)
                    iconsTransform[i].position = GetPointPositionByOffset(points, centerSlot, i - indexTo);
            }
        }

        private static Vector3 GetPointPositionByOffset(Transform[] points, int centerSlot, int offset)
        {
            int slot = Mathf.Clamp(centerSlot + offset, 0, points.Length - 1);
            return points[slot].position;
        }

    }
}