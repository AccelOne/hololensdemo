using UnityEngine;
using System.Collections;
using UnityEngine.VR.WSA.Input;
using UnityEngine.Windows.Speech;
using HoloToolkit.Unity;
using System;

public class GameSpeech : MonoBehaviour
{
	//private GestureRecognizer gestureRecognizer;
	public TextToSpeechManager textToSpeechManager;

	public void OnTalk(GameObject go, string text, bool defVoice)
	{
		TextToSpeechManager tts = null;
		if (go != null)
		{
			tts = go.GetComponent<TextToSpeechManager>();
		}

		if (tts != null)
		{
			tts.ChangeVoice (defVoice);
			tts.SpeakText(text);
		}
	}

	public void OnWin () {
		textToSpeechManager.ChangeVoice (true);
		textToSpeechManager.SpeakText("Congratulations");
	}

	public void OnLose () {
		textToSpeechManager.ChangeVoice (false);
		textToSpeechManager.SpeakText("You Lost");
	}
}
