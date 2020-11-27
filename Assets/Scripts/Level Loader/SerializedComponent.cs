using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// serialized component
// this class is used for adding components to a SerializedObject
// this is meant to be inherited so that other values can be provided.
// it's basically a struct, but it's not since a struct can be inherited.
[System.Serializable]
public class SerializedComponent
{
    public System.Type type = null;

    // constructor
    // use typeof(class_name) to get type
    public SerializedComponent(System.Type type)
    {
        this.type = type;
    }
}
