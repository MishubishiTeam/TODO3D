using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour {

    private Animator anim;
    private Rigidbody player;
    private float currentRotation = 0F;
    private Transform myTransform;
    private Transform cameraHolder;

    public float speed = 5.0F;
    public float rotationSpeed = 5.0F;
    public Camera cam;


    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Rigidbody>();

        myTransform = transform;
        cameraHolder = transform.Find("Camera Holder");

        if (isLocalPlayer)
        {
            cam.enabled = true;
            Camera.main.enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        float move = Input.GetAxis("Vertical");
        anim.SetFloat("Speed", move);
	}

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        var inputX = Input.GetAxis("Horizontal");
        var inputY = Input.GetAxis("Vertical");
        var inputR = Mathf.Clamp(Input.GetAxis("Mouse X"), -1.0F, 1.0F);

        Vector3 moveVectorX = myTransform.forward * inputY;
        Vector3 moveVectorY = myTransform.forward * inputX;
        Vector3 moveVector = (moveVectorX + moveVectorY).normalized * speed * Time.deltaTime;

        currentRotation = ClampAngle(currentRotation + (inputR * rotationSpeed));
        Quaternion rotationAngle = Quaternion.Euler(0.0F, currentRotation, 0.0F);

        myTransform.position = myTransform.position + moveVector;
        myTransform.rotation = rotationAngle;

    }

    float ClampAngle(float angle)
    {
        if (angle < -360.0F)
            angle += 360.0F;
        else if (angle > 360.0F)
            angle -= 360.0F;

        return angle;
    }
}
