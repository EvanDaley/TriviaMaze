/* LaunchPad.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: A simple mechanism to launch the player. By default this is 
 * deactivated. Must be turned on by answering a question. While pad is activated,
 * if the player enters the trigger zone he wil be catapulted.
 */

using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class LaunchPad : MonoBehaviour {

	private Animator anim;
	private Rigidbody charRigidbody;
	private RigidbodyFirstPersonController rfpc;
	private AudioSource playerAudioSource;
	private bool activated;

	public float forceMultiplier = 2000;
	public Vector3 direction;

	public AudioClip launchClip;
	public AudioClip gaspClip;
	public AudioClip gaspClip2;
	public AudioClip powerOnClip;
	

	public bool Activated
	{
		get{return activated;}
		set{activated = value;}
	}

	void Start()
	{
		anim = GetComponentInChildren<Animator>();
		charRigidbody = Character.Instance.gameObject.GetComponent<Rigidbody>();
		rfpc = Character.Instance.gameObject.GetComponent<RigidbodyFirstPersonController>();
		playerAudioSource = Character.Instance.GetComponent<AudioSource>();
	}

	void OnTriggerEnter()
	{
		if(Activated)
			Invoke ("Activate",.3f);
	}

	void Activate()
	{
		print("Activating LaunchPad");

		// Trigger the animation
		anim.SetBool ("Activate",true);

		rfpc.CanMove = false;
		charRigidbody.AddForce(direction*forceMultiplier);

		Invoke ("Deactivate",.2f);
		Invoke ("Gasp",.5f);
		Invoke ("Gasp2",2.1f);

		playerAudioSource.PlayOneShot(launchClip,1);
	}

	void Deactivate()
	{
		// Turn off the animation so it doesn't repeat
		anim.SetBool ("Activate",false);

		rfpc.CanMove = true;
	}

	void Gasp()
	{
		playerAudioSource.PlayOneShot(gaspClip,1);
	}

	void Gasp2()
	{
		playerAudioSource.PlayOneShot(gaspClip2,1);
	}

	void PowerOn()
	{
		Activated = true;
		playerAudioSource.PlayOneShot (powerOnClip,1);
	}
}
