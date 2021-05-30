using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExceptionManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Application.logMessageReceived += HandleException;
        DontDestroyOnLoad(gameObject);
    }

    void HandleException(string logString, string stackTrace, LogType type)
    {
        Debug.Log(logString + "///" + stackTrace + "///" + type.ToString());
    }
}
