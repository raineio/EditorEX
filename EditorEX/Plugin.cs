﻿using HarmonyLib;
using IPA;
using IPA.Config.Stores;
using IPA.Loader;
using SiraUtil.Attributes;
using SiraUtil.Zenject;
using System.Reflection;
using IPAConfig = IPA.Config.Config;
using IPALogger = IPA.Logging.Logger;

namespace EditorEX
{
    [Plugin(RuntimeOptions.DynamicInit), Slog]
    public class Plugin
    {
        internal const string HARMONYID = "com.github.ItsKaitlyn03.EditorEX";
        internal static Harmony HarmonyInstance { get; private set; } = new Harmony(HARMONYID);

        [Init]
        public Plugin(IPALogger logger, IPAConfig conf, PluginMetadata metadata, Zenjector zenjector)
        {
            zenjector.UseLogger(logger);

            Config config = conf.Generated<Config>();
            zenjector.Install(Location.App, container =>
            {
                container.BindInstance(config).AsSingle();
                container.BindInstance(new UBinder<Plugin, PluginMetadata>(metadata));
            });

            //zenjector.Install<EXEditorInstaller>(Location.App); // TODO: Editor
        }

        [OnEnable]
        public void OnEnable()
        {
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        }

        [OnDisable]
        public void OnDisable()
        {
            HarmonyInstance.UnpatchSelf();
        }
    }
}
