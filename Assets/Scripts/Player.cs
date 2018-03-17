using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	// Properties
	private Text _livesText;

	private int _lives = 3;
	public int Lives
	{
		get { return _lives; }
		set
		{
			// Update value and text
			_lives = value;
			_livesText.text = _lives.ToString();
		}
	}

	public bool Dead;
	public bool Leaping;
	public bool Travelling;

	public float MinBoundX, MaxBoundX, MinBoundY, MaxBoundY;

	// Variables
	private Animator _animator;
	private AudioSource _audioSource;
	public Rigidbody2D Rb2D;

	private Game _game;

	// Store frequently changed sounds
	private AudioClip _leapSound, _successSound, _deathSound;

	public Vector3 TargetPosition;

	private void Start()
	{
		// Cache game
		_game = GameObject.Find("GameManager").GetComponent<Game>();

		// Cache components
		_animator = GetComponent<Animator>();
		_audioSource = GetComponent<AudioSource>();
		Rb2D = GetComponent<Rigidbody2D>();

		// Cache audio clips
		_leapSound = (AudioClip) Resources.Load("Audio/leap");
		_successSound = (AudioClip) Resources.Load("Audio/success");
		_deathSound = (AudioClip)Resources.Load("Audio/death");

		// Initialise variables
		_livesText = GameObject.Find("LivesText").GetComponent<Text>();
		Lives = _lives;

		TargetPosition = transform.position;
	}

	private void Update()
	{
		// Check for input
		if (Input.GetKeyDown(KeyCode.W))
			Leap(Direction.Up);
		else if (Input.GetKeyDown(KeyCode.S))
			Leap(Direction.Down);
		else if (Input.GetKeyDown(KeyCode.A))
			Leap(Direction.Left);
		else if (Input.GetKeyDown(KeyCode.D))
			Leap(Direction.Right);

		// Check if the audio pitch needs resetting
		if (!_audioSource.isPlaying && Mathf.Approximately(_audioSource.pitch, 1f))
			_audioSource.pitch = 1;
	}

	private void LateUpdate()
	{
		// Check if the boundaries have been breached while travelling
		Vector3 pos = transform.position;
		float boundPadding = 0.6f;
		if (pos.x > MaxBoundX + boundPadding || pos.x < MinBoundX - boundPadding)
			Die();
	}

	private void FixedUpdate()
	{
		// Move player
		if (!Dead)
			transform.position = Vector2.MoveTowards(transform.position, TargetPosition, Time.fixedDeltaTime / 0.25f);

		// Check if player is still leaping
		if (transform.position == TargetPosition)
			Leaping = false;
	}

	private void Leap(Direction direction)
	{
		// Check if the player is still in the middle of a leap or dead
		if (Leaping || Dead) return;

		// Play animation
		_animator.Play("Leap");

		// Play sound
		if (_audioSource.clip != _leapSound)
			_audioSource.clip = _leapSound;

		// Set pitch
		_audioSource.pitch = 2;
		_audioSource.Play();

		// Move and rotate player
		Vector3 curRotEuler = transform.rotation.eulerAngles;

		// Changes depending on where the player is on the map;
		Vector3 curPos = transform.position;
		TargetPosition = curPos;

		float modifier = (curPos.y >= -2.408f && curPos.y <= -0.488f) ? 0.48f : 0.64f;

		switch (direction)
		{
			case Direction.Up:
				curRotEuler.z = 0;
				TargetPosition.y = Mathf.Clamp(TargetPosition.y + modifier, MinBoundY, MaxBoundY);
				break;
			case Direction.Down:
				curRotEuler.z = 180;
				TargetPosition.y = Mathf.Clamp(TargetPosition.y - modifier, MinBoundY, MaxBoundY);
				break;
			case Direction.Left:
				curRotEuler.z = 90;
				TargetPosition.x = Mathf.Clamp(TargetPosition.x - modifier, MinBoundX, MaxBoundX);
				break;
			case Direction.Right:
				curRotEuler.z = -90;
				TargetPosition.x = Mathf.Clamp(TargetPosition.x + modifier, MinBoundX, MaxBoundX);
				break;
		}

		// Set rotation
		transform.rotation = Quaternion.Euler(curRotEuler);

		// Hotfix for when the player jumps back to the starting position
		if (Mathf.Approximately(TargetPosition.y, -2.888f))
			TargetPosition.y = -3.048f;

		// Set variables
		Leaping = true;
		Travelling = false;
	}

	public void Die()
	{
		// Check if player is already dead
		if (Dead) return;

		// Play death animation
		_animator.Play("Death");

		// Play death sound
		_audioSource.clip = _deathSound;
		_audioSource.Play();

		// Set booleans
		Dead = true;
		Leaping = false;
		Travelling = false;

		// Subtract life
		Lives--;

		// Check for game over
		if (Lives <= 0)
			_game.GameOver();

		// Reset rotation
		transform.rotation = Quaternion.identity;

		// Reset position
		Invoke("ResetPosition", 1f);
	}

	public void ResetPosition()
	{
		// Reset position and target position
		Vector3 newPos = new Vector3(0.0f, MinBoundY, 0.0f);
		transform.position = newPos;
		TargetPosition = newPos;

		// Reset animation
		_animator.Play("Idle");

		// Reset dead boolean
		Dead = false;
	}
}