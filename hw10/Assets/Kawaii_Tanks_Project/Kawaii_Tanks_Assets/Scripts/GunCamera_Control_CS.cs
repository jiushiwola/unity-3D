using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ChobiAssets.KTP
{
	
	public class GunCamera_Control_CS : MonoBehaviour
	{

		[ Header ("Gun Camera settings")]
		[ Tooltip ("Main Camera of this tank.")] public Camera mainCamera;
		[ Tooltip ("Name of Image for Reticle.")] public string reticleName = "Reticle";

		Camera thisCamera;
		Image reticleImage;

		void Awake ()
		{
			this.tag = "MainCamera";
			thisCamera = GetComponent <Camera> ();
			thisCamera.enabled = false;
			if (mainCamera == null) {
				Debug.LogError ("'Main Camera is not assigned in the Gun_Camera.");
				Destroy (this);
			}
			Find_Image ();
		}

		void Find_Image ()
		{
			// Find Reticle Image.
			if (string.IsNullOrEmpty (reticleName) == false) {
				GameObject reticleObject = GameObject.Find (reticleName);
				if (reticleObject) {
					reticleImage = reticleObject.GetComponent <Image> ();
				}
			}
			if (reticleImage == null) {
				Debug.LogWarning (reticleName + " (Image for Reticle) cannot be found in the scene.");
			}
		}

		public void GunCam_On ()
		{ // Called from "Turret_Control_CS".
			mainCamera.enabled = false;
			thisCamera.enabled = true;
			if (reticleImage) {
				reticleImage.enabled = true;
			}
		}

		public void GunCam_Off ()
		{ // Called from "Turret_Control_CS".
			thisCamera.enabled = false;
			mainCamera.enabled = true;
			if (reticleImage) {
				reticleImage.enabled = false;
			}
		}

		void Get_ID_Script (ID_Control_CS tempScript)
		{
			tempScript.gunCamScript = this;
		}

		public void Switch_Player (bool isPlayer)
		{
			thisCamera.enabled = false;
			if (reticleImage) {
				reticleImage.enabled = false;
			}
		}
	}

}
