/* LoadGameMenu.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: The basic mechanics of the save/load user interface
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LoadGameMenu : MonoBehaviour {
	
	public GameObject panelOfSaves;
	public GameObject savePrefab;
	public int offset = 60;
	
	void Start()
	{
		GrabList ();
	}
	
	public void ButtonPressed_ReturnToMain()
	{
		print("Return to MainMenu selected");
		Application.LoadLevel ("MainMenu");
	}
	
	public void ButtonPressed_NewGame()
	{
		print ("Loading new game");
		SaveLoad.sceneObjects = new List<Transform>();
		SavedGame.current = null;
		Application.LoadLevel ("Level1Preloader");
	}
	
	public void ButtonPressed_LoadGame(SimpleIndexContainer container)
	{
		SaveLoad.Load (SaveLoad.savedGames[SaveLoad.savedGames.Count - container.index - 1]);
	}
	
	void GrabList()
	{
		SaveLoad.FetchList ();
		
		for(int i = 0; i < SaveLoad.savedGames.Count; i++) // while i < list length
		{
			CreateOneSaveElement (SaveLoad.savedGames[SaveLoad.savedGames.Count - i - 1],i);
			//print(SaveLoad.savedGames[SaveLoad.savedGames.Count - i - 1],i);
			//SavedObjectState savable = SaveLoad.savedGames[SaveLoad.savedGames.Count - i - 1].savedStates[0];
			//print ("length: " + SaveLoad.savedGames[SaveLoad.savedGames.Count - i - 1].savedStates.Count + " first id: " + savable.uniqueID);
		}
	}
	
	void CreateOneSaveElement(SavedGame singleGame, int index)
	{
		GameObject savedGame = GameObject.Instantiate (savePrefab,transform.position,transform.rotation) as GameObject;
		savedGame.transform.SetParent (panelOfSaves.transform,false);
		
		SimpleIndexContainer indexContainer =  savedGame.GetComponent<SimpleIndexContainer>();
		indexContainer.index = index;
		
		RectTransform cachedRect = savedGame.GetComponent<RectTransform>();
		cachedRect.anchoredPosition = new Vector3(445, -offset*index - 30, 0);
		
		Text cachedLabel = savedGame.GetComponentInChildren<Text>();

		string temp = "";
		if(index == 0)
			temp = "      (most recent)";

		cachedLabel.text = singleGame.location + "  " + " [" + singleGame.time + "]" + temp;
		
		Button button = savedGame.GetComponentInChildren<Button>();
		button.gameObject.name = index.ToString ();
	}
	
}
