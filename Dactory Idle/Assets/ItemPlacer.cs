using UnityEngine;
using System.Collections;

public class ItemPlacer : MonoBehaviour {

	public GameObject beltPrefab;
	public GameObject[] items;
	public GameObject curItem;
	public ItemBaseScript itemScript;

	public bool isPlacingItem = false;
	public bool isMovementEnabled = true;

	public int curItemId = 0;

	Camera mycam;

	// Use this for initialization
	void Start () {
		mycam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isPlacingItem)
			return;

		if (Input.touchCount > 0) {
			Ray myRay = mycam.ScreenPointToRay (Input.GetTouch (0).position);
			RaycastHit hit = new RaycastHit ();
			if (Physics.Raycast (myRay, out hit)) {
				TileBaseScript tileS;
				try {
					tileS = hit.collider.gameObject.GetComponent<TileBaseScript> ();
				} catch {
					return;
				}

				itemScript.x = tileS.x;
				itemScript.y = tileS.y;
				curItem.transform.position = tileS.transform.position; 
			}
		} else {
			isPlacingItem = false;
			if (curItem != null) {
				itemScript.PlaceSelf ();
				Destroy (curItem.gameObject);
				curItem = null;
				itemScript = null;
			}
		}
	}

	public void PlaceItem (int id) {
		print ("place item");
		isPlacingItem = true;
		curItemId = id;
		curItem = (GameObject)Instantiate (items[curItemId], transform.position, Quaternion.identity);
		itemScript = curItem.GetComponent<ItemBaseScript> ();
	}

	//--------------------------------------------------------------------------------------------------BELT STUFF
	public void ActivateBeltMode(){
		isMovementEnabled = false;
	}
}
