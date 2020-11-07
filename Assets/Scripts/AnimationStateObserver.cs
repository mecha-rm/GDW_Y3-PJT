using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateObserver : StateObserver
{
    // the animation
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {

        if(anim == null)
            anim = GetComponent<Animator>();
    }

    // called when the state is changed.
    public override void OnStateChange()
    {
        anim.SetInteger("State", subject.GetStateNumber());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
