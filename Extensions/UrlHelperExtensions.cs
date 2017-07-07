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

namespace Kaliko.ImageLibrary.WebResizer.Extensions {
    using System.Web.Mvc;
    using Configuration;

    public static class UrlHelperExtensions {

        public static string CachedImage(this UrlHelper urlHelper, string path, ImagePreset preset) {
            return CachedImage(urlHelper, path, preset.Name);
        }

        public static string CachedImage(this UrlHelper urlHelper, string path, string presetName) {
            path = path.TrimStart('/');

            return $"/{ImageCacheConfiguration.VirtualRoot}/{presetName}/{path}";
        }

    }
}
