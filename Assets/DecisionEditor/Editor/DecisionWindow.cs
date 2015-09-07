using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Adnc.Decision {
	public class DecisionWindow : EditorWindow {
		static DecisionDatabase database; // Current database we are editing
		static List<DecisionBase> decisionTmp; // Temporary dump of decisions for filter purspoes

		GUIStyle titleStyle; // Style used for title in upper left
		GUIStyle errorStyle; // Used for error styling
		GUIStyle errorFoldoutStyle;

		int paddingSize = 15; // Total padding wrapping the window
		GUIStyle containerPadding;

		string filter; // Current search target
		Vector2 scrollPos; // Scroll window details
		int deleteIndex = -1;

		[MenuItem("Window/Decision Editor")]
		public static void ShowEditor () {
			// Show existing window instance. If one doesn't exist, make one.
			EditorWindow.GetWindow<DecisionWindow>("Decision Editor");
		}

		void OnEnable () {
			containerPadding = new GUIStyle();
			containerPadding.padding = new RectOffset(paddingSize, paddingSize, paddingSize, paddingSize);

			titleStyle = new GUIStyle();
			titleStyle.fontSize = 20;

			errorStyle = new GUIStyle();
			errorStyle.normal.textColor = Color.red;
		}

		DecisionBase decision;
		bool errorId;
		void OnGUI () {
			// We have to get the foldout error style in OnGUI or it will error on us
			if (errorFoldoutStyle == null) errorFoldoutStyle = GetFoldoutErrorStyle();

			EditorGUILayout.BeginVertical(containerPadding); // BEGIN Padding

			/***** BEGIN Header *****/
			if (database == null) {
				GUILayout.Label("Decision Database", titleStyle);
				GUILayout.Label("Please select a decision database from the assets and click the edit " +
					"button in the inspector pannel (or create one if you haven't).");
				return;
			}

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
				FilterDecisions(filter);
			}

			EditorGUILayout.EndHorizontal();

			if (GUILayout.Button("Add Decision")) AddDecision(true);
			/***** END Header *****/

			/***** BEGIN Body *****/
			EditorGUILayout.EndVertical(); // END Padding

			scrollPos = GUILayout.BeginScrollView(scrollPos);
			EditorGUILayout.BeginVertical(containerPadding); // BEGIN Padding

			EditorGUI.BeginChangeCheck();
			for (int i = 0, l = decisionTmp.Count; i < l; i++) {
				decision = decisionTmp[i];
			
				errorId = string.IsNullOrEmpty(decision.id);

				if (!errorId) {
					decision.expanded = EditorGUILayout.Foldout(decision.expanded, decision.displayName);
				} else {
					decision.expanded = EditorGUILayout.Foldout(decision.expanded, 
					                                            string.Format("{0}: ID cannot be left blank and must be unique", decision.displayName), 
					                                            errorFoldoutStyle);
				}

				if (decision.expanded) {
					BeginIndent(20f);
					
					decision.displayName = EditorGUILayout.TextField("Display Name", decision.displayName);
					decision.id = EditorGUILayout.TextField("ID", decision.id);
					decision.defaultValue = EditorGUILayout.Toggle("Default Value", decision.defaultValue);

					EditorGUILayout.LabelField("Notes");
					decision.notes = GUILayout.TextArea(decision.notes, GUILayout.MaxHeight(60f), GUILayout.Width(300f));

					if (GUILayout.Button(string.Format("Remove '{0}'", decision.displayName))) {
						if (ConfirmDelete(decision.displayName)) {
							deleteIndex = i;
						}
					}

					EndIndent();
				}
			}

			if (EditorGUI.EndChangeCheck()) {
				EditorUtility.SetDirty(database);
			}

			EditorGUILayout.EndVertical(); // END Padding
			GUILayout.EndScrollView();
			/***** END Body *****/			

			if (deleteIndex != -1) {
				RemoveDecision(deleteIndex);
				deleteIndex = -1;
			}
		}

		bool ConfirmDelete (string itemName) {
			return EditorUtility.DisplayDialog("Delete Item", 
			                                   string.Format("Are you sure you want to delete '{0}'", itemName), 
			                                   string.Format("Delete '{0}'", itemName),
			                                   "Cancel"
			);
		}
		
		void FilterDecisions (string search) {
			if (string.IsNullOrEmpty(search)) {
				decisionTmp = database.decisions;
				return;
			}

			string[] searchBits = search.ToLower().Split(' ');
			List<DecisionBase> matches = database.decisions.Where(d => searchBits.All(n => d.displayName.ToLower().Contains(n))).ToList();
		
			decisionTmp = matches;
		}

		void AddDecision (bool placeAtTop) {
			database.decisions.Insert(0, new DecisionDefault());

			FilterDecisions(filter);
			EditorUtility.SetDirty(database);
		}

		void RemoveDecision (int index) {
			database.decisions.RemoveAt(index);
			FilterDecisions(filter);
			EditorUtility.SetDirty(database);
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

		static GUIStyle GetFoldoutErrorStyle () {
			GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout);
			Color myStyleColor = Color.red;
			myFoldoutStyle.normal.textColor = myStyleColor;
			myFoldoutStyle.onNormal.textColor = myStyleColor;
			myFoldoutStyle.hover.textColor = myStyleColor;
			myFoldoutStyle.onHover.textColor = myStyleColor;
			myFoldoutStyle.focused.textColor = myStyleColor;
			myFoldoutStyle.onFocused.textColor = myStyleColor;
			myFoldoutStyle.active.textColor = myStyleColor;
			myFoldoutStyle.onActive.textColor = myStyleColor;

			return myFoldoutStyle;
		}

		public static void SetDatabase (DecisionDatabase newDatabase) {
			database = newDatabase;
			decisionTmp = database.decisions;
		}
	}
}
