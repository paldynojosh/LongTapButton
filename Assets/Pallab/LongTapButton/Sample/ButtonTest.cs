using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Pallab.LongTapButton.Sample
{
    namespace Scenes
    {
        public class ButtonTest : MonoBehaviour
        {
            [SerializeField] private LongTapEventHandler _longTapHandler;
            [SerializeField] private Image _image;
            [SerializeField] private Image _select;
            [SerializeField] private Image _progress;

            private void Start()
            {
                _longTapHandler.OnLongTap
                    .Subscribe(_ =>
                    {
                        _image.color = Color.white;
                        Debug.Log("long tap");
                    });
            }
        }
    }
}
