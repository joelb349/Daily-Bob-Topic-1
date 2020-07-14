using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ModelDialog : MonoBehaviour
{
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;


    [HideInInspector]
    public bool isVisible = true;

    private void Start()
    {
        text1.enabled = false;
        text2.enabled = false;
    }


    public void SetVisibility(bool pIsVisible)
    {
        gameObject.SetActive(pIsVisible);
        isVisible = pIsVisible;
    }

    public void Set(string pMessage)
    {
        SetVisibility(true);
        text2.enabled = false;
        text1.enabled = true;
        text1.text = pMessage;
    }

    public void DisplayNextMessage(string pMessage)
    {
        text1.enabled = false;
        text2.enabled = true;
        text2.text = "An Example of this is Flinkdrinkin";
    }
}
