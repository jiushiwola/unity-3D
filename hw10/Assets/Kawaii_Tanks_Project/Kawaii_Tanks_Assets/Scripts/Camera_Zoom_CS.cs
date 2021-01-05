using UnityEngine;
using System.Collections;

#if UNITY_ANDROID || UNITY_IPHONE
using UnityStandardAssets.CrossPlatformInput;
#endif

// This script must be attached to "Main Camera".
// (Note.) Main Camera must be placed on X Aixs of "Camera_Pivot".
namespace ChobiAssets.KTP
{
	
	public class Camera_Zoom_CS : MonoBehaviour
	{

		Transform thisTransform;
		Transform parentTransform;
		Transform rootTransform;
		Camera thisCamera;
		AudioListener thisAudioListener;
		float posX;
		float targetPosX;
		int layerMask = ~((1 << 10) + (1 << 2)); // Layer 2 = Ignore Ray, Layer 10 = Ignore All.
		float storedPosX;
		bool autoZoomFlag;
		float hitCount;

		#if UNITY_ANDROID || UNITY_IPHONE
		bool isButtonDown = false;
		Vector2 previousMousePos;
		int fingerID;
		#endif

		public float speed = 30.0f;

		ID_Control_CS idScript;

		void Awake ()
		{
			this.tag = "MainCamera";
			thisCamera = GetComponent < Camera > ();
			thisCamera.enabled = false;
			thisAudioListener = GetComponent < AudioListener > ();
			thisAudioListener.enabled = false;
			thisTransform = transform;
			parentTransform = thisTransform.parent;
			rootTransform = thisTransform.root;
			posX = transform.localPosition.x;
			targetPosX = posX;
		}

		void Update ()
		{
			if (idScript.isPlayer) {
				#if UNITY_ANDROID || UNITY_IPHONE
				if (CrossPlatformInputManager.GetButtonDown ("Zoom_Press")) {
					isButtonDown = true ;
				#if UNITY_EDITOR
					previousMousePos = Input.mousePosition;
				#else
					fingerID = Input.touches.Length - 1;
					previousMousePos = Input.touches [fingerID].position;
				#endif
					return;
				}
				if (isButtonDown && CrossPlatformInputManager.GetButton ("Zoom_Press")) {
				#if UNITY_EDITOR
					Vector3 currentMousePos = Input.mousePosition;
				#else
					Vector3 currentMousePos = Input.touches [fingerID].position;
				#endif
					float vertical = (currentMousePos.y - previousMousePos.y);
					targetPosX += vertical * 0.1f;
					targetPosX = Mathf.Clamp (targetPosX, 3.0f, 20.0f);
					previousMousePos = currentMousePos ;
				} else if (CrossPlatformInputManager.GetButtonUp ("Zoom_Press")) {
					isButtonDown = false;
				}
				#else
				float axis = Input.GetAxis ("Mouse ScrollWheel");
				if (axis != 0) {
					#if UNITY_WEBGL
					targetPosX -= axis * 10.0f;
					#else
					targetPosX -= axis * 30.0f;
					#endif
					targetPosX = Mathf.Clamp (targetPosX, 3.0f, 20.0f);
				}
				#endif
				if (posX != targetPosX) {
					posX = Mathf.MoveTowards (posX, targetPosX, speed * Time.deltaTime);
					thisTransform.localPosition = new Vector3 (posX, thisTransform.localPosition.y, thisTransform.localPosition.z);
				} else {
					Cast_Ray ();
				}
			}
		}

		void Cast_Ray () {
			Ray ray = new Ray (parentTransform.position, thisTransform.position - parentTransform.position);
			RaycastHit[] raycastHits;
			raycastHits = Physics.SphereCastAll (ray, 0.5f, thisTransform.localPosition.x + 1.0f, layerMask);
			foreach (RaycastHit raycastHit in raycastHits) {
				if (raycastHit.transform.root != rootTransform) { // not itself.
					hitCount += Time.deltaTime;
					if (hitCount > 0.5f) {
						hitCount = 0.0f;
						if (autoZoomFlag == false) {
							autoZoomFlag = true;
							storedPosX = posX;
							targetPosX = raycastHit.distance;
							targetPosX = Mathf.Clamp (targetPosX, 3.0f, 20.0f);
						} else {
							if (targetPosX > raycastHit.distance) {
								targetPosX = raycastHit.distance;
								targetPosX = Mathf.Clamp (targetPosX, 3.0f, 20.0f);
							}
						}
					}
					return;
				}
			}
			hitCount = 0.0f;
			if (autoZoomFlag) {
				autoZoomFlag = false;
				targetPosX = storedPosX;
			}
		}

		void Get_ID_Script (ID_Control_CS tempScript)
		{
			idScript = tempScript;
			if (idScript.isPlayer) {
				thisAudioListener.enabled = true;
				thisCamera.enabled = true;
			}
			idScript.mainCamScript = this;
		}

		void Pause (bool isPaused)
		{ // Called from "Game_Controller_CS".
			this.enabled = !isPaused;
		}

		public void Switch_Player (bool isPlayer)
		{
			thisAudioListener.enabled = isPlayer;
			thisCamera.enabled = isPlayer;
		}

	}

}
