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

        private readonly FloatReactiveProperty _currentTapTimeRatio = new FloatReactiveProperty();
        private readonly BoolReactiveProperty _isPointed = new BoolReactiveProperty();

        private readonly Subject<Unit> _onTap = new Subject<Unit>();
        private readonly Subject<Unit> _onLongTap = new Subject<Unit>();
        private readonly Subject<Unit> _onPointerDown = new Subject<Unit>();
        private readonly Subject<Unit> _onPointerUpWithoutEvent = new Subject<Unit>();

        public IObservable<float> TapTimeRatioAsObservable => _currentTapTimeRatio;
        public IObservable<bool> OnPointed => _isPointed;

        public IObservable<Unit> OnTap => _onTap;
        public IObservable<Unit> OnLongTap => _onLongTap;
        public IObservable<Unit> OnPointerDown => _onPointerDown;
        public IObservable<Unit> OnPointerUpWithoutEvent => _onPointerUpWithoutEvent;

        private CancellationTokenSource _cts = new CancellationTokenSource();
        private UniTask _lastWaitTask;

        private void Reset()
        {
            _raycastImage = GetComponent<Graphic>();
            _delayStartLongTap = 1.0f;
            _fireLongTapTime = 0.5f;
        }

        private void Start()
        {
            Debug.Assert(_raycastImage != null && _delayStartLongTap > 0);

            _raycastImage.OnPointerEnterAsObservable()
                .Subscribe(_ => { _isPointed.Value = true; });
            _raycastImage.OnPointerExitAsObservable()
                .Subscribe(_ =>
                {
                    _isPointed.Value = false;
                    _cts?.Cancel();
                    _cts = new CancellationTokenSource();
                });

            _raycastImage.OnPointerDownAsObservable()
                .Where(_ => _lastWaitTask.Status.IsCompleted())
                .Subscribe(_ =>
                {
                    _onPointerDown.OnNext(Unit.Default);
                    _lastWaitTask = WaitTapEvent();
                });
            _raycastImage.OnPointerUpAsObservable()
                .Subscribe(_ =>
                {
                    _cts?.Cancel();
                    _cts = new CancellationTokenSource();
                });
        }

        private async UniTask WaitTapEvent()
        {
            _currentTapTimeRatio.Value = 0.0f;
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_delayStartLongTap), cancellationToken: _cts.Token);
            }
            catch (Exception e) when (e is OperationCanceledException)
            {
                if (_isPointed.Value)
                {
                    _onTap.OnNext(Unit.Default);
                }
                else
                {
                    _onPointerUpWithoutEvent.OnNext(Unit.Default);
                }

                return;
            }

            try
            {
                for (var tapTime = 0.0f; tapTime < _fireLongTapTime; tapTime += Time.deltaTime)
                {
                    _currentTapTimeRatio.Value = tapTime / _fireLongTapTime;
                    await UniTask.DelayFrame(1, cancellationToken: _cts.Token);
                }

                _onLongTap.OnNext(Unit.Default);
            }
            catch (Exception e) when (e is OperationCanceledException)
            {
                _onPointerUpWithoutEvent.OnNext(Unit.Default);
            }

            _currentTapTimeRatio.Value = 0.0f;
        }
    }
}
