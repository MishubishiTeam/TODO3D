using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopUI : MonoBehaviour {

    public GameObject Element;

	// Use this for initialization
	void Start () {
        Element.transform.Translate(new Vector3(0,Screen.height/10+30, 0));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
