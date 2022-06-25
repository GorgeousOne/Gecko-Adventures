using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Daylight : MonoBehaviour {

	[SerializeField] private float minDistX = 2;
	[SerializeField] private float maxDistX = 10;
	[SerializeField] private float minIntensity = .5f;
	[SerializeField] private float maxIntensity = 1;
	[SerializeField] private float minAngle = 5;
	[SerializeField] private float maxAngle = 13;
	[SerializeField] private GameObject player;

	private Light2D _light;
	
	private void OnEnable() {
		_light = GetComponent<Light2D>();
	}

	void Update() {
		float dist = Mathf.Abs(player.transform.position.x - transform.position.x) - minDistX;
		float dist01 = 1 - Mathf.Clamp01(dist / (maxDistX - minDistX));
		dist01 = Mathf.SmoothStep(0, 1, dist01);
		_light.intensity = minIntensity + dist01 * (maxIntensity - minIntensity);
		_light.pointLightOuterAngle = minAngle + dist01 * (maxAngle - minAngle);
	}
}
