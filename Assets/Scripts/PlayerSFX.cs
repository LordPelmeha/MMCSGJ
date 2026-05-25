using UnityEngine;

namespace HalfEmpty.Presentation.Player
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerSFX : MonoBehaviour
    {
        [Header("Footsteps")]
        [SerializeField] private AudioClip? _footstepClip;
        [SerializeField][Range(0f, 1f)] private float _footstepVolume = 0.5f;

        private AudioSource? _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
        }

        /// <summary>Called from Animation Event on run animation.</summary>
        public void PlayFootstep()
        {
            if (_audioSource == null || _footstepClip == null) return;
            _audioSource.PlayOneShot(_footstepClip, _footstepVolume);
        }
    }
}