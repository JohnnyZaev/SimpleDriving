using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
	public const string HighScoreKey = "HighScore";
	
	[SerializeField] private TMP_Text scoreText;
	[SerializeField] private float scoreMultiplier = 2f;

	private float _score;

	private void Update()
	{
		_score += Time.deltaTime * scoreMultiplier;

		scoreText.text = Mathf.FloorToInt(_score).ToString();
	}

	private void OnDestroy()
	{
		var currentHighScore = PlayerPrefs.GetInt(HighScoreKey, 0);

		if (_score > currentHighScore)
		{
			PlayerPrefs.SetInt(HighScoreKey, Mathf.FloorToInt(_score));
		}
	}
}
