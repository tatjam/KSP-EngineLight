using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EngineLight
{
    class tjs_DecouplerLight : PartModule
    {
        [KSPField]
        public float lightMultiplier = 1.0f;

        [KSPField]
        public float lightRange = 15.0f;

        [KSPField]
        public float lightRed = 1.0f;

        [KSPField]
        public float lightGreen = 1.0f;

        [KSPField]
        public float lightBlue = 1.0f;

        [KSPField]
        public float lightDuration = 0.1f;

        public ModuleDecouple _decouplerModule = null;
        public ModuleDecouple decouplerModule
        
        {
            get
            {
                if (this._decouplerModule == null)
                    this._decouplerModule = part.FindModuleImplementing<ModuleDecouple>();
                return this._decouplerModule;
            }
        }

        public ModuleAnchoredDecoupler _decouplerModuleA = null;
        public ModuleAnchoredDecoupler decouplerModuleA
        {
            get
            {
                if (this._decouplerModuleA == null)
                    this._decouplerModuleA = part.FindModuleImplementing<ModuleAnchoredDecoupler>();
                return this._decouplerModuleA;
            }
        }


        public Light decouplerLight;

        public bool wasDecouplerActive = false;


        public override void OnStart(StartState state)
        {
            try
            {
                if (state == StartState.Editor || state == StartState.None)
                {
                    return; //Beware the bugs!
                }

                GameObject decouplerLightGM = new GameObject();
                decouplerLightGM.AddComponent<Light>();


                if (decouplerModule)
                {
                    decouplerLight = new Light();
                    decouplerLightGM.transform.position = decouplerModule.transform.position;
                    decouplerLightGM.transform.parent = decouplerModule.transform;

                    decouplerLightGM.GetComponent<Light>().range = lightRange;

                    decouplerLightGM.GetComponent<Light>().intensity = decouplerModule.ejectionForce / 30 * lightMultiplier; //The huge decoupler thing generates a light of 25

                    decouplerLightGM.GetComponent<Light>().color = new Color(lightRed, lightGreen, lightBlue);

                    decouplerLight = decouplerLightGM.GetComponent<Light>();

                    decouplerLight.enabled = false;
                }
                else
                {
                    decouplerLight = new Light();
                    decouplerLightGM.transform.position = decouplerModuleA.transform.position;
                    decouplerLightGM.transform.parent = decouplerModuleA.transform;

                    decouplerLightGM.GetComponent<Light>().range = lightRange;

                    decouplerLightGM.GetComponent<Light>().intensity = decouplerModuleA.ejectionForce / 30; //The huge decoupler thing generates a light of 25

                    decouplerLightGM.GetComponent<Light>().color = new Color(lightRed, lightGreen, lightBlue);

                    decouplerLight = decouplerLightGM.GetComponent<Light>();

                    decouplerLight.enabled = false;
                }

            }
            catch(Exception e)
            {
                Debug.LogError("[DecouplerLight] Error onStart: " + e.Message);
            }
        }




        public override void OnUpdate()
        {
            try
            {
                if(!wasDecouplerActive)
                {
                    if (decouplerModule)
                    {
                        if (decouplerModule.isDecoupled)
                        {
                            wasDecouplerActive = true;
                            decouplerLight.enabled = true;
                            StartCoroutine("shutLightDown");
                        }
                    }
                    else
                    {
                        if (decouplerModuleA.isDecoupled)
                        {
                            wasDecouplerActive = true;
                            decouplerLight.enabled = true;
                            StartCoroutine("shutLightDown");
                        }
                    }
                    

                }
            }
            catch(Exception e)
            {
                Debug.LogError("[DecouplerLight] Error onUpdate: " + e.Message);
            }
        }

        //Coroutines are really easy in C#/Unity

        IEnumerator shutLightDown()
        {
            yield return new WaitForSeconds(lightDuration);
            decouplerLight.enabled = false;
        }

    }
}
