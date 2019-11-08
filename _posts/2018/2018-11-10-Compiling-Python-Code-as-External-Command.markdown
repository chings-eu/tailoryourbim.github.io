---
layout: post
title: "Compiling Python Code as External Command | 裝載 Python 編碼成 Revit 外部程式"
date: 2018-11-10-161733 
categories: 
tags: 
published: true
---
A couple years back, when I started to use Revit and Revit API with my limited Python knowledge from using RhinoCommon for my Master thesis, I’ve got to contact with two very useful external applications – pyRevit and RevitPythonShell. Many thanks to their creators – Ehsan Iran-Nejad and Daren Thomas, respectively, and also thanks to those bloggers (among them – The Building Coder from Jeremy Tammik) who give numerous insights, and uncountable replies on Stack Overflow to those questions of problems which I happened also to encounter, I have since learnt and written many Python codes in my free time and use them daily at work. Furthermore, because of these experiences I can now write the stand-alone external commands and applications for Revit with F#.

幾年前，當我剛開始使用 Revit，用我當時寫論文時，從使用 Python 與 RhinoCommon 的經驗來使用 Revit API 時，我接觸到兩個非常有用的外部程式：pyRevit 和 RevitPythonShell。感謝此兩程式的作者：Ehsan Iran-Nejad 和 Daren Thomas；也感謝那些給予深刻經驗的許多部落格作者（其中，Jeremy Tammik 的 The Building Coder），還有許許多多在 Stack Overflow 的問題解答，而也解答了我在當時有的相關疑惑。從那時起，我學習了許多，也在閒暇之餘寫了很多之後在工作上很有效率的工具。此外，也因為這些經驗，我才能用 F# 寫出許多外部指令與程式。

Recently, I have done a little research,  took these two programs from above as references and wrote a simple Python code loader in F#, which loads a Python script into Revit as an external command, combining with IronPython.

最近，作了一些搜尋，參考此二程式，也用 F# 寫出了一個簡單的 Python 編碼裝載器。此一，裝載了 Python 的編碼，結合 IronPython 以一外部指令進入 Revit 的軟件環境中。

{% highlight fsharp %}
open IronPython.Compiler
module loadPythonScript =
  let loader(cdata:ExternalCommandData)(pth:string) =
    let lst = [("Frames", true:>obj); ("FullFrames", true:>obj); ("LightweightScoped", true:>obj)]
    let dic = Seq.ofList lst |> dict

    // IronPython Engine
    let engine = IronPython.Hosting.Python.CreateEngine(options = dic)
    engine.Runtime.LoadAssembly(typeof<Autodesk.Revit.DB.Document>.Assembly)
    engine.Runtime.LoadAssembly(typeof<Autodesk.Revit.UI.Result>.Assembly)

    // Builtin Module
    let mdlBuiltin = IronPython.Hosting.Python.GetBuiltinModule(engine)
    mdlBuiltin.SetVariable("uiapp", cdata.Application)
    mdlBuiltin.SetVariable("__window__", 0)

    // Scope
    let scope = IronPython.Hosting.Python.CreateModule(engine, "__main__")
    scope.SetVariable("d", d)
    //scope.SetVariable("msg", msg)
    scope.SetVariable("res", Result.Succeeded)
    scope.SetVariable("__file__", 0)

    // Script
    let script = engine.CreateScriptSourceFromFile(path = pth, encoding = System.Text.Encoding.UTF8, kind = Microsoft.Scripting.SourceCodeKind.Statements)

    // Compile
    let optCompiler = engine.GetCompilerOptions(scope):?>PythonCompilerOptions
    optCompiler.ModuleName <- "__main__"
    optCompiler.Module <- IronPython.Runtime.ModuleOptions.Initialize

    // Command
    let command = script.Compile(optCompiler)

    scope, script, command

