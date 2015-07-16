using UnityEngine;
using System.Collections;

public class BirdControl : MonoBehaviour {

	[HideInInspector]
	public Rigidbody2D player;

	bool facingRight = false;
	float h = 0f;
	public float speed = 5.0f;

	// jumping variables
	bool jump = false;
	int  jumpsRemaining = 4;
	const int jumpsMax  = 4;
	public float jumpForce = 1000f;

	bool grounded = false;
	Transform groundCheck;

	// Use this for initialization
	void Awake () {
		player = GetComponent<Rigidbody2D>();
		groundCheck = player.transform.Find("groundCheck");
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
			if (jumpsRemaining > 0) {
				Debug.Log ("jump!");
				jumpsRemaining--;
				player.AddForce(new Vector2(0, jumpForce));
			}
			jump = false;
		}
	}

	void Flip(Rigidbody2D player) {
		facingRight = !facingRight;

		Vector3 scalingFactor = player.transform.localScale;
		scalingFactor.x *= -1;
		player.transform.localScale = scalingFactor;
	}
}
