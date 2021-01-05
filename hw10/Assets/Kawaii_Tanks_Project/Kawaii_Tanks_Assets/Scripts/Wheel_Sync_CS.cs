using UnityEngine;
using System.Collections;

// This script must be attached to all the Apparent Wheels.
namespace ChobiAssets.KTP
{
	
	public class Wheel_Sync_CS : MonoBehaviour
	{
		[ Header ("Wheel Synchronizing settings")]
		[ Tooltip ("Set the RoadWheel to synchronize with.")] public Transform referenceWheel;
		[ Tooltip ("Offset value for the size of this wheel.")] public float radiusOffset = 0.0f;

		Transform thisTransform;
		bool isLeft;
		float previousAng;
		float radiusRate;

		void Awake ()
		{
			thisTransform = transform;
			// Set direction.
			if (transform.localPosition.y > 0.0f) {
				isLeft = true;
			} else {
				isLeft = false;
			}
			// Check and Find referenceWheel.
			if (referenceWheel == null) {
				Find_Reference_Wheel ();
			}
			// Calculate radiusRate.
			MeshFilter referenceMeshFilter = referenceWheel.GetComponent <MeshFilter> ();
			if (referenceMeshFilter) {
				float thisRadius = GetComponent <MeshFilter> ().mesh.bounds.extents.z + radiusOffset;
				float referenceRadius = referenceMeshFilter.mesh.bounds.extents.z;
				if (referenceRadius > 0.0f && thisRadius > 0.0f) {
					radiusRate = referenceRadius / thisRadius;
				}
			}
		}

		void Find_Reference_Wheel ()
		{
			Track_Scroll_CS[] scrollScripts = thisTransform.parent.parent.GetComponentsInChildren <Track_Scroll_CS> ();
			foreach (Track_Scroll_CS scrollScript in scrollScripts) {
				if ((isLeft && scrollScript.referenceWheel.localPosition.y > 0.0f) || (isLeft == false && scrollScript.referenceWheel.localPosition.y < 0.0f)) {
					referenceWheel = scrollScript.referenceWheel;
					break;
				}
			}
			if (referenceWheel == null) {
				Debug.LogError ("Reference Wheel is not assigned in " + this.name);
				Destroy (this);
			}
		}

		void Update ()
		{
			if (referenceWheel) {
				float currentAng = referenceWheel.localEulerAngles.y;
				float deltaAng = Mathf.DeltaAngle (currentAng, previousAng);
				thisTransform.localEulerAngles = new Vector3 (thisTransform.localEulerAngles.x, thisTransform.localEulerAngles.y - (radiusRate * deltaAng), thisTransform.localEulerAngles.z);
				previousAng = currentAng;
			}
		}

		void Pause (bool isPaused)
		{ // Called from "Game_Controller_CS".
			this.enabled = !isPaused;
		}

	}

}
