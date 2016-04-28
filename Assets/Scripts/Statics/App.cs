public static class App {
	public const int WelcomeSceneIndex = 0;
	public const int GallerySceneIndex = 1;
	public const int DrawingSceneIndex = 2;

	public static DrawingFile CurrentDrawingFile;

	public static void LoadScene (int sceneIndex) {
		UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
	}
}