using UnityEngine;

public class CollisionStateMachine : StateMachine
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // the object is triggered.
    public void OnTriggerEnter(Collider other)
    {
        SetState(1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
