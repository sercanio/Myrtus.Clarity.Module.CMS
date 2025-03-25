namespace CMSModule.Attributes
{
    internal static class CMSPermissions
    {
        // Content
        public const string ContentRead = "content:read";
        public const string ContentCreate = "content:create";
        public const string ContentUpdate = "content:update";
        public const string ContentDelete = "content:delete";

        // Media
        public const string MediaRead = "media:read";
        public const string MediaCreate = "media:create";
        public const string MediaUpdate = "media:update";
        public const string MediaDelete = "media:delete";

        // SEO
        public const string SEORead = "seo:read";
        public const string SEOCreate = "seo:create";
        public const string SEOUpdate = "seo:update";
        public const string SEODelete = "seo:delete";
    }
}
