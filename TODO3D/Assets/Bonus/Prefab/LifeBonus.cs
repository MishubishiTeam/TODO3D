using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBonus : MonoBehaviour {

    bool isAscending;
    float yRotation;
    float x;
    float y;
    float z;
    public const float PAS = 0.005F;

	// Use this for initialization
	void Start () {
        x = Random.Range(-4.0F, 10.0F);
        y = transform.position.y;
        z = Random.Range(-5.0F, 5.0F);
        gameObject.tag = "BonusLife";
        isAscending = true;
	}
	
	// Update is called once per frame
	void Update () {
        yRotation += Time.deltaTime * 300;

        if (transform.position.y > 0.6)
            isAscending = false;

        if (transform.position.y < 0.3)
            isAscending = true;

        if (isAscending)
            y += PAS;
        else
            y -= PAS;

        transform.rotation = Quaternion.Euler(0, yRotation, 0);
        transform.position = new Vector3(x, y, z);

	}

    
}
