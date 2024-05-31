using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.SceneManagement;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bayat.Games.Scenes.Utilities
{

    /// <summary>
    /// Scene manager window, an editor window for managing scenes.
    /// </summary>
    public class SceneManagerWindow : EditorWindow
    {

        public enum ScenesSource
        {
            BuildSettings,
            Project,
            Manual
        }

        protected Vector2 scrollPosition;
        protected Vector2 scenesTabScrollPosition;
        protected ScenesSource scenesSource = ScenesSource.BuildSettings;
        protected NewSceneSetup newSceneSetup = NewSceneSetup.DefaultGameObjects;
        protected NewSceneMode newSceneMode = NewSceneMode.Single;
        protected OpenSceneMode openSceneMode = OpenSceneMode.Single;
        protected bool showPath = false;
        protected bool showAddToBuild = true;
        protected bool askBeforeDelete = true;
        protected string[] guids;
        protected int selectedTab = 0;
        protected string[] tabs = new string[] {
            "Scenes",
            "Settings"
        };
        protected string lastScene;
        protected string searchFolder = "Assets";

        [SerializeField]
        protected TreeViewState scenesTreeViewState;
        [SerializeField]
        protected MultiColumnHeaderState multiColumnHeaderState;
        protected ScenesTreeView scenesTreeView;
        protected MultiColumnHeader multiColumnHeader;
        protected List<SceneTreeItem> sceneItems = new();

        public bool ShowPath => this.showPath;
        public bool ShowAddToBuild => this.showAddToBuild;

        [MenuItem("Window/Bayat/Games/Scenes/Scene Manager Utility")]
        public static void Init()
        {
            var window = EditorWindow.GetWindow<SceneManagerWindow>("Scene Manager");
            window.minSize = new Vector2(400f, 200f);
            window.Show();
        }

        protected virtual void PlayModeStateChanged(PlayModeStateChange state)
        {
            Debug.Log(state);
            Debug.Log(this.lastScene);
            switch (state)
            {
                case PlayModeStateChange.EnteredEditMode:
                    if (!string.IsNullOrEmpty(this.lastScene))
                    {
                        Open(this.lastScene);
                        this.lastScene = null;
                    }
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    break;
            }
        }

        protected virtual void OnEnable()
        {
            SetupTreeView();
            EditorApplication.playModeStateChanged += PlayModeStateChanged;
            this.scenesSource = (ScenesSource)EditorPrefs.GetInt(
                "SceneManager.scenesSource",
                (int)ScenesSource.BuildSettings);
            this.searchFolder = EditorPrefs.GetString("SceneManager.searchFolder", "Assets");
            this.newSceneSetup = (NewSceneSetup)EditorPrefs.GetInt(
                "SceneManager.newSceneSetup",
                (int)NewSceneSetup.DefaultGameObjects);
            this.newSceneMode = (NewSceneMode)EditorPrefs.GetInt("SceneManager.newSceneMode", (int)NewSceneMode.Single);
            this.openSceneMode = (OpenSceneMode)EditorPrefs.GetInt(
                "SceneManager.openSceneMode",
                (int)OpenSceneMode.Single);
            this.showPath = EditorPrefs.GetBool("SceneManager.showPath", false);
            this.showAddToBuild = EditorPrefs.GetBool("SceneManager.showAddToBuild", true);
            this.askBeforeDelete = EditorPrefs.GetBool("SceneManager.askBeforeDelete", true);

            UpdateScenes();
        }

        protected virtual void OnProjectChange()
        {
            UpdateScenes();
            Repaint();
        }

        protected virtual void OnFocus()
        {
            UpdateScenes();
        }

        protected virtual void OnDisable()
        {
            EditorApplication.playModeStateChanged -= PlayModeStateChanged;
            EditorPrefs.SetInt("SceneManager.scenesSource", (int)this.scenesSource);
            EditorPrefs.SetString("SceneManager.searchFolder", this.searchFolder);
            EditorPrefs.SetInt("SceneManager.newSceneSetup", (int)this.newSceneSetup);
            EditorPrefs.SetInt("SceneManager.newSceneMode", (int)this.newSceneMode);
            EditorPrefs.SetInt("SceneManager.openSceneMode", (int)this.openSceneMode);
            EditorPrefs.SetBool("SceneManager.showPath", this.showPath);
            EditorPrefs.SetBool("SceneManager.showAddToBuild", this.showAddToBuild);
            EditorPrefs.SetBool("SceneManager.askBeforeDelete", this.askBeforeDelete);
        }

        protected virtual void OnGUI()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            this.selectedTab = GUILayout.Toolbar(this.selectedTab, this.tabs, EditorStyles.toolbarButton);
            EditorGUILayout.EndHorizontal();
            this.scrollPosition = EditorGUILayout.BeginScrollView(this.scrollPosition);
            EditorGUILayout.BeginVertical();
            switch (this.selectedTab)
            {
                case 0:
                    ScenesTabGUI();
                    break;
                case 1:
                    SettingsTabGUI();
                    break;
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            GUILayout.Label("Made with ❤️ by Bayat Games", EditorStyles.centeredGreyMiniLabel);
        }

        protected virtual void SettingsTabGUI()
        {
            this.scenesSource = (ScenesSource)EditorGUILayout.EnumPopup("Scenes Source", this.scenesSource);
            if (this.scenesSource == ScenesSource.Manual)
            {
                this.searchFolder = EditorGUILayout.TextField("Search Folder", this.searchFolder);
            }

            this.newSceneSetup = (NewSceneSetup)EditorGUILayout.EnumPopup("New Scene Setup", this.newSceneSetup);
            this.newSceneMode = (NewSceneMode)EditorGUILayout.EnumPopup("New Scene Mode", this.newSceneMode);
            this.openSceneMode = (OpenSceneMode)EditorGUILayout.EnumPopup("Open Scene Mode", this.openSceneMode);
            this.showPath = EditorGUILayout.Toggle("Show Path", this.showPath);
            this.showAddToBuild = EditorGUILayout.Toggle("Show Add To Build", this.showAddToBuild);
            this.askBeforeDelete = EditorGUILayout.Toggle("Ask Before Delete", this.askBeforeDelete);
        }

        protected virtual void SetupTreeView()
        {
            if (this.scenesTreeViewState == null)
            {
                this.scenesTreeViewState = new();
            }

            if (this.multiColumnHeaderState == null)
            {
                this.multiColumnHeaderState = new(new MultiColumnHeaderState.Column[]
                {
                    new MultiColumnHeaderState.Column()  {
                        headerContent = new("Name"),
                        autoResize = true,
                        headerTextAlignment = TextAlignment.Left,
                        width = 100f,
                        canSort = false // No sorting functionality
                    },
                    new MultiColumnHeaderState.Column()  {
                        headerContent = new("Path"),
                        autoResize = true,
                        headerTextAlignment = TextAlignment.Left,
                        width = 200f,
                        canSort = false // No sorting functionality
                    },
                });
            }

            if (this.multiColumnHeader == null)
            {
                this.multiColumnHeader = new(this.multiColumnHeaderState);
            }

            if (this.scenesTreeView == null)
            {
                this.scenesTreeView = new(this, this.sceneItems, this.multiColumnHeader, this.scenesTreeViewState);
            }
        }

        protected virtual void UpdateScenes()
        {
            SetupTreeView();
            this.sceneItems.Clear();
            List<EditorBuildSettingsScene> buildScenes = new(EditorBuildSettings.scenes);
            this.guids = AssetDatabase.FindAssets("t:Scene");
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
                EditorBuildSettingsScene buildScene = buildScenes.Find((editorBuildScene) =>
                {
                    return editorBuildScene.path == path;
                });

                switch (this.scenesSource)
                {
                    case ScenesSource.BuildSettings:
                        if (buildScene == null)
                        {
                            continue;
                        }
                        break;
                    case ScenesSource.Manual:
                        if (!path.Contains(this.searchFolder))
                        {
                            continue;
                        }
                        break;
                }

                SceneTreeItem sceneItem = new(path, sceneAsset, buildScene);
                this.sceneItems.Add(sceneItem);
            }

            // Reload tree view
            this.scenesTreeView.Reload();
        }

        protected virtual void ScenesTabGUI()
        {
            SetupTreeView();

            this.scenesTabScrollPosition = EditorGUILayout.BeginScrollView(this.scenesTabScrollPosition);
            if (this.sceneItems.Count == 0)
            {
                GUILayout.Label("No Scenes Found", EditorStyles.centeredGreyMiniLabel);
                GUILayout.Label("Create New Scenes", EditorStyles.centeredGreyMiniLabel);
                GUILayout.Label("And Manage them here", EditorStyles.centeredGreyMiniLabel);
            }

            Rect rect = EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true), GUILayout.MinHeight(400f));
            EditorGUILayout.EndVertical();
            this.scenesTreeView.OnGUI(rect);
            EditorGUILayout.EndScrollView();
            if (GUILayout.Button("Create New Scene"))
            {
                Scene newScene = EditorSceneManager.NewScene(this.newSceneSetup, this.newSceneMode);
                EditorSceneManager.SaveScene(newScene);
            }
            GUILayout.Label("Bulk Actions", EditorStyles.boldLabel);
            bool anySelected = false;
            IList<int> selectedIndices = this.scenesTreeView.GetSelection();
            List<SceneTreeItem> selectedItems = new();
            if (this.sceneItems.Count > 0)
            {
                for (int i = 0; i < selectedIndices.Count; i++)
                {
                    int index = selectedIndices[i];
                    if (index >= this.sceneItems.Count)
                    {
                        continue;
                    }

                    selectedItems.Add(this.sceneItems[index]);
                    anySelected = true;
                }
            }

            EditorGUI.BeginDisabledGroup(!anySelected);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Delete"))
            {
                for (int i = 0; i < selectedItems.Count; i++)
                {
                    Delete(selectedItems[i].ScenePath);
                }
            }

            if (GUILayout.Button("Open Additive"))
            {
                for (int i = 0; i < selectedItems.Count; i++)
                {
                    Open(selectedItems[i].ScenePath, OpenSceneMode.Additive);
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
            GUILayout.Label("General Actions", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Save Modified Scenes"))
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            }

            if (GUILayout.Button("Save Open Scenes"))
            {
                EditorSceneManager.SaveOpenScenes();
            }

            EditorGUILayout.EndHorizontal();
        }

        public virtual void Open(string path, OpenSceneMode? openMode = null)
        {
            if (openMode == null)
            {
                openMode = this.openSceneMode;
            }

            if (EditorSceneManager.EnsureUntitledSceneHasBeenSaved("You don't have saved the Untitled Scene, Do you want to leave?"))
            {
                this.lastScene = EditorSceneManager.GetActiveScene().path;
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(path, openMode.Value);
            }
        }

        public virtual void Delete(string path)
        {
            if (!askBeforeDelete || EditorUtility.DisplayDialog(
                     "Delete Scene",
                     string.Format(
                         "Are you sure you want to delete the below scene: {0}",
                         path),
                     "Delete",
                     "Cancel"))
            {
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.Refresh();
            }
        }

        public virtual void AddToBuild(string path)
        {
            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            scenes.Add(new EditorBuildSettingsScene(path, true));
            EditorBuildSettings.scenes = scenes.ToArray();
        }

        public virtual void RemoveFromBuild(string path)
        {
            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            scenes.RemoveAll(scene =>
            {
                return scene.path == path;
            });
            EditorBuildSettings.scenes = scenes.ToArray();
        }

    }

}