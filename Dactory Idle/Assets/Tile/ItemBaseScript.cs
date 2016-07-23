using UnityEngine;
using System.Collections;

public class ItemBaseScript : MonoBehaviour {

	//public bool[,] shape = new bool[2,2];
	//public MultBool[] shape = new MultBool[2];
	public ArrayLayout shape;
	Vector2 center = new Vector2 (3, 3);

	public GameObject myPlacedItem;

	public int x = 0;
	public int y = 0;

	public GameObject ItemSpritePrefab;

	public GameObject[] mySprites = new GameObject[50];

	bool isPlaceAble = true;
	// Use this for initialization
	void Start () {
		int n = 0;
		for (int y = 0; y < shape.rows.Length; y++) {
			for (int x = 0; x < shape.rows[y].row.Length; x++) {
				if (shape.rows[y].row[x]) {
					//print (x + " - " + y);
					Vector3 displacement = new Vector3 (x - center.x, y - center.y, 0);

					mySprites [n] = (GameObject)Instantiate (ItemSpritePrefab, transform.position + displacement, transform.rotation);
					mySprites [n].transform.parent = transform;
					ItemSprite mySprite = mySprites [n].GetComponent<ItemSprite> ();
					mySprite.x = (int)displacement.x;
					mySprite.y = (int)displacement.y;
					n++;
				}
			}
		}
		transform.rotation = Quaternion.Euler (0, 0, 180);
	}
	
	// Update is called once per frame
	void Update () {
		CheckPlaceable ();
	}

	public void PlaceSelf(){
		CheckPlaceable();

		if(isPlaceAble){
			GameObject InstantiatedItem = (GameObject)Instantiate (myPlacedItem, transform.position, Quaternion.identity);

			foreach (GameObject thisSprite in mySprites) {
				ItemSprite mySprite;
				try{
					mySprite = thisSprite.GetComponentInChildren<ItemSprite> ();
				}catch{
					return;
				}
				int checkX = x - mySprite.x;
				int checkY = y - mySprite.y;

				try{
					TileBaseScript myTile = Grid.s.myTiles [checkX, checkY].GetComponent<TileBaseScript> ();
					myTile.itemPlaceable = false;
					myTile.areThereItem = true;
					myTile.myItem = InstantiatedItem;
				}catch{}
			}
		}
	}

	void CheckPlaceable(){
		isPlaceAble = true;
		foreach (GameObject thisSprite in mySprites) {
			ItemSprite mySprite;
			try{
				mySprite = thisSprite.GetComponentInChildren<ItemSprite> ();
			}catch{
				return;
			}
			int checkX = x - mySprite.x;
			int checkY = y - mySprite.y;

			//print (checkX + " - " + checkY);

			bool myVal = false;

			try{
				myVal = Grid.s.myTiles [checkX, checkY].GetComponent<TileBaseScript> ().itemPlaceable;
			}catch{

			}

			if (myVal) {
				mySprite.Placeable ();
			} else {
				mySprite.CantPlace ();
				isPlaceAble = false;
			}
		}
	}
	/*[System.Serializable]
	public class MultBool{
		public bool[] boolArr = new bool[2];
	}*/
}
