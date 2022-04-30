using UnityEngine;
using UnityEngine.Events;

public class Attachable : MonoBehaviour {
	
	[SerializeField] private UnityEvent attachAction;
	
	// private void OnTriggerEnter2D(Collider2D other) {
	// 	Debug.Log("meh");
	// 	attachAction.Invoke();
	// }
}
