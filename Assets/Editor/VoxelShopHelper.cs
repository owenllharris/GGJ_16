using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

//A note to anybody reading this:
//I'm just some guy - unaffiliated with VoxelShop
//Which you can get right here: http://blackflux.com/node/11
//I put these functions together on the off chance that you find it helpful
//Use as you please, and if something bad happens as a result of it...
//Standard disclaimers.
using System.Collections.Generic;

public class VoxelShopHelper : MonoBehaviour {
	
	//Change these if you like
	//This is where the script will place the resulting assets
	
	//Vertex-colored models will go here
	const string MODEL_FOLDER_NAME = "Processed Models";
	
	//The models you originally imported will go here
	const string RAW_MODEL_FOLDER_NAME = "Raw Models";
	
	//The meshes that the tool creates for the processed models will go here
	const string MESH_FOLDER_NAME = "Processed Meshes";
	
	//The textures you originally imported will go here
	const string TEXTURE_FOLDER_NAME = "Processed Textures";
	
#region Directory Helpers
	
	//Don't change this
	const string ASSETS_PREFIX = "Assets/";
	const string VERTEX_COLORED_MATERIAL_NAME = "Vertex_Colored_Material";
	const string DIFFUSE_MATERIAL_NAME = "Voxel_Atlas_Material";
		
	//These are just helper functions. Lots of directory business going on later
	static string MODEL_FOLDER{
		get{
			return ASSETS_PREFIX + MODEL_FOLDER_NAME+"/";
		}
	}
	
	static string RAW_MODEL_FOLDER{
		get{
			return ASSETS_PREFIX + RAW_MODEL_FOLDER_NAME+"/";
		}
	}
	
	static string MESH_FOLDER{
		get{
			return ASSETS_PREFIX + MESH_FOLDER_NAME+"/";
		}
	}
	
	static string TEXTURE_FOLDER{
		get{
			return ASSETS_PREFIX + TEXTURE_FOLDER_NAME+"/";
		}
	}
		
	static string _EDITOR_PATH = null;
	
	static string EDITOR_PATH{
		get{
		
			if(string.IsNullOrEmpty(_EDITOR_PATH)){
			
				string rawDataPath = Application.dataPath;
				
				rawDataPath = rawDataPath.Replace("\\","/");
				
				int indexOf = rawDataPath.LastIndexOf("/");
				
				_EDITOR_PATH = rawDataPath.Substring(0,rawDataPath.Length-(rawDataPath.Length-indexOf));
				
			}
			
			return _EDITOR_PATH;
		
		}
	}
	
	const string ATLAS_NAME = "VoxelShop_Atlas";
	const string ATLAS_EXTENSION = ".png";
	
	static string ATLAS_FULL_NAME{
		get{
			return ATLAS_NAME + ATLAS_EXTENSION;
		}
	}
	
	static string ATLAS_PATH{
		get{
		
			return EDITOR_PATH + "/" + TEXTURE_FOLDER + "/" + ATLAS_FULL_NAME;
		
		}
	}
	
#endregion

#region Error Helpers

	const string ERROR_HELP_PREFIX = "Select a model and a texture from your Project View\n(Not scene view!)\nAnd try again.\n\n";

#endregion

	static void ValidateDirectoryStructure(){
	
		if(!Directory.Exists(EDITOR_PATH + "/" + MODEL_FOLDER)){
			AssetDatabase.CreateFolder("Assets",MODEL_FOLDER_NAME);
		}
		
		if(!Directory.Exists(EDITOR_PATH + "/" + RAW_MODEL_FOLDER)){
			AssetDatabase.CreateFolder("Assets",RAW_MODEL_FOLDER_NAME);
		}
		
		if(!Directory.Exists(EDITOR_PATH + "/" + MESH_FOLDER)){
			AssetDatabase.CreateFolder("Assets",MESH_FOLDER_NAME);
		}
		
		if(!Directory.Exists(EDITOR_PATH + "/" + TEXTURE_FOLDER)){
			AssetDatabase.CreateFolder("Assets",TEXTURE_FOLDER_NAME);
		}
			
	}

#region Menu Functions

