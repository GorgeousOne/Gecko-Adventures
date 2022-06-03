using UnityEngine;

/// <summary>
/// Time util class that runs time in the same way as <see cref="Time"/> but allows for jumping back in time
/// </summary>
public class LevelTime : MonoBehaviour {

	private static float _time;
	public static float time => _time;

	/// <summary>
	/// Sets time to a new value (will probably effect animations, movement cycles etc)
	/// </summary>
	/// <param name="newTime"></param>
	public static void SetTime(float newTime) {
		_time = newTime;
	}

	private void Start() {
		_time = Time.time;
	}

	private void Update() {
		_time += Time.deltaTime;
	}
}