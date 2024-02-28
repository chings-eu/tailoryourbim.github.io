---
layout: post
title: "How to Investigate Your Ifc Model with IfcOpenShell and Jupyter Notebook"
date: 2024-02-20-073633 
categories: 
tags: Python IfcOpenShell Ifc Jupyter Pandas
published: true
---
<script src="/assets/signup/signup.js"></script>
<signup-component></signup-component>

Once you've set up your Python virtual environment by following this post, you can start to test and play around with you codes, without worrying wasting time on worrying if installed packages would get mixed up with other Python projects. Since now you have this one folder with all you needs for your Ifc model at your disposal.

Let's dig in to see how to use it efficiently.

## Run you code step by step

Sometimes your Ifc file could be heavy for loading. You'll also have to load Ifc file with IfcOpenShell, and only after that, the data is ready for you. It could be very time consuming to load your Ifc model repeatedly, every time you update your codes.

That's where the Jupyter comes in.

Jupyter lets you split your Python codes into sections, and you can run the sections separately. The great thing is, that you can modify your code in a section and instantly re-run it without repeatedly running something from above again and again. But, of course, the variables that you're using in the current cell must be run before hand from above.

![alt text](image.png)

## Visualize and manipulate your data with Pandas

<iframe width="768" height="432" src="https://miro.com/app/live-embed/uXjVNpgQ2-Q=/?moveToViewport=-1793,-156,2174,1364&embedId=600564706540" frameborder="0" scrolling="no" allow="fullscreen; clipboard-read; clipboard-write" allowfullscreen></iframe>