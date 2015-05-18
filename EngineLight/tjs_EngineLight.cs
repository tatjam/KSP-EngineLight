/*
 *          [KSP] Engine Light Mod 
 *          Made by Tatjam (Tajampi)  
 *                TajamSoft
 *--------------------------------------------------
 *
 * Notes:
 *      
 *  I'm implementing my own light, to make sure i don't break anything
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EngineLight
{
    

    

    public class tjs_EngineLight : PartModule
    {
        //Variables:

        public bool isDebug = true; //for now!

        [KSPField]
        public float lightPower = 1.0f;  //LightSource power (get's a porcentage based on thrust)
        
        //Not config-able until i know how to handle colors in config...

        public Color lightColor = new Color(1, 0.88f, 0.68f); //A light orange color

        
        //Thanks pizzaoverhead for this!: 

        public ModuleEngines _engineModule = null;
        public ModuleEngines engineModule
        {
            get
            {
                if (this._engineModule == null)
                    this._engineModule = (ModuleEngines)this.part.Modules["ModuleEngines"];
                return this._engineModule;
            }
        }

        public override void OnStart(PartModule.StartState state)
        {
            try
            {
                if (state == StartState.Editor || state == StartState.None) 
                { 
                    return; //Beware the bugs!
                }

                print("[EngineLight] Initialized part (" + this.part.partName + ") Proceeding to patch!");

                //Make lights: 

                GameObject engineLight = new GameObject();
                engineLight.AddComponent<Light>();

                //Light Settings:

                engineLight.light.type = LightType.Point;
                engineLight.light.range = 0.0f; //For now, changes later!
                engineLight.light.color = lightColor;
                engineLight.light.intensity = 0.0f; //By default!
                engineLight.light.enabled = false; //For now!

                //Transform Settings:

                engineLight.transform.parent = engineModule.light.transform;
                engineLight.transform.forward = engineModule.light.transform.forward; //not really required
                engineLight.transform.position = engineModule.light.transform.position;

                //Done!
            }
            catch(Exception exception)
            {
                Debug.LogError("[EngineLight] Error onStart: " + exception.Message);
            }
            
        }

        //Useful, checks for debug before printing, do not use if message is important
        public void print(object text)
        {
            if(isDebug)
            {
                Debug.Log(text);
            }
        }
        
    }
}
