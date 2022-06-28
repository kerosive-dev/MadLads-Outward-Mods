using UnityEngine;

namespace BeastMaster
{
    public class SkillManager
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

        #region SummonPet
        public static void SetupSummonPet()
        {
            var summonPet = ResourcesPrefabManager.Instance.GetItemPrefab(-27999) as Skill;

            // destroy the existing skills, but keep the rest (VFX / Sound).
            GameObject.DestroyImmediate(summonPet.transform.Find("Lightning").gameObject);
            GameObject.DestroyImmediate(summonPet.transform.Find("SummonSoul").gameObject);

            var effects = new GameObject("Effects");
            effects.transform.parent = summonPet.transform;
            effects.AddComponent<SummonPet>();
        }
        #endregion
    }
}
