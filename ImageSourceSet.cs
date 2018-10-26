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
    using System.Collections;
    using System.Collections.Generic;

    public class ImageSourceSet : IEnumerable<ImageSource> {
        #region Properties

        public ImagePreset DefaultPreset { get; }

        public IList<ImageSource> Presets { get; }

        #endregion Properties

        #region Constructors

        public ImageSourceSet(ImagePreset defaultPreset) {
            DefaultPreset = defaultPreset;
            Presets = new List<ImageSource>();
        }

        public ImageSourceSet(ImagePreset defaultPreset, IEnumerable<ImageSource> presets) : this(defaultPreset) {
            Presets = new List<ImageSource>(presets);
        }

        #endregion Constructors

        #region Methods

        public void Add(ImageSource imageSetPreset) {
            Presets.Add(imageSetPreset);
        }

        public IEnumerator<ImageSource> GetEnumerator() {
            return Presets.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Presets.GetEnumerator();
        }

        #endregion Methods
    }
}