using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MenuCreditsScript : MonoBehaviour
{
    [SerializeField] private GameObject[] parentObjects; // Массив родительских объектов
    private GameObject[][] sortedChildren; // Массив массивов отсортированных дочерних объектов

    private void Start()
    {
        sortedChildren = new GameObject[parentObjects.Length][];

        for (int i = 0; i < parentObjects.Length; i++)
        {
            if (parentObjects[i] != null)
            {
                sortedChildren[i] = parentObjects[i].transform
                    .Cast<Transform>()
                    .OrderBy(t => t.position.x)
                    .Select(t => t.gameObject)
                    .ToArray();
            }
        }

        // Пример вывода в консоль
        for (int i = 0; i < sortedChildren.Length; i++)
        {
            Debug.Log($"Parent {i}: {parentObjects[i].name}");
            foreach (var child in sortedChildren[i])
            {
                TextMeshPro textMesh = child.GetComponent<TextMeshPro>();

                if (textMesh != null)
                {
                    textMesh.color = Color.gray;
                }
            }
        }
        StartCoroutine(SettingMaterial(0.8f));
    }

    private IEnumerator SettingMaterial(float time)
    {

        for (int i = 0; i < sortedChildren.Length; i++)
        {
            Debug.Log($"Parent {i}: {parentObjects[i].name}");
            foreach (var child in sortedChildren[i])
            {
                TextMeshPro textMesh = child.GetComponent<TextMeshPro>();
                yield return new WaitForSeconds(Random.Range(0.3f * time, 0.8f * time));
                if (textMesh != null)
                {
                    textMesh.color = Color.white;
                }
            }
        }
    }
}
