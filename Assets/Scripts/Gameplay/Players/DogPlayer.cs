using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// dog
public class DogPlayer : PlayerObject
{
    // Start is called before the first frame update
    void Start()
    {
        // call object.
        base.Start();

        speedMult = 1.0F;
        knockbackMult = 1.5F;
        jumpMult = 1.0F;
        defenseMult = 1.0F;

        // death sound
        if (sfx_Death.clip == null)
        {
            // Destroy(sfx_Idle.clip);
            sfx_Death.clip = (AudioClip)Resources.Load("Audio/Cat/SFX_DOG_DEATH");
        }

        // loads the player icon
        LoadPlayerIcon("Prefabs/UI/Dog Box");

        // gets text object.
        if (playerIcon != null && playerScoreText == null)
        {
            playerScoreText = playerIcon.GetComponentInChildren<Text>();
        }

        //icons
        //GameObject parentObject = GameObject.Find("Players");
        //int childCount = parentObject.transform.childCount;

        //for (int index = 0; index < childCount; index++)
        //{
        //    GameObject childObject = parentObject.transform.GetChild(index).gameObject;
        //    if (childObject.name == playername)
        //    {
        //        childObject.SetActive(true);
        //        playerIcon = childObject;
        //    }

        //}

    }

    // gets the player type
    public override GameBuilder.playables GetPlayerType()
    {
        return GameBuilder.playables.dog;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
