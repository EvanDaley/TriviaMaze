/* Game.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: Represents a single game to save. Holds a reference to the player,
 * player location, door states, and terminal states.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SavedGame { 

	public static SavedGame current;
	
	public string time;
	public string name;
	public string level;
	public int levelIndex;
	public string completion;
	public string location;
	public List <SavedObjectState> savedStates = new List<SavedObjectState>();

	public string playerPosition;

	public SavedGame () {
		time = "Time";
		completion = "87%";
		location = "Test Room";
	}
		
}
