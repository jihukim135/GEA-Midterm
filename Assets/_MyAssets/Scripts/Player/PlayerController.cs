using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Sprites;

public class PlayerController : MonoBehaviour
{
    private GameManager _gameManager;
    private Rigidbody2D _rigidbody;

    // 점프 관련
    [SerializeField] private float jumpForce;
    private int _jumpCount = 0;
    public int MaxJumpCount { get; set; } = 2;
    private bool _isGrounded = false;
    private const float GravityScaleFactor = 1.25f;
    private bool _isGravityScaleChanged = false;

    // 오디오 관련
    private AudioSource _audioSource;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip hitClip;

    // 애니메이션 관련
    private Animator _animator;
    private static readonly int DIe = Animator.StringToHash("Die");
    private static readonly int Grounded = Animator.StringToHash("Grounded");

    // 피격 및 사망 관련
    private SpriteRenderer _renderer;
    private const int MaxHeartCount = 3;
    private int _currentHeartCount = MaxHeartCount;
    [SerializeField] private GameObject[] hearts = new GameObject[MaxHeartCount];
    private bool _isDead = false;
    public bool IsInvincible { get; set; } = false;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
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

        CheckInputAndJump();
        AdjustGravityScale();
    }

    private void CheckInputAndJump()
    {
        if (Input.GetMouseButtonDown(0) && _jumpCount < MaxJumpCount)
        {
            _jumpCount++;
            _gameManager.AddScore(1);

            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(new Vector2(0, jumpForce));

            _audioSource.clip = jumpClip;
            _audioSource.Play();
        }
        else if (Input.GetMouseButtonUp(0) && _rigidbody.velocity.y > 0)
        {
            _rigidbody.velocity *= 0.5f;
        }
    }

    private void AdjustGravityScale()
    {
        if (!_isGrounded && _rigidbody.velocity.y < 0.1f && !_isGravityScaleChanged)
        {
            _rigidbody.gravityScale *= GravityScaleFactor;
            _isGravityScaleChanged = true;
        }
    }

    private IEnumerator GetDamage()
    {
        if (IsInvincible)
        {
            yield break;
        }

        if (_currentHeartCount <= 0)
        {
            Die();
            yield break;
        }

        IsInvincible = true;

        _audioSource.clip = hitClip;
        _audioSource.Play();

        hearts[_currentHeartCount - 1].SetActive(false);
        _currentHeartCount--;

        Color color = new Color(1f, 1f, 1f, 0f);
        _renderer.color = color;

        while (_renderer.color.a < 1f)
        {
            color.a += Time.deltaTime / 0.5f;
            _renderer.color = color;

            yield return null;
        }

        IsInvincible = false;
    }

    private void Die()
    {
        _animator.SetTrigger(DIe);
        _audioSource.clip = deathClip;
        _audioSource.Play();

        _rigidbody.velocity = Vector2.zero;

        _isDead = true;
        _gameManager.OnPlayerDead();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isDead)
        {
            return;
        }

        if (other.CompareTag("Damage"))
        {
            StartCoroutine(GetDamage());
            return;
        }

        if (other.CompareTag("Dead"))
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
        _rigidbody.gravityScale /= GravityScaleFactor;
        _isGravityScaleChanged = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _isGrounded = false;
    }
}