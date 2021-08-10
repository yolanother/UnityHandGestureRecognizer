using System;
using TMPro;
using UnityEngine;

namespace DoubTech.ValemGestures
{
    public class TriggerGestureRecorder : MonoBehaviour
    {
        [SerializeField] private GestureDetector detector;
        [SerializeField] private TMP_InputField text;

        private void OnTriggerEnter(Collider other)
        {
            if (!string.IsNullOrEmpty(text.text))
            {
                detector.RecordGesture(text.text);
            }
        }
    }
}
