using UnityEngine;
using UnityEngine.UI;

public static class Extensions
{
    public static void SetSprite(this Image image, Sprite sprite) => image.sprite = sprite;
    public static void FillAmount(this Image image, float value) => image.fillAmount = value;
}