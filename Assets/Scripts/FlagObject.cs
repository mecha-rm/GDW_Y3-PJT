using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// flag object
public class FlagObject : MonoBehaviour
{
    // the owner of the flag.
    PlayerObject owner = null;

    // TODO: replace with vector of spawn positions
    private Vector3 spawnPos;


    // Start is called before the first frame update
    void Start()
    {
        spawnPos = transform.position;
    }

    // if the flag collides with something.
    // in order for this to work, the script MUST be on the same game object the collider is on.
    private void OnCollisionEnter(Collision collision)
    {
        
        PlayerObject po = collision.gameObject.GetComponent<PlayerObject>();

        // calls the player's function instead so that the sound effect plays.
        // if (po != null)
        //     AttachToPlayer(po);

        if (po != null)
            po.AttachFlag(this);

    }

    // attaches the flag to the player
    public void AttachToPlayer(PlayerObject po)
    {
        po.flag = this;
        owner = po;
        // gameObject.SetActive(false); // hide object
        transform.parent.gameObject.SetActive(false); // hide whole parent
        
        // shows flag indicator
        if (po.flagIndicator != null)
            po.flagIndicator.SetActive(true);
    }

    // detaches the flag from the player
    public void DetachFromPlayer()
    {
        owner.flag = null;
        // gameObject.SetActive(true);
        transform.parent.gameObject.SetActive(true);

        // hides flag indicator
        if (owner.flagIndicator != null)
            owner.flagIndicator.SetActive(false);

        owner = null;

        // TODO: repositon the flag
        // this is temporary; the flag should have spawn positions to return to.
        transform.position = spawnPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
