using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasHelper : MonoBehaviour {
    public GameObject PopupPrefab;

	// Use this for initialization
	void Start ()
	{
	    DisplayPopup("LMB/RMB/MMB to interact with the grid\nSpace + WASD to change grid parameters");
	}

    public void DisplayPopup(string text)
    {
        var go = Instantiate(PopupPrefab) as GameObject;
        go.GetComponentInChildren<Text>().text = text;
        go.transform.SetParent(transform, false);
    }

    // Update is called once per frame
	void Update () {
	
	}
}
