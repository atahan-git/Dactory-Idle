using UnityEngine;
using System.Collections;

public class BeltScript : MonoBehaviour {

	//[HideInInspector]
	public Place[] inputStorage = new Place[4];
	//[HideInInspector]
	public Place[] outputStorage = new Place[4];
	//[HideInInspector]
	public Place middleStorage;
	//[HideInInspector]
	public Place toBeGone;

	//[HideInInspector]
	public PlacedItemBaseScript[] feedingItems = new PlacedItemBaseScript[4];
	public PlacedItemBaseScript[] inputItems = new PlacedItemBaseScript[4];
	//[HideInInspector]
	public BeltScript[] feedingBelts = new BeltScript[4];
	public BeltScript[] inputBelts = new BeltScript[4];

	[HideInInspector]
	public bool[] inLocations = new bool[4];
	[HideInInspector]
	public bool[] outLocations = new bool[4];

	public int x = 0;
	public int y = 0;

	[HideInInspector]
	public GameObject tileCovered;

	public BeltSet mySet;

	float itemOffset = 0.33f;
	float toBeGoneMult = 1.5f;

	[HideInInspector]
	public GameObject[] oldShit = new GameObject[20];

	// Use this for initialization
	void Start () {
		middleStorage = new Place (Vector3.zero, 0);
	}
	
	void OnDestroy () {
	}
		
	/*public void ForwardsPulse () {

	}*/


	public void PrimalBackwardsPulse (Object sender) { 
		//print ("Primal Pulse - " + sender + " -> "  + gameObject.name);
		curPulseCount--;
		BackwardsPulse ();
	}


	public int reqPulseCount = 0;
	//int defPulseCount = 0;
	public int curPulseCount = 0;

	int n = 0;
	int m = 0;
	int k = 0;

	public void BackwardsPulse () { 
		if (reqPulseCount < curPulseCount)
			Debug.LogError ("too many backward calls");

		curPulseCount++;

		//print (curPulseCount + " - " + reqPulseCount + " - " + gameObject.name);

		if (reqPulseCount == curPulseCount) {		//if we recieved all the pulses we need
			for (int i = 0; i <= 3; i++) {																				//OUTPUT SORT
				if (outputStorage [n].n != -1) {
					if (outputStorage [n].myItem == null) {							//check if we have and item to give
						CycleArray (ref n);				

					} else if (feedingBelts [n] != null) {							//if there is a belt here
						if (feedingBelts [n].inputStorage [RevertLocation (n)].myItem == null) {	
							feedingBelts [n].inputStorage [RevertLocation (n)].myItem = outputStorage [n].myItem;
							feedingBelts [n].UpdateItemLocations ();
							outputStorage [n].myItem = null;
							break;
						} else {
							CycleArray (ref n);
						}
					} else if (feedingItems [n] != null) {							//if there is a item here
						if (feedingItems [n].TryAccepting (outputStorage [n].myItem, outputStorage[n].offSet)) {
							outputStorage [n].myItem = null;
							break;
						} else {
							CycleArray (ref n);
						}
					} else {														//if there is nothing here
						CycleArray (ref n);
					}
				} else {
					CycleArray (ref n);
				}
			}
			CycleArray (ref n);


			if (middleStorage.myItem != null) {		//if there is an item in the middle
				for (int i = 0; i <= 3; i++) {																			//MIDDLE SORT
					if (outputStorage [m].n != -1) {	//cycle
						if (outputStorage [m].myItem == null) {
							outputStorage [m].myItem = middleStorage.myItem;
							middleStorage.myItem = null;
							CycleArray (ref m);
							break;
						} else {
							CycleArray (ref m);
						}
					} else {
						CycleArray (ref m);
					}
				}
			}

			if (middleStorage.myItem == null) {	
				for (int i = 0; i <= 3; i++) {																				//INPUT SORT
					if (inputStorage [k].n != -1) {
						if (inputStorage [k].myItem != null) {	//if input have item
							middleStorage.myItem = inputStorage [k].myItem;
							inputStorage [k].myItem = null;
							break;
						} else {
							CycleArray (ref k);
						}
					} else {
						CycleArray (ref k);
					}
				}
				CycleArray (ref k);
			}

			UpdateItemLocations ();

			curPulseCount = 0;
			SendBackwardsPulse ();												//our job here is done
		} else {


		}
	}


	void SendBackwardsPulse () {
		foreach (PlacedItemBaseScript pitem in inputItems) {
			if (pitem != null) {
				//print (gameObject.name + " -> " + pitem.gameObject.name);
				pitem.BackwardsPulse ();
			}
		}

		foreach (BeltScript myBelt in inputBelts) {
			if (myBelt != null) {
				//print (gameObject.name + " -> "  + myBelt.gameObject.name);
				myBelt.BackwardsPulse ();
			}
		}
	}


	public void UpdateItemLocations () {

		if (middleStorage != null) {
			if (middleStorage.myItem != null) {
				middleStorage.myItem.placeToBe = transform.position + middleStorage.offSet;
			}
		}

		for (int i = 0; i <= 3; i++) {
			
			if (outputStorage [i].n != -1) {
				if (outputStorage [i].myItem != null) {
					outputStorage [i].myItem.placeToBe = transform.position + outputStorage [i].offSet;
				}
			}

			if (inputStorage [i].n != -1) {
				if (inputStorage [i].myItem != null) {	
					inputStorage [i].myItem.placeToBe = transform.position + inputStorage [i].offSet;
				}
			}

		}

		/*if (toBeGone != null) {
			if (toBeGone.myItem != null) {
				toBeGone.myItem.transform.position = transform.position + (toBeGone.offSet * toBeGoneMult) + new Vector3 (0, 0, -0.5f);
			}
		}*/
	}

