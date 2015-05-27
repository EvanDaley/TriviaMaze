/* MusicManager.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: A simple music player. You can change the primary loop in the inspector.
 */

using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	public AudioClip loop1;
	public bool playOnStart;

	// Use this for initialization
	void Start () {
		if(playOnStart)	
			Invoke ("PlayClipOnPlayer",2f);
	}
	
	void PlayClipOnPlayer()
	{
		AudioSource.PlayClipAtPoint (loop1,Character.Instance.transform.position);
	}
}
