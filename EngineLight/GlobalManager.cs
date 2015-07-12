using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EngineLight
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    class GlobalManager : MonoBehaviour
    {
        public List<LightConcentration> lightLocations;
        public List<tjs_EngineLight> eLights;
        public List<Transform> lights;

        public Vessel vs = FlightGlobals.ActiveVessel;

        public bool hasStartRun = false;


        public void RecalculateLights()
        {
            foreach(Transform l in lights)
            {
                GameObject.Destroy(l.gameObject);
            }

            //Generate the light cube (Make sure to separate it!):

            for(int y = 0; y < 3; y++)
            {
                for(int x = 0; x < 3; x++)
                {
                    for(int z = 0; z < 2; z++)
                    {
                        GameObject tmpLight = new GameObject();
                        tmpLight.AddComponent<Light>();
                        tmpLight.transform.parent = vs.transform;               
                        Instantiate(tmpLight, Vector3.zero, Quaternion.identity);
                        tmpLight.transform.localPosition = new Vector3(x * 33, y * 33, z * 50);  
                        lights.Add(tmpLight.transform);
                        tmpLight.transform.LookAt(vs.transform);

                        //DEBUG CUBE OF DEATH:
                        GameObject tmp = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        tmp.transform.parent = tmpLight.transform;
                        tmp.transform.localPosition = Vector3.zero;


                        //Actual light:

                        Light aLight = tmpLight.GetComponent<Light>();
                        aLight.type = LightType.Spot;
                        
                        //This angle might be horrible! On theory enough as to cover the vessl
                        aLight.spotAngle = 160;
                        aLight.range = Vector3.Distance(tmpLight.transform.position, vs.transform.position);
                        aLight.intensity = 1.0f;
                        aLight.enabled = true;
                    
                    }
                }
            }


        }

        public void UpdateLights()
        { 

        }

        public void Update()
        {
            UpdateLights();
        }

        public void OnFlightReady()
        {
            RecalculateLights();
        }


        public void OnVesselWasModified()
        {
            Debug.Log("Vessel was modified!");

            //Clean up any light that may be dead by now:
            foreach(tjs_EngineLight l in eLights)
            {
                if(l == null)
                {
                    eLights.Remove(l);
                }
            }

            RecalculateLights();

        }

    }
}
