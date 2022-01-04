---
layout: post
title: "UI tailoryourrevit"
date: 2019-09-14-061324 
categories: Concept
tags: [core, ui, f#, python, thoughts]
published: false
---
<script src="/assets/signup/signup.js"></script>
<signup-component></signup-component>

Though people told me that no one really want to program by their own for creating their own external tools for improving their daily works, I still believe that's just the current situation. Think about the situation back 20 years agos, nobody would believe that smart phones will be everywhere, people even get to _customize_ their own devices by their own will. So what about thinking 20 years ahead? Remember, now even in promary schools, they are teaching children more about computers, and already about how to program.  

Back to the days when I was studying, in the university there were just a handful of people who digged deeply in the coding world and made something for their thesis. But nowaday there are more, not everyone though. However imagine, when they graduate, get jobs in the architect's offices, how many there would be, who can use coding helping their daily work!  

The interface of <span style="color:#33adff"> **tailoryourrevit** </span>, I'll call it _**tyrUI.exe**_, what I thought to develop, is simple. For example, I have now the <span style="color:#33adff"> **tailoryourrevit core** </span> (_**tyrCore.exe**_), which compiles an F# code on the go, when users press the botton, and after he/she modifies the code and save it, the command will simply run again instantly compiled.  

The interface is just a visualized coding pad. But not like Grasshoppper or Dynamo where people use connection with many small command buttons to run a whole function. Instead, with the thoughts on functional programming and taking the advantage of the structure of F#, we can make the coding pad more human friendly, make the pipelines to run small coding functions. When a user drags a block, say, select, he/she has just to fill the blank, what of a _<u>BuiltInCategory</u>_ is wanted, the canvas will export to a code text, save it as _.fs_, and finally run it through the core, and then, the command is running.  

This makes me think, why not just seperate the UI from the core? As a totally different thing? This can also be using to export a text as _Python_ script (_.py_), since I can write and have written a code to run Python code through IronPython with F# and run it with tailoryourrevit Core.