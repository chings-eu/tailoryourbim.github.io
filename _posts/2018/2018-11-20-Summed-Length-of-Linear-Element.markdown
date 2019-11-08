---
layout: post
title: "Summed Length of Linear Element | 線型元件的長度總和"
date: 2018-11-20-174226 
categories: 
tags: 
published: true
---
還記得我們上一回的文章中，我們探討了如何取得線段長總和，還有以相似的方法取得空間的面積總和？我們也在上述的第二篇中，加入了自動依照案件的長度單位，換算成符合的面積單位。

Remember in the last post, where we discussed about how we retrieve the summed length of curve elements? After that post, we had a similar one, where the summed value is the retrieved area of room instead of length? And we also had a improved function, with which the report of value is determined automatically according to the current project unit. 

在本篇中，我們更利用從 F# 中相當方便的功能，將上述的程式作改變與加強。我們將取得 “線型元件”的總長度，例如在此範例中，曲線元件、牆面與樑架構元件的長度總和。

Now, in this post, we are also going to do some improvement, by using some of the convenient F# functions. We are going to retrieve the summed length of linear elements, such as, in this example, we combined the length of curve elements, wall and structural framing elements. 

{% highlight fsharp %}
type FindSumLengthOfLinearElement() as this = 
  interface IExternalCommand with
    member x.Execute(cdata, msg, elset) =
      let uidoc = cdata.Application.ActiveUIDocument
      let selected =
        uidoc.Selection.GetElementIds() 
        |> Seq.map uidoc.Document.GetElement
        |> Seq.map(
          fun e ->
            match e with
            | :? CurveElement as ce -> Some ce.GeometryCurve
            | :? Wall as w -> Some (w.Location:?>LocationCurve).Curve
            | :? FamilyInstance as fi ->
              match fi.Category.Id.IntegerValue with
              | x when x = int BuiltInCategory.OST_StructuralFraming -> 
                Some (fi.Location:?>LocationCurve).Curve
              | _ -> None
            | _ -> None
        )
        |> Seq.filter(
          fun opt -> opt.IsSome
        ) |> List.ofSeq
      match selected with
      | [] -> 
        msg <- "Select line(s) / Wall(s)."
        Result.Cancelled
      | _ ->
        let unitlen =
          selected
          |> List.map(fun opt -> opt.Value.Length)
          |> List.sum
          |> fsMath.toCurrentUnits uidoc.Document 1.0
        let txt =
          let uSys, sum = unitlen
          match uSys with
          | fsMath.Metric ->
            sum |> fsMath.toRoundUp 4.0 |> string |> fun x -> x + " m"
          | fsMath.Imperial ->
            sum |> fsMath.toRoundUp 4.0 |> string |> fun x -> x + " ft"
        TaskDialog.Show(string this, string txt) |> ignore
        Result.Succeeded
{% endhighlight %}

如同上面的編碼所展現的，這其實是與取得曲線線長總和的程式非常相似。但不同之處在於，如何確認何種物件應當被選取（14行）。在此步驟，不同於之前在直接求線長程式中的過濾 （filter）功能，我們使用序列（Seq）的映射（map）功能。此功能將輸入的物件映射於它們的代表曲線作為輸出，而這取決於我們要選出的類型：第一與第二兩類，也就是曲線元件與牆體（16、17 行）。這一功能將映射出一包含其幾何曲線的 Some 物件 –  Some 屬於在 F# 語言中的選擇權 Option 性質物件。而其所包含的曲線，即為從曲線物件中取得的幾何曲線（GeometryCurve）或從牆體所得的地點曲線（LocationCurve）。或是第三類，結構框架（StructuralFraming）。這一類將經由下列方式被決定：第一，物件是否屬於家族物件（FamilyInstance） ，然後再由其品類決定它是否是結構框架。最後，在此映射的程式中，因為 F# 是一 “type safe“的程式語言，就算輸入的物件不符合上述的任一類別，我們還是必須要與以 Some 物件同類的輸出。而此時，這程式將以另一同 Option 的物件回應：None。在接下來的選取過程中，我們將可在選項物件上用 “Seq.filter” 直接過濾出那些含有 ”IsSome“ 即是我們要的：曲線，在此一曲線中，將含有線型物件長度的屬性。

As here demonstrated, it is quite similar to the code for the summed length of curve elements. However, it is different at the part, determining which kind of elements are supposed to be selected (line 14). In this step, we use, instead of the “filter“, we use the “map” function of Seq. This function maps the elements to their representative curves according to our chosen types: If the element is, of the first and second types, i.e. of Curve Element or Wall types (line 16, 17), the function returns a “Some“, an F# Option instance, with a value of Curve, which is retrieved from the element’s GeometryCurve or LocationCurve properties, respectively. Or it is of the third type, StructuralFraming, which is being determined by, firstly, if the element is a FamilyInstance, then, secondly,  if its category is StructuralFraming. Finally in this mapping function, since F# is a “type safe” language, if none of these three types is matched, we still have to have the same type as Some returned, the function will return another Option instance: None. In the following selection process, we can then filter out what we want – the curves, which have the length of the linear elements, by using “Seq.filter” on the option objects, if its “IsSome” property is true.

至此，若不論那被過濾和選取後的物件還仍不是曲線，此程式已接近完成。我們要等待曲線還仍被包裹在屬於 “Option” 類的 “Some” 的物件中。但擷取它們只需要呼喚出在 ”Some“ 中的 ”Value“ 屬性，而長度正是此 ”Value“（也就是 Curve） 的眾多屬性之一（35行）。

Until this point, this code is almost completed, except that the filtered and selected elements are not curves yet. They are still “packed” in the Some instance of the Option type. But to get them is just simply by calling the Value property of Some, and the Length is directly an property of this Value, i.e. the Curve (line 35). 