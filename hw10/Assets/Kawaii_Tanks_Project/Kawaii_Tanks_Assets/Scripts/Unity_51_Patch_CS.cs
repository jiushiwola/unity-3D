using UnityEngine;
using System.Collections;

namespace ChobiAssets.KTP
{
	
	public class Unity_51_Patch_CS : MonoBehaviour
	{
		void Awake ()
		{
			#if UNITY_5_1
		HingeJoint [] hingeJoints = GetComponentsInChildren < HingeJoint > () ;
		foreach ( HingeJoint hingeJoint in hingeJoints ) {
			JointSpring jointSpring = hingeJoint.spring ;
			jointSpring.targetPosition *= -1.0f ;
			hingeJoint.spring = jointSpring ;
		}
			#endif
			Destroy (this);
		}
	}

}