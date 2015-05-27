/* PauseMenu.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: An simple pause menu. Controlled through the Pause Menu Canvas attached to the player.
 */

using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if(PauseMenuActivator.Instance.Paused)
		{
			if(Input.GetKeyDown (KeyCode.Q))
			{
				ButtonPressed_QuitGame();
			}
			
			if(Input.GetKeyDown (KeyCode.R))
			{
				ButtonPressed_Resume();
			}

			if(Input.GetKeyDown (KeyCode.O))
			{
				ButtonPressed_Options();
			}

			if(Input.GetKeyDown (KeyCode.S))
			{
				ButtonPressed_Save();
			}
		}
	}

	public void ButtonPressed_QuitGame()
	{
		Time.timeScale = 1;
		print ("Quiting from pause menu");
		//Application.Quit ();
		Application.LoadLevel ("MainMenu");
	}

	public void ButtonPressed_Resume()
	{
		PauseMenuActivator.Instance.Paused = false;
	}

	public void ButtonPressed_Save()
	{
		print ("Save the game!");
		SaveLoad.Save();
	}

	public void ButtonPressed_Options()
	{
		//trigger options somehow
		print ("User pressed options button in pause menu");
	}
}
