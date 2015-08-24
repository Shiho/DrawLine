using UnityEngine;
using System.Collections;

public class Backgrounds : MonoBehaviour
{
	
	[SerializeField]
	private GameObject back;
	[SerializeField]
	private GameObject front;

	private float backUpY = 1.0f;
	private float backCurrentY = 0.0f;
	
	private float frontUpY = 0.7f;
	private float frontCurrentY = 0.0f;

	void Start ()
	{
		Vector2 backOffset = back.GetComponent<Renderer> ().sharedMaterial.GetTextureOffset ("_MainTex");
		Vector2 frontOffset = front.GetComponent<Renderer> ().sharedMaterial.GetTextureOffset ("_MainTex");
		backCurrentY = backOffset.y;
		frontCurrentY = frontOffset.y;
	}

	public void OnUpMove ()
	{
		iTween.ValueTo (gameObject, iTween.Hash (
			"from", backCurrentY,
			"to", backCurrentY + backUpY,
			"time", 1.0f,
			"easetype", iTween.EaseType.easeOutSine,
			"onupdate", "backUpdateHandler"
		));

		iTween.ValueTo (gameObject, iTween.Hash (
			"from", frontCurrentY,
			"to", frontCurrentY + frontUpY,
			"time", 1.0f,
			"easetype", iTween.EaseType.easeOutSine,
			"onupdate", "frontUpdateHandler"
		));
	
		Stage stage = Stage.GetInstance ();
		stage.Score.AddScore (10);
		stage.OnBlockGenerate ();
		stage.OnWaveMove ();
	}

	private void backUpdateHandler (float value)
	{
		float y = Mathf.Repeat (value, 1);
		Vector2 offset = new Vector2 (0, y);
		back.GetComponent<Renderer> ().sharedMaterial.SetTextureOffset ("_MainTex", offset);
		backCurrentY = y;
	}

	private void frontUpdateHandler (float value)
	{
		float y = Mathf.Repeat (value, 1);
		Vector2 offset = new Vector2 (0, y);
		front.GetComponent<Renderer> ().sharedMaterial.SetTextureOffset ("_MainTex", offset);
		frontCurrentY = y;
	}
}
