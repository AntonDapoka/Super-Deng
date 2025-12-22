using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TriangleButtonForPasswordScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string buttonCode;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite selectedSprite;

    private Image image;
    private PolygonCollider2D polygonCollider;  // Коллайдер для кнопки
    private bool isSelected = false;

    private void Start()
    {
        image = GetComponent<Image>();
        image.sprite = normalSprite;
        polygonCollider = GetComponent<PolygonCollider2D>();  

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (polygonCollider.OverlapPoint(eventData.position))
        {
            isSelected = !isSelected;
            image.sprite = isSelected ? selectedSprite : normalSprite;


            Debug.Log(buttonCode);
        }
    }
}
