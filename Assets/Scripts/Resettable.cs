
using System.Collections.Generic;
using UnityEngine;

public abstract class Resettable : MonoBehaviour {

	public abstract void SaveState();
	public abstract void ResetState();
}