using UnityEngine;
using System.Collections;
using UnityEngine.VR.WSA.Input;
using UnityEngine.Windows.Speech;
using HoloToolkit.Unity;
using System;

public class TextToSpeechTest : MonoBehaviour
{
	//private GestureRecognizer gestureRecognizer;
	public TextToSpeechManager textToSpeechManager;

	// Use this for initialization
	void Start ()
	{
		// Set up a GestureRecognizer to detect Select gestures.
		/*gestureRecognizer = new GestureRecognizer();
		gestureRecognizer.TappedEvent += GestureRecognizer_TappedEvent;
		gestureRecognizer.StartCapturingGestures();*/

	}

	public void OnTalk()
	{
		GazeManager gm = GazeManager.Instance;
		if (gm.FocusedObject)
		{
			// Get the target object
			GameObject obj = gm.FocusedObject;

			// Try and get a TTS Manager
			TextToSpeechManager tts = null;
			if (obj != null)
			{
				tts = obj.GetComponent<TextToSpeechManager>();
			}

			// If we have a text to speech manager on the target object, say something.
			// This voice will appear to emanate from the object.
			if (tts != null)
			{
				tts.SpeakText("This voice should sound like it's coming from the object you clicked. Feel free to walk around and listen from different angles.");
			}
		}
	}

	public void SpeakTime()
	{
		// Say something using the text to speech manager on THIS test class (the "global" one).
		// This voice will appear to follow the user.
		textToSpeechManager.SpeakText("The time is " + DateTime.Now.ToString("t"));
	}

}
