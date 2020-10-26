using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the state machine class is based on a script written for assignment 1 of Game Engine Design & Implementation
// observes the state of a subject
public abstract class StateObserver : MonoBehaviour
{
    // the subject being observed
    public StateMachine subject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // gets the subject this observer is observing
    public StateMachine GetSubject()
    {
        return subject;
    }

    // sets the subject of the observer
    public void SetSubject(StateMachine newSubject)
    {
        // detaches observer from current subject
        if (subject != null)
            subject.DetachObserver(this);

        // attaches this observer to a new subject. This sets the value of 'subject'.
        if(newSubject != null)
            newSubject.AttachObserver(this);
    }

    // called by the subject object when the state changes. This must be overridden by observer subclasses.
    public abstract void OnStateChange();

    // Update is called once per frame
    void Update()
    {

    }
}
