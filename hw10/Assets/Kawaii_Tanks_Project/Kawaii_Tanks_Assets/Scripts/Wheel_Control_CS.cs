using UnityEngine;
using System.Collections;

#if UNITY_ANDROID || UNITY_IPHONE
using UnityStandardAssets.CrossPlatformInput;
#endif

// This script must be attached to "MainBody".
namespace ChobiAssets.KTP
{
	
	public class Wheel_Control_CS : MonoBehaviour
	{
		[ Header ("Driving settings")]
		[ Tooltip ("Torque added to each wheel.")] public float wheelTorque = 3000.0f; // Reference to "Wheel_Rotate".
		[ Tooltip ("Maximum Speed (Meter per Second)")] public float maxSpeed = 7.0f; // Reference to "Wheel_Rotate".
		[ Tooltip ("Rate for ease of turning."), Range (0.0f, 2.0f)] public float turnClamp = 0.8f;
		[ Tooltip ("'Solver Iteration Count' of all the rigidbodies in this tank.")] public int solverIterationCount = 7;

		// Reference to "Wheel_Rotate".
		[HideInInspector] public float leftRate;
		[HideInInspector] public float rightRate;

		Rigidbody thisRigidbody;
		bool isParkingBrake = false;
		float lagCount;
		float speedStep;
		float autoParkingBrakeVelocity = 0.5f;
		float autoParkingBrakeLag = 0.5f;

		ID_Control_CS idScript;

		/* for reducing Calls.
		Wheel_Rotate_CS[] rotateScripts;
		*/

		void Awake ()
		{
			this.gameObject.layer = 11; // Layer11 >> for MainBody.
			thisRigidbody = GetComponent < Rigidbody > ();
			thisRigidbody.solverIterations = solverIterationCount;
			/* for reducing Calls.
			rotateScripts = GetComponentsInChildren <Wheel_Rotate_CS> ();
			*/
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
		}

		#if UNITY_ANDROID || UNITY_IPHONE
		void Mobile_Input ()
		{
			if (CrossPlatformInputManager.GetButtonDown ("Up")) {
				speedStep += 0.5f;
				speedStep = Mathf.Clamp (speedStep, -1.0f, 1.0f);
			} else if (CrossPlatformInputManager.GetButtonDown ("Down")) {
				speedStep -= 0.5f;
				speedStep = Mathf.Clamp (speedStep, -1.0f, 1.0f);
			}
			float vertical = speedStep;
			float horizontal = 0.0f;
			if (CrossPlatformInputManager.GetButton ("Left")) {
				horizontal = Mathf.Lerp (-turnClamp, -1.0f, Mathf.Abs (vertical / 1.0f));
			} else if (CrossPlatformInputManager.GetButton ("Right")) {
				horizontal = Mathf.Lerp (turnClamp, 1.0f, Mathf.Abs (vertical / 1.0f));
			}
			if (vertical < 0.0f) {
				horizontal = -horizontal; // like a brake-turn.
			}
			leftRate = Mathf.Clamp (-vertical - horizontal, -1.0f, 1.0f);
			rightRate = Mathf.Clamp (vertical - horizontal, -1.0f, 1.0f);
		}
		#else

		void Desktop_Input ()
		{
			if (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.W)) {
				speedStep += 0.5f;
				speedStep = Mathf.Clamp (speedStep, -1.0f, 1.0f);
			} else if (Input.GetKeyDown (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.S)) {
				speedStep -= 0.5f;
				speedStep = Mathf.Clamp (speedStep, -1.0f, 1.0f);
			} else if (Input.GetKeyDown (KeyCode.X)) {
				speedStep = 0.0f;
			}
			float vertical = speedStep;
			float horizontal = Input.GetAxis ("Horizontal");
			float clamp = Mathf.Lerp (turnClamp, 1.0f, Mathf.Abs (vertical / 1.0f));
			horizontal = Mathf.Clamp (horizontal, -clamp, clamp);
			if (vertical < 0.0f) {
				horizontal = -horizontal; // like a brake-turn.
			}
			leftRate = Mathf.Clamp (-vertical - horizontal, -1.0f, 1.0f);
			rightRate = Mathf.Clamp (vertical - horizontal, -1.0f, 1.0f);
		}
		#endif

		void FixedUpdate ()
		{
			// Auto Parking Brake using 'RigidbodyConstraints'.
			if (leftRate == 0.0f && rightRate == 0.0f) {
				float velocityMag = thisRigidbody.velocity.magnitude;
				float angularVelocityMag = thisRigidbody.angularVelocity.magnitude;
				if (isParkingBrake == false) {
					if (velocityMag < autoParkingBrakeVelocity && angularVelocityMag < autoParkingBrakeVelocity) {
						lagCount += Time.fixedDeltaTime;
						if (lagCount > autoParkingBrakeLag) {
							isParkingBrake = true;
							thisRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY;
						}
					}
				} else {
					if (velocityMag > autoParkingBrakeVelocity || angularVelocityMag > autoParkingBrakeVelocity) {
						isParkingBrake = false;
						thisRigidbody.constraints = RigidbodyConstraints.None;
						lagCount = 0.0f;
					}
				}
			} else {
				isParkingBrake = false;
				thisRigidbody.constraints = RigidbodyConstraints.None;
				lagCount = 0.0f;
			}
			/* for reducing Calls.
			for (int i = 0; i < rotateScripts.Length; i++) {
				rotateScripts [i].FixedUpdate_Me ();
			}
			*/
		}

		void Destroy ()
		{ // Called from "Damage_Control_CS".
			StartCoroutine ("Disable_Constraints");
		}

		IEnumerator Disable_Constraints ()
		{
			// Disable constraints of MainBody's rigidbody.
			yield return new WaitForFixedUpdate (); // This wait is required for PhysX.
			thisRigidbody.constraints = RigidbodyConstraints.None;
			Destroy (this);
		}

		void Get_ID_Script (ID_Control_CS tempScript)
		{
			idScript = tempScript;
		}

		void Pause (bool isPaused)
		{ // Called from "Game_Controller_CS".
			this.enabled = !isPaused;
		}

	}

}
