/* HolographicMenu.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: A floating menu that recieves questions from a terminal.
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HolographicMenu : MonoBehaviour {
	
	public Text question;
	public Text slot1;
	public Text slot2;
	public Text slot3;
	public Text slot4;
	public Text category;
	public Text difficulty;

	public StateEnum state;

	// Update the text on a Holographic Menu
	public void UpdateQuestion(TriviaEntry entry)
	{
		this.question.text = entry.question;
		this.slot1.text = entry.a;
		this.slot2.text = entry.b;

		// Check if we have a reference to slot 3 and if we do, update it's text
		if(slot3)
			this.slot3.text = entry.c;

		if(slot4)
			this.slot4.text = entry.d;

		if(category)
			this.category.text = entry.category;

		if(difficulty)
			difficulty.text = entry.difficulty;
	}
}
