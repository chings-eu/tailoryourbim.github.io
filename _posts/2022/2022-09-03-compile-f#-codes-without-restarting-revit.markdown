---
layout: post
title: "Compile F# Codes Without Restarting Revit"
date: 2022-09-03-153706 
categories: RevitExternalCommand
tags: tailor-your-revit
published: true
---
  
**Visual Studio Community** is a great tool, when coding your own <u>Revit add-in</u> command. While typing, you have its <u>IntelliSense</u> helping you by suggesting your next move, so that you will not get lost in the API library jungle. Especially the one from Revit. However, you've always got have to re-compile your whole *dynamic linked library* from the whole solution, and it took you an huge amount of time for waiting Revit to re-start and re-load your extremely heavy model, even if you've just changed a tiny bit of code for debugging. Even if just for one letter. Nevertheless, in this case you have to have all your codes - not only the one you're debugging now - compiling-ready to prevent any error emerging while building it. But sometimes you'll have a few other unfinished codes, and you want to just test this current one really quickly to see if it works or how a certain section of it works.  

I found this in-efficient issue hugely annoying right at beginning of my coding experience for Revit. Since then, I have been working on developing an <u>efficient</u>, <u>dynamic</u> **coding environment** along with an **instant** <u>F#-code-compiler</u>. After daily testing and repeated improvement, I'd like to share with the public: the **Tailor-your-Revit core (tyRCore)**.

<script src="/assets/signup/signup.js"></script>
<signup-component></signup-component>

# _Coding **Environment**_
As mentioned, the advantage that we <u>definitely</u> should take from Visual Studio Community is its IntelliSense, and this is the reason that this compiler should be built upon with a <u>matching</u> solution folder structure. Here are the steps for setting up your _efficient, dynamic and instant_ coding environment to your Revit:

* _Download and install **Visual Studio Community**_  
This is quite straight forward - just go to this [page][Get Visual Studio Community] for Visual Studio and get the **community** version, for Windows or Mac, and install it with those <u>packages</u>, relevant to developing F# desktop programs.

* _Create an **F# solution**_  
Follow the [other post][Set up F# solution] for a step-by-step tutorial to set up an F# solution. For working with **tyRCore**, you just need additionally set up a folder collecting your codes with a <u>strucutre</u> as mentioned [below](#structure), or directly fork me on [GitHub][Using_tyRCore] to get a template with all the needed setups for Visual Studio solution to work with tyRCore. Without further setup, this repository can be immediately loaded into Revit through _tyRCore_ add-in.

* _Download and install **tyRCode**_  
Download tyRCore [here][Setup tyRCore] or from Autodesk App Store. The installation is straight forward. However, if you're not sure if you'd do it right, just follow the _**README**_ section in this [repository][Using_tyRCore] from above.

# _**Loading** Codes_  
This is a <u>trial version</u> of **tyRCore** with simple interface layouts and automatic toolbar setups in the Revit environment. However, regardless of its work-in-progress, it's a version which serves the basic needs, as I work with it everyday in Revit and it saved me huge amount of time from inefficient restarts while debugging, and I could then produce a lot more within a narrow time frame.  

After installing tyRCore into Revit, you'll find these <u>two new features</u> added onto the tool ribbon: 

* A new _PushButton_ **Load F# Codes** in the external tools from _Add-ins_ tab, and
<p align="center">
    <img src="/assets/img/2022/220829_01_Button in Add-Ins Tab.png" style="width:80%;">
</p>

* A new _RibbonTab_ named **tyR Core** on the ribbon. 
<p align="center">
    <img src="/assets/img/2022/220829_02_Button in tyR Tab.png" style="width:80%;">
</p>

# _**Structure**_  
Both of these two buttons are calling the same command (i.e. same source code) and have the loading _PushButton_ "Load Your FSharp Codes". With a click on either of them, you'll be asked to pick the folder with your F# codes and load them into Revit ribbon, according to your folder structure. The difference between the two buttons is just that, you can only call this command without an open document, if it's registered standalone as an external command. For demonstration purpose, I'll just take our pre-set F# solution <u>Using_tyRCore</u> as an example:

* The name of the upper directory _Using_tyRCore_ from your choosen folder **cmd** will be the name of the *<u>ribbon tab</u>* (It's also the name of your project in Visual studio.)
* The names of **sub-folders** will be mapped with formats, either _RibbonPanel_ or _RibbonPanel.PullDownButton_. (e.g. <u>modify</u> or <u>modify.parameter</u>)
* In the sub-folders, you saved your F# source codes, and each of them will be connect to a _PushButton_ and put under a _RibbonPanel_ or within a _PullDownButton_ respectively.

By the way, **tyRCore** will also load automatically all those source codes, which are located in the folders, whose names starting with <u>"standalone"</u>, additionally as standalone external commands, which can be found in the "Add-Ins" tab, "External" panel. They can be executed without opening any Revit documents at first. This comes in handy, if you code some commands which should run _"formlessly"_.

# _**Furthermore**_
This was an introduction to the tyRCore, a great starting point for you to build a comprehensive, structured <u>Revit external commands library</u> step-by-step for your own workflow. I'll cover more about, how to put it into action, later on, and you'll see how this will change your experience with **coding in BIM**!

I'll be grateful to receive some feedbacks via [e-mail][Contact] from you to improve **tyRCore**!  
And if you like it, buy me a coffee by following a click on the coffee cup on the right-hand side corner of this page. 

Until then, dig in and have fun!


[Set up F# solution]: ../Start-Up-an-FSharp-Solution-for-Revit-Plug-In
[Get Visual Studio Community]: https://visualstudio.microsoft.com/
[Using_tyRCore]: https://github.com/chings-eu/Using_tyRCore
[Setup tyRCore]: ../../downloads/tyR22Core_v1.0.exe
[Contact]: mailto:info@tailoryourbim.com