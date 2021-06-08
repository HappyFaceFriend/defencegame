using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Image image;
    [SerializeField] Text text;
    [SerializeField] LevelManager levelManager;

    public void OnAnimationEnd()
    {
        levelManager.StartLevel();
        gameObject.SetActive(false);
    }
}
