#pragma warning disable 0618
using UnityEngine;
using System.Collections;
using UnityEditor;
[CanEditMultipleObjects]
[CustomEditor(typeof(ZippyLights2D))]
public class ZippyLights2DEditor : Editor
{
	SerializedProperty staticLight;
	SerializedProperty resolution;
	SerializedProperty degrees;
	SerializedProperty offset;
	SerializedProperty offsetSpherify;
	SerializedProperty moveToUpdate;
	SerializedProperty layers;
	SerializedProperty unityLight;
	SerializedProperty follow;
	SerializedProperty range;
	SerializedProperty animateRange;
	SerializedProperty rangeAnimation;
	SerializedProperty animateRangeSpeed;
	SerializedProperty animateRangeScale;
	SerializedProperty enableVertexColors;
	SerializedProperty vertexFade;
	SerializedProperty vertexColor;
	SerializedProperty enableOuterColor;
	SerializedProperty vertexColorOuter;
	SerializedProperty ColorCycleEnabled;
	SerializedProperty ColorCycle;
	SerializedProperty ColorCycleSpeed;
	SerializedProperty ColorCycleOuterEnabled;
	SerializedProperty ColorCycleOuter;
	SerializedProperty ColorCycleSpeedOuter;
	SerializedProperty CreateUV;
	SerializedProperty UVScale;
	SerializedProperty noise;
	SerializedProperty noiseDelay;
	SerializedProperty sortingOrder;
	SerializedProperty sortingLayer;
	SerializedProperty particles;
	SerializedProperty particleEmitDelay;
	SerializedProperty particleRayAmount;
	SerializedProperty particleEmitAmount;
	SerializedProperty particleRangeLimitMin;
	SerializedProperty particleRangeLimitMax;

	SerializedProperty falloffAfterglow;
	SerializedProperty falloff;
	SerializedProperty falloffMobileFix;

	SerializedProperty meshTransform;
	SerializedProperty duplicatedLights;

	public override void OnInspectorGUI() {
		//DrawDefaultInspector(); 
		DrawCustomInspector();
	}

	void OnEnable() {
		CreateTextures();
		Serialization();
	}

	void Serialization() {
		staticLight = serializedObject.FindProperty("staticLight");
		resolution = serializedObject.FindProperty("resolution");
		degrees = serializedObject.FindProperty("degrees");
		offset = serializedObject.FindProperty("offset");
		offsetSpherify = serializedObject.FindProperty("offsetSpherify");
		moveToUpdate = serializedObject.FindProperty("moveToUpdate");
		layers = serializedObject.FindProperty("layers");
		unityLight = serializedObject.FindProperty("unityLight");
		follow = serializedObject.FindProperty("follow");
		range = serializedObject.FindProperty("range");
		animateRange = serializedObject.FindProperty("animateRange");
		rangeAnimation = serializedObject.FindProperty("rangeAnimation");
		animateRangeSpeed = serializedObject.FindProperty("animateRangeSpeed");
		animateRangeScale = serializedObject.FindProperty("animateRangeScale");
		enableVertexColors = serializedObject.FindProperty("enableVertexColors");
		vertexFade = serializedObject.FindProperty("vertexFade");
		vertexColor = serializedObject.FindProperty("vertexColor");
		enableOuterColor = serializedObject.FindProperty("enableOuterColor");
		vertexColorOuter = serializedObject.FindProperty("vertexColorOuter");
		ColorCycleEnabled = serializedObject.FindProperty("ColorCycleEnabled");
		ColorCycle = serializedObject.FindProperty("ColorCycle");
		ColorCycleSpeed = serializedObject.FindProperty("ColorCycleSpeed");
		ColorCycleOuterEnabled = serializedObject.FindProperty("ColorCycleOuterEnabled");
		ColorCycleOuter = serializedObject.FindProperty("ColorCycleOuter");
		ColorCycleSpeedOuter = serializedObject.FindProperty("ColorCycleSpeedOuter");
		CreateUV = serializedObject.FindProperty("CreateUV");
		UVScale = serializedObject.FindProperty("UVScale");
		noise = serializedObject.FindProperty("noise");
		noiseDelay = serializedObject.FindProperty("noiseDelay");
		sortingOrder = serializedObject.FindProperty("sortingOrder");
		sortingLayer = serializedObject.FindProperty("sortingLayer");
		particles = serializedObject.FindProperty("particles");
		particleEmitDelay = serializedObject.FindProperty("particleEmitDelay");
		particleRayAmount = serializedObject.FindProperty("particleRayAmount");
		particleEmitAmount = serializedObject.FindProperty("particleEmitAmount");
		particleRangeLimitMin = serializedObject.FindProperty("particleRangeLimitMin");
		particleRangeLimitMax = serializedObject.FindProperty("particleRangeLimitMax");

		falloffAfterglow = serializedObject.FindProperty("falloffAfterglow");
		falloff = serializedObject.FindProperty("falloff");
		falloffMobileFix = serializedObject.FindProperty("falloffMobileFix");

		meshTransform = serializedObject.FindProperty("meshTransform");

		

		duplicatedLights = serializedObject.FindProperty("duplicatedLights");

	}

