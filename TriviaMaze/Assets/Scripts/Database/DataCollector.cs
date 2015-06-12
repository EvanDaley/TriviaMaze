/* DataCollector.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: An SQLite to Unity class. Collects everything from the database,
 * stores each tuple in a TriviaEntry object, and stores TriviaEntry objects
 * in a private arraylist. To request a question call DataCollector.GetQuestion(cat, diff)
 */

using UnityEngine;
using System.Data;
using Mono.Data.SqliteClient;
using System.IO;
using System.Text;
using System.Collections;
using System;
using System.Text.RegularExpressions;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Sql;

public class DataCollector : Singleton<DataCollector> {
	
	[SerializeField]
	private bool DebugMode = false;

	// A list of all the questions stored as TriviaEntries
	private ArrayList triviaList; 

	// Table name and DB actual file location
	private const string SQL_DB_NAME = "AllQuestions";

	// A path to the Database. Set in awake function so we can check operating system path seperator.
	private static string SQL_DB_LOCATION = "";
	
	// The names of all the tables we want to load
	private const string SQL_TABLE_NAME = "AllQuestions";
	private const string SQL_TABLE_NAME2 = "";

	// The maximum difficulty level of questions in the database
	[SerializeField]
	private int maxDifficulty = 3;

	// DB objects
	private IDbConnection mConnection = null;
	private IDbCommand mCommand = null;
	private IDataReader mReader = null;
	private string mSQLString;
	
	public bool mCreateNewTable = false;
	private bool finished = false;

	#region General Functions

	/// Awake finds the database and calls a method to load all trivia entries into memory. 
	void Awake()
	{
		SQL_DB_LOCATION = "URI=file:" + Application.streamingAssetsPath 
				+ Path.DirectorySeparatorChar
				+ SQL_DB_NAME;

		GetAllWords();
	}

	/// When the game ends and the object is destroyed we close the connection.
	new void OnDestroy()
	{
		SQLiteClose();
		print ("DataCollector was destroyed");
	}
	
	/// Print the entire list of questions to the console.
	private void PrintList()
	{
		for(int i = 0; i < triviaList.Count; i++)
		{
			TriviaEntry entry = (TriviaEntry)triviaList[i];
			print (entry.question + ", " + entry.a + ", " + entry.b + ", " + entry.c + ", " 
			       + entry.d +", "+ entry.category +", "+ entry.difficulty+ "\n");
		}
	}
	
	/// Gets a question from the list based on category and difficulty.
	public TriviaEntry GetQuestion(string cat, int diff)
	{
		// Choose a random starting index
		int startingIndex = UnityEngine.Random.Range (1,triviaList.Count);

		// Start at a random point in the list and walk to the end
		for(int i = startingIndex; i < triviaList.Count; i++)
		{
			TriviaEntry entry = (TriviaEntry)triviaList[i];
			int parsedDiff = 1;
			Int32.TryParse (entry.difficulty, out parsedDiff);
			if(entry.category.Equals (cat) && parsedDiff == diff)
			{
				triviaList.Remove (entry);
				return entry;
			}
		}

		// We went from the random index to the end and didn't find a match
		// Start over at the very beginning
		for(int i = 0; i < triviaList.Count; i++)
		{
			TriviaEntry entry = (TriviaEntry)triviaList[i];
			int parsedDiff = 1;
			Int32.TryParse (entry.difficulty, out parsedDiff);
			if(entry.category.Equals (cat) && parsedDiff == diff)
			{
				triviaList.Remove (entry);
				return entry;
			}
		}

		/// We didn't find a matching entry 
		/// Search again in the same category but randomize the difficulty. 
		return GetQuestion(cat, UnityEngine.Random.Range (1,maxDifficulty+1));
	}
	
