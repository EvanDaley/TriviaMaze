/* GameManeger.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: A singleton that monitors gameplay. This class 
 * is notified when the player sets off the alarm, or when the player
 * dies. The turrets check in here to see if they should 
 * attack the player.
 */

using UnityEngine;
using System.Collections;

public class AlarmManager : Singleton<AlarmManager> {

	// Control variables
	private bool gameOver = false;
	
	public bool alarmActive = false;
	private float alarmTime = 0;
	public float alarmDuration = 3000f;

	private Map map;

	public AudioClip alarmClip;

	void Start()
	{
		base.Revivable = true;
		if(map == null)
		{
			GameObject map_OB = (GameObject)GameObject.FindGameObjectWithTag("Map");
			if(map_OB)
				map = map_OB.GetComponent<Map>();
			else
				print ("We don't have a Map object in our scene");
		}
	}

	void OnLevelWasLoaded()
	{
		AlarmActive = false;
		alarmActive = false;
		gameOver = false;
	}

	// Return true if the character is dead
	public bool PlayerDead
	{
		get
		{ 
			return Character.Instance.Health < 0;
		}
	}

	// Return true if the game is over
	public bool GameOver
	{
		set{ gameOver = value; }
		get{ return gameOver; }
	}

	public bool AlarmActive
	{
		set{ 
			bool previousValue = alarmActive;
			alarmActive = value; 
			if(alarmActive && !previousValue)
			{
				alarmTime = Time.time + alarmDuration;
				Invoke("DropBarriers",2f);
				RecuringAlarm();
			}
		}
		get { return alarmActive; }
	}

	void RecuringAlarm()
	{
		if(AlarmActive && !PlayerDead)
		{
			if(alarmClip)
				AudioSource.PlayClipAtPoint (alarmClip,Character.Instance.transform.position);
			else
				print ("alarm clip not assigned");
			Invoke ("RecuringAlarm",3f);
		}
	}

	void DropBarriers()
	{
		GameObject [] barriers = GameObject.FindGameObjectsWithTag("Barrier");
		for(int i = 0; i < barriers.Length; i++)
		{
			barriers[i].BroadcastMessage ("PowerOn");
		}
	}

	public void IncrementLockedDoors(Room currentRoom)
	{
		//accesibleDoors --;
		print ("Checking paths");

		if(map == null)
		{
			GameObject map_OB = (GameObject)GameObject.FindGameObjectWithTag("Map");
			map = map_OB.GetComponent<Map>();
		}

		if(map == null)
		{
			Debug.LogError ("Couldn't find map - the scene wasn't build correctly!");
		}

		//room = find the room that the player is in

		//clean up GetPaths from map class

		bool pathsExist = false;
		pathsExist = map.firstRoom.CheckPaths ();

		if(!pathsExist)
		{
			pathsExist = currentRoom.CheckPaths ();
		}

		if(pathsExist)
		{
			print("At least one route to the finish exits");
		}
		else
		{
			AlarmActive = true;
			print ("No routes to finish");
		}

	}

	void Update()
	{
		if(AlarmActive && Time.time > alarmTime)
		{
			AlarmActive = false;
		}
	}
}
