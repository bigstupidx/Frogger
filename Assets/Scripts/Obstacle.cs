using UnityEngine;

public class Obstacle : MonoBehaviour
{
	private Game _game;

	private Player _player;
	private bool _occupied;

	[Tooltip("The number of score points this object is worth")]
	public int Points = 10;

	private float _travelDiffX;
	public static Transform PlayerLastTrigger;

	private void Start()
	{
		// Cache game
		_game = GameObject.Find("GameManager").GetComponent<Game>();
		// Get components
		_player = GameObject.Find("Frog").GetComponent<Player>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		// Check player collisions
		if (other.CompareTag("Player"))
		{
			// Update last trigger
			PlayerLastTrigger = transform;

			if (gameObject.CompareTag("Lilypad"))
				Lilypad();
			else if (gameObject.CompareTag("Walkable"))
			{
				// Stick player to object
				UpdateTravelDiff();
			}
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		// Check player collisions
		if (other.CompareTag("Player"))
		{
			if (gameObject.CompareTag("Walkable"))
			{
				// Stick player to object
				if (!_player.Leaping)
				{
					Vector3 newPos = transform.position;
					newPos.x += -_travelDiffX;
					_player.transform.position = newPos;
				}
				else
				{
					// Calculate new travel difference
					UpdateTravelDiff();
				}
			}
			else if (gameObject.CompareTag("Danger"))
			{
				Danger();
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		// Check player collisions
		if (other.CompareTag("Player"))
		{
			if (gameObject.CompareTag("Walkable"))
			{
				_player.Travelling = false;
			}
		}
	}

	private void Danger()
	{
		// Check if the player is still leaping or travelling
		if (_player.Leaping || _player.Travelling) return;

		// Die
		_player.Die();
	}

	private void Lilypad()
	{
		// Check if lilypad is occupied
		if (_occupied)
		{
			_player.Die(); // Die
			return;
		}

		// Display landed frog
		GameObject landedFrog = Instantiate((GameObject)Resources.Load("Prefabs/Landed Frog"), transform.position, Quaternion.identity, GameObject.Find("Landed Frogs").transform);

		// Marked lilypad as occupied
		_occupied = true;

		// Add points
		_game.Score += Points;

		// Add to tracker
		_game.LilypadsOccupied++;

		// Reset player position
		_player.ResetPosition();
	}

	// Updates the travel distance
	private void UpdateTravelDiff()
	{
		// Calculate difference
		Vector3 playerPos = _player.transform.position;
		Vector3 pos = transform.position;
		_travelDiffX = pos.x - playerPos.x;

		// Set boolean
		_player.Travelling = true;
	}
}