using System.Collections.Generic;
using HarmonyLib;
using BepInEx;
using BepInEx.Logging;
using UnityEngine;

namespace GalacticScale.Generators
{   
    [BepInPlugin("dsp.galactic-scale.2.spaghetti", "Galactic Scale 2 Spaghetti Mod", "1.0.0.4")]
    [BepInDependency("dsp.galactic-scale.2")]
    public class Spaghetti : BaseUnityPlugin, iConfigurableGenerator
    {
        
        public string Name => "Spaghetti";
        public string Author => "innominata";
        public string Description => "Spaghetti Generator Custom Mission";
        public string Version => "1.0.0.4";
        public string GUID => "space.customizing.generators.spaghetti";
        public new GSGeneratorConfig Config => config; // Return our own generator config when asked, instead of using default config
        public GSOptions Options => options; // Likewise for options
        public bool sauce => preferences.GetBool("sauce", true);

        public bool warned
        {
            get { return preferences.GetBool("warned", false); }
            set { preferences.Set("warned", value);}
        }

        public void Init()
        {
            GS2.Log("Initializing Spaghetti Generator"); // Use Galactic Scales Log system for debugging purposes.
            // config.DisableSeedInput = false;
            // config.DisableStarCountSlider = true;
            config.MaxStarCount = 1024; //1024 is game limit, and already ridiculous. Setting this higher will cause the game to crash.
            config.MinStarCount = 1;
            options.Add(GSUI.Checkbox("Sauce?", true, "sauce", null, "Make the planet look stupid :D"));
            options.Add(GSUI.Button("DeSauce", "I hate sauce", desauce, null, "Make the planet look normal :D"));
        }

        public void desauce(Val o)
        {
            var gsplanet = GS2.GetGSPlanet(GameMain.localPlanet);
            gsplanet.GsTheme.oceanMaterial = Themes.OceanWorld.oceanMaterial;
            gsplanet.GsTheme.terrainMaterial = Themes.OceanWorld.terrainMaterial;
            gsplanet.GsTheme.Process();
        }
        public void Import(GSGenPreferences prefs) //This is called on game start, with the same object that was exported last time
        {
            preferences = prefs;
        }
        public GSGenPreferences Export() // Send our custom preferences object to be saved to disk
        {
            return preferences;
        }

        //////////////////////////////////////////////////////////////////////
        ///// All code below here is generator specific
        //////////////////////////////////////////////////////////////////////
        public GSOptions options = new GSOptions();
        private GSGenPreferences preferences = new GSGenPreferences();
        private GSGeneratorConfig config = new GSGeneratorConfig();

        //////////////////////////////
        /// Finally, lets do something
        //////////////////////////////
        
        public void Generate(int starCount, StarData forcedStar = null)
        {
            CreateThemes();
            //Create a list containing a single planet, that has default values.
            GS2.Random random = new GS2.Random(GSSettings.Seed);
            
            GSPlanet Polpetta = new GSPlanet("Polpetta Prime", "Pastabowl", 500, 1, 0, 5000, -1, 45, 1000, 0, -1);
            GSPlanet Salsa = new GSPlanet("Salsa", "GasGiant", 200, 2, 90, 5000, 90, 0, 5000, 0, -1, new GSPlanets() { Polpetta });
            GSSettings.BirthPlanetName = "Polpetta Prime"; //We need to decide where to spawn
            //Create one O-type main sequence star, containing the above planet. Set the seed to one. This is the minimum requirement for a star. There are more options available.
            var Aglio = GSSettings.Stars.Add(new GSStar(1, "Aglio", ESpectrType.X, EStarType.NeutronStar, new GSPlanets(){Salsa}));
            Aglio.radius = 3f;
            Aglio.luminosity = 10f;
            Polpetta.randomizeVeinAmounts = false;
            Polpetta.randomizeVeinCounts = false;
            //Create a whole bunch of identical empty F-type Giant stars
            for (var i = 0; i < starCount -1; i++)
            {
                var star = GSSettings.Stars.Add(new GSStar(1, "Parmesan", ESpectrType.X, EStarType.WhiteDwarf, new GSPlanets()));
                star.position = random.PointOnSphere(10);
            }
        }

