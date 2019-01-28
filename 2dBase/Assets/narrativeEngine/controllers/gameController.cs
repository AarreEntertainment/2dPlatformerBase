using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour {
	public TextAsset gameFile;
	public game game;
	public UnityEngine.UI.Text textbox;
	// Use this for initialization
	void Start () {
			game= JsonUtility.FromJson<game> (gameFile.text);
			if (PlayerPrefs.GetInt ("gameSaved")<1) {
				foreach (flag flag in game.flags) {
					PlayerPrefs.SetInt (flag.name, flag.value ? 1 : 0);
				}
			}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
