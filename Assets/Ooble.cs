// using System;
// using System.Collections;
// using System.Collections.Generic;
// using TMPro;
// using UnityEditorInternal;
// using UnityEngine;
// using UnityEngine.Serialization;
// using Random = System.Random;
//
// public class Ooble : MonoBehaviour
// {
//     public bool isBeingClimbed;
//     
//     [SerializeField] private float minSpeed = 2;
//     [SerializeField] private float maxSpeed = 6;
//     private float speed;
//     public float radius = 0.5f;
//     public float climbSpeed = 5f;
//     
//     private bool _isMovingLeft = false;
//
//     private static List<Ooble> _oobles = new List<Ooble>();
//     private static Dictionary<Ooble, int> _prios = new Dictionary<Ooble, int>();
//     private static int _prioIndex = 0;
//
//     public bool climbing;
//     public float maxClimbForce = 1;
//     public float flyingMaxSpeed = 0.2f;
//     public Vector3 climbPosition;
//
//     private Rigidbody2D _rb;
//     
//     void Start()
//     {
//         speed = UnityEngine.Random.Range(minSpeed, maxSpeed);
//         _oobles.Add(this);
//         _prios.Add(this, _prioIndex++);
//         _rb = GetComponent<Rigidbody2D>();
//     }
//     
//     void Update()
//     {
//         var count = 0;
//         foreach (var ooble in _oobles)
//         {
//             if(ooble == this)
//                 continue;
//         
//             if (Vector2.Distance(transform.position, ooble.transform.position) <= radius)
//             {
//                 count++;
//                 // if (_prios[this] < _prios[ooble])
//                 // {
//                 //     Debug.Log(name);
//                 //     if (transform.position.x < ooble.transform.position.x)
//                 //     {
//                 //         climbing = true;
//                 //         climbPosition = ooble.transform.position;
//                 //     }
//                 // }
//             }
//         }
//         //
//         // if (climbing)
//         // {
//         //     _rb.gravityScale = 0;
//         //     if (transform.position.x >= climbPosition.x)
//         //         climbing = false;
//         //     else
//         //         MoveRightAndAvoid(transform.position, climbPosition);
//         // }
//         
//         if(!climbing)
//         {
//             _rb.gravityScale = 1;
//             Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);
//             foreach (var hitCollider in hitColliders)
//             {
//                 if (hitCollider.CompareTag("Wall"))
//                 {
//                     Debug.Log("InWall!");
//                     if (transform.position.x < hitCollider.transform.position.x)
//                         _isMovingLeft = true;
//                     else
//                         _isMovingLeft = false;
//                 }
//             }
//
//             // Vector2 targetVelocity;
//             if (_isMovingLeft)
//             {
//                 // targetVelocity = new Vector2(-speed, 0);
//                 _rb.velocity = new Vector2(-speed, _rb.velocity.y);
//             }
//             else
//             {
//                 // targetVelocity = new Vector2(speed, 0);
//                 _rb.velocity = new Vector2(speed, _rb.velocity.y);
//             }
//             
//             // Vector2 currentVelocity = _rb.velocity;
//             // _rb.AddForce((targetVelocity - currentVelocity) * speed);
//
//             // if (_rb.velocity.y > flyingMaxSpeed && count < 1)
//             //     _rb.velocity = new Vector2(_rb.velocity.x, flyingMaxSpeed);
//         }
//     }
//     
//     // void MoveRightAndAvoid(Vector3 a, Vector3 b)
//     // {
//     //     Vector2 centerToCenter = b - a;
//     //     Vector2 collisionNormal = centerToCenter.normalized;
//     //     Vector2 tangentDirection = new Vector2(-collisionNormal.y, collisionNormal.x);
//     //     Vector2 moveDirection = (tangentDirection).normalized;
//     //
//     //     transform.position += (Vector3)moveDirection * Time.deltaTime;
//     // }
//
//     
//     private void OnCollisionStay2D(Collision2D other)
//     {
//         if (isBeingClimbed)
//             return;
//         
//         if (other.transform.CompareTag("Ooble"))
//         {
//             if (transform.position.y >= other.transform.position.y || other.transform.GetComponent<Ooble>().isBeingClimbed)
//             {
//                 // _rb.velocity = new Vector2(_rb.velocity.x, climbSpeed);
//                 other.transform.GetComponent<Ooble>().isBeingClimbed = true;
//                 _climbingOoble = other.transform.gameObject;
//                 
//                 // Calculate the vertical distance between the spheres
//                 float verticalDistance = transform.position.y - other.transform.position.y;
//                 // Clamp the distance to avoid extreme values
//                 verticalDistance = Mathf.Clamp(verticalDistance, 0, 1);
//                 
//                 // Calculate the force to apply, with a maximum limit
//                 float forceMagnitude = Mathf.Min(climbSpeed / (1 + verticalDistance), maxClimbForce);
//                 
//                 // Apply the force to the rigidbody
//                 _rb.AddForce(
//                     Vector2.up * forceMagnitude,
//                     ForceMode2D.Impulse);
//             }
//         }
//     }
//
//     private GameObject _climbingOoble = null;
//     private void OnCollisionExit2D(Collision2D other)
//     {
//         if (_climbingOoble == other.transform.gameObject)
//         {
//             other.transform.GetComponent<Ooble>().isBeingClimbed = false;
//             _climbingOoble = null;
//         }
//     }
//
//     // void OnCollisionEnter2D(Collision2D collision)
//     // {
//     //     if (collision.transform.CompareTag("Wall"))
//     //     {
//     //         if (transform.position.x < collision.transform.position.x)
//     //             _isMovingLeft = true;
//     //         else
//     //             _isMovingLeft = false;
//     //     }
//     // }
// }
