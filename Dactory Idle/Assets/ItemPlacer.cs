using UnityEngine;
using System.Collections;

public class ItemPlacer : MonoBehaviour {

	public GameObject[] items;
	public GameObject curItem;
	public ItemBaseScript itemScript;

	public int curItemId = 0;

	Camera mycam;

	// Use this for initialization
	void Start () {
		mycam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
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
			if (curItem != null) {
				itemScript.PlaceSelf ();
				Destroy (curItem.gameObject);
				curItem = null;
				itemScript = null;
			}
		}
	}

	public void SelectItem (int id) {

		curItemId = id;
	}

	public void PlaceItem (int id) {
		print ("place item");
		curItemId = id;
		curItem = (GameObject)Instantiate (items[curItemId], transform.position, transform.rotation);
		itemScript = curItem.GetComponent<ItemBaseScript> ();
	}
}
