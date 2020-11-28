using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// STRUCTS
// Vectors and Quaternions can't be serialized, so shortforms needed to be made.
// basic vector 2
[System.Serializable]
public struct Vec2
{
    public float x;
    public float y;
}

// basic vector 3
[System.Serializable]
public struct Vec3
{
    public float x;
    public float y;
    public float z;
}

// basic vector 4
[System.Serializable]
public struct Vec4
{
    public float x;
    public float y;
    public float z;
    public float w;
}

// basic quaternion
[System.Serializable]
public struct Quat
{
    public float x;
    public float y;
    public float z;
    public float w;
}

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
    public System.Type type = null; // the type of the object (just set this to GameObject type)
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
    public List<SerializedComponent> components = new List<SerializedComponent>(); // components
    public List<SerializedObject> children = new List<SerializedObject>(); // children

    // transformation information
    // public Vector3 position = new Vector3(); // position
    // public Quaternion rotation = new Quaternion(0, 0, 0, 1); // rotation
    // public Vector3 localScale = new Vector3(1, 1, 1); // scale

    public Vec3 position; // position
    public Quat rotation; // rotation
    public Vec3 localScale; // scale

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

        position = SerializedObject.Vector3ToVec3(pos);
        rotation = SerializedObject.QuaternionToQuat(rot);
        localScale = SerializedObject.Vector3ToVec3(scale);
    }

    // constructor - name, type, and transformation
    public SerializedObject(string name, System.Type type, Transform transform)
    {
        this.name = name;
        this.type = type;

        position = SerializedObject.Vector3ToVec3(transform.position);
        rotation = SerializedObject.QuaternionToQuat(transform.rotation);
        localScale = SerializedObject.Vector3ToVec3(transform.localScale);
    }

    // constructor - name, type, prefab path, and Transform
    public SerializedObject(string name, System.Type type, string prefabPath, Vector3 pos, Quaternion rot, Vector3 scale)
    {
        this.name = name;
        this.type = type;
        this.prefabPath = prefabPath;

        position = SerializedObject.Vector3ToVec3(pos);
        rotation = SerializedObject.QuaternionToQuat(rot);
        localScale = SerializedObject.Vector3ToVec3(scale);
    }

    // constructor - name, type, prefab path, and Transform
    public SerializedObject(string name, System.Type type, string prefabPath, Transform transform)
    {
        this.name = name;
        this.type = type;
        this.prefabPath = prefabPath;

        position = SerializedObject.Vector3ToVec3(transform.position);
        rotation = SerializedObject.QuaternionToQuat(transform.rotation);
        localScale = SerializedObject.Vector3ToVec3(transform.localScale);
    }

    // STRUCT TO UNITY ENGINE CONVERSION

    // unity vector2 to vec2
    public static Vec2 Vector3ToVec3(UnityEngine.Vector2 v1)
    {
        Vec2 v2 = new Vec2();
        v2.x = v1.x;
        v2.y = v1.y;

        return v2;
    }

    // vec2 to unity vector2
    public static UnityEngine.Vector2 Vec3ToUnityVector3(Vec2 v1)
    {
        Vector2 v2 = new Vector2();
        v2.x = v1.x;
        v2.y = v1.y;

        return v2;
    }

    // unity vector3 to vec3
    public static Vec3 Vector3ToVec3(UnityEngine.Vector3 v1)
    {
        Vec3 v2 = new Vec3();
        v2.x = v1.x;
        v2.y = v1.y;
        v2.z = v1.z;

        return v2;
    }

    // vec3 to unity vector3
    public static UnityEngine.Vector3 Vec3ToUnityVector3(Vec3 v1)
    {
        Vector3 v2 = new Vector3();
        v2.x = v1.x;
        v2.y = v1.y;
        v2.z = v1.z;

        return v2;
    }

    // unity vector4 to vec4
    public static Vec4 Vector4ToVec4(UnityEngine.Vector4 v1)
    {
        Vec4 v2 = new Vec4();
        v2.x = v1.x;
        v2.y = v1.y;
        v2.z = v1.z;
        v2.w = v1.w;

        return v2;
    }

    // vec4 to unity vector4
    public static UnityEngine.Vector4 Vec4ToUnityVector4(Vec4 v1)
    {
        Vector4 v2 = new Vector4();
        v2.x = v1.x;
        v2.y = v1.y;
        v2.z = v1.z;
        v2.w = v1.w;

        return v2;
    }

    // unity vector4 to vec4
    public static Quat QuaternionToQuat(UnityEngine.Quaternion q1)
    {
        Quat q2 = new Quat();
        q2.x = q1.x;
        q2.y = q1.y;
        q2.z = q1.z;
        q2.w = q1.w;

        return q2;
    }

    // vec4 to unity vector4
    public static UnityEngine.Quaternion QuatToQuaternion(Quat q1)
    {
        Quaternion q2 = new Quaternion();
        q2.x = q1.x;
        q2.y = q1.y;
        q2.z = q1.z;
        q2.w = q1.w;

        return q2;
    }


}