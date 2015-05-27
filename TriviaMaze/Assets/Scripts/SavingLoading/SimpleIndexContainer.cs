/* SimpleIndexContainer.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: A basic class to hold the index of a UI button. We can't update the listener on 
 * a UI button dynamically so I'm using this as a workaround. We create buttons dynamically, based
 * on the number of saved games, and set the index at runtime.
 * 
 * Each saved game UI button get's an index. Index is used to load the correct game. 
 */

using UnityEngine;
using System.Collections;

public class SimpleIndexContainer : MonoBehaviour {

	public int index = 0;

}
