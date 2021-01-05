using UnityEngine;
using System.Collections;
using UnityEngine.UI;

#if UNITY_ANDROID || UNITY_IPHONE
using UnityStandardAssets.CrossPlatformInput;
#endif

// This script must be attached to "Cannon_Base".
namespace ChobiAssets.KTP
{
	
	public class Fire_Control_CS : MonoBehaviour
	{
		float count = 4.0f;
		[Header ("Fire control settings")]
		[Tooltip ("Loading time. (Sec)")] public float reloadTime = 4.0f;
		[Tooltip ("Recoil force with firing.")] public float recoilForce = 5000.0f;

		bool isReady = true;
		Transform thisTransform;
		Rigidbody bodyRigidbody;

		ID_Control_CS idScript;
		Barrel_Control_CS[] barrelScripts;
		Fire_Spawn_CS[] fireScripts;


		void Awake ()
		{
			thisTransform = this.transform;
			barrelScripts = GetComponentsInChildren <Barrel_Control_CS> ();
			fireScripts = GetComponentsInChildren <Fire_Spawn_CS> ();
		}

		void Update ()
		{
			if (idScript.isPlayer) {
				#if UNITY_ANDROID || UNITY_IPHONE
				Mobile_Input ();
				#else
				Desktop_Input ();
				#endif
			}
			else
            {
                count -= Time.deltaTime;
                if(count <= 0)
                {
                    Fire();
                    count = 4.0f;
                }
            }
		}

		#if UNITY_ANDROID || UNITY_IPHONE
		void Mobile_Input ()
		{
			if (CrossPlatformInputManager.GetButtonUp ("GunCam_Press") && isReady) {
				Fire ();
			}
		}
		#else

		void Desktop_Input ()
		{
			if (idScript.fireButton && isReady) {
				Fire ();
			}
		}
		#endif

		void Fire ()
		{
			// Call barrelScripts and fireScripts to fire.
			for (int i = 0; i < barrelScripts.Length; i++) {
				barrelScripts [i].Fire ();
			}
			for (int i = 0; i < fireScripts.Length; i++) {
				fireScripts [i].StartCoroutine ("Fire");
			}
			// Add recoil shock.
			bodyRigidbody.AddForceAtPosition (-thisTransform.forward * recoilForce, thisTransform.position, ForceMode.Impulse);
			isReady = false;
			StartCoroutine ("Reload");
		}

		IEnumerator Reload ()
		{
			yield return new WaitForSeconds (reloadTime);
			isReady = true;
		}

		void Destroy ()
		{ // Called from "Damage_Control_CS".
			Destroy (this);
		}

		void Get_ID_Script (ID_Control_CS tempScript)
		{
			idScript = tempScript;
			bodyRigidbody = idScript.storedTankProp.bodyRigidbody;
		}

		void Pause (bool isPaused)
		{ // Called from "Game_Controller_CS".
			this.enabled = !isPaused;
		}

	}

}
