using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//Static util classes
public static class Utils{
	public static string Md5Sum(string strToEncrypt) {
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
		byte[] bytes = ue.GetBytes(strToEncrypt);

		// encrypt bytes
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] hashBytes = md5.ComputeHash(bytes);

		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";

		for (int i = 0; i < hashBytes.Length; i++) {
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
		}

		return hashString.PadLeft(32, '0');
	}

	//Returns coordinates within a "range" from a certain position.
	// TODO: Maybe not use vector2? we only need ints
	public static List<Vector2> CoordsInRange(int range, int ox, int oy) {
		List<Vector2> coords = new List<Vector2>();

		for (int xx = ox - range; xx < ox + range; xx++) {
			for (int yy = oy - range; yy < oy + range; yy++) {
				coords.Add(new Vector2(xx, yy));
			}
		}

		return coords;
	}
}
