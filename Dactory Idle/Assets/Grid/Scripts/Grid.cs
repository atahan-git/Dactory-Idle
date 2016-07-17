using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Grid : MonoBehaviour {

	public float gridScaleX = 1f;
	public float gridScaleY = 1f;

	public int gridSizeX = 10;
	public int gridSizeY = 5;

	public Color color = Color.white;

	public Transform tilePrefab;

	public TileSet tileSet;

	public bool draggable;

	void Update(){
		//transform.position = Vector3.up;



	}

	void OnDrawGizmos(){
		Vector3 pos = Camera.current.transform.position;
		Gizmos.color = this.color;


		/*for(float y = pos.y - 800.0f; y < pos.y + 800.0f; y+= this.gridScaleY){
			Gizmos.DrawLine(new Vector3(-1000000.0f, Mathf.Floor(y/this.gridScaleY)*this.gridScaleY, 0.0f), 
			                new Vector3(1000000.0f, Mathf.Floor(y/this.gridScaleY)*this.gridScaleY, 0.0f));
		}

		for(float x = pos.x -1200.0f; x < pos.x + 1200.0f; x+= this.gridScaleX){
			Gizmos.DrawLine(new Vector3(Mathf.Floor(x/this.gridScaleX)*this.gridScaleX,-1000000.0f, 0.0f), 
			                new Vector3(Mathf.Floor(x/this.gridScaleX)*this.gridScaleX, 1000000.0f, 0.0f));
		}*/

		for (int i = 0; i <= gridSizeX; i++) {
			Gizmos.DrawLine (transform.position + new Vector3 (i * gridScaleX, 0, 0), 
				transform.position + new Vector3 (i * gridScaleX, 0, 0) + new Vector3 (0, gridSizeY * gridScaleY, 0));
		}

		for (int i = 0; i <= gridSizeY; i++) {
			Gizmos.DrawLine (transform.position + new Vector3 (0, i * gridScaleY, 0), 
				transform.position + new Vector3 (0, i * gridScaleY, 0) + new Vector3 (gridSizeX * gridScaleX, 0, 0));
		}

		Gizmos.color = Color.red;
				Gizmos.DrawSphere (transform.position, gridScaleX / 5);

	}

}
