using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

public delegate float Ease(float start, float end, float value);

#region EasingFunction_enum
public enum EasingFunction 
{
	LinerEase,
	SineEaseIn,
	SineEaseOut,
	SineEaseInOut,
	
	QuadraticEaseIn,
	QuadraticEaseOut,
	QuadraticEaseInOut,
	
	CubicEaseIn,
	CubicEaseOut,
	CubicEaseInOut,
	
	QuarticEaseIn,
	QuarticEaseOut,
	QuarticEaseInOut,
	
	QuinticEaseIn,
	QuinticEaseOut,
	QuinticEaseInOut,
	
	CircularEaseIn,
	CircularEaseOut,
	CircularEaseInOut,
	
	ElasticEaseIn,
	ElasticEaseOut,
	ElasticEaseInOut,
	
	BounceEaseIn,
	BounceEaseOut,
	BounceEaseInOut,
	BounceEaseOutIn,
	
	BackEaseIn,
	BackEaseOut,
	BackEaseInOut,
	
	ExpoEaseIn,
	ExpoEaseOut,
	ExpoEaseInOut,

	Null
}
#endregion

public static class RobTween
{
	public static readonly string[] enumStringLookUp = 
	{
		"LinerEase",

		"SineEaseIn",
		"SineEaseOut",
		"SineEaseInOut",

		"QuadraticEaseIn",
		"QuadraticEaseOut",
		"QuadraticEaseInOut",
		
		"CubicEaseIn",
		"CubicEaseOut",
		"CubicEaseInOut",
		
		"QuarticEaseIn",
		"QuarticEaseOut",
		"QuarticEaseInOut",
		
		"QuinticEaseIn",
		"QuinticEaseOut",
		"QuinticEaseInOut",
		
		"CircularEaseIn",
		"CircularEaseOut",
		"CircularEaseInOut",
		
		"ElasticEaseIn",
		"ElasticEaseOut",
		"ElasticEaseInOut",
		
		"BounceEaseIn",
		"BounceEaseOut",
		"BounceEaseInOut",
		"BounceEaseOutIn",
		
		"BackEaseIn",
		"BackEaseOut",
		"BackEaseInOut",
		
		"ExpoEaseIn",
		"ExpoEaseOut",
		"ExpoEaseInOut",
		
		"Null"
	};
	
	/// <summary>
	/// Moves transform to "start" position then moves back to the transforms original position
	/// </summary>
//	public static void MoveFrom(this MonoBehaviour mono, Transform transform, Vector3 start, float duration, Ease ease)
//	{
//		mono.StartCoroutine(MoveToCoroutine(mono, transform, start, transform.position, duration, ease));
//	}

	/// <summary>
	/// Moves transform to "start" position then moves to "end" position
	/// </summary>
//	public static void MoveFrom(this MonoBehaviour mono, Transform transform, Vector3 start, Vector3 end, float duration, Ease ease)
//	{
//		mono.StartCoroutine(MoveToCoroutine(mono, transform, start, end, duration, ease));
//	}

	/// <summary>
	/// Moves transforms to "end" position
	/// </summary>
//	public static void MoveTo(this MonoBehaviour mono, Transform transform, Vector3 end, float duration, Ease ease)
//	{
//		mono.StartCoroutine(MoveToCoroutine(mono, transform, transform.position, end, duration, ease));
//	}

	/// <summary>
	/// Moves transforms from a "start" position to "end" position
	/// </summary>
//	public static void MoveTo(this MonoBehaviour mono, Transform transform, Vector3 start, Vector3 end, float duration, Ease ease)
//	{
//		mono.StartCoroutine(MoveToCoroutine(mono, transform, start, end, duration, ease));
//	}

	/// <summary>
	/// Scales transforms from "start" position back to transforms original position
	/// </summary>
//	public static void ScaleFrom(this MonoBehaviour mono, Transform transform, Vector3 start, float duration, Ease ease)
//	{
//		mono.StartCoroutine(ScaleToCoroutine(mono, transform, start, transform.localScale, duration, ease));
//	}

