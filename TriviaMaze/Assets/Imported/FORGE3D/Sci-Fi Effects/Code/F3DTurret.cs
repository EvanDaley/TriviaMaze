using UnityEngine;
using System.Collections;

public class F3DTurret : MonoBehaviour
{
    public Transform hub;        
    public Transform barrel;       
	private Transform player;
	private F3DFXController f3dxC;
	private bool clearLOS;	//does the turret have a clear line-of-sight to the player?
	private float maxRange = 50;
	private float fireTime = 0;

    //RaycastHit hitInfo;           
    bool isFiring;                 
    
    //float hubAngle, 
	//float barrelAngle;    

	void Start()
	{
		f3dxC = transform.parent.GetComponentInChildren<F3DFXController>();
		player = Character.Instance.transform;
	}

    // Project vector on plane
    Vector3 ProjectVectorOnPlane(Vector3 planeNormal, Vector3 vector)
    {
        return vector - (Vector3.Dot(vector, planeNormal) * planeNormal);
    }

    // Get signed vector angle
    float SignedVectorAngle(Vector3 referenceVector, Vector3 otherVector, Vector3 normal)
    {
        Vector3 perpVector;
        float angle;
       
        perpVector = Vector3.Cross(normal, referenceVector);
        angle = Vector3.Angle(referenceVector, otherVector);
        angle *= Mathf.Sign(Vector3.Dot(perpVector, otherVector));

        return angle;
    }

    // Turret tracking
    void Track()
    {
        if(hub != null && barrel != null)
        {
			Vector3 headingVector = ProjectVectorOnPlane(hub.up, player.position - hub.position);
           
			Quaternion newHubRotation = Quaternion.LookRotation(headingVector);

            //hubAngle = SignedVectorAngle(transform.forward, headingVector, Vector3.up);
                            
            hub.rotation = Quaternion.Slerp(hub.rotation, newHubRotation, Time.deltaTime * 5f);

			Vector3 elevationVector = ProjectVectorOnPlane(hub.right, player.position - barrel.position);
           
			Quaternion newBarrelRotation = Quaternion.LookRotation(elevationVector);

            //barrelAngle = SignedVectorAngle(hub.forward, elevationVector, hub.right);

            barrel.rotation = Quaternion.Slerp(barrel.rotation, newBarrelRotation, Time.deltaTime * 5f);
        }
    }

	//Check if the turret can see the player
	void CheckLOS()
	{
		clearLOS = false;
		Transform player = Character.Instance.transform;
		RaycastHit hit;

		if(Vector3.Distance(transform.position,player.position) < maxRange)
		{
			if(Physics.Raycast(transform.position, (player.position - transform.position), out hit, maxRange))
			{
				//print (hit.transform);
				if(hit.transform == player)
				{
					clearLOS = true;
				}
				else
					clearLOS = false;
			}
			else
				clearLOS = false;
		}
		else
			clearLOS = false;

		//print (clearLOS);
	}

    void Update()
    {
		// Check Line-of-Sight to the player
		CheckLOS();

        // Track turret
        Track();

        // Fire turret
		if (!AlarmManager.Instance.PlayerDead && AlarmManager.Instance.AlarmActive && clearLOS && Time.time > fireTime + 1.3f)
        {
			fireTime = Time.time;
            isFiring = true;
			f3dxC.Fire();
        }

        // Stop firing
		if (isFiring && (AlarmManager.Instance.PlayerDead || !clearLOS))
        {
            isFiring = false;
			f3dxC.Stop();
        }
    }
}
