using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DebugConsole : MonoBehaviour {
	static int maxLineCount;

	static Text uiText;
	static readonly LinkedList<string> textLines = new LinkedList<string>();
	static string textString;

	void Awake () {
		uiText = GetComponent<Text>();
		maxLineCount = (int)(800.0f / uiText.fontSize);
	}

	void Start () {
		WriteLine("Console ready - " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
	}

	void FixedUpdate () {
		textString = "";
		foreach (var line in textLines) {
			textString += line + "\n";
		}

		uiText.text = textString;
	}

	public static void Clear () {
		textLines.Clear();
	}

	public static void Refresh (object o) {
		Debug.Log(o);
		textLines.Last.Value = o.ToString();
	}

	public static void Write (object o) {
		Debug.Log(o);
		textLines.Last.Value += o.ToString();
	}

	public static void WriteLine (object o = null) {
		o = o ?? "";

		Debug.Log(o);
		textLines.AddLast(o.ToString());
		while (textLines.Count > maxLineCount) {
			textLines.RemoveFirst();
		}
	}

	// Alias for WriteLine(object o)
	public static void Log (object o = null) {
		WriteLine(o);
	}
}
