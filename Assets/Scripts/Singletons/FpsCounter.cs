using UnityEngine;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour {
	Text uiText;

	void Awake () {
		uiText = GetComponent<Text>();
	}

	void Update () {
		var fps = 1 / Time.smoothDeltaTime;
		uiText.text = fps.ToString("F1");
	}
}
