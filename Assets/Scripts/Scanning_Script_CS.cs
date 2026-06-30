using ChobiAssets.KTP;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class Scanning_Script_CS : MonoBehaviour
{
    public Text thisText;

    const string textFormat = "Scan: ";
    string scanText;

    private PosMarker_Control_CS markerCheck;

    void Start()
    {
        if (thisText == null)
        {
            thisText = GetComponent<Text>();
        }

        markerCheck = GameObject.Find("Game_Controller").GetComponent<PosMarker_Control_CS>();
    }


    void Update()
    {
        if (markerCheck.markerEnabled)
            scanText = "Scanning";
        else if (markerCheck.markerReady)
            scanText = "Ready";
        else
            scanText = "CoolDown";

        //Debug.Log("Scan Text: " + thisText.text);

        thisText.text = string.Format(textFormat + scanText);
    }
}
