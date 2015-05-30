/* Character.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: A Singleton class for character traits (health, sound effects, death)
 */

using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;
using UnityStandardAssets.Characters.FirstPerson;

public class Character : Singleton<Character> {
	
	[SerializeField]
	private AudioClip gasp1;
	[SerializeField]
	private AudioClip gasp2;
	
	private float gaspTime = 0f;
	private ScreenOverlay screenOverlay;
	private BloomAndFlares bloomAndFlares;
	private RigidbodyFirstPersonController firstPersonController;
	
	private float dieTime = 0;
	private bool dead = false;
	private float healTime;
	
	// Signifies which audio clip to play next
	private int marker;	
	
	[SerializeField]
	private int health = 100;
	
	public int Health
	{
		get{ return health; }
		set{ health = value; }
	}
	
	void Start()
	{
		if(SavedGame.current == null)
		{
			// We haven't created a game object yet
		}
		else if(SavedGame.current.playerPosition != "")
		{
			//transform.position = SavedGame.current.playerPosition;
		}
		
		base.Revivable = true;
		Time.timeScale = 1;
		screenOverlay = GetComponentInChildren<ScreenOverlay>();
		bloomAndFlares = GetComponentInChildren<BloomAndFlares>();
		firstPersonController = GetComponent<RigidbodyFirstPersonController>();
		
		screenOverlay.intensity = 0;
		bloomAndFlares.bloomIntensity = 0;
		bloomAndFlares.lensflareIntensity = 0;
	}
	
	void OnParticleCollision(GameObject other) {
		TakeDamage(2);
		//print ("Health:  " + Health);
	}
	
	void TakeDamage(int damage)
	{
		healTime = Time.time + .7f;
		health -= damage;
		if(Time.time > gaspTime + 1f)
		{
			gaspTime = Time.time;
			marker ++;
			if(marker % 2 == 0)
				AudioSource.PlayClipAtPoint(gasp1,transform.position);
			else
				AudioSource.PlayClipAtPoint(gasp2,transform.position);
		}
	}
	
	void Update()
	{
		if(Input.GetKeyDown (KeyCode.Tab))
		{
			firstPersonController.CanMove = !firstPersonController.CanMove;
		}
		
		if(Time.timeScale == 0)
			return;
		
		if(dead)
		{
			float bloomIntensity = (Time.time - dieTime)*2;
			//if(bloomIntensity > 9)
			//	bloomIntensity = 9;
			
			bloomAndFlares.bloomIntensity = bloomIntensity;
			bloomAndFlares.lensflareIntensity = bloomIntensity;
			
			screenOverlay.intensity = 0;
		}
		else
		{
			float overlayIntensity = ((100-health) * .05f);
			
			if(overlayIntensity > 1.5)
				overlayIntensity = .9f;
			
			screenOverlay.intensity = overlayIntensity;
			
			bloomAndFlares.bloomIntensity = 0;
			bloomAndFlares.lensflareIntensity = 0;
		}
		
		if(Time.time > healTime && health > 0 && health < 100)
		{
			health ++;
			healTime = Time.time + .05f;
		}
		
		if(health > 100)
			health = 100;
		
		if(health < 0 && dead == false)
		{
			Die ();
		}
	}
	
	void Die()
	{
		Time.timeScale = .5f;
		dieTime = Time.time + .2f;
		dead = true;
		
		// Lock the movement controls
		firstPersonController.CanMove = false;
		GameObject child = bloomAndFlares.gameObject;
		
		// Seperate the camera from the player
		child.transform.parent = null;
		Rigidbody cameraRigidBody = child.AddComponent<Rigidbody>();
		cameraRigidBody.isKinematic = false;
		
		// Add a little bit of force to make it look like the player is falling to the right
		cameraRigidBody.AddForce (cameraRigidBody.gameObject.transform.right * 100);
		
		Invoke ("PauseAfterDelay",3.2f);
	}
	
	void PauseAfterDelay()
	{
		PauseMenuActivator.Instance.Paused = true;
	}
}
