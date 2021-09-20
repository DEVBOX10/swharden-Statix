# Statix

[![](https://img.shields.io/github/workflow/status/swharden/Statix/build%20and%20test)](https://github.com/swharden/Statix/actions/workflows/build-and-test.yaml)

**Statix is a C# static site generator** designed to create a flat-file websites from folders containing markdown files. Statix is ideal for creating small websites from GitHub repositories.

**How it works:** Statix crawls a directory tree and whenever it finds `index.md` it generates `index.html` according to a `template.html` containing [mustache](https://mustache.github.io) tags.

> **⚠️WARNING:** Statix is pre-alpha and not recommended for use in production

## Features

* ✔️ `![](image.jpg)` becomes a clickable image
* ✔️ `![](YouTubeURL)` becomes an embedded video
* ✔️ `![](TOC)` inserts a table of contents
* ✔️ Anchors are added to headings automatically
* ✔️ Github style tables are supported
* ✔️ Automatic syntax highlighting ([highlight.js](https://highlightjs.org/))
* ✔️ Pages link to their own source code on GitHub
* ❌ Sitemap
* ❌ RSS

## Quickstart

Build the sample website from the console:

```
dotnet run --project src/Statix --content sample/content --theme sample/themes/statixdemo --source https://github.com/swharden/Statix/blob/main/sample/content
```

Use Docker to preview the site locally:
* Install [Docker Desktop](https://www.docker.com/products/docker-desktop) 
* `docker-compose up -d`
* Go to http://localhost:8080

## Deployment

**Statix can build locally**, allowing you to generate a static website from local content and upload it using traditional methods like FTP/SFTP.

**Statix can build in the cloud,** allowing static sites to be generated from GitHub repositories and deployed automatically in response to GitHub actions.

## Similar Projects
* [Palila](https://github.com/swharden/Palila) (Python) 
* [md2html](https://github.com/swharden/md2html-php) (PHP)