using UnityEngine;
using System.Collections;

// This script must be attached to "Barrel_Base".
namespace ChobiAssets.KTP
{
	
	public class Barrel_Control_CS : MonoBehaviour
	{
		[ Header ("Recoil Brake settings")]
		[ Tooltip ("Time it takes to push back the barrel. (Sec)")] public float recoilTime = 0.2f;
		[ Tooltip ("Time it takes to to return the barrel. (Sec)")] public float returnTime = 1.0f;
		[ Tooltip ("Movable length for the recoil brake. (Meter)")] public float length = 0.3f;

		Transform thisTransform;
		bool isReady = true;
		Vector3 initialPos;
		const float HALF_PI = Mathf.PI * 0.5f;

		void Awake ()
		{
			thisTransform = this.transform;
			initialPos = thisTransform.localPosition;
		}

		IEnumerator Recoil_Brake ()
		{
			// Move backward.
			float count = 0.0f;
			while (count < recoilTime) {
				float rate = Mathf.Sin (HALF_PI * (count / recoilTime));
				thisTransform.localPosition = new Vector3 (initialPos.x, initialPos.y, initialPos.z - (rate * length));
				count += Time.deltaTime;
				yield return null;
			}
			// Return to the initial position.
			count = 0.0f;
			while (count < returnTime) {
				float rate = Mathf.Sin (HALF_PI * (count / returnTime) + HALF_PI);
				thisTransform.localPosition = new Vector3 (initialPos.x, initialPos.y, initialPos.z - (rate * length));
				count += Time.deltaTime;
				yield return null;
			}
			//
			isReady = true;
		}

		public void Fire ()
		{ // Called from "Fire_Control_CS".
			if (isReady) {
				isReady = false;
				StartCoroutine ("Recoil_Brake");
			}
		}
	}

}
