using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    private string nom;
    private float rearmTime;
    private float damage;
    private int maxCapacity;
    private float reloadTime;
    private float range;
    private int actualCapacity;
    private bool currReload;
    private bool currRearm;
    private DateTime initRearming;

    #region constructeurs + propriétes
    public string Nom
    { get; set; }

    public float RearmTime
    { get; set; }

    public float Damage
    { get; set; }

    public int MaxCapacity
    { get; set; }

    public float ReloadTime
    { get; set; }

    public float Range
    { get; set; }

    public int ActualCapacity
    { get; set; }

    public Weapon(string pNom, float pRearmTime, float pDamage, int pMaxCapacity, float pReloadTime, float pRange)
    {
        Nom = pNom;
        RearmTime = pRearmTime;
        Damage = pDamage;
        MaxCapacity = pMaxCapacity;
        ReloadTime = pReloadTime;
        Range = pRange;
        ActualCapacity = MaxCapacity;
    }

    public Weapon () { }

    #endregion



    #region méthodes Unity
    // Use this for initialization
    void Start () { 
        
       
    }
	
	// Update is called once per frame
	void Update () {

       
        if (Input.GetButtonDown("Fire1"))
            tirer();

        if (Input.GetKeyDown(KeyCode.R))
            currReload = true;  // début du rechargement
       
        }

    #endregion



    #region Méthodes tirs - rechargement

    void tirer()
    {
        if (ActualCapacity > 0) // si le chargeur possède des balles
        {
            if (currentlyReloading() == false) // si le rechargement n'est pas en cours
            {
                if (currentlyRearming() == false) // si le réarmement n'est pas en cours (cadence)
                {
                    RaycastHit hit = new RaycastHit();
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit)) //bool qui définit si qqchose a été touché (.position = pos actuelle, .transdirection = direction du tir, hit = objet RaycastHit)
                    {
                        float distance = hit.distance;
                        if (distance <= Range) // si la cible est a portée
                        {
                            ActualCapacity -= 1;
                            hit.transform.SendMessage("ApplyDamage", Damage, SendMessageOptions.DontRequireReceiver); //ApplyDamage = nom de la méthode de l'objet touché infligeant les dégats - a modifier en cas d'appellation différente
                            initRearming = DateTime.Now;
                            currRearm = true;
                        }
                    }
                }
            }
        }
        else
            currReload = true;  //début rechargement
    }

    

    bool currentlyReloading()
    {
        DateTime now = DateTime.Now;
        if (now > initRearming.AddSeconds(ReloadTime) && currReload == true) // si le tps est supérieur au tps de reload et qu'il était en train de reload
        {
            currReload = false;
            return false;
        }
        else
            return true;
    }

    bool currentlyRearming()
    {
        DateTime now = DateTime.Now;
        if (now > initRearming.AddSeconds(RearmTime) && currRearm == true) // si le tps est supérieur au tps de relarm et qu'il était en train de rearm
        {
            currRearm = false;
            return false;
        }
        else
            return true;
    }

    
    #endregion

}
