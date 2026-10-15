# Week 2 | Portfolio

# Links

Live Site: [https://annarogers04.github.io/Portfolio/](https://annarogers04.github.io/Portfolio/)

Docs: [https://annarogers04.github.io/Portfolio/docs/](https://annarogers04.github.io/Portfolio/docs/)

## What this is
This is my portfolio page to show who I am and what ive made. In this page I promote my indie name Lunar Lynx Games. Ive created a brief about me page so you can see my intrests and what games I want to make. Included is also a projects page that links to my itch so you can play the games ive made as well as a docs and contact me page.

On my docs page is a code snippet of a bell curve from a game I made in unity. This game is not currently promoted on my itch beacuse its unfinished.

## How to Automate docs


1. Install [Doxygen](https://www.doxygen.nl/download.html).
2. Create a new folder anywhere on your computer. You can call it whatever you like, for example `my-docs`.
3. Inside that folder, create another folder called exactly `src`, and put your code file (or files) inside it. See below for which languages work.
4. In the main folder (next to `src`, not inside it), create a file called `Doxyfile` with no extension and paste in this:

```
PROJECT_NAME         = "My Code Docs"
INPUT                = src
RECURSIVE            = YES
FILE_PATTERNS        = *.c *.cpp *.h *.hpp *.cs *.java *.py *.php *.js
# Delete the next line if your code is C or C++
OPTIMIZE_OUTPUT_JAVA = YES
EXTRACT_ALL          = YES
EXTRACT_PRIVATE      = YES
HAVE_DOT             = NO
CLASS_GRAPH          = NO
COLLABORATION_GRAPH  = NO
OUTPUT_DIRECTORY     = doxygen-out
GENERATE_HTML        = YES
GENERATE_LATEX       = NO
```

5. Open a terminal in that folder and run:

       doxygen Doxyfile

6. Open `doxygen-out/html/index.html` in a browser to see the docs.

### Which code works

Doxygen can read C, C++, C#, Objective-C, Java, Python, PHP and JavaScript, plus a few less common ones like Fortran and VHDL. JavaScript support is more basic than the others. It can't read Rust, Go, Swift, Kotlin, Ruby or TypeScript without extra add-on tools, so code in those languages will give you empty docs.

Your code also needs documentation comments in Doxygen style, such as `/** ... */` or `///` in C-style languages, or `"""docstrings"""` in Python. Ordinary `//` comments are ignored, so a file with no special comments will only show names with no descriptions.

## How to automate via Gitpages