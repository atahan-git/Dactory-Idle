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


	public bool shouldGiveItem = false;
	// Use this for initialization
	void Start () {
		//BeltPulseControl.secondPulse += SecondPulse;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool TryAccepting (MovingObject myObject, Vector3 offset) {
		//check if we can accept it
		//if yes store it and stuff
		//if not return false
		myObject.SelfDestruct(offset);
		return true;
	}

	public void BackwardsPulse (){
		if (!shouldGiveItem)
			return;
		print ("gived item");
		foreach (BeltScript bS in outConveyors) {
			if (bS != null) {
				for (int i = 0; i <= 3; i++) {					//if not find any other
					if (bS.inputStorage [i].n != -1) {
						if (bS.inputStorage [i].myItem == null) {
							bS.inputStorage [i].myItem = ((GameObject)Instantiate(movingItem, transform.position, transform.rotation)).GetComponent<MovingObject>();
							bS.UpdateItemLocations ();
							bS.inputStorage [i].myItem.transform.position = bS.inputStorage [i].myItem.placeToBe + bS.inputStorage[i].offSet;
						}
					}
				}
			}
		}
	}

	/*public void SecondPulse(){
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
	}*/
}
