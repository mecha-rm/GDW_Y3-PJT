using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// member of interpolation list.
public class InterpolationMember : MonoBehaviour
{
    // starting position
    public Vector3 startPos;

    // index of interpolation list
    public int index;

    // t value
    public float t;

    // factor that the increase in t is multiplied by.
    // if set to '1', t is just increased by deltaTime.
    public float speed = 1.0F;

    // if the member should be paused.
    public bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
