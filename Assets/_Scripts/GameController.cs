using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace SlimeKingdom{
	public class GameController : MonoBehaviour {


		public GameObject pixelObject;
		public GameObject gridParent;

		public int cellSize = 1;
		public int gridSize = 1;

		public Text progressText;
		private float progressValue = 0.0f;

		private GameObject[,] grid;

		private GameObject[] allPixels;
		private Vector2 gridParentPosition;

		//TODO: Keep track of player movement on the Grid. Enable / Disable Tile colliders based on this, to allow selection. Make UI for this.
		//TODO: Tiles own class for better code.
		//TODO: Turn logic, alternating between N players. Make UI for this.
		//TODO: King logic, placing different types of units while moving

		public Texture2D tex;


		public void CreateImageByPixels(){

			ClearImage ();

			float pixelSize = pixelObject.transform.localScale.x;

			Color[] imagePixels = tex.GetPixels ();
			System.Array.Reverse (imagePixels);

			gridParent.transform.DOMove (new Vector2(gridParentPosition.x + (pixelSize * tex.height)/2.0f, gridParentPosition.y + (pixelSize * tex.width)/2.0f), 0.9f);

			for (int i = 0; i < tex.height; i++) {
				for (int j = 0; j < tex.width; j++) {

					int pixelIndex = (i * tex.width) + j;

					//Debug.Log (pixels[pixelIndex]);
					//Debug.Log ("i = " + i);
					//Debug.Log ("j = " + j);

					if (imagePixels [pixelIndex].a <= 0.0f)
						continue;

					StartCoroutine(SpawnPixels(i, j, pixelIndex, imagePixels, pixelSize));

				}
			}


		}

		IEnumerator SpawnPixels(int i, int j, int pixelIndex, Color[] imagePixels, float pixelSize){
			
			yield return new WaitForSeconds (Random.Range(0.1f, 0.4f));

			var newPixel = Instantiate (pixelObject, gridParent.transform);
			var pixel = newPixel.GetComponent<Pixel> ();

			var color = new Color (imagePixels [pixelIndex].r, imagePixels [pixelIndex].g, imagePixels [pixelIndex].b);
			pixel.SetColor (color);

			newPixel.transform.DOLocalMove (new Vector2 ((pixelSize) * -j, (pixelSize) * -i), 0.4f).SetDelay(0.02f * j);

		}

		public void ToggleImageColliders(){

			GetAllChildren ();

			foreach (GameObject child in allPixels) {
				
				var pixel = child.GetComponent<Pixel> ();
				pixel.ToggleCollider ();

			}
		}

		private void ClearImage(){

			GetAllChildren ();

			foreach (GameObject child in allPixels) {
				Destroy (child.gameObject);
			}
		}
			
		private void GetAllChildren(){
			
			allPixels = new GameObject[gridParent.transform.childCount];
			int count = 0;

			foreach (Transform child in gridParent.transform) {
				allPixels [count] = child.gameObject;
				count++;
			}
		}

		private void GetProgress(){

			int paintedPixels = 0;

			foreach (GameObject pixel in allPixels) {
				if (pixel.GetComponent<Pixel> ().isPainted)
					paintedPixels++;
			}
				
			progressValue = (float) (paintedPixels / (float) allPixels.Length) * 100;
			progressText.text = progressValue.ToString("F1") + "%";
		}

		private void CheckForPlayerInput(){
			
			if (Input.GetMouseButton (0) || Input.GetMouseButtonDown (0)) {
				Ray r = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit2D[] hits = Physics2D.CircleCastAll(r.origin, 0.4f, r.direction);


				foreach(RaycastHit2D hit in hits){
					if (hit){
						if (hit.collider.gameObject.tag == "Tile") {
							var pixel = hit.collider.gameObject.GetComponent<Pixel> ();
							pixel.PaintColor ();
							GetProgress ();
						}
					}
				}

			}

		}

		// Use this for initialization
		void Start () {
			
			gridParentPosition = gridParent.transform.position;
			CreateImageByPixels ();

			progressText.text = "0%";
			//CreateBattlefieldGrid ();
		}
		
		// Update is called once per frame
		void Update () {
			CheckForPlayerInput ();
		}

		public void CreateBattlefieldGrid(){

			grid = new GameObject[gridSize, gridSize];

			var tParent = gridParent.transform;
			tParent.DOMove (new Vector2(tParent.position.x + (cellSize * -gridSize)/2.0f, tParent.position.y + (cellSize * gridSize)/2.0f), 0.9f);

			for (int i = 0; i < gridSize; i++) {
				for (int j = 0; j < gridSize; j++) {

					var newTile = Instantiate (pixelObject);
					newTile.transform.parent = gridParent.transform;

					grid [i, j] = newTile;

					var t = newTile.transform;
					t.DOLocalMove (new Vector2 ((t.position.x + cellSize) * j, (t.position.y + cellSize) * -i), 0.4f).SetDelay (0.1f * j);


				}
			}

		}

	}
}