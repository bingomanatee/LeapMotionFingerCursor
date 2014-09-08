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

								Debug.Log (">>>>> Menu State: " + state_ + " to " + value);

								if (state_ == MenuState.Open || state_ == MenuState.Over)
										ReflectChosenMenu ();

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

				void ReflectChosenMenu ()
				{
						Debug.Log ("=========Choice Made: " + ((lastHotMenu == null) ? "(none)" : lastHotMenu.value));
						lastHotMenu = null;
				}

				void AnimateMenuOpen ()
				{
						background.SetInteger ("MenuState", 1);
				}

				void ShowMenuItems (bool show)
				{
						Debug.Log ("ShowMenuItems: " + show);
						foreach (ProximityMenuItem i in items) {
								i.Display (show);
						}
				}

				void AnimateMenuClose ()
				{
						background.SetInteger ("MenuState", 2);
				}

				void RefreshMenu ()
				{
						if (cursor != null && cursor.state != null) {
								if (state == MenuState.Open) {
										if (cursor.state != oldState) {
												switch (cursor.state) {
												case FingerCursor.PinchState.NoHands:
														break;
					
												case FingerCursor.PinchState.Open:
														ShowMenuItems (false);
														state = MenuState.Closed;
														break;
					
												case FingerCursor.PinchState.Part:
														ShowMenuItems (false);
														state = MenuState.Closed;
														break;
					
												case FingerCursor.PinchState.Full:
														ShowMenuItems (true);
														break;
												}
												oldState = cursor.state;
										}
										FindHotMenuItem ();
								} else if (state == MenuState.Over) {
										if (cursor.state != oldState) {
												switch (cursor.state) {
												case FingerCursor.PinchState.NoHands:
														break;
							
												case FingerCursor.PinchState.Open:
														break;
							
												case FingerCursor.PinchState.Part:
														break;
							
												case FingerCursor.PinchState.Full:
														ShowMenuItems (true);
														state = MenuState.Open;
														break;
												}
												oldState = cursor.state;
										}
								}
						}
				}
		
				ProximityMenuItem lastHotMenu;
		
				void FindHotMenuItem ()
				{
						if (state != MenuState.Open)
								return;
						ProximityMenuItem hotMenu = null;
						Vector3 cimp = cursor.immediateSprite.transform.position;
						foreach (ProximityMenuItem mi in items) {
								if (hotMenu == null || (mi.transform.position - cimp).sqrMagnitude < (hotMenu.transform.position - cimp).sqrMagnitude) {
										hotMenu = mi;
								}
						}

						if (hotMenu != lastHotMenu) {
								if (lastHotMenu != null) 
										lastHotMenu.Select (false);
								lastHotMenu = hotMenu;
								lastHotMenu.Select ();
						}
						Debug.Log ("finding hot menu: " + lastHotMenu.name + "," + lastHotMenu.value);
			
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