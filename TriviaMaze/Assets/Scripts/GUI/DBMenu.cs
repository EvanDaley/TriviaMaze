/* DBMenu.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: A simple menu for accepting new questions from the player.
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DBMenu : MonoBehaviour {

	public GameObject Canvas;
	public InputField question;
	public InputField correctAnswer;
	public InputField wrongAnswer1;
	public InputField wrongAnswer2;
	public InputField wrongAnswer3;

	public void ButtonPressed_ReturnToMain()
	{
		print("Return to MainMenu selected");
		Application.LoadLevel ("MainMenu");
	}

	public void ButtonPressed_Add()
	{
		
		//print("Trying to add user input to database:");
		string result = question.text + ", " +
				correctAnswer.text + ", " +
				wrongAnswer1.text + ", " +
				wrongAnswer2.text + ", " + 
				wrongAnswer3.text;

		print("Adding: (" + result + ")");
		DataCollector.Instance.AddWord (question.text, correctAnswer.text, wrongAnswer1.text, wrongAnswer2.text, wrongAnswer3.text);

		//After we add it clear out the text
		question.text = "";
		correctAnswer.text = "";
		wrongAnswer1.text = "";
		wrongAnswer2.text = "";
		wrongAnswer3.text = "";
	}
}
