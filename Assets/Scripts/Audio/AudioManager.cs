using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    #region Serialized Fields
    [Range(0f, 2f)]
    [SerializeField] private float masterVolume = 1f;
    [SerializeField] private SoundsCollectionSO soundsCollectionSO;

    [SerializeField] private AudioMixerGroup sfxMixerGroup;
    [SerializeField] private AudioMixerGroup musicMixerGroup; 
    #endregion

    private AudioSource _currentMusic;

    #region Unity Methods
    private void OnEnable()
    {
        SubscribeEvents();
    }
    private void SubscribeEvents()
    {
        Gun.OnShoot += OnShoot;
        PlayerController.OnJump += OnJump;
        PlayerController.OnJetpack += OnJetpack;
        Health.OnDeath += OnDeath;
        DiscoBallManager.OnDiscoBallHitEvent += DiscoBallMusic;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    private void UnSubscribeEvents()
    {
        Gun.OnShoot -= OnShoot;
        PlayerController.OnJump -= OnJump;
        PlayerController.OnJetpack -= OnJetpack;
        Health.OnDeath -= OnDeath;
        DiscoBallManager.OnDiscoBallHitEvent -= DiscoBallMusic;
    }
    private void Start()
    {
        FightMusic();
    }

    #endregion

    #region Sound Methods
    private void PlayRandomSound(SoundSO[] sounds)
    {
        if (sounds != null && sounds.Length > 0)
        {
            SoundSO sound = sounds[Random.Range(0, sounds.Length)];
            SoundToPlay(sound);
        }
    }
    private void SoundToPlay(SoundSO soundSO)
    {
        AudioClip clip = soundSO.Clip;
        float pitch = soundSO.Pitch;
        float volume = soundSO.Volume * masterVolume;
        bool loop = soundSO.Loop;
        AudioMixerGroup audioMixerGroup;

        pitch = RandomizePitch(soundSO, pitch);
        audioMixerGroup = DetermineAudioMixerGroup(soundSO);

        PlaySound(clip, pitch, volume, loop, audioMixerGroup);
    }

    private AudioMixerGroup DetermineAudioMixerGroup(SoundSO soundSO)
    {
        AudioMixerGroup audioMixerGroup;
        switch (soundSO.AudioType)
        {
            case AudioTypes.SFX:
                audioMixerGroup = sfxMixerGroup;
                break;

            case AudioTypes.Music:
                audioMixerGroup = musicMixerGroup;
                break;

            default:
                audioMixerGroup = null;
                break;
        }

        return audioMixerGroup;
    }

    private static float RandomizePitch(SoundSO soundSO, float pitch)
    {
        if (soundSO.RandomizePitch)
        {
            float randomPitchModifier = Random.Range(-soundSO.RandomPitchRangeModifier, soundSO.RandomPitchRangeModifier);
            pitch = soundSO.Pitch + randomPitchModifier;
        }

        return pitch;
    }

    private void PlaySound(AudioClip clip, float Pitch, float Volume, bool Loop, AudioMixerGroup audioMixerGroup)
    {
        GameObject soundObject = new GameObject("Temp Audio Source");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        //AudioSource audioSource = SoundObject.GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.pitch = Pitch;
        audioSource.volume = Volume;
        audioSource.loop = Loop;
        audioSource.outputAudioMixerGroup = audioMixerGroup;
        audioSource.Play();

        if (!Loop) { Destroy(soundObject, clip.length); }

        DetermineMusic(audioMixerGroup, audioSource);
    }

    private void DetermineMusic(AudioMixerGroup audioMixerGroup, AudioSource audioSource)
    {
        if (audioMixerGroup == musicMixerGroup)
        {
            if (_currentMusic != null)
            {
                _currentMusic.Stop();
            }

            _currentMusic = audioSource;
        }
    }

    #endregion

    #region SFX
    private void OnShoot()
    {
        PlayRandomSound(soundsCollectionSO.GunShoot);
    }
    private void OnJump()
    {
        PlayRandomSound(soundsCollectionSO.Jump);
    }
    private void OnDeath(Health health)
    {
        PlayRandomSound(soundsCollectionSO.Splat);
    }
    private void OnJetpack()
    {
        PlayRandomSound(soundsCollectionSO.Jetpack);
    }
    #endregion

    #region Music
    private void FightMusic()
    {
        PlayRandomSound(soundsCollectionSO.FightMusic);
    }
    private void DiscoBallMusic()
    {
        PlayRandomSound(soundsCollectionSO.DiscoParty);
        float soundLength = soundsCollectionSO.DiscoParty[0].Clip.length;
        Utils.RunAfterDelay(this, soundLength, FightMusic);
    } 

    #endregion
}
