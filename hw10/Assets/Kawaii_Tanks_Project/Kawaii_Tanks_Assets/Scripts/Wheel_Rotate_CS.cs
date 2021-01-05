using UnityEngine;
using System.Collections;

// This script must be attached to all the Driving Wheels.
namespace ChobiAssets.KTP
{
	
	public class Wheel_Rotate_CS : MonoBehaviour
	{

		bool isLeft;
		Rigidbody thisRigidbody;
		float maxAngVelocity;
		Wheel_Control_CS controlScript;
		Transform thisTransform;
		Transform parentTransform;
		Vector3 angles;

		void Awake ()
		{
			this.gameObject.layer = 9; // Layer9 >> for wheels.
			thisRigidbody = GetComponent <Rigidbody> ();
			// Set direction.
			if (transform.localPosition.y > 0.0f) {
				isLeft = true;
			} else {
				isLeft = false;
			}
			// Get initial rotation.
			thisTransform = transform;
			parentTransform = thisTransform.parent;
			angles = thisTransform.localEulerAngles;
			// Find controlScript.
			controlScript = parentTransform.parent.GetComponent <Wheel_Control_CS> ();
			// Set maxAngVelocity.
			float radius = GetComponent <SphereCollider> ().radius;
			maxAngVelocity = Mathf.Deg2Rad * ((controlScript.maxSpeed / (2.0f * Mathf.PI * radius)) * 360.0f);
		}

		/* for reducing Calls.
		public void FixedUpdate_Me ()
		*/
		void FixedUpdate ()
		{
			float rate;
			if (isLeft) {
				rate = controlScript.leftRate;
			} else {
				rate = controlScript.rightRate;
			}

			thisRigidbody.AddRelativeTorque (0.0f, Mathf.Sign (rate) * controlScript.wheelTorque, 0.0f);
			thisRigidbody.maxAngularVelocity = Mathf.Abs (maxAngVelocity * rate);
			// Stabilize angle.
			angles.y = thisTransform.localEulerAngles.y;
			thisRigidbody.rotation = parentTransform.rotation * Quaternion.Euler (angles);
		}

		void Destroy ()
		{ // Called from "Damage_Control_CS".
			thisRigidbody.angularDrag = Mathf.Infinity;
			Destroy (this);
		}

		void Pause (bool isPaused)
		{ // Called from "Game_Controller_CS".
			this.enabled = !isPaused;
		}

	}

}
