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

        [KSPField]
        public float lightRed = 1.0f;
        
        [KSPField]
        public float lightGreen = 0.88f;
        
        [KSPField]
        public float lightBlue = 0.68f;
        
        //Changes with thrust
        
        //Not config-able until i know how to handle colors in config...
        
        public Color lightColor;
        
        void colorInit()
        {
            
            lightColor = new Color(lightRed, lightGreen, lightBlue); //A light orange color
        }

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

                //Generate light power:
                // (Thanks Excel!!) It's an almost perfect cuadratic function!

                lightPower = -0.000001f * engineModule.maxThrust * engineModule.maxThrust + 0.0126f * engineModule.maxThrust + 1.6074f;

                if (lightPower > 30f || engineModule.maxThrust > 5000)
                {
                    lightPower = 30f;
                }
                //Make lights: (Using part position)


                //NOTE: We use the center part position, which may, or may not look bad

                Transform tmpVector = engineModule.thrustTransforms[0]; //At the first reactor (Sadly, only one! (Else it will lag, awaiting feedback!)



                GameObject TengineLight = new GameObject();
                TengineLight.AddComponent<Light>();

                //Light Settings:


                //Update color from confg
                colorInit();

                TengineLight.light.type = LightType.Point;
                TengineLight.light.range = lightRange; //For now, changes later!
                TengineLight.light.color = lightColor;
                TengineLight.light.intensity = 0.0f; //By default!
                TengineLight.light.enabled = false; //For now!

                //Transform Settings:

                TengineLight.transform.parent = tmpVector.transform;
                TengineLight.transform.forward = tmpVector.transform.forward; //not really required
                Vector3 TPos = tmpVector.transform.position;

                TengineLight.transform.position = new Vector3(TPos.x, TPos.y - TengineLight.transform.forward.y - 0.3f, TPos.z);

                engineLight = TengineLight.light;

                //Done!
            }

            catch (Exception exception)
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
                    if (engineModule.finalThrust > 0)
                    {
                        engineLight.enabled = true;
                    }
                    else
                    {
                        engineLight.enabled = false;
                    }

                    //Update light status:

                    //Intensity = lightIntensity / 100 * thrust  (Porcentage)
                    //Calculate WORKING thrust percentage:
                    if (engineModule.resultingThrust > 0)
                    {
                        float tmpThrust = engineModule.resultingThrust / engineModule.maxThrust * 100;
                        engineLight.intensity = (lightPower / 100) * tmpThrust;
                        engineLight.range = (lightRange / 100) * tmpThrust;
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.LogError("[EngineLight] Error onUpdate: " + ex.Message);
            }
        }
        //Useful, checks for debug before printing, do not use if message is important
        public void print(object text)
        {
            if (isDebug)
            {
                Debug.Log(text);
            }
        }


        //FXENGINE MODULE: (Why SQUAD, why...)
        //This module is for engines using EnginesFx
        
        public class tjs_EngineLightFX : PartModule
        {
            //Variables:

            public bool isDebug = true; //for now!

            [KSPField]
            public float lightPower = 1.0f;  //LightSource power (get's a porcentage based on thrust)

            [KSPField]
            public float lightRange = 15.0f;
            
            [KSPField]
            public float lightRed = 1.0f;
            
            [KSPField]
            public float lightGreen = 0.88f;
            
            [KSPField]
            public float lightBlue = 0.68f;
            
            //Changes with thrust

            //Not config-able until i know how to handle colors in config...

            public Color lightColor;

            void colorInit()
            {

                lightColor = new Color(lightRed, lightGreen, lightBlue); //A light orange color
            }

            public Light engineLight = null; //On minus!

            //Thanks pizzaoverhead for this!: 

            public ModuleEnginesFX _engineModule = null;
            public ModuleEnginesFX engineModule
            {
                get
                {
                    if (this._engineModule == null)
                        this._engineModule = (ModuleEnginesFX)this.part.Modules["ModuleEnginesFX"];
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

                    //Generate light power:
                    // (Thanks Excel!!) It's an almost perfect cuadratic function!

                    lightPower = -0.000001f * engineModule.maxThrust * engineModule.maxThrust + 0.0126f * engineModule.maxThrust + 1.6074f;

                    if (lightPower > 30f || engineModule.maxThrust > 5000)
                    {
                        lightPower = 30f;
                    }
                    //Make lights: (Using part position)


                    //NOTE: We use the center part position, which may, or may not look bad

                    Transform tmpVector = engineModule.thrustTransforms[0]; //At the first reactor (Sadly, only one! (Else it will lag, awaiting feedback!)



                    GameObject TengineLight = new GameObject();
                    TengineLight.AddComponent<Light>();

                    //Light Settings


                    //Update color from confg

                    colorInit();

                    TengineLight.light.type = LightType.Point;
                    TengineLight.light.range = lightRange; //For now, changes later!
                    TengineLight.light.color = lightColor;
                    TengineLight.light.intensity = 0.0f; //By default!
                    TengineLight.light.enabled = false; //For now!

                    //Transform Settings:

                    TengineLight.transform.parent = tmpVector.transform;
                    TengineLight.transform.forward = tmpVector.transform.forward; //not really required
                    Vector3 TPos = tmpVector.transform.position;

                    TengineLight.transform.position = new Vector3(TPos.x, TPos.y - TengineLight.transform.forward.y - 0.3f, TPos.z);

                    engineLight = TengineLight.light;

                    //Done!
                }

                catch (Exception exception)
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
                        if (engineModule.finalThrust > 0)
                        {
                            engineLight.enabled = true;
                        }
                        else
                        {
                            engineLight.enabled = false;
                        }

                        //Update light status:

                        //Intensity = lightIntensity / 100 * thrust  (Porcentage)
                        //Calculate WORKING thrust percentage:
                        if (engineModule.resultingThrust > 0)
                        {
                            float tmpThrust = engineModule.resultingThrust / engineModule.maxThrust * 100;
                            engineLight.intensity = (lightPower / 100) * tmpThrust;
                            engineLight.range = (lightRange / 100) * tmpThrust;
                        }

                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("[EngineLight] Error onUpdate: " + ex.Message);
                }
            }
            //Useful, checks for debug before printing, do not use if message is important
            public void print(object text)
            {
                if (isDebug)
                {
                    Debug.Log(text);
                }
            }

        }
    }
}