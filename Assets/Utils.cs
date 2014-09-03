using UnityEngine;
using System.Collections;

namespace PinchRadialMenu
{
		public class Utils
		{
		#region smoothing
/**
 * exponential smoothing uses "inertial" combination to produce a smoother transition 
 * from one value to another.
 * the higher the smoothing value the smoother the movement from one value to another 
 * but the less the value reflects the actual imput.
 * note that for the float version, there is the option 
 * to have one smoothing value for increasing values,
 * and another one for decreasing values
 * to allow a difference for how fast a value climbs 
 * and how fast the value cools off. 
 */
				public static  float ExponentialSmoothing (float oldValue, float newValue, float smoothing)
				{
						if (oldValue == newValue)
								return newValue;
						smoothing = Mathf.Clamp (smoothing, 0f, 1f);
						return oldValue * smoothing + newValue * (1 - smoothing);
				}

				public static Vector3 ExponentialVectorSmoothing (Vector3 oldValue, Vector3 newValue, float smoothing)
				{
						return new Vector3 (ExponentialSmoothing (oldValue.x, newValue.x, smoothing),
			                   ExponentialSmoothing (oldValue.y, newValue.y, smoothing),
			                   ExponentialSmoothing (oldValue.z, newValue.z, smoothing));
				}
  
				public static float ExponentialSmoothing (float oldValue, float newValue, float smoothingIn, float smoothingOut)
				{
						return ExponentialSmoothing (oldValue, newValue, ((newValue > oldValue) ? smoothingIn : smoothingOut));
				}
#endregion

#region count
/**
 these methods are sugar on "++"/"--" type incrementing
*/
				public static int AddCount (int value)
				{
						return ++value;
				}

				public static int AddCount (int value, int max)
				{
						return Mathf.Clamp (value + 1, 0, max);
				}
    
				public static int SubCount (int value)
				{
						return Mathf.Max (value - 1, 0);
				}

				public static int SubCount (int value, int max)
				{
						return Mathf.Clamp (value - 1, 0, max);
				}
#endregion
		}
}