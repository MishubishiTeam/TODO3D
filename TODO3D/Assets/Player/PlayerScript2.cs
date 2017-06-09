using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerScript2 : NetworkBehaviour
{

    // Public
    public GameObject BulletPrefab;
    public float Speed = 5.0F;
    public float RotationSpeed = 5.0F;
    public float jumpSpeed = 5.0F;
    public GameObject playerCamera;
    [SyncVar]
    public float currentHealth = maxHealth;
    public float respawnTime;
    public GameObject healthBar;
    public GameObject mesh;

    public const int maxHealth = 100;

    public bool isDead { get { return currentHealth <= 0; } }

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
    private float respawnTimer;
    private Image healthBarTransform;
    private SkinnedMeshRenderer meshRenderer;
    private DateTime timeOfDeath;

    private void Start()
    {
        anim = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody>();
        searcher = GetComponent<GameObjectSearcher>();
        cameraHolder = transform.Find("CameraHolder");
        respawnTimer = respawnTime;
        healthBarTransform = healthBar.GetComponent<Image>();
        meshRenderer = mesh.GetComponent<SkinnedMeshRenderer>();

        if (isLocalPlayer)
        {
            playerCamera.GetComponent<Camera>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
            playerCamera.GetComponent<Camera>().enabled = false;

        Spawn();
    }

    [ClientRpc]
    private void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            meshRenderer.enabled = true;
            foreach (var item in weapon.GetComponentsInChildren<MeshRenderer>())
            {
                item.enabled = true;
            }
            currentHealth = maxHealth;
            playerCamera.GetComponent<Camera>().enabled = true;
            System.Random rnd = new System.Random();
            var spawns = GameObject.FindGameObjectsWithTag("Spawn").ToList();
            var spawn = spawns[rnd.Next(spawns.Count)];
            transform.position = spawn.transform.position;
        }
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            ApplyDamage(100);
        }

        if (isDead)
        {
            RpcRespawn();
        }

        healthBarTransform.fillAmount = currentHealth / 100;



        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

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

        CheckShoot();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            playerRigidBody.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            anim.SetTrigger(jumpHash);
        }
    }

    private void CheckShoot()
    {
        switch (weapon.Mode)
        {
            case Weapon.ShootingMode.CoupParCoup:
                if (Input.GetMouseButtonDown(0))
                    CmdFire();
                break;
            case Weapon.ShootingMode.Rafale:
                break;
            case Weapon.ShootingMode.Auto:
                if (Input.GetMouseButton(0))
                    CmdFire();
                break;
            default:
                break;
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

        Vector3 moveVectorX = transform.forward * inputY;
        Vector3 moveVectorY = transform.right * inputX;
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

    public void ApplyDamage(float dmg)
    {
        if (!isServer)
            return;
        currentHealth -= dmg;
        if (isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        timeOfDeath = DateTime.Now;
        currentHealth = 0;
        meshRenderer.enabled = false;
        foreach (var item in weapon.GetComponentsInChildren<MeshRenderer>())
        {
            item.enabled = false;
        }
        playerCamera.GetComponent<Camera>().enabled = false;
        Camera.main.enabled = true;
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

    [Command]
    void CmdFire()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (weapon.CanShoot)
        {
            weapon.Tirer();
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
}
