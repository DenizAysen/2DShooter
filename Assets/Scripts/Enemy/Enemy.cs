using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour 
{
    #region Serialized Fiedls
    [SerializeField] private float _jumpForce = 7f;
    [SerializeField] private float _jumpInterval = 4f;
    [SerializeField] private float _changeDirectionInterval = 3f;

    [SerializeField] private int damageAmount = 1;
    [SerializeField] private float knockbackThrust = 25f;
    #endregion

    private float _currentDirection;

    private Rigidbody2D _rigidBody;
    private Movement _movement;
    private ColorChanger _colorChanger;
   
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _movement = GetComponent<Movement>();
        _colorChanger = GetComponent<ColorChanger>();       
    }

    private void Start() {
        StartCoroutine(ChangeDirectionRoutine());
        StartCoroutine(RandomJumpRoutine());
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        Movement movement = other.gameObject.GetComponent<Movement>();
        if (!player) return;

        if (movement.CanMove)
        {
            IHitable iHitable = other.gameObject.GetComponent<IHitable>();
            iHitable?.TakeHit();

            IDamageable iDamageable = other.gameObject.GetComponent<IDamageable>();
            iDamageable?.TakeDamage(transform.position, damageAmount, knockbackThrust);

            AudioManager.Instance.OnPlayerHit();
        }
        
    }
    public void Init(Color color)
    {
        _colorChanger.SetDefaultColor(color);
    }

    private IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            _currentDirection = UnityEngine.Random.Range(0, 2) * 2 - 1; // 1 or -1
            _movement.SetCurrentDirection(_currentDirection);
            yield return new WaitForSeconds(_changeDirectionInterval);
        }
    }

    private IEnumerator RandomJumpRoutine() 
    {
        while (true)
        {
            yield return new WaitForSeconds(_jumpInterval);
            float randomDirection = Random.Range(-1, 1);
            Vector2 jumpDirection = new Vector2(randomDirection, 1f).normalized;
            _rigidBody.AddForce(jumpDirection * _jumpForce, ForceMode2D.Impulse);
        }
    }

   
}
