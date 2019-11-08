// directories
let dirTopic = "/Users/ching/Documents/Dropbox/41_PhD"
let dirPost = "/_posts" |> sprintf "%s%s" dirTopic
//let fnam = "/Users/ching/Documents/Dropbox/41_PhD/_posts/2019-09-28-0_1-Short-abstract.markdown"
// posts
let posts =
    dirPost
    |> fun dir -> System.IO.Directory.GetFiles(dir, "*.markdown", System.IO.SearchOption.TopDirectoryOnly)
let exposes =
    posts
    |> Seq.filter(
        fun str -> 
            str.Split '-' |> Array.item 3 |> fun t -> t.Split '_' |> Seq.head = "0"
    )
    |> Seq.sort |> List.ofSeq
exposes |> printfn "%A"
// functions
let pair(l:int list) = l.Tail |> List.zip l.[..(l.Length-2)]
let extractionOf(fnam:string) =
    let title = 
        fnam.Split '.' |> Seq.head
        |> fun str -> str.Split '-' 
        |> fun ary -> ary.[4..]
        |> String.concat " "
    let ary = fnam |> System.IO.File.ReadAllLines |> Array.filter(fun txt -> txt.Trim() <> "")
    let nbs =
        ary
        |> Seq.indexed
        |> Seq.filter(fun (i, ln) -> ln.Trim() = "---")
        |> Seq.map(fun(i, _) -> i) 
        |> List.ofSeq
        |> fun ls -> ls @ [(ary.Length - 1)]
    let extract =
        nbs
        |> pair 
        |> List.map(
            fun (a, b) ->
                ary.[a+1..b-1] |> String.concat "\n"
        )
    title, extract.[2] 

let extraction = exposes |> List.map extractionOf
