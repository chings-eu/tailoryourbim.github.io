---
layout: post
title: "View Control for Design Option Elements | 圖形取代設計選項中的物件顏色"
date: 2018-10-27-160742 
categories: RevitExternalCommand
tags: [f#, revit api, view, design option, override graphic settings]
published: true
---
In this blog entry, let’s talk about how to handle the display of elements in views for example, giving different color to elements from the other similar elements. Here, I’ll demonstrate with an example: assigning distinctive colors to elements according to their different design options. 

Sometimes, we have to work with DesignOptions for considering different design possibilities to improve our planing, and after a while, in the same model file, we might have many design options. In Revit’s default setting, in one single View, we can only show one of the Design Options in their group. Or we can select “automatic”, which shows the content of the Design Option which is currently being edited. It has a blind spot. We can distinguish the elements between different Design Option Groups, if they are similar in every other field, such as of the same category or of the same type.
We can however simply write a command to solve this problem.

接下來，來討論該操控物件（Element）在視圖中如何顯示的問題。譬如，給予與其他類似物件不同的顏色。這裏，我以一個例子來做說明：依據不同的設計選項群組，在同一視圖中，賦予不同的顏色。

時常，我們需要利用設計選項（Design Option）使設計能被完善的考慮，而當我們設計了一段時間之後，在同一個模型檔案中，可能有好幾個不同的設計選項。在Revit的預設中，在同一個視圖裡，可以呈現選擇在選項群組的某一設計選項。或選擇自動，也就是所呈現的會是依據當下編輯的選項內容。但這有一盲點，我們無法在同一視圖中，區分介於不同設計選項群組中的物件，如果它們在於其他個方面都類似，譬如，都屬同種類（Category）且同類型（Type）。
我們可簡單地寫一個指令來解決這個問題。

{% highlight fsharp %}
type FitColorOfOptionElement() as this =
  interface IExternalCommand with
    member x.Execute(cdata, msg, elset) =
      let uidoc = cdata.Application.ActiveUIDocument
      let selected =
        let col = new FilteredElementCollector(uidoc.Document, uidoc.ActiveView.Id)
        col.WhereElementIsNotElementType()
        |> Seq.filter(
          fun e -> 
            try
              match e.DesignOption.Id.IntegerValue with
              | x when x = ElementId.InvalidElementId.IntegerValue -> false
              | _ -> true
            with
            | :? NullReferenceException -> false
        )
        |> Seq.map(
          fun e -> e.DesignOption, e
        )
        |> Seq.sortBy(fun(o, _) -> o.Id.IntegerValue)
      match selected |> List.ofSeq with
      | [] ->
        msg <- "No DesignOption in this document"
        Result.Cancelled
      | _ ->
        let options =
          selected |> Seq.map(fun(o, _) -> o) |> Seq.distinctBy(fun o -> o.Id.IntegerValue)
        let colors =
          options 
          |> Seq.indexed
          |> Seq.map(
            fun(i, o) -> 
              let ratio = float i / (float (Seq.length options))
              let color = new Color(byte (int(255.0 * ratio)), byte 128, byte 250)
              let ogs = new OverrideGraphicSettings()
              ogs.SetCutLineColor(color) |> ignore
              ogs.SetCutFillColor(color) |> ignore
              ogs.SetProjectionLineColor(color) |> ignore
              ogs.SetProjectionFillColor(color) |> ignore
              o, ogs
          )
        let t = new Transaction(uidoc.Document, string this)
        t.Start() |> ignore
        selected
        |> Seq.iter(
          fun(o, e) ->
            let _, ogs = 
              colors 
              |> Seq.find(fun(opt, _) -> opt.Id.IntegerValue = o.Id.IntegerValue)
            uidoc.ActiveView.SetElementOverrides(e.Id, ogs)
        )
        t.Commit() |> ignore
        Result.Succeeded
{% endhighlight %}

Note, when we try to filter those elements, belonging to design options, we might confront the ``System.NullReferenceException`` error, if the elements cannot belong to any design option – such as detail lines, and we can handle it with “try… with” to avoid program crash (line 16). If the elements can belong to a design option but not, the id of its DesignOption parameter can be compared with InvalidElementId to be sorted. The next point is how to assign an override setting to a certain element; now the OverrideGraphicSettings would be useful (line41). This usage is similar to the way in Revit interface. After initialization of the OverrideGraphicSettings, its methods contain the setting available in Revit as well: CutLineColor, CutLinePattern… etc. In our example, we want to change the color of elements, for which we need to set color (line 40). Finally, assign these overriding settings to each of the design option elements in the current active view (line 56), and according to their distinguish design options, they will appear with different red tones in the RGB value.

在此，要注意的是，當我們篩選在設計選項的物件時，如果此物件依照它的屬性不能歸類於任何一設計選項時（例如細部線），我們會碰上 ``System.NullReferenceExeception`` 的錯誤。這時，可以用 try… with 來處理（16行）。而如果是可以歸類於設計選項，但並沒有被歸類，則可用判斷它的設計選項是否有無效的識別碼 InvalidElementId 來篩選。接下來的重點是，如何賦予特定的物件，特定的圖形取代；這時，OverrideGraphicSettings 將派上用場（41行）。這與我們在Revit中使用圖形取代時，是一樣的方法。在初使化 OverrideGraphicSetting 後，在其底下有與在Revit中一樣的設定：CutLineColor, CutLinePattern…等等，在此範例，我們要的是不同的顏色，將需要顏色的設定（40行）。最後，將此設定在現下的視圖中賦予各個物件（56行），依據它們不同的設計選項，將呈現不同的 RGB 紅色層級以作示範。