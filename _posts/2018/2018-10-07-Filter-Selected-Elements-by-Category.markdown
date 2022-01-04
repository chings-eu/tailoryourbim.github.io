---
layout: post
title: "Filter Selected Elements by Category"
date: 2018-10-07-062319 
categories: RevitExternalCommand
tags: [f#, revit api, filter, element, category]
published: true
---
<script src="/assets/signup/signup.js"></script>
<signup-component></signup-component>

When a model is at its beginning building phase, the drawn element are just a few, it’s easy to select them by picking in the views. However, with the model getting more complex, it could be quite a lot of elements overlapping each other just at a corner of the building. Now it could be hard to try to just select a group of elements of targeted categories quickly. Certainly, we can select all of them at once by picking them up with a rectangle all at once, then go to the multi-select tab, filter them, un-check those categories which you don’t want, and then hit OK button.

Can it even be faster?

Yes, a little faster we can do is by adding a shortcut for the filter command, and after we select the whole bunch of elements and hit the shortcut keys, we have the selection window popping up in front of us. However, doing this much faster will not be possible since we will have a progress threshold that the machine has to know which category we want to choose from these elements with this pop-up and before we have selected them, it will not know what are those categories to shown in order to know which one to filter. This makes this process with a break and I think, the most of the time we wasted by drawing is at moving the cursor between buttons, check-boxes or windows.

How can we improve this and make it even faster? Think about the process to reach our goal. Select – give category – filter. The only window here we have is the one to provide the category according to which the elements are filtered, and the process has been slowed down at this point. This window is inevitable if we wand to dynamically tell the computer which of the category is what we want, between those of the selected elements. However, if you are doing a routine job, such as selecting those only of the one category – for example, room category – and summing up the area to see if it exceeds the maximum of fire proving sections, then we can do something about it. Otherwise, when the pop-up comes up, we still need time to un-check those categories we don’t need.

{% highlight fsharp%}
type FilterSelectedElementsByCategory() =
  interface IExternalCommand with
    member x.Execute(cdata, msg, elset) =
      let uidoc = cdata.Application.ActiveUIDocument
      let selected = uidoc.Selection.GetElementIds() |> Seq.map uidoc.Document.GetElement |> List.ofSeq
      match selected with
      | [] ->
        msg <- "Select Element(s)"
        Result.Cancelled
      | _ ->
        let idcats =
            selected |> List.map(fun sel -> sel.Category.Id)
        let intIds =
            idcats |> List.map(fun id -> id.IntegerValue) |> List.distinct
        let idPicked =
          let strCats = selected |> Seq.map string |> Seq.map(fun str -> Array.last (str.Split '.')) |> String.concat ", "
          let promp = 
            sprintf "Pick elements to filter by category: %s." strCats
          uidoc.Selection.PickElementsByRectangle(promp)
          |> Seq.filter(
            fun e -> List.contains e.Category.Id.IntegerValue intIds
          )
          |> Seq.map(fun e -> e.Id)
        uidoc.Selection.SetElementIds(ResizeArray<ElementId>(idPicked))
        Result.Succeeded
{% endhighlight%}

When talking about the efficiency of modeling, I found it useful when we more focus on clicking, for example, keys and mouse buttons, and less on moving cursor around. In this selection process example, as said above, a window pops up when it comes to getting user information, according to which categories we want to filter. Using pop-ups and check-boxes after we start the command is not the only way we can tell computer which category it is that we want. How about we give the information before we run the command? We can pre-select arbitrary objects with the exact categories we want, then retrieve this information in code from the Selection of ActiveUIDocument and then run pick object by rectangle.

Usually we would have zoomed to the area when beginning to start this specific selection process, maybe also have one element within the area, whose category is the one we want to filter. We can just select it as an example for running the command. The work-flow to reach our goal would be as simple as just only three steps: pre-selecting elements with one mouse click, hitting the shortcut of the command – depending on the keys you set as shortcut – and finally picking object with a rectangle. The rest will be don by the command for you: getting wanted categories – filtering picked elements by the categories – putting the picked object into the Selection bucket – and show them selected on the screen.

That’s it! As shortly as it can be!