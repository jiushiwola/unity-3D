using UnityEngine;
using System.Collections;

namespace ChobiAssets.KTP
{
	public class Delete_Timer_CS : MonoBehaviour
	{

		[ Header ("Life time settings")]
		[ Tooltip ("Life time of this gameobject. (Sec)")] public float lifeTime = 2.0f;

		void Awake ()
		{
			Destroy (this.gameObject, lifeTime);
		}

	}

}
