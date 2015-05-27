/* MapEditor.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: A helper class to generate maps in the Unity Editor. This class is extended by 
 * MapEditorInspector so that it is displayed in the editor with buttons. 
 */

using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

[System.Serializable]
public class MapEditor : MonoBehaviour {

	[SerializeField]
	private int mapWidth = 50;
	[SerializeField]
	private int mapLength = 50;
	[SerializeField]
	private GameObject terminalGrouping;
	[SerializeField]
	public int roomWidth = 30;
	[SerializeField]
	public int roomLength = 30;

	[SerializeField]
	public Vector3 position;

	public int Rooms
	{
		get{ return (mapWidth / 30) * (mapLength / 30); }
	}

	[SerializeField]
	public int MapLength
	{
		get
		{
			return mapLength;
		}
		
		set
		{
			mapLength = (int)Math.Round ((double)value / 30)*30;
		}
	}

	[SerializeField]
	public int MapWidth
	{
		get
		{
			return mapWidth;
		}
		
		set
		{
			mapWidth = (int)Math.Round ((double)value / 30)*30;
		}
	}


	#region Building Functions

	public void BuildCubeAtZero()
	{
		GameObject test = GameObject.CreatePrimitive (PrimitiveType.Cube);
		test.transform.position = Vector3.zero;
		test.name = "Instantiated test cube";
		print ("Creating test cube");
		Undo.RegisterCreatedObjectUndo(test,"Undo Create Test Cube");
	}

	public void BuildCubeOnPlayer()
	{
		GameObject test = GameObject.CreatePrimitive (PrimitiveType.Cube);
		test.transform.position = Character.Instance.transform.position;
		test.name = "Instantiated test cube";
		print ("Creating test cube");
		Undo.RegisterCreatedObjectUndo (test,"Undo Build Cube");
	}

	public void BuildTerminalGrouping1()
	{
		Vector3 spawnPosition = Vector3.zero;
		if(MapEditorCache.Instance.spawnMarkerInstance)
			spawnPosition = MapEditorCache.Instance.spawnMarkerInstance.transform.position;
		GameObject terminal = (GameObject)GameObject.Instantiate (MapEditorCache.Instance.doorGrouping,spawnPosition, Quaternion.identity);
		print ("Creating terminal grouping: " + terminal.name + " at position " + terminal.transform.position);
		Undo.RegisterCreatedObjectUndo (terminal,"Undo Build Cube");
	}

	public void CreateSpawnMarker()
	{
		if(!MapEditorCache.Instance)
			return;

		MapEditorCache.Instance.spawnMarkerInstance = (GameObject)GameObject.Instantiate (MapEditorCache.Instance.spawnMarkerPrefab,Vector3.zero,Quaternion.identity);
		Undo.RegisterCreatedObjectUndo (MapEditorCache.Instance.spawnMarkerInstance,"Undo Build Cube");
	}

	//somehow parent it onto the room!
	public void BuildTurret()
	{
		
	}

	public GameObject BuildTurret(Transform parent)
	{
		return null;
	}

	public void CreateRoom()
	{
		print ("Creating room");
	}

	public void CreateMap()
	{
		GameObject map_GO = new GameObject("Map");
		map_GO.tag = "Map";
		Map map = map_GO.AddComponent<Map>();
		map_GO.transform.position = Vector3.zero;

		Undo.RegisterCreatedObjectUndo (map_GO, "Create object");

		map.rooms = new Room[MapLength,MapWidth];

		for(int i = 0; i < MapLength/30; i++)
		{
			for(int j = 0; j < MapWidth /30; j++)
			{
				int x = i*30;
				int z = j*30;

				GameObject room_GO = new GameObject("Room (" + x + ", 0, "+ z +")");
				room_GO.transform.position = new Vector3(x,0,z);
				room_GO.transform.parent = map_GO.transform;

				Room current = room_GO.AddComponent<Room>();
				map.rooms[i,j] = current;
			}
		}

		/*
		 * Now we have all the rooms stored in a 2 by 2 array
		 * 				x = length = i
		 *  z		[					]
		 *  width	[					]
		 *  j		[					]
		 */

		// All rooms have been created - tell them to initialize
		for(int i = 0; i < MapLength/30; i++)
		{
			for(int j = 0; j < MapWidth /30; j++)
			{
				Room left = null;
				Room right = null;
				Room forward = null;
				Room backward = null;

				if(i > 0)
					left = map.rooms[i-1,j];

				if(i<(MapLength/30-1))
					right = map.rooms[i+1,j];

				if(j > 0)
					backward = map.rooms[i,j-1];

				if(j<(MapWidth/30-1))
					forward = map.rooms[i,j+1];

				map.rooms[i,j].InitializeRoom(left,right,forward,backward);

				if(i == 0 && j == 0)
				{
					map.firstRoom = map.rooms[i,j];
				}

				if(i == (MapLength/30)-1 && j == (MapWidth/30)-1)
				{
					map.rooms[i,j].FinalRoom = true;
				}
			}
		}
	}

	#endregion
}
