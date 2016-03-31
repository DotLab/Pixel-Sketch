using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WelcomeSceneScheduler : MonoBehaviour {
	public const int MenuSceneIndex = 1;

	public float Delay = 2;
	public GameObject Overlay;
	IHidable overlayHidable;


	void Awake () {
		overlayHidable = Overlay.GetComponent<IHidable>();
	}

	void Start () {
		StartCoroutine(WelcomeSceneHandler());
	}

	IEnumerator WelcomeSceneHandler () {
		yield return new WaitForSeconds(0.5f);

		DebugConsole.Log(Application.version);
		DebugConsole.Log(Screen.currentResolution);
		DebugConsole.Log(SystemInfo.operatingSystem);
		DebugConsole.Log();
		DebugConsole.Log(SystemInfo.systemMemorySize + "MB");
		DebugConsole.Log(SystemInfo.deviceModel);
		DebugConsole.Log();
		DebugConsole.Log(SystemInfo.processorCount + " x " + SystemInfo.processorFrequency + "MHz on " + SystemInfo.processorType);
		DebugConsole.Log();
		DebugConsole.Log(SystemInfo.graphicsMemorySize + "MB");
		DebugConsole.Log(SystemInfo.graphicsDeviceName + " (" + SystemInfo.graphicsDeviceVendor + ")");
		DebugConsole.Log(SystemInfo.graphicsDeviceType + " on " + SystemInfo.graphicsDeviceVersion);
		DebugConsole.Log();
		DebugConsole.Log(SystemInfo.npotSupport + " NPOT Support");
		DebugConsole.Log(SystemInfo.supportsImageEffects + " Image Effect Support");

		yield return new WaitForSeconds(Delay);

		overlayHidable.Show();
		yield return new WaitForSeconds(0.5f);

		SceneManager.LoadScene(MenuSceneIndex);
	}
}
