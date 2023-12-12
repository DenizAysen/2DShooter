using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Cinemachine;
public class Gun : MonoBehaviour
{
    public static Action OnShoot;

    #region SerializedFields
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform bulletParent;
    [SerializeField] private float gunFireCD = .5f;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private float muzzleFlashTime = .05f;
    #endregion

    #region Privates
    private Coroutine _muzzleFlashRoutine;
    private static readonly int FIRE_HASH = Animator.StringToHash("Fire");
    private ObjectPool<Bullet> _bulletPool;
    private Vector2 _mousePos;
    private float _lastFireTime = 0f;
    private Animator _animator;
    private CinemachineImpulseSource _impulseSource;
    private Vector2 _direction; 
    #endregion
    private void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        SubscribeEvents();
    }
    private void Start()
    {
        CreateBulletPool();
    }
    private void SubscribeEvents()
    {
        OnShoot += ShootProjectile;
        OnShoot += FireAnimation;
        OnShoot += GunScreenShake;
        OnShoot += MuzzleFlash;
    }
    private void OnDisable()
    {
        UnSubscribeEvents();
    }
    private void UnSubscribeEvents()
    {
        OnShoot -= ShootProjectile;
        OnShoot -= FireAnimation;
        OnShoot -= GunScreenShake;
        OnShoot -= MuzzleFlash;
    }
    public void ReleaseBulletFromPool(Bullet bullet)
    {
        _bulletPool.Release(bullet);
    }
    private void CreateBulletPool()
    {
        _bulletPool = new ObjectPool<Bullet>(() => { return Instantiate(_bulletPrefab,bulletParent); },
            bullet => { bullet.gameObject.SetActive(true); }, 
            bullet => { bullet.gameObject.SetActive(false); }, 
            bullet => { Destroy(bullet); },
            false, 20, 40);
    }
    private void Update()
    {
        Shoot();
        RotateGun();
    }

    private void Shoot()
    {
        if (Input.GetMouseButton(0)) {
            _lastFireTime += Time.deltaTime;

            if (_lastFireTime >= gunFireCD)
            {
                _lastFireTime = 0;
                OnShoot?.Invoke();
            }
        }
    }

    private void ShootProjectile()
    {
        Bullet newBullet = _bulletPool.Get();
        newBullet.Init(this, _bulletSpawnPoint.position, _mousePos);
    }
    private void FireAnimation()
    {
        _animator.Play(FIRE_HASH, 0, 0f);
    }
    private void GunScreenShake()
    {
        _impulseSource.GenerateImpulse();
    }
    private void RotateGun()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //_direction = _mousePos - (Vector2)PlayerController.Instance.transform.position;
        _direction = PlayerController.Instance.transform.InverseTransformPoint(_mousePos);
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
    private void MuzzleFlash()
    {
        if(_muzzleFlashRoutine != null)
        {
            StopCoroutine(_muzzleFlashRoutine);
        }

        _muzzleFlashRoutine = StartCoroutine(MuzzleFlashRoutine());
    }
    private IEnumerator MuzzleFlashRoutine()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(muzzleFlashTime);
        muzzleFlash.SetActive(false);
    }
}
