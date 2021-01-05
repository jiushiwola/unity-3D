using UnityEngine;
using System.Collections;
using UnityEngine.UI;

#if UNITY_ANDROID || UNITY_IPHONE
using UnityStandardAssets.CrossPlatformInput;
#endif

namespace ChobiAssets.KTP
{
	
	public class Damage_Control_CS : MonoBehaviour
	{
		[Header ("Damage settings")]
		[Tooltip ("Durability of this tank.")] public float durability = 300.0f;
		[Tooltip ("Prefab used for destroyed effects.")] public GameObject destroyedPrefab;
		[Tooltip ("Prefab of Damage Text.")] public GameObject textPrefab;
		[Tooltip ("Name of the Canvas used for Damage Text.")] public string canvasName = "Canvas_Texts";

		Transform bodyTransform;
		Damage_Display_CS displayScript;
		float initialDurability;

		ID_Control_CS idScript;

		void Start ()
		{ // Do not change to "Awake()".
			initialDurability = durability;
		}

		void Set_DamageText ()
		{
			if (textPrefab == null || string.IsNullOrEmpty (canvasName) || durability == Mathf.Infinity) {
				return;
			}
			// Instantiate Damage Text, and set it to the Canvas.
			GameObject textObject = Instantiate (textPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			displayScript = textObject.GetComponent <Damage_Display_CS> ();
			displayScript.targetTransform = bodyTransform;
			GameObject canvasObject = GameObject.Find (canvasName);
			if (canvasObject) {
				displayScript.transform.SetParent (canvasObject.transform);
				displayScript.transform.localScale = Vector3.one;
			} else {
				Debug.LogWarning ("Canvas for Damage Text cannot be found.");
			}
		}

		void Update ()
		{
			if(idScript.isPlayer)
            {
                displayScript.Get_Damage(durability, initialDurability);
            }
			// Destruct
			if (idScript.isPlayer) {
				#if UNITY_ANDROID || UNITY_IPHONE
				if (CrossPlatformInputManager.GetButtonDown ("Destruct")) {
				#else
				if (Input.GetKeyDown (KeyCode.Return)) {
				#endif
					Start_Destroying ();
				}
			}
		}

		public void Get_Damage (float damageValue)
		{ // Called from "Bullet_Nav_CS".
			durability -= damageValue;
			if (durability > 0.0f) { // Still alive.
				// Display Text
				if (displayScript) {
					displayScript.Get_Damage (durability, initialDurability);
				}
			} else { // Dead
				Start_Destroying ();
			}
		}

		void Start_Destroying ()
		{
			// Send message to all the parts.
			BroadcastMessage ("Destroy", SendMessageOptions.DontRequireReceiver);
			// Create destroyedPrefab.
			if (destroyedPrefab) {
				GameObject tempObject = Instantiate (destroyedPrefab, bodyTransform.position, Quaternion.identity) as GameObject;
				tempObject.transform.parent = bodyTransform;
			}
			// Remove the Damage text.
			if (displayScript) {
				Destroy (displayScript.gameObject);
			}
			// Destroy this script.
			Destroy (this);
		}

		void Get_ID_Script (ID_Control_CS tempScript)
		{
			idScript = tempScript;
			bodyTransform = idScript.storedTankProp.bodyTransform;
			Set_DamageText ();
		}

		void Pause (bool isPaused)
		{ // Called from "Game_Controller_CS".
			this.enabled = !isPaused;
		}

	}

}