	/// <summary>
	/// Scales transforms from "start" position to "end" position
	/// </summary>
//	public static void ScaleFrom(this MonoBehaviour mono, Transform transform, Vector3 start, Vector3 end, float duration, Ease ease)
//	{
//		mono.StartCoroutine(ScaleToCoroutine(mono, transform, start, end, duration, ease));
//	}

	/// <summary>
	/// Scales transforms to "end" position
	/// </summary>
//	public static void ScaleTo(this MonoBehaviour mono, Transform transform, Vector3 end, float duration, Ease ease)
//	{
//		mono.StartCoroutine(ScaleToCoroutine(mono, transform, transform.localScale, end, duration, ease));
//	}

	/// <summary>
	/// Scales transforms from a "start" position to "end" position
	/// </summary>
//	public static void ScaleTo(this MonoBehaviour mono, Transform transform, Vector3 start, Vector3 end, float duration, Ease ease)
//	{
//		mono.StartCoroutine(ScaleToCoroutine(mono, transform, start, end, duration, ease));
//	}

//	private static IEnumerator MoveToCoroutine(MonoBehaviour mono, Transform transform, Vector3 start, Vector3 end, float duration, Ease ease)
//	{
//		float elapsed = 0f;		
//		while(elapsed < duration)
//		{
//			if(mono.enabled)
//			{
//				elapsed += Time.deltaTime;
//				transform.position = transform.position.EaseVector(start, end, elapsed / duration, ease);
//			}
//			yield return null;
//		}
//	}

//	private static IEnumerator ScaleToCoroutine(MonoBehaviour mono, Transform transform, Vector3 start, Vector3 end, float duration, Ease ease)
//	{
//		float elapsed = 0f;		
//		while(elapsed < duration)
//		{
//			if(mono.enabled)
//			{
//				elapsed += Time.deltaTime;
//				transform.localScale = transform.localScale.EaseVector(start, end, elapsed / duration, ease);
//			}
//			yield return null;
//		}
//	}
//
//	public static void ShakePosition(this MonoBehaviour mono, Transform transform, Vector3 shakeAmount, float shakeDuration)
//	{
//		mono.StartCoroutine(Shake(transform, shakeAmount, shakeDuration));
//	}

