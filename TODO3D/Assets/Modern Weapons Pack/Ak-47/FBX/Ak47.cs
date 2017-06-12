using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ak47 : Weapon {

	// Use this for initialization
	override protected void Start () {
        Nom = "AK-47";
        RearmTime = 0.1F;
        Damage = 50;
        MaxCapacity = 30;
        ReloadTime = 2.5F;
        Range = 100000;
        ActualCapacity = MaxCapacity;
        AnimationArme = animationArme;
        AnimName = "FireAnimTest";

        base.Start();
	}
	
	// Update is called once per frame
	
		
       

    
}
