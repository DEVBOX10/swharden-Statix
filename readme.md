# Statix

[![](https://img.shields.io/github/workflow/status/swharden/Statix/CI)](https://github.com/swharden/Statix/actions/workflows/ci.yaml)

**Statix is a C# static site generator** designed to create a flat-file websites from folders containing markdown files. Statix is ideal for creating small websites from GitHub repositories.

**How it works:** Statix crawls a directory tree and whenever it finds `index.md` it generates `index.html` according to a `template.html` containing [mustache](https://mustache.github.io) tags.

**Deploy with GitHub Actions:** [build-test-deploy.yaml](.github/workflows/build-test-deploy.yaml) builds the project, runs the tests, generates the website, and uploads it to the web. Actions for this project demonstrate how to upload to Azure Blob Storage and to a traditional website with FTP.

**Similar projects:** [Palila](https://github.com/swharden/Palila) (Python) and [md2html](https://github.com/swharden/md2html-php) (PHP)

> **WARNING:** Statix is pre-alpha and not recommended for production use

## Features

* ✔️ `![](image.jpg)` becomes a clickable image
* ✔️ `![](YouTubeURL)` becomes an embedded video
* ✔️ `![](TOC)` inserts a table of contents
* ✔️ Anchors are added to headings automatically
* ✔️ Github style tables are supported
* ✔️ Automatic syntax highlighting ([highlight.js](https://highlightjs.org/))
* ✔️ Pages link to their own source code on GitHub
* ✔️ Automated deployment with GitHub Actions
* ❌ Sitemap
* ❌ RSS

## Quickstart

Statix is a console application that accepts command line arguments to indicate folder paths and URLs. 

```
dotnet run --project ./src/Statix
```

### Build and Serve Locally
* Install [Docker Desktop](https://www.docker.com/products/docker-desktop)
* `docker-compose up -d`
* [`build.bat`](build.bat) generates a local site with relative URLs
* Go to http://localhost:8080

### Build and Deploy with GitHub Actions
* Refer to [`build-test-deploy.yaml`](.github/workflows/build-test-deploy.yaml) 
  * Absolute URLs are required (achieved by defining `<base>` in HTML)
  * Example deployment in Azure Blob Storage: https://statix.z20.web.core.windows.net
  * Example deployment on a traditional web server: https://swharden.com/software/statix