	private static IEnumerator Shake(Transform transform, Vector3 magnitude, float duration)
	{
		magnitude *= 0.5f;
		float elapsed = 0.0f;		
		float shakeMinDiff = 0.5f;
		Vector3 originalPos = transform.position;
		float lastX = 0f;
		float lastY = 0f;
		float lastZ = 0f;
		while(elapsed < duration)
		{
			elapsed += Time.deltaTime;
			float percentageComplete = elapsed / duration;
			float damper = 1.0f - Mathf.Clamp(4.0f * percentageComplete - 3.0f, 0.0f, 1.0f); // slows shake for the last 25%
			
			float x = UnityEngine.Random.value * 2f - 1f; // get random value between -1 and 1
			float y = UnityEngine.Random.value * 2f - 1f; // get random value between -1 and 1
			float z = UnityEngine.Random.value * 2f - 1f; // get random value between -1 and 1
			while(Mathf.Abs(x - lastX) < shakeMinDiff)
				x = UnityEngine.Random.value * 2f - 1f; // get random value between -1 and 1
			while(Mathf.Abs(y - lastY) < shakeMinDiff)
				y = UnityEngine.Random.value * 2f - 1f; // get random value between -1 and 1
			while(Mathf.Abs(z - lastZ) < shakeMinDiff)
				z = UnityEngine.Random.value * 2f - 1f; // get random value between -1 and 1
			
			lastX = x;
			lastY = y;
			lastZ = z;
			x *= magnitude.x * damper;
			y *= magnitude.y * damper;
			z *= magnitude.z * damper;
			
			transform.position = new Vector3 (originalPos.x + x, originalPos.y + y, originalPos.z + z);
			yield return null;
		}		
		transform.position = originalPos;
	}

//	public static Vector3 EaseVector(Vector3 start, Vector3 end, float percentageComplete, Ease ease)
//	{
//		return percentageComplete >= 1f ? end : new Vector3(ease(start.x, end.x, percentageComplete), ease(start.y, end.y, percentageComplete), ease(start.z, end.z, percentageComplete));		
//	}

//	public static Vector3 EaseVector(this Vector3 vector3, Vector3 start, Vector3 end, float percentageComplete, Ease ease)
//	{
//		return percentageComplete >= 1f ? end : new Vector3(ease(start.x, end.x, percentageComplete), ease(start.y, end.y, percentageComplete), ease(start.z, end.z, percentageComplete));		
//	}
//
//	public static Vector3 EaseVector(this Vector3 vector3, Vector3 start, Vector3 end, float currentTime, float duration, Ease ease)
//	{
//		float percentageComplete = currentTime / duration;
//		return percentageComplete >= 1f ? end : new Vector3(ease(start.x, end.x, percentageComplete), ease(start.y, end.y, percentageComplete), ease(start.z, end.z, percentageComplete));		
//	}

//	public static float EaseFloat(this float f, float start, float end, float percentageComplete, Ease ease)
//	{
//		return percentageComplete >= 1f ? end : ease(start, end, percentageComplete);
//	}

//	public static float EaseFloat(this float f, float start, float end, float currentTime, float duration, Ease ease)
//	{
//		float percentageComplete = currentTime / duration;
//		return percentageComplete >= 1f ? end : ease(start, end, percentageComplete);
//	}

//	public static Color EaseColor(this Color color, Color start, Color end, float percentageComplete, Ease ease)
//	{
//		return percentageComplete >= 1f ? end : new Color(ease(start.r, end.r, percentageComplete), ease(start.g, end.g, percentageComplete), ease(start.b, end.b, percentageComplete));
//	}

//	public static Color EaseColorWithAlpha(this Color color, Color start, Color end, float percentageComplete, Ease ease)
//	{
//		return percentageComplete >= 1f ? end : new Color(ease(start.r, end.r, percentageComplete), ease(start.g, end.g, percentageComplete), ease(start.b, end.b, percentageComplete), ease(start.a, end.a, percentageComplete));
//	}

	/// <summary>
	/// Gets the ease with name of function
	/// </summary>
	/// <param name="name">Name.</param>
	public static Ease GetEase(string name)
	{
		MethodInfo mi = typeof(RobTween).GetMethod(name, BindingFlags.Public | BindingFlags.Static);	
		Ease e = (Ease)Delegate.CreateDelegate(typeof(Ease), mi);
		return e;
	}

