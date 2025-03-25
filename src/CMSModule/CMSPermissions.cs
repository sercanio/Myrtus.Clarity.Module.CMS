using AppTemplate.Domain.Roles;

namespace CMSModule
{
    public static class CMSPermissions
    {
        // Content
        public static readonly Permission ContentRead = new(
            Guid.Parse("f29a86ba-3da7-4e51-a513-7d821b06ab3e"),
            "content",
            "content:read");

        public static readonly Permission ContentCreate = new(
            Guid.Parse("5b607a75-2268-4f31-824e-3639f9bd4877"),
            "content",
            "content:create");

        public static readonly Permission ContentUpdate = new(
            Guid.Parse("f1859df6-1597-47b3-9364-44e4bc3336aa"),
            "content",
            "content:update");

        public static readonly Permission ContentDelete = new(
            Guid.Parse("f511138c-4bb9-40c0-ae71-5ad86108fc63"),
            "content",
            "content:delete");

        // Media
        public static readonly Permission MediaRead = new(
            Guid.Parse("791a2265-a548-4b25-bdb4-e9858106fa11"),
            "media",
            "media:read");

        public static readonly Permission MediaCreate = new(
            Guid.Parse("5cde0e20-0491-4322-b805-a703051a716c"),
            "media",
            "media:create");

        public static readonly Permission MediaUpdate = new(
            Guid.Parse("88497de0-c523-4f0f-9c95-81d7210b0e80"),
            "media",
            "media:update");

        public static readonly Permission MediaDelete = new(
            Guid.Parse("e3719283-3080-4293-b606-21507eb2b60e"),
            "media",
            "media:delete");

        // SEO
        public static readonly Permission SEORead = new(
            Guid.Parse("5aaa0b92-15c7-448b-9c51-2533be1d8d25"),
            "seo",
            "seo:read");

        public static readonly Permission SEOCreate = new(
            Guid.Parse("54ba53db-5b86-4ce8-8698-a65501e3d73e"),
            "seo",
            "seo:create");

        public static readonly Permission SEOUpdate = new(
            Guid.Parse("3092fdf2-992a-4ab7-972b-a5af041dfc4e"),
            "seo",
            "seo:update");

        public static readonly Permission SEODelete = new(
            Guid.Parse("c1e0a259-df8f-41dc-ba5f-6c563c68948b"),
            "seo",
            "seo:delete");
    }
}
