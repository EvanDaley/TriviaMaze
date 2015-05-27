/* LockIndicator.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: A holographic light that indicates the state of doors and terminals.
 */
using UnityEngine;
using System.Collections;

public class LockIndicator : MonoBehaviour {

	public Material locked;
	public Material idle;
	public Material open;

	void IndicateLock()
	{
		Renderer [] indicators = gameObject.GetComponentsInChildren<Renderer>();
		for(int i = 0; i < indicators.Length; i++)
		{
			indicators[i].material = locked;
		}
		
	}

	void Reset()
	{
		Renderer [] indicators = gameObject.GetComponentsInChildren<Renderer>();
		for(int i = 0; i < indicators.Length; i++)
		{
			indicators[i].material = idle;
		}
	}

	void IndicateOpen()
	{
		Renderer [] indicators = gameObject.GetComponentsInChildren<Renderer>();
		for(int i = 0; i < indicators.Length; i++)
		{
			indicators[i].material = open;
		}
	}

}

