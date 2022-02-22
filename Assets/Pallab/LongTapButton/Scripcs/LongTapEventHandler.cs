using System;
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

        private void Start()
        {
            Debug.Assert(_raycastImage != null && _delayStartLongTap > 0);

            _raycastImage.OnPointerDownAsObservable()
                .Subscribe(_ =>
                {
                })
                .AddTo(this);
        }
    }
}
