using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    // Use this for initialization
    private void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        var p = hit.GetComponent<PlayerScript2>();
        if (p != null)
        {
        }

        Destroy(gameObject);
    }
}
