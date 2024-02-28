---
layout: post
title: "Start Coding F# Library for Revit | 開始編程 Revit 的 F# 指令集"
date: 2017-10-07-212312 
categories: RevitExternalCommand
tags: [f#, revit api]
published: true
---
<script src="/assets/signup/signup.js"></script>
<signup-component></signup-component>

Let's just jump right in to having the first look of the essential of coding a command as  Dynamic-linked library (.dll) for Autodesk Revit.  

To write a Revit plug-in, the first step is to set the transaction mode to manual:

{% highlight fsharp %}
[<TransactionAttribute(TransactionMode.Manual)>]
{% endhighlight %}

As each of the plugins is an external command, each command class is built with the interface of IExternalCommand:

{% highlight fsharp %}
type MyCommand(args) =
  interface IExternalCommand with
{% endhighlight %}

To run the command, the execute methode is to overload with arguments: CommandData, Message and Elements. 

{% highlight fsharp %}
  member this.Execute(cdata: CommandData, msg: string, elm: ElementSet) =
{% endhighlight %}

Then goes the content of the command, until the end of the code, and it must be a result returned:

{% highlight fsharp %}
    Result.Succeeded
{% endhighlight %}

To build the command into a dynamic-link library (.dll), an confortable way is to set up a F# library with Visual Studio Community. With the interface of Visual Studio, it is easy to set up the correct environment and to have the neccesary references for building up an assembly, to be deployed in Revit, which is based on the .NET Framework.  

Stay tuned...
