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

namespace Kaliko.ImageLibrary.WebResizer.Controllers {
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    public class ImageFactoryController : Controller {
        public ActionResult Index(string preset, string path) {
            if (string.IsNullOrEmpty(path)) {
                return HttpNotFound();
            }

            var imageFactory = new ImageFactory(preset, path);

            if (!imageFactory.IsValid()) {
                return HttpNotFound();
            }

            var requestETag = GetETagFromRequest();

            if (!imageFactory.HasChanged(requestETag)) {
                return new HttpStatusCodeResult(HttpStatusCode.NotModified);
            }

            try {
                var imageResult = imageFactory.GetImageResult();

                if (imageResult == null) {
                    return HttpNotFound();
                }

                SetETagForResponse(imageFactory.GetETag());

                return imageResult;
            }
            catch {
                return HttpNotFound();
            }
        }

        #region Private functions

        private void SetETagForResponse(string etag) {
            Response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);
            Response.Cache.SetETag(etag);
        }

        private string GetETagFromRequest() {
            return Request.Headers["If-None-Match"];
        }

        #endregion
    }
}
