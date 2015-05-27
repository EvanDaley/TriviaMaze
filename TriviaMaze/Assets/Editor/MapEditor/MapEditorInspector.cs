using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.AnimatedValues;

[CustomEditor(typeof(MapEditor))]
public class MapEditorInspector : Editor {
	
	string m_string;
	Color m_Color = Color.white;

	AnimBool m_showRoomCreation;
	AnimBool m_showMapCreation;
	AnimBool m_showObjectCreation;

	//[MenuItem]("Window/My Map Editor")]
	//static void Init()
	//{
	//	ClassName window = (ClassName)EditorWindow.GetWindow (typeof(ClassName));
	//}

	void OnEnable()
	{
		m_showMapCreation = new AnimBool(true);
		m_showRoomCreation = new AnimBool(false);
		m_showObjectCreation = new AnimBool(false);

		m_showRoomCreation.valueChanged.AddListener (Repaint);
		m_showObjectCreation.valueChanged.AddListener (Repaint);
		m_showMapCreation.valueChanged.AddListener (Repaint);
	}

	public override void OnInspectorGUI()
	{
		MapEditor myTarget = (MapEditor)target;

		m_showObjectCreation.target = EditorGUILayout.ToggleLeft("Object Creation",m_showObjectCreation.target);

		if(EditorGUILayout.BeginFadeGroup(m_showObjectCreation.faded))
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PrefixLabel ("Color");
			m_Color = EditorGUILayout.ColorField (m_Color);
			EditorGUILayout.LabelField ("Create Room Items:");

			if(GUILayout.Button ("Instantiation Marker"))
			{
				myTarget.CreateSpawnMarker ();
			}

			if(GUILayout.Button ("Test Cube at (0,0,0)"))
			{
				myTarget.BuildCubeAtZero ();
			}

			if(GUILayout.Button ("Test Cube on player"))
			{
				myTarget.BuildCubeOnPlayer ();
			}

			if(GUILayout.Button ("Terminal Grouping"))
			{
				myTarget.BuildTerminalGrouping1 ();
			}
			EditorGUI.indentLevel--;
		}

		EditorGUILayout.EndFadeGroup ();
	//	EditorGUILayout.LabelField ("Create Room:");


		// CHANGES

		m_showRoomCreation.target = EditorGUILayout.ToggleLeft("Room Creation",m_showRoomCreation.target);

		if(EditorGUILayout.BeginFadeGroup(m_showRoomCreation.faded))
		{
			//EditorGUILayout.BeginVertical();
			//EditorGUILayout.BeginScrollView(Vector3.zero);
			EditorGUI.indentLevel++;

			myTarget.roomWidth = EditorGUILayout.IntField ("Room Width", myTarget.roomWidth);
			myTarget.roomLength = EditorGUILayout.IntField ("Room Length", myTarget.roomLength);
			
			myTarget.position = EditorGUILayout.Vector3Field("World Position",myTarget.position);

			if(GUILayout.Button ("Create Room"))
			{
				myTarget.CreateRoom ();
			}

			EditorGUI.indentLevel--;
		}

		//EditorGUILayout.EndScrollView ();
		//EditorGUILayout.EndVertical();

		EditorGUILayout.EndFadeGroup ();

		m_showMapCreation.target = EditorGUILayout.ToggleLeft ("Map Creation",m_showMapCreation.target);

		if(EditorGUILayout.BeginFadeGroup (m_showMapCreation.faded))
		{
			EditorGUI.indentLevel++;

			EditorGUILayout.LabelField ("Map Size:");
			myTarget.MapLength = EditorGUILayout.IntField ("X Length",myTarget.MapLength);
			myTarget.MapWidth = EditorGUILayout.IntField ("Z Length",myTarget.MapWidth);
			
			EditorGUILayout.LabelField ("Num of Rooms: ",myTarget.Rooms.ToString ());

			if(GUILayout.Button ("Create Map"))
			{
				myTarget.CreateMap();
			}

			EditorGUI.indentLevel--;
		}

		EditorGUILayout.EndFadeGroup ();

		if(GUI.changed)
			EditorUtility.SetDirty(myTarget);  
	}
}