[<TransactionAttribute(TransactionMode.Manual)>]
type LoadPythonScript() =
  interface IExternalCommand with 
    member x.Execute(cdata, msg, elset) =
      let pth = @"C:\pythonRunByFSharp.py"

      let scope, script, command = loadPythonScript.loader cdata pth
      match command with
      | null -> Result.Cancelled
      | _ ->
        try
          script.Execute(scope)
          Result.Succeeded
        with
        | :? System.Exception as exn ->
          Result.Cancelled
{% endhighlight %}

Writing til this point, must say, this code is actually simple, and thus quite rough. Using it doesn’t feel as smoothly as the other mature programs like pyRevit or RevitPythonShell. This program just simply loads the Python code through IronPython into Revit. Once we have time, there are many points to be improved, such as importing other Python tools or auto-completion… etc.

寫到這裡，必須要說，這個程式碼還是相當粗糙。使用起來，還是沒有像 pyRevit 或 RevitPythonShell 成熟。這個編碼只是讓我們可簡單地，在 Revit 中，使用 Python 編碼。我們若有時間，在許多方面還是必須要加強（譬如說，引入其它的 Python 工具，或自動完成文字輸入（auto-complete）），使它更加完備。

It has two parts. First one is loading Python code with IronPython (line 5). The other one is our main external command. The reason to split it into two parts is it allows us to re-use the first part in other F# external command just by giving different path towards other Python code files. In the first part, we set up a IronPython engine (line 15), built-in module (line 20) and scope (line 25). Then, output from the first part would be the variables used in the next part – the built scope and C# script, read by IronPython engine, and the command, compiled by the engine. In the next step, be sure that within these three variables, the command is not “null” (line 55), and then use “try… with…” (line 56) from F# to instruct the program how it should response, when any error or exception once happened (line 59).

這個編程分於兩部分：第一部分是用 IronPython 裝載 Python 編碼（行5），而第二是主要的執行外部指令編程。分兩部分的原因，是讓這第一部分的模距，可在其他的外部指令中，只需再鍵入不同的 Python 編碼檔案路徑，被重複利用。在第一部分中，先建立 IronPython 引擎（行15），預設模組（行20）和領域（行25），然後，從此部分輸出的，是在第二部分會使用到的：建立的領域（scope）、IronPython 引擎從 Python 編碼讀取成的 C# 底稿（script）和編譯的 C# 指令（command）。再接下來的步驟中，先確定在此三物件中，編譯的指令不是空集合（行55），在藉由 F# 的 “try… with…” （行56）來確定，倘若在執行底稿時，有錯誤發生時，主要外部指令應該如何反應（行59）。

Below, it is a Python code example. Note, “uiapp” is a pre-set variable, which is connected between the external command through the built-in module in line 21, with the python code. Just save this code under C drive, named as pythonRunByFSharp.py – of course, name and location are freely to decide, however, it must correspond to the setting in line 50.

下面，是一 Python 編碼範例。其中值得注意的是，“uiapp” 是一預設程式變數。此一變數是經由主要執行外部指令編程的第21行，經由 IronPython 預設模組與 Python 編碼連結的。只需將此範例存為一 pythonRunByFSharp.py 於 C 硬碟底下（當然，命名與檔案位置可自由決定，但必須與符合第50行。）在 Revit 中，即可藉由執行外部指令，執行 Python 編碼。

{% highlight python %}
import Autodesk.Revit.DB as db
import Autodesk.Revit.UI as ui
uidoc = uiapp.ActiveUIDocument

idsel = uidoc.Selection.GetElementIds()
sel = [uidoc.Document.GetElement(id) for id in idsel]
txt = "\n".join([s.Name for s in sel])
ui.TaskDialog("FSharp runs Python code", txt)
{% endhighlight %}

**勿忘參考 / References**：  
Start up an F# solution for Revit plug-in / 在 Visual Studio 中用 F# 編程 Revit 外部指令  
Run external command in Revit / 加入 Revit 外部指令  
Start coding F# library for Revit / 開始編程 Revit 的 F# 指令集  