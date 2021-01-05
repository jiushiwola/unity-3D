using UnityEngine;
using System.Collections;

namespace ChobiAssets.KTP
{
	
	public class Armor_Collider_CS : MonoBehaviour
	{
		[ Header ("Armor settings")]
		[ Tooltip ("Multiplier for the damage.")] public float damageMultiplier = 1.0f;

		void Awake ()
		{
			// Make it a trigger and invisible.
			GetComponent < Collider > ().isTrigger = true;
			GetComponent < MeshRenderer > ().enabled = false;
		}

	}

}
