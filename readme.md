# Statix

![](https://img.shields.io/github/workflow/status/swharden/Statix/build%20and%20test)

**Statix is a C# static site generator** designed to create a flat-file websites from folders containing markdown files. Statix is ideal for creating small websites from GitHub repositories. Statix is similar to [Palila](https://github.com/swharden/Palila) (Python) and [md2html](https://github.com/swharden/md2html-php) (PHP).

**How it works:** Statix crawls a directory tree and whenever it finds `index.md` it generates `index.html` according to a `template.html` containing [mustache](https://mustache.github.io) tags.

> **⚠️WARNING:** Statix is pre-alpha and not intended to be used by others at this time.

## Quickstart

Build the sample website from the console:

```
dotnet run --project src/Statix --content sample/content --theme sample/themes/statixdemo
```

Use Docker to preview the site locally:
* Install [Docker Desktop](https://www.docker.com/products/docker-desktop) 
* `docker-compose up -d`
* Go to http://localhost:8080

## Deployment

**Statix can run locally**, allowing you to generate a static website from local content and upload it using traditional methods like FTP/SFTP.

**Statix can run in the cloud,** allowing static sites to be built from GitHub repositories and deployed automatically in response to GitHub actions.