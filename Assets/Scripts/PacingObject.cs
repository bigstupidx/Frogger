using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacingObject : MonoBehaviour
{
	// Components
	private Rigidbody2D _rb2D;

	// Properties
	public float Speed;
	public float MinBoundX, MaxBoundX;

	private void Start()
	{
		// Cache components
		_rb2D = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		// Move the object left and right within the bounds
		Vector3 pos = transform.position;

		// Check bounds
		if (pos.x < MinBoundX || pos.x > MaxBoundX)
		{
			// Invert speed
			Speed = -Speed;

			// Invert sprite
			Vector3 scale = transform.localScale;
			scale.x = -scale.x;
			transform.localScale = scale;
		}

		// Move sprite
		_rb2D.velocity = new Vector2(Speed * Time.fixedDeltaTime, 0f);
	}
}
