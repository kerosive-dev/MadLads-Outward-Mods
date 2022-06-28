using SideLoader;
using System;

namespace BeastMaster
{
    public class CustomVisuals
    {
        public class SL_CustomVisuals : SL_Effect, ICustomModel
        {
            public virtual Type SLTemplateModel => typeof(SL_CustomVisuals);
            public virtual Type GameModel => typeof(SL_CustomVisuals);

            public string SLPackName;
            public string AssetBundleName;
            public string PrefabName;

            public override void ApplyToComponent<T>(T component)
            {
                SL_CustomVisuals comp = component as SL_CustomVisuals;
                comp.SLPackName = SLPackName;
                comp.AssetBundleName = AssetBundleName;
                comp.PrefabName = PrefabName;
            }

            public override void SerializeEffect<T>(T effect)
            {
                SL_CustomVisuals eff = effect as SL_CustomVisuals;
                this.SLPackName = eff.SLPackName;
                this.AssetBundleName = eff.AssetBundleName;
                this.PrefabName = eff.PrefabName;
            }
        }

        public class SLEx_CustomVisuals : Effect, ICustomModel
        {
            public virtual Type SLTemplateModel => typeof(SL_CustomVisuals);
            public Type GameModel => typeof(SLEx_CustomVisuals);

            public string SLPackName;
            public string AssetBundleName;
            public string PrefabName;

            public CharacterVisuals customVisuals;

            public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
            {
                customVisuals = _affectedCharacter.GetComponent<CharacterVisuals>();
                CharacterVisuals Prefab = OutwardHelpers.GetFromAssetBundle<CharacterVisuals>(SLPackName, AssetBundleName, PrefabName);
                customVisuals = Prefab;
            }
        }
    }
}
