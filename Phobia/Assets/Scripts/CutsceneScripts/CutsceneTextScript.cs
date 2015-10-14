﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * 
 * Class which handles cutscene functionality.
 * 
 **/
public class CutsceneTextScript : MonoBehaviour {

	public Text textBoxString;
	public Text nameBoxString;
	public TextAsset TextFile;
	public Image portraitLImage;
	public Image portraitRImage;
	public Sprite clientHappy;
	public Sprite clientMasked;
	public Sprite clientExtra;
	public string sceneToTransitionTo;
	public float textTypingDelay;
	
	private string[] linesInFile;
	private string[] scriptLine;
	private string lineText;
	private int lineNumber;

	void Start() {
		linesInFile = TextFile.text.Split('\n'); 			// Get lines in file.
		lineNumber = 0;										// Initialize line number to 0.

		scriptLine = linesInFile[lineNumber].Split(':');	// Split line in text file into Speaker and dialogue.

		lineText = scriptLine[1];							// Get dialogue text.

		StartCoroutine("ProcessScriptLine");				// Type out first line to text box. 

		lineNumber++;										// Increment line number.
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Escape)) {

			//Skip cutscene to game loading scene.
			Application.LoadLevelAsync(sceneToTransitionTo);

		} else if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("space")) {

			if (lineNumber < linesInFile.Length) {
				// Stop current Coroutine for typing.
				StopCoroutine("ProcessScriptLine");

				// Get next line in text file.
				scriptLine = linesInFile[lineNumber].Split(':');

				// Start typing new line.
				StartCoroutine("ProcessScriptLine");

				// Increment line number.
				lineNumber++;
			} else {
				//Transition to game loading scene.
				Application.LoadLevelAsync(sceneToTransitionTo);
			}
		}
	}

	/**
	 *
	 * Coroutine Method for typing out string from 
	 * file into textbox of cutscene.
	 *
	 **/
	IEnumerator ProcessScriptLine() {

		// Clear text box.
		textBoxString.text = "";

		// Set name of speaker.
		nameBoxString.text = scriptLine [0];

		//Manipulate Images if needed.
		if (scriptLine.Length > 2) {
			ImageProcessing();
		}

		// Determine what image to highlight.
		if (scriptLine [0] == "Dr. Ndoto") {
			portraitLImage.color = Color.white;
			portraitRImage.color = Color.gray;

		} else if (scriptLine [0] == "Client") {
			portraitLImage.color = Color.gray;
			portraitRImage.color = Color.white;

		} else {
			portraitLImage.color = Color.gray;
			portraitRImage.color = Color.gray;
		}

		// Get dialogue string to write to text box. 
		lineText = scriptLine [1];

		int charCount = 0;

		// Print out string character by character.
		foreach (char c in lineText.ToCharArray()) {
			textBoxString.text += c;

			if (charCount < 0) {
				// Play a sound effect for each character.
				CutsceneSoundScript.PlayTextSound ();
				charCount = 8;
			}

			charCount--;

			yield return new WaitForSeconds (textTypingDelay);
		}
	}

	/**
	 *
	 * Method for enabling and disabling images in cutscenes.
	 *
	 **/
	public void ImageProcessing() {

		if (scriptLine [2] == "DisableL") {
			//Disable left image only.
			portraitLImage.enabled = false;
			
		} else if (scriptLine [2] == "DisableL") {
			//Disable right image only.
			portraitRImage.enabled = false;

		} else if (scriptLine [2] == "DisableB") {
			//Disable both images.
			portraitLImage.enabled = false;
			portraitRImage.enabled = false;
			
		} else if (scriptLine [2] == "EnableL") {
			//Enable left image only.
			portraitLImage.enabled = true;

		} else if (scriptLine [2] == "EnableR") {
			//Enable right image only.
			portraitRImage.enabled = true;

		} else if (scriptLine [2] == "EnableB") {
			//Enable both images.
			portraitLImage.enabled = true;
			portraitRImage.enabled = true;
			
		} else if (scriptLine [2] == "ChangeClientHappy") {
			//Change to happy client image.
			portraitRImage.sprite = clientHappy;

		} else if (scriptLine [2] == "ChangeClientMasked") {
			//Change to masked client image.
			portraitRImage.sprite = clientMasked;

		} else if (scriptLine [2] == "ChangeClientExtra") {
			//Change to extra client image.
			portraitRImage.sprite = clientExtra;
		}
	}
}