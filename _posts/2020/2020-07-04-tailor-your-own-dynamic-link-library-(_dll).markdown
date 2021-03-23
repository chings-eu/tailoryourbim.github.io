---
layout: post
title: "Tailor your Own Dynamic-link Library (.dll)"
date: 2020-07-04-222137 
categories: Application
tags: [core, ui, f#, dll, load]
published: false
---

I have thought about it for awhile to create an external application, which can load arbitrary dynamic-link libraries - known as its extension ".dll" - which is compiled with a handful external commands and the application can follow a certain rule to load each external command into ribbon panels, neatly categorized under pulldown buttons.

Having able to manage some time to do this, I've got to finally create a first version of it. It is also very easy to use. Here are the installation processes, demonstrated as the following: 



It's not perfect yet and needs improvement and your feedbacks. Want to give it a try out? [Here][1] is it, the zipped program available to download.

[1]:{{ site.url }}/downloads/tailoryourowndll.zip