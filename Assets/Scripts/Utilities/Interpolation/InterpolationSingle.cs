using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// interpolation (single)
public class InterpolationSingle : Interpolation // MonoBehaviour
{
    // interpolation type
    // NOTE: if 'Interpolate()' is overridden, this variable is disregarded.
    public interType mode;

    // time
    public float t = 0;

    // travel points
    public List<Vector3> travelPoints = new List<Vector3>();

    // if 'true', the starting position is added to the list.
    public bool addStartPos;

    // current position
    private Vector3 startPos;

    // index of the travel points list.
    public int index;

    // if 'true', the interpolation loops.
    public bool loop = true;

    // if 'true', the interpolation process is paused.
    public bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;

        // adds the starting position
        if (addStartPos)
            travelPoints.Add(transform.position);
    }

    // function for interpolation
    // override this function to change the interpolation type
    public virtual void Interpolate(Vector3 v1, Vector3 v2, float t)
    {
        transform.position = InterpolateByType(mode, v1, v2, t);
        // transform.position = Vector3.Lerp(v1, v2, t);
    }

    // checks to see if the path is paused.
    public bool IsPaused()
    {
        return paused;
    }

    // sets whether the process should be paused or not.
    public void SetPause(bool pause)
    {
        paused = pause;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!paused)
        {
            t += Time.deltaTime;
            t = Mathf.Clamp(t, 0.0F, 1.0F);

            // clamps the index value
            index = Mathf.Clamp(index, 0, travelPoints.Count);

            // transform.position = Vector3.Lerp(currPos, travelPoints[index], t);
            Interpolate(startPos, travelPoints[index], t);

            // end of segment reached
            if (t >= 1.0F)
            {
                index++;
                t = 0.0F;
            }

            // list has been gone through, so go back to the beginning.
            if (index >= travelPoints.Count)
            {
                index = 0;

                // if the path shouldn't loop, it pauses the interpolation after it's down.
                if (!loop)
                    paused = true;
            }
        }
    }
}
