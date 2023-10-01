using UnityEngine;

public class LookCamera : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector2 _sensitivity = new (1, 1);
    [SerializeField] private Vector2 _yClamp = new (-60, 60);
    
    [SerializeField] private bool _smooth;
    [SerializeField] private float _interpolationSpeed = 25.0f;
    
    private Rigidbody _playerCharacterRigidbody;
    private PlayerBehaviour _playerCharacter;
    
    private Quaternion _rotationCharacter;
    private Quaternion _rotationCamera;

    public void Init(PlayerBehaviour playerBehaviour, Rigidbody playerRigidbody)
    {
        _playerCharacter = playerBehaviour;
        _playerCharacterRigidbody = playerRigidbody;
        
        _rotationCharacter = _playerCharacter.transform.localRotation;
        _rotationCamera = transform.localRotation;
    }

    private void LateUpdate()
    {
        Vector2 frameInput = _playerCharacter.IsCursorLocked() ? _playerCharacter.GetInputLook() : default;
        frameInput *= _sensitivity;

        Quaternion rotationYaw = Quaternion.Euler(0.0f, frameInput.x, 0.0f);
        Quaternion rotationPitch = Quaternion.Euler(-frameInput.y, 0.0f, 0.0f);

        _rotationCamera *= rotationPitch;
        _rotationCharacter *= rotationYaw;

        Quaternion localRotation = transform.localRotation;

        if (_smooth)
        {
            localRotation = Quaternion.Slerp(localRotation, _rotationCamera, Time.deltaTime * _interpolationSpeed);
            _playerCharacterRigidbody.MoveRotation(Quaternion.Slerp(_playerCharacterRigidbody.rotation,
                _rotationCharacter, Time.deltaTime * _interpolationSpeed));
        }
        else
        {
            localRotation *= rotationPitch;
            localRotation = Clamp(localRotation);

            _playerCharacterRigidbody.MoveRotation(_playerCharacterRigidbody.rotation * rotationYaw);
        }

        transform.localRotation = localRotation;
    }

    private Quaternion Clamp(Quaternion rotation)
    {
        rotation.x /= rotation.w;
        rotation.y /= rotation.w;
        rotation.z /= rotation.w;
        rotation.w = 1.0f;

        float pitch = 2.0f * Mathf.Rad2Deg * Mathf.Atan(rotation.x);

        pitch = Mathf.Clamp(pitch, _yClamp.x, _yClamp.y);
        rotation.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * pitch);

        return rotation;
    }
}