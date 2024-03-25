---
layout: post
title: "Quick Note: What you need for modifying Ifc files."
date: 2023-03-12-204910 
categories: 
tags: 
published: true
---
<script src="/assets/signup/signup.js"></script>
<signup-component></signup-component>

Getting tired of re-exporting your Ifc files for submission and exchanges just because of a few tiny changes or corrections in the model? In fact, an Ifc file is not the end product of your BIM planing, but actually a start of you BIM journey. There are many ways to modify Ifc files, aside from re-exporting them. Let me just use this quick note series to document the workflows I found for different topics and leave the details for later.

Here the first topic - modifying Ifc files - and, by far, my personal favorite goes to:

# _Coding in **Python** with **IfcOpenShell**_
Having known [IfcOpenshell](https://ifcopenshell.org/) for a while, not unitl I dig more into it, did I realize how powerful and beautiful when using it with Python and its numerous available modules. Here are the tools one needs:

* [Visual Studio Code](https://code.visualstudio.com/)
* [Python](https://www.python.org/downloads/)
* [vituralenv](https://virtualenv.pypa.io/en/latest/)
* [ifcopenshell](https://blenderbim.org/docs-python/ifcopenshell-python.html)

Personally, I like to put all of the resources together in one folder, in case I forget how it went later on. That's why, for me, the virtual environment of Python together with VS Code create a great playground for coding and for the uncountable trials and errors. Here is the quick note for setting up:

* Create a folder, where you want to gether all for your project.
* Open the folder with VS Code.
* Select your default Python intepreter
* Open a terminal in VS Code, and do:
    - Install virtualenv `pip install virtualenv`
    - Install ifcopenshell `pip install ifcopenshell`
* Create a virtual environment
    - Windows `Python3 -m venv /path/to/venv`
    - Mac `python3 -m venv venv`
* Select Python intepreter form the virtual environment `/path/to/venv/Scripts/Python.exe`
* Activate the virtual environment
    - Windows `/path/to/venv/Scripts/activate`
    - Mac `source /path/to/venv/bin/activate`
* In terminal, run `pip install ifcopenshell`
* Create a `play.py` file for testing
    - `import ifcopenshell`
    - `ifc = ifcopenshell.open(/path/to/ifc)`
    - `print(ifc)`

That's it! Now you can retrieve information and also modify the ifc model. Let's go into some details later on. Until then, there are great examples and documentations for ifcopenshel:
* Like learnig every other programming language, [Hello World](https://blenderbim.org/docs-python/ifcopenshell-python/hello_world.html) is always the best first step.

Stay tuned!
