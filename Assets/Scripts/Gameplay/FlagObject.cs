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

    // flag transfer cooldown for when the flag goes from one player to another.
    // private float flagTransferCooldown = 0.0F;
    // private float FLAG_TRANSFER_COOLDOWN_MAX = 3.0F;

    // used to give the flag from one player to another. It doesn't work otherwise.
    // this can work at 1, but it's set to 2 just to be safe.
    private int MAX_WAIT_CYCLES = 2;
    private int waitCycles = 0;
    PlayerObject nextPlayer = null;


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
        // if the flag is already owned by someone, then it detaches itself from them.
        if(owner != null)
            owner.flag = null;

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

    // transfers the flag to player 2
    public void TransferFlag(PlayerObject p2)
    {
        // cannot transfer flag.
        if (p2 == null)
            return;

        DetachFromPlayer();
        // AttachToPlayer(p2); // this doesn't work

        // moves flag offscreen
        transform.position = new Vector3(10000.0F, 10000.0F, 10000.0F);

        nextPlayer = p2;
        waitCycles = MAX_WAIT_CYCLES;
    }

    // Update is called once per frame
    void Update()
    {
        // countdown to allow the flag to transfer.
        // if(flagTransferCooldown > 0.0F)
        // {
        //     // reduces countdown timer
        //     flagTransferCooldown -= Time.deltaTime;
        // 
        //     // stopping value form going below 0.0F.
        //     if (flagTransferCooldown < 0.0F)
        //         flagTransferCooldown = 0.0F;
        // }

        // reduces wait cycles
        if(waitCycles > 0)
        {
            waitCycles--;

            // wait cycle time has been reached.
            if (waitCycles <= 0)
            {
                waitCycles = 0;

                // attaches flag to new player.
                if (nextPlayer != null)
                    AttachToPlayer(nextPlayer);
            }
        }
    }
}
