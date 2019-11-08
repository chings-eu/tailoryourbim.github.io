---
layout: post
title: "A Simple Message Command | 簡單的訊息指令"
date: 2018-01-02-223053 
categories: RevitExternalCommand
tags: [f#, revit api]
published: true
---

Last time, we have introduced the basic structure of an external Revit command. To test it, let’s complete it by writing a simple “Hello World!” command (just like every other beginner’s first code is supposed to be), which, when executed, will return a message by using Revit’s built-in UI-command “TaskDialog“. 

{% highlight fsharp %}
[<TransactionAttribute(TransactionMode.Manual)>]
type ASimpleMessage() =
  interface IExternalCommand with
    member x.Execute(cdata, msg, elset) =
      TaskDialog.Show("Title", "Hello World") |> ignore
      Result.Succeeded
{% endhighlight %}

Surely, don’t forget to declare the namespace of this library at the beginning of the F# source file(.fs). 

{% highlight fsharp %}
namespace TYB.Tutorial.Wordpress
{% endhighlight %}

And also the modules of Revit which we use here – by default if you use Microsoft Visual Studio for coding, it will suggest you which of the reference that has been used in your code, and put it into the correct position for you. (See the post regarding setting coding environment) 

{% highlight fsharp %}
open Autodesk.Revit.Attributes
open Autodesk.Revit.UI
{% endhighlight %}

An external command is just as simple as this is! The code, which you want to tell Revit to do just like what you want, comes right into line number 5, between the “member” definition until the final “Result” output. In this case we have a simple call for a “TaskDialog” from Revit’s UI namespace and a dialog window will pop-up when you hit the command button.

This is a simple introduction of the common coding structure (what we want to do comes in the line #5) for an external command in Revit. To complete compiling the command as a dynamic-link library file (.dll), Microsoft Visual Studio comes in handy. However, before getting into the compilation of a code, let’s take a look at the add-in file for loading an external command – the .dll file – into Revit environment.

More is to come…