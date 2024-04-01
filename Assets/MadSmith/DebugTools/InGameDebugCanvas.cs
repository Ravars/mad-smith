using System;
using IngameDebugConsole;
using MadSmith.Scripts.Utils;
using UnityEngine;
using Utils;

namespace MadSmith.DebugTools
{
    public class InGameDebugCanvas : Singleton<InGameDebugCanvas>
    {
        [SerializeField] private FpsCounter fpsCounter;

        protected override void Awake()
        {
            base.Awake();
            fpsCounter.enabled = false;
            fpsCounter.gameObject.SetActive(false);
        }


        [ConsoleMethod("debug.fps", "Show an fps counter")]
        public static void ShowFps(bool status)
        {
            InstantiateSelfIfNotExists();
            Instance.fpsCounter.enabled = status;
            Instance.fpsCounter.gameObject.SetActive(status);
        }

        public static void InstantiateSelfIfNotExists()
        {
            if (!InstanceExists)
            {
                var a = Resources.Load<InGameDebugCanvas>("DebugTools/DebugCanvas");
                Instantiate(a.gameObject);
            }
        }
    }
}