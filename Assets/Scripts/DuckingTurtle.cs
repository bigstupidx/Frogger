using UnityEngine;

public class DuckingTurtle : MonoBehaviour
{
	// Components
	private Obstacle _obstacle;
	private Player _player;
	private SpriteRenderer _spriteRenderer;

	private void Start()
	{
		// Cache components
		_obstacle = GetComponent<Obstacle>();
		_player = GameObject.Find("Frog").GetComponent<Player>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		// Check if the turtle is ducking whilst the player is travelling on it, if so kill the player
		if (_player.Travelling && 
			Obstacle.PlayerLastTrigger == transform && 
			_spriteRenderer.sprite == null)
		{
			// Kill player
			_player.Die();
		}
	}
}