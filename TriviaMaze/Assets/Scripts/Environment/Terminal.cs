/* Terminal.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: An interactive terminal object. Requests a TriviaEntry from the 
 * DataCollector at the start of the game. Activates when a player gets close enough.
 * While active, accepts input (1,2,3,4). After recieving input it triggers an alarm
 * or opens a door and then deactivates permanently.
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Terminal : MonoBehaviour, IActivate, ISaveState {

	// Player is inside the trigger box to activate the terminal
	private bool playerInside;  

	// The state of the object
	private StateEnum state;

	// The question we retrieve from DataCollector
	private TriviaEntry entry;

	// Category to search for
	[SerializeField]
	private Category category;

	// The number of guesses the player has made so far
	private int actualTries = 0;

	// The max number of guesses before a door locks
	[SerializeField]
	private int maxTries = 1;

	// The index of the correct answer
	[SerializeField]
	private int correctAnswer;

	// Desired difficulty of questions at this terminal
	[SerializeField]
	private int difficulty = 1;

	// Activatable object (door, launchpad, etc)
	public GameObject powerableObject;
	private IActivate powerableObjectScript;

	// The holographic canvas we will apply the question to
	[SerializeField]
	private HolographicMenu holoMenu;

	// Optional Indicator lights
	public GameObject indicator1;
	public GameObject indicator2;
	public GameObject indicatorPath;
	public Room room;

	// Audio clips for responding to player input
	public AudioClip menuSound1;
	public AudioClip wrongAnswerClip;
		
	// Should this terminal ask the database for a question?
	public bool queryDatabase = true;

	private float cooldownTime = -1f;
	private int uniqueID = 0;

	#region Initialization
	// Initialize the Terminal
	public void Start()
	{
		if(queryDatabase)
			InitializeQuestion();

		if(powerableObject)
		{
			powerableObjectScript = powerableObject.GetComponent<IActivate>();
			if(powerableObjectScript == null)
			{
				print ("We couldn't find a IActivate on our target powerable object");
			}
		}

		UniqueID = SaveLoad.GetUniqueID (this.transform);
	}

	// Get a question from the Databse according to our own level and category and send it to the holographic menu.
	void InitializeQuestion()
	{
		string categoryName = GetMyCategory ();

		// Ask the Data Collector Singleton for a unique question based on Category and Difficulty
		entry = DataCollector.Instance.GetQuestion (categoryName, difficulty);

		// Move the correct answer out of the first slot
		SwapAnswers();

		holoMenu.UpdateQuestion (entry);
	}

	// Get the category we chose for this terminal. Based on enum to remove chance of mispelling or non-existent category.
	string GetMyCategory()
	{
		string categoryName = "Default";
		
		// The category is determined by the enum displayed in the inspector
		if(category == Category.EnglishAndArts)
		{
			categoryName = "English & Arts";
		}
		else if(category == Category.Biology)
		{
			categoryName = "Biology";
		}
		else if(category == Category.FoodAndDrink)
		{
			categoryName = "Food & Drink";
		}
		else if(category == Category.GeneralKnowledge)
		{
			categoryName = "General Knowledge";
		}
		else if(category == Category.GeographyAndPlaces)
		{
			categoryName = "Geography & Places";
		}
		else if(category == Category.History)
		{
			categoryName = "History";
		}
		else if(category == Category.Music)
		{
			categoryName = "Music";
		}
		else if(category == Category.People)
		{
			categoryName = "People";
		}
		else if(category == Category.ScienceAndNature)
		{
			categoryName = "Science & Nature";
		}
		else if(category == Category.Sport)
		{
			categoryName = "Sport";
		}
		else if(category == Category.TVAndFilm)
		{
			categoryName = "TV & Film";
		}
		else if(category == Category.RandomCategory)
		{
			categoryName = GetRandomCategory();
		}

		return categoryName;
	}

	void SetRoom(Room room)
	{
		this.room = room;
	}

	string GetRandomCategory()
	{
		print ("Random category incomplete!!!");
		return "Biology";
	}

	void SwapAnswers()
	{
		string answer = entry.a;
		int slot = UnityEngine.Random.Range (1,5);
		
		// Swap the correct answer from the first slot to the random slot
		switch(slot)
		{
		case 1:
			correctAnswer = 1;
			break;
		case 2:
			entry.a = entry.b;
			entry.b = answer;
			correctAnswer = 2;
			break;
		case 3:
			entry.a = entry.c;
			entry.c = answer;
			correctAnswer = 3;
			break;
		case 4:
			entry.a = entry.d;
			entry.d = answer;
			correctAnswer = 4;
			break;
		}
	}
	
	#endregion

	#region User Interaction
	void OnTriggerEnter()
	{
		playerInside = true;
	}

	void OnTriggerExit()
	{
		playerInside = false;
	}
	
	void SelectAnswer(int selection)
	{
		print ("User selected: " + selection);

		actualTries ++;

		cooldownTime = Time.time + 1.2f;

		State = StateEnum.deactivated;

		// User entered 0 for guaranteed wrong answer
		if (selection == 0)
			selection = correctAnswer + 1 % 4;

		if(selection == 9)
			selection = correctAnswer;

		if(correctAnswer == selection)
		{
			CorrectGuess();
			powerableObjectScript.State = StateEnum.powered;
			State = StateEnum.locked;
		}
		else
		{
			if(actualTries >= maxTries)
			{
				powerableObjectScript.State = StateEnum.locked;
				State = StateEnum.locked;
			}
			else
			{
				if(queryDatabase)
					Invoke ("InitializeQuestion",.7f);
			}

			IncorrectGuess();
		}
			
	}

	// Update is called once per frame. Wait for user input.
	void Update () {

		if(playerInside)
		{
			// We were not active. Activate using State property. Property set method updates holographic menu.
			if(Time.time > cooldownTime && !Active)
			{
				State = StateEnum.active;
			}
		}
		else
		{
			if(Active)
				State = StateEnum.deactivated;
		}

		if(state == StateEnum.active)
		{
			// Check for input from the keyboard
			if(Input.GetKeyDown(KeyCode.Alpha1))
			{
				// User pressed "1" on the keyboard. 
				SelectAnswer(1);
			}
			if(Input.GetKeyDown(KeyCode.Alpha2))
			{
				SelectAnswer(2);
			}
			
			if(Input.GetKeyDown(KeyCode.Alpha3))
			{
				SelectAnswer(3);
			}
			
			if(Input.GetKeyDown(KeyCode.Alpha4))
			{
				SelectAnswer(4);
			}

			// 9 automatically picks the correct answer
			if(Input.GetKeyDown(KeyCode.Alpha9))
			{
				SelectAnswer(9);
			}

			// 0 automatically picks the wrong answer
			if(Input.GetKeyDown(KeyCode.Alpha0))
			{
				SelectAnswer(0);
			}
		}
	}

	#endregion 

	#region Communicate with Door

	// Trigger the door animation
	void CorrectGuess()
	{
		// If there is a blue path from the terminal to the door tell it we opened the door.
		if(indicatorPath)
			indicatorPath.BroadcastMessage("IndicateOpen");

		// On the first correct guess we change all indicator lights to green
		if(indicator1)
			indicator1.BroadcastMessage ("IndicateOpen");
		if(indicator2)
			indicator2.BroadcastMessage ("IndicateOpen");

		// Activate the object (door, launchpad, etc.) so that it can power up or move out of the way.
		powerableObjectScript.State = StateEnum.powered;
	}

	// Trigger the door lock event
	void IncorrectGuess()
	{
		// Play an audio clip
		AudioSource.PlayClipAtPoint (wrongAnswerClip,Character.Instance.transform.position);

		// If we got our first guess wrong make the first indicator turn red
		if(actualTries == 1 && indicator1)
		{
			indicator1.BroadcastMessage ("IndicateLock");
		}

		// If we got our second guess wrong make the second indicator turn red
		if(actualTries == 2 && indicator2 || actualTries >= maxTries)
		{
			indicator2.BroadcastMessage ("IndicateLock");
			indicatorPath.BroadcastMessage("IndicateLock");
		}

		AlarmManager.Instance.IncrementLockedDoors(room);
	}

	#endregion

	#region Properties
	public StateEnum State
	{
		get{ return state; }
		set
		{
			if(state == StateEnum.locked)
				return;

			if(value == StateEnum.active)
				AudioSource.PlayClipAtPoint(menuSound1,Character.Instance.transform.position);

			state = value;
			holoMenu.BroadcastMessage ("StateChange");
		}
	}

	public bool Active
	{
		get{ return state == StateEnum.active; }
	}

	// Which holographic canvas was assigned to us? 
	public HolographicMenu HolographicMenu
	{
		get{ return holoMenu; }
	}

	public int UniqueID
	{
		get{ return uniqueID;}
		set{ uniqueID = value; }
	}

	#endregion
}
