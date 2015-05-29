/* SubmitButton.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: The submit button for a short answer question. If the user gets the question right tell the terminal.
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SubmitButton : MonoBehaviour {

	[SerializeField]
	private InputField userInput;

	[SerializeField]
	private Terminal terminal;

	[SerializeField]
	private string correctAnswer;
	
	public void ButtonPressed_SubmitAnswer()
	{
		if(userInput.text.Equals (correctAnswer))
		{
			terminal.SelectAnswer (9);
		}
		else
		{
			char [] temp = new char[correctAnswer.Length];

			for(int i = 0; i < correctAnswer.Length; i++)
			{	
				if(userInput.text.Length > i)
				{
					if(userInput.text[i] != correctAnswer[i])
					{
						temp[i] = '-';
					}
					else
					{
						temp[i] = userInput.text[i];
					}
				}
				else
					temp[i] = '-';
			}

			userInput.text = new string(temp);
		}
	}

	void Update()
	{
		if(Input.GetKeyDown (KeyCode.Return))
		{
			ButtonPressed_SubmitAnswer();
		}
	}
}
