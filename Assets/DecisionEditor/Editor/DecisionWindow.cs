using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Adnc.Decision {
	public class DecisionWindow : EditorWindow {
		static DecisionDatabase database;
		static List<DecisionBase> decisionTmp;
		static bool repaint;

		GUIStyle titleStyle; // Style used for title in upper left

		int paddingSize = 30;
		GUIStyle containerPadding;
		string filter;


		[MenuItem("Window/Decision Editor")]
		public static void ShowEditor () {
			//Show existing window instance. If one doesn't exist, make one.
			EditorWindow.GetWindow<DecisionWindow>("Decision Editor");
		}

		void OnEnable () {
			containerPadding = new GUIStyle();
			containerPadding.padding = new RectOffset(paddingSize, paddingSize, paddingSize, paddingSize);

			titleStyle = new GUIStyle();
			titleStyle.fontSize = 20;
		}

		DecisionBase decision;
		void OnGUI () {
			EditorGUILayout.BeginVertical(containerPadding);

			if (database == null) {
				GUILayout.Label("Decision Database", titleStyle);
				GUILayout.Label("Please select a decision database from the assets and click the edit " +
					"button in the inspector pannel (or create one if you haven't).");
				return;
			}


			/***** BEGIN Header *****/
			EditorGUILayout.BeginHorizontal();

			EditorGUI.BeginChangeCheck();

			GUILayout.Label(string.Format("Decision Database: {0}", database.title), titleStyle);
			GUI.SetNextControlName("Filter");
			if (GUI.GetNameOfFocusedControl() == "Filter") {
				filter = EditorGUILayout.TextField(filter);
			} else {
				EditorGUILayout.TextField("Filter");
			}

			if (EditorGUI.EndChangeCheck()) {
				decisionTmp = SearchDecisions(filter);
			}

			EditorGUILayout.EndHorizontal();
			/***** END Header *****/

			for (int i = 0, l = decisionTmp.Count; i < l; i++) {
				decision = decisionTmp[i];
				decision.expanded = EditorGUILayout.Foldout(decision.expanded, decision.displayName);
				if (decision.expanded) {
					BeginIndent(20f);

					decision.displayName = EditorGUILayout.TextField("Display Name", decision.displayName);
					decision.id = EditorGUILayout.TextField("ID", decision.id);

					EndIndent();
				}
			}

			EditorGUILayout.EndVertical();

//			expanded = EditorGUILayout.Foldout(expanded, "Test Foldout");

//			EditorGUI.BeginDisabledGroup(false);
//			EditorGUI.LabelField()
//			EditorGUI.EndDisabledGroup();

//			foreach ()
//			if (database.decisions != null) GUILayout.Label(database.decisions.Count.ToString(), EditorStyles.label);
			
			//		myString = EditorGUILayout.TextField ("Text Field", myString);
			//		
			//		groupEnabled = EditorGUILayout.BeginToggleGroup ("Optional Settings", groupEnabled);
			//		myBool = EditorGUILayout.Toggle ("Toggle", myBool);
			//		myFloat = EditorGUILayout.Slider ("Slider", myFloat, -3, 3);
			//		EditorGUILayout.EndToggleGroup ();
		}

		List<DecisionBase> SearchDecisions (string search) {
			string[] searchBits = search.ToLower().Split(' ');
			List<DecisionBase> matches = database.decisions.Where(d => searchBits.All(n => d.displayName.ToLower().Contains(n))).ToList();

			return matches;
		}

		void BeginIndent (float indent) {
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(indent);
			EditorGUILayout.BeginVertical();
		}

		void EndIndent () {
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
		}

		public static void SetDatabase (DecisionDatabase newDatabase) {
			database = newDatabase;
			decisionTmp = database.decisions;

//			database.decisions.Add(new DecisionDefault());
//			EditorUtility.SetDirty(database); // Must be done to mark the data for saving
		}
	}
}
