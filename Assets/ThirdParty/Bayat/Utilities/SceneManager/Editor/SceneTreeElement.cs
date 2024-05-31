using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEditor.IMGUI.Controls;

using UnityEngine;

namespace Bayat.Games.Scenes.Utilities
{

    public class SceneTreeItem
    {

        protected string scenePath;
        protected SceneAsset scene;
        protected EditorBuildSettingsScene buildSettingsScene;

        public string ScenePath => this.scenePath;
        public SceneAsset Scene => this.scene;
        public EditorBuildSettingsScene BuildSettingsScene => this.buildSettingsScene;

        public SceneTreeItem(string scenePath, SceneAsset scene, EditorBuildSettingsScene buildSettingsScene)
        {
            this.scenePath = scenePath;
            this.scene = scene;
            this.buildSettingsScene = buildSettingsScene;
        }

    }

}