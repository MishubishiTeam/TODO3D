using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour
{

    public enum WeaponTypes
    {
        AK47,
        Uzi,
        M4A1
    }

    public WeaponTypes WeaponType;
    public GameObject AkPrefab;
    public GameObject UziPrefab;
    public GameObject M4A1Prefab;
    public Camera cam;

    private GameObject weapon;
    private Transform weaponTransform;

    // Use this for initialization
    void Start()
    {
        switch (WeaponType)
        {
            case WeaponTypes.AK47:
                weapon = (GameObject)Instantiate(AkPrefab);
                weaponTransform = weapon.transform;
                weaponTransform.SetParent(transform.parent, false);
                weapon.GetComponent<Weapon>().cam = cam;
                break;
            case WeaponTypes.Uzi:
                break;
            case WeaponTypes.M4A1:
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
