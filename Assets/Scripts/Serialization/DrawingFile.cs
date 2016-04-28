using fastJSON;

[System.Serializable]
public class DrawingFile {
	public int Index;
	public string Version;

	public string DateCreated;
	public string DateModified;

	public Short2 Size;
	public LayerFile[] Layers;

	public DrawingFile () {
	}

	public static string Serialize (DrawingFile file) {
		return JSON.ToNiceJSON(file, JSON.Parameters);
	}

	public static DrawingFile Deserialize (string file) {
		return JSON.ToObject(file) as DrawingFile;
	}
}