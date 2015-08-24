using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour
{
	[SerializeField]
	private GUIText scoreText;

	private int currentScore = 0;

	public void Init ()
	{
		currentScore = 0;
	}

	public int GetCurrentScore ()
	{
		return currentScore;
	}

	public void AddScore (int plus)
	{
		iTween.ValueTo (gameObject, iTween.Hash (
			"from", currentScore,
			"to", currentScore + plus,
			"time", 1.0f,
			"easetype", iTween.EaseType.easeOutSine,
			"onupdate", "scoreUpdateHandler"
		));
		currentScore += plus;
	}

	private void scoreUpdateHandler (int value)
	{
		scoreText.text = value.ToString () + "M";
	}
}
