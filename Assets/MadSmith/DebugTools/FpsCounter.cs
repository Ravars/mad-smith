using TMPro;
using UnityEngine;

namespace MadSmith.DebugTools
{
    public class FpsCounter : MonoBehaviour
    {
        private int _lastFrameIndex;
        private float[] _frameDeltaTimeArray;
        [SerializeField] private TextMeshProUGUI uiText;

        private void Awake()
        {
            _frameDeltaTimeArray = new float[50];
        }

        private void Update()
        {
            _frameDeltaTimeArray[_lastFrameIndex] = Time.deltaTime;
            _lastFrameIndex = (_lastFrameIndex + 1) % _frameDeltaTimeArray.Length;
            uiText.text = Mathf.RoundToInt(CalculateFPS()).ToString();
        }

        private float CalculateFPS()
        {
            float total = 0f;
            foreach (var deltaTime in _frameDeltaTimeArray)
            {
                total += deltaTime;
            }
            return _frameDeltaTimeArray.Length / total;
        }
    }
}
