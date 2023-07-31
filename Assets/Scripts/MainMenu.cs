using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private TMP_Text highScoreText;
	[SerializeField] private TMP_Text energyText;
	[SerializeField] private Button playButton;
	[SerializeField] private AndroidNotificationHandler androidNotificationHandler;
	[SerializeField] private IOSNotificationHandler iOSNotificationHandler;
	[SerializeField] private int maxEnergy;
	[SerializeField] private int energyRechargeDuration;

	private int _energy;
	
	private const string EnergyKey = "Energy";
	private const string EnergyReadyKey = "EnergyReady";

	private void Start()
	{
		OnApplicationFocus(true);
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (!hasFocus)
			return;
		
		CancelInvoke();
		
		highScoreText.text = $"High Score: {PlayerPrefs.GetInt(ScoreSystem.HighScoreKey, 0)}";

		_energy = PlayerPrefs.GetInt(EnergyKey, maxEnergy);

		if (_energy == 0)
		{
			string energyReadyString = PlayerPrefs.GetString(EnergyReadyKey, string.Empty);
			if (energyReadyString == string.Empty)
				return;

			DateTime energyReady = DateTime.Parse(energyReadyString);
			if (DateTime.Now > energyReady)
			{
				_energy = maxEnergy;
				PlayerPrefs.SetInt(EnergyKey, maxEnergy);
			}
			else
			{
				playButton.interactable = false;
				Invoke(nameof(EnergyRecharged),(energyReady - DateTime.Now).Seconds);
			}
		}

		energyText.text = $"PLAY ({_energy})";
	}

	private void EnergyRecharged()
	{
		playButton.interactable = true;
		_energy = maxEnergy;
		PlayerPrefs.SetInt(EnergyKey, maxEnergy);
		energyText.text = $"PLAY ({_energy})";
	}

	public void Play()
	{
		if (_energy < 1)
			return;
		_energy--;
		PlayerPrefs.SetInt(EnergyKey, _energy);
		if (_energy == 0)
		{
			DateTime energyReady = DateTime.Now.AddMinutes(energyRechargeDuration);
			PlayerPrefs.SetString(EnergyReadyKey, energyReady.ToString());
#if UNITY_ANDROID
			androidNotificationHandler.ScheduleNotification(energyReady);
#elif UNITY_IOS
			iOSNotificationHandler.ScheduleNotification(energyRechargeDuration);
#endif
		}
		SceneManager.LoadScene(1);
	}

	public void Exit()
	{
	#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
	#else
		Application.Quit();
	#endif
	}
}
