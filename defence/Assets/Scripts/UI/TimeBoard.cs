using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimeBoard : MonoBehaviour
{
    public string Time { set { timeText.text = value; } }
    public int Wave { set { waveText.text = value.ToString(); } }

    [Header("References")]
    [SerializeField] Text waveText;
    [SerializeField] Text timeText;

    
}
