---
layout: post
title: "Rhino Plug-in with F# | 用F#寫Rhino外掛程式"
date: 2018-11-14-165141 
categories: 
tags: 
published: true
---
A few years back, when I was writing my thesis, titled “Permeation”, about philosophy, architectural geometry and programming, RhinoCommon, Grasshopper and Python were the great helpers to bring my ideas into the light. Since then, they are essential parts for my work along with Revit. When F# came to my life, it brought my understanding of programming further into a broader view and another level.

> F# is a mature, open source, cross-platform, functional-first programming language. It empowers users and organizations to tackle complex computing problems with simple, maintainable and robust code. -[F# Software Foundation](http://fsharp.org)

Parallel to introductions about writing external commands for Revit with Revit API in F# or Python, I’d like to share writing commands for Rhino with RhinoCommon in both languages as well.

Let’s start with adapting a Python code example from the Rhino Developer Docs samples: Custom Getpoint. We will build a dynamic-link library (.dll) as plug-in for the newest version Rhino 6, with its RhinoCommon.dll and Rhino.UI.dll as References. For building an dll file, we can use Visual Studio Community as coding environment, and you can find a similar post about setting it up. However, the difference is just instead of references from Revit, we’ll have to use the ones from Rhino 6. 

{% highlight sharp %}
namespace TYB.RHN.DOES
type PlugIn() =
  class
    inherit Rhino.PlugIns.PlugIn()
  end
{% endhighlight %}

Just like coding with Revit API – as mentioned in this previous post, we have to set the TransactionAttribute and using IExternalCommand interface for each external command – and here we’ll have to write, for RhinoCommon, at least, two types with similar setups – first, a type inheriting PlugIn as base class (code above) and, second, a type inheriting Command as base class (code below). We can have many of the types that inherit Command class, but we must have one type which inherits the PlugIn class within one project, i. e.  within one dynamic-link library (.dll) file.

{% highlight fsharp %}
namespace TYB.RHN.Does
open Rhino
open Rhino.Geometry
open Rhino.Input
type DrawDynamicLines() as this =
  inherit Rhino.Commands.Command()
  override x.EnglishName = string this
  override x.RunCommand(doc, mode) =
    RhinoApp.WriteLine(string this)
    let gp0 = RhinoGet.GetPoint("Pick a point", false)
    let gp1 = RhinoGet.GetPoint("Pick 2nd point", true)
    let points =
      match gp0, gp1 with
      | (_, pt0), (_, pt1) ->
        [pt0; pt1]
    let gp2 = new Rhino.Input.Custom.GetPoint()
    gp2.DynamicDraw.Add(
      fun arg ->
        arg.Display.DrawLine(points.[0], arg.CurrentPoint, System.Drawing.Color.AliceBlue)
        arg.Display.DrawLine(points.[1], arg.CurrentPoint, System.Drawing.Color.HotPink)
    )
    gp2.Get()
    doc.Views.Redraw()
    Commands.Result.Success 
{% endhighlight %}

Just as I felt, as I slowly learnt how to write in F# with the understanding of Python, translating from Python code to F# is easy. The most different part might be to declare a type, to inherit a class and override the name of this command and the RunCommand() method. Since I didn’t have to do those. If you’re using Python editor or writing Python script in Grasshopper, these processes are not necessary, since these are already taken care of.

The process of this code is clear and similar to the one in Python. By overriding the EnglishName, we can set the name of this command to be called in Rhino command line and under RunCommand we implement what this code is actually doing. By using RhinoGet, the user enter the two points on screen and the third one we set for the CurrentPoint, where your mouse cursor is located, from the event argument of DynamicDraw event.

Finally, build this solution in Visual Studio, you’ll have a dynamic-link library file (.dll). In Rhino 6 under Option >> Plug-in, you can import this command and call it from the command line.