using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RingParameter
{
    public Sprite ringSprite;
    public ColorType colorType;
    public CircleSizeType sizeType;

    public RingParameter()
    { }

    public RingParameter(Sprite ringSprite, ColorType colorType, CircleSizeType sizeType)
    {
        this.ringSprite = ringSprite;
        this.colorType = colorType;
        this.sizeType = sizeType;
    }
}

[CreateAssetMenu(menuName = "IconDatabase/RingDatabase")]
[System.Serializable]
public class RingDatabase : ScriptableObject
{
    public List<RingParameter> listRingSprite;

    public RingParameter GetRingByColorAndSize(ColorType colorType, CircleSizeType sizeType)
    {
        foreach (RingParameter ring in listRingSprite)
        {
            if (ring.colorType == colorType && ring.sizeType == sizeType)
            {
                return ring;
            }
        }

        return listRingSprite[0];
    }

    public List<ColorType> GetListColor()
    {
        List<ColorType> colorTypes = new List<ColorType>();
        foreach(RingParameter ringParameter in listRingSprite)
        {
            if(!colorTypes.Contains(ringParameter.colorType))
            {
                colorTypes.Add(ringParameter.colorType);
            }
        }

        return colorTypes;
    }

    public List<CircleSizeType> GetListSize()
    {
        List<CircleSizeType> sizeTypes = new List<CircleSizeType>();
        foreach (RingParameter ringParameter in listRingSprite)
        {
            if (!sizeTypes.Contains(ringParameter.sizeType))
            {
                sizeTypes.Add(ringParameter.sizeType);
            }
        }

        return sizeTypes;
    }
}