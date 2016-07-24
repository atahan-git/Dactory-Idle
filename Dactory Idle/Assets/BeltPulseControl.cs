using UnityEngine;
using System.Collections;

public class BeltPulseControl : MonoBehaviour {

	public delegate void FirstPulse();
	public static event FirstPulse firstPulse;

	public delegate void SecondPulse();
	public static event SecondPulse secondPulse;

	public float pulseTime = 0.3f;

	// Use this for initialization
	void Start () {
		InvokeRepeating ("Pulsate", pulseTime, pulseTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Pulsate () {
		if (firstPulse != null) {
			firstPulse ();
			secondPulse ();
		}
	}
}
