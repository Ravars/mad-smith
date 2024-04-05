using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Systems.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace MadSmith.Scripts.UI
{
    public class SettingsPanel : MonoBehaviour
    {
        [SerializeField] private FloatEventChannelSo masterVolumeChannel;
        [SerializeField] private FloatEventChannelSo musicVolumeChannel;
        [SerializeField] private FloatEventChannelSo sfxVolumeChannel;

        [SerializeField] private Slider masterVolumeSlider; 
        [SerializeField] private Slider musicVolumeSlider; 
        [SerializeField] private Slider sfxVolumeSlider; 
        public void SetCurrentStates()
        {
            masterVolumeSlider.value = PlayerPrefs.GetFloat(AudioGroups.MasterVolume.ToString());
            musicVolumeSlider.value = PlayerPrefs.GetFloat(AudioGroups.MusicVolume.ToString());
            sfxVolumeSlider.value = PlayerPrefs.GetFloat(AudioGroups.SfxVolume.ToString());
        }
        
        
        public void ChangeMasterVolume(float value)
        {
            masterVolumeChannel.RaiseEvent(value);
        }
        public void ChangeMusicVolume(float value)
        {
            musicVolumeChannel.RaiseEvent(value);
        }
        public void ChangeSfxVolume(float value)
        {
            sfxVolumeChannel.RaiseEvent(value);
        }
    }
}