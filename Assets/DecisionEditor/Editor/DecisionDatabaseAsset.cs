using UnityEngine;
using UnityEditor;
using System;
using System.IO;

namespace Adnc.Decision {
	public class DecisionDatabaseAsset {  
		[MenuItem("Assets/Create/DecisionDatabase")]
		public static void CreateDecisionDatabase () {
			DecisionDatabase asset = new DecisionDatabase();

			AssetDatabase.CreateAsset(asset, GetPath() + "/DecisionDatabase.asset");
			AssetDatabase.SaveAssets();
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = asset;        
		}

		public static string GetPath () {
			string path = "Assets";
			foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
			{
				path = AssetDatabase.GetAssetPath(obj);
				if (File.Exists(path))
				{
					path = Path.GetDirectoryName(path);
				}
				break;
			}

			return path;
		}
	}
}
