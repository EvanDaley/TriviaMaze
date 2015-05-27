using UnityEngine;
using System.Collections;
using System;

// Weapon types
public enum F3DFXType
{
    Vulcan,
    SoloGun,
    Sniper,
    ShotGun,
    Seeker,
    RailGun,
    PlasmaGun,
    PlasmaBeam,
    PlasmaBeamHeavy,
    LightningGun,
    FlameRed,
    LaserImpulse
}

public class F3DFXController : MonoBehaviour
{
    // Singleton instance
    //public static F3DFXController instance;
	
    // Current firing socket
    int curSocket;          
    // Timer reference                
    int timerID = -1;

    [Header("Turret setup")]
    public Transform[] TurretSocket;            
    public ParticleSystem[] ShellParticles;     
   
    public F3DFXType DefaultFXType;             
    
    [Header("Vulcan")]
    public Transform vulcanProjectile;
    public Transform vulcanMuzzle;
    public Transform vulcanImpact;

    [Header("Solo gun")]
    public Transform soloGunProjectile;
    public Transform soloGunMuzzle;
    public Transform soloGunImpact;

    [Header("Sniper")]
    public Transform sniperBeam;
    public Transform sniperMuzzle;
    public Transform sniperImpact;

    [Header("Shotgun")]
    public Transform shotGunProjectile;
    public Transform shotGunMuzzle;
    public Transform shotGunImpact;

    [Header("Seeker")]
    public Transform seekerProjectile;
    public Transform seekerMuzzle;
    public Transform seekerImpact;

    [Header("Rail gun")]
    public Transform railgunBeam;
    public Transform railgunMuzzle;
    public Transform railgunImpact;

    [Header("Plasma gun")]
    public Transform plasmagunProjectile;
    public Transform plasmagunMuzzle;
    public Transform plasmagunImpact;

    [Header("Plasma beam")]
    public Transform plasmaBeam;

    [Header("Plasma beam heavy")]
    public Transform plasmaBeamHeavy;

    [Header("Lightning gun")]
    public Transform lightningGunBeam;

    [Header("Flame")]
    public Transform flameRed;

    [Header("Laser impulse")]
    public Transform laserImpulseProjectile;
    public Transform laserImpulseMuzzle;
    public Transform laserImpulseImpact;

    void Awake()
    {
        // Initialize singleton  
        //instance = this;

        // Initialize bullet shells particles
        for (int i = 0; i < ShellParticles.Length; i++)
        {
            ShellParticles[i].enableEmission = false;
            ShellParticles[i].gameObject.SetActive(true);
        }
    }

    // Switch to next weapon type
    void NextWeapon()
    {
        if ((int)DefaultFXType < Enum.GetNames(typeof(F3DFXType)).Length - 1)
        {
            Stop();
            DefaultFXType++;
        }        
    }

    // Switch to previous weapon type
    void PrevWeapon()
    {
        if (DefaultFXType > 0)
        {
            Stop();
            DefaultFXType--;
        }
    }

    // Advance to next turret socket
    void AdvanceSocket()
    {
        curSocket++;
        if (curSocket > 3)
            curSocket = 0;
    }

    // Fire turret weapon
    public void Fire()
    {
        switch (DefaultFXType)
        {
            case F3DFXType.Vulcan:
                // Fire vulcan at specified rate until canceled
                timerID = F3DTime.time.AddTimer(0.05f, Vulcan);
                // Invoke manually before the timer ticked to avoid initial delay
                Vulcan();
                break;
            
            case F3DFXType.SoloGun:
                timerID = F3DTime.time.AddTimer(0.2f, SoloGun);
                SoloGun();
                break;

            case F3DFXType.Sniper:
                timerID = F3DTime.time.AddTimer(0.3f, Sniper);
                Sniper();
                break;
           
            case F3DFXType.ShotGun:
                timerID = F3DTime.time.AddTimer(0.3f, ShotGun);
                ShotGun();
                break;
            
            case F3DFXType.Seeker:
                timerID = F3DTime.time.AddTimer(0.2f, Seeker);
                Seeker();
                break;
           
            case F3DFXType.RailGun:
                timerID = F3DTime.time.AddTimer(0.2f, RailGun);
                RailGun();
                break;
            
            case F3DFXType.PlasmaGun:
                timerID = F3DTime.time.AddTimer(0.2f, PlasmaGun);
                PlasmaGun();
                break;
           
            case F3DFXType.PlasmaBeam:   
                // Beams has no timer requirement
                PlasmaBeam();
                break;
            
            case F3DFXType.PlasmaBeamHeavy:
                // Beams has no timer requirement
                PlasmaBeamHeavy();
                break;
           
            case F3DFXType.LightningGun:
                // Beams has no timer requirement
                LightningGun();
                break;
            
            case F3DFXType.FlameRed:
                // Flames has no timer requirement
                FlameRed();
                break;
           
            case F3DFXType.LaserImpulse:
                timerID = F3DTime.time.AddTimer(0.15f, LaserImpulse);
                LaserImpulse();
                break;

            default:
                break;
        }
    }

