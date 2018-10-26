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
    public class ImageSource {
        #region Properties

        public int Breakpoint { get; }

        public ImagePreset Preset { get; }

        public string Unit { get; }

        #endregion Properties

        #region Constructors

        public ImageSource(ImagePreset preset, int breakpoint, string unit = "w") {
            Breakpoint = breakpoint;
            Preset = preset;
            Unit = unit;
        }

        public ImageSource(ImagePreset preset) {
            Breakpoint = preset.ScaleMethod.TargetWidth;
            Preset = preset;
            Unit = "w";
        }

        #endregion Constructors
    }
}