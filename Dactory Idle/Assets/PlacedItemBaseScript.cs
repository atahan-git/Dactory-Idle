using UnityEngine;
using System.Collections;

public class PlacedItemBaseScript : MonoBehaviour {

	public BeltScript[] outConveyors = new BeltScript[99];
	public int n_out = 0;
	public BeltScript[] inConveyors = new BeltScript[99];
	public int n_in = 0;

	public TileBaseScript[] tilesCovered = new TileBaseScript[99];
	public int n_cover = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
