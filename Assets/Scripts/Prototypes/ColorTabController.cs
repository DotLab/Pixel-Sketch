using UnityEngine;

public class ColorTabController : MonoBehaviour {
	public ColorTabContentFitter ColorTabContentFitter;

	public void OnColorPickerClicked () {
		ColorTabContentFitter.Contents[1].Active = !ColorTabContentFitter.Contents[1].Active;
		ColorTabContentFitter.Fit();
	}

	public void OnColorSwatchClicked () {
		ColorTabContentFitter.Contents[2].Active = !ColorTabContentFitter.Contents[2].Active;
		ColorTabContentFitter.Fit();
	}
}