	/// <summary>
	/// Gets the ease with name of function
	/// </summary>
	/// <param name="name">Name.</param>
	public static Ease GetEase(EasingFunction name)
	{
		MethodInfo mi = typeof(RobTween).GetMethod(enumStringLookUp[name.GetHashCode()], BindingFlags.Public | BindingFlags.Static);	
		Ease e = (Ease)Delegate.CreateDelegate(typeof(Ease), mi);
		return e;
	}

//	private static string GetQuickEnumString(EasingFunction enumName)
//	{
//		switch(enumName)
//		{
//		case EasingFunction.BackEaseIn:
//			return "BackEaseIn";
//			break;
//		case EasingFunction.BackEaseInOut:
//			return "BackEaseInOut";
//			break;
//		case EasingFunction.BackEaseOut:
//			return "BackEaseOut";
//			break;
//		case EasingFunction.BounceEaseIn:
//			return "BounceEaseIn";
//			break;
//		case EasingFunction.BounceEaseInOut:
//			return "BounceEaseInOut";
//			break;
//		case EasingFunction.BounceEaseOut:
//			return "BounceEaseOut";
//			break;
//		case EasingFunction.BounceEaseOutIn:
//			return "BounceEaseOutIn";
//			break;
//		case EasingFunction.CircularEaseIn:
//			return "CircularEaseIn";
//			break;
//		case EasingFunction.CircularEaseInOut:
//			return "CircularEaseInOut";
//			break;
//		case EasingFunction.CircularEaseOut:
//			return "CircularEaseOut";
//			break;
//		case EasingFunction.CubicEaseIn:
//			return "CubicEaseIn";
//			break;
//		case EasingFunction.CubicEaseInOut:
//			return "CubicEaseInOut";
//			break;
//		case EasingFunction.CubicEaseOut:
//			return "CubicEaseOut";
//			break;
//		case EasingFunction.ElasticEaseIn:
//			return "ElasticEaseIn";
//			break;
//		case EasingFunction.ElasticEaseInOut:
//			return "ElasticEaseInOut";
//			break;
//		case EasingFunction.ElasticEaseOut:
//			return "ElasticEaseOut";
//			break;
//		case EasingFunction.ExpoEaseIn:
//			return "ExpoEaseIn";
//			break;
//		case EasingFunction.ExpoEaseInOut:
//			return "ExpoEaseInOut";
//			break;
//		case EasingFunction.ExpoEaseOut:
//			return "ExpoEaseOut";
//			break;
//		case EasingFunction.LinerEase:
//			return "LinerEase";
//			break;
//		case EasingFunction.Null:
//			return "Null";
//			break;
//		case EasingFunction.QuadraticEaseIn:
//			return "QuadraticEaseIn";
//			break;
//		case EasingFunction.QuadraticEaseInOut:
//			return "QuadraticEaseInOut";
//			break;
//		case EasingFunction.QuadraticEaseOut:
//			return "QuadraticEaseOut";
//			break;
//		case EasingFunction.QuarticEaseIn:
//			return "QuarticEaseIn";
//			break;
//		case EasingFunction.QuarticEaseInOut:
//			return "QuarticEaseInOut";
//			break;
//		case EasingFunction.QuarticEaseOut:
//			return "QuarticEaseOut";
//			break;
//		case EasingFunction.QuinticEaseIn:
//			return "QuinticEaseIn";
//			break;
//		case EasingFunction.QuinticEaseInOut:
//			return "QuinticEaseInOut";
//			break;
//		case EasingFunction.QuinticEaseOut:
//			return "QuinticEaseOut";
//			break;
//		case EasingFunction.SineEaseIn:
//			return "SineEaseIn";
//			break;
//		case EasingFunction.SineEaseInOut:
//			return "SineEaseInOut";
//			break;
//		case EasingFunction.SineEaseOut:
//			return "SineEaseOut";
//			break;
//		}
//		return "Null";		
//	}

	public static Vector3 Slerp(Vector3 start, Vector3 end, float percentageComplete)
	{
		// Dot product - the cosine of the angle between 2 vectors.
		float dot = Vector3.Dot(start, end);     
		// Clamp it to be in the range of Acos()
		// This may be unnecessary, but floating point
		// precision can be a fickle mistress.
		Mathf.Clamp(dot, -1.0f, 1.0f);
		// Acos(dot) returns the angle between start and end,
		// And multiplying that by percent returns the angle between
		// start and the final result.
		float theta = Mathf.Acos(dot) * percentageComplete;
		Vector3 relativeVector = end - start * dot;
		relativeVector.Normalize();     // Orthonormal basis
		// The final result.
		return ((start * Mathf.Cos(theta)) + (relativeVector * Mathf.Sin(theta)));
	}

//	public static Vector3 Slerp(Vector3 start, Vector3 end, float percentageComplete, Ease ease)
//	{
//		// Dot product - the cosine of the angle between 2 vectors.
//		float dot = Vector3.Dot(start, end);     
//		// Clamp it to be in the range of Acos()
//		// This may be unnecessary, but floating point
//		// precision can be a fickle mistress.
//		Mathf.Clamp(dot, -1.0f, 1.0f);
//		// Acos(dot) returns the angle between start and end,
//		// And multiplying that by percent returns the angle between
//		// start and the final result.
//		float theta = Mathf.Acos(dot) * percentageComplete;
//
//		Vector3 relativeVector = end - start * dot;
//		relativeVector.Normalize();     // Orthonormal basis
//		// The final result.
//		return ((start * Mathf.Cos(theta)) + (relativeVector * Mathf.Sin(theta)));
//	}

//	public static Vector3 Nlerp(Vector3 start, Vector3 end, float percentageComplete, Ease ease)
//	{
//		return EaseVector(start, end, percentageComplete, ease).normalized;
//	}
//
//	public static float PingPong(float from, float to, float time, Ease ease)
//	{
//		float alpha = (0.4f * Mathf.Sin(time)) + 0.6f;
//		return alpha;
//	}

