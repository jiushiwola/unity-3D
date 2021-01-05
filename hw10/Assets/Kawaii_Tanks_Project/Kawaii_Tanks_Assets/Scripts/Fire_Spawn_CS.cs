using UnityEngine;
using System.Collections;

// This script must be attached to "Fire_Point".
namespace ChobiAssets.KTP
{
	
	public class Fire_Spawn_CS : MonoBehaviour
	{

		[ Header ("Firing settings")]
		[ Tooltip ("Prefab of muzzle fire.")] public GameObject firePrefab;
		[ Tooltip ("Prefab of bullet.")] public GameObject bulletPrefab;
		[ Tooltip ("Attack force of the bullet.")] public float attackForce = 100.0f;
		[ Tooltip ("Speed of the bullet. (Meter per Second)")] public float bulletVelocity = 250.0f;
		[ Tooltip ("Offset distance for spawning the bullet. (Meter)")] public float spawnOffset = 1.0f;

		Transform thisTransform;

		void Awake ()
		{
			thisTransform = this.transform;
		}

		public IEnumerator Fire ()
		{
			// Muzzle Fire
			if (firePrefab) {
				GameObject fireObject = Instantiate (firePrefab, thisTransform.position, thisTransform.rotation) as GameObject;
				fireObject.transform.parent = thisTransform;
			}
			// Shot Bullet
			if (bulletPrefab) {
				GameObject bulletObject = Instantiate (bulletPrefab, thisTransform.position + thisTransform.forward * spawnOffset, thisTransform.rotation) as GameObject;
				bulletObject.GetComponent <Bullet_Nav_CS> ().attackForce = attackForce;
				Vector3 tempVelocity = thisTransform.forward * bulletVelocity;
				// Shoot
				yield return new WaitForFixedUpdate ();
				bulletObject.GetComponent <Rigidbody> ().velocity = tempVelocity;
			}
		}

	}

}
