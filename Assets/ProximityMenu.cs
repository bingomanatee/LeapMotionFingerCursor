using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PinchRadialMenu
{

/**
 This class coordinates the state of the menu
 with the state of its cursor. 
 Partly or fully pinching the cursor will trigger menu opening. 
 Opening the fingers or having no hands in the scene will trigger menu closing.
*/
		public class ProximityMenu : MonoBehaviour
		{
				enum MenuState
				{
						Closed,
						Over,
						Open
				}

				public FingerCursor cursor;
				public Color activeColor = Color.red;
				public Animator background;
				List<ProximityMenuItem> items;
				const float MIN_DISTANCE = 2f;
				ProximityMenuItem activeItem;
				MenuState state_ = MenuState.Closed;
				FingerCursor.PinchState oldState = FingerCursor.PinchState.NoHands;

				MenuState state {
						get {
								return state_;
						}

						set {
								if (value == state_ || value == null)
										return;
								switch (value) {
								case MenuState.Closed:
										AnimateMenuClose ();
										ShowMenuItems (false);
										break;

								case MenuState.Over:
										AnimateMenuOpen ();
										break;

								case MenuState.Open: 
										ShowMenuItems (true);
										break;

								default: 
										Debug.Log ("Not handling state " + value);
										break;
								}
								state_ = value; 

						}
				}

				void AnimateMenuOpen ()
				{
						background.SetInteger ("MenuState", 1);
				}

				void ShowMenuItems (bool show)
				{
						Debug.Log ("Setting visible of " + items.Count + " items to " + show);
						foreach (ProximityMenuItem i in items) {
								i.renderer.enabled = show;
						}
				}

				void AnimateMenuClose ()
				{
						background.SetInteger ("MenuState", 2);
				}

				void HighlightClosest ()
				{ 
						return;
						float nearestDistance = 10000;
						ProximityMenuItem nearestMenu;
						Vector3 cursorPosition = cursor.curosrSprite.transform.position;
						foreach (ProximityMenuItem mi in items) {
								float distance = (mi.transform.position - cursorPosition).magnitude;
								if (distance < nearestDistance) {
										nearestDistance = distance;
										nearestMenu = mi;
								}
						}
						if (activeItem != null) {
								activeItem.SetActive (false);
						}
				}

				void RefreshMenu ()
				{
						if (cursor != null && cursor.state != null) {
				
								switch (cursor.state) {
								case FingerCursor.PinchState.NoHands:
										if (cursor.state != oldState) {
										}
										break;
				
								case FingerCursor.PinchState.Open:
										if (cursor.state != oldState) {
											
						ShowMenuItems (false);
										}
										break;
				
								case FingerCursor.PinchState.Part:
										if (cursor.state != oldState) {
												
						ShowMenuItems (true);
										}
										break;
				
								case FingerCursor.PinchState.Full:
										if (cursor.state != oldState) {
											
						ShowMenuItems (true);
										} 
				//if (state == MenuState.Open)
				//	HighlightClosest ();
										break;
								}
						}
				}
		
		#region main loops
		
				// Use this for initialization
				void Start ()
				{
						items = new List<ProximityMenuItem> ();
						foreach (ProximityMenuItem mi in GetComponentsInChildren<ProximityMenuItem>())
								items.Add (mi);
						ShowMenuItems (false);
						background.SetInteger ("MenuState", 0);
				}
		
				bool ad = false;
				bool ad1 = false;

				// Update is called once per frame
				void Update ()
				{

						/*		if (Time.time > 8 && (!ad)) {
								Debug.Log ("Closing Menu");
								background.SetInteger ("MenuState", 2);
								ad = true;
						} else if (Time.time > 3 && (!ad1)) {
								Debug.Log ("Opening Menu");
								background.SetInteger ("MenuState", 1);
								ad1 = true;
						} */
						RefreshMenu ();
				}
		#endregion

		#region triggers

				void OnTriggerEnter2D (Collider2D c)
				{ 
						if (c.tag == "mainCursor")
								Debug.Log ("Trigger enter");
						if (state == MenuState.Closed)
								state = MenuState.Over;
				}

				void OnTriggerStay2d (Collider2D c)
				{

				}

				void OnTriggerExit2D (Collider2D c)
				{
			
						if (c.tag == "mainCursor")
								Debug.Log ("Trigger exit");
						if (state == MenuState.Over)
								state = MenuState.Closed;

				}

#endregion

		}
}