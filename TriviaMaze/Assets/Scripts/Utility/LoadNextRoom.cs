/* LoadNextRoom.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: A simple script to load the next room after we query the DB.
 * (Press Ctrl Shift B to see the list of rooms)
 */

using UnityEngine;
using System.Collections;

public class LoadNextRoom : MonoBehaviour {

	// Use this for initialization
	void Start () {
		print("LoadNextRoom Start");
		Invoke ("CheckReady",.4f);
	}
	
	public void CheckReady()
	{
		if(DataCollector.Instance.Finished)
		{
			Application.LoadLevel(Application.loadedLevel + 1);
		}
		else
		{
			print ("Data collector isn't ready");
			Invoke ("CheckReady",.3f);
		}
	}
}
