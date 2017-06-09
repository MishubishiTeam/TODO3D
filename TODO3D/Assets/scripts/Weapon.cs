using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Weapon : NetworkBehaviour {

    public enum ShootingMode
    {
        CoupParCoup,
        Rafale,
        Auto
    }

    public ShootingMode Mode;
    public GameObject Bullet;
    public Transform Origin;
    public Camera cam;
    public GameObject player;

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

    public bool CanShoot
    {
        get
        {
            return ActualCapacity >= 1 && !currRearm && !currReload;
        }
    }

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
        else
        {
            switch (Mode)
            {
                case ShootingMode.CoupParCoup:
                    if (Input.GetKeyDown(KeyCode.T))
                        Tirer();
                    break;
                case ShootingMode.Rafale:
                    break;
                case ShootingMode.Auto:
                    if (Input.GetKey(KeyCode.T))
                        Tirer();
                    break;
                default:
                    break;
            }
        }

    }

    
    public void Tirer()
    {
        if (ActualCapacity > 0)
        {
            RaycastHit hit = new RaycastHit();
            InitRearming = DateTime.Now;

            currRearm = true;
            ActualCapacity -= 1;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
            {
                float distance = hit.distance;
                if (distance <= Range)
                {
                    hit.transform.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
        else
            currReload = true;
    }

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
