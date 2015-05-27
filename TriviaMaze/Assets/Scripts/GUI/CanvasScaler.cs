/* CanvasScaler.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: A simple class to stretch a Canvas horizontally
 * when the associated terminal becomes active. We use Transform.localScale(x,y,z)
 * to scale the canvas in x, y, z directions. 
 */

using UnityEngine;
using System.Collections;

public class CanvasScaler : MonoBehaviour {
	
	[SerializeField]
	private Terminal terminal;
	[SerializeField]
	public float scale;

	private Vector3 normalScale;
	private Transform canvas;
	private bool active = false;
	private Vector3 startingScale = new Vector3(0,0,0);

	// When the object is created we cache references to needed objects
	void Start()
	{
		canvas = transform;
		canvas.localScale = startingScale;
		normalScale = new Vector3(scale,scale,scale);
	}

	void Update () {

		if(active)
		{
			// We are activated. Make the canvas expand horizontally if it isn't at full length.
			if(canvas.localScale.x < scale)
			{
				// Scale up
				float delta = canvas.localScale.x + (.0015f * Time.timeScale);
				canvas.localScale = new Vector3(delta,scale,delta);
			}
			else 
			{
				// Once x is large enough we can stop scaling
				canvas.localScale = normalScale;
			}
		}
		else
		{
			// We are deactivated so we should be moving toward scale(0,0,0)
			if(canvas.localScale.x > 0)
			{
				// Scale down
				float delta = canvas.localScale.x - (.0015f * Time.timeScale);
				canvas.localScale = new Vector3(delta,scale,delta);
			}
			else 
			{
				// The canvas has become sufficiently small. Round down to (0,0,0)
				canvas.localScale = Vector3.zero;
			}
		}
	}
	
	// A broadcast from the terminal class invokes this method and updates state.
	void StateChange()
	{
		active = terminal.Active;
	}
}
