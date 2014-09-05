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
						Opening,
						Open,
						Closing
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
								if (value == state_)
										return;
								switch (value) {
								case MenuState.Closed:
// no action
										break;

								case MenuState.Opening:
										AnimateMenuOpen (state_);
										break;

								case MenuState.Open: 
// no action
										break;

								case MenuState.Closing: 
										AnimateMenuClose (state_);
										break;
								}
								state = value; 

						}
				}

				void AnimateMenuOpen (MenuState oldState)
				{
						switch (oldState) {
						case MenuState.Closed:
// do transition;
								break;

						case MenuState.Closing:
// do transition
								break;

						default: 
// opened or opening -- do nothing
								break;

						}
				}

				void AnimateMenuClose (MenuState oldState)
				{
						switch (oldState) {
						case MenuState.Open:
				// do transition
								break;
				
						case MenuState.Opening:
				// do transition
								break;
				
						default: 
				// closed or closing -- do nothing
								break;
				
						}
				}

				void ShowMenu (bool show)
				{
						if (show) {
								if (state == MenuState.Closed || state == MenuState.Closing) {
										state = MenuState.Opening;
								}
						} else {
								if (state == MenuState.Open || state == MenuState.Opening) {
										state = MenuState.Closing;
								}
						}
				}

				void HighlightClosest ()
				{ 
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

				void ShowMenu ()
				{
						ShowMenu (true);
						background.SetTrigger ("Start");
				}
		
		#region main loops
		
				// Use this for initialization
				void Start ()
				{
						items = new List<ProximityMenuItem> ();
						foreach (ProximityMenuItem mi in GetComponentsInChildren<ProximityMenuItem>())
								items.Add (mi);
			
						background.SetInteger ("MenuState", 0);
				}
		
				bool ad = false;
				bool ad1 = false;

				// Update is called once per frame
				void Update ()
				{

						if (Time.time > 8 && (!ad)) {
								Debug.Log ("Closing Menu");
								background.SetInteger ("MenuState", 2);
								ad = true;
						} else if (Time.time > 3 && (!ad1)) {
								Debug.Log ("Opening Menu");
								background.SetInteger ("MenuState", 1);
								ad1 = true;
						}

						switch (cursor.state) {
						case FingerCursor.PinchState.NoHands:
								if (cursor.state != oldState)
										ShowMenu (false);
								break;
				
						case FingerCursor.PinchState.Open:
								if (cursor.state != oldState)
										ShowMenu (false);
								break;

						case FingerCursor.PinchState.Part:
								if (cursor.state != oldState)
										ShowMenu (true);
								break;

						case FingerCursor.PinchState.Full:
				
								if (cursor.state != oldState)
										ShowMenu (true);
								if (state == MenuState.Open)
										HighlightClosest ();
								break;
						}
				}
		#endregion
		}
}