using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum ColorType
{
    None = -1,
    White = 0,
    Red,
    Orange,
    Yellow,
    Green,
    Blue,
    Purple,
    Pink
}

[System.Serializable]
public enum CircleSizeType
{
    Big,
    Normal,
    Small
}

[System.Serializable]
public class Circle
{
    public CircleSizeType circleSize;
    public ColorType colorType;
    public Image circleImage;
    public bool isAcive = false;

    public void SetColor(ColorType colorType)
    {
        switch(colorType)
        {
            case ColorType.White:
                {
                    circleImage.color = Color.white;
                }
                break;
            case ColorType.Red:
                {
                    circleImage.color = Color.red;
                }
                break;
            case ColorType.Orange:
                {
                    circleImage.color = new Color();
                }
                break;
            case ColorType.Yellow:
                {
                    circleImage.color = Color.white;
                }
                break;
            case ColorType.Green:
                {
                    circleImage.color = Color.white;
                }
                break;
            case ColorType.Purple:
                {
                    circleImage.color = Color.white;
                }
                break;
        }
        this.colorType = colorType;
    }    
}

[System.Serializable]
public class ColorSprite
{
    public ColorType type;
    public Color color;
}