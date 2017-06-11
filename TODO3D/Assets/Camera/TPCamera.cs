using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TPCamera : NetworkBehaviour {

    public Transform LookAt;
    public Transform camTransform;

    private const float Y_ANGLE_MIN = -50f;
    private const float Y_ANGLE_MAX = 40.0f;

    private Camera Cam;

    public float sensivity = 4.0f;
    public float currentX = 0f;
    public float currentY = 0f;
    private float distance = 0.7f;

	// Use this for initialization
	void Start () {
        camTransform = transform;
        Cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        //if (!isLocalPlayer) return;
        currentX += Input.GetAxis("Mouse X");
        currentY -= Input.GetAxis("Mouse Y");

        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }

    private void LateUpdate()
    {
        //if (!isLocalPlayer) return;
        Vector3 pos = new Vector3(0, 0.5f, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        camTransform.position = LookAt.position + rotation * pos;
        camTransform.LookAt(new Vector3(LookAt.position.x, LookAt.position.y+0.4f, LookAt.position.z));
    }
}
