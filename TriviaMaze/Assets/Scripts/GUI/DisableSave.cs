/* DisableSave.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: After the player has been killed disable the players ability to save	*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisableSave : MonoBehaviour {

	Button saveButton;

	void Start()
	{
		saveButton = GetComponent<Button>();
	}

	// Update is called once per frame
	void Update () {
		if(Character.Instance.Health < 1)
		{
			saveButton.interactable = false;
		}
		else
		{
			saveButton.interactable = true;
		}
	}
}
