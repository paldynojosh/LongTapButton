using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Pallab.LongTapButton
{
    [RequireComponent(typeof(Graphic))]
    public class LongTapEventHandler : MonoBehaviour
    {
        [SerializeField] private Graphic _raycastImage;
        [SerializeField] private float _delayStartLongTap;
        [SerializeField] private float _fireLongTapTime;

        private readonly Subject<Unit> _onLongTap = new Subject<Unit>();

        public IObservable<Unit> OnLongTap => _onLongTap;

        private void Reset()
        {
            _raycastImage = GetComponent<Graphic>();
            _delayStartLongTap = 1.0f;
            _fireLongTapTime = 0.5f;
        }

        private void Awake()
        {
            _raycastImage = GetComponent<Graphic>();
            Validate();

            _raycastImage.OnPointerDownAsObservable()
                .Subscribe(_ =>
                {
                    WaitFinishLongTap(this.GetCancellationTokenOnDestroy()).Forget();
                })
                .AddTo(this);
        }

        private async UniTaskVoid WaitFinishLongTap(CancellationToken cancellationToken)
        {
            var time = Time.time;
            await _raycastImage.OnPointerUpAsObservable().Take(1).ToUniTask(cancellationToken: cancellationToken);

            if (_delayStartLongTap + _fireLongTapTime < Time.time - time)
            {
                _onLongTap.OnNext(Unit.Default);
            }
        }

        private void Validate()
        {
            Debug.Assert(_raycastImage != null && _delayStartLongTap > 0);
        }
    }
}
