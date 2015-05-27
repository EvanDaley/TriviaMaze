/* ISaveState.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: A simple saving interface. Anything that wants to save should generate an ID
 * at the start of the level. This id will be used in saving and loading this objects data. 
 * When the object wants to find its last saved state is communicates with the saving class
 * and asks for the save data associated with this object's id.
 */

using UnityEngine;
using System.Collections;

interface ISaveState {

	int UniqueID
	{
		get;
		set;
	}

	StateEnum State
	{
		get; 
		set;
	}

	/// <summary>
	/// When the scene loads this object should contact SaveManager and get a unique id
	/// </summary>
	void Start();
}
