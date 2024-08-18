using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Ooble2 : MonoBehaviour
{
    public float climbForce = 3;
    public float maxClimbForce = 10;
    public float maxAirbornForce = 2;
    public float minSpeed = 2;
    public float maxSpeed = 6;
    private float speed = 3;

    public float minSize = 0.8f;
    public float maxSize = 1.2f;
    
    private bool _isMovingRight = true;
    private Rigidbody2D _rb;

    public int _beingClimedCount = 0;
    public Ooble2 _climbOoble = null;
    
    private static List<Ooble2> _oobles = new List<Ooble2>();

    private SpriteRenderer _sprite;

    /// <summary>
    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///                         AUDIO
    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    enum SfxType
    {
        Death,
        Birth,
        Falling,
        Ooh
    }

    static string[] _deathSfx = { "Death-1", "Death-2", "Death-3", "Death-4", "Death-5" };
    static string[] _birthSfx = { "Birth-1", "Birth-2", "Birth-3", "Birth-4" };
    static string[] _fallingSfx = { "Falling-1", "Falling-2", "Falling-3" };
    static string[] _oohSfx = { "Ooh!-1", "Ooh!-2", "Ooh!-3" };
    static string[][] _sfx = { _deathSfx, _birthSfx, _fallingSfx, _oohSfx };

    private static List<float> _deathTimes = new List<float>();
    private static List<float> _birthTimes = new List<float>();
    private static List<float> _fallingTimes = new List<float>();
    private static List<float> _oohTimes = new List<float>();
    static List<float>[] _times = { _deathTimes, _birthTimes, _fallingTimes, _oohTimes };
    static int[] _sfxMaxCountEachCategory = { 2, 2, 2, 2 };

    private static int soundsPlaying = 0;

    private static string SfxTypeToString(SfxType type)
    {
        switch ((int)type)
        {
            case (int)SfxType.Death:
                return "Death";
            case (int)SfxType.Birth:
                return "Birth";
            case (int)SfxType.Falling:
                return "Falling";
            case (int)SfxType.Ooh:
                return "Ooh";
            default:
                return "Invalid case";
        }
    }

    private static bool PlaySound(SfxType soundType)
    {
        if (_times[(int)soundType].Count >= _sfxMaxCountEachCategory[(int)soundType] || soundsPlaying >= 2)
        {
            Debug.Log("Chose not to play sound");
            return false;
        }

        string[] soundArr = _sfx[(int)soundType];
        string soundStr = soundArr[UnityEngine.Random.Range(0, soundArr.Length)];

        float timeLength = AudioPlayer.Instance.SoundEffect(soundStr);
        if (soundType == SfxType.Ooh)
        {
            timeLength *= 100;
        }
        _times[(int)soundType].Add(timeLength + (int)soundType * 2);
        soundsPlaying++;

        Debug.Log("Playing " + soundStr + " from " + SfxTypeToString(soundType));
        return true;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void Dead()
    {
        GetComponent<Animator>().SetBool("dead", true);
        PlaySound(SfxType.Death);

        Debug.Log("Ooble Died");
        AudioPlayer.Instance.oobles -= 1;

        _oobles.Remove(this);
    }

    public void Jump(float strength)
    {
        _isJumping = true;
        _rb.AddForce(Vector2.up * strength, ForceMode2D.Impulse);
        StartCoroutine(JumpRoutine());
    }

    private bool _isJumping;
    IEnumerator JumpRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        _isJumping = false;
    }

    void Start()
    {
        var size = UnityEngine.Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(size, size, size);
        
        _sprite = GetComponent<SpriteRenderer>();
        //_isMovingRight = UnityEngine.Random.value > 0.5f;
        speed = UnityEngine.Random.Range(minSpeed, maxSpeed);
        _rb = GetComponent<Rigidbody2D>();

        PlaySound(SfxType.Birth);        

        AudioPlayer.Instance.oobles += 1;
        _oobles.Add(this);
       
    }
    


    void FixedUpdate()
    {
        // manage audio timers
        for (int list = 0; list < _times.Length; list++)
        {
            for (int timerIndex = 0; timerIndex < _times[list].Count; timerIndex++)
            {
                _times[list][timerIndex] -= Time.fixedDeltaTime;
                if (_times[list][timerIndex] < 0)
                {
                    _times[list].RemoveAt(timerIndex);
                    soundsPlaying--;
                    Debug.Log("Sound ended in " + list);
                }
            }
        }

        // if (!_climbOoble)
        // {
        //     foreach (var ooble in _oobles)
        //     {
        //         if (ooble == this)
        //             continue;
        //
        //         if (Vector2.Distance(transform.position, ooble.transform.position) <= 0.5
        //             && transform.position.y >= ooble.transform.position.y)
        //         {
        //             if ((_isMovingRight && transform.position.x < ooble.transform.position.x) ||
        //                 (!_isMovingRight && transform.position.x > ooble.transform.position.x))
        //             {
        //                 ooble._beingClimedCount++;
        //                 _climbOoble = ooble;
        //                 break;
        //             }
        //         }
        //     }
        // }

        if (_climbOoble)
        {
            float verticalDistance = transform.position.y - _climbOoble.transform.position.y;
            verticalDistance = Mathf.Clamp(verticalDistance, 0, 1);
            float forceMagnitude = Mathf.Min(climbForce / (1 + verticalDistance), maxClimbForce);
            
           _rb.AddForce(Vector2.up * forceMagnitude, ForceMode2D.Impulse);

            PlaySound(SfxType.Ooh);     
        }
        
        if (_beingClimedCount > 0)
        {
            _rb.mass = 10;
            
            if (_rb.velocity.y > 0)
                _rb.velocity = new Vector2(_rb.velocity.x, 0);
        }
        else
        {
            _rb.mass = 1;
            if (_isMovingRight)
            {
                _rb.velocity = new Vector2(speed, _rb.velocity.y);
                _sprite.flipX = false;
            }
            else
            {
                _rb.velocity = new Vector2(-speed, _rb.velocity.y);
                _sprite.flipX = true;
            }

            if (!_isJumping && _rb.velocity.y > maxAirbornForce)
                _rb.velocity = new Vector2(_rb.velocity.x, maxAirbornForce);
        }

    
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 5);
        if (!hit)
        {
            PlaySound(SfxType.Falling);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (_beingClimedCount > 0)
            return;
        
        if (_climbOoble == null && other.transform.tag == "Ooble")
        {
            var ooble = other.transform.GetComponent<Ooble2>();
            
            if (transform.position.y >= ooble.transform.position.y - 0.1)
            {
                if ((_isMovingRight && transform.position.x < ooble.transform.position.x) ||
                    (!_isMovingRight && transform.position.x > ooble.transform.position.x))
                {
                    ooble._beingClimedCount++;
                    _climbOoble = ooble;
                }
            }
        }
        
        //if (other.transform.CompareTag("Wall"))
        //{
        //    if (transform.position.x < other.transform.position.x)
        //        _isMovingRight = false;
        //    else
        //        _isMovingRight = true;
        //    Dead();
        //}
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.CompareTag("Ooble"))
        {
            var ooble = other.transform.GetComponent<Ooble2>();
            if (_climbOoble == ooble)
            {
                ooble._beingClimedCount--;
                _climbOoble = null;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // if (_beingClimedCount > 0)
        //     return;
        
        // if (other.transform.CompareTag("Ooble"))
        // {
        //     if (transform.position.y >= other.transform.position.y)
        //     {
        //         if ((_isMovingRight && transform.position.x < other.transform.position.x) ||
        //             (!_isMovingRight && transform.position.x > other.transform.position.x))
        //         {
        //             var ooble = other.transform.GetComponent<Ooble2>();
        //             ooble._beingClimedCount++;
        //             _climbOoble = ooble;
        //         }
        //     }
        // }
        
        if (other.transform.CompareTag("Wall"))
        {
            ContactFilter2D filter2D = new ContactFilter2D();
            filter2D.SetLayerMask(LayerMask.GetMask("Wall"));
            List<ContactPoint2D> contacts = new List<ContactPoint2D>();
            if (_rb.GetContacts(filter2D, contacts) > 0)
            {
                if (contacts[0].point.y > transform.position.y - 0.2f)
                {
                    _isMovingRight = !_isMovingRight;
                }
            }
        }
    }
}
