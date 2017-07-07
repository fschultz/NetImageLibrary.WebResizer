#region License and copyright notice
/* 
 * Kaliko Image Library Web Cache
 * 
 * Copyright (c) Fredrik Schultz / Kaliko
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 * http://www.gnu.org/licenses/lgpl-3.0.html
 */
#endregion

namespace Kaliko.ImageLibrary.WebResizer.Configuration {
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Helpers;
    using Providers;
    using Stores;

    public class ImageCacheConfiguration {
        #region Public properties

        public static ObjectCreation.Creator<ImageProviderBase> DefaultImageProvider { get; private set; }
        public static IImageStore ImageStore { get; set; }
        public static Dictionary<string, ObjectCreation.Creator<ImageProviderBase>> ImageProviders { get; }
        public static Dictionary<string, ImagePreset> Presets { get; }
        public static string VirtualRoot { get; set; }

        #endregion

        #region Constructors

        static ImageCacheConfiguration() {
            // Defaults
            VirtualRoot = "images";
            ImageStore = new FileImageStore();
            SetDefaultImageProvider<FileImageProvider>();

            ImageProviders = new Dictionary<string, ObjectCreation.Creator<ImageProviderBase>>();
            Presets = new Dictionary<string, ImagePreset>();
        }

        #endregion

        #region Public functions

        public static void AddImageProvider<T>(string basePath) where T : ImageProviderBase {
            if (string.IsNullOrEmpty(basePath)) {
                throw new ArgumentException("Base path may not be null or empty. Use SetDefaultImageProvider<T> instead.");
            }
            if (ImageProviders.ContainsKey(basePath)) {
                throw new ArgumentException("Base path already exists in collection.");
            }

            var objectCreator = ObjectCreation.GetCreator<T, ImageProviderBase>(); 
            ImageProviders.Add(basePath, objectCreator);
        }

        public static void AddPreset(ImagePreset preset) {
            if (preset == null) {
                throw new ArgumentException("Preset may not be null.");
            }
            if (string.IsNullOrEmpty(preset.Name)) {
                throw new ArgumentException("Preset name may not be null or empty.");
            }
            if (Presets.ContainsKey(preset.Name)) {
                throw new ArgumentException("Preset name already exists in collection.");
            }

            Presets.Add(preset.Name, preset);
        }

        public static ImageProviderBase GetImageProvider(string presetName, string path, int version) {
            foreach (var imageProvider in ImageProviders) {
                if (path.StartsWith(imageProvider.Key)) {
                    return imageProvider.Value(presetName, path, version);
                }
            }

            return DefaultImageProvider(presetName, path, version);
        }

        public static void RegisterRoute(RouteCollection routeCollection) {
            routeCollection.MapRoute("ImageFactory", $"{VirtualRoot}/{{preset}}/{{*path}}", new { controller = "ImageFactory", action = "Index" });
        }

        public static void SetDefaultImageProvider<T>() where T : ImageProviderBase {
            DefaultImageProvider = ObjectCreation.GetCreator<T, ImageProviderBase>();
        }

        #endregion
    }
}
