using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Alarm : MonoBehaviour
{
    [SerializeField] private float _volumeChangeStep;
    [SerializeField] private float _delay;
    [SerializeField] private float _maxVolume = 1;
    [SerializeField] private float _minVolume = 0;

    private AudioSource _audio;
    private bool _isAlarmOn;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Thief thief))
        {
            _isAlarmOn = true;
            _audio.volume = _minVolume;
            _audio.Play();
            StartCoroutine(AlarmVolumeChanger());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Thief thief))
        {
            _isAlarmOn = false;
            _audio.Stop();
            StopCoroutine(AlarmVolumeChanger());
        }
    }

    private IEnumerator AlarmVolumeChanger()
    {
        if (_audio.volume >= _maxVolume)
            while (_audio.volume > _minVolume)
            {
                _audio.volume -= _volumeChangeStep;
                yield return new WaitForSeconds(_delay);
            }

        if (_audio.volume <= _minVolume)
            while (_audio.volume < _maxVolume)
            {
                _audio.volume += _volumeChangeStep;
                yield return new WaitForSeconds(_delay);
            }

        if (_isAlarmOn)
            StartCoroutine(AlarmVolumeChanger());
    }
}
