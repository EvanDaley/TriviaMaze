/* Map.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: A map class that holds references to rooms. When the map is created
 * and rooms are instantiated the rooms[,] list is used to mediate room connections.
 */

using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

	public Room firstRoom;

	public Room [,] rooms;
	//public Room [][];

}
