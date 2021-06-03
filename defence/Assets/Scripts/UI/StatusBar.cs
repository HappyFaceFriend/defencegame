using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    public float MaxValue { get { return slider.maxValue; } set { slider.maxValue = value; } }
    public float Value { get { return slider.value; } set { slider.value = value; } }


    [SerializeField] Slider slider;
    Transform owner;
    RectTransform rectTransform;
    Vector3 offset;

    public void Init(float maxValue, Transform owner, Vector2 offset)
    {
        this.owner = owner;
        MaxValue = maxValue;
        this.offset = offset;
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