	static Texture2D buttonTexture;
	static Texture2D buttonTextureActive;
	static Texture2D texture;

	void CreateTextures() {
		if (texture == null) {
			texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
			texture.SetPixel(0, 0, Color.white);
			texture.Apply();
			texture.hideFlags = HideFlags.HideAndDontSave;
		}
		if (buttonTexture == null) {
			buttonTexture = new Texture2D(1, 1);
			buttonTexture.SetPixel(0, 0, Color.white);
			buttonTexture.Apply();
			buttonTexture.hideFlags = HideFlags.HideAndDontSave;
		}
		if (buttonTextureActive == null) {
			buttonTextureActive = new Texture2D(1, 1);
			buttonTextureActive.SetPixel(0, 0, new Color(.5f, .5f, .5f));
			buttonTextureActive.Apply();
			buttonTextureActive.hideFlags = HideFlags.HideAndDontSave;
		}
	}

	public void DrawCustomInspector() {
		serializedObject.Update();
		ZippyLights2D t = (ZippyLights2D)target;
		GUIBeginBox();
		if (GUILayout.Button(GUIButtoneText("Light", t.showLightSettings), EditorStyles.boldLabel)) {
			t.showLightSettings = !t.showLightSettings;
		}
		if (t.showLightSettings) {
			GUIBeginBox("", true, 2);
			EditorGUILayout.PropertyField(staticLight);
			EditorGUILayout.PropertyField(resolution);
			EditorGUILayout.PropertyField(degrees);
			EditorGUILayout.PropertyField(offset);
			EditorGUILayout.PropertyField(offsetSpherify);

			if (t.offset < 0 || t.offsetSpherify < 0) {
				EditorGUILayout.HelpBox("Negative offsets can cause bugs if light is too close to a collider.\nMake sure to create a buffer collider between the light and other colliders.", MessageType.Warning);
			}

			EditorGUILayout.PropertyField(moveToUpdate);
			EditorGUILayout.PropertyField(layers);
			EditorGUILayout.PropertyField(unityLight);
			EditorGUILayout.PropertyField(follow);
			EditorGUILayout.PropertyField(meshTransform);

			GUIEndBox();


		}
		GUIEndBox();


		GUIBeginBox();
		if (GUILayout.Button(GUIButtoneText("Duplicate Lights", t.showDuplicate), EditorStyles.boldLabel)) {
			t.showDuplicate = !t.showDuplicate;
		}
		if (t.showDuplicate) {
			GUIBeginBox("", true, 2);

			if (GUIButton("Create Dupe")) {
				GameObject go =  new GameObject("LightMesh Dupe " + t.duplicatedLights.Count, typeof(MeshRenderer), typeof(MeshFilter));
				Transform tr = go.transform;
				MeshRenderer parentMr = t.meshTransform.GetComponent<MeshRenderer>();
				MeshFilter mf = tr.GetComponent<MeshFilter>();
				MeshRenderer mr =  tr.GetComponent<MeshRenderer>();
				tr.parent = t.meshTransform;
				tr.localPosition = Vector3.zero;
				tr.localRotation = Quaternion.identity;
				mr.sharedMaterial = parentMr.sharedMaterial;
				mr.receiveShadows = false;
				mr.castShadows = false;
				EditorUtility.SetSelectedWireframeHidden(mr, true);
				t.duplicatedLights.Add(mf);
			}
			GUILayout.Space(2);
			if (t.duplicatedLights.Count == 0) GUI.enabled = false;
			if (GUIButton("Remove Dupe")) {

				DestroyImmediate(t.duplicatedLights[t.duplicatedLights.Count - 1].gameObject);
				t.duplicatedLights.RemoveAt(t.duplicatedLights.Count - 1);

			}
			if (t.duplicatedLights.Count == 0) GUI.enabled = true;

			GUILayout.Space(2);
			EditorGUILayout.HelpBox("Duplicated lights can be rotated, moved or scaled after creation to create special effects.\nChanging to a different material can also improve the look and feel of the lights.", MessageType.Info);



			EditorGUILayout.PropertyField(duplicatedLights, true);
			GUIEndBox();

		}
		GUIEndBox();


		GUIBeginBox();
		if (GUILayout.Button(GUIButtoneText("Range", t.showRange), EditorStyles.boldLabel)) {
			t.showRange = !t.showRange;
		}
		if (t.showRange) {
			GUIBeginBox("", true, 2);
			EditorGUILayout.PropertyField(range);
			EditorGUILayout.PropertyField(animateRange);
			EditorGUILayout.PropertyField(rangeAnimation);
			EditorGUILayout.PropertyField(animateRangeSpeed);
			EditorGUILayout.PropertyField(animateRangeScale);
			GUIEndBox();
		}
		GUIEndBox();

		GUIBeginBox();
		if (GUILayout.Button(GUIButtoneText("Falloff", t.showFalloff), EditorStyles.boldLabel)) {
			t.showFalloff = !t.showFalloff;
		}
		if (t.showFalloff) {
			GUIBeginBox("", true, 2);
			EditorGUILayout.HelpBox("Experimental, can have bugs in mobile or other builds.", MessageType.Warning);
			EditorGUILayout.PropertyField(falloff);
			EditorGUILayout.PropertyField(falloffAfterglow);
			EditorGUILayout.PropertyField(falloffMobileFix);
			if (GUIButton("Mobile Mode Toggle (Fix)")) {
				t.ForceUpdate();
					t.falloffMobileFix = !t.falloffMobileFix;
				}
			GUIEndBox();
		}
		GUIEndBox();

		GUIBeginBox();
		if (GUILayout.Button(GUIButtoneText("Color", t.showColor), EditorStyles.boldLabel)) {
			t.showColor = !t.showColor;
		}
		if (t.showColor) {
			GUIBeginBox("", true, 2);
			EditorGUILayout.PropertyField(enableVertexColors);
			EditorGUILayout.PropertyField(vertexFade);
			EditorGUILayout.PropertyField(vertexColor);
			EditorGUILayout.PropertyField(enableOuterColor);
			EditorGUILayout.PropertyField(vertexColorOuter);
			EditorGUILayout.PropertyField(ColorCycleEnabled);
			EditorGUILayout.PropertyField(ColorCycle);
			EditorGUILayout.PropertyField(ColorCycleSpeed);
			EditorGUILayout.PropertyField(ColorCycleOuterEnabled);
			EditorGUILayout.PropertyField(ColorCycleOuter);
			EditorGUILayout.PropertyField(ColorCycleSpeedOuter);
			GUIEndBox();
		}
		GUIEndBox();

		GUIBeginBox();
		if (GUILayout.Button(GUIButtoneText("UV", t.showUV), EditorStyles.boldLabel)) {
			t.showUV = !t.showUV;
		}
		if (t.showUV) {
			GUIBeginBox("", true, 2);
			EditorGUILayout.PropertyField(CreateUV);
			EditorGUILayout.PropertyField(UVScale);
			GUIEndBox();
		}
		GUIEndBox();

		GUIBeginBox();
		if (GUILayout.Button(GUIButtoneText("Noise", t.showNoise), EditorStyles.boldLabel)) {
			t.showNoise = !t.showNoise;
		}
		if (t.showNoise) {
			GUIBeginBox("", true, 2);
			EditorGUILayout.PropertyField(noise);
			EditorGUILayout.PropertyField(noiseDelay);
			GUIEndBox();
		}
		GUIEndBox();

		GUIBeginBox();
		if (GUILayout.Button(GUIButtoneText("Sort", t.showSort), EditorStyles.boldLabel)) {
			t.showSort = !t.showSort;
		}
		if (t.showSort) {
			GUIBeginBox("", true, 2);
			EditorGUILayout.PropertyField(sortingOrder);
			EditorGUILayout.PropertyField(sortingLayer);
			GUIEndBox();
		}
		GUIEndBox();

		GUIBeginBox();
		if (GUILayout.Button(GUIButtoneText("Particles", t.showParticle), EditorStyles.boldLabel)) {
			t.showParticle = !t.showParticle;
		}
		if (t.showParticle) {
			GUIBeginBox("", true, 2);
			EditorGUILayout.PropertyField(particles);
			EditorGUILayout.PropertyField(particleEmitDelay);
			EditorGUILayout.PropertyField(particleRayAmount);
			EditorGUILayout.PropertyField(particleEmitAmount);
			EditorGUILayout.PropertyField(particleRangeLimitMin);
			EditorGUILayout.PropertyField(particleRangeLimitMax);
			GUIEndBox();
		}
		GUIEndBox();

		GUIBeginBox();
		if (GUILayout.Button(GUIButtoneText("Functions", t.showFunctions), EditorStyles.boldLabel)) {
			t.showFunctions = !t.showFunctions;
		}
		if (t.showFunctions) {
			GUIBeginBox("", true, 2);
			if (!Application.isPlaying) {
				if (GUIButton("Update")) {
					t.ForceUpdate();
					//Debug.Log(t.GetComponent<MeshRenderer>().sortingLayerName);
					//Debug.Log(t.GetComponent<MeshRenderer>().sortingOrder);
				}
				GUILayout.Space(2);
				if (GUIButton("Force New Mesh")) {
					t.ForceNewMesh();
				}
				GUILayout.Space(2);
				if (GUIButton("Create Material Instances\n(ignore error)")) {
					MeshRenderer r = t.GetComponent<MeshRenderer>();
					for (int i = 0; i < r.materials.Length; i++) {
						r.materials[i] = new Material(r.materials[i]);
					}
				}
			}
			EditorGUILayout.HelpBox("Tip : Use [Unity Menu > GameObject > Break Prefab Instance] to improve performance in editor.\nApply changes to connect scene object to prefab again.\nTip2 : Improve inspector lag by copying Zippy Light component, removing it, then paste it back as new.", MessageType.Info);
			GUIEndBox();
		}
		GUIEndBox();
		CheckIdle(t);
		CheckUnityLight(t);
		CheckResolution(t);
		CheckTexture(t);
		

		if (GUI.changed) {
			EditorUtility.SetDirty(t);
			serializedObject.ApplyModifiedProperties();
		}

	}


