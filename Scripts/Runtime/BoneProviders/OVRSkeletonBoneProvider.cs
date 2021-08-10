using System.Linq;
using UnityEngine;


namespace DoubTech.ValemGestures.BoneProviders
{
    public class OVRSkeletonBoneProvider : BoneProvider
    {
        [SerializeField] private OVRSkeleton skeleton;
        private Transform[] bones;

        public override Transform BoneRoot => skeleton.transform;

        public override Transform[] Bones
        {
            get
            {
                if (null == bones)
                {
                    bones = skeleton.Bones.Select(v => v.Transform).ToArray();
                }

                return bones;
            }
        }

        public override bool AreBonesReady => skeleton && skeleton.Bones.Count > 0;
    }
}
