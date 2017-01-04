﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnadvancedStabilizers
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    class VesselModuleRotationListener : MonoBehaviour
    {
        private bool vesselLoadOnSceneChange;

        private void Start()
        {
            GameEvents.onSetSpeedMode.Add(onSetSpeedMode);
            GameEvents.onVesselChange.Add(onVesselChange);
            vesselLoadOnSceneChange = true;
        }

        // Detect active vessel changed by switching to an unloaded vessel (flight scene was rebuilt)
        private void FixedUpdate()
        {
            if (vesselLoadOnSceneChange && FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.vesselModules.OfType<VesselModuleRotation>().Count() > 0)
            {
                FlightGlobals.ActiveVessel.vesselModules.OfType<VesselModuleRotation>().First().vesselSASHasChanged = true;
                vesselLoadOnSceneChange = false;
            }
        }

        // Detect active vessel change when switching vessel in the physics bubble
        private void onVesselChange(Vessel v)
        {
            v.vesselModules.OfType<VesselModuleRotation>().First().vesselSASHasChanged = true;
        }

        // Detect navball context (orbit/surface/target) changes
        private void onSetSpeedMode(FlightGlobals.SpeedDisplayModes mode)
        {
            FlightGlobals.ActiveVessel.vesselModules.OfType<VesselModuleRotation>().First().autopilotContextCurrent = (int)mode;
        }

        private void OnDestroy()
        {
            GameEvents.onSetSpeedMode.Remove(onSetSpeedMode);
            GameEvents.onVesselChange.Remove(onVesselChange);
        }

    }
}
