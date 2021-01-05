using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// This script must be attached to Tracks.
namespace ChobiAssets.KTP
{
	
	[System.Serializable]
	public class IntArray
	{
		public int[] intArray;
		public IntArray (int[] newIntArray)
		{
			intArray = newIntArray;
		}
	}

	public class Track_Deform_CS : MonoBehaviour
	{
	
		public int anchorNum;
		public Transform[] anchorArray;
		public float[] widthArray;
		public float[] heightArray;
		public float[] offsetArray;

		Mesh thisMesh;
		public float[] initialPosArray;
		public Vector3[] initialVertices;
		public IntArray[] movableVerticesList;

		Vector3[] currentVertices;

		void Awake ()
		{
			thisMesh = GetComponent < MeshFilter > ().mesh;
			thisMesh.MarkDynamic ();
			// Check Anchor wheels.
			for (int i = 0; i < anchorArray.Length; i++) {
				if (anchorArray [i] == null) {
					Debug.LogError ("Anchor Wheel is not assigned in " + this.name);
					Destroy (this);
				}
			}
			// Check vertices list.
			if (initialPosArray == null || initialPosArray.Length == 0 || initialVertices == null || initialVertices.Length == 0 || movableVerticesList == null || movableVerticesList.Length == 0) {
				Set_Vertices ();
			}
			//
			currentVertices = new Vector3 [initialVertices.Length];
		}

		void Set_Vertices ()
		{ // for old version tanks.
			Debug.Log ("Vertices Lists are not prepared in the prefab.");
			initialPosArray = new float [anchorArray.Length];
			initialVertices = thisMesh.vertices;
			movableVerticesList = new IntArray [anchorArray.Length];
			// Get vertices in the range.
			for (int i = 0; i < anchorArray.Length; i++) {
				if (anchorArray [i] != null) {
					Transform anchorTransform = anchorArray [i];
					initialPosArray [i] = anchorTransform.localPosition.x;
					Vector3 anchorPos = transform.InverseTransformPoint (anchorTransform.position);
					List <int> withinVerticesList = new List <int> ();
					for (int j = 0; j < thisMesh.vertices.Length; j++) {
						float distZ = Mathf.Abs (anchorPos.z - thisMesh.vertices [j].z);
						float distY = Mathf.Abs (anchorPos.y - thisMesh.vertices [j].y);
						if (distZ <= widthArray [i] * 0.5f && distY <= heightArray [i] * 0.5f) {
							withinVerticesList.Add (j);
						}
					}
					IntArray withinVerticesArray = new IntArray (withinVerticesList.ToArray ());
					movableVerticesList [i] = withinVerticesArray;
				}
			}
		}

		void Update ()
		{
			initialVertices.CopyTo (currentVertices, 0);
			for (int i = 0; i < anchorArray.Length; i++) {
				float tempDist = anchorArray [i].localPosition.x - initialPosArray [i];
				for (int j = 0; j < movableVerticesList [i].intArray.Length; j++) {
					currentVertices [movableVerticesList [i].intArray [j]].y += tempDist;
				}
			}
			thisMesh.vertices = currentVertices;
		}

		void OnDrawGizmos ()
		{
			if (anchorArray != null && anchorArray.Length != 0 && offsetArray != null && offsetArray.Length != 0) {
				Gizmos.color = Color.green;
				for (int i = 0; i < anchorArray.Length; i++) {
					if (anchorArray [i] != null) {
						Vector3 tempSize = new Vector3 (0.0f, heightArray [i], widthArray [i]);
						Vector3 tempCenter = anchorArray [i].position;
						tempCenter.y += offsetArray [i];
						Gizmos.DrawWireCube (tempCenter, tempSize);
					}
				}
			}
		}

		void Pause (bool isPaused)
		{ // Called from "Game_Controller_CS".
			this.enabled = !isPaused;
		}

	}

}