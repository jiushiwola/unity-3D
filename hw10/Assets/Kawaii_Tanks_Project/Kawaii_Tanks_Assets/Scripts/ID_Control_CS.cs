using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

// This script must be attached to the top object of the tank.
namespace ChobiAssets.KTP
{

	public class ID_Control_CS : MonoBehaviour
	{

		[Header ("ID settings")]
		[Tooltip ("ID number")] public int id = 0;

		[HideInInspector] public bool isPlayer; // Referred to from child objects.
		[HideInInspector] public Game_Controller_CS controllerScript;
		[HideInInspector] public TankProp storedTankProp; // Set by "Game_Controller_CS".

		[HideInInspector] public Turret_Control_CS turretScript;
		[HideInInspector] public Camera_Zoom_CS mainCamScript;
		[HideInInspector] public GunCamera_Control_CS gunCamScript;


		void Start ()
		{ // Do not change to "Awake ()".
			// Send this reference to the "Game_Controller" in the scene.
			GameObject gameController = GameObject.FindGameObjectWithTag ("GameController");
			if (gameController) {
				controllerScript = gameController.GetComponent <Game_Controller_CS> ();
			}
			if (controllerScript) {
				controllerScript.Receive_ID (this);
			} else {
				Debug.LogError ("There is no 'Game_Controller' in the scene.");
			}
			// Broadcast this reference.
			BroadcastMessage ("Get_ID_Script", this, SendMessageOptions.DontRequireReceiver);
		}

		#if !UNITY_ANDROID && !UNITY_IPHONE
		[HideInInspector] public bool aimButton;
		[HideInInspector] public bool aimButtonDown;
		[HideInInspector] public bool aimButtonUp;
		[HideInInspector] public bool dragButton;
		[HideInInspector] public bool dragButtonDown;
		[HideInInspector] public bool fireButton;
		void Update ()
		{
			if (isPlayer) {
				aimButton = Input.GetKey (KeyCode.Space);
				aimButtonDown = Input.GetKeyDown (KeyCode.Space);
				aimButtonUp = Input.GetKeyUp (KeyCode.Space);
				dragButton = Input.GetMouseButton (1);
				dragButtonDown = Input.GetMouseButtonDown (1);
				fireButton = Input.GetMouseButton (0);
			}
		}
		#endif

		void Destroy ()
		{ // Called from "Damage_Control_CS".
			gameObject.tag = "Finish";
		}

		public void Get_Current_ID (int currentID)
		{ // Called from "Game_Controller_CS".
			if (id == currentID) {
				isPlayer = true;
			} else {
				isPlayer = false;
			}
			// Call Switch_Player.
			turretScript.Switch_Player (isPlayer);
			mainCamScript.Switch_Player (isPlayer);
			gunCamScript.Switch_Player (isPlayer);
		}

	}

}
