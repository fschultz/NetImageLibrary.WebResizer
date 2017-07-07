﻿#region License and copyright notice
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Configuration;

    public static class HtmlHelperExtensions {

        public static MvcHtmlString CachedImage(this HtmlHelper htmlHelper, string path, ImagePreset preset, object htmlAttributes = null) {
            return CachedImage(htmlHelper, path, preset.Name, htmlAttributes);
        }

        public static MvcHtmlString CachedImage(this HtmlHelper htmlHelper, string path, string presetName, object htmlAttributes = null) {
            if (string.IsNullOrEmpty(path)) {
                return null;
            }

            path = path.TrimStart('/');
            var src = $"/{ImageCacheConfiguration.VirtualRoot}/{presetName}/{path}";

            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            var tagBuilder = new TagBuilder("img");
            tagBuilder.MergeAttribute("src", src);
            tagBuilder.MergeAttributes(attributes);

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString CachedImageSet(this HtmlHelper htmlHelper, string path, IEnumerable<ImagePreset> presets, object htmlAttributes = null) {
            if (string.IsNullOrEmpty(path)) {
                return null;
            }
            if (presets == null) {
                return null;
            }
            if (!presets.Any()) {
                return null;
            }

            path = path.TrimStart('/');
            var mainPresetName = presets.First().Name;
            var src = $"/{ImageCacheConfiguration.VirtualRoot}/{mainPresetName}/{path}";

            var sources = new List<string>();
            foreach (var preset in presets) {
                sources.Add($"/{ImageCacheConfiguration.VirtualRoot}/{preset.Name}/{path} {preset.ScaleMethod.TargetWidth}w");
            }
            var srcSet = string.Join(", ", sources);

            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            var tagBuilder = new TagBuilder("img");
            tagBuilder.MergeAttribute("src", src);
            tagBuilder.MergeAttribute("srcset", srcSet);
            tagBuilder.MergeAttributes(attributes);

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }
        
    }
}
