using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
	// Text variables
	private Text _levelText, _scoreText, _timerText;

	// Game variables
	private int _level;
	public int Level
	{
		get { return _level; }
		set
		{
			_level = value;
			_levelText.text = _level.ToString();
		}
	}

	private int _score;
	public int Score
	{
		get { return _score; }
		set
		{
			// Set value and update text
			_score = value;
			_scoreText.text = _score.ToString();
		}
	}

	public int LilypadsOccupied;

	private float _time = 99f;
	private CountdownTimer _countdownTimer;
	private Player _player;

	private GameObject _snake;

	// Audio components
	private AudioSource _audioSource;
	private AudioClip _winSound, _loseSound;

	private void Awake()
	{
		DontDestroyOnLoad(gameObject); // Allows audio to play during scene transition
	}

	private void Start()
	{
		// Get components
		_levelText = GameObject.Find("LevelText").GetComponent<Text>();
		_scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
		_timerText = GameObject.Find("TimerText").GetComponent<Text>();

		_player = GameObject.Find("Frog").GetComponent<Player>();
		_audioSource = GetComponent<AudioSource>();

		// Get audio clips
		_winSound = (AudioClip)Resources.Load("Audio/win");
		_loseSound = (AudioClip)Resources.Load("Audio/lose");

		ResetGame();
	}

	private void Update()
	{
		// Check if the timer is done
		_countdownTimer.Update();

		if (_countdownTimer.IsDone())
		{
			// Level failed (Game Over is automatically triggered by the player when lives == 0)
			_player.Die();
			// Reset timer
			ResetTimer();
		}
		else
		{
			// Update timer text
			if (_timerText != null)
				_timerText.text = _countdownTimer.Seconds.ToString("00");
		}

		// Check if all 5 lilypads have been occupied
		if (LilypadsOccupied == 5)
			NextLevel();
	}

	public void ResetTimer()
	{
		// Check level
		_countdownTimer.ResetClock();
		_time = Level > 1f ? 60f : 99f;
		_countdownTimer.Seconds = _time;
		_countdownTimer.Begin();
	}

	public void ResetGame()
	{
		// Set variables
		Level = 1;
		Score = 0;

		// Set timer
		_countdownTimer = new CountdownTimer { Seconds = _time };
		_countdownTimer.Begin();
	}

	public void NextLevel()
	{
		// Play sound
		PlaySound("Win");

		// Reset lilypads occupied
		LilypadsOccupied = 0;

		// Reset timer
		ResetTimer();

		// Update level
		Level++;

		// Check level
		if (Level >= 3)
		{
			// Show snake
			if (_snake == null)
			{
				_snake = Resources.Load("Prefabs/Snake") as GameObject;
				Instantiate(_snake);
			}
		}

		// Add a life
		_player.Lives++;
	}

	public void GameOver()
	{
		// Play sound
		PlaySound("Lose");
		// Return to the main menu
		SceneManager.LoadSceneAsync("Menu");
	}

	public void PlaySound(string sound)
	{
		switch (sound)
		{
			case "Win":
				_audioSource.clip = _winSound;
				break;
			case "Lose":
				_audioSource.clip = _loseSound;
				break;
		}

		_audioSource.Play();
	}
}