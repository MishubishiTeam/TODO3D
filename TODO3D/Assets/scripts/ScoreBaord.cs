using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBaord : MonoBehaviour {
    private bool pressed = true;
    public GameObject scoreBoard;
    
	// Use this for initialization
	void Start () {
        //GameObject scoreBoard = GameObject.FindGameObjectWithTag("ScoreBoard");

    }
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {

            scoreBoard.SetActive(true);
        }
        if(Input.GetKeyUp(KeyCode.Tab))
        {
            scoreBoard.SetActive(false);
        }

        
            
		
	}
}
