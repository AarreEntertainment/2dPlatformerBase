using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class textBubble : MonoBehaviour {
	public Text textArea;
	public float letterSpeed = 0.1f;
	public int letterIndex;
	public bool ended = false;
	string text;
	// Use this for initialization
	void OnEnable () {
		letterSpeed = 0.1f;
		ended = false;
		text = textArea.text;
		textArea.text = "";
		StartCoroutine (writeLetter());
	}

	IEnumerator writeLetter(){
		yield return new WaitForSeconds (letterSpeed);
		textArea.text += text [letterIndex];
		letterIndex++;
		if (letterIndex < text.Length) {
			StartCoroutine (writeLetter ());
		}
		else{
			ended = true;
			text = "";
			letterIndex = 0;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
