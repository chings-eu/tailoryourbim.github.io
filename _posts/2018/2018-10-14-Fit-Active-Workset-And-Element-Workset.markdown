---
layout: post
title: "Fit Active Workset And Element Workset"
date: 2018-10-14-080251 
categories: RevitExternalCommand
tags: [f#, revit api, workset]
published: true
---
<script src="/assets/signup/signup.js"></script>
<signup-component></signup-component>

It has been said that using Worksets in Revit is like using layers, analog to AutoCAD or Rhino. Apart from that, working with a work-shared model and manipulating the worksets, either quickly switching / activating between worksets or assigning elements to worksets, can simply improve our working efficiency – at least the additional possibility to change the visibility of each workset in views is already an improvement.

Even this improvement can be further improved. Imagine we’d have to change the active workset often, in order to put the newly created element into correct “layers” – i.e. worksets. Every time you switch the workset, your mouse cursor have to move down the screen and activate the workset you want then you can create elements. This moving up and down frequently actually costs time and your wrist. Why not make your own command to switch and assign between workset more efficiently?

{% highlight fsharp %}
type FitWorkset() as this =
  interface IExternalCommand with
    member x.Execute(cdata, msg, elset) =
      let uidoc = cdata.Application.ActiveUIDocument
      let selected = uidoc.Selection.GetElementIds() |> Seq.map uidoc.Document.GetElement |> List.ofSeq
      match uidoc.Document.IsWorkshared with
      | false ->
        msg <- "File is not workshared."
        Result.Cancelled
      | true ->
        let namWsTo = "02"
        let col = new FilteredWorksetCollector(uidoc.Document)
        let ws = col.ToWorksets() |> Seq.filter(fun w -> namWsTo = Array.item 0 (w.Name.Split '_')) |> List.ofSeq
        match ws with
        | [] ->
          msg <- "No such keyword of workset"
          Result.Cancelled
        | [ws] ->
          let tblWorkset = uidoc.Document.GetWorksetTable()
          match selected with
          | [] -> 
            tblWorkset.SetActiveWorksetId(ws.Id)
          | _ ->
            let bip = BuiltInParameter.ELEM_PARTITION_PARAM
            let t = new Transaction(uidoc.Document, string this)
            t.Start() |> ignore
            selected |> List.iter(fun e -> (e.get_Parameter(bip)).Set(ws.Id.IntegerValue) |> ignore)
            t.Commit() |> ignore
          Result.Succeeded
          | _ ->
            msg <- "Keyword refer to more than one workset"
            Result.Cancelled
{% endhighlight %}

The concept of this code is, firstly, checking if the document is work-shared, if not then the command is cancelled since it has no worksets created by users; and secondly, checking if any elements are pre-selected. After the first checking, we know the file is work-shared and we’ll find the workset, which we want to activate or assign to the elements, at first (line 18). If the workset with the keyword is not available and the list will contain nothing, the program returns an error message (line 21) and is cancelled. Otherwise, the program is split further into two cases: at one hand, if there is only one workset left at the end of filtering workset lists according to given keyword (line 23), then this workset will be further operated, otherwise in the rest of the cases, meaning more than one workset returned, the program will be terminated with cancellation and replying with error message (line 36). How the chosen workset is being operated depends on if there is any element selected prior to the starting point of running this program. If so, it is implied that these elements are meant to be assigned with the chosen workset, but if nothing is selected which implies we just want the chosen workset to be active. Assigning the active workset is by retrieving the workset table and using SetActiveWorksetId() method without initialization of a new transaction, which is contrary to assign workset parameter to elements by setting with the IntergerValue of the chosen workset’s id.

To be continued…