using UnityEngine;
using System.Collections;

public class CursorSprite : MonoBehaviour
{
		public Animator ani;
		// Use this for initialization
		void Start ()
		{
				ani = GetComponent<Animator> (); //get the animator
		}
	
		// Update is called once per frame
		void Update ()
		{
		}
}
