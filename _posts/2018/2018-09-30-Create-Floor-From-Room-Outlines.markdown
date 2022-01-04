---
layout: post
title: "Create Floor From Room Outlines"
date: 2018-09-30-060438 
categories: RevitExternalCommand
tags: [floor, room, f#, revit api]
published: true
---
<script src="/assets/signup/signup.js"></script>
<signup-component></signup-component>

By using default Revit’s tools, one can create ceiling automatically. However, to create floors, by default, one has to draw the outlines by hand. Although, luckily, we can draw the outlines by selecting the edges from the adjacent elements, sometimes if we are not so lucky, we will probably select the wrong one and it’s time-consuming to erase the last one and select again. It happens, for example, when we have more than one layer of wall elements over each other and the thinner one at front stands over the floor, whereas the inner wall element is outer circumference of the floor element, and its edge is supposed to be selected.

A much useful tool, drawing automatically floors, doesn’t exist yet, maybe one can found something in the forums, but not in the default Revit tools, as far as I know. However, it’s actually rather easy to write one by yourself. The process is simple, just like you can imagine how you draw them by hand: select the edges of the adjacent elements – walls, columns… etc. These elements are mostly room-binding, which means the outline of a floor would be similar in the form of the room, and one just needs to define until which room-binding element the floor is supposed to be constructed – not only in model but also in reality.

In the case above, there could be more than one wall layers, or, technically speaking, more than one room-binding element overlapping, what can we do about it?  In my experience, with this modelling rules, the front layer is mostly a covering of the standing walls and in our project we name their types in a standard way and move them to a certain workset. Which means we can easily select them all at once and make them not room-binding. Then each room will have their circumference extending to the inner standing walls. In this case the outline of the room will have the form of our desired floor.

{% highlight fsharp %}
type CreateFloorByRoom() as this =
  interface IExternalCommand with
    member x.Execute(cdata, msg, elset) =
      let uiapp = cdata.Application
      let uidoc = uiapp.ActiveUIDocument
      let selected = 
        uidoc.Selection.GetElementIds() 
        |> Seq.map uidoc.Document.GetElement 
        |> Seq.filter(
          fun e -> 
            match e with
            | :? Room -> true
            | _ -> false
        ) |> List.ofSeq
      match selected with
      | [] -> 
        msg <- "Select Room(s)"
        Result.Cancelled
      | _ as sel ->
        let t = new Transaction(uidoc.Document, string this)
        t.Start() |> ignore
        sel |> Seq.cast<Room> |> List.ofSeq
        |> List.map(
          fun rm ->
            let curvearray =
              rm.GetBoundarySegments(new SpatialElementBoundaryOptions()) 
              |> Seq.map(
                fun seqSeg ->  
                  let cary = new CurveArray()
                  seqSeg |> Seq.iter(fun sg -> cary.Append (sg.GetCurve()))
                  cary
              ) |> List.ofSeq
            match curvearray with
            | [] -> failwith "Unknown error with the room"
            | [cary] ->
              uidoc.Document.Create.NewFloor(cary, false) |> ignore
              ()
            | hd::tl ->
              let flr = uidoc.Document.Create.NewFloor(hd, false)
              tl
              |> List.iter(fun cary -> uidoc.Document.Create.NewOpening(flr, cary, false) |> ignore)
              ()
        )
        |> ignore
        t.Commit() |> ignore
        Result.Succeeded
{% endhighlight %}

Naturally, the first step of the code is getting the rooms which are selected elements prior to running this command. To secure that it runs only when elements are selected, here we check whether the list of selected elements is empty. If so, a pop-up of error will come up says “Select Room(s)”, terminates the code and gives result as a cancellation (Line 26). If not, it works on the list further by starting a transaction at first(Line 29), and then, after casting all selected elements as rooms, we can retrieve the outlines out of rooms.

The outlines of a room are saved as its boundary segments and available by calling GetBoundarySegments() with an instance of SpatialElementBoundaryOptions. These boundary segments are collected in generic lists within a generic list. Before we are getting into the next step, we should take a look at the structure of these lists.

In the first item of the outer generic list are the segments of the outermost outlines, and the rest items – if there is any – are the inner outlines within a room, where the inner boundaries are. For example, if we have a room or a column with a room, which we want to create a floor element from, then the first item, i.e. the inner list, will be the outermost outlines and the outlines of the inner boundaries and of the column will be the rest items. The segments from the rest items will be the outlines where we “cut out” the floor element, and for doing that with Revit API, we create opening elements with the floor as their host element.

As much of the said, let’s have a short look at the codes. We convert the generic list of boundary segments into an F# list, with which we make pattern comparison that we can be sure or distinguish the different processes in cases of nothing in the list, only one item in the list – which means there is no inner boundary within the room, and more than one item in the outermost list, where we use head and tail pattern to retrieve first and the rest list items.

Hopefully, more or less it introduces a bit of how to automatically create floor elements and leads to an interest of writing automation for Revit. If there is any question or feedback, feel free to leave comments and stay tuned!