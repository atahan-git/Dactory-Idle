using UnityEngine;
using System.Collections;

public class BeltScript : MonoBehaviour {

	public GameObject[] items = new GameObject[10];

	public PlacedItemBaseScript[] feedingItems = new PlacedItemBaseScript[4];
	public int n_item = 0;
	public BeltScript[] feedingBelts = new BeltScript[4];
	public int n_belt = 0;

	public bool[] inLocations = new bool[4];
	public bool[] outLocations = new bool[4];

	public int x = 0;
	public int y = 0;

	public GameObject tileCovered;

	public BeltSet mySet;

	[HideInInspector]
	public GameObject[] oldShit = new GameObject[20];

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ForwardPulse(){


	}

	public void BackwardsPulse(){

	}


	public void UpdateGraphic () {

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
					break;
				case 1:
					oldShit [n].transform.Translate (new Vector3 (0, mySet.offset, 0));
					break;
				case 2:
					oldShit [n].transform.Translate (new Vector3 (mySet.offset, 0, 0));
					break;
				case 3:
					oldShit [n].transform.Translate (new Vector3 (0, -mySet.offset, 0));
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
					break;
				case 1:
					oldShit [n].transform.Translate (new Vector3 (0, mySet.offset, 0));
					break;
				case 2:
					oldShit [n].transform.Translate (new Vector3 (mySet.offset, 0, 0));
					break;
				case 3:
					oldShit [n].transform.Translate (new Vector3 (0, -mySet.offset, 0));
					break;
				default:
					print ("cry like a bitch");
					break;
				}
				n++;
			}
		}
	}
}
