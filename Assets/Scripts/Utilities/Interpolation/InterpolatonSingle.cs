using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// interpolation (single)
public class InterpolatonSingle : Interpolation // MonoBehaviour
{
    // TODO: add interpolation options.

    // time
    public float t = 0;

    // travel points
    public List<Vector3> travelPoints = new List<Vector3>();

    // if 'true', the starting position is added to the list.
    public bool addStartPos;

    // current position
    private Vector3 currPos;

    // index of the travel points list.
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        currPos = transform.position;

        // adds the starting position
        if (addStartPos)
            travelPoints.Add(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        t = Mathf.Clamp(t, 0.0F, 1.0F);

        // clamps the index value
        index = Mathf.Clamp(index, 0, travelPoints.Count);

        transform.position = Vector3.Lerp(currPos, travelPoints[index], t);

        // end of line reached
        if (t >= 1.0F)
            index++;

        // list has been gone through, so go back to the beginning.
        if(index >= travelPoints.Count)
        {
            index = 0;
        }
    }
}
