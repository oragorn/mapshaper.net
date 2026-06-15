# Release Guide

This project publishes NuGet packages and GitHub Releases from version tags.

## Prerequisites

- A NuGet Trusted Publishing policy exists for this repository.
- The policy points to:
  - Package owner: `sidz`
  - Repository owner: `oragorn`
  - Repository: `mapshaper.net`
  - Workflow file: `ci.yml`
  - Environment: blank
- The `main` branch is passing CI.

## Release Steps

1. Update `CHANGELOG.md`.

   Move the version you are releasing out of `Unreleased` and set the release date:

   ```md
   ## 0.0.2 - 2026-06-15
   ```

2. Commit and push the changelog.

   ```powershell
   git add CHANGELOG.md
   git commit -m "Prepare 0.0.2 release"
   git push origin main
   ```

3. Confirm the latest `main` workflow run passes.

   Open:

   ```text
   https://github.com/oragorn/mapshaper.net/actions
   ```

4. Create and push the version tag.

   ```powershell
   git tag v0.0.2
   git push origin v0.0.2
   ```

5. Confirm the tag workflow run passes.

   The tag workflow will:

   - Restore dependencies
   - Build
   - Run tests
   - Pack `Mapshaper.Net` using the tag version
   - Publish the package to NuGet through Trusted Publishing
   - Create a GitHub Release using the matching `CHANGELOG.md` section
   - Attach the generated `.nupkg` to the GitHub Release

6. Verify the release.

   Check NuGet:

   ```text
   https://www.nuget.org/packages/Mapshaper.Net/0.0.2
   ```

   Check GitHub Releases:

   ```text
   https://github.com/oragorn/mapshaper.net/releases
   ```

## Versioning

The package version is derived from the pushed tag.

Examples:

- `v0.0.2` publishes package version `0.0.2`
- `v1.2.3` publishes package version `1.2.3`

Do not edit the package version in `Mapshaper.Net.csproj` for normal releases.

## If A Release Fails

- If NuGet publishing fails, check the Trusted Publishing policy values.
- If GitHub Release creation fails, check whether a release for the tag already exists.
- If the package already exists on NuGet, increment the version and push a new tag. NuGet package versions cannot be overwritten.
