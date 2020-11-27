using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a serialized object, which is used for storing data for making an object.
// this should be a struct, but it's not since it wouldn't be able to be overridden.
[System.Serializable]
public class SerializedObject
{
    /// <summary>
    /// name: the name of the object when generated.
    /// type: the type of the object. This will be used to generate the base type.
    /// prefabPath: the path (from Resources) for the prefabPath.
    ///     * if the prefab is provided, the type is ignored.
    ///     * if the prefab is left blank, the type is used.
    /// </summary>
    public string name = ""; // the name of the object
    public System.Type type = null; // the type of the object
    public string prefabPath = ""; // the path of the object prefab.

    /// <summary>
    /// components: components attached to the object.
    ///     * these components are generated based on information found in a given class.
    ///     * if a component is already attached to the object, it is grabbed and given the values instead of a new instance being made.
    /// children: children of the serialized object.
    ///     * SerializedObjects that are children of this object.
    ///     * These work in the same fashion to this object.
    ///     * DO NOT ADD MULTIPLE ITERATIONS OF THE SAME OBJECT.
    /// </summary>
    public List<SerializedComponent> components; // components
    public List<SerializedObject> children; // children

    // transformation information
    public Vector3 position = new Vector3(); // position
    public Quaternion rotation = new Quaternion(0, 0, 0, 1); // rotation
    public Vector3 localScale = new Vector3(1, 1, 1); // scale

    // constructor - name and type
    // use typeof(class_name) to get type
    public SerializedObject(string name, System.Type type)
    {
        this.name = name;
        this.type = type;
    }

    // constructor - name, type, and prefab path
    // use typeof(class_name) to get type
    public SerializedObject(string name, System.Type type, string prefabPath)
    {
        this.name = name;
        this.type = type;
        this.prefabPath = prefabPath;
    }

    // constructor - name, type, and transformation
    public SerializedObject(string name, System.Type type, Vector3 pos, Quaternion rot, Vector3 scale)
    {
        this.name = name;
        this.type = type;

        position = pos;
        rotation = rot;
        localScale = scale;
    }

    // constructor - name, type, and transformation
    public SerializedObject(string name, System.Type type, Transform transform)
    {
        this.name = name;
        this.type = type;

        position = transform.position;
        rotation = transform.rotation;
        localScale = transform.localScale;
    }

    // constructor - name, type, prefab path, and Transform
    public SerializedObject(string name, System.Type type, string prefabPath, Vector3 pos, Quaternion rot, Vector3 scale)
    {
        this.name = name;
        this.type = type;
        this.prefabPath = prefabPath;

        position = pos;
        rotation = rot;
        localScale = scale;
    }

    // constructor - name, type, prefab path, and Transform
    public SerializedObject(string name, System.Type type, string prefabPath, Transform transform)
    {
        this.name = name;
        this.type = type;
        this.prefabPath = prefabPath;

        position = transform.position;
        rotation = transform.rotation;
        localScale = transform.localScale;
    }

}