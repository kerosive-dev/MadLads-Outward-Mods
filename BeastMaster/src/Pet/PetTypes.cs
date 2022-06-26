using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;

namespace BeastMaster
{
    [System.Serializable]
    public class PetType
    {
        public string PetTypeName;
        public string SLPackName;
        public string AssetBundleName;
        public string PrefabName;

        //movement
        public float MoveSpeed;
        //used by nav mesh agent
        public float Acceleration;
        public float RotateSpeed;
    
        public List<string> Names = new List<string>();

        public string GetRandomName()
        {
            return Names[UnityEngine.Random.Range(0, Names.Count)];
        }
    }
}