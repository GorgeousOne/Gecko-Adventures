using System;
using UnityEngine;
using UnityEngine.Assertions;

public class MathUtil {
	
	public static float FloorMod(float a, float b){
		return a - Mathf.Floor(a / b) * b;
	}

	public static float WrapToPi(float angle) {
		return FloorMod(angle + 180, 360) - 180;
	}

	public static bool IsZero(float f, float margin = 0.01f) {
		return Mathf.Abs(f) < margin;
	}

	public static float SquareIn(float percent) {
		float oneMinus = 1 - Mathf.Clamp01(percent);
		return 1f - oneMinus * oneMinus;
	}
}
