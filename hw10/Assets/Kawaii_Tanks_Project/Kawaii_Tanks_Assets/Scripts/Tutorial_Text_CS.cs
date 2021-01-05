using UnityEngine;
using System.Collections;

namespace ChobiAssets.KTP
{
	
	public class Tutorial_Text_CS : MonoBehaviour
	{
		#if UNITY_ANDROID || UNITY_IPHONE
		void Awake ()
		{
			Destroy (this.gameObject);
		}
		#endif
	}

}
