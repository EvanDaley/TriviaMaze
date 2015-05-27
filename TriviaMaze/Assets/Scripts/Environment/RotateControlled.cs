/* RotateControlled.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: A simple script that rotates an object around its local x, y, z.
 */
using UnityEngine;
using System.Collections;

public class RotateControlled : MonoBehaviour {

	public float xRotation;
	public float yRotation;
	public float zRotation;

	// Update is called once per frame
	void Update () {
		transform.Rotate (Time.deltaTime*xRotation,Time.deltaTime * yRotation,Time.deltaTime*zRotation,Space.Self);
	}
}
