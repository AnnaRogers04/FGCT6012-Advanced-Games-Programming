# Week 2 | Setup

Every time I push my code, GitHub builds my docs and updates my website for me. This page explains how that works.

## How the workflow works

The workflow is the file `.github/workflows/pages.yml`. It runs every time I push to `main`. I can also run it myself from the Actions tab.

It does two jobs, one after the other.

1. It grabs a copy of my repo.
2. It installs Doxygen.
3. It runs `doxygen Doxyfile`, which reads my C# script and turns the comments into a docs website.
4. It puts my pages, css, js, src and assets folders into a folder called `_site`, and copies the docs into `_site/docs`.
5. It packs up `_site` ready to publish.


If the build worked, this job publishes that folder to GitHub Pages.

## How I turned on GitHub Pages

1. Go to the repo on GitHub and click **Settings**.
2. Click **Pages** on the left.
3. Under **Build and deployment**, set **Source** to **GitHub Actions**.
4. Push a commit to `main` and the workflow does the rest.

I didn't have to pick a branch or folder. If you have an old Pages workflow in the repo (mine was `static.yml`), delete or rename it so it doesn't clash.

## Where the docs show up

- Site: https://annarogers04.github.io/Portfolio/
- Docs: https://annarogers04.github.io/Portfolio/docs/

There's also a **Docs** link in the nav bar on every page. The docs are rebuilt every push, so they always match my latest code.

## If something goes wrong

Open the **Actions** tab and click the latest run. A green tick means it worked. A red cross means a step failed, and clicking it shows why.

- **Docs page says 404:** the run probably failed, or the docs weren't copied into `_site/docs`.
- **Deploy fails:** check Settings > Pages > Source is set to GitHub Actions.
- **Workflow never runs:** make sure the file is in `.github/workflows/` and you pushed to `main`.
