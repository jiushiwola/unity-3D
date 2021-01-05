using UnityEngine;
using System.Collections;

namespace ChobiAssets.KTP
{
	
	public class Break_Object_CS : MonoBehaviour
	{
		[ Header ("Broken object settings")]
		[ Tooltip ("Prefab of the broken object.")] public GameObject brokenPrefab;
		[ Tooltip ("Lag time for breaking. (Sec)")] public float lagTime = 1.0f;

		Transform thisTransform;

		void Awake ()
		{
			thisTransform = transform;
		}

		void OnJointBreak ()
		{
			StartCoroutine ("Broken");
		}

		void OnTriggerEnter (Collider collider)
		{
			if (collider.isTrigger == false) {
				StartCoroutine ("Broken");
			}
		}

		IEnumerator Broken ()
		{
			yield return new WaitForSeconds (lagTime);
			if (brokenPrefab) {
				Instantiate (brokenPrefab, thisTransform.position, thisTransform.rotation);
			}
			Destroy (gameObject);
		}

	}

}
