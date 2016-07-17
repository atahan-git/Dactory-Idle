using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Grid : MonoBehaviour {

	public static Grid s;

	public float gridScaleX = 1f;
	public float gridScaleY = 1f;

	public int gridSizeX = 10;
	public int gridSizeY = 5;

	public Color color = Color.white;

	public TileSet tileSet;

	GameObject[,] allTiles = new GameObject[10,10];
	public GameObject emptyTile;

	public enum TileTypes {dirt, grass, empty};

	public TileTypes myType = TileTypes.empty;

	public void Awake(){
		s = this;
	}

	void Update(){
		//transform.position = Vector3.up;
		if (Application.isPlaying) {


		} else {



		}

	}

	public void UpdateTileSize(){
		s = this;
		DeleteAllTiles ();
		allTiles = new GameObject[gridSizeX,gridSizeY];
		ResetGrid ();
	}

	public void ResetGrid(){
		DeleteAllTiles ();

		for (int x = 0; x < allTiles.GetLength (0); x++) {
			for (int y = 0; y < allTiles.GetLength (1); y++) {

				GameObject myTile = (GameObject)Instantiate (emptyTile, transform.position, transform.rotation);
				allTiles [x, y] = myTile;

				TileBaseScript myTileScript = myTile.GetComponent<TileBaseScript> ();

				myTileScript.x = x;
				myTileScript.y = y;
				myTileScript.UpdateLocation ();

				myTileScript.transform.parent = transform;
			}
		}
	}

	void DeleteAllTiles(){
		foreach(GameObject gam in allTiles){
			DestroyImmediate (gam);
		}

		GameObject[] myChildren = new GameObject[transform.childCount];
		int n = 0;
		foreach (Transform child in transform) {
			myChildren [n] = child.gameObject;
			n++;
		}

		foreach(GameObject gam in myChildren){
			DestroyImmediate (gam);
		}
	}

	void OnDrawGizmos(){
		//Vector3 pos = Camera.current.transform.position;
		Gizmos.color = this.color;

		for (int i = 0; i <= gridSizeX; i++) {
			Gizmos.DrawLine (transform.position + new Vector3 (i * gridScaleX, 0, 0), 
				transform.position + new Vector3 (i * gridScaleX, 0, 0) + new Vector3 (0, gridSizeY * gridScaleY, 0));
		}
		for (int i = 0; i <= gridSizeY; i++) {
			Gizmos.DrawLine (transform.position + new Vector3 (0, i * gridScaleY, 0), 
				transform.position + new Vector3 (0, i * gridScaleY, 0) + new Vector3 (gridSizeX * gridScaleX, 0, 0));
		}

		//origin
		Gizmos.color = Color.red;
				Gizmos.DrawSphere (transform.position, gridScaleX / 5);
	}

	public void ClickTile(GameObject tile){
		s = this;
		//print (tile);

		TileBaseScript myTileScript = tile.GetComponent<TileBaseScript> ();

		int x = myTileScript.x;
		int y = myTileScript.y;

		DestroyImmediate (allTiles [x, y].gameObject);

		GameObject tileToSpawn;

		switch (myType) {
		case TileTypes.empty:
			tileToSpawn = emptyTile;
			break;
		case TileTypes.dirt:
			tileToSpawn = tileSet.prefabs [0];
			break;
		case TileTypes.grass:
			tileToSpawn = tileSet.prefabs [1];
			break;
		default:
			tileToSpawn = emptyTile;
			break;
		}

		GameObject myTile = (GameObject)Instantiate (tileToSpawn, transform.position, transform.rotation);
		allTiles [x, y] = myTile;

		myTileScript = myTile.GetComponent<TileBaseScript> ();

		myTileScript.x = x;
		myTileScript.y = y;
		myTileScript.UpdateLocation ();

		myTileScript.transform.parent = transform;

	}

}
