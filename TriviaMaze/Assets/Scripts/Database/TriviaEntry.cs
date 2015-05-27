/// A single entry in the list of trivia questions. 
public class TriviaEntry
{
	public string question;
	public string a;
	public string b;
	public string c;
	public string d;
	public string category;
	public string difficulty;
	
	public TriviaEntry(string question, string a, string b, string c, string d, string cat, string diff)
	{
		this.question = question;
		this.a = a;
		this.b = b;
		this.c = c;
		this.d = d; 
		this.category = cat;
		this.difficulty = diff;
	}
}