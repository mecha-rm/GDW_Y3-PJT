using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public struct V3
{
	public int x;
	public int y;
	public int z;
}

public class LevelFileSystem : FileStream
{
	public StageObject temp;

	// Start is called before the first frame update
	void Start()
    {
		base.Start();

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

		// System.Type t = System.Type.GetType(typeof(GameObject).FullName, "UnityEngine");
		// GameObject go = (GameObject)System.Activator.CreateInstance(System.Type.GetType("GameObject"));
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
