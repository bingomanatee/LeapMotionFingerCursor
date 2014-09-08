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

				// Use this for initialization
				void Start ()
				{
						if (value == "")
								value = name;
						sr = GetComponent<SpriteRenderer> ();
						baseColor = sr.color;
				}
	
				// Update is called once per frame
				void Update ()
				{
	
				}

				public void Select (bool active)
				{
						if (active)
								sr.color = activeColor;
						else
								sr.color = baseColor;
				}

				public void Select ()
				{
						Select (true);
				}
		}

}