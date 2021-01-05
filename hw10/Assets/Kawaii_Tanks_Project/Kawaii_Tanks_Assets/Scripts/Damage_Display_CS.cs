using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ChobiAssets.KTP
{

	public class Damage_Display_CS : MonoBehaviour
	{
		
		[Header ("Display settings")]
		[Tooltip ("Upper offset for displaying the value.")] public float offset = 256.0f;
		[Tooltip ("Displaying time.")] public float time = 1.5f;

		Transform thisTransform;
		Text thisText;

		// Set by "Damage_Control_CS".
		[HideInInspector] public Transform targetTransform ;

		void Awake ()
		{
			thisTransform = GetComponent <Transform> ();
			thisText = GetComponent <Text> ();
			thisText.enabled = false;
		}

		public void Get_Damage (float durability, float initialDurability)
		{ // Called from "Damage_Control_CS".
			thisText.text = Mathf.Ceil (durability) + "/" + initialDurability;
			StartCoroutine ("Display");
		}

		int displayingCount;
		IEnumerator Display ()
		{
			float count = 0.0f;
			displayingCount += 1;
			int myNum = displayingCount;
			Color currentColor = thisText.color;
			while (count < time) {
				if (myNum < displayingCount) {
					yield break;
				}
				if (targetTransform) {
					Set_Position ();
				} else {
					break;
				}
				currentColor.a = Mathf.Lerp (1.0f, 0.0f, count / time);
				thisText.color = currentColor;
				count += Time.deltaTime;
				yield return null;
			}
			displayingCount = 0;
			thisText.enabled = false;
		}

		void Set_Position ()
		{
			float lodValue = 2.0f * Vector3.Distance (targetTransform.position, Camera.main.transform.position) * Mathf.Tan (Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
			Vector3 currentPos = Camera.main.WorldToScreenPoint (targetTransform.position);
			if (currentPos.z > 0.0f) { // In front of the camera.
				thisText.enabled = true;
				currentPos.z = 100.0f;
				currentPos.y += (5.0f / lodValue) * offset;
				thisTransform.position = currentPos;
				thisTransform.localScale = Vector3.one;
			} else { // Behind of the camera.
				thisText.enabled = false;
			}
		}

	}

}