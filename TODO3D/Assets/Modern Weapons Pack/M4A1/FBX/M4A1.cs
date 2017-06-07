using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class M4A1 : Weapon {

	// Use this for initialization
	override protected void Start () {

        Nom = "M4A1";
        RearmTime = 0.05F;
        Damage = 50;
        MaxCapacity = 30;
        ReloadTime = 2.5F;
        Range = 200;
        ActualCapacity = MaxCapacity;
        AnimationArme = animationArme;
        AnimName = "FireAnimM4A1";
    }
	
	
	
}
