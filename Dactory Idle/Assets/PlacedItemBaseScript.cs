using UnityEngine;
using System.Collections;

public class PlacedItemBaseScript : MonoBehaviour {

	public BeltScript[] outConveyors = new BeltScript[99];
	public int n_out = 0;
	public BeltScript[] inConveyors = new BeltScript[99];
	public int n_in = 0;

	public TileBaseScript[] tilesCovered = new TileBaseScript[99];
	public int n_cover = 0;

	public GameObject movingItem;

	// Use this for initialization
	void Start () {
		BeltPulseControl.secondPulse += SecondPulse;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool TryAccepting (GameObject myObject) {
		//check if we can accept it
		//if yes store it and stuff
		//if not return false

		return false;
	}


	public void SecondPulse(){
		foreach (BeltScript bS in outConveyors) {
			if (bS != null) {
				for (int i = 0; i <= 3; i++) {					//if not find any other
					if (bS.inputStorage [i] != null) {
						if (bS.inputStorage [i].myItem == null) {
							bS.inputStorage [i].myItem = (GameObject)Instantiate(movingItem, transform.position, transform.rotation);
							bS.UpdateItemLocations ();
						}
					}
				}
			}
		}
	}
}
