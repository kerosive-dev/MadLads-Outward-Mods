using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using UnityEngine;
using SideLoader;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeastMaster
{
    public class SL_SpawnPet : SL_SummonAI, ICustomModel
    {
        public virtual Type SLTemplateModel => typeof(SL_SpawnPet);
        public virtual Type GameModel => typeof(SpawnPet);

        public string SLPackName;
        public string AssetBundleName;
        public string PrefabName;
        public Vector3 PositionOffset;
        public Vector3 RotationOffset;

        public override void ApplyToComponent<T>(T component)
        {
            SpawnPet comp = component as SpawnPet;
            comp.SLPackName = SLPackName;
            comp.AssetBundleName = AssetBundleName;
            comp.PrefabName = PrefabName;
            comp.PositionOffset = PositionOffset;
            comp.RotationOffset = RotationOffset;
        }

        //TODO : Clarify
        public override void SerializeEffect<T>(T effect)
        {
            SpawnPet eff = effect as SpawnPet;
            this.SLPackName = eff.SLPackName;
            this.AssetBundleName = eff.AssetBundleName;
            this.PrefabName = eff.PrefabName;
            this.PositionOffset = eff.PositionOffset;
            this.RotationOffset = eff.RotationOffset;
        }
    }

    public class SpawnPet : Effect, ICustomModel
    {
        public virtual Type SLTemplateModel => typeof(SL_SpawnPet);
        public virtual Type GameModel => typeof(SpawnPet);

        public string SLPackName;
        public string AssetBundleName;
        public string PrefabName;
        public Vector3 PositionOffset;
        public Vector3 RotationOffset;

        private GameObject Instance;

        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            GameObject Prefab = GetFromAssetBundle<GameObject>(SLPackName, AssetBundleName, PrefabName);

            if (Prefab != null)
            {
                if (Instance == null)
                {
                    //spawn it
                    Instance = GameObject.Instantiate(Prefab);
                }

                //position it near player
                Instance.transform.position = _affectedCharacter.transform.position + PositionOffset;
                Instance.transform.eulerAngles = RotationOffset;

                //reference a static manger of some kind to register the instance
            }
            else
            {
                //ExtendedEffects.Log($"SLEx_PlayAssetBundleVFX Prefab from AssetBundle {AssetBundleName} was null.");
            }
        }
        /*///might not want to delete it at all so remove if so
        public override void CleanUpOnDestroy()
        {
            base.CleanUpOnDestroy();

            if (Instance)
            {
                GameObject.Destroy(Instance);
            }

        }

        ///might not want to delete it at all so remove if so
        public override void StopAffectLocally(Character _affectedCharacter)
        {
            if (Instance)
            {
                GameObject.Destroy(Instance);
            }
        }*/

        public T GetFromAssetBundle<T>(string SLPackName, string AssetBundle, string key) where T : UnityEngine.Object
        {
            if (!SL.PacksLoaded)
            {
                return default(T);
            }

            return SL.GetSLPack(SLPackName).AssetBundles[AssetBundle].LoadAsset<T>(key);
        }
    }
}
