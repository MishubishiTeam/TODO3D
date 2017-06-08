using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    private Animator anim;
    private Rigidbody player;
    private float currentRotation = 0F;
    private float currentVerticalRotation = 0F;
    private Transform myTransform;
    private Transform cameraHolder;

    public float speed = 5.0F;
    public float rotationSpeed = 5.0F;


    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Rigidbody>();

        myTransform = transform;
        cameraHolder = transform.Find("Camera Holder");
    }

    // Update is called once per frame
    void Update()
    {
        float move = Input.GetAxis("Vertical");
        anim.SetFloat("Speed", move);
        anim.SetBool("isBackwards", Input.GetKey(KeyCode.S));
    }

    private void FixedUpdate()
    {
        var inputX = Input.GetAxis("Horizontal");
        var inputY = Input.GetAxis("Vertical");
        var inputR = Mathf.Clamp(Input.GetAxis("Mouse X"), -1.0F, 1.0F);
        var inputW = Mathf.Clamp(Input.GetAxis("Mouse Y"), -1.0F, 1.0F);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentRotation += 180.0F;
        }

        Vector3 moveVectorX = myTransform.forward * inputX;
        Vector3 moveVectorY = myTransform.forward * inputY;
        Vector3 moveVector = (moveVectorX + moveVectorY).normalized * speed * Time.deltaTime;

        currentRotation = ClampAngle(currentRotation + (inputR * rotationSpeed));
        currentVerticalRotation = Mathf.Clamp(ClampAngle(currentVerticalRotation + (inputW * rotationSpeed)), -30, 30);

        Quaternion rotationAngle = Quaternion.Euler(0.0F, currentRotation, 0.0F);

        cameraHolder.GetChild(0).transform.rotation = Quaternion.Euler(-currentVerticalRotation, currentRotation, 0.0F);

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