	[MenuItem ("VoxelShop/Convert to Vertex Coloring")]
	static void SetMeshColors () {
	
		ProcessModels(ProcessModelColors);
	
	}
	
	[MenuItem ("VoxelShop/Atlas all Models")]
	static void AtlasModels(){
		
		ProcessModels(ProcessModelUVs);
		
	}
		
	[MenuItem ("VoxelShop/Apply Ideal Texture Settings")]
	static void SetTextureImportSettings () {
		ProcessTextureImportSettings();
	}
		
#endregion

#region Delegates

	
	delegate void ProcessModelToTextureCollection(
		Dictionary<GameObject,Texture2D> modelsToTextures,
		GameObject[] modelPrefabs,
		Texture2D[] textures);
	

#endregion

#region Model Processing

	static void ProcessModels (ProcessModelToTextureCollection processDelegate) {
		
		//Check our directories and all of that business
		ValidateDirectoryStructure();
		
		//Fix all of the Textures in our selection
		ProcessTextureImportSettings();
		
		//Fix all of the Models in our selection
		ProcessModelImportSettings();
		
		//Correlate all models to their textures from the selection
		Dictionary<GameObject,Texture2D> modelsToTextures = GetModelTexturePairsFromSelection();
		
		//We'll need these for archiving later
		Texture2D[] textures = new Texture2D[modelsToTextures.Count];
		modelsToTextures.Values.CopyTo(textures,0);
		
		GameObject[] modelPrefabs = new GameObject[modelsToTextures.Count];
		modelsToTextures.Keys.CopyTo(modelPrefabs,0);
		
		//Start delegate here
		processDelegate(modelsToTextures,modelPrefabs,textures);
		
		for(int i = 0; i < textures.Length; i++){
			
			//Move the texture to the archive
			ArchiveTexture(textures[i]);
			
		}
		
		for(int i = 0; i < modelPrefabs.Length; i++){
			
			//Move the texture to the archive
			ArchiveModelPrefab(modelPrefabs[i]);
			
		}
		
		AssetDatabase.SaveAssets();
		
	}
	
	static void ProcessModelUVs(
		Dictionary<GameObject,Texture2D> modelsToTextures,
		GameObject[] modelPrefabs,
		Texture2D[] textures){
	
		//Atlas all of the textures and correlate models with their new texture square
		Texture2D atlas = null;
		
		Dictionary<GameObject,Rect> modelToUVRect = AtlasTextures(modelsToTextures, out atlas);
		
		//Save the texture
		SaveAtlasTexture(atlas);
		
		Editor.DestroyImmediate(atlas);
		
		//Fetch the material if it exists and apply the texture
		Material material = FindMaterialForName(DIFFUSE_MATERIAL_NAME);
		
		if(material != null){
			
			string atlasPath = TEXTURE_FOLDER + ATLAS_FULL_NAME;
			
			Texture2D savedAtlas = AssetDatabase.LoadAssetAtPath(atlasPath,typeof(Texture2D)) as Texture2D;
			
			if(savedAtlas != null){
				material.mainTexture = savedAtlas;
			}
			else{
				Debug.LogWarning("Couldn't load saved atlas for some reason. Don't worry. It just means we won't automatically apply the material.");
			}
						
		}
		
		//Duplicate and modify all of the models
		foreach(GameObject o in modelToUVRect.Keys){
			
			//Create a new object for all of the models
			GameObject newObject = ProcessMeshHost(o);
			
			MeshFilter mf = newObject.GetComponent<MeshFilter>();
			
			//Modify the UVs on all of the meshes
			mf.sharedMesh = ModifyMeshUVs(newObject,modelToUVRect[o],material);
			
			//Apply the material if we have it
			MeshRenderer mr = newObject.GetComponent<MeshRenderer>();
			
			if(material != null){
				mr.material = material;
			}
			
			//And now complete the process
			SaveMeshHostAsPrefab(newObject);
			
		}
	
	}

	
	static void ProcessModelColors(
		Dictionary<GameObject,Texture2D> modelsToTextures,
		GameObject[] modelPrefabs,
		Texture2D[] textures){
		
		Material material = FindMaterialForName (VERTEX_COLORED_MATERIAL_NAME);
		
		foreach(GameObject o in modelsToTextures.Keys){
			
			//Create a new object for all of the models
			GameObject newObject = ProcessMeshHost(o);
			
			MeshFilter mf = newObject.GetComponent<MeshFilter>();
			
			//Modify the UVs on all of the meshes
			ModifyMeshColors(mf.sharedMesh,modelsToTextures[o]);
			
			//Apply the material if we have it
			MeshRenderer mr = newObject.GetComponent<MeshRenderer>();
			
			if(material != null){
				mr.material = material;
			}
			
			//And now complete the process
			SaveMeshHostAsPrefab(newObject);
			
		}		
		
	}

#endregion

#region Mesh Modification Logic

