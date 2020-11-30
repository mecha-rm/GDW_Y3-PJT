using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// serialized as an independent object (not needed)
// [System.Serializable]
// public class SerializedStageObject : SerializedObject
// {
//     public SerializedStageObject(string name) : base(name)
//     {
//     }
// }

// serialized as a component
[System.Serializable]
public class SerializedStageObjectComponent : SerializedComponent
{
    public SerializedStageObjectComponent() : base(typeof(StageObject)) 
    {

    }
}

// class
public class StageObject : SerializableObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // exports serialized component. If the prefab is not set, it is provided by the stage object.
    // for the export from the level editor, it is set through this component.
    public override SerializedComponent ExportSerializedComponent(SerializedObject serialObject)
    {
        // typeof(StageObject) and GetType() are the same in this context.
        SerializedComponent newComponent = new SerializedComponent(GetType());
        serialObject.prefabPath = prefabPath; // gives prefab path in case it's not set.

        return newComponent;
    }

    // saves prefab path
    public override void ImportSerializedComponent(SerializedObject serialObject, SerializedComponent component)
    {
        prefabPath = serialObject.prefabPath;
        return;
    }
}
