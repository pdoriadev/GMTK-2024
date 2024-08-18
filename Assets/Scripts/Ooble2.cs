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

    void Dead()
    {
        GetComponent<Animator>().SetBool("dead", true);
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
        _oobles.Add(this);
    }

    void FixedUpdate()
    {
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
        
        // if (other.transform.CompareTag("Wall"))
        // {
        //     if (transform.position.x < other.transform.position.x)
        //         _isMovingRight = false;
        //     else
        //         _isMovingRight = true;
        //     //Dead();
        // }
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
        if (other.transform.CompareTag("Wall"))
        {
            ContactFilter2D filter2D = new ContactFilter2D();
            filter2D.SetLayerMask(LayerMask.GetMask("Wall"));
            List<ContactPoint2D> contacts = new List<ContactPoint2D>();
            if(_rb.GetContacts(filter2D, contacts) > 0)
            {
                if (contacts[0].point.y > transform.position.y - 0.2f)
                {
                    _isMovingRight = !_isMovingRight;
                }
            }
        }
    }
}
