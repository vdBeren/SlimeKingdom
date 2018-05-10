using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameController : MonoBehaviour {


	public GameObject tileObject;
	public GameObject gridParent;

	public int cellSize = 1;
	public int gridSize = 1;

	private GameObject[,] grid;


	//TODO: Keep track of player movement on the Grid. Enable / Disable Tile colliders based on this, to allow selection. Make UI for this.
	//TODO: Tiles own class for better code.
	//TODO: Turn logic, alternating between N players. Make UI for this.
	//TODO: King logic, placing different types of units while moving

	public void CreateBattlefieldGrid(){

		grid = new GameObject[gridSize, gridSize];

		var tParent = gridParent.transform;
		tParent.DOMove (new Vector2(tParent.position.x + (cellSize * -gridSize)/2.0f, tParent.position.y + (cellSize * gridSize)/2.0f), 0.9f);
		
		for (int i = 0; i < gridSize; i++) {
			for (int j = 0; j < gridSize; j++) {

				var newTile = Instantiate (tileObject);
				newTile.transform.parent = gridParent.transform;

				grid [i, j] = newTile;

				var t = newTile.transform;
				t.DOLocalMove (new Vector2 ((t.position.x + cellSize) * j, (t.position.y + cellSize) * -i), 0.4f).SetDelay (0.1f * j);


			}
		}

	}

	public void CheckForPlayerInput(){
		
		if (Input.GetMouseButtonDown (0)) {
			Ray r = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(r.origin, r.direction, Mathf.Infinity);

			if (hit){
				if (hit.collider.gameObject.tag == "Tile") {
					SpriteRenderer sr = hit.collider.gameObject.GetComponent<SpriteRenderer> ();
					sr.DOColor (new Color (1, 0, 0), 0.4f);
				}
			}
		}

	}

	// Use this for initialization
	void Start () {
		CreateBattlefieldGrid ();
	}
	
	// Update is called once per frame
	void Update () {
		CheckForPlayerInput ();
	}

}
