using Myrtus.Clarity.Core.Domain.Abstractions;

namespace CMSModule.Errors;

public static class CMSModuleErrors
{
    public static class Content
    {
        public static readonly DomainError NotFound = new(
            "Content.NotFound",
            404,
            "The content with the specified identifier was not found.");

        public static readonly DomainError VersionNotFound = new(
            "Content.VersionNotFound",
            404,
            "The specified content version was not found.");
    }

    public static class Media
    {
        public static readonly DomainError NotFound = new(
            "Media.NotFound",
            404,
            "The media with the specified identifier was not found.");

        public static readonly DomainError InvalidFile = new(
            "Media.InvalidFile",
            400,
            "The provided file is invalid or empty.");

        public static readonly DomainError CannotGenerateSasUri = new(
            "Media.CannotGenerateSasUri",
            500,
            "Unable to generate SAS URI for the media.");

        public static readonly DomainError StorageKeyMissing = new(
            "Media.StorageKeyMissing",
            500,
            "Azure Blob Storage connection string or container name is missing.");

        //new DomainError("Media.UploadFailed", 500, ex.Message)
        public static DomainError UploadFailed(string message) => new(
            "Media.UploadFailed",
            500,
            message);
    }

    public static class SEO
    {
        public static readonly DomainError NotFound = new(
            "SEO.NotFound",
            404,
            "SEO settings not found.");
    }

    // Add more error categories as needed
}