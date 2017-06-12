using Prototype.NetworkLobby;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerScript2 : NetworkBehaviour
{
    public struct PlayerColor
    {
        public float r;
        public float g;
        public float b;
        public PlayerColor(float R, float G, float B)
        {
            r = R;
            g = G;
            b = B;
        }
    }


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
    public GameObject ammoText;
    public GameObject nameLabel;
    public GameObject reloadingLabel;
    [SyncVar]
    public string playerName;
    [SyncVar]
    public PlayerColor color;
    

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
    private new TPCamera camera;
    private Text ammotext;
    private void Start()
    {
        anim = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody>();
        searcher = GetComponent<GameObjectSearcher>();
        cameraHolder = transform.Find("Camera Holder");
        respawnTimer = respawnTime;
        healthBarTransform = healthBar.GetComponent<Image>();
        meshRenderer = mesh.GetComponent<SkinnedMeshRenderer>();
        camera = cameraHolder.GetChild(0).GetComponent<TPCamera>();
        ammotext = ammoText.GetComponent<Text>();
        ammotext.color = Color.yellow;
        nameLabel.GetComponent<TextMesh>().text = playerName;
        nameLabel.GetComponent<TextMesh>().color = new Color(color.r, color.g, color.b);

        if (isLocalPlayer)
        {
            playerCamera.GetComponent<Camera>().enabled = true;
            nameLabel.GetComponent<TextMesh>().GetComponent<Renderer>().enabled = false;
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
            
            playerCamera.GetComponent<Camera>().enabled = true;
            System.Random rnd = new System.Random();
            var spawns = GameObject.FindGameObjectsWithTag("Spawn").ToList();
            var spawn = spawns[rnd.Next(spawns.Count)];
            transform.position = spawn.transform.position;
            playerRigidBody.velocity = Vector3.zero;
        }
    }

    private void Update()
    {

        if (!isLocalPlayer)
            return;

        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            ApplyDamage(100);
        }

        ammotext.text = weapon.ActualCapacity.ToString() + " / " + weapon.MaxCapacity.ToString();
        if ((float)weapon.ActualCapacity / (float)weapon.MaxCapacity <= 0.15F)
            ammotext.color = Color.red;
        else
            ammotext.color = Color.yellow;

        healthBarTransform.fillAmount = currentHealth / 100;

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

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("jump_inPlace"))
        {
            anim.GetHashCode();
        }

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
                    Fire();
                break;
            case Weapon.ShootingMode.Rafale:
                break;
            case Weapon.ShootingMode.Auto:
                if (Input.GetMouseButton(0))
                    Fire();
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
        Quaternion rotationAngle = Quaternion.Euler(0.0F, currentRotation, 0.0F);

        Vector3 moveVectorX = transform.forward * inputY;
        Vector3 moveVectorY = transform.right * inputX;
        Vector3 moveVector = (moveVectorX + moveVectorY).normalized * Speed * Time.deltaTime;

        transform.position = transform.position + moveVector;
        transform.rotation = rotationAngle;
        playerRigidBody.rotation = Quaternion.Euler(0.0F, camera.currentX, 0.0F);
    }

    private bool isGrounded()
    {
        var collider = GetComponent<Collider>();
        return !Physics.CapsuleCast(collider.bounds.center, collider.bounds.center - new Vector3(0.1F, 0.1F, 0.1F), 0.3F, -Vector3.up, 1F);
    }

    public void ApplyDamage(float dmg)
    {
        if (!isServer)
            return;
        currentHealth -= dmg;
        if (isDead)
        {
            currentHealth = maxHealth;
            RpcRespawn();
        }
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

    void Fire()
    {
        Vector3 mousepos = playerCamera.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, weapon.Range));
        if (weapon.CanShoot)
            weapon.MakeSomeNoiseBoomBoom();
        CmdFire(mousepos.x, mousepos.y, mousepos.z);
    }

    [Command]
    void CmdFire(float dirX, float dirY, float dirZ)
    {
        if (weapon.CanShoot)
        {
            weapon.Tirer();
            var bullet = Instantiate(
            BulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation
            );


            bullet.GetComponent<BulletScript>().sender = this;
            bullet.GetComponent<BulletScript>().dmg = weapon.damage;

            //Vector3 mousepos = playerCamera.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, weapon.Range));
            bullet.transform.LookAt(new Vector3(dirX, dirY, dirZ));

            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * weapon.BulletSpeed;


            NetworkServer.Spawn(bullet);

            Destroy(bullet, 2.0F);
        }
    }
}
