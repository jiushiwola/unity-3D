using UnityEngine;
using System.Collections;

// This script must be attached to "Engine_Sound".
namespace ChobiAssets.KTP
{

	public class SE_Control_CS : MonoBehaviour
	{
		[ Header ("Engine Sound settings")]
		[ Tooltip ("Set the Left RoadWheel to synchronize with.")] public Rigidbody leftReferenceWheel;
		[ Tooltip ("Set the Left RoadWheel to synchronize with.")] public Rigidbody rightReferenceWheel;
		[ Tooltip ("Minimum Pitch")] public float minPitch = 1.0f;
		[ Tooltip ("Maximum Pitch")] public float maxPitch = 2.0f;
		[ Tooltip ("Minimum Volume")] public float minVolume = 0.1f;
		[ Tooltip ("Maximum Volume")] public float maxVolume = 0.3f;

		AudioSource thisAudioSource;
		float leftCircumference;
		float rightCircumference;
		float currentRate;
		const float DOUBLE_PI = Mathf.PI * 2.0f;
		float maxSpeed;

		void Awake ()
		{
			thisAudioSource = GetComponent <AudioSource> ();
			if (thisAudioSource == null) {
				Debug.LogError ("AudioSource is not attached to" + this.name);
				Destroy (this);
			}
			thisAudioSource.loop = true;
			thisAudioSource.volume = 0.0f;
			thisAudioSource.Play ();
			// Check and Find referenceWheel.
			if (leftReferenceWheel == null || rightReferenceWheel == null) {
				Find_Reference_Wheels ();
			}
			// Calculate Circumferences.
			leftCircumference = DOUBLE_PI * leftReferenceWheel.GetComponent <SphereCollider> ().radius;
			rightCircumference = DOUBLE_PI * rightReferenceWheel.GetComponent <SphereCollider> ().radius;
		}

		void Start ()
		{ // Do not change to "Awake".
			// Set maxSpeed.
			maxSpeed = transform.parent.GetComponent <Wheel_Control_CS> ().maxSpeed;
		}

		void Find_Reference_Wheels ()
		{
			Track_Scroll_CS[] scrollScripts = transform.parent.GetComponentsInChildren <Track_Scroll_CS> ();
			foreach (Track_Scroll_CS scrollScript in scrollScripts) {
				Rigidbody tempRigidbody = scrollScript.referenceWheel.GetComponent <Rigidbody> ();
				if ((tempRigidbody.transform.localPosition.y > 0.0f)) { // Left
					leftReferenceWheel = tempRigidbody;
				} else { // Right
					rightReferenceWheel = tempRigidbody;
				}
			}
			if (leftReferenceWheel == null || rightReferenceWheel == null) {
				Debug.LogError ("Reference Wheels are not assigned in the 'Engine_Sound'.");
				Destroy (this);
			}
		}

		void Update ()
		{
			if (leftReferenceWheel && rightReferenceWheel) {
				float leftVelocity;
				float rightVelocity;
				leftVelocity = leftReferenceWheel.angularVelocity.magnitude / DOUBLE_PI * leftCircumference;
				rightVelocity = rightReferenceWheel.angularVelocity.magnitude / DOUBLE_PI * rightCircumference;
				float targetRate = (leftVelocity + rightVelocity) / 2.0f / maxSpeed;
				currentRate = Mathf.MoveTowards (currentRate, targetRate, 0.02f);
				thisAudioSource.pitch = Mathf.Lerp (minPitch, maxPitch, currentRate);
				thisAudioSource.volume = Mathf.Lerp (minVolume, maxVolume, currentRate);
			}
		}

		void Destroy ()
		{ // Called from "Damage_Control_CS".
			Destroy (this.gameObject);
		}

		void Pause (bool isPaused)
		{ // Called from "Game_Controller_CS".
			this.enabled = !isPaused;
			if (isPaused) {
				thisAudioSource.volume = 0.0f;
			}
			
		}

	}

}