	/// Shuffles the list of trivia.
	private void ShuffleList()
	{
		ArrayList tempList = new ArrayList();

		// Move everything into a seperate list.
		for(int i = 0; i < triviaList.Count; i++)
		{
			tempList.Add (triviaList[i]);
		}

		// Clean out our list
		triviaList = new ArrayList();

		// Repopulate the trivia list taking values at random
		while(tempList.Count > 0)
		{
			int randomIndex = UnityEngine.Random.Range (0,tempList.Count);
			triviaList.Add (tempList[randomIndex]);
			tempList.RemoveAt (randomIndex);
		}
	}

	#endregion

	#region Update Database

	public void AddWord(string question, string correctA, string wrong1, string wrong2, string wrong3)
	{
		// Strip out unwanted characters
		question = Sanitize (question);
		correctA = Sanitize (correctA);
		wrong1 = Sanitize (wrong1);
		wrong2 = Sanitize (wrong2);
		wrong3 = Sanitize (wrong3);

		// Create a connection
		mConnection = new SqliteConnection(SQL_DB_LOCATION);

		// Prepare to give SQL Commands
		mCommand = mConnection.CreateCommand();
		mConnection.Open ();
		
		// Insert user's text into our SQL command as paramaterized statements. 
		mCommand.CommandText = "INSERT INTO " + SQL_TABLE_NAME + 
			" (Question, CorrectAnswer, Answer2, Answer3, Answer4, Category, Difficulty) VALUES ('" + 
				question + "', '" + correctA  +"', '" + wrong1 + "', '" + wrong2 + "', '" + wrong3 + "', 'Art + Lit', '1');";

		mCommand.ExecuteNonQuery ();

		mConnection.Close ();
	}
	
	private string Sanitize(string userInput)
	{
		userInput = Regex.Replace (userInput, @"'[&^$#@!()+-,:;]", "", RegexOptions.None); 
		if(userInput == "" || userInput == " ")
		{
			userInput = "(No answer given)";
		}
		return userInput;
	}

	#endregion

	#region Query The Database

	/// Get each question out of the DB and store them in triviaList as TriviaEntry objects.
	public void GetAllWords()
	{
		triviaList = new ArrayList();
		Debug.Log("Opening SQLite Connection at " + SQL_DB_LOCATION);

		// Create a connection
		mConnection = new SqliteConnection(SQL_DB_LOCATION);

		// Prepare to give SQL Commands
		mCommand = mConnection.CreateCommand();
		mConnection.Open();

		// Request all entries from the desired table
		mCommand.CommandText = "SELECT * FROM " + SQL_TABLE_NAME;
		mReader = mCommand.ExecuteReader();

		while (mReader.Read())
		{
			// Take a whole row and store it's values as strings
			string question = mReader.GetString(0);
			string a = mReader.GetString(1);
			string b = mReader.GetString(2);
			string c = mReader.GetString(3);
			string d = mReader.GetString(4);
			string category = mReader.GetString(5);
			string difficulty = mReader.GetString(6);

			// Store the row in a TriviaEntry object
			TriviaEntry entry = new TriviaEntry(question, a, b, c, d, category, difficulty);
			triviaList.Add (entry);
		}

		// Randomly shuffle everything
		ShuffleList();

		// Display the list to the console?
		if (DebugMode)
		{
			PrintList ();
		}

		mReader.Close();
		mConnection.Close();

		finished = true;
	}

	/// <summary>
	/// Basic execute command - open, create command, execute, close
	/// </summary>
	/// <param name="commandText"></param>
	public void ExecuteNonQuery(string commandText)
	{
		mConnection.Open();
		mCommand.CommandText = commandText;
		mCommand.ExecuteNonQuery();
		mConnection.Close();
	}
	
	// Close everything
	private void SQLiteClose()
	{
		if (mReader != null && !mReader.IsClosed)
			mReader.Close();
		mReader = null;
		
		if (mCommand != null)
			mCommand.Dispose();
		mCommand = null;
		
		if (mConnection != null && mConnection.State != ConnectionState.Closed)
			mConnection.Close();
		mConnection = null;
	}
	
	#endregion

	#region Public Properties

	public bool Finished
	{
		get{ return finished; }
	}

	#endregion
}
