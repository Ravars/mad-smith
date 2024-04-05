using System;
using MadSmith.Scripts.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Audio;

namespace MadSmith.Scripts.Systems.Settings
{
    [Serializable]
    public enum AudioGroups
    {
        MasterVolume,
        MusicVolume,
        SfxVolume
    }
    public class AudioManager : MonoBehaviour
    {
        [Header("Audio control")]
        [SerializeField] private AudioMixer audioMixer = default;
        [Range(0f, 1f)]
        [SerializeField] private float masterVolume = 1f;
        [Range(0f, 1f)]
        [SerializeField] private float musicVolume = 1f;
        [Range(0f, 1f)]
        [SerializeField] private float sfxVolume = 1f;
        
        
        [Header("Listening on channels")]
        [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to change Master volume")]
        [SerializeField] private FloatEventChannelSo masterVolumeChannel;
        [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to change Music volume")]
        [SerializeField] private FloatEventChannelSo musicVolumeChannel;
        [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to change SFXs volume")]
        [SerializeField] private FloatEventChannelSo sfxVolumeChannel;
        public static readonly int MaxVolume = 1;
        public static readonly int StepVolume = 1;

        private void Start()
        {
            SetMasterVolume(PlayerPrefs.GetFloat(AudioGroups.MasterVolume.ToString()));
            SetMusicVolume(PlayerPrefs.GetFloat(AudioGroups.MusicVolume.ToString()));
            SetSfxVolume(PlayerPrefs.GetFloat(AudioGroups.SfxVolume.ToString()));
        }

        private void OnEnable()
        {
            masterVolumeChannel.OnEventRaised += SetMasterVolume;
            musicVolumeChannel.OnEventRaised += SetMusicVolume;
            sfxVolumeChannel.OnEventRaised += SetSfxVolume;
        }

        private void OnDestroy()
        {
            masterVolumeChannel.OnEventRaised -= SetMasterVolume;
            musicVolumeChannel.OnEventRaised -= SetMusicVolume;
            sfxVolumeChannel.OnEventRaised -= SetSfxVolume;
        }
        public void SetMasterVolume(float newVolume)
        {
            masterVolume = newVolume;
            SetGroupVolume(AudioGroups.MasterVolume.ToString(),newVolume);
        }
        public void SetMusicVolume(float newVolume)
        {
            musicVolume = newVolume;
            SetGroupVolume(AudioGroups.MusicVolume.ToString(),newVolume);
        }
        public void SetSfxVolume(float newVolume)
        {
            sfxVolume = newVolume;
            SetGroupVolume(AudioGroups.SfxVolume.ToString(),newVolume);
        }

        private void SetGroupVolume(string groupName, float volume)
        {
            volume /= MaxVolume;
            PlayerPrefs.SetFloat(groupName, volume);
            bool volumeSet = audioMixer.SetFloat(groupName, LogarithmicDbTransform(volume));
            if (!volumeSet)
            {
                Debug.LogError("AudioMixer not found.");
            }
        }


        public static float LogarithmicDbTransform(float volume)
        {
            volume = Mathf.Clamp01(volume);
            volume = (Mathf.Log(89 * volume + 1) / Mathf.Log(90)) * 80;
            float retorno = volume - 80;
            return retorno;
        }
    }
}