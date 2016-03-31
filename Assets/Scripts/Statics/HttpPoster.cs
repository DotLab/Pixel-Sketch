using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;

public static class HttpPoster {
	const string UrlPrefix = "http://thmix.cc/";

	public static byte[] Post (string url, params object[] postObjects) {
		var postByteList = new List<byte>();

		foreach (var postObject in postObjects) {
			if (postObject is Int16) {
				postByteList.AddRange(BitConverter.GetBytes((Int16)postObject));
			} else if (postObject is UInt16) {
				postByteList.AddRange(BitConverter.GetBytes((UInt16)postObject));
			} else if (postObject is Int32) {
				postByteList.AddRange(BitConverter.GetBytes((Int32)postObject));
			} else if (postObject is UInt32) {
				postByteList.AddRange(BitConverter.GetBytes((UInt32)postObject));
			} else if (postObject is Int64) {
				postByteList.AddRange(BitConverter.GetBytes((Int64)postObject));
			} else if (postObject is UInt64) {
				postByteList.AddRange(BitConverter.GetBytes((UInt64)postObject));
			} else if (postObject is Single) {
				postByteList.AddRange(BitConverter.GetBytes((Single)postObject));
			} else if (postObject is Double) {
				postByteList.AddRange(BitConverter.GetBytes((Double)postObject));
			} else if (postObject is Char) {
				postByteList.AddRange(BitConverter.GetBytes((Char)postObject));
			} else if (postObject is String) {
				postByteList.AddRange(Encoding.UTF8.GetBytes((String)postObject));
			} else if (postObject is Byte[]) {
				postByteList.AddRange((Byte[])postObject);
			} else {
				throw new NotImplementedException("Type " + postObject.GetType() + " is not supported.");
			}
		}

		return Post(url, postByteList.ToArray());
	}

	public static byte[] Post (string url, byte[] postData) {
		var requestByteList = new List<byte>();

		requestByteList.AddRange(postData);

		var unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
		requestByteList.AddRange(BitConverter.GetBytes(unixTimestamp));

		int hash = ComputeFnvHash(requestByteList.ToArray());
		requestByteList.AddRange(BitConverter.GetBytes(hash));

		WebRequest webRequest = WebRequest.Create(UrlPrefix + url);
		webRequest.Proxy = null;
		webRequest.Method = "POST";

		DebugConsole.Log("<color=white>-post " + UrlPrefix + url + "</color>");
		using (Stream requestStream = webRequest.GetRequestStream()) {
			requestStream.Write(requestByteList.ToArray(), 0, requestByteList.Count);
		}
		DebugConsole.Log("-sent " + BitConverter.ToString(requestByteList.ToArray()));

		byte[] responseData;
		using (WebResponse webResponse = webRequest.GetResponse()) {
			using (var responceStream = webResponse.GetResponseStream()) {
				using (var bufferStream = new MemoryStream()) {
					int count;
					var responseBuffer = new byte[256];
					while (true) {
						count = responceStream.Read(responseBuffer, 0, 256);
						if (count > 0) {
							bufferStream.Write(responseBuffer, 0, count);
						} else {
							break;
						}
					}
					responseData = bufferStream.ToArray();
				}
			}
		}

		DebugConsole.Log("-rece " + BitConverter.ToString(responseData));
		//DebugConsole.Log("-str " + Encoding.UTF8.GetString(responseData));

		return responseData;
	}

	public static Int32 ComputeFnvHash (byte[] data) {
		unchecked {
			Int32 p = 34542;
			Int32 hash = 4523421;

			for (int i = 0; i < data.Length; i++)
				hash = (hash ^ data[i]) * p;

			hash += hash << 10;
			hash ^= hash >> 5;
			hash += hash << 11;
			hash ^= hash >> 9;
			hash += hash << 13;

			return hash;
		}
	}
}
