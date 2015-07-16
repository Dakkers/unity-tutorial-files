using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BirdControl : MonoBehaviour {

	[HideInInspector]
	public Rigidbody2D player;

	bool facingRight = false;
	float h = 0f;
	public float speed = 5.0f;

	// jumping variables
	bool jump = false;
	int  jumpsRemaining = 2;
	const int jumpsMax  = 2;
	public float jumpForce = 1000f;

	bool grounded = false;
	Transform groundCheck;

	// UI stuff
	int score = 0;
	Text scoreText;

	// Use this for initialization
	void Awake () {
		player = GetComponent<Rigidbody2D>();
		groundCheck = player.transform.Find("groundCheck");
		scoreText = GameObject.Find("Canvas/ScoreLabel/ScoreText").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		h = Input.GetAxisRaw("Horizontal");

		grounded = Physics2D.Linecast(player.transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		if (Input.GetButtonDown("Jump"))
			jump = true;
	}

	void FixedUpdate() {
		if (h > 0) {
			player.velocity = new Vector2(speed, player.velocity.y);
			if (!facingRight)
				Flip(player);	
		} else if (h < 0) {
			player.velocity = new Vector2(-speed, player.velocity.y);
			if (facingRight)
				Flip(player);
		} else
			player.velocity = new Vector2(0.0f, player.velocity.y);

		if (jump) {
			jumpsRemaining = (grounded) ? jumpsMax : jumpsRemaining;
			// if (player.velocity.y < 0)
			// 	jumpsRemaining = Mathf.Min(1, jumpsRemaining);
			if (jumpsRemaining > 0) {
				jumpsRemaining--;
				player.AddForce(new Vector2(0, jumpForce));
			}
			jump = false;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("Entered trigger!");
		if (other.gameObject.CompareTag("Coin")) {
			score = score + other.gameObject.GetComponent<Coin>().score;
			scoreText.text = score.ToString();
			Destroy(other.gameObject);
		}
	}

	void Flip(Rigidbody2D player) {
		facingRight = !facingRight;

		Vector3 scalingFactor = player.transform.localScale;
		scalingFactor.x *= -1;
		player.transform.localScale = scalingFactor;
	}
}
