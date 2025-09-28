using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableBackgroundInstantiatorScript : MonoBehaviour
{
    [SerializeField] AssetReferenceGameObject background;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            background.LoadAssetAsync().Completed += OnAddressableLoaded;
        }
    }

    private void OnAddressableLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Instantiate(handle.Result);
        }
        else
        {
            Debug.LogError("Background Error");
        }
    }
}
