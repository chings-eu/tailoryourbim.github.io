---
layout: post
title: "Find Sum Of Line Length"
date: 2018-10-18-080529 
categories: RevitExternalCommand
tags: [f#, revit api, sum, length, line]
published: true
---
<script src="/assets/signup/signup.js"></script>
<signup-component></signup-component>

In Revit 2017, I haven’t find the way to see directly the sum of the line lengths by selecting line elements within the default Revit interface. However, it is relative easy to write a few lines of code and use the TaskDialog interface in Revit API to return the summed length. And here it goes…

在使用 Revit 2017 中，我至今尚未找到一個方法，當我選取多重線段時，可以直接得到總長度的方法。但事實上，寫幾行 F# 程式經由 Revit API 取得長度，配合 TaskDialog 介面呈現總長數值，來解決這個問題是相當簡單的。以下是一範例：

{% highlight fsharp %}
type FindSumLengthOfLines() as this =
  interface IExternalCommand with
    member x.Execute(cdata, msg, elset) =
      let uidoc = cdata.Application.ActiveUIDocument
      let units = uidoc.Document.GetUnits()
      let uf = units.GetFormatOptions(UnitType.UT_Length).DisplayUnits
      let factor, tUnits =
        let ary = (string uf).Split '_'
        match Array.contains "METERS" ary with
          | true -> 0.3048, " m"
          | false -> 1.0, " ft"
      let selected =
          uidoc.Selection.GetElementIds()
          |> Seq.map uidoc.Document.GetElement
          |> Seq.filter(
            fun e ->
              match e with
              | :? CurveElement -> true
              | _ -> false
          ) |> Seq.cast |> List.ofSeq
      match selected with
      | [] ->
        msg
        let txt =
          selected
          |> Seq.map(
            fun ce ->
              ce.GeometryCurve.Length * factor
              |> fun x -> x * 10.0**4.0 |> Math.Round
              |> fun x -> x / 10.0**4.0
          ) |> Seq.sum |> string
          TaskDialog.Show("Sum of length:", txt + tUnits) |> ignore
          Result.Succeeded
{% endhighlight %}

As usual, we pre-select elements prior to running this command. To note is that we set a filter aiming for the element which are the curve element (line), by comparing the type of each of the pre-selected elements. From the filtered curve elements, we retrieve their length (line). Here comes a fun part. Since we are not sure with which units we should show the length, there is a function at the beginning of the code to determine which units this document has, where we compare if the keyword “METERS” exists in the DisplayUnitType. If so, we have metric system here and multiply the factor as 0.3048 to the summed lengths in order to show them correctly, and if not, it is then imperial units and the factor is 1.0 and we also append a string, either “m” or “ft” to the end of the dimension.

Simple and short, we make our work more efficient and save our valuable time for the other fun part of life…

**勿忘參考 / References**：  

Start up an F# solution for Revit plug-in / 在 Visual Studio 中用 F# 編程 Revit 外部指令  
Run external command in Revit / 加入 Revit 外部指令  
Start coding F# library for Revit / 開始編程 Revit 的 F# 指令集  