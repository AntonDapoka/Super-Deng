using UnityEngine;

public interface ISphereFigureHandler<T> where T : ISphereFigureData
{
    T CollectData(GameObject rootObject);
    void ApplyData(GameObject prefab, T data, Transform parent);
}
