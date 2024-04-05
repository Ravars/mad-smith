using System;
using MadSmith.Scripts.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Audio;
using Utils;

namespace MadSmith.Scripts.Systems.Settings
{
    [Serializable]
    public enum AudioGroups
    {
        MasterVolume,
        MusicVolume,
        SfxVolume
    }
    public class AudioManager : PersistentSingleton<AudioManager>
    {
        [Header("Audio control")]
        [SerializeField] private AudioMixer audioMixer = default;
        [Range(0f, 1f)]
        [SerializeField] private float defaultMasterVolume = 1f;
        [Range(0f, 1f)]
        [SerializeField] private float defaultMusicVolume = 0.8f;
        [Range(0f, 1f)]
        [SerializeField] private float defaultSfxVolume = 1f;
        
        
        [Header("Listening on channels")]
        [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to change Master volume")]
        [SerializeField] private FloatEventChannelSo masterVolumeChannel;
        [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to change Music volume")]
        [SerializeField] private FloatEventChannelSo musicVolumeChannel;
        [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to change SFXs volume")]
        [SerializeField] private FloatEventChannelSo sfxVolumeChannel;

        private static readonly int MaxVolume = 1;

        private void Start()
        {
            SetMasterVolume(PlayerPrefs.GetFloat(AudioGroups.MasterVolume.ToString(), defaultMasterVolume));
            SetMusicVolume(PlayerPrefs.GetFloat(AudioGroups.MusicVolume.ToString(), defaultMusicVolume));
            SetSfxVolume(PlayerPrefs.GetFloat(AudioGroups.SfxVolume.ToString(), defaultSfxVolume));
        }

        private void OnEnable()
        {
            masterVolumeChannel.OnEventRaised += SetMasterVolume;
            musicVolumeChannel.OnEventRaised += SetMusicVolume;
            sfxVolumeChannel.OnEventRaised += SetSfxVolume;
        }

        protected override void OnDestroy()
        {
            masterVolumeChannel.OnEventRaised -= SetMasterVolume;
            musicVolumeChannel.OnEventRaised -= SetMusicVolume;
            sfxVolumeChannel.OnEventRaised -= SetSfxVolume;
            base.OnDestroy();
        }

        private void SetMasterVolume(float newVolume)
        {
            defaultMasterVolume = newVolume;
            SetGroupVolume(AudioGroups.MasterVolume.ToString(),newVolume);
        }

        private void SetMusicVolume(float newVolume)
        {
            defaultMusicVolume = newVolume;
            SetGroupVolume(AudioGroups.MusicVolume.ToString(),newVolume);
        }

        private void SetSfxVolume(float newVolume)
        {
            defaultSfxVolume = newVolume;
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


        private static float LogarithmicDbTransform(float volume)
        {
            volume = Mathf.Clamp01(volume);
            volume = (Mathf.Log(89 * volume + 1) / Mathf.Log(90)) * 80;
            float retorno = volume - 80;
            return retorno;
        }
    }
}