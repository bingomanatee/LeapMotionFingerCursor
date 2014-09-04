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
				enum PinchState
				{
						NoHands,
						Open,
						Part,
						Full
				}
				public GameObject curosrSprite;
				public GameObject immediateSprite; // shown when fully pinched. 
				Vector3 cursorPosition = Vector3.zero; // the visible cursor's position
				Vector3 immediatePosition = Vector3.zero; // the actual fingerpinch position
				public GameObject thumbSprite;
				public GameObject fingerSprite;
				public Camera camera;
				public HandController handController;

// positional transform variables
				public Vector3 cursorOffset = new Vector3 (0, 0, 0);
				public float prePowScale = 0.5f;
				public float ScaleX = 2.0f;
// because of gravity it is harder to lift your finger than to drop it 
// so a higher positiveY scale allows for an easier lift.
				public float ScalePositiveY = 3.0f;
				public float ScaleNegativeY = 2.5f;
				public float cursorPow = 1.5f;
// pinch state
				bool pinching = false;
				const float PINCH_PART = 0.25f;
				const float PINCH_FULL = 0.50f;
				PinchState state_ = PinchState.NoHands;

				PinchState state {
						get { 
								return state_;
						}
			
						set {  
								state_ = value;
								ShowCursors ();
								Debug.Log ("Setting state to " + state);
						}
				}
// smoothing
				public bool smoothCursor = true;
				public float cursorExp = 0.2f;

				// Use this for initialization
				void Start ()
				{
						if (curosrSprite == null)
								curosrSprite = transform.Find ("cursor").gameObject;

						if (camera == null)
								camera = Camera.main;
				}
	
				// Update is called once per frame
				void Update ()
				{
						FingerModel index = GetIndex ();
						FingerModel thumb = GetThumb ();
			
						if (index != null && thumb != null) {
								AnimateCursor (index, thumb);
								SetPinchAnimation ();
								switch (GetPinch ()) {
								case 1:
										state = PinchState.Full;
										break;

								case 2: 
										state = PinchState.Part;
										break;

								case 3:
										state = PinchState.Open;
										break;
								}
						} else {
								state = PinchState.NoHands;
						}

						SetFingerSpritePosition (index, thumb);
				}

				void AnimateCursor (FingerModel index, FingerModel thumb)
				{
						Vector3 midPosition = (index.GetTipPosition () + thumb.GetTipPosition ()) / 2;
						immediatePosition = (smoothCursor) ? Utils.ExponentialVectorSmoothing (immediatePosition, midPosition, cursorExp) : midPosition;
						if (state == PinchState.Open) {
								cursorPosition = immediatePosition;
						}
						curosrSprite.transform.position = TransformPosition (cursorPosition);
						immediateSprite.transform.position = TransformPosition(immediatePosition);
				}

				void ShowCursors ()
				{
						switch (state) {
						case PinchState.NoHands:
								curosrSprite.SetActive (false);
								fingerSprite.SetActive (false);
								thumbSprite.SetActive (false);
								immediateSprite.SetActive (false);
								break;
				
						case PinchState.Full: 
								immediateSprite.SetActive (true);
								break;

						default:
								curosrSprite.SetActive (true);
								fingerSprite.SetActive (true);
								thumbSprite.SetActive (true);
								immediateSprite.SetActive (false);
								break;
						}
				}

				void SetFingerSpritePosition (FingerModel index, FingerModel thumb)
				{
						if (index != null)
								fingerSprite.transform.position = TransformPosition (index.GetTipPosition ());
						if (thumb != null)
								thumbSprite.transform.position = TransformPosition (thumb.GetTipPosition ());
				}

				void SetPinchAnimation ()
				{
						curosrSprite.GetComponent<CursorSprite> ().ani.SetInteger ("pinch", GetPinch ());
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
			
						// using foreach artifically to get the first hands' pinch strength
						foreach (HandModel hh in handController.hand_graphics_.Values) {
								Hand h = hh.GetLeapHand ();
								if (h.PinchStrength > PINCH_FULL)
										return 1;
								else if (h.PinchStrength > PINCH_PART)
										return 2;
								else 
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