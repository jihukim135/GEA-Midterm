using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Sprites;

public class PlayerController : MonoBehaviour
{
    private GameManager _gameManager;

    [SerializeField] private AudioClip deathClip;
    [SerializeField] private float jumpForce = 1000f;

    private int _jumpCount = 0;
    private bool _isGrounded = false;
    private bool _isDead = false;

    private const float IncreasedGravityScale = 1.25f;
    private bool _isGravityScaleIncreased = false;

    private Rigidbody2D _playerRigidbody;
    private Animator _animator;
    private AudioSource _playerAudio;
    private static readonly int DIe = Animator.StringToHash("Die");
    private static readonly int Grounded = Animator.StringToHash("Grounded");

    private const int MaxHeartCount = 3;
    private int _currentHeartCount = MaxHeartCount;
    [SerializeField] private GameObject[] hearts = new GameObject[MaxHeartCount];

    private bool _isInvincible = false;
    private SpriteRenderer _renderer;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerAudio = GetComponent<AudioSource>();
        _renderer = GetComponent<SpriteRenderer>();

        foreach (var heart in hearts)
        {
            heart.SetActive(true);
        }
    }

    private void Update()
    {
        if (_isDead)
        {
            return;
        }

        _animator.SetBool(Grounded, _isGrounded);

        if (Input.GetMouseButtonDown(0) && _jumpCount < 2)
        {
            _jumpCount++;
            _gameManager.AddScore(1);

            _playerRigidbody.velocity = Vector2.zero;
            _playerRigidbody.AddForce(new Vector2(0, jumpForce));

            _playerAudio.Play();
        }
        else if (Input.GetMouseButtonUp(0) && _playerRigidbody.velocity.y > 0)
        {
            _playerRigidbody.velocity *= 0.5f;
        }

        if (!_isGrounded && _playerRigidbody.velocity.y < 0.1f && !_isGravityScaleIncreased)
        {
            _playerRigidbody.gravityScale *= IncreasedGravityScale;
            _isGravityScaleIncreased = true;
        }
    }

    private void Die()
    {
        _animator.SetTrigger(DIe);
        _playerAudio.clip = deathClip;
        _playerAudio.Play();

        _playerRigidbody.velocity = Vector2.zero;
        _isDead = true;
        GameManager.Instance.OnPlayerDead();
    }

    private void GetDamage()
    {
        if (_isInvincible)
        {
            return;
        }

        if (_currentHeartCount <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(GetInvincible(2f));
        hearts[_currentHeartCount - 1].SetActive(false);
        _currentHeartCount--;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Damage") && !_isDead)
        {
            GetDamage();
            return;
        }

        if (other.CompareTag("Dead") && !_isDead)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.GetContact(0).normal.y < 0.7f)
        {
            return;
        }

        _isGrounded = true;
        _jumpCount = 0;
        _playerRigidbody.gravityScale /= IncreasedGravityScale;
        _isGravityScaleIncreased = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _isGrounded = false;
    }

    private IEnumerator GetInvincible(float duration)
    {
        _isInvincible = true;

        Color color = new Color(1f, 1f, 1f, 0f);
        _renderer.color = color;

        while (_renderer.color.a < 1f)
        {
            color.a += Time.deltaTime / duration;
            _renderer.color = color;

            yield return null;
        }

        _isInvincible = false;
    }
}