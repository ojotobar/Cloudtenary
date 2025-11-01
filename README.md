# Cloudtenary

### A lightweight .NET wrapper around Cloudinary for effortless file uploads.

---

## Table of Contents

1. [Introduction](#introduction)
2. [Features](#features)
3. [Installation](#installation)
4. [Configuration](#configuration)
5. [Usage](#usage)
6. [Example Methods](#example-methods)
7. [Links](#links)

---

## Introduction

**Cloudtenary** is a small, elegant .NET library that simplifies connecting to [Cloudinary](https://cloudinary.com) and performing media operations such as uploading, transforming, and deleting images, videos, and documents.

It wraps the official `CloudinaryDotNet` SDK with intuitive, ready-to-use methods and dependency injection support for modern .NET projects.

---

## Features

* Simple setup with a single `AddCloudtenary()` extension method
* Image upload with optional resizing, cropping, and quality transformations
* Video upload with support for resizing, trimming, fade effects, and text overlays
* Document (raw file) upload
* Clean deletion of any media type (image, video, or raw)
* Built with DI-friendly design (`ICloudtenary` interface)
* Small footprint, zero configuration clutter

---

## Installation

Install the package via NuGet:

```bash
dotnet add package Cloudtenary
```

Or add it via the NuGet Package Manager in Visual Studio.

---

## Configuration

In your `appsettings.json` or secret store, add your Cloudinary credentials:

```json
"CloudtenarySettings": {
  "CloudName": "your-cloud-name",
  "Key": "your-api-key",
  "Secret": "your-api-secret"
}
```

---

## Registration

In your `Program.cs` or `Startup.cs`, register the service:

```csharp
using Cloudtenary.Extensions;
using Cloudtenary.Settings;

builder.Services.AddCloudtenary(options =>
{
    options.CloudName = builder.Configuration["CloudtenarySettings:CloudName"];
    options.Key = builder.Configuration["CloudtenarySettings:Key"];
    options.Secret = builder.Configuration["CloudtenarySettings:Secret"];
});
```

> This automatically registers `ICloudtenary` as a singleton for dependency injection.

---

## Usage

Inject the interface anywhere you need to upload or manage files:

```csharp
using Cloudtenary;
using CloudinaryDotNet.Actions;

public class MediaService
{
    private readonly ICloudtenary _cloudtenary;

    public MediaService(ICloudtenary cloudtenary)
    {
        _cloudtenary = cloudtenary;
    }

    public async Task<string?> UploadImageAsync(Stream fileStream, string fileName)
    {
        var result = await _cloudtenary.UploadImageAsync(fileName, fileStream);
        return result?.Url;
    }
}
```

---

## Example Methods

### Upload an Image (with transformations)

Automatically resizes and optimizes the image:

```csharp
await _cloudtenary.UploadImageAsync("avatar", "avatar.jpg", stream, 400, 400);
```

### Upload an Image (original)

Uploads without any transformation:

```csharp
await _cloudtenary.UploadImageAsync("avatar", stream);
```

### Upload a Video (with effects)

Trims, fades in, and overlays text:

```csharp
await _cloudtenary.UploadVideoAsync("intro.mp4", stream, 720, 480, 3, 60, "© Toba");
```

### Upload a Document

Uploads any raw file (PDF, DOCX, ZIP, etc.):

```csharp
await _cloudtenary.UploadRawFileAsync("resume.pdf", "resume.pdf", stream);
```

### Delete a File

Removes any media type by its public ID:

```csharp
await _cloudtenary.DeleteMediaFileAsync("videos/intro", ResourceType.Video);
```

---

## Notes

* `CloudtenaryUploadResult` contains `PublicId` and `Url`.
* Videos are saved in the `/videos` folder, images in `/images`, and raw files in `/documents`.
* Transformations use Cloudinary’s on-upload capabilities — no post-processing needed.

---

## Links

* **GitHub Repository:** [ojotobar/Cloudtenary](https://github.com/ojotobar/Cloudtenary)
* **Contact:** [ojotobar@gmail.com](mailto:ojotobar@gmail.com)
* **Cloudinary SDK:** [Official Docs](https://cloudinary.com/documentation/dotnet_integration)