	static Mesh ModifyMeshUVs (GameObject o, Rect rect, Material material)
	{
		
		Mesh m = null;
		
		MeshFilter mf = o.GetComponent<MeshFilter>();
		
		m = mf.sharedMesh;
		
		Vector2[] uvs = m.uv;
		Vector2[] newUvs = new Vector2[uvs.Length];
		
		for(int i = 0; i < uvs.Length; i++){
			
			float x = uvs[i].x * rect.width + rect.x;
			float y = uvs[i].y * rect.height + rect.y;
			
			Vector2 newUV = new Vector2(x,y);
			
			newUvs[i] = newUV;
			
		}
		
		m.uv = newUvs;
		
		return m;
		
	}
	
	static void ModifyMeshColors (Mesh targetMesh, Texture2D sourceTexture)
	{
		Vector2[] uvs = targetMesh.uv;
		Color[] colors = new Color[uvs.Length];
		
		for(int i = 0; i < uvs.Length; i++){
			
			colors[i] = GetColorAtNormalized(sourceTexture,uvs[i].x,uvs[i].y);
			
		}
		
		targetMesh.colors = colors;
	}
	
	static Dictionary<GameObject, Rect> AtlasTextures (Dictionary<GameObject, Texture2D> modelsToTextures, out Texture2D atlas)
	{
		Dictionary<GameObject, Rect> uvRects = new Dictionary<GameObject, Rect>();
		
		//Preserving the dictionary order so that we can iterate and add next
		List<Texture2D> textures = new List<Texture2D>();
		
		foreach(GameObject o in modelsToTextures.Keys){
			textures.Add(modelsToTextures[o]);
		}
		
		Texture2D atlasTexture = new Texture2D(2,2,TextureFormat.RGBA32,false);
		
		atlasTexture.name = ATLAS_NAME;
		
		Rect[] rects = atlasTexture.PackTextures(textures.ToArray(),2,4096,false);
		
		int index = 0;
		
		foreach(GameObject o in modelsToTextures.Keys){
			
			Rect r = rects[index];
			
			index += 1;
			
			uvRects.Add(o,r);
		}
		
		atlas = atlasTexture;
		
		return uvRects;
	}
	
#endregion
	
	//Take a mesh prefab and turn it into a prefab we can work with
	static GameObject ProcessMeshHost (GameObject o)
	{
		
		//Now we'll create the new object that we'll be using
		GameObject host = Editor.Instantiate(o) as GameObject;
		MeshFilter targetMeshHost = host.GetComponent<MeshFilter>();
		
		//And we'll make a copy of our mesh
		Mesh targetMesh = Instantiate(targetMeshHost.sharedMesh) as Mesh;
		targetMeshHost.sharedMesh = targetMesh;
		targetMesh.name = targetMesh.name.Replace("(Clone)","_Modified");
		
		//And clean up the "Clone" tag that Unity adds
		host.name = host.name.Replace("(Clone)","");
		
		return host;
		
	}
	
	//Save that prefab to the Project
	static void SaveMeshHostAsPrefab(GameObject host){
		
		Mesh targetMesh = null;
		
		MeshFilter mf = host.GetComponent<MeshFilter>();
		
		targetMesh = mf.sharedMesh;
		
		AssetDatabase.CreateAsset(targetMesh,MESH_FOLDER + targetMesh.name);
		
		//And create a prefab that we can operate on
		PrefabUtility.CreatePrefab(MODEL_FOLDER + host.name + ".prefab",host,ReplacePrefabOptions.ReplaceNameBased);
		
		//And then destroy the instance we made
		Editor.DestroyImmediate(host);
		
	}

#region Import Settings Functions
	
