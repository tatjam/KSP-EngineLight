/*
 *          [KSP] Engine Light Mod 
 *          Made by Tatjam (Tajampi)  
 *                TajamSoft
 *         ---------------------------
 *               Contributors: 
 *                Saabstory88
 *            SpaceTiger (aka Xarun)
 *                
 *--------------------------------------------------
 *
 * Notes:
 *      
 *  I'm implementing my own light, to make sure i don't break anything
 * 
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;



namespace EngineLight
{



	public class tjs_EngineLight : PartModule
	{
		//Variables:
		[KSPField]
		public bool isDebug = true; //for now!

		[KSPField]
		public float lightPower = 1.0f;  //LightSource power (get's a porcentage based on thrust)

		[KSPField]
		public float maxLightPower = 27.0f; //LightSource clamped power

		[KSPField]
		public float lightRange = 15.0f;

		[KSPField]
		public float lightRed = 1.0f;

		[KSPField]
		public float lightGreen = 0.88f;

		[KSPField]
		public float lightBlue = 0.68f;

		[KSPField]
		public float jitterMultiplier = 10.5f; //Remember jitter is a Random value between 0 and 1, and we calculate thrust using percentage

		[KSPField]
		public float multiplierOnIva = 0.4f;

		//public GlobalManager gm;

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
					this._engineModule = part.FindModuleImplementing<ModuleEngines>();
				//SpaceTiger: No need for duplicate code. ModuleEnginesFX is a derivative of ModuleEngines so searching for ModuleEngines will get all the ModuleEnginesFX too
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

				float oldPow = lightPower;
				lightPower = (-0.0000004f * engineModule.GetMaxThrust() * engineModule.GetMaxThrust() + 0.0068f * 
					engineModule.GetMaxThrust() + 0.1304f) * oldPow; //Use the multiplier (1.1)

				if (lightPower > maxLightPower || engineModule.GetMaxThrust() > 5000)
				{
					lightPower = maxLightPower;
				}



				//Make lights: (Using part position)


				//NOTE: We use the thrust transform[0] to place the light

				Transform tmpVector = engineModule.thrustTransforms[0]; //At the first reactor (Sadly, only one! (Else it will lag, awaiting feedback!)


				GameObject TengineLight = new GameObject();
				TengineLight.AddComponent<Light>();

				//Light Settings:


				//Update color from confg
				colorInit();

				TengineLight.GetComponent<Light>().type = LightType.Point;
				TengineLight.GetComponent<Light>().range = lightRange; //For now, changes later!
				TengineLight.GetComponent<Light>().color = lightColor;
				TengineLight.GetComponent<Light>().intensity = 0.0f; //By default!
				TengineLight.GetComponent<Light>().enabled = false; //For now!

				//Transform Settings:
				TengineLight.transform.parent = tmpVector.transform;
				TengineLight.transform.forward = tmpVector.transform.forward; //not really required
				Vector3 TPos = tmpVector.transform.position;

				TengineLight.transform.position = new Vector3(TPos.x, TPos.y, TPos.z);
				TengineLight.transform.localPosition = new Vector3(TengineLight.transform.localPosition.x + engineModule.fxOffset.x, TengineLight.transform.localPosition.y + engineModule.fxOffset.y, TengineLight.transform.localPosition.z + engineModule.fxOffset.z);
				engineLight = TengineLight.GetComponent<Light>();


				print("[EngineLight] Light calculations (" + this.part.partName + ") resulted in:" + lightPower);
				print("X of engine: " + TengineLight.transform.position.x);

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

					if (!(Utils.isIVA() && multiplierOnIva == 0.0f))
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

						//Intensity = lightIntensity / 100 * thrust  (Percentage)
						//Calculate WORKING thrust percentage:
						if (engineModule.GetCurrentThrust() > 0)
						{
							float tmpRand = UnityEngine.Random.value * jitterMultiplier;  //Noisy Random, could use a Perlin Noise
							float tmpThrust = engineModule.GetCurrentThrust() / engineModule.GetMaxThrust() * 100 + tmpRand;
							if (tmpThrust < 0)  //Due to jitter, it might get under 0, if it happens, then make the number not calculated with jitter
							{
								tmpThrust = engineModule.GetCurrentThrust() / engineModule.GetMaxThrust() * 100;
							}
							engineLight.intensity = (lightPower / 100) * tmpThrust;
							engineLight.range = (lightRange / 100) * tmpThrust;

							if (Utils.isIVA() && multiplierOnIva < 1.0f)
							{
								engineLight.intensity = engineLight.intensity * multiplierOnIva;
							}
						}

					}
					else
					{
						//Disable all light stuff, perfomance!
						engineLight.intensity = 0;
						engineLight.range = 0;
						engineLight.enabled = false;
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


		//FXEngine module is not anymore there!

	}
}