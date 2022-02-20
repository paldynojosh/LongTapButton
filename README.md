# LongTapButton
Image/RawImageを長押し可能ボタンにするコンポーネントを提供します。

# Dependencies
* UniRx
    * 2.2.5で動作確認済
* UniTask
    * 7.1.0で動作確認済

# Usage
1. LongTapEventHandlerコンポーネントをImage/RawImageにアタッチ
1. LongTapEventHandlerコンポーネントのパラメータを指定
    - DelayStartLongTap: 長押し判定開始までの時間、これ以前に指を離すとボタン単押し判定
    - FireLongTapTime: 長押し待ちの時間、これが完了したら長押し判定
1. LongTapEventHandlerコンポーネントのイベントを自由にSubscribeする

## 使えるEvent
### OnTap
* 単押しした(DelayStartLongTapを完了する前に指を離した)
### OnLongTap
* 長押しした
### OnPointerDown
* ボタンを押した(長押し待ちを開始した)
### OnPointerUpWithoutEvent
* 単押し/長押しではない状態でボタンを離した
### OnPointed
* ボタンの上にカーソルがあるか
### TapTimeRatioAsObservable
* 長押し判定の経過割合