	//GUI Funtions
	string GUIButtoneText(string s, bool b) {
		if (b) return "= " + s;
		return "+ " + s;
	}

	static void GUIBeginBox(string label = "", bool white = false, int s = 0) {
		if (white) {
			if (EditorGUIUtility.isProSkin)
				GUI.color = new Color(0f, 0f, 0f);
			else
				GUI.color = new Color(0.8f, 0.8f, 0.8f);
		} else {
			GUI.color = new Color(.85f, .85f, .85f);
		}
		GUIStyle b = new GUIStyle("Box");
		if (!EditorGUIUtility.isProSkin)
			b.normal.background = buttonTexture;
		EditorGUILayout.BeginVertical(b);
		GUI.color = Color.white;
		if (label != "") EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
		if (s > 0) GUILayout.Space(s);
	}

	static bool GUIButton(string label = "", bool white = false, float width = -1f) {
		bool r = false;
		GUIBeginBox("", white);
		GUIStyle b = new GUIStyle(EditorStyles.miniButton);
		b.alignment = TextAnchor.MiddleCenter;
		b.normal.background = null;
		b.active.background = buttonTextureActive;
		if (width > 1) {
			if (GUILayout.Button(label, b, GUILayout.Width(width))) r = true;
		} else {
			if (GUILayout.Button(label, b)) r = true;
		}
		EditorGUILayout.EndVertical();
		return r;
	}

