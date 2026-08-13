using UnityEngine;

public sealed class CameraLimit : MonoBehaviour
{
    [SerializeField]
    private bool _isLeftLimit;
    
    [SerializeField]
    private float _leftLimit;
    
    [SerializeField]
    private bool _isDownLimit;
    
    [SerializeField]
    private float _downLimit;

    [SerializeField]
    private bool _isRightLimit;
    
    [SerializeField]
    public float _rightLimit;
    
    [SerializeField]
    private bool _isUpLimit;
    
    [SerializeField]
    private float _upLimit;
    
    private void OnEnable()
    {
        LimitUpgrade();
    }
    
    private void Update()
    {
        LimitUpgrade();
    }
    
    private void LimitUpgrade()
    {
        OverworldCamera.Instance.IsDownLimit = _isDownLimit;
        OverworldCamera.Instance.DownLimit = _downLimit;
        
        OverworldCamera.Instance.IsLeftLimit = _isLeftLimit;
        OverworldCamera.Instance.LeftLimit = _leftLimit;
        
        OverworldCamera.Instance.IsRightLimit = _isRightLimit;
        OverworldCamera.Instance.RightLimit = _rightLimit;
        
        OverworldCamera.Instance.IsUpLimit = _isUpLimit;
        OverworldCamera.Instance.UpLimit = _upLimit;
    }
}