        private void CreateThemes()
        {
            GSTheme Pasta = new GSTheme("Pastabowl", "Pasta Bowl", "OceanWorld");
            Pasta.Algo = 6;
            Pasta.CustomGeneration = true;
            Pasta.TerrainSettings = new GSTerrainSettings()
            {
                Algorithm = "GSTA1",
                BaseHeight = -1.3f,
                BiomeHeightMulti = 2,
                BiomeHeightModifier = 0,
                HeightMulti = 1,
                LandModifier = -0.7f,
                RandomFactor = 0.3f
            };
            Pasta.VeinSettings = new GSVeinSettings()
            {
                Algorithm = "GS2W",
                VeinPadding = 0.1f,
                VeinTypes = new GSVeinTypes()
            };
            Pasta.VeinSettings.VeinTypes.Add(GSVeinType.Generate(EVeinType.Iron, 3000, 3000, 3, 3, 1, 1, false));
            Pasta.VeinSettings.VeinTypes.Add(GSVeinType.Generate(EVeinType.Copper, 3000, 3000, 3, 3, 1, 1, false));
            Pasta.VeinSettings.VeinTypes.Add(GSVeinType.Generate(EVeinType.Coal, 1000, 1000, 3, 3, 1, 1, false));
            Pasta.VeinSettings.VeinTypes.Add(GSVeinType.Generate(EVeinType.Stone, 1000, 1000, 3, 3, 1, 1, false));
            Pasta.VeinSettings.VeinTypes.Add(GSVeinType.Generate(EVeinType.Titanium, 1000, 1000, 3, 3, 1, 1, false));
            Pasta.VeinSettings.VeinTypes.Add(GSVeinType.Generate(EVeinType.Silicium, 1000, 1000, 3, 3, 1, 1, false));
            Pasta.VeinSettings.VeinTypes.Add(GSVeinType.Generate(EVeinType.Oil, 100, 100, 1, 1, 1, 1, false));
            Pasta.VeinSettings.VeinTypes.Add(GSVeinType.Generate(EVeinType.Bamboo, 200, 200, 3, 3, 1, 1, false));
            Pasta.VeinSettings.VeinTypes.Add(GSVeinType.Generate(EVeinType.Crysrub, 200, 200, 3, 3, 1, 1, false));
            Pasta.VeinSettings.VeinTypes.Add(GSVeinType.Generate(EVeinType.Grat, 200, 200, 3, 3, 1, 1, false));
            Pasta.VeinSettings.VeinTypes.Add(GSVeinType.Generate(EVeinType.Mag, 200, 200, 3, 3, 1, 1, false));
            Pasta.VeinSettings.VeinTypes.Add(GSVeinType.Generate(EVeinType.Diamond, 200, 200, 3, 3, 1, 1, false));
            Pasta.VeinSettings.VeinTypes.Add(GSVeinType.Generate(EVeinType.Fireice, 400, 400, 3, 3, 1, 1, false));
            Pasta.VeinSettings.VeinTypes.Add(GSVeinType.Generate(EVeinType.Fractal, 200, 200, 3, 3, 1, 1, false));
            Pasta.VegeSettings = new GSVegeSettings()
            {
                Algorithm = "GS2",
                Group1 = new List<string>() { },
                Group2 = new List<string>()
                {
                    "MedStone5",
                    "MedStone4",
                    "MedStone5",
                    "MedStone3",
                    "MedStone4",
                    "MedFragment6",
                    "MedFragment7",
                    "MedFragment8",
                    "MedFragment7",
                    "MedFragment8"
                },
                Group3 = new List<string>()
                {
                    "MedStone5",
                    "MedStone5",
                    "MedStone5",
                    "MedStone4",
                    "MedStone5",
                    "MedStone3",
                    "MedStone4",
                    "MedFragment6",
                    "MedFragment7",
                    "MedFragment8",
                    "MedFragment7",
                    "MedFragment8"
                },
                Group4 = new List<string>()
                {
                    "RedStoneTree5",
                    "MedFragment6",
                    "MedFragment7",
                    "MedFragment8",
                    "MedFragment7",
                    "MedFragment8"
                },
                Group5 = new List<string>()
                {
                    "MedFragment6",
                    "MedFragment7",
                    "MedFragment8",
                    "MedFragment7",
                    "MedFragment8"
                },
                Group6 = new List<string>()
                {
                    "RedStoneTree5",
                    "RedStoneTree5",
                    "RedStoneTree5",
                    "RedStoneTree5",
                    "RedStoneTree5"
                }
            };
            Pasta.Wind = 2;
            Pasta.WaterItemId = 1000;
            if (sauce)
            {


                Pasta.oceanMaterial.CopyFrom = "Lava.oceanMat";
           

            Pasta.terrainMaterial.CopyFrom = "Gobi.terrainMat";
            Pasta.oceanMaterial.Colors["_Color0"] = new Color { r = 1.0f, g = 0.36f, b = 0.2f, a = 1 };
            Pasta.oceanMaterial.Colors["_Color1"] = new Color { r = 0.6f, g = 0.15f, b = 0.06f, a = 1 };
            Pasta.oceanMaterial.Colors["_Color2"] = new Color { r = 0.22f, g = 0.04f, b = 0.02f, a = 0.8f };
            Pasta.oceanMaterial.Colors["_Color3"] = new Color { r = 0.6f, g = 0.2f, b = 0.1f, a = 1 };
            Pasta.oceanMaterial.Params["_ChaosDistort"] = 0.45f;
            Pasta.oceanMaterial.Params["_ChaosOverlay"] = 0.95f;
            Pasta.oceanMaterial.Params["_ChaosTile"] = 1.5f;
            Pasta.oceanMaterial.Params["_NoiseSpeed"] = 0.25f;
            Pasta.oceanMaterial.Params["_NoiseTile"] = 1200f;
            Pasta.oceanMaterial.Params["_RimPower"] = 4.0f;
            Pasta.oceanMaterial.Params["_RotSpeed"] = 0.0005f;
            Pasta.oceanMaterial.Params["_SpotIntens"] = 5.0f;
            Pasta.terrainMaterial.Params["_Multiplier"] = 0.8f;
            Pasta.terrainMaterial.Colors.Add("_SpeclColor", new Color(0, 0, 0, 1));
            Pasta.terrainMaterial.Textures = new Dictionary<string, string>();
            Pasta.terrainMaterial.Textures.Add("_BioTex0A", "GS2|grey-snow");
            Pasta.terrainMaterial.Textures.Add("_BioTex1A", "GS2|grey-snow");
            Pasta.terrainMaterial.Textures.Add("_BioTex2A", "GS2|grey-snow");
            Pasta.terrainMaterial.Tint = new Color(0.95f, 0.95f, 0.8f, 1f);
            // Pasta.oceanMaterial.Tint = new Color(0.285f, 0.301f, 0.215f, 0.5f);
            Pasta.oceanMaterial.Colors["_Color"] = new Color(0.8f, 0.4f, 0.4f, 0.8f);
        }

        Pasta.Process();
        }

