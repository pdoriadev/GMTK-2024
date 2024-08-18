using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public float oobles = 0;
    public AudioSource convoSource;

    static public AudioPlayer Instance;
    static AudioSource _audio;
    static AudioSource _convoSource;
    
    private static Dictionary<string, AudioClip> _clips;
    private static float _convoTimeLeft = 0;
    
    private string[] _convoSfx = { "Convo-1", "Convo-2" };

    void Awake()
    {
        _convoSource = convoSource;
        _audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _convoTimeLeft -= Time.deltaTime;
        if (_convoTimeLeft <= 0)
        {
            string convo = _convoSfx[UnityEngine.Random.Range(0, _convoSfx.Length)];
            _convoSource.PlayOneShot(_clips[convo]);
            _convoTimeLeft = _clips[convo].length;
        }

        _convoSource.volume = oobles / 50;
    }

    public float SoundEffect(string effect)
    {
        _audio.PlayOneShot(_clips[effect]);
        return _clips[effect].length;
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