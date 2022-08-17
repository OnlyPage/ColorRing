using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorParamater
{
    public ColorType colorType;
    public Color color;
}

[System.Serializable]
[CreateAssetMenu(menuName = "IconDatabase/ColorDatabase")]
public class ColorDatabase : ScriptableObject
{
    [SerializeField]
    private List<ColorParamater> colors;

    public ColorParamater GetColorByColorType(ColorType type)
    {
        foreach(ColorParamater color in colors)
        {
            if(color.colorType == type)
            {
                return color;
            }
        }
        return colors[0];
    }
}
