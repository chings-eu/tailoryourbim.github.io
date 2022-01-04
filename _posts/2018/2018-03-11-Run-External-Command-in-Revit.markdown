---
layout: post
title: "Run External Command in Revit | 加入Revit外部指令"
date: 2018-03-11-224652 
categories: RevitExternalCommand
tags: [f#, revit api, external command]
published: true
---
<script src="/assets/signup/signup.js"></script>
<signup-component></signup-component>

After we’ve created a simple message command, we can now register the compiled dynamic-link library file (.dll) into Revit program.

For doing that, we have to create an add-in file.

{% highlight xml %}
<?xml version=”1.0″ encoding=”utf-8″?>
<RevitAddIns>
<AddIn Type=”Command”>
{% endhighlight %}

Second, where the assembly(.dll file), which we have compiled, is, and to which this add-in is linked. For the convenience of debugging, I’d directly point this add-in file to the one .dll which is in the debug directory, re-built from Visual Studio program. 

{% highlight xml %}
    <Assembly>fs-revit.dll</Assembly>
{% endhighlight %}

Two important features as the following code are the FullClassName and AddInId tags. The former can be found by hovering your mouse cursor over the line in Visual Studio where type/class is declared and it is usually formed as “namespace.classname”. As to the later one, “AddInId”, it can be online generated, and each add-in file pointing to your Revit program, the assembly, must have a unique id number. 

{% highlight xml %}
    <FullClassName>TYB.Tutorial.FSRevit.ASimpleMessage</FullClassName>
    <AddInId>04a6882f-6d70-408b-b9bb-c7391c733a5f</AddInId>
{% endhighlight %}

And then, a few essential attribute tags describing the command and the .dll file to register. More information about the add-in tags can be found by following the registration link above. 

{% highlight xml %}
    <Text>Level01: Revit Returns A Simple Message</Text>
    <VendorId>TYB</VendorId>
    <VendorDescription>tailoryourbim.com</VendorDescription>
    <VisibilityMode>AlwaysVisible</VisibilityMode>
    <Discipline>Any</Discipline>
{% endhighlight %}

Finally, the tail tags for closing up the xml file. 

{% highlight xml %}
</AddIn>
</RevitAddIns>
{% endhighlight %}

Now save this file to the add-in folder, which usually is located at:

_C:\Users\UserName\AppData\Roaming\Autodesk\Revit\Addins\2017_

After the file is copying into this folder, if your Revit is running, a dialog window will pop-up asking if the add-in is to load, if not, start Revit and the pop-up will show up. After loading the add-in you can find the command button located in external command panel within the Add-In tab.