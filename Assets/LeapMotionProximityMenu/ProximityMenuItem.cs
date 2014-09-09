using UnityEngine;
using System.Collections;

namespace PinchRadialMenu
{
		public class ProximityMenuItem : MonoBehaviour
		{

				SpriteRenderer sr;
				Color baseColor;
				Color activeColor = new Color (0.75f, 0, 0.25f);
				bool isCancel = false;
				public string value = "";
				public	GameObject baseSprite;
				public	GameObject activeSprite;
		public GameObject center; // used to determine proximity

				// Use this for initialization
				void Start ()
				{
						if (value == "")
								value = name;
				}
	
				// Update is called once per frame
				void Update ()
				{
	
				}

				public void Select (bool active)
				{
						if (active) {
								baseSprite.GetComponent<SpriteRenderer> ().enabled = false;
								activeSprite.GetComponent<SpriteRenderer> ().enabled = true;
						} else {
								baseSprite.GetComponent<SpriteRenderer> ().enabled = true;
								activeSprite.GetComponent<SpriteRenderer> ().enabled = false;
						}
				}

				public void Display (bool active)
				{
						baseSprite.GetComponent<SpriteRenderer> ().enabled = active;
						activeSprite.GetComponent<SpriteRenderer> ().enabled = false; // always false til rollover
				}

				public void Display ()
				{
						Display (true);
				}

				public void Select ()
				{
						Select (true);
				}
		}

}