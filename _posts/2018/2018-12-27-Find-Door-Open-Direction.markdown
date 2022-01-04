---
layout: post
title: "Find Door Open Direction"
date: 2018-12-27-004346 
categories: RevitExternalCommand
tags: [f#, revit api, door]
published: true
---
<script src="/assets/signup/signup.js"></script>
<signup-component></signup-component>

When using Revit, have you ever ask yourself: How could I know the opening direction of a door? This question emerged while I was working on a project in its execution phase, and it was a huge task to deliver a door list with the full information about all doors – including door opening directions.

While taking on this issue, I have come to three relevant questions: How could one know efficiently, that a door family instance has been flipped by using the flipping handle in the family instance and/or mirrored by using the mirror function? Is there any difference between using flipping handle and the mirror without copy function? How does the mirror function affect the family which has no flipping handles? These questions are probably especially interesting for those who have happened to be the former users of AutoCAD’s blocks, someone like me.

After a few simple tests, many try-and-errors, and some “looking-into”, I might can answer these questions. Let’s begin with questions number two and three, and the answer is that by using mirroring without copy one creates actually a new family instance with new element id whose flipping parameters document in which direction this element is flipped, no matter if this family has flipping handle or not. As to the question number one, it might help at the answer with a code example:

{% highlight fsharp %}
type FindDoorOpenDirection() as this =
  interface IExternalCommand with
    member x.Execute(cdata, msg, elset) =
      let uidoc = cdata.Application.ActiveUIDocument
      let dir = Left
      let selected =
        uidoc.Selection.GetElementIds() |> Seq.map uidoc.Document.GetElement
        |> Seq.filter(
          fun s ->
            match s with
            | :? FamilyInstance -> true
            | _ -> false
        ) 
        |> Seq.filter(
          fun s ->
            match s.Category.Id.IntegerValue with
            | x when x = int BuiltInCategory.OST_Doors -> true
            | _ -> false
        ) |> List.ofSeq
      match selected with
      | [] -> msg <- "Select door(s)"; Result.Cancelled
      | _ ->
        let t = new Transaction(uidoc.Document, string this)
        t.Start() |> ignore
        selected |> Seq.cast<FamilyInstance>
        |> Seq.map(
          fun dr ->
            let opendir = 
              match dr.HandFlipped, dr.FacingFlipped with
              | true, true | false, false -> dir
              | _, _ -> dir.ops
            let par = dr.get_Parameter(BuiltInParameter.DOOR_NUMBER)
            par.Set(par.AsString() + " | " + string opendir)
        ) |> List.ofSeq |> ignore
        t.Commit() |> ignore
        Result.Succeeded
{% endhighlight %}

In this example, we see the usual selection filtration (line ), for the type match, each of the selected elements is compared with its type against FamilyInstance, since the door elements are created family elements. However, this includes also the other building elements, such as windows. For specifying the door element filtration, we have to filter the selected with its category against the door category – to be exact, it’s the IntegerValue of the category id that we are comparing here.

Before we determine in which direction a door family instance opens, we have to define, of course, the default situation of the certain family type – since it’s the default situation of the door type to be newly set in the model scene – here we say that the default door family type has been drawn as a left side opened door. To know how the door instance has been mirrored or flipped from its default to its now status, all we need to retrieve are just two of its parameters – HandFlipped and FacingFlipped. (line ) By comparing them with boolean operants, if both of them are true or false at the same time, the door has the same status as the default, and vice versa.

Before I introduce the following handy type, let’s finish the main code at writing the retrieved door open status into its door number parameter additionally. Here it is set that every time when we re-run this code, the the information of door opening direction will be appended to the existing text information in the door number parameter. Surely this is something to be defined according to the project requirement.

{% highlight fsharp %}
type OpenDirection =
  | Left
  | Right
  member x.ops = 
    match x with
    | Left -> Right
    | Right -> Left
{% endhighlight %}

This one handy “discriminated union” type exists for helping me to treat each status of door opening direction as a union case. When we create a value of this type, it can either be “Left” or “Right (these are just tags or case identifier with no sub-type.) It is implemented in the line of our main code and assigned to the value “dir“, which is set as default to the case of Left. “dir” has a function member “ops“, which, when called, will toggle the status of “dir” to the opposite status. (line ) It is used in the line of the main code, while we check the combination of the two kinds of flipping status of a door instance, as explained above.