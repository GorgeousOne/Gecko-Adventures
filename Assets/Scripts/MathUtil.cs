using System;
using UnityEngine;
using UnityEngine.Assertions;

public class MathUtil {

	public static float Map(float value, float low1, float high1, float low2, float high2) {
		float normal = Mathf.InverseLerp(low1, high1, value);
		return Mathf.Lerp(low2, high2, normal);
	}
	
	public static float FloorMod(float a, float b){
		return a - Mathf.Floor(a / b) * b;
	}

	public static float WrapToPi(float angle) {
		return FloorMod(angle + 180, 360) - 180;
	}

	public static bool IsZero(float f, float margin = 0.1f) {
		return Mathf.Abs(f) < margin;
	}

	public static float SquareIn(float percent) {
		float oneMinus = 1 - Mathf.Clamp01(percent);
		return 1f - oneMinus * oneMinus;
	}

	public static float SquareOut(float percent) {
		float oneMinus = Mathf.Clamp01(percent) - 1;
		return (oneMinus*oneMinus);
	}
}
