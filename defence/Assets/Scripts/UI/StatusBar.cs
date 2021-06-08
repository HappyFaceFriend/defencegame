using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatusBar : MonoBehaviour
{
    public float Value { get { return slider.value; } set { slider.value = value; } }
    public float MaxValue { get { return slider.maxValue; } set { slider.maxValue = value; } }

    [SerializeField] Slider slider;

    Transform owner;
    RectTransform rectTransform;
    Vector3 offset;

    public void Init(float maxValue, float currentValue, Transform owner, Vector2 offset)
    {
        this.owner = owner;
        this.offset = offset;
        MaxValue = maxValue;
        Value = currentValue;
    }
    public void Init(float maxValue, Transform owner, Vector2 offset)
    {
        Init(maxValue, maxValue, owner, offset);
    }
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void LateUpdate()
    {
        rectTransform.position = owner.position + offset;
    }


}
