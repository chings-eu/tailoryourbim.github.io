---
layout: post
title: "Synchronize Section Box"
date: 2018-10-08-075251 
categories: RevitExternalCommand
tags: [f#, revit api, section box, sync]
published: true
---

<a href="../../downloads/tailoryourbim_demo.zip" download>Here</a> to download the compiled Dynamic-Linked Library(.dll)

When modeling in Revit, we set up a 3D view properly for our reviewing of the collisions in models to be easily recognizable and efficiently modified, and we would like to jump from one issue to the next quickly, we save the settings in the view template for the similar reviewing attentions.

However, the display of the model can easily be saved in template, but the location of the issues can be shown by modifying the section box in the 3d view. by hand. This can be extremely inefficient when dealing with multiple issues with scattered locations. For example, according to my experience, especially when working on the BCFs (Building Collaboration Format). When opening a issue, you will get a default 3d view opened, and the objects that might have collision are shown in the scene. However, the display of this default window might not be the one you want, especially when many overlapping objects are in the scene, you might wish you have your own view open, where you have setup the display.

If you are going to open your own view, every time you open one issue, and then fit the section box in your own view to the one from the BCF, it will be already dark outside your window, since each time the model has been checked, there might be hundreds of new issues coming, and you will be wasting too much time on just adjusting the views.

I don’t know if there is any tool for synchronizing section boxes between more than two views out there or already in the new version of Revit. But with F# and Revit API we can have the opened 3d views synchronized with current active view.

{% highlight fsharp %}
type SyncSectionBox() as this =
  interface IExternalCommand with
    member x.Execute(cdata, msg, elset) =
      let uidoc = cdata.Application.ActiveUIDocument
      let vAct = uidoc.ActiveView
      let bbxAct = (vAct:?>View3D).GetSectionBox()
      let uiv3ds = 
        uidoc.GetOpenUIViews()
        |> Seq.filter(fun uiv -> uiv.ViewId <> vAct.Id)
        |> Seq.filter(
          fun uiv -> 
          let view = uiv.ViewId |> uidoc.Document.GetElement
          match view with
          | :? View3D -> true
          | _ -> false
        ) |> List.ofSeq
      match uiv3ds with
      | [] ->
        msg <- "No other opened 3d view to sync."
        Result.Cancelled
      | _ ->
        let t = new Transaction(uidoc.Document, string this)
        t.Start() |> ignore
        uiv3ds
        |> Seq.iter(
          fun uiv ->
            let view = uiv.ViewId |> uidoc.Document.GetElement
            (view:?>View3D).SetSectionBox(bbxAct)
            uiv.ZoomToFit()
        )
        t.Commit() |> ignore
        Result.Succeeded
{% endhighlight %}

It is just a short code, but, I think, it can help shortening our time for finding the right view when jumping from spot to spot. The concept is simple: just a manipulation of section boxes in 3d views. First, find the section box of the current 3d view (line 11). Section boxes are in a bounding boxes, which is a rectangular box and has its maximum and minimum points. Second, find the opened UI-views (line 13) which we want to apply this section box into. At this step, we filter out first the active view, since we don’t have to synchronize its section box with itself, and then we filter again and get rid of those views which are not 3d (line 15).

Then it’s the main body of the command. We match the list of opened UI-views, which are filtered. If it is empty, then there is nothing to sync, and we response with message to tell user to open other 3d views. Else, we open a transaction to be ready to apply section box to views (line 27), since we are going to modify the model information. The rest is one iteration of the 3d view list – for each of the view we use the SetSectionBox method to apply the section box to it. However we have to note that the downcast of the view as an element to view is necessary (line 33), just like how we get the section box at first (line 11), otherwise these two methods will not be available. The final step is just for a convenient modeling work-flow, to zoom the UI-view, with the new section box, to fit the screen, or we might see nothing after the view manipulation, since the next spot could be far away.

Stay tuned!