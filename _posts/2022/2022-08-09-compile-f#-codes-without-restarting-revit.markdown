---
layout: post
title: "Compile F# Codes Without Restarting Revit"
date: 2022-08-09-153706 
categories: 
tags: tailor-your-revit
published: false
---
  
**Visual Studio Community** is a great tool, when coding your own Revit add-in command. While typing, you have its intellisense helping you by suggesting your next move, so that you will not get lost in the jungle API library. Especially the Revit one. However you've got always have to re-compile your whole *dynamic linked library* from the whole solution, and it took you an amount of time for waiting Revit to re-start and re-load your extremely heavy model, even if you've just changed a tiny bit of code. Even if just for one letter. Nevertheless, in this case you have to have all your code compiling-ready to prevent any error emerging while building it. But sometimes you have a few other unfinished codes, and you want to just test this one really quickly to see if it works or to retrieve some parameter information.  

I found this huge in-efficient issue annoying right at beginning of my coding for Revit. Since then, I have been working on developing an efficient, dynamic **coding environment** along with an instant F#-code-compiler: **Tailor-your-Revit core (tyRCore)**.

<script src="/assets/signup/signup.js"></script>
<signup-component></signup-component>

## Coding Environment
As mentioned, the advantage that we should take from Visual Studio Community is its IntelliSense, and this is the reason that this compiler should be built upon its solution structure. Here are the steps for setting up your efficient, dynamic coding environment to your Revit:

# Download and install **tyRCode.dll** to Revit add-in folder as an external application.

# Download and install **Visual Studio Community**
This is quite straight forward - just go to the link above and download the community version of Visual Studio, and install all of those packages, relevant to developing F# programs.
# Create an **F# solution**

# Set-up a **folder structure** for holding commands, e.g. F# source codes (.fs)
The external application **tyRCode.dll** has been  

