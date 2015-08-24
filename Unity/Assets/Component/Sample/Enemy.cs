using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

	void OnCollisionEnter2D (Collision2D collision)
	{
		string layerName = LayerMask.LayerToName (collision.gameObject.layer);
		if (layerName == "Floor" || layerName == "Ball") {
			Destroy (gameObject);
		}
	}
}
