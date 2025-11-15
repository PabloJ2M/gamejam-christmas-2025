using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [Header("Parent")]
    [SerializeField] private Image _parentMask;
    [SerializeField, Range(0, 1)] private float _threshold = 1f;

    [Header("Progress")]
    [SerializeField] private Image _fillAmount;
    [SerializeField, Range(0, 1)] private float _value = 1f;

    public float FillAmount
    {
        set { _fillAmount?.FillAmount(value * _threshold); _value = value; }
        get => _value;
    }
    private void OnValidate()
    {
        _parentMask?.FillAmount(_threshold);
        FillAmount = _value;
    }
}