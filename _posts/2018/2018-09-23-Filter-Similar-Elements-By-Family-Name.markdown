---
layout: post
title: "Filter Similar Elements By Family Name"
date: 2018-09-23-054834 
categories: RevitExternalCommand
tags: [f#, revit api]
published: true
---
A basic and common way to optimize the efficiency is by giving shortcuts to our most frequently used commands. By combining different shortcuts, i.e. commands, we create drawing processes, and by doing which repeatedly we save the time we wasted by moving mouse cursor and selecting between panels and buttons, repeatedly.

However, sometimes we don’t find the commands we need, or we wish for a certain drawing process but it does not exist as a command or the function is in the shortcut list, and now we might need improved optimizing methods to reach our goals. One of them is to create our own commands, and we can either combine them as supplements with existing commands and run them together by a series of shortcuts, or we can also directly write a single command with all the commands we need and run it with one shortcut.

A simple example would be to select the elements which have the same family all at once. A default way could be expanding the family list, find the certain family and expanding the tree list structure of all its types and then select the instances by right-click on each of the types, while also holding the control button to collect them all at once. Yes, it is possible to do it with the default tools, but there is no possible shortcut to assign to this function, because it is not a command, and it is time-consuming and error-prone since it has many key combinations and redundant that we have to apply each key combination to each type the family has.

How about to write one command, where we give an input about the family we want, then it filter through all the elements which belong to this family and select? And here it goes:

{% highlight fsharp %}
type FilterSimilarElementsByFamily() =
  interface IExternalCommand with
    member x.Execute(cdata, msg, elset) =
      let uiapp = cdata.Application
      let uidoc = uiapp.ActiveUIDocument
      let doc = uidoc.Document
      // Get selected element(s)
      let selected = uidoc.Selection.GetElementIds() |> Seq.cast |> Seq.map (fun (eId:ElementId) -> doc.GetElement eId)
      // Get family name(s) of the selected element(s)
      let bip = BuiltInParameter.ALL_MODEL_FAMILY_NAME
      let namAndcat = 
        selected
        |> Seq.map(
          fun(e:Element) ->
          let typ = e.Document.GetElement (e.GetTypeId())
          try
            typ.get_Parameter(bip).AsString(), e.Category.Id
          with
          | :? System.NullReferenceException ->
            "", e.Category.Id
          )
        |> Seq.filter (fun(s:string, id:ElementId) -> s <> "" && id <> ElementId.InvalidElementId)
      // If element(s) in the active view
      let isSameView = true
      // If element(s) in group
      let isInGroup = true
      // Run
      match Seq.length selected with
        | x when x = 0 -> 
          msg <- "Select Element(s) to have similar family(s) before running command"
          Result.Failed
        | _ ->
          // Use FilteredElementCollector
          let col =
            match isSameView with
            | true -> new FilteredElementCollector(doc, uidoc.ActiveView.Id)
            | false -> new FilteredElementCollector(doc)
            |> fun c -> c.WhereElementIsNotElementType()
            |> Seq.cast
            |> match isInGroup with
              | false -> Seq.filter (fun(e:Element) -> e.GroupId = ElementId.InvalidElementId)
              | true -> Seq.filter (fun _ -> true)
            |> Seq.filter (
              fun e ->
                let t = e.Document.GetElement(e.GetTypeId())
                Seq.exists (
                  fun (s, id) -> 
                    try
											e.Category.Id = id &&
											t.get_Parameter(bip).AsString() = s
                    with
                    | :? System.NullReferenceException ->
											false
                  ) namAndcat
              )
            |> Seq.map (fun e -> e.Id)
            uidoc.Selection.SetElementIds(ResizeArray<ElementId> col)
            Result.Succeeded
{% endhighlight %}

The process is simple. Just like we thought – we are looking for the similar elements of the same family. We pre-select the element before we run the command, and by pre-selecting elements we will have the information in the code about which families of those pre-selected element are those we are looking for, and then by filtering those elements in the whole project which are of the same families will be then selected after the implementation of the external command.

So it goes: we can retrieve the pre-selected elements, as introduced in the post “Selected Elements”, from ActiveUIDocument.Selection, where the pre-selected elements are stored. By running all the selected element ids through a series of functions, we finally mapped the selection of elements as an F# sequence of elements for the further deployment.

Before getting into the next stop, I’d like to add a note – I would usually add a type matching here right after we read the selection to prevent (or better said, to clarify and to warn) the program from getting into an error, since the user can select nothing before he/she start the command, and if so, the program will run into trouble, because it will have nothing to progress. It’s a simple step, but essential, if the command users are not familiar with warnings and errors, it could effect their working efficiency. The detail of type matching will be summed up in later session. Let’s assume the users are simply informed of the step to select element prior to running this command.

The key point of this command is the question – how do we eventually get the family names? The answer lies with the structure of the elements. Each element has a type, and a type belong to a family, and we retrieve the element type by sending GetTypeId() to the GetElement() method of the ActiveUIDocument.Document. Once we have a type of an element, we can know the family name of the type by getting the parameter ALL_MODEL_FAMILY_NAME. If we gathered the family names of the selected elements, then we know what families we are looking for. Next step is then simple: We just have to compare the family name of each of the element in the document by using a FilteredElementCollector.

We have two choices to use the collector: filter all of the elements in the document or only the ones in a certain view. This is very useful, since most of time we’d like to modify the element which we can comprehend, like within the active view, and as we know, the less elements the program has to process, the less resource that it needs, hence the shorter time and the more efficiency. Determining if only the elements in the view will be a chance to do so. The rest will be as easy as you can think of: get each element and see if its family name are listed in the gathered family names. However, if we think a bit beyond finishing our code, we’ll probably ask ourselves, what do we want do do with this command? What will we do the next if we have all elements with similar family names selected? We’d probably want to, for example, modify their parameter value all at once. However, if just one of the post-selected element is in either a group, which has more then one duplicate, or in a design option, the dream of all-you-can-eat will probably be broken. Thus, filters for not letting the elements within groups or design options are the prevention of this kind of failures, and here I demonstrate how to filter out the elements in the groups.

After we filtered all the elements that we want to select in an F# sequence, we can no put it back to the ActiveUIDocument.Selection by SetElementIds(). However, we have to convert the F# sequence to the C# ICollection by ResizeArray of our sequence. Once we finish the code and run it, we’ll have the new selected as the ones with similar family names.

By the way, the “if only elements in a certain view”, “if element is in a group” or “if element is in a design option” can be integrated into an interactive window, where the users can choose between those ifs or if-nots, and this will be discussed in other sections.

Stay tuned!