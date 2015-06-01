using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EngineLight
{
    /// <summary>
    /// This class manages lights globally, and implements the GUI
    /// </summary>
    /// 
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    class EngineLightManager : MonoBehaviour
    {

        float lightMultiplier = 1.0f;
        float rangeMultiplier = 1.0f;

        void Start()
        {
            try
            {

                //Makes sure the plugin is not running before physics(?) load
                if (!FlightGlobals.ready) 
                {
                    return;
                }

                Debug.Log("[EngineLightManager] Up and running!");
                
            }
            catch(Exception ex)
            {
                Debug.LogError("[EngineLightManager] Error on Start: " + ex.Message);
            }
        }
    
        void Update()
        {
            try
            {

            }
            catch(Exception ex)
            {
                Debug.LogError("[EngineLightManager] Error on Update: " + ex.Message);
            }
        }


        //Simple functions for easy access:

        public float getLightMultiplier()
        {
            return lightMultiplier;
        }

        public float getRangeMultiplier()
        {
            return rangeMultiplier;
        }


    }
}
