# <img src="dev/icon/icon.png" height="24" width="24"> Statix

[![](https://img.shields.io/github/workflow/status/swharden/Statix/CI)](https://github.com/swharden/Statix/actions/workflows/ci.yaml)

**Statix is a C# static site generator** designed to create a flat-file websites from folders containing markdown files. Statix is ideal for creating small websites from GitHub repositories.

**How it works:** Statix crawls a directory tree and whenever it finds `index.md` it generates `index.html` according to HTML templates in the themes folder. The resulting site uses folder names as URLs.

> **WARNING:** Statix is pre-alpha and not recommended for production use

## Features

* `![](image.jpg)` becomes a clickable image
* `![](YouTubeURL)` becomes an embedded video
* `![](TOC)` inserts a table of contents
* Anchors are added to headings automatically
* Github style tables are supported
* Syntax highlighting with [highlight.js](https://highlightjs.org/)
* Pages link to their own source code on GitHub
* Automated deployment with GitHub Actions

## Quickstart

```
dotnet run --project ./src/Statix
```

## Build and Deploy with GitHub Actions
* **Usage:** Refer to [`ci.yaml`](ci.yaml)
* **Example:** Azure Blob Storage: https://statix.z20.web.core.windows.net
* **Example:** Traditional web server: https://swharden.com/software/statix

## Build and Serve Locally
* Set Up Docker
  * Install [Docker Desktop](https://www.docker.com/products/docker-desktop)
  * `docker-compose up -d`
* Build the Site
  * Run [build.bat](build.bat)
  * Go to http://localhost:8080