using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace CollisionFXUpdated
{
    public static class ColourManager
    {
        // TODO: Add support for 0 - 255 range colours rather than 0 - 1.
        public class BodyDust
        {
            public string Name;
            public List<BiomeDust> Biomes;
            public List<StructureDust> Structures;
            public Color DefaultBiomeColour = Color.clear;
            public override string ToString() { return Name; }
        }

        public class BiomeDust
        {
            public string Name;
            public Color DustColour;
            public override string ToString() { return Name; }
        }

        public class StructureDust
        {
            public string Name;
            public Color DustColour;
            public override string ToString() { return Name; }
        }

        private static List<BodyDust> _bodies = null;

        public static void LoadDustColours()
        {

            //c.collider.name = "runway_lev1"


            _bodies = new List<BodyDust>();
            ConfigNode config = ConfigNode.Load(CollisionFX.ConfigPath);
            if (config == null)
            {
                Debug.LogError("[CollisionFX] Configuration file not found at " + CollisionFX.ConfigPath);
                return;
            }
            foreach (ConfigNode node in config.nodes)
            {
                if (!node.name.Equals("BodyDust"))
                    continue;

                // Load body
                BodyDust body;
                if (node.HasValue("name"))
                {
                    body = new BodyDust
                    {
                        Name = node.GetValue("name"),
                        Biomes = new List<BiomeDust>(),
                        Structures = new List<StructureDust>()
                    };
                }
                else
                {
                    Debug.LogWarning("[CollisionFX] Invalid BodyDust definition: \"name\" field is missing.");
                    continue;
                }

                // Load biomes
                if (node.HasNode("Biomes"))
                {
                    ConfigNode biomeNode = node.GetNode("Biomes");
                    foreach (ConfigNode.Value biomeDefinition in biomeNode.values)
                    {
                        Color c;
                        if (!TryParseDustColour(biomeDefinition, body.Name, out c))
                            continue;

                        BiomeDust biome = new BiomeDust
                        {
                            Name = biomeDefinition.name,
                            DustColour = c
                        };
                        if (biomeDefinition.name.Equals("Default", StringComparison.InvariantCultureIgnoreCase))
                            body.DefaultBiomeColour = c;

                        body.Biomes.Add(biome);
                    }
                }

                // Load structures
                if (node.HasNode("Structures"))
                {
                    ConfigNode structureNode = node.GetNode("Structures");
                    foreach (ConfigNode.Value structureDefinition in structureNode.values)
                    {
                        Color c;
                        if (!TryParseDustColour(structureDefinition, body.Name, out c))
                            continue;

                        StructureDust structure = new StructureDust
                        {
                            Name = structureDefinition.name,
                            DustColour = c
                        };
                        body.Structures.Add(structure);
                    }
                }

                _bodies.Add(body);
            }

            if (_bodies.Count == 0)
            {
                Debug.LogWarning("[CollisionFX] Unable to find dust definitions. Ensure " + CollisionFX.ConfigPath + " contains correct BodyDust entries.");
            }
        }

        public static bool TryParseDustColour(ConfigNode.Value cfgVal, string bodyName, out Color loadedColour)
        {
            loadedColour = Color.clear;
            string colourString = cfgVal.value;
            string[] colourValues = colourString.Split(' ');
            if (colourValues.Length > 4)
            {
                Debug.LogWarning("[CollisionFX] Invalid colour definition in body \"" +
                    bodyName + "\": Too many parameters.");
                return false;
            }
            float r, g, b, a;
            NumberStyles flags = NumberStyles.AllowDecimalPoint;
            if (!float.TryParse(colourValues[0], flags, CultureInfo.InvariantCulture, out r))
            {
                Debug.LogWarning("[CollisionFX] Invalid colour definition in body \"" +
                    bodyName + "\": \"" + colourValues[0] + "\" is not a valid integer.");
                return false;
            }
            if (!float.TryParse(colourValues[1], flags, CultureInfo.InvariantCulture, out g))
            {
                Debug.LogWarning("[CollisionFX] Invalid colour definition in body \"" +
                    bodyName + "\": \"" + colourValues[1] + "\" is not a valid integer.");
                return false;
            }
            if (!float.TryParse(colourValues[2], flags, CultureInfo.InvariantCulture, out b))
            {
                Debug.LogWarning("[CollisionFX] Invalid colour definition in body \"" +
                    bodyName + "\": \"" + colourValues[2] + "\" is not a valid integer.");
                return false;
            }

            // Allow using the value ranges 0.0 -> 1.0 and 0 -> 255.
            // Determine whether there are any numbers after a decimal point.
            if (r % 1 == 0) r /= 255f;
            if (g % 1 == 0) g /= 255f;
            if (b % 1 == 0) b /= 255f;

            if (colourValues.Length == 4)
            {
                if (!float.TryParse(colourValues[3], flags, CultureInfo.InvariantCulture, out a))
                {
                    Debug.LogWarning("[CollisionFX] Invalid colour definition in body \"" +
                        bodyName + "\": \"" + colourValues[3] + "\" is not a valid integer.");
                    return false;
                }
                if (a % 1 == 0) a /= 255;
                loadedColour = new Color(r, g, b, a);
            }
            else
                loadedColour = new Color(r, g, b);

            return true;
        }

        public static Color GenericDustColour = new Color(0.8f, 0.8f, 0.8f, 0.007f); // Grey 210 210 210
        static Color dirtColour = new Color(0.65f, 0.48f, 0.34f, 0.05f); // Brown 165, 122, 88
        static Color lightDirtColour = new Color(0.65f, 0.52f, 0.34f, 0.05f); // Brown 165, 132, 88
        static Color sandColour = new Color(0.80f, 0.68f, 0.47f, 0.05f); // Light brown 203, 173, 119
        static Color snowColour = new Color(0.90f, 0.94f, 1f, 0.05f); // Blue-white 230, 250, 255
        private static BodyDust _previousBodyDust;
        private static BiomeDust _previousBiomeDust;
        private static StructureDust _previousStructureDust;

        public static BodyDust GetBodyDust(string bodyName)
        {
            if (_bodies == null)
                LoadDustColours();

            BodyDust body = null;
            if (_previousBodyDust != null &&
                _previousBodyDust.Name.Equals(bodyName, StringComparison.InvariantCultureIgnoreCase))
                body = _previousBodyDust;
            else
            {
                _previousBiomeDust = null;
                foreach (BodyDust db in _bodies)
                {
                    if (db.Name.Equals(bodyName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        body = db;
                        _previousBodyDust = db;
                        break;
                    }
                }
            }
            return body;
        }

        public static Color GetDustColour(string colliderName)
        {
            string currentBody = FlightGlobals.ActiveVessel.mainBody.name;
            BodyDust body = GetBodyDust(currentBody);
            if (body == null)
            {
                Debug.LogWarning("[CollisionFX] Unable to find dust definition for body \"" + currentBody + "\"; using CollisionFX default.");
                return GenericDustColour;
            }

            if (Utils.IsPQS(colliderName))
            {
                return GetBiomeDustColour(body, Utils.GetCurrentBiomeName(FlightGlobals.ActiveVessel));
            }
            else
            {
                return GetStructureDustColour(body.Structures, colliderName);
            }
        }

        public static Color GetBiomeDustColour(BodyDust body, string currentBiome)
        {
            BiomeDust biome = null;
            if (_previousBiomeDust != null &&
                _previousBiomeDust.Name.Equals(currentBiome, StringComparison.InvariantCultureIgnoreCase))
                biome = _previousBiomeDust;
            else
            {
                foreach (BiomeDust b in body.Biomes)
                {
                    if (b.Name.Equals(currentBiome, StringComparison.InvariantCultureIgnoreCase))
                    {
                        biome = b;
                        _previousBiomeDust = b;
                        break;
                    }
                }
            }

            if (biome != null)
                return biome.DustColour;
            if (body.DefaultBiomeColour != Color.clear)
                return body.DefaultBiomeColour;

            // No colour found.
            return Color.clear;
        }

        /* Known structure (collider) names:
         * Runway: Tier 1
            runway_lev1
         * Runway: Tier 2
            runway1
            runway2
            runway3
            runway4
            runway5
            runway6
            runway7
            grass_dirt_transition
            groundPlane
         * VAB: Tier 2
            VAB_lev2_groundPlane
         * SPH: Tier 1
            SPH_1_ground
         */

        public static Color GetStructureDustColour(List<StructureDust> structures, string colliderName)
        {
            StructureDust structure = null;
            if (_previousStructureDust != null &&
                _previousStructureDust.Name.Equals(colliderName, StringComparison.InvariantCultureIgnoreCase))
                structure = _previousStructureDust;
            else
            {
                foreach (StructureDust s in structures)
                {
                    if (s.Name.Equals(colliderName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        structure = s;
                        _previousStructureDust = s;
                        break;
                    }
                }
            }
            if (structure != null)
                return structure.DustColour;

            // No colour found.
            return Color.clear;
        }

        public static Color GetStructureDustColour(string colliderName)
        {
            string currentBody = FlightGlobals.ActiveVessel.mainBody.name;
            BodyDust body = GetBodyDust(currentBody);
            if (body == null)
                return Color.clear; // Dust is disabled for this structure.

            return Color.clear;
        }
    }
}
