using UnityEngine;
using System.Collections;
using Leap;

/**
 * note this method requires that the formerly 
*/

namespace PinchRadialMenu
{
		public class FingerCursor : MonoBehaviour
		{
				public GameObject sprite;
				public GameObject thumbSprite;
				public GameObject fingerSprite;
				public Camera camera;
				public Camera menuCamera;
				public HandController handController;
				public Vector3 cursorOffset = new Vector3 (0, 0, 0);
				public float prePowScale = 0.5f;
				public float ScaleX = 2.0f;
				public float ScalePositiveY = 3.0f;
				public float ScaleNegativeY = 2.5f;
				public float cursorPow = 1.5f;
				bool pinching = false;
				Vector3 cursorPosition = Vector3.zero;
				const float MIN_PINCH = 0.5f;
				const float MID_PINCH = 0.25f;
				public bool freezeOnPinch = true;
				public bool smoothCursor = true;
				public float cursorExp = 0.2f;

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
						FingerModel thumb = GetThumb ();

						sprite.SetActive (index != null);
						fingerSprite.SetActive (index != null);
						thumbSprite.SetActive (index != null);
			

						if (index != null && thumb != null) {
				
								if ((!freezeOnPinch) || (GetPinch () == 3)) {
										Vector3 midPosition = (index.GetTipPosition () + thumb.GetTipPosition ()) / 2;
					cursorPosition = (smoothCursor) ? Utils.ExponentialVectorSmoothing (midPosition, cursorPosition, cursorExp) : midPosition;
								}
								sprite.transform.position = TransformPosition (cursorPosition);
								sprite.GetComponent<CursorSprite> ().ani.SetInteger ("pinch", GetPinch ());

								fingerSprite.transform.position = TransformPosition (index.GetTipPosition ());
								thumbSprite.transform.position = TransformPosition (thumb.GetTipPosition ());
						} else {
								Debug.Log ("No Index Finger");
						}
				}

				Vector3 TransformPosition (Vector3 original)
				{
						Vector3 v = (original + cursorOffset) * prePowScale;
						v.z = 0;
						v.x = Mathf.Pow (Mathf.Abs (v.x), cursorPow) * (v.x < 0 ? -1 : 1) * ScaleX;
						v.y = Mathf.Pow (Mathf.Abs (v.y), cursorPow) * (v.y < 0 ? -1 : 1) * ((v.y > 0) ? ScalePositiveY : ScaleNegativeY);
						return v;
				}

				int GetPinch ()
				{
						if (handController.hand_graphics_.Count != 1)
								return 3;
			
						foreach (HandModel hh in handController.hand_graphics_.Values) {
								Hand h = hh.GetLeapHand ();
								if (h.PinchStrength > MIN_PINCH)
										return 1;
								if (h.PinchStrength > MID_PINCH)
										return 2;
								return 3;
						}
			
						return 3;
				}
		
				FingerModel GetIndex ()
				{
						HandModel h = GetHandModel ();
						if (h == null)
								return null;
						return GetIndex (h);
				}
		
				FingerModel GetIndex (HandModel hh)
				{
						return hh.fingers [1];
				}
		
				FingerModel GetThumb ()
				{
						HandModel h = GetHandModel ();
						if (h == null)
								return null;
						return GetThumb (h);
				}
		
				FingerModel GetThumb (HandModel hh)
				{
						return hh.fingers [0];
				}
		
				HandModel GetHandModel ()
				{
						if (handController.hand_graphics_.Count != 1)
								return null;
			
						foreach (HandModel hh in handController.hand_graphics_.Values) {
								return hh;
						}
			
						return null;
			
				}

		}
}