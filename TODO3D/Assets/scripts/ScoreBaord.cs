using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreBaord : MonoBehaviour {
    private bool pressed = true;
    public GameObject scoreBoard;
    private List<PlayerScript2> listeJoueurs
    {
        get
        {
             return GameObject.FindGameObjectsWithTag("Player").ToList().Select(x => x.GetComponent<PlayerScript2>()).ToList();
        }
    }
    
	// Use this for initialization
	void Start () {
        //GameObject scoreBoard = GameObject.FindGameObjectWithTag("ScoreBoard");
        scoreBoard.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        
        

        
            
		
	}

    private void OnGUI()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            float x = (Screen.width / 2) - (scoreBoard.GetComponent<RectTransform>().rect.width / 2);
            float y = (Screen.height / 2) - (scoreBoard.GetComponent<RectTransform>().rect.height / 2) + 30;
            scoreBoard.SetActive(true);

            foreach (var p in listeJoueurs)
            {
                GUI.Label(new Rect(x, y, 100, 100), p.playerName);
                y += 15;
            }
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            scoreBoard.SetActive(false);
        }
    }


}
