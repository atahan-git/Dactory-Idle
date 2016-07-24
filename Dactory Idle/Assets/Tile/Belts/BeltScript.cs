using UnityEngine;
using System.Collections;

public class BeltScript : MonoBehaviour {

	[HideInInspector]
	public Place[] inputStorage = new Place[4];
	[HideInInspector]
	public Place[] outputStorage = new Place[4];
	[HideInInspector]
	public Place middleStorage;
	[HideInInspector]
	public Place toBeGone;

	[HideInInspector]
	public PlacedItemBaseScript[] feedingItems = new PlacedItemBaseScript[4];
	//[HideInInspector]
	public BeltScript[] feedingBelts = new BeltScript[4];

	[HideInInspector]
	public bool[] inLocations = new bool[4];
	[HideInInspector]
	public bool[] outLocations = new bool[4];

	public int x = 0;
	public int y = 0;

	[HideInInspector]
	public GameObject tileCovered;

	public BeltSet mySet;

	float itemOffset = 0.25f;
	float toBeGoneMult = 1.5f;

	[HideInInspector]
	public GameObject[] oldShit = new GameObject[20];

	// Use this for initialization
	void Start () {
		middleStorage = new Place (Vector3.zero, 0);
		BeltPulseControl.firstPulse += FirstPulse;
		BeltPulseControl.secondPulse += SecondPulse;
	}
	
	void OnDestroy () {
		BeltPulseControl.firstPulse -= FirstPulse;
		BeltPulseControl.secondPulse -= SecondPulse;
	}

	int n = 0;
	int m = 0;
	int k = 0;
	public void FirstPulse () {
		int sPlace = -1;
		if (toBeGone == null) {								//no item to be gone

			for (int i = 0; i <= 3; i++) {
				if (outputStorage [i] != null) {
					if (outputStorage [i].myItem != null) {	//find and conveyor going clockwise
						if (i != n) {
							sPlace = i;
							n = i;
							break;
						}
					}
				}
			}
			if (sPlace == -1)								//if cant find set the last one
				sPlace = n;

			if (outputStorage [sPlace] != null) {			//set to be gone
				toBeGone = outputStorage [sPlace];
				outputStorage [sPlace].myItem = null;

				if (middleStorage.myItem != null) {			//set that one
					outputStorage [sPlace].myItem = middleStorage.myItem;
					middleStorage.myItem = null;
				}
			}
		}

		if (middleStorage.myItem != null) {					//if middle is full but there is an empty out conveyor fill it
			for (int i = 0; i <= 3; i++) {
				if (outputStorage [i] != null) {
					if (outputStorage [i].myItem == null) {
						if (i != m) {
							outputStorage [i].myItem = middleStorage.myItem;
							middleStorage.myItem = null;
							m = i;
							break;
						}
					}
				}
			}
			if (middleStorage.myItem != null) {				//hardcore shit m8 prob gonna fail but....... yeah
				if (outputStorage [m] != null) {
					if (outputStorage [m].myItem == null) {
						outputStorage [m].myItem = middleStorage.myItem;
						middleStorage.myItem = null;
					}
				}
			}
		}

		if (middleStorage.myItem == null) {					//if middle is empty try to fill it
			sPlace = -1;
			for (int i = 0; i <= 3; i++) {
				if (inputStorage [i] != null) {
					if (inputStorage [i].myItem != null) {	//find and conveyor going clockwise
						if (i != k) {
							sPlace = i;
							k = i;
							break;
						}
					}
				}
			}
			if (sPlace == -1)								//if cant find set the last one
				sPlace = k;
			
			if (inputStorage [sPlace] != null) {			//fill the middle
				middleStorage.myItem = inputStorage [sPlace].myItem;
				inputStorage [sPlace].myItem = null;

			}
		}
	}

	public void SecondPulse (){ 
		
		if (toBeGone == null)
			return;
		if (feedingBelts [n] != null) {					//first check if n exist and empty
			if (feedingBelts [n].inputStorage [RevertLocation (n)] != null) {
				if (feedingBelts [n].inputStorage [RevertLocation (n)].myItem == null) {
					feedingBelts [n].inputStorage [RevertLocation (n)].myItem = toBeGone.myItem;
					toBeGone = null;
					feedingBelts [n].UpdateItemLocations ();
				}
			}
		}
		if (toBeGone == null)
			return;
		for (int i = 0; i <= 3; i++) {					//if not find any other
			if (feedingBelts [i] != null) {
				if (feedingBelts [i].inputStorage [RevertLocation (i)] != null) {
					if (feedingBelts [i].inputStorage [RevertLocation (i)].myItem == null) {
						feedingBelts [i].inputStorage [RevertLocation (i)].myItem = toBeGone.myItem;
						toBeGone = null;
						feedingBelts [i].UpdateItemLocations ();
						break;
					}
				}
			}

			if (feedingItems [i] != null) {
				if (feedingItems [i].TryAccepting(toBeGone.myItem)) {
					toBeGone = null;
					break;
				}
			}
		}

		UpdateItemLocations ();
	}

	public void UpdateItemLocations () {

		if (middleStorage != null) {
			if (middleStorage.myItem != null) {
				middleStorage.myItem.transform.position = transform.position + middleStorage.offSet;
			}
		}

		for (int i = 0; i <= 3; i++) {
			
			if (outputStorage [i] != null) {
				if (outputStorage [i].myItem != null) {
					outputStorage [i].myItem.transform.position = transform.position + outputStorage [i].offSet;
				}
			}

			if (inputStorage [i] != null) {
				if (inputStorage [i].myItem != null) {	
					inputStorage [i].myItem.transform.position = transform.position + inputStorage [i].offSet;
				}
			}

		}

		if (toBeGone != null) {
			if (toBeGone.myItem != null) {
				toBeGone.myItem.transform.position = transform.position + (toBeGone.offSet * toBeGoneMult) + new Vector3 (0, 0, -0.5f);
			}
		}
	}

	//[System.Serializable]
	public class Place {

		public GameObject myItem;
		public Vector3 offSet = Vector3.zero;
		public int n = 0;

		public Place (Vector3 myOffset, int m){
			offSet = myOffset;
			n = m;
			myItem = null;
		}
	}


	public void UpdateGraphic () {

		for (int i = 0; i <= 3; i++) {					//if not find any other
			if (inputStorage [i] != null) {
				inputStorage [i] = null;
			}
			if (outputStorage [i] != null) {
				outputStorage [i] = null;
			}
		}
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
}
