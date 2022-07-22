using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CirclePrefab : MonoBehaviour 
{
    [SerializeField]
    List<Circle> Circles;

    private void Start()
    {
        ClearAllCircle();
    }

    private int index;

    public void setIndex(int index)
    {
        this.index = index;
        ClearAllCircle();
    }

    public int GetIndex()
    {
        return index;
    }

    protected List<RingParameter> circles = new List<RingParameter>();
    public bool SetCircle(List<RingParameter> circles)
    {
        foreach (RingParameter ringParameter in circles)
        {
            Circle circle = getCircleBySize(ringParameter.sizeType);
            if (circle.isAcive)
                return false;
        }

        foreach (RingParameter ringParameter in circles)
        {
            Circle circle = getCircleBySize(ringParameter.sizeType);
            circle.circleImage.gameObject.SetActive(true);
            circle.circleImage.sprite = ringParameter.ringSprite;
            circle.isAcive = true;
        }

        this.circles.AddRange(circles);

        return true;
    }

    public Circle getCircleBySize(CircleSizeType circleSize)
    {
        return Circles.Find(data => data.circleSize == circleSize);
    }

    public void ClearCircleByCircleSize(CircleSizeType circleSize)
    {
        Circle circle = getCircleBySize(circleSize);
        circle.circleImage.gameObject.SetActive(false);
        circle.isAcive = false;
    }

    public void ClearAllCircle()
    {
        circles.Clear();
        ClearCircleByCircleSize(CircleSizeType.Big);
        ClearCircleByCircleSize(CircleSizeType.Normal);
        ClearCircleByCircleSize(CircleSizeType.Small);
    }

    public bool ContainsColor(ColorType color)
    {
        foreach(RingParameter ring in circles)
        {
            if (ring.colorType == color)
                return true;
        }
        return false;
    }

    public int ClearColor(ColorType color)
    {
        List<CircleSizeType> circleSizes = new List<CircleSizeType>();
        foreach (RingParameter ring in circles)
        {
            if(ring.colorType == color)
            {
                circleSizes.Add(ring.sizeType);
            }
        }

        foreach(CircleSizeType circle in circleSizes)
        {
            ClearCircleByCircleSize(circle);
        }
        return circleSizes.Count;
    }

    public int GetNumberColor()
    {
        return circles.Count;
    }
}
