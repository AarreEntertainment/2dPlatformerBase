using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class interaction{
	public string text;
	public string[] flagsRequiredtoInteract;
	public bool hasoptions {get{ 
			if (options.Length > 0)
				return true;
			else
				return false;}}
	public option[] options;
}