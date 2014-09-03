using UnityEngine;
using System.Collections;

/**
 * note this method requires that the formerly 
*/

namespace PinchRadialMenu
{
		public class FingerCursor : MonoBehaviour
		{
				public GameObject sprite;
				public Camera camera;
				public Camera menuCamera;
				public HandController handController;
				public Vector3 cursorOffset = new Vector3 (0, 0, 0);
				public float prePowScale = 0.5f;
				public float ScaleX = 2.0f;
				public float ScalePositiveY = 3.0f;
				public float ScaleNegativeY = 2.5f;
				public float cursorPow = 1.5f;

				// Use this for initialization
				void Start ()
				{
						if (sprite == null)
								sprite = transform.Find ("cursor").gameObject;

						if (camera == null)
								camera = Camera.main;
				}
	
				// Update is called once per frame
				void Update ()
				{
						FingerModel index = GetIndex ();
						sprite.SetActive (index != null);
						if (index != null) {
								Vector3 v = (index.GetTipPosition () + cursorOffset) * prePowScale;
								v.z = 0;
								v.x = Mathf.Pow (Mathf.Abs (v.x), cursorPow) * (v.x < 0 ? -1 : 1) * ScaleX;
								v.y = Mathf.Pow (Mathf.Abs (v.y), cursorPow) * (v.y < 0 ? -1 : 1) * ((v.y > 0) ? ScalePositiveY : ScaleNegativeY);
								sprite.transform.position = v;
						} else {
								Debug.Log ("No Index Finger");
						}
				}

				FingerModel GetIndex ()
				{
						if (handController.hand_graphics_.Count < 1)
								return null;

						foreach (HandModel hh in handController.hand_graphics_.Values) {
								return hh.fingers [1];
						}
						// returning an arbitrary hand's index finger
			
						return null;
				}
		}
}