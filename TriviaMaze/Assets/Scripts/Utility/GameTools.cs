/* GameTools.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: A simple helper class filled with static methods to take care of repetitive tasks.
 */

using UnityEngine;
using System.Collections;

public class GameTools : MonoBehaviour {

	/// <summary>
	/// Find a child of a GameObject with a certain tag.
	/// </summary>
	/// /// <long summary>
	/// Example Usage: At the start of the game each terminal gets a question from the database
	/// and then updates it's Canvas with the question and 4 possible answers. In order to 
	/// update the Canvas we have to grab each of it's children (Question, slot A, slot B, etc)
	/// and update their Text components. This function makes it easy for us to grab the chidren. 
	/// of the Canvas. 
	/// </long summary>
	/// <returns>The child with tag.</returns>
	/// <param name="root">Root.</param>
	/// <param name="PI_tag">P i_tag.</param>
	public static GameObject GetChildWithTag(Transform root, string PI_tag)
	{
		foreach (Transform child in root)
		{
			if(child.tag == PI_tag)
			{
				return child.gameObject;
			}
		}

		// We didn't find the tag in the children so search through grand-children
		foreach (Transform child in root)
		{
			foreach (Transform grandchild in child)
			{
				if(grandchild.tag == PI_tag)
				{
					return grandchild.gameObject;
				}
			}
		}

		return null;
	}

	public static Transform GetRootTransform(Transform someTransform)
	{
		while(someTransform.transform.parent != null)
			someTransform = someTransform.transform.parent;

		//print (someTransform);
		return someTransform.transform;
	}

	public static void CountSceneObjects()
	{
		GameObject[] allGameobjects = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[]; 
		Debug.Log(allGameobjects.Length);
	}
}
