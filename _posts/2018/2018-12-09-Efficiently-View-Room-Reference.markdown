---
layout: post
title: "Efficiently View Room Reference | 有效率地快速顯現與隱藏房間的參考"
date: 2018-12-09-174858 
categories: RevitExternalCommand
tags: [f#, revit api, room, view]
published: true
---
Are you tired of repeatedly taping the tab key for selecting hidden room references? And you are doing so because of the applied view template, which you don’t want to switch to none and back, because it will take too long and you’ve got a deadline to make? Then all you’ll need is a efficient way to just quickly switch the visibility of the room reference on and off.

你厭倦了反覆地按同一 tab 鍵只為了選取房間量體的參考十字嗎？而你這麼做只是為了不想把已經設定好的視圖樣版在視圖控制取不取消之間轉換，而浪費寶貴的趕圖時間？若是，你需要的只是一個快速的房間參考顯示轉換機制。

The following code gives you an overview to see how to manipulate the active view by switch the category’s visibility. Surely, it depends on how the status of your currently view setup is. In this short code, we have two scenarios, with or without view template, as examples.

下面的編碼將給你一個概念，我們可以如何利用品類的顯示與否來控制當下視圖。當然，這是視你當下視圖的預設設定而定。在此簡短的編碼中，我們可先設定兩個預定情況，已設或未設視圖樣版，當作示範。

{% highlight fsharp %}
type ViewRoomReference() as this = 
  interface IExternalCommand with
    member x.Execute(cdata, msg, elset) =
      let uidoc = cdata.Application.ActiveUIDocument        
      let catRoom = 
        uidoc.Document.Settings.Categories
        |> Seq.cast<Category>
        |> Seq.filter(fun cat -> cat.Name = "Rooms")
        |> Seq.head
      let subcats =
        catRoom.SubCategories |> Seq.cast<Category>
        |> Seq.filter(
          fun subcat ->
            subcat.Name = "Reference" ||
            subcat.Name = "Interior Fill"
        )
      match uidoc.ActiveView with
      | :? View3D ->
        msg <- "3D View"
        Result.Cancelled
      | _ ->
        let catAll = [catRoom] @ (subcats |> List.ofSeq)
        let isStat = catAll |> List.map(fun cat -> uidoc.ActiveView.GetCategoryHidden(cat.Id))
        let toggle() =
          match isStat with
          | [false; false; false] ->
            catAll 
            |> List.iter(fun cat -> uidoc.ActiveView.SetCategoryHidden(cat.Id, true))
          | _ ->
            catAll 
            |> List.iter(fun cat -> uidoc.ActiveView.SetCategoryHidden(cat.Id, false))
        let t = new Transaction(uidoc.Document, string this)
        t.Start() |> ignore
        match uidoc.ActiveView.ViewTemplateId with
        | x when x = ElementId.InvalidElementId ->
          toggle()
        | _ ->
          match uidoc.ActiveView.IsTemporaryViewPropertiesModeEnabled() with
          | true ->
            uidoc.ActiveView.DisableTemporaryViewMode(
              TemporaryViewMode.TemporaryViewProperties
              )
          | false ->
            uidoc.ActiveView.EnableTemporaryViewPropertiesMode(
              uidoc.ActiveView.ViewTemplateId
              ) |> ignore
            toggle()                       
        t.Commit() |> ignore
        Result.Succeeded
{% endhighlight %}

Our goal is to turn on the visibility of the room category in active view, and its sub-categories “interior fill” and “reference” as well, so that we can easily recognize where the room reference points are and select the room body which we want. If the active view is not under the control of a view template, it’s simple to turn on and off the categories related to rooms. However, even if it is, we can still set the view under the “temporary view properties” mode, and toggle the visibility of the room categories.

我們的目標，是在當下的視圖中，快速地顯示或隱藏房間品類與其下《内部填滿》和《參考》的兩個次品類；而藉此，我們可容易地辨識房間的參考點在何處，進而選取我們要的房間量體，以達有效率地進行編輯。如果現下的視圖並未被試圖樣板控制，開啟或關閉與房間相關的品類是相當簡單的。若被控制，我們還是可將視圖設於暫時視圖性質模式之下，再進行房間品類的顯示編輯。

Of course, according to different workflows or office standards, the process can be customized according to the needs. The most important is, take your time to discover your own efficient way in Revit modeling.

當然，這是視各個不同的工作方式與公司案件標準而定。這個進程是可視不同需要，而量身定作的。重點是，我們應當利用一點時間，你可以發掘屬於自己最有效率的建模方式。