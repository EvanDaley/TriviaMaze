/* Character.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: Gets the list of levels and stores string names in ReadSceneNames.scenes.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReadSceneNames : MonoBehaviour
{
	public string[] scenes;
	#if UNITY_EDITOR
	private static string[] ReadNames()
	{
		List<string> temp = new List<string>();
		foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
		{
			if (S.enabled)
			{
				string name = S.path.Substring(S.path.LastIndexOf('/')+1);
				name = name.Substring(0,name.Length-6);
				temp.Add(name);
			}
		}
		return temp.ToArray();
	}
	[UnityEditor.MenuItem("CONTEXT/ReadSceneNames/Update Scene Names")]
	private static void UpdateNames(UnityEditor.MenuCommand command)
	{
		ReadSceneNames context = (ReadSceneNames)command.context;
		context.scenes = ReadNames();
	}
	
	private void Reset()
	{
		scenes = ReadNames();
	}
	#endif
}