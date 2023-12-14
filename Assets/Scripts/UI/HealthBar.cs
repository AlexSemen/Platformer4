using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(IHealthChanged))]
public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _sliderSmooth;

    private IHealthChanged _unit;
    private Coroutine _fadeInJob;
    private float _stepChangingVolume;
    private float _timeOneStepChangingVolume;
    private WaitForSeconds _waitForDelay;

    private void Awake()
    {
        _unit = GetComponent<IHealthChanged>();
        _stepChangingVolume = 0.01f;
        _timeOneStepChangingVolume = 0.02f;
        _waitForDelay = new WaitForSeconds(_timeOneStepChangingVolume);
    }

    private void OnEnable()
    {
        _unit.HealthChanged += HealthChanged;
    }

    private void OnDisable()
    {
        _unit.HealthChanged -= HealthChanged;
    }

    private void HealthChanged(int value, int maxValue)
    {
        ChangedSliderSmooth(value, maxValue);
    }

    private void ChangedSliderSmooth(int value, int maxValue)
    {
        if (_fadeInJob != null)
        {
            StopCoroutine(_fadeInJob);
        }

        _fadeInJob = StartCoroutine(ChangingVolume(value, maxValue));
    }

    private IEnumerator ChangingVolume(int value, int maxValue)
    {
        _sliderSmooth.maxValue = maxValue;

        while (_sliderSmooth.value != value)
        {
            if(_sliderSmooth.value > value)
            {
                _sliderSmooth.value -= _stepChangingVolume;
            }
            else
            {
                _sliderSmooth.value += _stepChangingVolume;
            }

            yield return _waitForDelay;
        }
    }
}

     
