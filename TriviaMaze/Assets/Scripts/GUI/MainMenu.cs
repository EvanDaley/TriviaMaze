/* MainMenu.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: An interactive menu to start the game.
 */

using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	
	// Called when the user presses "PLAY SINGLE PLAYER". Loads the first scene.
	public void ButtonPressed_StartGame()
	{
		print ("Starting new game");
		Application.LoadLevel(Application.loadedLevel + 1);
	}

	public void ButtonPressed_Quit()
	{
		print ("Quitting from main menu");
		Application.Quit();
	}

	// User selected "Modify Database". Load the DB editor scene.
	public void ButtonPressed_MDB()
	{
		print ("Modify database");
		Application.LoadLevel ("ModifyDB");
	}

	public void ButtonPressed_Options()
	{
		print ("ButtonPressed Options");
	}

	public void ButtonPressed_Extras()
	{
		print ("ButtonPressed Extras");
	}
}
