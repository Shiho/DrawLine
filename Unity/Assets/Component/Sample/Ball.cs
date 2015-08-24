using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{

	void OnCollisionEnter2D (Collision2D collision)
	{
		string layerName = LayerMask.LayerToName (collision.gameObject.layer);
		if (layerName == "Floor") {
			Destroy (gameObject);
		}
	}
}
