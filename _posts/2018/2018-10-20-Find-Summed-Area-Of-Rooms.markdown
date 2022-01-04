---
layout: post
title: "Find Summed Area Of Rooms | 房間面積總和"
date: 2018-10-20-081657 
categories: RevitExternalCommand
tags: [f#, revit api, sum, area, room]
published: true
---
<script src="/assets/signup/signup.js"></script>
<signup-component></signup-component>

下一個指令其實相當簡單的。上一次我們說過，該如何從簡單的曲線選取，指令的輸入，快速地取得曲線的長度總和。而我們這一次要做的其實很類似。假設我們想要快速地知道，好幾個空間的面積總和。這個可以用在當我們做建築內空間規劃時，想要快速的知道分區面積總合，是否超過最大的法規規定防火分區面積。在Revit API中，面積之於空間，就好比長度之於曲線，是一個它的相當簡單的預設參數。

The following command is actually quite simple. In the last entry we talked about efficiently pre-selecting curves, then running the external command and retrieve the sum of the line lengths. What we are going to do this time is similar. Say, we want to know instantly the sum of areas from different rooms. This is quite useful, when planning the fire compartments we want to be sure that it does not exceed the limit by the law. In Revit API, an area of a room is in the API’s structure similar to as a length to a curve. It is a pre-set parameter.

{% highlight fsharp %}
type FindSumAreaOfRooms() as this = 
  interface IExternalCommand with
    member x.Execute(cdata, msg, elset) =
      let uidoc = cdata.Application.ActiveUIDocument
      let selected =
        uidoc.Selection.GetElementIds() 
        |> Seq.map uidoc.Document.GetElement
        |> Seq.filter(
          fun e ->
            match e with
            | :? Room -> true
            | _ -> false
        ) |> Seq.cast<Room> |> List.ofSeq
      match selected with
      | [] -> 
        msg <- "Select Room(s)."
        Result.Cancelled
      | _ ->
        let txt =
          let unitareas =
            selected 
            |> Seq.map(
              fun rm -> 
                fsMath.toCurrentUnits uidoc.Document 2.0 rm.Area
            )
          let uSys, area = 
            unitareas |> Seq.head 
            |> fun(u, _) -> u, unitareas 
            |> Seq.sumBy(fun(_,a) -> a)
          match uSys with
          | fsMath.Metric ->
            area |> fsMath.toRoundUp 4.0 |> string |> fun s -> s + " m²"
          | fsMath.Imperial ->
            area |> fsMath.toRoundUp 4.0 |> string |> fun s -> s + " sq. ft"
        TaskDialog.Show(string this, txt) |> ignore
        Result.Succeeded
{% endhighlight %}

我們在這裡可以清楚的看到跟之前的不同處。基本的不同點是：這裏當我們過濾預先選取的物件，比較其是否為空間種類。而當我們要得到面積總和，我們尋找的是空間的面積參數。其餘的過程其實是很相似的。譬如，根據文件的單位在決定是否轉換單位為公尺或保留為英尺（API的預設單位為英制）。但也就是因為相似，我們可以把一樣的過程寫成一模距，然後在其他指令中重複使用。

We can see clearly the difference. The basic ones are: We filter the pre-selected elements by their categories to see if it is room, and when we want to have the sum of areas, we search for the area parameter of rooms. The rest of the process is actually similar. For example, according to the units of the document to decide if we convert the value to meters or keep it in feet (the default units of API is imperial.) The similarity is the reason that we can manage the same process into a module and re-use the module in the other commands.

{% highlight fsharp %}
module fsMath
type Units =
  | Metric
  | Imperial
let toCurrentUnits (doc:Autodesk.Revit.DB.Document)(pow:float)(ft:float) =
  let units = doc.GetUnits()
  let ut = Autodesk.Revit.DB.UnitType.UT_Length
  let fo = units.GetFormatOptions(ut)
  let du = fo.DisplayUnits
  match Array.contains "METERS" ((string du).Split '_') with
  | true -> Metric, ft * 0.3048**pow
  | false -> Imperial, ft
let toRoundUp (dig:float)(n:float) =
  System.Math.Round (n * 10.0 ** dig) / (10.0 ** dig)
{% endhighlight %}

程式的詳述還是用原文英文來解釋會比較清楚。
This module has one type and two functions. This type, called discriminated union, has two cases, metric or imperial. In the first function we have three arguments: Revit document, power and dimension in feet, and the function finds whether this document is using metric or imperial system and by given value of the power argument to return one of the union cases with the converted dimension value – depending on the value of power, if the power is 1.0, it’s linear, between meter and foot; if it is 2.0, then it’s of areas, between square meter and square foot, and so on and so forth. The second function rounds a number up, according to the digit value. These three have been used in our main code. You can also try to improve our last code by implementing them in the similar way.