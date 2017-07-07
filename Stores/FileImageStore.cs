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

namespace Kaliko.ImageLibrary.WebResizer.Stores {
    using System;
    using System.IO;
    using System.Web.Mvc;
    using Providers;

    public class FileImageStore : IImageStore {
        #region Properties

        public static string ImageCacheLocation { get; set; }

        #endregion

        #region Constructors

        static FileImageStore() {
            ImageCacheLocation = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}ImageCache\\";
        }

        #endregion

        #region Public functions

        public ActionResult GetImageResult(ImageProviderBase imageProvider, ImagePreset preset) {
            var imagePath = BuildImagePath(imageProvider);

            if (File.Exists(imagePath)) {
                return new FilePathResult(imagePath, "image/jpeg");
            }

            using (var image = imageProvider.GetImage()) {
                GetScaledImage(preset, image, imagePath);
            }

            return new FilePathResult(imagePath, "image/jpeg");
        }

        #endregion

        #region Private functions

        private static void GetScaledImage(ImagePreset preset, KalikoImage image, string filePath) {
            using (var scaledImage = image.Scale(preset.ScaleMethod, preset.PreventUpscaling)) {
                foreach (var filter in preset.Filters) {
                    scaledImage.ApplyFilter(filter);
                }

                scaledImage.SaveJpg(filePath, preset.Quality);
            }
        }

        private static string BuildImagePath(ImageProviderBase imageProvider) {
            var eTag = imageProvider.GetETag();
            return $"{ImageCacheLocation}{eTag}.jpg";
        }

        #endregion
    }
}