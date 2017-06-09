using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapScript : MonoBehaviour {


    private GameObject Map;
    public GameObject MiniMap;
    public GameObject Joueur;
    public GameObject PtRouge;
    // Use this for initialization
    void Start()
    {
        Map = GameObject.FindGameObjectWithTag("GameMap");
    }

    // Update is called once per frame
    void Update()
    {
        PtRouge.transform.position = new Vector3(
            (-(float)Joueur.transform.position.x / (float)Map.GetComponent<Collider>().bounds.size.x) * 100+MiniMap.transform.position.x,
            (-(float)Joueur.transform.position.z / (float)Map.GetComponent<Collider>().bounds.size.z) * 100 + MiniMap.transform.position.y,
            0);
    }
}
