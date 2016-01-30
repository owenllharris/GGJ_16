//recreated by Neodrop. 
//mailto : neodrop@unity3d.ru
using UnityEngine;
using System.Collections;


#if UNITY_EDITOR
using UnityEditor;
#endif

/*
Attach this script as a parent to some game objects. The script will then combine the meshes at startup.
This is useful as a performance optimization since it is faster to render one big mesh than many small meshes. See the docs on graphics performance optimization for more info.

Different materials will cause multiple meshes to be created, thus it is useful to share as many textures/material as you can.
*/
//[ExecuteInEditMode()]
[AddComponentMenu("Mesh/Combine Children")]
public class CombineChildrenExtended : EditorWindow 
{

	/// Usually rendering with triangle strips is faster.
	/// However when combining objects with very low triangle counts, it can be faster to use triangles.
	/// Best is to try out which value is faster in practice.
	public int frameToWait = 0;
	public bool generateTriangleStrips = true, 
	combineOnStart = true, 
	destroyAfterOptimized = false, 
	castShadow = true, 
	receiveShadow = true, 
	keepLayer = true, 
	addMeshCollider = false;

	[MenuItem("Window/Mesh/CombineChildrenExtended")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(CombineChildrenExtended));
	}

	void OnGUI()
	{		
		generateTriangleStrips = EditorGUILayout.Toggle("generateTriangleStrips", generateTriangleStrips);
		destroyAfterOptimized = EditorGUILayout.Toggle("destroyAfterOptimized", destroyAfterOptimized);
		castShadow = EditorGUILayout.Toggle("castShadow", castShadow);
		receiveShadow = EditorGUILayout.Toggle("receiveShadow", receiveShadow);
		keepLayer = EditorGUILayout.Toggle("keepLayer", keepLayer);
		addMeshCollider = EditorGUILayout.Toggle("addMeshCollider", addMeshCollider);		

		EditorGUILayout.Separator();
		if(GUILayout.Button("Combine Mesh"))
		{
			Combine(Selection.activeGameObject);
		}
		EditorGUILayout.Separator();
		if(GUILayout.Button("Save Mesh As Asset"))
		{
			SaveMeshAsAsset(Selection.activeGameObject);
		}
	}

	public void Combine (GameObject currentGO) 
	{
		#if UNITY_EDITOR
		if (!Application.isPlaying)
			Undo.RegisterSceneUndo("Combine meshes");
		#endif
		Component[] filters  = currentGO.GetComponentsInChildren(typeof(MeshFilter));
		Matrix4x4 myTransform = currentGO.transform.worldToLocalMatrix;
		Hashtable materialToMesh= new Hashtable();

		for (int i=0;i<filters.Length;i++)
		{
			MeshFilter filter = (MeshFilter)filters[i];
			Renderer curRenderer  = filters[i].GetComponent<Renderer>();
			CombineInstance instance = new CombineInstance();

			instance.mesh = filter.sharedMesh;
			if (curRenderer != null && curRenderer.enabled && instance.mesh != null)
			{
				instance.transform = myTransform * filter.transform.localToWorldMatrix;

				Material[] materials = curRenderer.sharedMaterials;
				for (int m = 0; m < materials.Length; m++) 
				{
					instance.subMeshIndex = System.Math.Min(m, instance.mesh.subMeshCount - 1);

					ArrayList objects = (ArrayList)materialToMesh[materials[m]];
					if (objects != null) 
					{
						objects.Add(instance);
					}
					else
					{
						objects = new ArrayList ();
						objects.Add(instance);
						materialToMesh.Add(materials[m], objects);
					}
				}
				if (Application.isPlaying && destroyAfterOptimized && combineOnStart) 
					Destroy(curRenderer.gameObject);
				else if (destroyAfterOptimized)
					DestroyImmediate(curRenderer.gameObject);
				else 
					curRenderer.enabled = false;
			}
		}

		foreach (DictionaryEntry de  in materialToMesh) 
		{
			ArrayList elements = (ArrayList)de.Value;
			CombineInstance[] instances = (CombineInstance[])elements.ToArray(typeof(CombineInstance));

			// We have a maximum of one material, so just attach the mesh to our own game object
			if (materialToMesh.Count == 1)
			{
				// Make sure we have a mesh filter & renderer
				if (currentGO.GetComponent(typeof(MeshFilter)) == null)
				{
					currentGO.gameObject.AddComponent<MeshFilter>();
				}
				if (currentGO.GetComponent<MeshRenderer>() == null)
				{
					currentGO.gameObject.AddComponent<MeshRenderer>();
				}

				MeshFilter filter = currentGO.GetComponent<MeshFilter>();
				filter.mesh = new Mesh();
				if(Application.isPlaying)
				{
					filter.mesh.CombineMeshes(instances, generateTriangleStrips);
				}
				else 
				{
					filter.sharedMesh.CombineMeshes(instances, generateTriangleStrips);
				}
				currentGO.GetComponent<Renderer>().material = (Material)de.Key;
				currentGO.GetComponent<Renderer>().enabled = true;
				if (addMeshCollider)
				{
					currentGO.gameObject.AddComponent<MeshCollider>();
				}
				currentGO.GetComponent<Renderer>().castShadows = castShadow;
				currentGO.GetComponent<Renderer>().receiveShadows = receiveShadow;
			}
			// We have multiple materials to take care of, build one mesh / gameobject for each material
			// and parent it to this object
			else
			{
				GameObject go = new GameObject("Combined mesh");
				if (keepLayer) 
				{
					go.layer = currentGO.gameObject.layer;
				}
				go.transform.parent = currentGO.transform;
				go.transform.localScale = Vector3.one;
				go.transform.localRotation = Quaternion.identity;
				go.transform.localPosition = Vector3.zero;
				go.AddComponent(typeof(MeshFilter));
				go.AddComponent<MeshRenderer>();
				go.GetComponent<Renderer>().material = (Material)de.Key;
				MeshFilter filter = currentGO.GetComponent<MeshFilter>();
				filter.mesh = new Mesh();
				if(Application.isPlaying)
				{
					filter.mesh.CombineMeshes(instances, generateTriangleStrips);
				}
				else 
				{
					filter.sharedMesh.CombineMeshes(instances, generateTriangleStrips);
				}
				go.GetComponent<Renderer>().castShadows = castShadow;
				go.GetComponent<Renderer>().receiveShadows = receiveShadow;
				if (addMeshCollider) 
				{
					go.AddComponent<MeshCollider>();
				}
			}
		}	
	}

	//	void SaveMesh (Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh) 
	//	{
	//		string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/Art Assets/Meshes/", name, "asset");
	//		if (string.IsNullOrEmpty(path)) 
	//			return;
	//		
	//		path = FileUtil.GetProjectRelativePath(path);
	//		
	//		Mesh meshToSave = (makeNewInstance) ? Object.Instantiate(mesh) as Mesh : mesh;
	//		
	//		if (optimizeMesh)
	//			meshToSave.Optimize();
	//		
	//		AssetDatabase.CreateAsset(meshToSave, path);
	//		AssetDatabase.SaveAssets();
	//	}

	void SaveMeshAsAsset(GameObject currentGO)
	{
		#if UNITY_EDITOR
		var path = EditorUtility.SaveFilePanelInProject("Save mesh asset", "CombinedMesh", "asset", "Select save file path");
		if(!string.IsNullOrEmpty(path))
		{
			AssetDatabase.CreateAsset(currentGO.GetComponent<MeshFilter>().sharedMesh, path);
			AssetDatabase.Refresh();
			EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(path, typeof(Object)));
		}
		#endif
	}
}