	/// <summary>
	/// Bezier Curve woo woo :P
	/// </summary>
	/// <returns>The bezier point.</returns>
	/// <param name="t">T.</param>
	/// <param name="p0">P0.</param>
	/// <param name="p1">P1.</param>
	/// <param name="p2">P2.</param>
	/// <param name="p3">P3.</param>
	public static Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		float u1 = 1f - t;
		float tt = t * t;
		float uu = u1*u1;
		float uuu = uu * u1;
		float ttt = tt * t;
		
		Vector3 p = uuu * p0; //first term
		p += 3 * uu * t * p1; //second term
		p += 3 * u1 * tt * p2; //third term
		p += ttt * p3; //fourth term
		
		return p;
	}

	#region Linear
	public static float LinerEase(float start, float end, float percentageComplete)
	{
		end -= start;		
		return end * percentageComplete + start;
	}
	#endregion

	#region Sine
	public static float SineEaseIn(float start, float end, float percentageComplete)
	{
		end -= start;
		return -end * Mathf.Cos(percentageComplete / 1 * (Mathf.PI / 2)) + end + start;
	}
	
	public static float SineEaseOut(float start, float end, float percentageComplete)
	{
		end -= start;
		return end * Mathf.Sin(percentageComplete / 1 * (Mathf.PI / 2)) + start;
	}
	
	public static float SineEaseInOut(float start, float end, float percentageComplete)
	{
		end -= start;
		return -end / 2 * (Mathf.Cos(Mathf.PI * percentageComplete / 1) - 1) + start;
	}
	#endregion

	#region Quadratic
	public static float QuadraticEaseIn(float start, float end, float percentageComplete)
	{
		end -= start;
		return end * percentageComplete * percentageComplete + start;
	}
	
	public static  float QuadraticEaseOut(float start, float end, float percentageComplete)
	{
		end -= start;
		return -end * percentageComplete * (percentageComplete - 2) + start;
	}
	
	public static float QuadraticEaseInOut(float start, float end, float percentageComplete){
		percentageComplete /= 0.5f;
		end -= start;
		if (percentageComplete < 1) return end / 2 * percentageComplete * percentageComplete + start;
		percentageComplete--;
		return -end / 2 * (percentageComplete * (percentageComplete - 2) - 1) + start;
	}
	#endregion

	#region Cubic
	public static float CubicEaseIn(float start, float end, float percentageComplete)
	{
		end -= start;
		return end * percentageComplete * percentageComplete * percentageComplete + start;
	}
	
	public static float CubicEaseOut(float start, float end, float percentageComplete)
	{
		percentageComplete--;
		end -= start;
		return end * (percentageComplete * percentageComplete * percentageComplete + 1) + start;
	}
	
	public static float CubicEaseInOut(float start, float end, float percentageComplete)
	{
		percentageComplete /= 0.5f;
		end -= start;
		if (percentageComplete < 1) return end / 2 * percentageComplete * percentageComplete * percentageComplete + start;
		percentageComplete -= 2;
		return end / 2 * (percentageComplete * percentageComplete * percentageComplete + 2) + start;
	}
	#endregion

	#region Quartic
	public static float QuarticEaseIn(float start, float end, float percentageComplete)
	{
		end -= start;
		return end * percentageComplete * percentageComplete * percentageComplete * percentageComplete + start;
	}
	
	public static float QuarticEaseOut(float start, float end, float percentageComplete)
	{
		percentageComplete--;
		end -= start;
		return -end * (percentageComplete * percentageComplete * percentageComplete * percentageComplete - 1) + start;
	}
	
	public static float QuarticEaseInOut(float start, float end, float percentageComplete)
	{
		percentageComplete /= 0.5f;
		end -= start;
		if (percentageComplete < 1) return end / 2 * percentageComplete * percentageComplete * percentageComplete * percentageComplete + start;
		percentageComplete -= 2;
		return -end / 2 * (percentageComplete * percentageComplete * percentageComplete * percentageComplete - 2) + start;
	}
	#endregion

	#region Quintic
	public static float QuinticEaseIn(float start, float end, float percentageComplete)
	{
		end -= start;
		return end * percentageComplete * percentageComplete * percentageComplete * percentageComplete * percentageComplete + start;
	}
	
	public static float QuinticEaseOut(float start, float end, float percentageComplete)
	{
		percentageComplete--;
		end -= start;
		return -end * (percentageComplete * percentageComplete * percentageComplete * percentageComplete * percentageComplete- 1) + start;
	}
	
	public static float QuinticEaseInOut(float start, float end, float percentageComplete)
	{
		percentageComplete /= 0.5f;
		end -= start;
		if (percentageComplete < 1) 
			return end / 2 * percentageComplete * percentageComplete * percentageComplete * percentageComplete * percentageComplete + start;
		percentageComplete -= 2;
		return end / 2 * (percentageComplete * percentageComplete * percentageComplete * percentageComplete * percentageComplete + 2) + start;
	}
	#endregion

	#region Circular
	public static float CircularEaseIn(float start, float end, float percentageComplete)
	{
		end -= start;
		return -end * (Mathf.Sqrt(1 - percentageComplete * percentageComplete) - 1) + start;
	}
	
	public static float CircularEaseOut(float start, float end, float percentageComplete)
	{
		percentageComplete--;
		end -= start;
		return end * Mathf.Sqrt(1 - percentageComplete * percentageComplete) + start;		
	}
	
	public static float CircularEaseInOut(float start, float end, float percentageComplete)
	{
		percentageComplete /= 0.5f;
		end -= start;
		if (percentageComplete < 1) 
			return -end / 2 * (Mathf.Sqrt(1 - percentageComplete * percentageComplete) - 1) + start;
		percentageComplete -= 2;
		return end / 2 * (Mathf.Sqrt(1 - percentageComplete * percentageComplete) + 1) + start;
	}
	#endregion

	#region Elastic
	public static float ElasticEaseIn(float start, float end, float percentageComplete)
	{
		end -= start;
		if(percentageComplete == 1)
			return start + end;
		
		float p = 0.3f;
		float s = p / 4f;
		
		return -(end * Mathf.Pow( 2, 10 * (percentageComplete -= 1)) * Mathf.Sin((percentageComplete - s) * (2 * Mathf.PI) / p)) + start;
	}

	public static float ElasticEaseOut(float start, float end, float percentageComplete)
	{
		end -= start;
		if(percentageComplete == 1)
			return start + end;
		
		float p = 0.3f;
		float s = p / 4f;
		
		return (end * Mathf.Pow( 2, -10 * percentageComplete) * Mathf.Sin((percentageComplete - s) * ( 2 * Mathf.PI ) / p ) + end + start);
	}

	public static float ElasticEaseInOut(float start, float end, float percentageComplete)
	{
		end -= start;
		if ((percentageComplete /= 0.5f) == 2 )
			return start + end;
		
		float p = (0.3f * 1.5f);
		float s = p / 4f;
		
		if (percentageComplete < 1)
			return -0.5f * (end * Mathf.Pow(2, 10 * (percentageComplete -= 1)) * Mathf.Sin((percentageComplete - s) * (2f * Mathf.PI) / p)) + start;
		return end * Mathf.Pow( 2, -10 * (percentageComplete -= 1 ) ) * Mathf.Sin((percentageComplete - s) * ( 2 * Mathf.PI ) / p ) * 0.5f + end + start;
	}
	#endregion

	#region Bounce
	public static float BounceEaseIn(float start, float end, float percentageComplete)
	{
		end -= start;
		return end - BounceEaseOut(0, end, percentageComplete) + start;
	}

	public static float BounceEaseOut(float start, float end, float percentageComplete)
	{
		end -= start;
		if ( (percentageComplete) < (1f / 2.75f) )
			return end * ( 7.5625f * percentageComplete * percentageComplete ) + start;
		else if ( percentageComplete < ( 2 / 2.75f ) )
			return end * ( 7.5625f * ( percentageComplete -= (1.5f / 2.75f) ) * percentageComplete + 0.75f ) + start;
		else if ( percentageComplete < ( 2.5f / 2.75f ) )
			return end * ( 7.5625f * ( percentageComplete -= ( 2.25f / 2.75f ) ) * percentageComplete + 0.9375f ) + start;
		else
			return end * ( 7.5625f * ( percentageComplete -= ( 2.625f / 2.75f ) ) * percentageComplete + .984375f ) + start;
	}

	public static float BounceEaseInOut(float start, float end, float percentageComplete)
	{
		end -= start;
		if (percentageComplete < 0.5f)
			return BounceEaseIn(0, end, percentageComplete * 2f) * 0.5f + start;
		else
			return BounceEaseOut(0, end, percentageComplete * 2f - 1f) * 0.5f + end * 0.5f + start;
	}

	public static float BounceEaseOutIn(float start, float end, float percentageComplete)
	{
		end -= start;
		if (percentageComplete < 0.5f)
			return BounceEaseOut(start, end / 2f, percentageComplete * 2);
		return BounceEaseIn(start + end / 2f, end / 2f, (percentageComplete * 2 ) - 1f);
	}
	#endregion

	#region Back
	public static float BackEaseIn(float start, float end, float percentageComplete)
	{
		end -= start;
		float s = 1.70158f;
		return end * (percentageComplete) * percentageComplete * ( ( s + 1f ) * percentageComplete - s ) + start;
	}

	public static float BackEaseOut(float start, float end, float percentageComplete)
	{
		end -= start;
		float s = 1.70158f;
		return end * ((percentageComplete - 1) * percentageComplete * ((s + 1) * percentageComplete + s) + 1 ) + start;
	}

	public static float BackEaseInOut(float start, float end, float percentageComplete)
	{
		end -= start;
		float s = 1.70158f;
		if ( ( percentageComplete * 0.5f) < 1f )
			return end / 2 * ( percentageComplete * percentageComplete * ( ( (s *= 1.525f) + 1 ) * percentageComplete - s ) ) + start;
		return end / 2 * ( ( percentageComplete -= 2 ) * percentageComplete * ( ( (s *= 1.525f) + 1 ) * percentageComplete + s ) + 2 ) + start;
	}

	public static float BackEaseOutIn(float start, float end, float percentageComplete)
	{
		end -= start;
		if (percentageComplete < 0.5f)
			return BackEaseOut(start, end / 2, percentageComplete * 2);
		return BackEaseIn(start + end / 2, end / 2, (percentageComplete * 2 * 2) - 1f );
	}
	#endregion

	#region Expo
	public static float ExpoEaseIn(float start, float end, float percentageComplete)
	{
		end -= start;
		return (percentageComplete == 0f) ? start : (end * Mathf.Pow(2f, 10f * (percentageComplete - 1 )) + start);
	}

	public static float ExpoEaseOut(float start, float end, float percentageComplete)
	{
		end -= start;
		return (percentageComplete == 1f) ? (start + end) : (end * (-Mathf.Pow(2, -10 * percentageComplete) + 1 ) + start);
	}

	public static float ExpoEaseInOut(float start, float end, float percentageComplete)
	{
		end -= start;
		if (percentageComplete == 0)
			return start;
		
		if (percentageComplete == 1f)
			return start + end;
		
		if ((percentageComplete /= 0.5f) < 1)
			return end / 2f * Mathf.Pow( 2, 10 * (percentageComplete - 1)) + start;
		
		return end / 2f * (-Mathf.Pow(2, -10 * --percentageComplete) + 2) + start;
	}

	#endregion
}