	static void ProcessModelImportSettings(){
		
		//Looking through the user's selections...
		for(int i = 0; i < Selection.objects.Length; i++){
			
			UnityEngine.Object o = Selection.objects[i];
			
			string path = AssetDatabase.GetAssetPath(o);
			
			ModelImporter importer = ModelImporter.GetAtPath (path) as ModelImporter;
			
			if(importer != null){
			
				FixModelImporter(path);
			
			}
			
		}
		
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}
	
	static void FixModelImporter(string path){
	
		ModelImporter importer = ModelImporter.GetAtPath (path) as ModelImporter;
		
		importer.animationType = ModelImporterAnimationType.None;
		importer.importAnimation = false;
		
		importer.importBlendShapes = false;
		importer.importMaterials = false;
		
		AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate );
		
	}
	
	static void ProcessTextureImportSettings(){
	
		//Looking through the user's selections...
		for(int i = 0; i < Selection.objects.Length; i++){
			
			UnityEngine.Object o = Selection.objects[i];
			
			//Is it a texture? Then let's fix it!
			if(o is Texture2D){
				
				Texture2D texture = o as Texture2D;
				
				string path = AssetDatabase.GetAssetPath(texture);
				
				FixTextureImporter(path);
				
			}
			
		}
		
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	
	}
	
	static void FixTextureImporter(string path){
		
		TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
		
		if(importer == null){
			Debug.LogWarning("No importer at " + path);
		}
		
		importer.npotScale = TextureImporterNPOTScale.None;
		importer.mipmapEnabled = false;
		importer.filterMode = FilterMode.Point;	
		importer.isReadable = true;
		importer.wrapMode = TextureWrapMode.Clamp;
		importer.textureFormat = TextureImporterFormat.RGB24;
		
		AssetDatabase.ImportAsset( path, ImportAssetOptions.ForceUpdate );
	
	}

#endregion

	static Material FindMaterialForName(string name){
	
		string[] GUIDS = AssetDatabase.FindAssets(name);
		
		Material material = null;
		
		for(int i = 0; i < GUIDS.Length; i++){
			string gPath = AssetDatabase.GUIDToAssetPath(GUIDS[i]);
			
			Material m = AssetDatabase.LoadAssetAtPath(gPath,typeof(Material)) as Material;
			
			if(m != null){
				material = m;
				break;
			}
		}
		
		return material;
	
	}
	
#region Selection Handling
	
	static Dictionary<GameObject,Texture2D> GetModelTexturePairsFromSelection ()
	{
		
		Dictionary<GameObject,Texture2D> modelToTexture = new Dictionary<GameObject, Texture2D>();
		
		List<GameObject> models = new List<GameObject>();
		List<Texture2D> textures = new List<Texture2D>();
		List<string> textureNames = new List<string>();
		
		for(int i = 0; i < Selection.objects.Length; i++){
			
			UnityEngine.Object o = Selection.objects[i];
			
			if(o is Texture2D){
				textures.Add(o as Texture2D);
				
				//Pre process the name for later
				
				string name = textures[textures.Count-1].name.Split('_')[0];
				
				textureNames.Add(name);
				
			}
			else if(o is GameObject && IsModelPrefab(o as GameObject)){
				
				models.Add(o as GameObject);
			}
			
		}
		
		//Now we need to link them together
		for(int j = 0; j < models.Count; j++){
			
			GameObject o = models[j];
			
			string name = o.name;
			
			bool found = false;
			
			//Now we find the closest texture name. Exploiting the naming conventions from VoxelShop
			for(int i = 0; i < textureNames.Count; i++){
				
				if(textureNames[i] == name){
					
					found = true;
					modelToTexture.Add(o,textures[i]);
					break;
					
				}
				
			}
			
			if(!found){
				Debug.LogWarning("No texture found to match model: " + name);
			}
			
		}
		
		return modelToTexture;
		
	}
	
#endregion
	
