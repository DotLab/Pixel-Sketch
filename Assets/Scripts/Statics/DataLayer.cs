using UnityEngine;

public static class DataLayer {
	const string UnsavedFileStringKey = "UnsavedFile";

	const string DrawingFileCountIntKey = "DrawingFileCount";
	const string DrawingFileStringKeyPrefix = "Drawing-";

	public static bool HasUnsavedFile () {
		return PlayerPrefs.HasKey(UnsavedFileStringKey);
	}

	public static void SaveUnsavedFile (DrawingFile file) {
		DebugConsole.Log("Save Unsaved " + file.Index);

		PlayerPrefs.SetString(UnsavedFileStringKey, DrawingFile.Serialize(file));
//		PlayerPrefs.Flush();
	}

	public static DrawingFile LoadUnsavedFile () {
		DebugConsole.Log("Load Unsaved File");

		if (!HasUnsavedFile()) return null;

		return DrawingFile.Deserialize(PlayerPrefs.GetString(UnsavedFileStringKey));
	}

	public static void DiscardUnsavedFile () {
		DebugConsole.Log("Discard Unsaved File");

		if (HasUnsavedFile()) {
			PlayerPrefs.DeleteKey(UnsavedFileStringKey);
//			PlayerPrefs.Flush();
		}
	}

	public static int GetFileCount () {
		return PlayerPrefs.GetInt(DrawingFileCountIntKey, 0);
	}

	public static bool HasFile (int index) {
		return PlayerPrefs.HasKey(DrawingFileStringKeyPrefix + index);
	}

	public static void SaveFile (DrawingFile file) {
		DebugConsole.Log("Save File " + file.Index);
		PlayerPrefs.SetString(DrawingFileStringKeyPrefix + file.Index, DrawingFile.Serialize(file));

		if (file.Index >= GetFileCount())
			PlayerPrefs.SetInt(DrawingFileCountIntKey, file.Index + 1);
		
//		PlayerPrefs.Flush();
	}

	public static DrawingFile LoadFile (int index) {
		DebugConsole.Log("Load File " + index);
		if (!HasFile(index)) return null;

		return DrawingFile.Deserialize(PlayerPrefs.GetString(DrawingFileStringKeyPrefix + index));
	}

	public static void DeleteFile (int index) {
		DebugConsole.Log("Delete File " + index);

		if (HasFile(index)) {
			PlayerPrefs.DeleteKey(DrawingFileStringKeyPrefix + index);
//			PlayerPrefs.Flush();
		}
	}

	public static DrawingFile DuplicateFile (int index) {
		if (!HasFile(index)) return null;

		var copy = LoadFile(index);
		copy.Index = GetFileCount();
		SaveFile(copy);

		return copy;
	}
}