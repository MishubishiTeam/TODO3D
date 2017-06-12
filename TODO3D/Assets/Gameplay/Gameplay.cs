using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Gameplay : MonoBehaviour {

    public GameObject bonusLife;
    public GameObject bonusSpeed;

    private DateTime timerBonusRespawn;
    
	// Use this for initialization
	void Start () {

        spawnBonus();
       
    }
	
	// Update is called once per frame
	void Update () {

        if (bonusMustRespawn())
            spawnBonus();
	}

    void spawnBonus()
    {
        Instantiate(bonusLife);
        Instantiate(bonusSpeed);
        timerBonusRespawn = DateTime.Now;
    }

    bool bonusMustRespawn()
    {
        return DateTime.Now >= timerBonusRespawn.AddSeconds(20);
    }
}
