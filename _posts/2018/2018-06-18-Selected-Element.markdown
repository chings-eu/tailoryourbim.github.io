---
layout: post
title: "Selected Element | 選取物件"
date: 2018-06-18-053026 
categories: RevitExternalCommand
tags: [f#, revit api]
published: true
---
We successfully connected our code with Revit by using “IExternalCommand” interface as an external command, by running which a simple message will pop-up. This connection shows the first step of tailoring your Revit. Now your Revit is standby and listening what you want it to do.

The goal of tailoryourbim is about to improve the efficiency of workiung with BIM software, and in this blog, it’s Revit. By writing codes on your own and for your own intention, you can customize a command according to your needs – needs like combining your daily modelling processes into one, customizing how a command should run considering your office standards and combining office drawing procedure (such as line style, annotation style or the title block for your submitting drawings… and so on and so forth). You will no longer be limited by the ability of those built-in commands and your efficiency will not be constrained within the speed of clicking those preset commands one after another and your time will be saved from those lost minutes while waiting for many commands to be fulfilled.

So many advantages of coding in Revit. However, you might wonder, how do we begin by getting the elements from Revit into our code, if we just want to “ask Revit” for the information of their, say, names and Revit can reply with a simple message to show the names in a pop-up, technically speaking – a TaskDialog window?

Basically, when we open a .rvt file by using Revit user interface (as we press the open file button), we read the element from the database of this document. To retrieve the elements, we will get them through the ExternalCommandData parameter of the Execute method when we implement the IExternalCommand interface in the code.

We can conveniently expand and modify our ASimpleMessage code from earlier:

{% highlight fsharp %}
type SelectedElement() = 
  interface IExternalCommand with
    member this.Execute(cdata:ExternalCommandData, msg, elset) =
      let uiapp = cdata.Application
      let uidoc = uiapp.ActiveUIDocument
      let selected = 
        uidoc.Selection.GetElementIds() |> Seq.cast
        |> Seq.map (fun(eid:ElementId) -> uidoc.Document.GetElement(eid))
      let msg =
        selected
        |> Seq.map (fun(e:Element) -> e.Name)
        |> String.concat "\n"
      TaskDialog.Show("Title", msg) |> ignore
      Result.Succeeded
{% endhighlight %}

In this part of our code we can see in line 8, the parameter cdata is a value of type ExternalCommanData. From this value we can retrieve our UIApplication, value-bound as uiapp, which “Represents an active session of the Autodesk Revit user interface, providing access to UI customization methods, events, and the active document.” , from which we get the ActiveUIDocument and bind its value as uidoc, which “Provides access to an object that represents the currently active project.”
If we can successfully connect to the current active project through the uidoc, then we can retrieve the selected elements’ ids with method GetElementIds() in its Selection property and map through a functional pipeline of method GetElement() one by one as demonstrated in line 12-13, and we have our selected elements in a sequence, value bound with “selected”.

Finally, for each of the selected elements we call the map function for sequence to retrieve the name property of them, concatenate them with new line separation operator “\n” as a single string and bind it with value msg. To show all names of the selected elements, we use TaskDialog.Show() as a simple message.

{% highlight fsharp %}
[<TransactionAttribute(TransactionMode.Manual)>]
{% endhighlight %}

In each modification, which results the changes in the model, a transaction process must be declared for Revit to proceed the code. If anything went wrong in the run time, an error message will pop-up and this process can be re-winded back to the original model stand before you call the command. In this way, no unwanted half-way progress will be done and hidden somewhere in the model which might result minor inaccuracy or even damage the model. When we set the transaction mode to manual, we must declare a new transaction in code with marks of starting and committing points, if a modification must happen. We will see to this point later on. 

{% highlight fsharp %}
open Autodesk.Revit.Attributes
open Autodesk.Revit.UI
open Autodesk.Revit.DB
{% endhighlight %}

As usual, we must declare which references of library we’ll be using (such as the listing above) and to maintain the clarity of the coding structure, in case its size goes exponential, we can start to construct namespaces. tailoryourbim does not only mean writing codes to improve your efficiency at work but coding itself is supposed to be efficient also. With a clear and well-organised namespace structural, we can easily reuse the codes that we have written, either by copying from one into another and modifying it for the newly usage, or calling methods, functions direct from within the code that is in development. 

{% highlight fsharp %}
namespace TYB.Tutorial.Wordpress
{% endhighlight %}

Stay tuned! 