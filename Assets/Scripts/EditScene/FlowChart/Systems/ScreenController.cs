using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class ScreenController : MonoBehaviour
{
    // ２本指のタッチ情報
    private TouchState _touchState0;
    private TouchState _touchState1;

    // Touch #0 入力
    public void OnTouch0(InputAction.CallbackContext context)
    {
        _touchState0 = context.ReadValue<TouchState>();

        OnPinch();
    }

    // Touch #1 入力
    public void OnTouch1(InputAction.CallbackContext context)
    {
        _touchState1 = context.ReadValue<TouchState>();

        OnPinch();
    }

    // ピンチ判定処理
    private void OnPinch()
    {
        // ２本指が移動していなかれば操作なしと判断
        if (!_touchState0.isInProgress || !_touchState1.isInProgress) return;

        // タッチ位置（スクリーン座標）
        var pos0 = _touchState0.position;
        var pos1 = _touchState1.position;

        // 移動量（スクリーン座標）
        var delta0 = _touchState0.delta;
        var delta1 = _touchState1.delta;

        // 移動前の位置（スクリーン座標）
        var prevPos0 = pos0 - delta0;
        var prevPos1 = pos1 - delta1;

        // 距離の変化量を求める
        var pinchDelta = Vector3.Distance(pos0, pos1) - Vector3.Distance(prevPos0, prevPos1);

        // 距離の変化量をログ出力
        Debug.Log($"ピンチ操作量 : {pinchDelta}");
    }
}
