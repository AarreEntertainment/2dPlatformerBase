using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactableController : MonoBehaviour {
	gameController gamectrl;
	public interactable interactable;
	GameObject localPlayer;
	int currentInteraction = -1;
	int currentOption=-1;
	bool isInOption;
	string text;
	// Use this for initialization
	void Start () {
		

	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Player") {
			localPlayer = col.gameObject;
		}
	}
	void OnTriggerExit2D(Collider2D col){
		if (col.tag == "Player") {
			localPlayer = null;
			isInOption = false;
			gamectrl.textbox.transform.parent.GetComponent<textBubble> ().letterIndex = 0;
			currentOption = -1;
			currentInteraction = -1;
			gamectrl.textbox.transform.parent.gameObject.SetActive (false);
		}
	}

	int getInteraction(){
		int ret = -1;
		for (int i = 0; i<interactable.interactions.Length;i++) {
			bool finished = true;
			foreach (string flg in interactable.interactions[i].flagsRequiredtoInteract) {
				finished = (PlayerPrefs.GetInt (flg) > 0);
			}
			if (finished) {
				ret = i;
			}
		}
		return ret;
	}

	void act(){
		if (currentInteraction < 0) {
			currentInteraction = getInteraction ();
			if (currentInteraction < 0) {
				return;
			}
		}

		if (!gamectrl.textbox.transform.parent.gameObject.activeSelf) {
			gamectrl.textbox.text = interactable.interactions[currentInteraction].text;
			if (interactable.interactions [currentInteraction].hasoptions) {
				gamectrl.textbox.text += "\n[ ... ]";
			}
			gamectrl.textbox.transform.parent.gameObject.SetActive (true);
		} else {
			textBubble bubble = gamectrl.textbox.transform.parent.GetComponent<textBubble> ();
			if (!bubble.ended) {
				bubble.letterSpeed = 0.02f;
			} else {
				if (interactable.interactions [currentInteraction].hasoptions) {
					if (!isInOption) {
						isInOption = true;
						currentOption = 0;
						gamectrl.textbox.text = interactable.interactions [currentInteraction].options [currentOption].text + " [ ↓ ]";
						return;
					} else {
						PlayerPrefs.SetInt (interactable.interactions [currentInteraction].options [currentOption].flagname, interactable.interactions [currentInteraction].options [currentOption].flagValue ? 1 : 0);
						isInOption = false;
					}
				}
				currentOption = -1;
				currentInteraction = -1;
				gamectrl.textbox.transform.parent.gameObject.SetActive (false);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>().game.interactables.Length>0 && gamectrl == null) {
			
			gamectrl = GameObject.FindGameObjectWithTag ("GameController").GetComponent<gameController> ();
			foreach(interactable inter in gamectrl.game.interactables){
					if (inter.name == this.name) {
						interactable = inter;
						return;
					}
				}
		}

		if (Input.GetButtonDown ("Fire1") && localPlayer != null) {
			act ();
		}
		if (isInOption && Input.GetButtonDown ("Down")) {
			currentOption++;
			if (currentOption >= interactable.interactions [currentInteraction].options.Length) {
				currentOption = 0;
			}
				gamectrl.textbox.text = interactable.interactions [currentInteraction].options [currentOption].text + " [ ↓ ]";
		}
	}
}
