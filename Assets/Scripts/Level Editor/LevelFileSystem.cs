using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


// TODO: rename or remove this.
public class LevelFileSystem : FileStream
{

	public EditorObject temp;

	// Start is called before the first frame update
	void Start()
    {
		base.Start();

		SerializedObject obj = new SerializedObject("john", typeof(GameObject));
		// Test obj = new Test();
		// Vector3 obj = new Vector3();
		// List<int> list = new List<int>();
		// list.Add(1);
		byte[] data;

		// data = FileStream.SerializeObject(list);
		data = FileStream.SerializeObject(obj);
		Debug.Log(data.ToString());

		// FileStream fs = new FileStream();
		// fs.SetRecordFile("FMV.dat");
		// bool x = fs.RecordFileAvailable();
		// 
		// Debug.Log(fs.GetRecordFile() + " : " + fs.RecordFileAvailable());
		// 
		// fs.SetRecordFile("Assets/Resources/Saves/level_test.txt");
		// x = fs.RecordFileAvailable();
		// Debug.Log(fs.GetRecordFile() + " : " + fs.RecordFileAvailable());


		// V3 v3 = new V3();
		// byte[] v3Arr;
		// string v3Str;
		// object v3Obj = new object();
		// 
		// v3.x = 1;
		// v3.y = 2;
		// v3.z = 3;
		// 
		// v3Obj = v3;
		// 
		// 
		// // Convert to Bytes
		// // BinaryFormatter bf = new BinaryFormatter();
		// // MemoryStream ms = new MemoryStream();
		// // 
		// // bf.Serialize(ms, v3Obj);
		// // v3Arr = ms.ToArray();
		// v3Arr = FileStream.SerializeObject(v3);
		// 
		// // v3Str = bf.ToString();
		// v3Str = System.Text.Encoding.ASCII.GetString(v3Arr);
		// v3Arr = System.Text.Encoding.ASCII.GetBytes(v3Str);
		// 
		// // v3Str = System.BitConverter.ToString(v3Arr);
		// // v3Str. = (char)v3Arr[0];
		// 
		// 
		// // Debug.Log(v3Str);
		// 
		// v3 = new V3();
		// v3.x = -1;
		// 
		// // Convert From Bytes
		// // bf = new BinaryFormatter();
		// // ms = new MemoryStream();
		// // 
		// // ms.Write(v3Arr, 0, v3Arr.Length);
		// // ms.Seek(0, 0);
		// 
		// 
		// // GameObject entity = (GameObject)converter.Deserialize(mStream);
		// // v3Obj = bf.Deserialize(ms);
		// // v3 = (V3)v3Obj;
		// // v3 = (V3)bf.Deserialize(ms);
		// v3Obj = FileStream.DeserializeObject(v3Arr);
		// v3 = (V3)(v3Obj);
		// 
		// // prints out data
		// v3Str = "(" + v3.x + ", " + v3.y + ", " + v3.z + ")";
		// Debug.Log(v3Str);
		// 
		// // v3Str = System.Text.Encoding.ASCII.GetString(v3Arr);
		// // v3 = new V3();

		// System.Type t = System.Type.GetType("UnityEngine.GameObject");
		// System.Type t = System.Type.GetType(typeof(GameObject).FullName);
		// System.Type t = (typeof(GameObject).AssemblyQualifiedName).GetType(); // this should work
		// System.Type t = (typeof(GameObject).GetType()); // doesn't work
		// t = (typeof(GameObject).AssemblyQualifiedName).GetType();
		// System.Type t = typeof(GameObject); // this works!
		// // object test = (System.Activator.CreateInstance(t));
		// 
		// System.Reflection.Assembly assembly = typeof(GameObject).Assembly;
		// Debug.Log("Assembly Name: " + assembly.GetName());
		// 
		// // get the type, convert it to data, then convert it back afterwards
		// byte[] arr = SerializeObject(t); // conversion works
		// t = null;
		// t = (System.Type)DeserializeObject(arr);
		// 
		// 
		// System.Type t2 = System.Type.GetType(t.Name); // GameObject
		// t2 = System.Type.GetType(t.FullName); // Unity.GameObject
		// 
		// Debug.Log((t == null) ? "null" : t.Name); 
		// object o = System.Activator.CreateInstance(t);
		// GameObject go = (GameObject)o; // generates game object
		// Debug.Log((go == null) ? "null" : go.ToString());

		/*
		 * Parent (Serializable Object)
		 *	- System.Type of Parent Object
		 *	- List of Components (Serialized Components)
		 *		- Components get 
		 * Components (SerializedComponent):
		 *	- System.Type of Child Object (gets created and upcasted to SerializedComponent)
		 *	- Child Values (call Virtual Function in Order to Have these downcasted0
		 *	
		 */

		/*
		 * So here's what's going to be done:
		 * (1) You get the type of the parent, its name, its prefab (if applicable) and save that as System.Type.
		 * (2) The parent class is inherited by children that store any other base values they need, which also get saved.
		 * (3) You get all components, and save every component that can be upcasted to SerializableObject. You use ExportSerializeComponent for this.
			* Anything that can't be upcasted would just be set in one of the classes that CAN be upcasted. 
		 * (4) You serialize the object and its data. You put it in a file.
		 * (5) You take the data out of the file and unpack it back to a SerializedObject.
		 * (6) You use the parent's type (System.Type) to create the base object, or you load up the prefab if that's provided.
			* You also give it its name.	
		 * (7) You go through each component, creating the type (or prefab) it applies to. You add each component at this stage.
		 * (8) If a component can be upcasted to SerializableObject, you do that.
		 * (9) You call the ImportSerializedComponent function, giving it the component and the SerializedComponent class.
		 * (10) When downcasted, the SerializedComponent should have the values that are needed to be filled. Said values are then filled.
		 */

		// 
		// object objTemp;
		// byte[] arrTemp = FileStream.SerializeObject(temp);
		// Destroy(temp);
		// objTemp = FileStream.DeserializeObject(arrTemp);
		// GameObject goTemp = Instantiate((GameObject)objTemp);
		// temp = goTemp.GetComponent<StageObject>();
		// Debug.Log("goTemp - " + temp.entityName);

	}


	// Update is called once per frame
	void Update()
    {
        
    }
}
