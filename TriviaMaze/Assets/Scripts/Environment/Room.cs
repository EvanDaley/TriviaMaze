/* Room.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: A basic room. Contains references to adjacent rooms as well as doors. 
 */

using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {
	
	public bool automaticallyCreateDoors = true;

	public Room left;
	public Room right;
	public Room forward;
	public Room backward;

	public Door leftDoor;
	public Door rightDoor;
	public Door forwardDoor;
	public Door bakcDoor;

	public bool finalRoom = false;
	public bool FinalRoom
	{
		get{ return finalRoom;}
		set{ finalRoom = value; }
	}

	// Use this for initialization
	void Start () {
		BroadcastMessage ("SetRoom",(Room)this,SendMessageOptions.DontRequireReceiver);
	}

	public bool CheckPaths()
	{
		bool canMoveUp = false;
		bool canMoveRight = false;

		if(finalRoom == true)
			return true;

		if(forwardDoor)
		{
			if(!forwardDoor.Locked)
			{
				canMoveUp = forward.CheckPaths ();
			}
		}

		if(rightDoor)
		{
			if(!rightDoor.Locked)
			{
				canMoveRight = right.CheckPaths ();
			}
		}

		if(!canMoveUp && !canMoveRight)
			print (this.gameObject.name + " couldn't find path");

		return canMoveUp || canMoveRight;
	}

	#region Initialization
	public void InitializeRoom(Room left, Room right, Room forward, Room backward)
	{
		this.left = left;
		this.right = right;
		this.forward = forward;
		this.backward = backward;

		CreateFloor ();
		CreateWallLeft ();
		CreateWallRight ();
		CreateWallForward ();
		CreateWallBack ();
	}

	public void CreateFloor()
	{
		if(MapEditorCache.Instance)
		{
			if(MapEditorCache.Instance.floor)
			{
				GameObject temp  = (GameObject)GameObject.Instantiate (MapEditorCache.Instance.floor,transform.position,transform.rotation);
				temp.transform.parent = this.transform;
			}
		}
	}

	public void CreateWallLeft()
	{
		if(left == null)
		{
			GameObject leftWall = (GameObject)GameObject.Instantiate (MapEditorCache.Instance.fullSidewall, transform.position,Quaternion.identity);
			leftWall.transform.position = leftWall.transform.position += new Vector3(-15,5,0);
			leftWall.transform.parent = this.transform;
		}
	}

	public void CreateWallRight()
	{
		
		if(right == null)
		{
			GameObject rightWall = (GameObject)GameObject.Instantiate (MapEditorCache.Instance.fullSidewall, transform.position,Quaternion.identity);
			rightWall.transform.position = rightWall.transform.position += new Vector3(15,5,0);
			rightWall.transform.parent = this.transform;
		}
		else
		{
			GameObject wall3 = (GameObject)GameObject.Instantiate (MapEditorCache.Instance.cubePrefab1,transform.position,transform.rotation);
			wall3.transform.localScale = new Vector3(.5f,10,10f);
			wall3.transform.position = wall3.transform.position += new Vector3(15,5,10);
			wall3.transform.parent = this.transform;
			
			GameObject wall4 = (GameObject)GameObject.Instantiate (MapEditorCache.Instance.cubePrefab1,transform.position,transform.rotation);
			wall4.transform.localScale = new Vector3(.5f,10,10f);
			wall4.transform.position = wall4.transform.position += new Vector3(15,5,-10);
			wall4.transform.parent = this.transform;

			if(automaticallyCreateDoors)
			{
				GameObject sideDoor = (GameObject)GameObject.Instantiate (MapEditorCache.Instance.doorGrouping,transform.position,transform.rotation);
				sideDoor.transform.Rotate (0,90,0);
				sideDoor.transform.parent = this.transform;
				rightDoor = sideDoor.GetComponentInChildren<Door>();
			}
		}
	}

	public void CreateWallForward()
	{
		if(forward == null)
		{
			GameObject forwardWall = (GameObject)GameObject.Instantiate (MapEditorCache.Instance.fullBackWall, transform.position,Quaternion.identity);
			forwardWall.transform.position = forwardWall.transform.position += new Vector3(0,5,15);
			forwardWall.transform.parent = this.transform;
		}
		else
		{
			GameObject wall3 = (GameObject)GameObject.Instantiate (MapEditorCache.Instance.cubePrefab1,transform.position,transform.rotation);
			wall3.transform.localScale = new Vector3(10f,10,.5f);
			wall3.transform.position = wall3.transform.position += new Vector3(10,5,15);
			wall3.transform.parent = this.transform;
			
			GameObject wall4 = (GameObject)GameObject.Instantiate (MapEditorCache.Instance.cubePrefab1,transform.position,transform.rotation);
			wall4.transform.localScale = new Vector3(10f,10,.5f);
			wall4.transform.position = wall4.transform.position += new Vector3(-10,5,15);
			wall4.transform.parent = this.transform;

			if(automaticallyCreateDoors)
			{
				GameObject fwd_door = (GameObject)GameObject.Instantiate (MapEditorCache.Instance.doorGrouping,transform.position,transform.rotation);
				fwd_door.transform.parent = this.transform;
				forwardDoor = fwd_door.GetComponentInChildren<Door>();
			}
		}
	}

	public void CreateWallBack()
	{
		if(backward == null)
		{
			GameObject forwardWall = (GameObject)GameObject.Instantiate (MapEditorCache.Instance.fullBackWall, transform.position,Quaternion.identity);
			forwardWall.transform.position = forwardWall.transform.position += new Vector3(0,5,-15);
			forwardWall.transform.parent = this.transform;
		}
	}

	public int CountAccessibleDoors()
	{
		int count = 0;

		return count;
	}
	#endregion

	#region Gizmos
	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;

		Vector3 slightlyHigherPosition = transform.position + new Vector3(0,2,0);
		
		if(left)
		{
			//print ("drawing line");
			//Gizmos.DrawLine(slightlyHigherPosition,left.transform.position + new Vector3(0,2,0));
		}
		
		if(right)
		{
			Gizmos.DrawLine(slightlyHigherPosition,right.transform.position + new Vector3(0,2,0));
		}
		
		if(forward)
		{
			Gizmos.DrawLine(slightlyHigherPosition,forward.transform.position + new Vector3(0,2,0));
		}

		
		if(backward)
		{
			//Gizmos.DrawLine(slightlyHigherPosition,backward.transform.position + new Vector3(0,2,0));
		}

		Gizmos.color = Color.green;

		if(forwardDoor)
		{
			Gizmos.DrawLine(slightlyHigherPosition,forwardDoor.transform.position);
		}
		
		if(rightDoor)
		{
			Gizmos.DrawLine(slightlyHigherPosition,rightDoor.transform.position);
		}
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		
		Vector3 slightlyHigherPosition = transform.position + new Vector3(0,2,0);
		
		if(left)
		{
			//print ("drawing line");
			Gizmos.DrawLine(slightlyHigherPosition,left.transform.position);
		}
		
		if(right)
			Gizmos.DrawLine(slightlyHigherPosition,right.transform.position);
		
		if(forward)
			Gizmos.DrawLine(slightlyHigherPosition,forward.transform.position);
		
		if(backward)
			Gizmos.DrawLine(slightlyHigherPosition,backward.transform.position);
	}
	#endregion
}
