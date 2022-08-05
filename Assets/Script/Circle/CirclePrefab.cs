using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class CirclePrefab : MonoBehaviour 
{
    [SerializeField]
    List<Circle> Circles;

    private bool isFull;
    public bool IsFull { get => isFull; private set => isFull = value; }

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
        if(circles.Count >= 3)
        {
            isFull = true;
        }

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
        circles.RemoveAll(data => data.sizeType == circleSize);
        if(circles.Count < 3)
        {
            isFull = false;
        }
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

    public List<CircleSizeType> GetCircleActive()
    {
        List<CircleSizeType> circleSizeTypes = new List<CircleSizeType>();
        foreach(RingParameter ring in circles)
        {
            circleSizeTypes.Add(ring.sizeType);
        }
        return circleSizeTypes;
    }
}