    // Stop firing 
    public void Stop()
    {
        // Remove firing timer
        if (timerID != -1)
        {
            F3DTime.time.RemoveTimer(timerID);
            timerID = -1;
        }
    }

    // Fire vulcan weapon
    void Vulcan()
    {
        // Get random rotation that offset spawned projectile
        Quaternion offset = Quaternion.Euler(UnityEngine.Random.onUnitSphere);

        // Spawn muzzle flash and projectile with the rotation offset at current socket position
        F3DPool.instance.Spawn(vulcanMuzzle, TurretSocket[curSocket].position, TurretSocket[curSocket].rotation, TurretSocket[curSocket]);
		F3DPool.instance.Spawn(vulcanProjectile, TurretSocket[curSocket].position + TurretSocket[curSocket].forward, offset * TurretSocket[curSocket].rotation, TurretSocket[curSocket]);

        // Emit one bullet shell
        ShellParticles[curSocket].Emit(1);

        // Play shot sound effect
        F3DAudioController.instance.VulcanShot(TurretSocket[curSocket].position);

        // Advance to next turret socket
        AdvanceSocket();
    }

    // Spawn vulcan weapon impact
    public void VulcanImpact(Vector3 pos)
    {
        // Spawn impact prefab at specified position
		F3DPool.instance.Spawn(vulcanImpact, pos, Quaternion.identity, TurretSocket[curSocket]);
        // Play impact sound effect
        F3DAudioController.instance.VulcanHit(pos);
    }

    // Fire sologun weapon
    void SoloGun()
    {
        Quaternion offset = Quaternion.Euler(UnityEngine.Random.onUnitSphere);

        F3DPool.instance.Spawn(soloGunMuzzle, TurretSocket[curSocket].position, TurretSocket[curSocket].rotation, TurretSocket[curSocket]);
		F3DPool.instance.Spawn(soloGunProjectile, TurretSocket[curSocket].position + TurretSocket[curSocket].forward, offset * TurretSocket[curSocket].rotation, TurretSocket[curSocket]);
        
        F3DAudioController.instance.SoloGunShot(TurretSocket[curSocket].position);

        AdvanceSocket();
    }

    // Spawn sologun weapon impact
    public void SoloGunImpact(Vector3 pos)
    {
		F3DPool.instance.Spawn(soloGunImpact, pos, Quaternion.identity, TurretSocket[curSocket]);
        F3DAudioController.instance.SoloGunHit(pos);
    }

    // Fire sniper weapon
    void Sniper()
    {
        Quaternion offset = Quaternion.Euler(UnityEngine.Random.onUnitSphere);

        F3DPool.instance.Spawn(sniperMuzzle, TurretSocket[curSocket].position, TurretSocket[curSocket].rotation, TurretSocket[curSocket]);
		F3DPool.instance.Spawn(sniperBeam, TurretSocket[curSocket].position, offset * TurretSocket[curSocket].rotation, TurretSocket[curSocket]);

        F3DAudioController.instance.SniperShot(TurretSocket[curSocket].position);

        ShellParticles[curSocket].Emit(1);

        AdvanceSocket();
    }

    // Spawn sniper weapon impact
    public void SniperImpact(Vector3 pos)
    {
		F3DPool.instance.Spawn(sniperImpact, pos, Quaternion.identity, TurretSocket[curSocket]);
        F3DAudioController.instance.SniperHit(pos);
    }

    // Fire shotgun weapon
    void ShotGun()
    {
        Quaternion offset = Quaternion.Euler(UnityEngine.Random.onUnitSphere);

        F3DPool.instance.Spawn(shotGunMuzzle, TurretSocket[curSocket].position, TurretSocket[curSocket].rotation, TurretSocket[curSocket]);
		F3DPool.instance.Spawn(shotGunProjectile, TurretSocket[curSocket].position, offset * TurretSocket[curSocket].rotation, TurretSocket[curSocket]);

        F3DAudioController.instance.ShotGunShot(TurretSocket[curSocket].position);
        ShellParticles[curSocket].Emit(1);
        AdvanceSocket();
    }  

    // Fire seeker weapon
    void Seeker()
    {
        Quaternion offset = Quaternion.Euler(UnityEngine.Random.onUnitSphere);

        F3DPool.instance.Spawn(seekerMuzzle, TurretSocket[curSocket].position, TurretSocket[curSocket].rotation, TurretSocket[curSocket]);
		F3DPool.instance.Spawn(seekerProjectile, TurretSocket[curSocket].position, offset * TurretSocket[curSocket].rotation, TurretSocket[curSocket]);

        F3DAudioController.instance.SeekerShot(TurretSocket[curSocket].position);       
        AdvanceSocket();
    }

