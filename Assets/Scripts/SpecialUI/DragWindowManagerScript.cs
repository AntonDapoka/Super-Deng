using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragWindowManagerScript : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private DragWindowScript[] windows;

    private void Start()
    {
        foreach (var window in windows)
        { 
            window.SetCanvas(canvas);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
