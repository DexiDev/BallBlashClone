using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DexiDev.Game.Assets;
using DexiDev.Game.Data;
using DexiDev.Game.Data.Fields;
using DexiDev.Game.Gameplay.Weapons.Stats;
using UnityEngine;
using VContainer;

namespace DexiDev.Game.Gameplay.Weapons
{
    public class WeaponController : DataController
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private Vector2 _offset;
        [SerializeField] private WeaponBullet _weaponBulletContract;
        [SerializeField] private float _bulletWidth;
        [SerializeField] private Animator _animator;
        [SerializeField] private string _speedAnimationKey = "Speed";
        [SerializeField] private string _shootAnimtionKey = "Shoot";
        
        [Inject] private AssetsManager _assetsManager;
        
        private DamagePowerField _damagePowerField;
        private DamageTickRateField _damageTickRateField;
        private ShootCountField _shootCountField;
        private IsBattleStateField _isBattleStateField;
        private DirectionField _directionField;
        
        private CancellationTokenSource _cancellationTokenShoot;
        private HashSet<WeaponBullet> _weaponBulletsCached = new();
        
        private void Awake()
        {
            _directionField = _player.GetDataField<DirectionField>(true);
            _isBattleStateField = _player.GetDataField<IsBattleStateField>(true);
            _damagePowerField = _player.GetDataField<DamagePowerField>(true);
            _damageTickRateField = _player.GetDataField<DamageTickRateField>(true);
            _shootCountField = _player.GetDataField<ShootCountField>(true);
        }

        private void OnEnable()
        {
            ShootLoop();

            OnDirectionChanged(_directionField.Value);
            _directionField.OnChanged += OnDirectionChanged;
        }

        private void OnDisable()
        {
            _directionField.OnChanged -= OnDirectionChanged;
            _cancellationTokenShoot.Cancel();
            foreach (var weaponBullet in _weaponBulletsCached.ToArray())
            {
                OnWeaponBulletPoolable(weaponBullet);
                weaponBullet.gameObject.SetActive(false);
            }
            _weaponBulletsCached.Clear();
        }

        private void OnDirectionChanged(Vector3 direction)
        {
            if (_animator != null)
            {
                _animator.SetFloat(_speedAnimationKey, direction.x);
            }
        }

        private async void ShootLoop()
        {
            _cancellationTokenShoot = new();
            try
            {
                while (!_cancellationTokenShoot.IsCancellationRequested)
                {
                    if (_isBattleStateField.Value)
                    {
                        Shoot();

                        if (_damageTickRateField.Value > 0)
                        {
                            await UniTask.Delay(TimeSpan.FromSeconds(_damageTickRateField.Value), cancellationToken: _cancellationTokenShoot.Token);
                            continue;
                        }
                    }
                    await UniTask.Yield(cancellationToken: _cancellationTokenShoot.Token);
                }
            }
            catch (OperationCanceledException) { }
        }
        
        private void Shoot()
        {
            float totalSpacing = _offset.x + _bulletWidth;
            Vector3 shootPosition = transform.position;
            shootPosition.y += _offset.y;

            for (var i = 0; i < _shootCountField.Value; i++)
            {
                float positionX = shootPosition.x;

                if (_shootCountField.Value % 2 == 0)
                {
                    int halfCount = _shootCountField.Value / 2;
                    positionX += totalSpacing * (i - halfCount + 0.5f);
                }
                else
                {
                    if (i == 0) positionX += 0;
                    else
                    {
                        int sideIndex = (i - 1) / 2;
                        positionX += totalSpacing * (sideIndex + 1) * (i % 2 == 1 ? 1 : -1);
                    }
                }

                var weaponBullet = _assetsManager.GetAsset<WeaponBullet>(_weaponBulletContract, new Vector3(positionX, shootPosition.y, shootPosition.z), Quaternion.identity, null);
                weaponBullet.Init(_damageTickRateField.Value, _damagePowerField.Value);
                weaponBullet.OnPoolable += OnWeaponBulletPoolable;
                
                if(_animator != null) _animator.SetTrigger(_shootAnimtionKey);
            }
        }

        private void OnWeaponBulletPoolable(IAssetInstance assetInstance)
        {
            assetInstance.OnPoolable -= OnWeaponBulletPoolable;
            
            if (assetInstance is WeaponBullet weaponBullet)
            {
                _weaponBulletsCached.Remove(weaponBullet);
            }
        }
    }
}