    // Spawn seeker weapon impact
    public void SeekerImpact(Vector3 pos)
    {
		F3DPool.instance.Spawn(seekerImpact, pos, Quaternion.identity, TurretSocket[curSocket]);
        F3DAudioController.instance.SeekerHit(pos);
    }

    // Fire rail gun weapon
    void RailGun()
    {
        Quaternion offset = Quaternion.Euler(UnityEngine.Random.onUnitSphere);

        F3DPool.instance.Spawn(railgunMuzzle, TurretSocket[curSocket].position, TurretSocket[curSocket].rotation, TurretSocket[curSocket]);
		F3DPool.instance.Spawn(railgunBeam, TurretSocket[curSocket].position, offset * TurretSocket[curSocket].rotation, TurretSocket[curSocket]);

        F3DAudioController.instance.RailGunShot(TurretSocket[curSocket].position);

        ShellParticles[curSocket].Emit(1);

        AdvanceSocket();
    }

    // Spawn rail gun weapon impact
    public void RailgunImpact(Vector3 pos)
    {
		F3DPool.instance.Spawn(railgunImpact, pos, Quaternion.identity, TurretSocket[curSocket]);

        F3DAudioController.instance.RailGunHit(pos);
    }

    // Fire plasma gun weapon
    void PlasmaGun()
    {
        Quaternion offset = Quaternion.Euler(UnityEngine.Random.onUnitSphere);

        F3DPool.instance.Spawn(plasmagunMuzzle, TurretSocket[curSocket].position, TurretSocket[curSocket].rotation, TurretSocket[curSocket]);
		F3DPool.instance.Spawn(plasmagunProjectile, TurretSocket[curSocket].position, offset * TurretSocket[curSocket].rotation, TurretSocket[curSocket]);

        F3DAudioController.instance.PlasmaGunShot(TurretSocket[curSocket].position);
        AdvanceSocket();
    }

    // Spawn plasma gun weapon impact
    public void PlasmaGunImpact(Vector3 pos)
    {
		F3DPool.instance.Spawn(plasmagunImpact, pos, Quaternion.identity, TurretSocket[curSocket]);
        F3DAudioController.instance.PlasmaGunHit(pos);
    }

    // Fire plasma beam weapon
    void PlasmaBeam()
    {
        F3DPool.instance.Spawn(plasmaBeam, TurretSocket[0].position, TurretSocket[0].rotation, TurretSocket[0]);
        F3DPool.instance.Spawn(plasmaBeam, TurretSocket[2].position, TurretSocket[2].rotation, TurretSocket[2]);
    }

    // Fire heavy beam weapon
    void PlasmaBeamHeavy()
    {
        F3DPool.instance.Spawn(plasmaBeamHeavy, TurretSocket[0].position, TurretSocket[0].rotation, TurretSocket[0]);
        F3DPool.instance.Spawn(plasmaBeamHeavy, TurretSocket[2].position, TurretSocket[2].rotation, TurretSocket[2]);
    }

    // Fire lightning gun weapon
    void LightningGun()
    {
        F3DPool.instance.Spawn(lightningGunBeam, TurretSocket[0].position, TurretSocket[0].rotation, TurretSocket[0]);
        F3DPool.instance.Spawn(lightningGunBeam, TurretSocket[2].position, TurretSocket[2].rotation, TurretSocket[2]);
    }

    // Fire flames weapon
    void FlameRed()
    {
        F3DPool.instance.Spawn(flameRed, TurretSocket[0].position, TurretSocket[0].rotation, TurretSocket[0]);
        F3DPool.instance.Spawn(flameRed, TurretSocket[2].position, TurretSocket[2].rotation, TurretSocket[2]);
    }

    // Fire laser pulse weapon
    void LaserImpulse()
    {
        Quaternion offset = Quaternion.Euler(UnityEngine.Random.onUnitSphere);

        F3DPool.instance.Spawn(laserImpulseMuzzle, TurretSocket[curSocket].position, TurretSocket[curSocket].rotation, TurretSocket[curSocket]);
		F3DPool.instance.Spawn(laserImpulseProjectile, TurretSocket[curSocket].position, offset * TurretSocket[curSocket].rotation, TurretSocket[curSocket]);

        F3DAudioController.instance.LaserImpulseShot(TurretSocket[curSocket].position);

        AdvanceSocket();
    }

    // Spawn laser pulse weapon impact
    public void LaserImpulseImpact(Vector3 pos)
    {
		F3DPool.instance.Spawn(laserImpulseImpact, pos, Quaternion.identity, TurretSocket[curSocket]);
        F3DAudioController.instance.LaserImpulseHit(pos);
    }
}
