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

namespace Kaliko.ImageLibrary.WebResizer {
    using System.Collections.Generic;
    using Filters;
    using Scaling;

    public class ImagePreset {
        public List<IFilter> Filters { get; set; }
        public string Name { get; set; }
        public bool PreventUpscaling { get; set; }
        public ScalingBase ScaleMethod { get; set; }
        public int Quality { get; set; }
        public int Version { get; set; }

        public ImagePreset(string name, ScalingBase scaleMethod) {
            Name = name;
            ScaleMethod = scaleMethod;
            PreventUpscaling = true;
            Quality = 90;
            Filters = new List<IFilter>();
            Version = 1;
        }
    }
}
