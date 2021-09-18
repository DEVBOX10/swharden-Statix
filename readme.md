# Statix

**Statix is a C# static site generator** designed to create a flat-file websites from folders containing markdown files. Statix is ideal for creating small websites from GitHub repositories. Statix is similar to [Palila](https://github.com/swharden/Palila) (Python) and [md2html](https://github.com/swharden/md2html-php) (PHP).

**How it works:** Statix crawls a directory tree and whenever it finds `index.md` it generates `index.html` according to `template.html` containing [mustache](https://mustache.github.io) tags.


> **⚠️WARNING:** Statix is early in development and not intended to be used by others at this time