using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    // the bgm for the stage.
    public AudioClip bgm;

    // flag spawn position
    public GameObject flag = null;
    public Vector3 flagSpawn = new Vector3();
    public bool useFlagPosAsSpawn = true; // use flag's position as its spawn point.
    // public List<Vector3> flagSpawns = new List<Vector3>();
    // public bool randomizeFlagSpawn = false;

    // possible player spawn positions
    public List<Vector3> playerSpawns = new List<Vector3>();
    public bool randomPlayerPos = false; // randomizes player position


    // Start is called before the first frame update
    void Start()
    {
        // flag not set
        if (flag == null)
        {
            flag = GameObject.Find("Flag");

            if(flag == null) // if the flag doesn't exist, make one.
            {
                flag = Instantiate((GameObject)(Resources.Load("Prefabs/Flag")));
            }
        }

        // if the flag exists, change its position
        if(flag != null)
        {
            if (useFlagPosAsSpawn)
                flagSpawn = flag.transform.position;
            else
                flag.transform.position = flagSpawn;
        }


        // no spawns added
        if (playerSpawns.Count == 0)
            playerSpawns.Add(new Vector3(0, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
