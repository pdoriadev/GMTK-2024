using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    static public AudioPlayer Instance;
    static AudioSource _audio;
    
    private static Dictionary<string, AudioClip> _clips;
    
    void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void SoundEffect(string effect)
    {
        _audio.PlayOneShot(_clips[effect]);
    }
    
    // ----------------------
    // ----------------------
    // Seralize the Dictionary
    [SerializeField]
    private List<StringAudioClipPair> audioClipList;
    private void OnValidate()
    {
        // Convert the list to a dictionary for easy access at runtime
        _clips = new Dictionary<string, AudioClip>();
        foreach (var pair in audioClipList)
        {
            if (!_clips.ContainsKey(pair.key))
            {
                _clips.Add(pair.key, pair.value);
            }
            else
            {
                Debug.LogWarning($"Duplicate key found: {pair.key}. Ignoring duplicate.");
            }
        }
    }
    
    [System.Serializable]
    public class StringAudioClipPair
    {
        public string key;
        public AudioClip value;
    }
}