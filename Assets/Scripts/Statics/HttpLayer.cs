using System;
using System.Text;

public static class HttpLayer {
	public const int Version = 5;

	static int lsid;
	static string lshash;

	#region app

	public static void NetworkTest (string message) {
		DebugConsole.Log("network test started");

		HttpPoster.Post("app_test.php", message);

		DebugConsole.Log("network test passed");
	}

	public static bool VersionCheck () {
		DebugConsole.Log("network version check started");

		var bytes = HttpPoster.Post("app_version.php", Version);
		var serverVersion = BitConverter.ToUInt32(bytes, 0);

		DebugConsole.Log("<color=cyan>servr</color> " + serverVersion);
		DebugConsole.Log("<color=cyan>local</color> " + Version);
	
		return Version >= serverVersion;
	}

	public static string GetUpdateUrl () {
		DebugConsole.Log("network update url requested");

		var bytes = HttpPoster.Post("app_update.php", Version);
		var url = Encoding.ASCII.GetString(bytes);

		DebugConsole.Log("<color=cyan>url</color> " + url);

		return url;
	}

	#endregion

	#region user

	public static bool Login (string mail, string pass) {
		DebugConsole.Log("network login with " + mail);

//		if (!IsValidMail(mail)) {
//			DebugConsole.Log("<color=red>login fail</color> invalid mail");
//			return false;
//		} else if (!IsValidPass(pass)) {
//			DebugConsole.Log("<color=red>login fail</color> invalid hash");
//			return false;
//		}

		var bytes = HttpPoster.Post("user_login.php", Version, mail + "|" + pass);
		if ((bytes.Length != 4) && (bytes.Length != 20)) {
			DebugConsole.Log("<color=red>login fail</color> invalid bytes");
			return false;
		}

		if (bytes.Length == 4) {
			var errno = BitConverter.ToInt32(bytes, 0);
			switch (errno) {
			case 0:
				DebugConsole.Log("<color=red>login fail</color> invalid comb");
				return false;
			default:
				DebugConsole.Log("<color=red>login fail</color> invalid errno");
				return false;
			}
		}

		lsid = BitConverter.ToInt32(bytes, 0);
		lshash = Encoding.UTF8.GetString(bytes, 4, bytes.Length - 4);

		DebugConsole.Log("<color=cyan>lsid</color> " + lsid);
		DebugConsole.Log("<color=cyan>lshash</color> " + lshash);

		return true;
	}

	public static bool Register (string mail, string pass) {
		DebugConsole.Log("network register with " + mail);
		//		if (!IsValidName(name)) {
		//			DebugConsole.Log("<color=red>register fail</color> invalid name");
		//			return false;
		//		} else if (!IsValidMail(mail)) {
		//			DebugConsole.Log("<color=red>register fail</color> invalid mail");
		//			return false;
		//		} else if (!IsValidPass(pass)) {
		//			DebugConsole.Log("<color=red>register fail</color> invalid hash");
		//			return false;
		//		}

		var bytes = HttpPoster.Post("user_register.php", Version, mail + "|" + pass);
		if ((bytes.Length != 4) && (bytes.Length != 20)) {
			DebugConsole.Log("<color=red>register fail</color> invalid bytes");
			return false;
		}

		if (bytes.Length == 4) {
			var errno = BitConverter.ToInt32(bytes, 0);
			switch (errno) {
			case 0:
				DebugConsole.Log("<color=red>register fail</color> existing name");
				return false;
			case 1:
				DebugConsole.Log("<color=red>register fail</color> existing mail");
				return false;
			default:
				DebugConsole.Log("<color=red>register fail</color> invalid errno");
				return false;
			}
		}
			
		lsid = BitConverter.ToInt32(bytes, 0);
		lshash = Encoding.UTF8.GetString(bytes, 4, bytes.Length - 4);

		DebugConsole.Log("<color=cyan>lsid</color> " + lsid);
		DebugConsole.Log("<color=cyan>lshash</color> " + lshash);

		return true;
	}

	public static bool IsLoggedIn () {
		return lsid != 0 && !string.IsNullOrEmpty(lshash);
	}

	public static bool SetName (string name) {
		DebugConsole.Log("network setname ");

		if (!IsLoggedIn()) {
			DebugConsole.Log("<color=red>setname fail</color> not logged in");
			return false;
		}

		var bytes = HttpPoster.Post("user_set_name.php", lsid, lshash + '|' + name);
		if ((bytes.Length != 4)) {
			DebugConsole.Log("<color=red>setname fail</color> invalid bytes");
			return false;
		}

		var no = BitConverter.ToInt32(bytes, 0);
		switch (no) {
		case 0:
			DebugConsole.Log("<color=red>getinfo fail</color> invalid session");
			return false;
		case 1:
			DebugConsole.Log("<color=red>getinfo fail</color> invalid userid");
			return false;
		case 2:
			return true;
		default:
			DebugConsole.Log("<color=red>getinfo fail</color> invalid errno");
			return false;
		}
	}

	#endregion

	#region ultility

	static byte[] GetBytes (string str) {
		var bytes = new byte[str.Length * sizeof(char)];
		Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
		return bytes;
	}

	static string GetString (byte[] bytes) {
		var chars = new char[bytes.Length / sizeof(char)];
		Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
		return new string(chars);
	}

	#endregion
}