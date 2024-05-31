using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.SceneManagement;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bayat.Games.Scenes.Utilities
{

    public class ScenesTreeView : TreeView
    {

        public enum SceneColumnType
        {
            Name,
            Path,
            Actions
        }

        public enum SceneActionType
        {
            Path,
            AddToBuild,
            Play,
            OpenClose,
            Delete
        }

        protected SceneManagerWindow sceneManagerWindow;
        protected List<SceneTreeItem> scenes;

        public ScenesTreeView(SceneManagerWindow sceneManagerWindow, List<SceneTreeItem> scenes, MultiColumnHeader multiColumnHeader, TreeViewState treeViewState) : base(treeViewState, multiColumnHeader)
        {
            this.sceneManagerWindow = sceneManagerWindow;
            this.scenes = scenes;

            rowHeight = 20;
            showAlternatingRowBackgrounds = true;
            showBorder = true;

            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            // This section illustrates that IDs should be unique. The root item is required to 
            // have a depth of -1, and the rest of the items increment from that.
            TreeViewItem root = new TreeViewItem { id = 0, depth = -1, displayName = "Scenes" };
            List<TreeViewItem> allItems = new();

            for (int i = 0; i < this.scenes.Count; i++)
            {
                TreeViewItem treeItem = new(i + 1, 0, this.scenes[i].Scene.name);
                allItems.Add(treeItem);
            }

            // Utility method that initializes the TreeViewItem.children and .parent for all items.
            SetupParentsAndChildrenFromDepths(root, allItems);

            // Return root of the tree
            return root;
        }

        protected override void ContextClickedItem(int id)
        {
            base.ContextClickedItem(id);
            int index = id - 1;
            if (index < 0 || index >= this.scenes.Count)
            {
                return;
            }

            IList<int> selectedIndices = GetSelection();
            List<SceneTreeItem> selectedItems = new();
            if (this.scenes.Count > 0)
            {
                for (int i = 0; i < selectedIndices.Count; i++)
                {
                    int selectedIndex = selectedIndices[i];
                    if (selectedIndex >= this.scenes.Count)
                    {
                        continue;
                    }

                    selectedItems.Add(this.scenes[index]);
                }
            }

            SceneTreeItem sceneItem = this.scenes[index];
            Scene scene = SceneManager.GetSceneByPath(sceneItem.ScenePath);
            bool isOpen = scene.IsValid() && scene.isLoaded;
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("Add To Build"), false, () =>
            {
                this.sceneManagerWindow.AddToBuild(sceneItem.ScenePath);
            });
            menu.AddItem(new GUIContent("Play"), false, () =>
            {
                this.sceneManagerWindow.Open(sceneItem.ScenePath);
                EditorApplication.isPlaying = true;
            });
            menu.AddItem(new GUIContent(isOpen ? "Close" : "Open"), false, () =>
            {
                if (isOpen)
                {
                    EditorSceneManager.CloseScene(scene, true);
                }
                else
                {
                    this.sceneManagerWindow.Open(sceneItem.ScenePath);
                }
            });
            menu.AddItem(new GUIContent("Delete"), false, () =>
            {
                this.sceneManagerWindow.Delete(sceneItem.ScenePath);
            });
            if (selectedItems.Count > 1)
            {
                menu.AddItem(new GUIContent("Bulk/Add Selection To Build"), false, () =>
                {
                    for (int i = 0; i < selectedItems.Count; i++)
                    {
                        this.sceneManagerWindow.AddToBuild(selectedItems[i].ScenePath);
                    }
                });
                menu.AddItem(new GUIContent("Bulk/Delete Selection"), false, () =>
                {
                    for (int i = 0; i < selectedItems.Count; i++)
                    {
                        this.sceneManagerWindow.Delete(selectedItems[i].ScenePath);
                    }
                });
                menu.AddItem(new GUIContent("Bulk/Open Selection Additively"), false, () =>
                {
                    for (int i = 0; i < selectedItems.Count; i++)
                    {
                        this.sceneManagerWindow.Open(selectedItems[i].ScenePath, OpenSceneMode.Additive);
                    }
                });
            }

            menu.ShowAsContext();
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            TreeViewItem item = args.item;

            for (int i = 0; i < args.GetNumVisibleColumns(); ++i)
            {
                SceneTreeItem sceneItem = null;
                if (this.scenes.Count > 0 && args.item.id > 0)
                {
                    sceneItem = this.scenes[args.item.id - 1];
                }

                CellGUI(args.GetCellRect(i), sceneItem, item, (SceneColumnType)args.GetColumn(i), ref args);
            }
        }

        protected void CellGUI(Rect cellRect, SceneTreeItem sceneItem, TreeViewItem item, SceneColumnType column, ref RowGUIArgs args)
        {
            CenterRectUsingSingleLineHeight(ref cellRect);
            Scene scene = SceneManager.GetSceneByPath(sceneItem.ScenePath);
            bool isOpen = scene.IsValid() && scene.isLoaded;
            switch (column)
            {
                case SceneColumnType.Name:
                    GUI.Label(cellRect, sceneItem.Scene.name, args.selected ? EditorStyles.whiteLabel : EditorStyles.wordWrappedLabel);
                    //if (isOpen)
                    //{
                    //    GUILayout.Label(sceneItem.Scene.name, EditorStyles.whiteLabel);
                    //}
                    //else
                    //{
                    //    GUILayout.Label(sceneItem.Scene.name, EditorStyles.wordWrappedLabel);
                    //}
                    break;

                case SceneColumnType.Path:
                    if (this.sceneManagerWindow.ShowPath)
                    {
                        GUI.Label(cellRect, sceneItem.ScenePath, args.selected ? EditorStyles.whiteLabel : EditorStyles.wordWrappedLabel);
                    }
                    break;

                case SceneColumnType.Actions:
                    Rect buildRect = new(cellRect);
                    if (this.sceneManagerWindow.ShowAddToBuild)
                    {
                        buildRect.width = 60f;
                        if (GUI.Button(buildRect, sceneItem.BuildSettingsScene == null ? "+ Build" : "- Build"))
                        {
                            if (sceneItem.BuildSettingsScene == null)
                            {
                                this.sceneManagerWindow.AddToBuild(sceneItem.ScenePath);
                            }
                            else
                            {
                                this.sceneManagerWindow.RemoveFromBuild(sceneItem.ScenePath);
                            }
                        }
                    }

                    Rect playRect = new(cellRect);
                    playRect.width = 60f;
                    playRect.x += buildRect.width;
                    EditorGUI.BeginDisabledGroup(Application.isPlaying);
                    if (GUI.Button(playRect, "Play"))
                    {
                        this.sceneManagerWindow.Open(sceneItem.ScenePath);
                        EditorApplication.isPlaying = true;
                    }
                    EditorGUI.EndDisabledGroup();

                    Rect openCloseRect = new(cellRect);
                    openCloseRect.width = 60f;
                    openCloseRect.x += playRect.x + playRect.width;
                    if (GUI.Button(openCloseRect, isOpen ? "Close" : "Open"))
                    {
                        if (isOpen)
                        {
                            EditorSceneManager.CloseScene(scene, true);
                        }
                        else
                        {
                            this.sceneManagerWindow.Open(sceneItem.ScenePath);
                        }
                    }

                    Rect deleteRect = new(cellRect);
                    deleteRect.width = 60f;
                    deleteRect.x += openCloseRect.x + openCloseRect.width;
                    if (GUI.Button(deleteRect, "Delete"))
                    {
                        this.sceneManagerWindow.Delete(sceneItem.ScenePath);
                    }
                    break;
            }
        }

    }

}