	static void GUIEndBox() {
		EditorGUILayout.EndVertical();
	}

	void CheckIdle(ZippyLights2D t) {
		if (!Application.isPlaying) return;


		if (!t.lightEnabled) {
			EditorGUILayout.HelpBox("ZippyLight2D not rendered: \nDisabled and using very few resources.", MessageType.Info);

		} else {
			if (!t.idle)
				GUI.color = Color.clear;
				EditorGUILayout.HelpBox("ZippyLight2D is idle: \nUsing less resources.", MessageType.Info);
		}


		






		
		GUI.color = Color.white;
	}

	void CheckResolution(ZippyLights2D t) {
		if (t.resolution <= 360) return;
		EditorGUILayout.HelpBox("ZippyLight2D resolution set high: \nResolution not suitable for mobile.", MessageType.Warning);
	}

	void CheckUnityLight(ZippyLights2D t) {
		if (t.unityLight && t.unityLight.renderMode != LightRenderMode.ForceVertex) {
			EditorGUILayout.HelpBox("RenderMode slow on mobile. \nSet to light RenderMode to [Not Important] for mobile.", MessageType.Warning);
		}
	}

	void CheckTexture(ZippyLights2D t) {
		if (!t.CreateUV) return;
		MeshRenderer r = t.GetComponent<MeshRenderer>();
		if (r == null) return;
		Material m = r.sharedMaterial;
		if (m == null || m != null && m.GetTexture("_MainTex") == null) {
			EditorGUILayout.HelpBox("UV enabled but no Texture in Material.", MessageType.Error);
		}
	}
}