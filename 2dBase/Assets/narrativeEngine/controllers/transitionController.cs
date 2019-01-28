using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transitionController: MonoBehaviour {
	public int levelIndex;
	public string transitionPointName;

	// Use this for initialization
	gameController gamectrl;
	public transition transition;
	public Transform pivotPoint;
	GameObject localPlayer;
	int currentInteraction = -1;
	int currentOption=-1;
	bool isTransitable;
	bool isInOption;
	string text;
	// Use this for initialization
	void Start () {


	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Player") {
			isTransitable = true;
		}
	}
	void OnTriggerExit2D(Collider2D col){
		if (col.tag == "Player") {
			isTransitable = false;
		}
	}

	int getInteraction(){
		int ret = -1;
		for (int i = 0; i<transition.interactions.Length;i++) {
			bool finished = true;
			foreach (string flg in transition.interactions[i].flagsRequiredtoInteract) {
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
			gamectrl.textbox.text = transition.interactions[currentInteraction].text;
			if (transition.interactions [currentInteraction].hasoptions) {
				gamectrl.textbox.text += "\n[ ... ]";
			}
			gamectrl.textbox.transform.parent.gameObject.SetActive (true);
		} else {
			textBubble bubble = gamectrl.textbox.transform.parent.GetComponent<textBubble> ();
			if (!bubble.ended) {
				bubble.letterSpeed = 0.02f;
			} else {
				if (transition.interactions [currentInteraction].hasoptions) {
					if (!isInOption) {
						isInOption = true;
						currentOption = 0;
						gamectrl.textbox.text = transition.interactions [currentInteraction].options [currentOption].text + " [ ↓ ]";
						return;
					} else {
						PlayerPrefs.SetInt (transition.interactions [currentInteraction].options [currentOption].flagname, transition.interactions [currentInteraction].options [currentOption].flagValue ? 1 : 0);
						isInOption = false;
					}
				}
				currentOption = -1;
				currentInteraction = -1;
				gamectrl.textbox.transform.parent.gameObject.SetActive (false);
				localPlayer = null;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if (GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>().game.transitions.Length>0 && gamectrl == null) {

			gamectrl = GameObject.FindGameObjectWithTag ("GameController").GetComponent<gameController> ();
			foreach(transition transit in gamectrl.game.transitions){
				if (transit.name == this.name) {
					transition = transit;
					Debug.Log (this.name);
					if (PlayerPrefs.GetString ("TransitionPoint") == this.name) {
						
						localPlayer = GameObject.FindGameObjectWithTag ("Player");
						Transform maincam = GameObject.FindGameObjectWithTag ("MainCamera").transform;
						maincam.position = new Vector3 (pivotPoint.position.x, maincam.position.y, maincam.position.z);
						localPlayer.transform.position = pivotPoint.position;
						act ();
					}
					return;
				}
			}
		}

		if (Input.GetButtonDown ("Fire1") && isTransitable && localPlayer==null) {
			PlayerPrefs.SetString ("TransitionPoint", transitionPointName);
			UnityEngine.SceneManagement.SceneManager.LoadScene (levelIndex);
		}

		if (Input.GetButtonDown ("Fire1") && localPlayer != null) {
			act ();
		}


		if (isInOption && Input.GetButtonDown ("Down")) {
			currentOption++;
			if (currentOption >= transition.interactions [currentInteraction].options.Length) {
				currentOption = 0;
			}
			gamectrl.textbox.text = transition.interactions [currentInteraction].options [currentOption].text + " [ ↓ ]";
		}
	}
}