#region Prefab Handling
	
	static bool IsModelPrefab(GameObject o){
		
		MeshFilter[] mfs = o.GetComponentsInChildren<MeshFilter>(true);
		
		if(mfs.Length > 0){
			
			return true;
			
		}
		
		return false;
		
	}
	
	static Mesh GetMeshFromModelPrefab(GameObject o){
		
		return GetMeshFilterFromModelPrefab(o).sharedMesh;
		
	}
	
	static MeshRenderer GetMeshRendererFromModelPrefab(GameObject o){
		
		MeshRenderer[] mrs = o.GetComponentsInChildren<MeshRenderer>(true);
		
		if(mrs.Length > 0){
			
			return mrs[0];
			
		}
		
		return null;
		
	}
	
	static MeshFilter GetMeshFilterFromModelPrefab(GameObject o){
		
		MeshFilter[] mfs = o.GetComponentsInChildren<MeshFilter>(true);
		
		if(mfs.Length > 0){
			
			return mfs[0];
			
		}
		
		return null;
		
	}
	
#endregion
	
#region Archive Functions
	
	static void ArchiveTexture(Texture2D texture){
		
		string textureObjectName = "";
		
		string textPath = AssetDatabase.GetAssetPath(texture);
		
		int textDot = textPath.LastIndexOf(".");
		
		string textExtension = textPath.Substring(textDot,textPath.Length-textDot);
		
		textureObjectName = texture.name + textExtension;
		
		AssetDatabase.MoveAsset(textPath,TEXTURE_FOLDER+textureObjectName);
		
	}
	
	static void ArchiveModelPrefab (GameObject modelPrefab)
	{
		
		string meshHostObjectName = "";
		
		string hostPath = AssetDatabase.GetAssetPath(modelPrefab);
		
		int lastDot = hostPath.LastIndexOf(".");
		
		string extension = hostPath.Substring(lastDot,hostPath.Length-lastDot);
		
		meshHostObjectName = modelPrefab.name + extension;
		
		AssetDatabase.MoveAsset(hostPath,RAW_MODEL_FOLDER+meshHostObjectName);
		
	}
	
#endregion

	//Saves the texture atlas
	static void SaveAtlasTexture (Texture2D atlas)
	{
		
		if(!System.IO.File.Exists(ATLAS_PATH)){
			FileStream s = System.IO.File.Create(ATLAS_PATH);
			s.Close();
		}
		
		byte[] pngData = atlas.EncodeToPNG();
		
		System.IO.File.WriteAllBytes(ATLAS_PATH,pngData);
		
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		
		string assetPath = ASSETS_PREFIX + TEXTURE_FOLDER_NAME + "/" + ATLAS_FULL_NAME;
		
		FixTextureImporter(assetPath);
		
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		
	}
	
	//Gets the color of a texture given the UV coordinates
	static Color GetColorAtNormalized(Texture2D texture, float x, float y){
	
		float width = (float)texture.width;
		float height = (float)texture.height;
	
		x = x * width;
		y = y * height;
		
		return texture.GetPixel((int)x,(int)y);
	
	}

}


