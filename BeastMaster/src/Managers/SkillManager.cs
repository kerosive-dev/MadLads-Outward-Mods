using UnityEngine;

namespace BeastMaster
{
    internal class SkillManager
    {
        public static void Init()
        {
            SetupSkills();
        }
        private static void SetupSkills()
        {
            // I use the SideLoader to do the preliminary setup (clone existing skill, change ID / name / etc..)
            SetupSummonPet();
        }

        public static void SetupSummonPet()
        {
            var summon = ResourcesPrefabManager.Instance.GetItemPrefab(-27999) as Skill;

            // destroy the existing skills, but keep the rest (VFX / Sound).
            GameObject.DestroyImmediate(summon.transform.Find("Lightning").gameObject);
            GameObject.DestroyImmediate(summon.transform.Find("SummonSoul").gameObject);

            var effects = new GameObject("Effects");
            effects.transform.parent = summon.transform;
            effects.AddComponent<SummonPet>();

        }
    }
}
