using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScript2 : NetworkBehaviour {

    /*
    private Animator anim;
    private Rigidbody player;
    private float currentRotation = 0F;
    private float currentVerticalRotation = 0F;
    private Transform transform;
    private Transform cameraHolder;
    private Vector3 jump;
    private Weapon weapon;

    private int jumpHash = Animator.StringToHash("Jump");
    private int isJumpingHash = Animator.StringToHash("isJumping");

    public float speed = 5.0F;
    public float rotationSpeed = 5.0F;
    public float jumpSpeed = 5.0F;
    public Camera cam;
    public GameObject WeaponPrefab;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Rigidbody>();
        weapon = WeaponPrefab.GetComponent<Weapon>();
        weapon.player = gameObject;
        weapon.cam = cam;

        transform = transform;
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

    float ClampAngle(float angle)
    {
        if (angle < -360.0F)
            angle += 360.0F;
        else if (angle > 360.0F)
            angle -= 360.0F;

        return angle;
    }

    // Update is called once per frame
    [ClientCallback]
    void Update ()
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

    [ClientCallback]
    private void FixedUpdate()
    {
        if (!hasAuthority)
            return;

        var inputX = Input.GetAxis("Horizontal");
        var inputY = Input.GetAxis("Vertical");
        var inputR = Mathf.Clamp(Input.GetAxis("Mouse X"), -1.0F, 1.0F);
        var inputW = Mathf.Clamp(Input.GetAxis("Mouse Y"), -1.0F, 1.0F);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentRotation += 180.0F;
        }

        Vector3 moveVectorX = transform.forward * inputX;
        Vector3 moveVectorY = transform.forward * inputY;
        Vector3 moveVector = (moveVectorX + moveVectorY).normalized * speed * Time.deltaTime;

        currentRotation = ClampAngle(currentRotation + (inputR * rotationSpeed));
        currentVerticalRotation = Mathf.Clamp(ClampAngle(currentVerticalRotation + (inputW * rotationSpeed)), -30, 30);

        Quaternion rotationAngle = Quaternion.Euler(0.0F, currentRotation, 0.0F);

        cameraHolder.GetChild(0).transform.rotation = Quaternion.Euler(-currentVerticalRotation, currentRotation, 0.0F);

        transform.position = transform.position + moveVector;
        transform.rotation = rotationAngle;
        player.rotation = Quaternion.Euler(0.0F, currentRotation, 0.0F);
    }
    */

    // Public
    public GameObject BulletPrefab;
    public float Speed = 5.0F;
    public float RotationSpeed = 5.0F;
    public float jumpSpeed = 5.0F;
    public GameObject playerCamera;

    // Private
    private Vector3 jump;
    private Transform bulletSpawn;
    private Transform cameraHolder;
    private Weapon weapon;
    private Animator anim;
    private Rigidbody playerRigidBody;
    private GameObjectSearcher searcher;
    private float currentRotation = 0.0F;
    private float currentVerticalRotation = 0.0F;
    private int jumpHash = Animator.StringToHash("Jump");

    private void Start()
    {
        anim = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody>();
        searcher = GetComponent<GameObjectSearcher>();
        cameraHolder = transform.Find("CameraHolder");

        Spawn();

        if (isLocalPlayer)
        {
            playerCamera.GetComponent<Camera>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
            playerCamera.GetComponent<Camera>().enabled = false;
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if (weapon == null)
        {
            searcher.FindObjectwithTag("Weapon");
            weapon = searcher.actors[0].GetComponent<Weapon>();
            bulletSpawn = weapon.Origin;
            return;
        }

        float move = Input.GetAxis("Vertical");
        anim.SetFloat("Speed", move);
        anim.SetBool("isBackwards", Input.GetKey(KeyCode.S));

        if (Input.GetMouseButton(0))
        {
            CmdFire();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            playerRigidBody.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            anim.SetTrigger(jumpHash);
        }
    }

    private void FixedUpdate()
    {
        if (weapon == null)
        {
            searcher.FindObjectwithTag("Weapon");
            weapon = searcher.actors[0].GetComponent<Weapon>();
            bulletSpawn = weapon.Origin;
            return;
        }

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

        Vector3 moveVectorX = transform.forward * inputX;
        Vector3 moveVectorY = transform.forward * inputY;
        Vector3 moveVector = (moveVectorX + moveVectorY).normalized * Speed * Time.deltaTime;

        currentRotation = ClampAngle(currentRotation + (inputR * RotationSpeed));
        currentVerticalRotation = Mathf.Clamp(ClampAngle(currentVerticalRotation + (inputW * RotationSpeed)), -30, 30);

        Quaternion rotationAngle = Quaternion.Euler(0.0F, currentRotation, 0.0F);

        playerCamera.transform.rotation = Quaternion.Euler(-currentVerticalRotation, currentRotation, 0.0F);

        transform.position = transform.position + moveVector;
        transform.rotation = rotationAngle;
        playerRigidBody.rotation = Quaternion.Euler(0.0F, currentRotation, 0.0F);
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

    float ClampAngle(float angle)
    {
        if (angle < -360.0F)
            angle += 360.0F;
        else if (angle > 360.0F)
            angle -= 360.0F;

        return angle;
    }

    //[Command]
    void CmdFire()
    {
        var bullet = Instantiate(
            BulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation
            );

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

        Vector3 mousepos = playerCamera.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, weapon.Range));
        bullet.transform.LookAt(mousepos);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * weapon.BulletSpeed;


        NetworkServer.Spawn(bullet);

        Destroy(bullet, 2.0F);
    }
}
