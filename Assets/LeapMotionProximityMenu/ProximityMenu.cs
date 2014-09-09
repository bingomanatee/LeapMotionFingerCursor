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
				public TextMesh stateFeedback;
				bool overMenu = false;

				MenuState state {
						get {
								return state_;
						}

						set {
								if (value == state_ || value == null)
										return;

								Debug.Log (">>>>> Menu State: " + state_ + " to " + value);
								if (stateFeedback != null)
										stateFeedback.text = value.ToString () + " - " + (overMenu ? "over" : " not over");

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
					//	Debug.Log ("ShowMenuItems: " + show);
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
						
								if (cursor.state != oldState) {
										switch (cursor.state) {
										case FingerCursor.PinchState.NoHands:
												ShowMenuItems (false);
												if (state != MenuState.Closed) {
														state = MenuState.Closed;
												}
												break;
					
										case FingerCursor.PinchState.Open:
												ShowMenuItems (false);
//												Debug.Log ("cursor open; setting state to closed");
												if (state != MenuState.Closed) {
														state = MenuState.Closed;
												}
												break;
					
										case FingerCursor.PinchState.Part:
												break;
					
										case FingerCursor.PinchState.Full:
												if (overMenu && state == MenuState.Closed || state == MenuState.Over) {
														ShowMenuItems (true);
														state = MenuState.Open;
												}
												break;
										}
										oldState = cursor.state;
								}

								if (state == MenuState.Open) {
										FindHotMenuItem ();
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
						Vector3 delta;
						Vector3 hotDelta = Vector3.zero;
						foreach (ProximityMenuItem mi in items) {
								delta = (mi.center.transform.position - cimp);
								if (hotMenu == null || delta.sqrMagnitude < (hotMenu.transform.position - cimp).sqrMagnitude) {
										hotMenu = mi;
										hotDelta = delta;
								}
						}

						if (lastHotMenu == hotMenu)
								return;
			
						if (lastHotMenu != null) {
								lastHotMenu.Select (false);
								lastHotMenu = null;
						}

						if (hotMenu != null && hotDelta.magnitude <= MIN_DISTANCE) {
								lastHotMenu = hotMenu;			
								Debug.Log ("finding hot menu: " + lastHotMenu.name + "," + lastHotMenu.value + ": distance = " + hotDelta.magnitude);
				
								lastHotMenu.Select ();
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
						RefreshMenu ();
				}
		#endregion

		#region triggers

				void OnTriggerEnter2D (Collider2D c)
				{ 
						if (c.tag == "mainCursor") {
								overMenu = true;
								Debug.Log ("Trigger enter");
								if (state == MenuState.Closed)
										state = MenuState.Over;
						}

				}

				void OnTriggerStay2d (Collider2D c)
				{
						if (c.tag == "mainCursor") {
								overMenu = true;
								if (state == MenuState.Closed) {
										state = MenuState.Open;
										Debug.Log ("cursor closed -- TriggerStay2D setting state to open");				
								}
						}
				}

				void OnTriggerExit2D (Collider2D c)
				{
						if (c.tag == "mainCursor") {
								overMenu = false;
							//	Debug.Log ("Trigger exit");
								if (state == MenuState.Over)
										state = MenuState.Closed;
						}
				}

#endregion

		}
}