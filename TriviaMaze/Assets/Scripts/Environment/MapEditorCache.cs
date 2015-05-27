/* MapEditorCache.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: A singleton that holds prefabricated object references for the map editor. 
 * We can assign different prefabs to this object and then the map editor will use those 
 * prefabs when it generates a new map.
 */

using UnityEngine;
using System.Collections;

public class MapEditorCache : Singleton<MapEditorCache> {

	public GameObject spawnMarkerPrefab;
	public GameObject spawnMarkerInstance;

	public GameObject wallPrefab1;
	public GameObject cubePrefab1;
	public GameObject mountedTurret;

	public GameObject floor;
	public GameObject fullSidewall;
	public GameObject fullBackWall;

	public GameObject doorGrouping;

	public void PrintCache()
	{
		print (floor);
	}
}
