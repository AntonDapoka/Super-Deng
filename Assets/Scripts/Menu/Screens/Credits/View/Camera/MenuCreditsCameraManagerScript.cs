using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Menu.Screens.Credits
{
    public class MenuCreditsCameraManagerScript : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Camera cam;

        private MenuCreditsCameraState state = MenuCreditsCameraState.Idle;
        private MenuCreditsSettings settings;
        private Transform transformCamera;
        private Vector3 positionInitial;
        private float elapsedSpeedUp;
        private float speedCurrent;

        private void Awake()
        {
            if (cam == null)
            {
                Debug.LogError("camera is not assigned");
                enabled = false;
                return;
            }

            transformCamera = cam.transform;
            positionInitial = transformCamera.position;
        }

        public void Initialize(MenuCreditsSettings settings)
        {
            this.settings = settings;
        }

        public void BeginRun()
        {
            elapsedSpeedUp = 0f;
            speedCurrent = 0f;
            state = MenuCreditsCameraState.Running;
        }

        public void BeginDeceleration()
        {
            if (state == MenuCreditsCameraState.Running) state = MenuCreditsCameraState.Decelerating;
        }

        public void StopMoving()
        {
            state = MenuCreditsCameraState.Idle;
            speedCurrent = 0f;
        }

        public Task ReturnToInitialAsync()
        {
            return this.RunAsync(ReturnCameraRoutine(positionInitial, settings.durationCameraReturn));
        }

        private void Update()
        {
            if (state == MenuCreditsCameraState.Running)
            {
                if (elapsedSpeedUp < settings.durationCameraSpeedUp)
                {
                    elapsedSpeedUp += Time.deltaTime;
                    speedCurrent = Mathf.Lerp(0, settings.speedCamera, elapsedSpeedUp / settings.durationCameraSpeedUp);
                }
                else speedCurrent = settings.speedCamera;

                MoveDown(speedCurrent * Time.deltaTime);
            }
            else if (state == MenuCreditsCameraState.Decelerating)
            {
                elapsedSpeedUp -= Time.deltaTime;
                if (elapsedSpeedUp <= 0f)
                {
                    elapsedSpeedUp = 0f;
                    speedCurrent = 0f;
                    state = MenuCreditsCameraState.Idle;
                }
                else
                {
                    speedCurrent = Mathf.Lerp(0, settings.speedCamera, elapsedSpeedUp / settings.durationCameraSpeedUp);
                    MoveDown(speedCurrent * Time.deltaTime);
                }
            }
        }

        private void MoveDown(float distance)
        {
            transformCamera.position += Vector3.down * distance;
        }

        private IEnumerator ReturnCameraRoutine(Vector3 targetPosition, float duration)
        {
            float startDistance = Vector3.Distance(transformCamera.position, targetPosition);

            if (duration <= 0f || startDistance <= settings.thresholdCameraArrival)
            {
                transformCamera.position = targetPosition;
                yield break;
            }

            float baseSpeed = startDistance / duration;

            while (Vector3.Distance(transformCamera.position, targetPosition) > settings.thresholdCameraArrival)
            {
                float distanceLeft = Vector3.Distance(transformCamera.position, targetPosition);
                float speed = baseSpeed * settings.curveSpeedByDistanceToTarget.Evaluate(Mathf.Clamp01(distanceLeft / startDistance));
                transformCamera.position = Vector3.MoveTowards(transformCamera.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }

            transformCamera.position = targetPosition;
        }
    }
}