/* Door.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: A powerable door object. Must be activated by terminal.
 */

using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour, IActivate, ISaveState {

	// An opional Audio Clip to play when the door opens
	public AudioClip audioClip;

	// Factor out
	private bool open = false;
	private bool locked = false;

	private StateEnum state;
	private int uniqueID;

	public StateEnum State
	{
		get{ return state; }
		set
		{ 
			state = value; 

			if(state == StateEnum.powered && open == false)
			{
				open = true;
				
				Animator doorAnimator = gameObject.GetComponent<Animator>();
				doorAnimator.SetBool ("Open", true);
				
				if(audioClip)
					AudioSource.PlayClipAtPoint(audioClip,transform.position);
			}

			if(state == StateEnum.locked)
			{
				locked = true;
			}
		}
	}

	public int UniqueID
	{
		get{ return uniqueID;}
		set{ uniqueID = value; }
	}

	public bool Open
	{
		get{ return open; }
		set{ open = value; }
	}

	public bool Locked
	{
		get{ return locked; }
		set{ locked = value; }
	}

	public void Start()
	{
		UniqueID = SaveLoad.GetUniqueID (this.transform);
	}

}
