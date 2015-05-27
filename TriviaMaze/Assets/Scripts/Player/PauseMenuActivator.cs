/* PauseMenuActivator.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: A menu activator that lives on the main character. Pressing 'p' 
 * or 'escape' triggers the menu activation.
 */

using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.ImageEffects;
using UnityEngine.UI;

public class PauseMenuActivator : Singleton<PauseMenuActivator> {

	private bool paused = false;
	public GameObject Canvas;
	private float previousSpeed = 1;
	private RigidbodyFirstPersonController fps;
	private BloomAndFlares bloomAndFlares;
	private float previousIntensity = 0;
	private ScreenOverlay overlay;
	private float overlayIntensity = 0;

	// Use this for initialization
	void Start () {
		fps = GetComponent <RigidbodyFirstPersonController>();
		Canvas.SetActive(false);
		bloomAndFlares = GetComponentInChildren<BloomAndFlares>();
		overlay = GetComponentInChildren<ScreenOverlay>();
		Paused = false;
		base.Revivable = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown (KeyCode.Escape) || Input.GetKeyDown (KeyCode.P))
		{
			Paused = !Paused;
		}
	}

	public bool Paused
	{
		set
		{ 
			if(paused == value)
				return;

			paused = value;
			if(paused)
			{
				previousSpeed = Time.timeScale;
				Time.timeScale = 0;
				Canvas.SetActive (true);
				fps.CanMove = false;
				previousIntensity = bloomAndFlares.bloomIntensity;
				bloomAndFlares.bloomIntensity = -.32f;
				overlayIntensity = overlay.intensity;
				overlay.intensity = .03f;
				Cursor.visible = true;
			}
			else
			{
				Time.timeScale = previousSpeed;
				Canvas.SetActive (false);
				if(Character.Instance.Health > 0)
					fps.CanMove = true;
				bloomAndFlares.bloomIntensity = previousIntensity;
				overlay.intensity = overlayIntensity;
				Cursor.visible = false;
			}
		}
		get{return paused;}
	}
}
