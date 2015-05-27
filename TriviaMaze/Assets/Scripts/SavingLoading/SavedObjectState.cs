/* SavedObjectState.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: A class to encapsulate the state of an object.
 */

using UnityEngine;
using System.Collections;

[System.Serializable]
public class SavedObjectState 
{
	public int uniqueID;
	public StateEnum state;

	public SavedObjectState(int uniqueID, StateEnum state)
	{
		this.state = state;
		this.uniqueID = uniqueID;
	}
}
