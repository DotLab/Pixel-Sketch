using UnityEngine;

public class ColorTabController : MonoBehaviour {
	public EasedContentSizeFitter ColorTabSizeFitter;

	public void OnColorPickerClicked () {
		ColorTabSizeFitter.Contents[1].Active = !ColorTabSizeFitter.Contents[1].Active;
		ColorTabSizeFitter.Fit();
	}

	public void OnColorSwatchClicked () {
		ColorTabSizeFitter.Contents[2].Active = !ColorTabSizeFitter.Contents[2].Active;
		ColorTabSizeFitter.Fit();
	}
}
