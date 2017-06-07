using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skorpion : Weapon {

	// Use this for initialization
	override protected void Start () {

        Nom = "Skorpion VZ";
        RearmTime = 0.01F;
        Damage = 18;
        MaxCapacity = 50;
        ReloadTime = 2.5F;
        Range = 120;
        ActualCapacity = MaxCapacity;
        AnimationArme = animationArme;
        AnimName = "FireAnimSkorpion";
    }
	
	
}
