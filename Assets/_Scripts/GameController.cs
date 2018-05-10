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
				t.DOLocalMove (new Vector2 ((t.position.x + cellSize) * j, (t.position.y + cellSize) * -i), 0.4f).SetDelay (0.1f*j);


			}
		}

	}

	// Use this for initialization
	void Start () {
		CreateBattlefieldGrid ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
