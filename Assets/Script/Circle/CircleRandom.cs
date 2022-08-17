using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CircleRandom : CirclePrefab, IDragHandler, IEndDragHandler
{
    public static CircleRandom instance;

    private float posX;
    private float posY;
    private bool onDrag;

    private bool onTrigger;

    private CircleCollider2D circleCollider2D;
    private RectTransform rectTransform;
    private Transform parent;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            circleCollider2D = GetComponent<CircleCollider2D>();
            rectTransform = GetComponent<RectTransform>();
            posX = rectTransform.anchoredPosition.x;
            posY = rectTransform.anchoredPosition.y;
            onTrigger = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition = new Vector2(posX, posY);
        if (onTrigger && parent.GetComponent<CirclePrefab>().SetCircle(circles))
        {
            List<ColorType> colors = new List<ColorType>();
            foreach (RingParameter ring in circles)
            {
                colors.Add(ring.colorType);
            }
            GameController.Instance.CheckPoint(parent.GetComponent<CirclePrefab>(), colors);
            ClearAllCircle();
            GameController.Instance.RandomCircle();
            onTrigger = false;
            return;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        onDrag = true;
        rectTransform.anchoredPosition += eventData.delta;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        parent = collision.transform;
        onTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onTrigger = false;
    }

}
