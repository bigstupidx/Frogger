using UnityEngine;

public class Boundary : MonoBehaviour
{
	public float ResetX;

	private void OnTriggerEnter2D(Collider2D other)
	{
		// Reset the position of the object
		Vector3 pos = other.transform.position;
		pos.x = ResetX;
		other.transform.position = pos;
	}
}