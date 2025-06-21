using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyIcosahedronScript : MonoBehaviour
{
    public GameObject[] sourceObjects = new GameObject[6];
    public GameObject prefabToInstantiate;

    void Start()
    {
        if (prefabToInstantiate == null)
        {
            Debug.LogError("Не указан префаб.");
            return;
        }
        char c = 'A';
        for (int i = 0; i < sourceObjects.Length; i++)
        {
            GameObject source = sourceObjects[i];
            if (source == null) continue;
            
            // Создание нового родительского объекта
            GameObject newParent = new GameObject("Strip_" + (c));
            c++;
            newParent.transform.position = source.transform.position;
            newParent.transform.rotation = source.transform.rotation;
            newParent.transform.localScale = source.transform.localScale;

            // Проход по всем дочерним объектам
            foreach (Transform child in source.transform)
            {
                GameObject clone = Instantiate(prefabToInstantiate);
                clone.transform.position = child.position;
                clone.transform.rotation = child.rotation;
                clone.transform.localScale = child.localScale;

                // Сделать клон дочерним к новому родителю
                clone.transform.SetParent(newParent.transform, worldPositionStays: true);
                clone.transform.localRotation = child.localRotation * Quaternion.Euler(0, 180, 180);
                clone.name = prefabToInstantiate.name;
            }
        }
    }
}