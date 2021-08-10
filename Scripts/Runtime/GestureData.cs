using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DoubTech.ValemGestures.BoneProviders;
using UnityEngine;
using UnityEngine.Events;

namespace DoubTech.ValemGestures
{
    [CreateAssetMenu(fileName = "GestureData", menuName = "DoubTech/Hand Tracking/Gesture Data")]
    public class GestureData : ScriptableObject
    {
        [SerializeField] public List<Gesture> gestures = new List<Gesture>();
        private Dictionary<string, Gesture> gestureMap;

        public Dictionary<string, Gesture> Gestures
        {
            get
            {
                if (null == gestureMap)
                {
                    gestureMap = new Dictionary<string, Gesture>();
                    foreach (var gesture in gestures)
                    {
                        gestureMap[gesture.name] = gesture;
                    }
                }

                return gestureMap;
            }
        }

        public void AddGesture(string name, IBoneProvider boneProvider)
        {
            if (!Gestures.TryGetValue(name, out var gesture))
            {
                gesture = new Gesture()
                {
                    name = name
                };
                gestures.Add(gesture);
                Gestures[name] = gesture;
            }

            gesture.RecordGesture(boneProvider);
        }

        public void Save()
        {
            var path = Application.persistentDataPath + $"/gesture-data.{name}.data";
            using (var stream = File.Create(path))
            {
                BinaryFormatter writer = new BinaryFormatter();
                writer.Serialize(stream, gestures);
            }
        }

        public void Load()
        {
            var path = Application.persistentDataPath + $"/gesture-data.{name}.data";
            if (File.Exists(path))
            {
                using (var stream = File.Open(path, FileMode.Open))
                {
                    BinaryFormatter writer = new BinaryFormatter();
                    gestures = (List<Gesture>) writer.Deserialize(stream);
                }
            }
        }
    }

    [Serializable]
    public class Gesture
    {
        public string name;
        public List<TrainingData> trainingData = new List<TrainingData>();
        public UnityEvent onGestureStarted;
        public UnityEvent onGestureStopped;

        public void RecordGesture(IBoneProvider boneProvider)
        {
            var data = new List<Vector3>();
            foreach (var bone in boneProvider.Bones)
            {
                data.Add(boneProvider.BoneRoot.InverseTransformPoint(bone.position));
            }
            trainingData.Add(new TrainingData()
            {
                data = data.ToArray()
            });
        }
    }

    [Serializable]
    public struct TrainingData
    {
        public Vector3[] data;
    }
}