        public new static ManualLogSource Logger;
        private void Awake()
        {
            Logger = new ManualLogSource("GS2Spaghetti");
            BepInEx.Logging.Logger.Sources.Add(Logger);
            Logger.Log(LogLevel.Message, "Loaded");
            GS2.Generators.Add(this);
            // GS2.LoadPreferences();
            GS2.Warn("Test");
            Harmony.CreateAndPatchAll(typeof(Spaghetti));
        }


        [HarmonyPostfix, HarmonyPatch(typeof(UIBuildMenu), "OnVeinBuriedClick")]
        public static void OnVeinBuriedClick(ref UIBuildMenu __instance)
        {
            var Gen = GS2.GetGeneratorByID("space.customizing.generators.spaghetti") as Spaghetti;
            
            
            if (GSSettings.Stars[0].Name == "Aglio" && GSSettings.Instance.generatorGUID == null) GSSettings.Instance.generatorGUID = "space.customizing.generators.spaghetti";
            if (GSSettings.Instance.generatorGUID != "space.customizing.generators.spaghetti") return;
            __instance.reformTool.buryVeins = false;
            GS2.Log("Click");
            if (!Gen.warned)
            {
                UIMessageBox.Show("Spaghetti".Translate(), "It would be too easy if you could just bury veins!".Translate(), "I agree!".Translate(), 0);
                Gen.warned = true;
                GS2.SavePreferences();
            }
            
            GameMain.mainPlayer.sandCount += 99999;
        }
        
    }
}