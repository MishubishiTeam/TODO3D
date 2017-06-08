using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public enum ShootingMode
    {
        CoupParCoup,
        Rafale,
        Auto
    }

    public ShootingMode Mode;
    public Rigidbody Bullet;
    public Transform Origin;

    public string nom;
    public float rearmTime;
    public float damage;
    public float BulletSpeed;
    public int maxCapacity;
    public float reloadTime;
    public float range;
    public int currentCapacity;
    protected bool currReload;
    protected bool currRearm;
    private DateTime initRearming;
    public Transform animationArme;
    private string animName;


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

    public DateTime InitRearming
    { get; set; }

    public Transform AnimationArme
    { get; set; }

    public string AnimName
    { get; set; }


    // Use this for initialization
    virtual protected void Start () {
        
    }
	
	// Update is called once per frame
	protected void Update () {

        if (ActualCapacity < 1)
            currReload = true;

        if (currReload == true)
            Reloading();

        if (currRearm == true)
            Rearming();

        else if (Input.GetKeyDown(KeyCode.R))
        {
            currReload = true;  // début du rechargement
            InitRearming = DateTime.Now;
        }

        switch (Mode)
        {
            case ShootingMode.CoupParCoup:
                if (Input.GetKeyDown(KeyCode.T))
                    tirer();
                break;
            case ShootingMode.Rafale:
                break;
            case ShootingMode.Auto:
                if (Input.GetKey(KeyCode.T))
                    tirer();
                break;
            default:
                break;
        }
    }

    protected void tirer()
    {
        Bullet = Instantiate(Bullet, Origin);
        Bullet.transform.position = Origin.transform.position;
        if (ActualCapacity > 0) // si le chargeur possède des balles
        {
            //Bullet = Instantiate(Bullet, Origin);
            //Bullet.transform.position = Origin.transform.position;
            RaycastHit hit = new RaycastHit();
            InitRearming = DateTime.Now;
            //AnimationArme.GetComponent<Animation>().Play(AnimName); // ne fonctionne pas
            currRearm = true;
            ActualCapacity -= 1;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit)) //bool qui définit si qqchose a été touché (.position = pos actuelle, .transdirection = direction du tir, hit = objet RaycastHit)
            {
                 float distance = hit.distance;
                 if (distance <= Range) // si la cible est a portée
                 {
                     hit.transform.SendMessage("ApplyDamage", Damage, SendMessageOptions.DontRequireReceiver); //ApplyDamage = nom de la méthode de l'objet touché infligeant les dégats - a modifier en cas d'appellation différente
                 }
            }
         }
         else
            currReload = true;  //début rechargement
    }

    

   /*  bool currentlyReloading() // inutile mdr
    {
        DateTime now = DateTime.Now;
        if (now > InitRearming.AddSeconds(ReloadTime)) // si le tps est supérieur au tps de reload et qu'il était en train de reload
        {
            currReload = false;
            return false;
        }
        else
            return true;
    } */ 

   /* bool currentlyRearming()
    {
        DateTime now = DateTime.Now;
        if (now > InitRearming.AddSeconds(RearmTime)) // si le tps est supérieur au tps de relarm et qu'il était en train de rearm
        {
            currRearm = false;
            return false;
        }
        else
            return true;
    }*/ 

    protected void Reloading()
    {
        if (DateTime.Now > InitRearming.AddSeconds(ReloadTime)) // si le tps est supérieur au tps de reload et qu'il était en train de reload
        {
            currReload = false;
            ActualCapacity = MaxCapacity;
        }
    }

    protected void Rearming()
    {
        if (DateTime.Now > InitRearming.AddSeconds(RearmTime)) // si le tps est supérieur au tps de relarm et qu'il était en train de rearm
            currRearm = false;
    }

    
    

}
