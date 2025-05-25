using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class SFXManagerAdvanced : MonoBehaviour
{
    public static SFXManagerAdvanced Instance;

    [Header("Audio Settings")]
    [SerializeField] private GameObject soundFXPrefab;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;
    [SerializeField] private bool enablePooling = false;
    [SerializeField] private int poolSize = 10;

    private Queue<AudioSource> audioPool;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (enablePooling)
        {
            audioPool = new Queue<AudioSource>();
            for (int i = 0; i < poolSize; i++)
            {
                AudioSource src = CreateNewSource();
                src.gameObject.SetActive(false);
                audioPool.Enqueue(src);
            }
        }
    }

    private AudioSource CreateNewSource()
    {
        GameObject go = Instantiate(soundFXPrefab);
        AudioSource src = go.GetComponent<AudioSource>();
        if (src == null) src = go.AddComponent<AudioSource>();
        src.outputAudioMixerGroup = sfxMixerGroup;
        return src;
    }

    private AudioSource GetSource(Vector3 position)
    {
        AudioSource source;
        if (enablePooling && audioPool.Count > 0)
        {
            source = audioPool.Dequeue();
            source.transform.position = position;
            source.gameObject.SetActive(true);
        }
        else
        {
            source = CreateNewSource();
            source.transform.position = position;
        }
        return source;
    }

    public void PlaySFX(AudioClip clip, Vector3 position, float volume = 1f, bool randomPitch = false, bool spatial3D = true)
    {
        if (clip == null) return;

        AudioSource src = GetSource(position);
        src.clip = clip;
        src.volume = volume;
        src.pitch = randomPitch ? Random.Range(0.95f, 1.05f) : 1f;
        src.spatialBlend = spatial3D ? 1f : 0f;
        src.Play();

        StartCoroutine(DisableAfterPlay(src, clip.length));
    }

    public void PlayRandomSFX(AudioClip[] clips, Vector3 position, float volume = 1f, bool randomPitch = false, bool spatial3D = true)
    {
        if (clips == null || clips.Length == 0) return;

        int index = Random.Range(0, clips.Length);
        AudioClip selectedClip = clips[index];
        PlaySFX(selectedClip, position, volume, randomPitch, spatial3D);
    }

    private System.Collections.IEnumerator DisableAfterPlay(AudioSource source, float duration)
    {
        yield return new WaitForSeconds(duration);

        if (enablePooling)
        {
            source.Stop();
            source.gameObject.SetActive(false);
            audioPool.Enqueue(source);
        }
        else
        {
            Destroy(source.gameObject);
        }
    }
}
