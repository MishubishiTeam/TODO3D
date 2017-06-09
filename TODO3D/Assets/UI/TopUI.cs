using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopUI : MonoBehaviour {

    public GameObject Element;

	// Use this for initialization
	void Start () {
        Element.GetComponent<RectTransform>().position = (new Vector3(Screen.width / 2 - Element.GetComponent<RectTransform>().rect.width / 2 ,Screen.height/10+30, 0));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
