# Statix

**Statix is a C# static site generator** designed to create a flat-file websites from folders containing markdown files. Statix is ideal for creating small websites from GitHub repositories.

**How it works:** Statix crawls a directory tree and whenever it finds `index.md` it generates `index.html` according to `template.html` containing [mustache](https://mustache.github.io) tags.


> **⚠️WARNING:** This project is early in development and only intended to be used by the original author at this time