	[System.Serializable]
	public class Place {

		public MovingObject myItem;
		public Vector3 offSet = Vector3.zero;
		public int n = -1;

		public Place (Vector3 myOffset, int m){
			offSet = myOffset;
			n = m;
			myItem = null;
		}
	}

	public void UpdatePulseControls () {

		BeltPulseControl.pulseEvent -= PrimalBackwardsPulse;

		//print ("UpdatePulseControls - " + gameObject.name);
		//print ("update pulse count");
		reqPulseCount = 0;
		curPulseCount = 0;
		//print ("NEW BELT " + gameObject.name + " - " + reqPulseCount);
		for (int i = 0; i <= 3; i++) {					//if not find any other
			if (feedingBelts [i] != null) {
				reqPulseCount++;
				//print ("pulse count increased " + reqPulseCount);
			}
		}
		/*for (int i = 0; i <= 3; i++) {					//if not find any other
			if (feedingItems [i] != null) {
				reqPulseCount--;
				print ("pulse count decreased " + reqPulseCount);
			}
		}*/

		curPulseCount = 0;
		if (reqPulseCount == 0) {
			BeltPulseControl.pulseEvent += PrimalBackwardsPulse;
		}

		if (reqPulseCount < curPulseCount)
			Debug.LogError ("some weird shit happened");
	}

	public void UpdateGraphic () {

		for (int i = 0; i <= 3; i++) {					//if not find any other
			if(inputStorage [i].myItem != null)
				Destroy (inputStorage [i].myItem.gameObject);
			inputStorage [i] = new Place (new Vector3 (0, 1, 0), -1);

			if (outputStorage [i].myItem != null)
				Destroy (outputStorage [i].myItem.gameObject);
			outputStorage [i] = new Place (new Vector3 (0, 1, 0), -1);
		}

		if(middleStorage.myItem != null)
			Destroy (middleStorage.myItem.gameObject);
		middleStorage = new Place (Vector3.zero, 0);

		foreach (GameObject gam in oldShit) {
			if(gam != null)
				Destroy (gam.gameObject);
		}
		//do amele shit
		int n = 0;
		oldShit [n] = (GameObject)Instantiate (mySet.b_middle, transform.position, transform.rotation);
		oldShit [n].transform.parent = transform;
		n++;

		for (int i = 0; i <= 3; i++) {
			if (inLocations [i]) {
				
				oldShit [n] = (GameObject)Instantiate (mySet.b_in, transform.position, transform.rotation);
				oldShit [n].transform.parent = transform;
				switch (i) {
				case 0:
					oldShit [n].transform.Translate (new Vector3 (-mySet.offset, 0, 0));
					inputStorage [0] = new Place (new Vector3 (-itemOffset, 0, 0), n);
					break;
				case 1:
					oldShit [n].transform.Translate (new Vector3 (0, mySet.offset, 0));
					inputStorage [1] = new Place (new Vector3 (0, itemOffset, 0), n);
					break;
				case 2:
					oldShit [n].transform.Translate (new Vector3 (mySet.offset, 0, 0));
					inputStorage [2] = new Place (new Vector3 (itemOffset, 0, 0), n);
					break;
				case 3:
					oldShit [n].transform.Translate (new Vector3 (0, -mySet.offset, 0));
					inputStorage [3] = new Place (new Vector3 (0, -itemOffset, 0), n);
					break;
				default:
					print ("cry like a bitch");
					break;
				}
				n++;
			}

			if (outLocations [i]) {

				oldShit [n] = (GameObject)Instantiate (mySet.b_out, transform.position, transform.rotation);
				oldShit [n].transform.parent = transform;
				switch (i) {
				case 0:
					oldShit [n].transform.Translate (new Vector3 (-mySet.offset, 0, 0));
					outputStorage [0] = new Place (new Vector3 (-itemOffset, 0, 0), n);
					break;
				case 1:
					oldShit [n].transform.Translate (new Vector3 (0, mySet.offset, 0));
					outputStorage [1] = new Place (new Vector3 (0, itemOffset, 0), n);
					break;
				case 2:
					oldShit [n].transform.Translate (new Vector3 (mySet.offset, 0, 0));
					outputStorage [2] = new Place (new Vector3 (itemOffset, 0, 0), n);
					break;
				case 3:
					oldShit [n].transform.Translate (new Vector3 (0, -mySet.offset, 0));
					outputStorage [3] = new Place (new Vector3 (0, -itemOffset, 0), n);
					break;
				default:
					print ("cry like a bitch");
					break;
				}
				n++;
			}
		}

		UpdatePulseControls ();
	}

	int RevertLocation(int location){
		//return location;

		switch (location) {
		case 0:
			return 2;
			break;
		case 1:
			return 3;
			break;
		case 2:
			return 0;
			break;
		case 3:
			return 1;
			break;
		default:
			Debug.LogError("given wrong number: " + location);
			return -1;
			break;
		}
	}

	void CycleArray (ref int number){
		if (number == 3) {
			number = 0;
		} else {
			number++;
		}
	}
}
