using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CMSModule.DTOs;

public class UploadMediaRequest
{
    [Required]
    public IFormFile File { get; set; }
}
