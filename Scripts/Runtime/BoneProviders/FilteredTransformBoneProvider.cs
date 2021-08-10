using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace DoubTech.ValemGestures.BoneProviders
{
    public class FilteredTransformBoneProvider : BoneProvider
    {
        [SerializeField] public string[] filters;
        [SerializeField] private float boneRecheckTime = 1;
        [SerializeField] private Transform boneRoot;

        private Transform[] bones;

        private float lastBoneCheck;

        public override Transform BoneRoot => boneRoot;

        public override Transform[] Bones
        {
            get
            {
                if (boneRoot && boneRoot.transform.childCount > 0 && (null == bones || bones.Length == 0 && boneRecheckTime > 0 && Time.time - lastBoneCheck > boneRecheckTime))
                {
                    lastBoneCheck = Time.time;
                    List<Transform> boneList = new List<Transform>();
                    FindBone(boneList, boneRoot);
                    bones = boneList.ToArray();
                    Debug.Log($"Found {bones.Length} bones.");
                }

                return bones;
            }
        }

        public override bool AreBonesReady => boneRoot && null != Bones;

        private void FindBone(List<Transform> bones, Transform parent)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                var child = parent.transform.GetChild(i);
                foreach (var filter in filters)
                {
                    if (child.name.ToLower().Contains(filter.ToLower()))
                    {
                        bones.Add(child);
                    }
                }

                FindBone(bones, child);
            }
        }

        public void SetRootBone(Transform transform)
        {
            bones = null;
            boneRoot = transform;
        }
    }
}
