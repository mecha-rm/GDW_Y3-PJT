using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the state machine class is based on a script written for assignment 1 of Game Engine Design & Implementation
// allows for an entity to have different states
public class StateMachine : MonoBehaviour
{
    // the state number and the names used to identifiy a given number.
    // the names are case sensitive
    public int state = 0;
    public List<string> stateNames = new List<string>();

    // a list of state observers. When the state is changed, these state observers will be notified.
    public List<StateObserver> observers;

    // Start is called before the first frame update
    void Start()
    {
        // if the state is negative
        if (state < 0)
            state = 0;

        // sets subject for all starting observers
        for(int i = 0; i < observers.Count; i++)
            observers[i].subject = this;
    }

    // gets the state number
    public int GetStateNumber()
    {
        return state;
    }

    // gets the state identifier
    public string GetStateName()
    {
        // returns the state name
        // if there are state names, the state is not negative (which shouldn't be possible), and the state has a name...
        // the state's name is returned.
        if (stateNames.Count != 0 && state >= 0 && state < stateNames.Count)
        {
            return stateNames[state]; // state name
        }
        else
        {
            return ""; // empty
        }
    }

    // sets the state via its numerical value (cannot be set to -1)
    // state will not change if integer identifier is invalid
    // the observers will still be notified, even if the state did not change.
    public void SetState(int newState)
    {
        if (newState >= 0)
        {
            state = newState;
            NotifyObservers();
        }
    }

    // sets a new state based on the provided string
    // state will not change if string identifier is invalid
    public void SetState(string newState)
    {
        // list.findIndex - update this function to use findIndex instead.
        int index = -1;

        // finds the state name
        for (int i = 0; i < stateNames.Count; i++)
        {
            // the new state has been found
            if (stateNames[i] == newState)
            {
                index = i;
                break;
            }
        }

        // if the index is not set to -1, then a new state has been set.
        if (index != -1)
        {
            state = index;
            NotifyObservers();
        }
    }

    // called when the state is changed to notify all observers.
    private void NotifyObservers()
    {
        // called when the state changes
        foreach (StateObserver observer in observers)
        {
            observer.subject = this; // in case the observer doesn't have it already
            observer.OnStateChange();
        }
            
    }

    // adds a new state. Do note that this does NOT allow for states of the same name to be added, though it is case sensitive.
    // if two states of the same name are added through the Inspector, only the first instance of that state will ever be acknowledged.
    public void AddStateName(string newState)
    {
        // checks to see if the state name is in the list.
        // if it isn't, it gets added to the list.
        if (stateNames.Contains(newState))
            return;
        else // state name is not in the list.
            stateNames.Add(newState);
    }

    // removes a state from the list
    public void RemoveStateName(string stateName)
    {
        stateNames.Remove(stateName);
    }

    // attaches an observer
    public void AttachObserver(StateObserver observer)
    {
        // adds an observer and sets its 'subject' to this.
        observers.Add(observer);
        observer.subject = this;
    }

    // detaches an observer
    public void DetachObserver(StateObserver observer)
    {
        // removes the observer and sets 'subject' to null.
        observers.Remove(observer);
        observer.subject = null;
    }

    // returns 'true' if the observer list contains the provided observer
    public bool ContainsObserver(StateObserver observer)
    {
        return observers.Contains(observer);
    }

    // returns an observer's index in the list. Returns -1 if the observer is not in the list.
    public int GetObserver(StateObserver observer)
    {
        // if the observer is not in the list, return -1.
        if (!observers.Contains(observer))
            return -1;

        // searches the list for the observer
        for(int i = 0; i < observers.Count; i++)
        {
            if (observers[i] == observer)
                return i;
        }

        // observer was not in list
        return -1;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
