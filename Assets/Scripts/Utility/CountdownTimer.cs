using UnityEngine;

/// <summary>
/// Defines a countdown timer.
/// </summary>
public class CountdownTimer
{
	/// <summary>
	/// The paused state.
	/// </summary>
	public bool Paused = true;
	/// <summary>
	/// The number of seconds the timer has remaining.
	/// </summary>
	public float Seconds;

    private float _setSeconds;

	/// <summary>
	/// Starts the timer.
	/// </summary>
	public void Begin()
    {
        Paused = false;
        _setSeconds = Seconds;
    }

	/// <summary>
	/// Determines whether the timer is done.
	/// </summary>
	/// <returns>
	///   <c>true</c> if this instance is done; otherwise, <c>false</c>.
	/// </returns>
	public bool IsDone()
    {
        return Seconds <= 0;
    }

	/// <summary>
	/// Resets the clock.
	/// </summary>
	public void ResetClock()
    {
        Seconds = _setSeconds;
        Paused = true;
    }

	/// <summary>
	/// Updates this instance.
	/// </summary>
	public void Update()
    {
        if (!Paused || IsDone())
            Seconds -= Time.deltaTime;
    }
}