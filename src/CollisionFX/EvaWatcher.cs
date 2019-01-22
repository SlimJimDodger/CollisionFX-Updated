using System;
using UnityEngine;

namespace CollisionFX
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    class EvaWatcher : MonoBehaviour
    {
        //private string _configPath = "GameData/CollisionFX/settings.cfg";

        bool _scrapeSparks;
        string _collisionSound;
        string _scrapeSound;
        string _sparkSound;

        public void Start()
        {
            GameEvents.onCrewOnEva.Add(OnCrewEVA);
            ConfigNode config = ConfigNode.Load(CollisionFX.ConfigPath);
            if (config == null)
            {
                Debug.LogError("[CollisionFX] Configuration file not found at " + CollisionFX.ConfigPath);
                return;
            }
            foreach (ConfigNode node in config.nodes)
            {
                if (node.name.Equals("KerbalEVA"))
                {
                    if (node.HasValue("scrapeSparks"))
                        _scrapeSparks = bool.Parse(node.GetValue("scrapeSparks"));
                    if (node.HasValue("collisionSound"))
                        _collisionSound = node.GetValue("collisionSound");
                    if (node.HasValue("scrapeSound"))
                        _scrapeSound = node.GetValue("scrapeSound");
                    if (node.HasValue("sparkSound"))
                        _sparkSound = node.GetValue("sparkSound");
                }
            }
        }

        public void OnCrewEVA(GameEvents.FromToAction<Part, Part> action)
        {
            if (action.to.Modules["KerbalEVA"] != null)
            {
                CollisionFX cfx = action.to.AddModule("CollisionFX") as CollisionFX;
                cfx.scrapeSparks = _scrapeSparks;
                cfx.collisionSound = _collisionSound;
                cfx.scrapeSound = _scrapeSound;
                cfx.sparkSound = _sparkSound;
            }
        }

        public void Destroy()
        {
            GameEvents.onCrewOnEva.Remove(OnCrewEVA);
        }
    }
}