/*static void ProcessMesh() {
		
		//Step One: Let's make a few directories (if we need to) so our project is nice and clean
		ValidateDirectoryStructure();
		
		//Step Two: We need two things - a model file and a texture. 
		//Let's check that the user has selected these
		Texture2D sourceTexture = null;
		Mesh targetMesh = null;
		MeshFilter targetMeshHost = null;
		GameObject meshHostObject = null;
		
		//Looking through the user's selections...
		for(int i = 0; i < Selection.objects.Length; i++){
		
			UnityEngine.Object o = Selection.objects[i];
			
			//Is it a texture? Then it's the texture we're after
			if(o is Texture2D){
				sourceTexture = o as Texture2D;
			}
			else if(o is GameObject){
			
				//The model is a little harder - since it's technically not a thing we can use
				//We need to search for the meshfilter inside it
				GameObject go = o as GameObject;
			
				//We use GetComponents (plural) and the true argument since everything is technically
				//disabled / inactive in project view
				MeshFilter[] m = go.GetComponentsInChildren<MeshFilter>(true);
			
				//If there's a meshfilter there, we'll take it
				if(m.Length > 0){
			
					targetMesh = m[0].sharedMesh;
					targetMeshHost = m[0];
					meshHostObject = go;
				
				}
			}
		
		}
		
		string errorMsg = "";
		bool failed = false;
		
		if(targetMesh == null){
			
			errorMsg += ("No source mesh found in selection\n");
			failed = true;
			
		}
		
		if(PrefabUtility.GetPrefabType(meshHostObject) != PrefabType.ModelPrefab){
		
			errorMsg += ("Source mesh must be from your project (not your scene)\n");
			failed = true;
		
		}
		
		if(sourceTexture == null){
		
			errorMsg += ("No source texture found in selection\n");
			failed = true;
			
		}
		
		//If this didn't work, show a helpful warning
		if(failed){
			EditorUtility.DisplayDialog("Vertex Coloring Failed.",ERROR_HELP_PREFIX+errorMsg,"Ok");
			return;
		}
		
		//And now we're going to do some automated cleaning of our models and textures
		//Fixing up import settings and such
		
		string path = AssetDatabase.GetAssetPath(sourceTexture);
		
		string hPath = AssetDatabase.GetAssetPath(meshHostObject);
		
		//NOTE: This is an important one
		//I'm stripping animations from the imports here. You might not want that, I'm not sure
		//Comment the follow two lines out if you want to keep them
		
		FixModelImporter(hPath);
		
		//And now we'll set all of our texture import settings to be nice and etc etc
		FixTextureImporter(path);
		
		//Now we'll create the new object that we'll be using
		GameObject host = Editor.Instantiate(meshHostObject) as GameObject;
		targetMeshHost = host.GetComponent<MeshFilter>();
		
		//And we'll make a copy of our mesh
		targetMesh = Instantiate(targetMeshHost.sharedMesh) as Mesh;
		targetMeshHost.mesh = targetMesh;
		
		//And clean up the "Clone" tag that Unity adds
		host.name = host.name.Replace("(Clone)","");
		targetMesh.name = targetMesh.name.Replace("(Clone)","");
		
		//The magic!
		//For every UV in the mesh, sample that position on the texture and set the vertex color to that
		
		Vector2[] uvs = targetMesh.uv;
		Color[] colors = new Color[uvs.Length];
		
		for(int i = 0; i < uvs.Length; i++){
		
			colors[i] = GetColorAtNormalized(sourceTexture,uvs[i].x,uvs[i].y);
		
		}
		
		targetMesh.colors = colors;

		//Create the mesh asset (it'll come up as an unknown file in Unity
		//because it's not a DAE / FBX or such. Don't worry about it.																					
		AssetDatabase.CreateAsset(targetMesh,MESH_FOLDER + targetMesh.name);
		
		//And now! Let's put the Vertex Colored Material on it so it's nice
		//If there's none, it doesn't matter
		string[] GUIDS = AssetDatabase.FindAssets(VERTEX_COLORED_MATERIAL_NAME);
		
		Material vColoredMaterial = null;
		
		for(int i = 0; i < GUIDS.Length; i++){
			string gPath = AssetDatabase.GUIDToAssetPath(GUIDS[i]);
			
			Material m = AssetDatabase.LoadAssetAtPath(gPath,typeof(Material)) as Material;
			
			if(m != null){
				vColoredMaterial = m;
				break;
			}
		}
		
		if(vColoredMaterial != null){
			MeshRenderer mr = host.GetComponent<MeshRenderer>();
			mr.sharedMaterial = vColoredMaterial;
		}
		
		//Okay, finally (phew)
		//Create the prefab
		
		PrefabUtility.CreatePrefab(MODEL_FOLDER + host.name + ".prefab",host,ReplacePrefabOptions.ReplaceNameBased);
		
		//Move the original file to the raw processing folder
		
		ArchiveModelPrefab(meshHostObject);
			
		//Move the texture to the texture processing folder
		
		ArchiveTexture(sourceTexture);
						
		//Save the changes
														
		AssetDatabase.SaveAssets();
		
		//And kill the temporary object in the scene
		
		Editor.DestroyImmediate(host.gameObject);
		
		//Done!
		
	}*/

