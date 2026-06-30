using ChobiAssets.KTP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ammo_Script_CS : MonoBehaviour
{
    public Text thisText;

    const string textFormat = "Ammo: ";
    public int ammoCount;
    public bool reloading;

    void Start()
    {
        if (thisText == null)
        {
            thisText = GetComponent<Text>();
        }
    }


    void Update()
    {
        //Debug.Log("Scan Text: " + thisText.text);
        if (reloading)
            thisText.text = string.Format(textFormat + "Reloading");
        else
            thisText.text = string.Format(textFormat + ammoCount);
    }
}
