using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pixel : MonoBehaviour {

	public Color originalColor { get; set; }
	public Color grayColor { get; set; }

	public bool isPainted { get; set;}

	private BoxCollider2D pixelCollider;
	private SpriteRenderer spriteRenderer;


	void Awake(){

		isPainted = false;

		pixelCollider = this.GetComponent<BoxCollider2D> ();
		spriteRenderer = this.GetComponent<SpriteRenderer> ();

	}



	public void SetColor(Color newColor){
		originalColor = newColor;
		grayColor = new Color(newColor.grayscale, newColor.grayscale, newColor.grayscale);

		spriteRenderer.color = grayColor;
	}

	public void PaintColor (){
		
		if (!isPainted)
			spriteRenderer.color = originalColor;

		isPainted = true;
;
	}

	public void ToggleCollider(){
		pixelCollider.enabled = !pixelCollider.enabled;
	}
}
