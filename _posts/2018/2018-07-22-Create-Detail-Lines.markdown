---
layout: post
title: "Create Detail Lines | "
date: 2018-07-22-054112 
categories: RevitExternalCommand
tags: [f#, revit api, create, detail line]
published: true
---
<script src="/assets/signup/signup.js"></script>
<signup-component></signup-component>

So now, we can already select element in Revit and connect them with our code. Next you might ask: how do we create things in Revit by running our code? For starters, let take look at a simple example, say, we want to choose arbitrary three points in Revit and create lines by connecting points on the two lines, which divide the two lines into, say, n parts:

This process to draw with the code is just like the process that we can imagine when we draw in Revit by hand: get three points, draw two lines between them, divide them into n parts with points and connect points from one line to another in the oder of the parameters on each line.

{% highlight fsharp %}
type CreateDetailLines() = 
  interface IExternalCommand with
    member x.Execute(cdata, msg, elset) =
      let uiapp = cdata.Application
      let uidoc = uiapp.ActiveUIDocument
      let doc = uidoc.Document
      let threepoints = 
        [0..2]
        |> List.map(fun _ -> uidoc.Selection.PickPoint(ObjectSnapTypes.Intersections, "Select intersection point"))
      let ln02 = Line.CreateBound(threepoints.[0], threepoints.[2])
      let ln21 = Line.CreateBound(threepoints.[2], threepoints.[1])
      let pars = [0.0..0.1..1.0]
      let eval (ln:Curve)(par:float) = ln.Evaluate(par * ln.Length, false)
      let pts02 = pars |> List.map (eval ln02)
      let pts21 = pars |> List.map (eval ln21)
      let t = new Transaction(doc, "Create detaillines") 
      t.Start() |> ignore
      let detaillines =
        List.map2 (
          fun pa pb -> Line.CreateBound(pa, pb) 
          ) pts02 pts21
        |> List.map (
          fun (ln:Line) -> doc.Create.NewDetailCurve(uidoc.ActiveView, ln)
          )
      t.Commit() |> ignore
      Result.Succeeded
{% endhighlight %}

The beginning of the type declaration is a routine job: UIApplication from the ExternalCommandData, ActiveUIDocument from the Application of the UIApplication and from the active UI document we get the database Document, i.e. where we have every drawn element stored.

The following of the code has a few things to note: To select arbitrary three points, we use PickPoint() method of Selection property of the ActiveUIDocument, where we can define, if we need to, which kind of ObjectSnapTypes we’d like to limit our point selection to. For the method calling, we can start with a number range from 0 til 2, use the pipeline to call the method three times and have the picked points as instances of XYZ in return. In this way, instead of using a for loop, passing the range list is like counting, then for each count you’ll have a point gained, and you have got a clear functional syntax. I found this is quite handy in reading codes.

Drawing lines is done by utilizing the CreateBound() method of the Line type, i.e. class, in the DB (database) namespace, by giving start and end points of the XYZ type. For the division of the lines, we’ll need to apply the parameters on each line, which are related to the dividing points. To get a point on curve – in this case it is a line – we use Valuate() method of the line class, which requires a parameter and a boolean inputs. A parameter on a line is a length input between the starting point to the inquired point along the curve. The second boolean input determines, when true, we only give a parameter between 0.0 and 1.0, considerung the line length is unitized. Here we give false to the if-unitized-input and by multipling the whole length with ratios between 0 and 1, we’ll have the length parameter. Again, by using pipeline syntax, we can start with a range of floating point numbers, as many as we want the lines to be split up, and then assign them to evaluate the points on each of the lines.

In Revit, for the modification of a document, a transaction must be declared. A transaction has a starting and a commiting points which mark the start and the end of it. In this way, when an error happens during the action, the whole process can be undone, to prevent the unwanted changes, i.e. the changes before the error point, to be left behind and hidden somewhere in the model. In the transaction mode, we use the map2 function of the List module of F# for mapping the points from one line to another, in the order of the start and end points of the two lines. To use the map2, mapping the CreateBound() method to each pair of the points, the length, i.e. numbers, of both point lists must be equal. After we have the list of the lines between the splitting points, we use List.map function to create each of them by mapping each line to the NewDetailCurve() function, method of the Create property of the Document, which also requires, firstly, the information of a View, where the lines should be drawn – in our case, I give the ActiveView from the current UIDocument (assuming we’re drawing in a 2 dimensional view and the view is active) – and secondly the curves, i.e. the lines. Since we are mapping each line one by one onto the view, we can use a lambda expression – at the left side of an arror, starting with the fun keyword to declare a lambda function, followed by the declaration of parameter, a Line type, value-bound with ln, and at the right side of the arrow is the return value – a detail curve created by each given ln value in the active view.

Finally, before we return a successfult result to finish the external command, we have to enter the commiting point to end the transaction and make the changes valid.

By the way, do not forget the namespace, adding references and assigning attributes declaration!

{% highlight fsharp %}
namespace TYB.Tutorial.Wordpress

open Autodesk.Revit.UI
open Autodesk.Revit.UI.Selection
open Autodesk.Revit.DB
open Autodesk.Revit.Attributes
{% endhighlight %}

By doing coding with Revit API you’re learning and discovery Revit everyday deeper.  

Stay tuned! 