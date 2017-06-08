using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour
{

    private Animator anim;
    private Rigidbody player;
    private float currentRotation = 0F;
    private float currentVerticalRotation = 0F;
    private Transform myTransform;
    private Transform cameraHolder;
    private Vector3 jump;
    private CharacterController controller;

    private int jumpHash = Animator.StringToHash("Jump");
    private int isJumpingHash = Animator.StringToHash("isJumping");

    public float speed = 5.0F;
    public float rotationSpeed = 5.0F;
    public float jumpSpeed = 5.0F;


    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();

        myTransform = transform;
        cameraHolder = transform.Find("Camera Holder");
        jump = new Vector3(0.0F, 1F, 0F);

        Spawn();

        if (isLocalPlayer)
            cameraHolder.GetChild(0).GetComponent<Camera>().enabled = true;
        else
            cameraHolder.GetChild(0).GetComponent<Camera>().enabled = false;
    }

    private bool isGrounded()
    {
        var collider = GetComponent<Collider>();
        return !Physics.CapsuleCast(collider.bounds.center, collider.bounds.center - new Vector3(0.1F, 0.1F, 0.1F), 0.3F, -Vector3.up, 5F);
    }


    void Spawn()
    {
        if (isLocalPlayer)
        {
            System.Random rnd = new System.Random();
            var spawns = GameObject.FindGameObjectsWithTag("Spawn").ToList();
            var spawn = spawns[rnd.Next(spawns.Count)];
            transform.position = spawn.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;
        float move = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger(jumpHash);
        }

        anim.SetFloat("Speed", move);
        anim.SetBool("isBackwards", Input.GetKey(KeyCode.S));

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            player.AddForce(jump * jumpSpeed, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

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
        player.rotation = Quaternion.Euler(0.0F, currentRotation, 0.0F);

        


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
