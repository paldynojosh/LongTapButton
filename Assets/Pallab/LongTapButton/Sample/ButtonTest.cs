using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Pallab.LongTapButton.Sample
{
    namespace Scenes
    {
        public class ButtonTest : MonoBehaviour
        {
            [SerializeField] private LongTapEventHandler _tapHandler;
            [SerializeField] private Image _image;
            [SerializeField] private Image _select;
            [SerializeField] private Image _progress;

            private void Start()
            {
                _tapHandler.OnTap
                    .Subscribe(_ =>
                    {
                        _image.color = Color.white;
                        Debug.Log("tap");
                    });
                _tapHandler.OnLongTap
                    .Subscribe(_ =>
                    {
                        _image.color = Color.white;
                        Debug.Log("long tap");
                    });
                _tapHandler.OnPointerDown
                    .Subscribe(_ => _image.color = Color.gray);
                _tapHandler.OnPointerUpWithoutEvent
                    .Subscribe(_ => _image.color = Color.white);

                _tapHandler.OnPointed
                    .Subscribe(val => _select.gameObject.SetActive(val));
                _tapHandler.TapTimeRatioAsObservable
                    .Subscribe(val =>
                    {
                        {
                            _progress.fillAmount = val;
                        }
                    });
            }
        }
    }
}
