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

        [KSPField]
        public float lightRange = 15.0f; //Changes with thrust

        //Not config-able until i know how to handle colors in config...

        public Color lightColor = new Color(1, 0.88f, 0.68f); //A light orange color

        public Light engineLight = null; //On minus!

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

                GameObject TengineLight = new GameObject();
                TengineLight.AddComponent<Light>();

                //Light Settings:

                TengineLight.light.type = LightType.Point;
                TengineLight.light.range = lightRange; //For now, changes later!
                TengineLight.light.color = lightColor;
                TengineLight.light.intensity = 0.0f; //By default!
                TengineLight.light.enabled = false; //For now!

                //Transform Settings:

                TengineLight.transform.parent = engineModule.light.transform;
                TengineLight.transform.forward = engineModule.light.transform.forward; //not really required
                TengineLight.transform.position = engineModule.light.transform.position;

                engineLight = TengineLight.light;
                
                //Done!
            }
            catch(Exception exception)
            {
                Debug.LogError("[EngineLight] Error onStart: " + exception.Message);
            }
            
        }


        public override void OnUpdate()
        {
            try
            {


                if (engineLight != null)  //Make sure the light exists!
                {

                    //Check for engine activity:
                    if(engineModule.isActiveAndEnabled == true && engineLight.enabled == false)
                    {
                        engineLight.enabled = true;
                    }
                    else
                    {
                        if(engineLight.enabled == true) //Not sure if this is required...
                        {
                            engineLight.enabled = false;
                        }
                    }

                    //Update light status:
                    if (engineLight.enabled == true)
                    {
                        //Intensity = lightIntensity / 100 * thrust  (Porcentage)
                        
                            engineLight.intensity = (lightPower / 100) * engineModule.thrustPercentage;
                            engineLight.range = (lightRange / 100) * engineModule.thrustPercentage;

                    }
                    
                }
            }
            catch(Exception ex)
            {
                Debug.LogError("[EngineLight] Error onUpdate: " + ex.Message);
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
