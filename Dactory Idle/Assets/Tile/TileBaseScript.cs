using UnityEngine;
using System.Collections;

public class TileBaseScript : MonoBehaviour {

	public int tileType = -1;
	public int x = 0;
	public int y = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateLocation(){
		if (Grid.s != null) {
			transform.position = Grid.s.transform.position +
			new Vector3 (x * Grid.s.gridScaleX + Grid.s.gridScaleX / 2,
				y * Grid.s.gridScaleY + Grid.s.gridScaleY / 2, 0);
		} else {

			Grid grid = GameObject.FindObjectOfType<Grid> ();

			transform.position = grid.transform.position +
				new Vector3 (x * grid.gridScaleX + grid.gridScaleX / 2,
					y * grid.gridScaleY + grid.gridScaleY / 2, 0);

		}
	}
}
