using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private int damageAmount = 3;
    [SerializeField] private float knockbackThrust = 0f;

    [Header("Grenade")]
    [SerializeField] private GameObject grenadeLight;
    [SerializeField] private GameObject grenadeExplosionVFX;
    [SerializeField] private float grenadeExplosionRadius = 3f;
    #endregion

    #region Privates
    private Vector2 _throwDirection;
    private Rigidbody2D _rigidBody;
    private Gun _gun;
    private Coroutine _beepRoutine;
    private CinemachineImpulseSource _impulseSource;
    private bool _canBeep = false;
    #endregion
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    public void Init(Gun gun,Vector2 grenadeSpawnPos, Vector2 mousePos)
    {
        _gun = gun;
        transform.position = grenadeSpawnPos;
        _throwDirection = (mousePos - grenadeSpawnPos).normalized;
        _rigidBody.AddForce(_throwDirection * moveSpeed , ForceMode2D.Impulse);
        _rigidBody.AddTorque(rotateSpeed);
        _beepRoutine = StartCoroutine(BeepRoutine());
        _canBeep = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 3f);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable iDamageable = other.gameObject.GetComponent<IDamageable>();
        if(iDamageable != null)
        {
            //iDamageable?.TakeDamage(damageAmount, knockbackThrust);
            Explode();
        }
    }
    private IEnumerator BeepRoutine()
    {
        for (int i = 0; i < 3; i++)
        {
            grenadeLight.SetActive(false);
            yield return new WaitForSeconds(.5f);
            grenadeLight.SetActive(true);
            yield return new WaitForSeconds(1f);
        }

        Explode();
    }
    private void Explode()
    {
        if (_beepRoutine != null)
        {
            StopCoroutine(_beepRoutine);
            _beepRoutine = null;
        }
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,grenadeExplosionRadius);

        IDamageable damageable;
        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<IDamageable>() == null) continue;
            damageable = collider.GetComponent<IDamageable>();
            damageable.TakeDamage(damageAmount, knockbackThrust);
        }
        Instantiate(grenadeExplosionVFX, transform.position, transform.rotation);
        _canBeep = false;
        ExplosionScreenShake();
        _gun.ReleaseGrenadeFromPool(this);
    }
    public bool CanBeep()
    {
        return _canBeep;
    }
    private void ExplosionScreenShake()
    {
        _impulseSource.GenerateImpulse();
    }
}
