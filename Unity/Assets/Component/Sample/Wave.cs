using UnityEngine;
using System.Collections;

public class Wave : MonoBehaviour
{

	void Update ()
	{
		if (transform.position.y <= -6.0f) {
			Destroy (gameObject);
		}
	}

	public void OnMove ()
	{
		iTween.MoveTo (gameObject, iTween.Hash (
			"y", transform.position.y - 8.0f,
			"time", 1.0f,
			"easetype", iTween.EaseType.easeOutSine
		));
	}
}
