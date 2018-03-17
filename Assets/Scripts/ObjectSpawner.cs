using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
	public GameObject Spawnable;
	public int Amount = 3;
	public float Speed = 1;
	public float Spacing = 0;
	public Vector3 InitialSpawn;

	private List<GameObject> _spawned = new List<GameObject>();
	private GameObject _spawnedObjects;  // Create container for spawned objects

	private void Start()
	{
		_spawnedObjects = GameObject.Find("Spawned") ?? new GameObject("Spawned"); // Find or create spawned objects container
		// Create sub container for spawnable
		string subContainerName = Spawnable.name + "s";
		GameObject subContainer = GameObject.Find(subContainerName) ?? new GameObject(subContainerName);
		// Assign parent
		subContainer.transform.parent = _spawnedObjects.transform;

		// Create number of spawnables specified
		for (int i = 0; i < Amount; i++)
		{
			// Calculate position
			Vector3 initPos = InitialSpawn;
			initPos.x += Spacing * i;

			// Spawn
			GameObject obj = Instantiate(Spawnable, initPos, Quaternion.identity);
			// Assign parent
			obj.transform.parent = subContainer.transform;
			// Add to list
			_spawned.Add(obj);
		}
	}

	private void FixedUpdate()
	{
		// Apply velocity
		if (!Mathf.Approximately(Speed, 0f))
			foreach (var obj in _spawned)
				obj.GetComponent<Rigidbody2D>().velocity = new Vector2(Speed * Time.fixedDeltaTime, 0);
	}
}