using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public string nom;
    public float rearmTime;
    public float damage;
    public int maxCapacity;
    public float reloadTime;
    public float range;
    public int actualCapacity;
    bool canFire;
    DateTime initRearming;
    


	// Use this for initialization
	void Start () { 

        canFire = true;
       
    }
	
	// Update is called once per frame
	void Update () {

        if (canFire == true)
        {
            if (Input.GetButtonDown("Fire1"))
                tirer();

            if (Input.GetKeyDown(KeyCode.R))
                recharger();  // début du rechargement
        }
        else
            recharger();


        
       
		
	}

    void tirer()
    {
        if (actualCapacity > 0) // si le chargeur possède des balles
        {
            RaycastHit hit = new RaycastHit(); 
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit)) //bool qui définit si qqchose a été touché (.position = pos actuelle, .transdirection = direction du tir, hit = objet RaycastHit)
            {
                float distance = hit.distance;
                hit.transform.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
                initRearming = DateTime.Now;
            }
        }
        else
            recharger(); //début rechargement
    }

    void recharger()
    {
        canFire = false;
        currentlyRearming();
    }

    bool currentlyRearming()
    {
        DateTime now = DateTime.Now;
        if (now > initRearming.AddSeconds(reloadTime))
        {
            canFire = true;
            return false;
        }
        else
            return true;

    }

  
}
