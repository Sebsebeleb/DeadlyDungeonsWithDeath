using UnityEngine;
using System.Collections;

public class AnimatedTexture : MonoBehaviour {

	public float speed = 0.08f;

	private float x = 0f;
	private float y = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		x += Time.deltaTime * speed;
		y += Time.deltaTime * speed;

		Vector2 offset = new Vector2(x, y);
		renderer.material.SetTextureOffset ("_MainTex", offset);
	}
}
