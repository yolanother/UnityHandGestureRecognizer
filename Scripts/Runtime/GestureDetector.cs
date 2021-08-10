using System;
using DoubTech.ValemGestures.BoneProviders;
using UnityEngine;
using UnityEngine.Events;

namespace DoubTech.ValemGestures
{
    public class GestureDetector : MonoBehaviour
    {
        [SerializeField] private GestureData gestureData;

        [Header("Bone Setup")]
        [SerializeField] private BoneProvider boneProvider;

        [Header("Detection")]
        [SerializeField] private float threshold = 0.05f;

        [Header("Events")]
        [SerializeField] private UnityEvent<string> onGestureStarted = new UnityEvent<string>();
        [SerializeField] private UnityEvent<string> onGestureStopped = new UnityEvent<string>();

        [Header("Debugging")]
        [SerializeField] private bool debug = false;
        [SerializeField] private KeyCode debugRecordKeycode = KeyCode.Space;
        [HideInInspector]
        [NonSerialized] public string gestureName;


        [HideInInspector]
        [NonSerialized]
        public static readonly Gesture noGesture = new Gesture()
        {
            name = "No Gesture Detected"
        };

        private Gesture currentGesture = noGesture;

        private void Update()
        {
            if (!boneProvider || !boneProvider.AreBonesReady) return;

            Gesture gesture = Recognize();
            if (gesture != currentGesture)
            {
                onGestureStopped?.Invoke(currentGesture.name);
                currentGesture.onGestureStopped?.Invoke();
                currentGesture = gesture;
                currentGesture.onGestureStarted?.Invoke();
                onGestureStarted?.Invoke(currentGesture.name);
            }

            if (debug && Input.GetKeyDown(debugRecordKeycode))
            {
                RecordGesture(gestureName);
            }
        }

        public void RecordGesture(string name)
        {
            if (!boneProvider.AreBonesReady)
            {
                Debug.LogError("Bones are neat ready to be recorded.");
                return;
            }
            gestureData.AddGesture(name, boneProvider);
        }

        public Gesture Recognize()
        {
            if (!boneProvider.AreBonesReady)
            {
                return null;
            }
            Gesture currentGesture = noGesture;
            var currentMin = Mathf.Infinity;
            for(int i = 0; i < gestureData.gestures.Count; i++)
            {
                var gesture = gestureData.gestures[i];
                for (int j = 0; j < gesture.trainingData.Count; j++)
                {
                    var data = gesture.trainingData[j].data;

                    var sumDistance = 0f;
                    var isDiscarded = false;
                    for (int k = 0; k < boneProvider.Bones.Length; k++)
                    {
                        var bone = boneProvider.Bones[k];
                        Vector3 currentData = boneProvider.BoneRoot.InverseTransformPoint(bone.position);
                        var distance = Vector3.Distance(data[k], currentData);

                        if (distance > threshold)
                        {
                            isDiscarded = true;
                            break;
                        }

                        sumDistance += distance;
                    }

                    if (!isDiscarded && sumDistance < currentMin)
                    {
                        currentGesture = gesture;
                    }
                }
            }

            return currentGesture;
        }
    }
}
