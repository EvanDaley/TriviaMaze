/* Character.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: A singleton class for all singelton classes to extend. 
 * 
 * Badass Features: 
 * Creates a folder in the scene hierarchy to keep all singletons organized.
 * Preserves singeltons across levels (using DontDestroyOnLoad). 
 * 
 */

using UnityEngine;

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	// A private reference to this object. Can be accessed through public property.
    private static T _instance;
    
	// Make sure this is our only singelton of Type T
    private static object _lock = new object();

	// Is the game quitting? If so don't recreate the singleton even if a different object needs it.
	private static bool applicationIsQuitting = false;

	// Should we revive this singelton if it is destroyed?
	private bool revivable = false;

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting) {
                Debug.LogWarning("[Singleton] Instance '"+ typeof(T) +
                                 "' already destroyed on application quit." +
                                 " Won't create again - returning null.");
                return null;
            }
            
            lock(_lock)
            {
                if (_instance == null)
                {
                    _instance = (T) FindObjectOfType(typeof(T));
                    
                    if ( FindObjectsOfType(typeof(T)).Length > 1 )
                    {
                        Debug.LogError("[Singleton] Something went really wrong " +
                                       " - there should never be more than 1 singleton!" +
                                       " Reopenning the scene might fix it.");
                        return _instance;
                    }
                    
                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) " + typeof(T).ToString();

                        DontDestroyOnLoad(singleton);

                        Debug.Log("[Singleton] An instance of " + typeof(T) +
                                  " is needed. '" + singleton +
                                  "' was created with DontDestroyOnLoad.");

                        GameObject SingletonHolder = null;
                        if (SingletonHolder = GameObject.FindGameObjectWithTag("Singletons")) 
                        {

                        }
                        else
                        {
                            print("Creating Singleton Folder.");
                            SingletonHolder = new GameObject();
                            SingletonHolder.name = "Singletons";
                            SingletonHolder.tag = "Singletons";
							DontDestroyOnLoad (SingletonHolder);
                        }

                        singleton.transform.parent = SingletonHolder.transform;

                    }
                    else
					{
                        Debug.Log("[Singleton] Using instance already created: " +
                                  _instance.gameObject.name);
                    }
                }
                
                return _instance;
            }
        }
    }

    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    public void OnDestroy () {
		if(!Revivable)		
        	applicationIsQuitting = true;
    }

	public bool Revivable
	{
		set{revivable = value;}
		get{return revivable;}
	}
}