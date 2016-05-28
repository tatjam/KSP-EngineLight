using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
/*
namespace EngineLight
{
    //This class contains utils for saving the config!
    //Thanks planetshine for the source code, this is based on it!
    
    //We are not yet going for 1.4, so all this is disabled!
    //REMEMBER TO RE-ENABLE IT!!
    

    //This thing makes sure we don't have multiple configs

        /*
    public sealed class Config
    {
        private static readonly Config instance = new Config();

        private Config() { }

        public static Config Instance
        {
            get
            {
                return instance;
            }
        }

        public bool isEnabled = true;
        public bool isDebug = false;
        public bool useVertex = true; //TODO: Implement
        public float lightMultiplier = 1.0f;
        public float rangeMultiplier = 1.0f;
        

    }

    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class ConfigManager : MonoBehaviour
    {
        public static ConfigManager instance { get; private set; }
        private Config config = Config.Instance;
        private ConfigNode configFile;
        private ConfigNode configFileNode;

        public static String pathToConfig = KSPUtil.ApplicationRootPath + "GameData/EngineLight/settings.cfg";

        public void Start()
        {
            if (Instance != null)
                Destroy(Instance.gameObject);
            Instance = this;

            LoadSettings();
        }

        public void LoadSettings()
        {
            configFile = KSP.IO.ConfigNode.Load(pathToConfig);
            configFileNode = configFile.GetNode("EngineLight");

            if (bool.Parse(configFileNode.GetValue("isDebug")))
            {
                config.isDebug = true;
            }
            else
            {
                config.isDebug = false;
            }
            if (bool.Parse(configFileNode.GetValue("isEnabled")))
            {
                config.isEnabled = true;
            }
            else
            {
                config.isEnabled = false;
            }

            //Main checking
            config.lightMultiplier = float.Parse(configFileNode.GetValue("lightMultiplier"));
            config.rangeMultiplier = float.Parse(configFileNode.GetValue("rangeMultiplier"));


        }

        public void SaveSettings()
        {
            configFileNode.SetValue("lightMultiplier", config.lightMultiplier.ToString());
            configFileNode.SetValue("rangeMultiplier", config.rangeMultiplier.ToString());
            if(config.isEnabled)
            {
                configFileNode.SetValue("isEnabled", "True");
            }
            else
            {
                configFileNode.SetValue("isEnabled", "False");
            }

            if(config.isDebug)
            {
                configFileNode.SetValue("isDebug", "True");
            }
            else
            {
                configFileNode.SetValue("isDebug", "False");
            }


            configFile.Save(KSPUtil.ApplicationRootPath + "GameData/EngineLight/settings.cfg");
        }

    }

    
}
*/