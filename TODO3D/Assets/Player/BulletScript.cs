using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    [HideInInspector]
    public float dmg;
    [HideInInspector]
    public PlayerScript2 sender;

    // Use this for initialization
    private void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        var p = hit.GetComponent<PlayerScript2>();
        if (p != null && p != sender)
        {
            p.ApplyDamage(dmg);
            Destroy(gameObject);
        }
    }
}
