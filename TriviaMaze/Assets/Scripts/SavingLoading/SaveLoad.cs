/* SaveLoad.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: A static class that manages saving and loading games. 
 * To save the game call SaveLoad.Save(). 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad {

	public static List<SavedGame> savedGames = new List<SavedGame>();
	public static List<Transform> sceneObjects = new List<Transform>();

	public static int GetUniqueID(Transform savableT)
	{
		int uniqueID = 0;

		// This "nearly guarantees" us a unique ID for the door
		string IDString = Mathf.Abs (Mathf.FloorToInt(savableT.position.x)) + "" + Mathf.Abs (Mathf.FloorToInt (savableT.position.z)); 
		
		int.TryParse(IDString,out uniqueID);

		if(SavedGame.current != null)
		{
			// Check if we had a previous save. If so, load it.
			for(int i = 0; i < SavedGame.current.savedStates.Count; i++)
			{
				if(uniqueID == SavedGame.current.savedStates[i].uniqueID)
				{
					// We found a match! Now update the State property of the scene object
					ISaveState savedObject = savableT.GetComponent<ISaveState>();
					savedObject.State = SavedGame.current.savedStates[i].state;
				}
			}
		}

		sceneObjects.Add (savableT);
		
		//Debug.Log ("ID: " + uniqueID.ToString () + " at position: " + savableT.position + " from string: " + IDString);
		return uniqueID;
	}

	public static void UpdateLists()
	{
		if(File.Exists(Application.persistentDataPath + "/savedGames.maze")) {
			
			// Create a binary formatter and open target file
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/savedGames.maze", FileMode.Open);
			
			// Deserialize what is stored in the file (this gives us back our list of Games)
			SaveLoad.savedGames = (List<SavedGame>)bf.Deserialize(file);
			
			// Close the save file
			file.Close();
		}
		else
		{
			Debug.Log("Couldn't find save");
		}
	}

	// We have Game.cs objects stored in a generic list. We serialize the entire list to /savedGames.gd. Later we deserialize and get the generic list back. 
	public static void Save() {
		// Choose where we want to save
		string location = Application.persistentDataPath + "/savedGames.maze";

		Debug.Log("Location: " + location);

		// Load the previous save file so it isn't over-written
		SaveLoad.UpdateLists ();

		SavedGame.current = new SavedGame();

		// Collect the data we want to serialize
		SavedGame.current.time = System.DateTime.Now.ToString("hh:mm - MM/dd/yy");
		//SavedGame.current.playerPosition = Character.Instance.transform.position.ToString();

		SavedGame.current.savedStates = new List<SavedObjectState>();
		SavedGame.current.level = Application.loadedLevelName;
		SavedGame.current.levelIndex = Application.loadedLevel;

		for(int i = 0; i < sceneObjects.Count; i++)
		{
			ISaveState saveable = sceneObjects[i].GetComponent<ISaveState>();

			if(saveable != null)
				SavedGame.current.savedStates.Add(new SavedObjectState(saveable.UniqueID,saveable.State));
		}

		SaveLoad.savedGames.Add(SavedGame.current);

		// Create a binary formatter and target file
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (location); 

		// Now we serialize the entire generic list.
		bf.Serialize(file, SaveLoad.savedGames);

		file.Close();
		Debug.Log ("Finished saving");
	}	

	// Retrieve our stored generic list. This gives us an unsorted list of Game objects, each of which contain player location, timestamps, object states, etc.
	public static void Load(SavedGame game) {
		sceneObjects = new List<Transform>();
		UpdateLists ();

		// load level
		SavedGame.current = game;
		Application.LoadLevel (SavedGame.current.levelIndex - 1);

		// savable objects will request data when ready
	}
}
