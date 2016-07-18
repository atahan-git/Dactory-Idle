using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public class Grid : MonoBehaviour {

	public static Grid s;
	string saveName = "TestGrid";

	public float gridScaleX = 1f;
	public float gridScaleY = 1f;

	public int gridSizeX = 10;
	public int gridSizeY = 5;

	public Color color = Color.white;

	public TileSet tileSet;

	GameObject[,] myTiles = new GameObject[10,10];
	public GameObject emptyTile;
	public Tiles myTilesIDs = new Tiles();

	public enum TileTypes {dirt, grass, empty};

	public TileTypes myType = TileTypes.empty;

	public void Awake(){
		s = this;
		//UpdateTileSize ();

		if (!Load ()) {
			myTiles = new GameObject[gridSizeX, gridSizeY];
			myTilesIDs.tiles = new int[gridSizeX, gridSizeY];
		}

		DrawTiles ();
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
		myTiles = new GameObject[gridSizeX,gridSizeY];
		DrawTiles ();
	}

	public void DrawTiles(){
		DeleteAllTiles ();

		for (int x = 0; x < myTiles.GetLength (0); x++) {
			for (int y = 0; y < myTiles.GetLength (1); y++) {

				GameObject myTile = (GameObject)Instantiate (tileSet.prefabs[myTilesIDs.tiles [x, y]], transform.position, transform.rotation);
				myTiles [x, y] = myTile;

				TileBaseScript myTileScript = myTile.GetComponent<TileBaseScript> ();

				myTileScript.x = x;
				myTileScript.y = y;
				//myTileScript.mySet = tileSet;
				//myTileScript.tileType = myTilesIDs.tiles [x, y];
				myTileScript.UpdateLocation ();

				myTileScript.transform.parent = transform;
			}
		}
	}

	void DeleteAllTiles(){
		foreach(GameObject gam in myTiles){
			Destroy (gam);
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

		Destroy (myTiles [x, y].gameObject);

		GameObject tileToSpawn;

		switch (myType) {
		case TileTypes.empty:
			tileToSpawn = tileSet.prefabs [0];
			break;
		case TileTypes.dirt:
			tileToSpawn = tileSet.prefabs [1];
			break;
		case TileTypes.grass:
			tileToSpawn = tileSet.prefabs [2];
			break;
		default:
			tileToSpawn = emptyTile;
			break;
		}

		GameObject myTile = (GameObject)Instantiate (tileToSpawn, transform.position, transform.rotation);
		myTiles [x, y] = myTile;

		myTileScript = myTile.GetComponent<TileBaseScript> ();

		myTileScript.x = x;
		myTileScript.y = y;
		myTileScript.UpdateLocation ();

		myTileScript.transform.parent = transform;

	}

	void OnApplicationQuit(){
		Save ();
	}

	public void Save (){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/" + saveName + ".banana");

		Tiles data = myTilesIDs;

		bf.Serialize (file, data);
		file.Close ();
	}

	public bool Load(){
		if (File.Exists (Application.persistentDataPath + "/" + saveName + ".banana")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/" + saveName + ".banana", FileMode.Open);
			Tiles data = (Tiles)bf.Deserialize (file);
			file.Close ();

			myTilesIDs = data;
			myTiles = new GameObject[myTilesIDs.tiles.GetLength (0), myTilesIDs.tiles.GetLength (0)];
			return true;
		} else {
			return false;
		}
	}
}


[Serializable]
public class Tiles {
	public int[,] tiles;
}