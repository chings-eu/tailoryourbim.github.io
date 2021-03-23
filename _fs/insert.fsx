#load "/Users/ching/Documents/DropBox/41_PhD/_fs/extract.fsx"

let insertionOf(title:string)(content:string) = 
    sprintf """
\section{%s}
\paragraph{}
%s
\newpage{}
    """ title content
    |> fun str -> str.Split '\n'
    |> Seq.filter(fun ln -> ln.Trim() <> "")
    |> String.concat "\n"

let add = """
\maketitle{}
\newpage{}
\tableofcontents{}
\newpage{}
"""

let toInsert =
    Extract.extraction
    |> List.map(
        fun (t, c) ->
            insertionOf t c
    )
    |> List.append (add.Split '\n' |> List.ofArray)
    
// list functions
let insertAt n (ls1:'a list)(ls0:'a list) =
    ls0.[..n-1] @ ls1 @ ls0.[n..]
let replaceBetween n0 n1 (ls1:'a list)(ls0:'a list) =
    ls0.[..(n0-1)] @ ls1 @ ls0.[n1 ..]

// insert to _main.tex
let dirExpose = "/Expose" |> sprintf "%s%s" Extract.dirTopic
let mainTex = "/_main.tex" |> sprintf "%s%s" dirExpose
let ary =
    mainTex
    |> System.IO.File.ReadAllLines
let nb0, nb1 = // line number of begin and end of document
    ary |> Seq.findIndex(fun x -> x = @"\begin{document}"),
    ary |> Seq.findIndex(fun x -> x = @"\end{document}")

let lst =
    ary
    |> List.ofArray
    |> replaceBetween (nb0+1) nb1 toInsert

lst |> List.iter(fun str -> printfn "%s" str)
let tmp = System.IO.Path.GetTempFileName()
let writeTo(fil:string)(ls:string list) =
    use wstream = new System.IO.StreamWriter(fil)
    ls |> List.iter wstream.WriteLine
    //wstream.Close()

lst |> writeTo mainTex